Imports System.Data.SqlClient
Imports LiveChartsCore
Imports LiveChartsCore.SkiaSharpView
Imports LiveChartsCore.SkiaSharpView.WinForms

Public Class FormDashboard

    Private ReadOnly ConnStr As String
    Private _currentView As String = ""
    Private _activeBtn As Button = Nothing
    Private _kpiCards As New Dictionary(Of String, Label)()

    ' Variables para paginación y búsqueda
    Private _dtCurrent As DataTable
    Private _currentPage As Integer = 1
    Private Const _pageSize As Integer = 5000
    Private _filterCol As String = ""
    Private _filterVal As String = ""

    ' Controles del Toolbar generados por código
    Private PanelToolbar As Panel
    Private WithEvents CboFilterColumn As ComboBox
    Private CboFilterValue As ComboBox
    Private WithEvents BtnSearch As FontAwesome.Sharp.IconButton
    Private WithEvents BtnExport As FontAwesome.Sharp.IconButton
    Private WithEvents BtnToggleChart As FontAwesome.Sharp.IconButton
    Private WithEvents BtnNextPage As FontAwesome.Sharp.IconButton
    Private WithEvents BtnPrevPage As FontAwesome.Sharp.IconButton
    Private LblPageInfo As Label

    ' Gráfico
    Private CartesianChart As CartesianChart

    ' ─────────────────────────────────────────────────────────────────────────
    '  Constructor
    ' ─────────────────────────────────────────────────────────────────────────
    Public Sub New()
        InitializeComponent()

        Dim b As New SqlConnectionStringBuilder()
        b.DataSource = "172.190.120.3"
        b.InitialCatalog = "ReportDB"
        b.UserID = "sa"
        b.Password = "Dell2014#"
        b.TrustServerCertificate = True
        ConnStr = b.ConnectionString
    End Sub

    ' ─────────────────────────────────────────────────────────────────────────
    '  Load
    ' ─────────────────────────────────────────────────────────────────────────
    Private Async Sub FormDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StyleDataGridView(DgvData)
        
        ' Asignar eventos manualmente para evitar errores de WithEvents en el Designer
        AddHandler DgvData.CellFormatting, AddressOf DgvData_CellFormatting
        AddHandler DgvData.CellDoubleClick, AddressOf DgvData_CellDoubleClick
        
        BuildToolbar()
        SetupHoverEffects()
        BuildKpiPanels()
        Await LoadViewsListAsync()
        Await RefreshKpisAsync()
    End Sub

    Private Sub SetupHoverEffects()
        AddHandler BtnRefresh.MouseEnter, Sub(s, ev) BtnRefresh.BackColor = Color.FromArgb(29, 78, 187)
        AddHandler BtnRefresh.MouseLeave, Sub(s, ev) BtnRefresh.BackColor = Color.FromArgb(37, 99, 235)
    End Sub

    ' ─────────────────────────────────────────────────────────────────────────
    '  Sidebar: load views from sys.views
    ' ─────────────────────────────────────────────────────────────────────────
    Private Async Function LoadViewsListAsync() As Task
        PanelViewsList.Controls.Clear()
        PbSidebar.Visible = True

        Try
            Dim views As List(Of String) = Await Task.Run(Function()
                Dim result As New List(Of String)
                Using conn As New SqlConnection(ConnStr)
                    conn.Open()
                    Dim sql As String = "SELECT name FROM sys.views ORDER BY name"
                    Using cmd As New SqlCommand(sql, conn)
                        cmd.CommandTimeout = 30
                        Using reader = cmd.ExecuteReader()
                            While reader.Read()
                                result.Add(reader("name").ToString())
                            End While
                        End Using
                    End Using
                End Using
                Return result
            End Function)

            Dim yPos As Integer = 5
            For Each viewName As String In views
                Dim btn As New Button()
                btn.Text = "   " & viewName
                btn.Tag = viewName
                btn.Font = New Font("Segoe UI", 9)
                btn.ForeColor = Color.FromArgb(190, 215, 255)
                btn.BackColor = Color.Transparent
                btn.FlatStyle = FlatStyle.Flat
                btn.FlatAppearance.BorderSize = 0
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 50, 95)
                btn.Size = New Size(206, 38)
                btn.Location = New Point(4, yPos)
                btn.TextAlign = ContentAlignment.MiddleLeft
                btn.Cursor = Cursors.Hand
                AddHandler btn.Click, AddressOf ViewButton_Click
                PanelViewsList.Controls.Add(btn)
                yPos += 40
            Next

            If views.Count = 0 Then
                AddSidebarMessage("  (Sin vistas en ReportDB)", Color.FromArgb(120, 150, 190))
            End If

        Catch ex As Exception
            AddSidebarMessage("  ❌ Error al conectar", Color.FromArgb(255, 100, 100))
        Finally
            PbSidebar.Visible = False
        End Try
    End Function

    Private Sub AddSidebarMessage(msg As String, clr As Color)
        Dim lbl As New Label()
        lbl.Text = msg
        lbl.Font = New Font("Segoe UI", 9)
        lbl.ForeColor = clr
        lbl.Size = New Size(206, 40)
        lbl.Location = New Point(4, 5)
        lbl.TextAlign = ContentAlignment.MiddleLeft
        PanelViewsList.Controls.Add(lbl)
    End Sub

    ' ─────────────────────────────────────────────────────────────────────────
    '  KPI Cards — build 4 cards programmatically
    ' ─────────────────────────────────────────────────────────────────────────
    Private Sub BuildKpiPanels()
        PanelKPIs.Controls.Clear()
        _kpiCards.Clear()

        Dim cardDefs() As (icon As String, title As String, subtitle As String, key As String, accent As Color) = {
            ("📋", "Total Pólizas", "último snapshot", "total_policies", Color.FromArgb(37, 99, 235)),
            ("📅", "Último Snapshot", "fecha de corte", "last_snapshot", Color.FromArgb(22, 163, 74)),
            ("✅", "Pólizas Activas", "status = 'A'", "active_policies", Color.FromArgb(14, 116, 144)),
            ("📦", "Total Snapshots", "períodos históricos", "total_snapshots", Color.FromArgb(126, 34, 206))
        }

        Dim totalGap As Integer = 15 * (cardDefs.Length - 1)
        Dim cardWidth As Integer = (PanelKPIs.ClientSize.Width - totalGap) \ cardDefs.Length
        Dim xPos As Integer = 0

        For Each def In cardDefs
            Dim card As New Panel()
            card.BackColor = Color.FromArgb(18, 30, 58)
            card.Size = New Size(cardWidth, PanelKPIs.ClientSize.Height - 10)
            card.Location = New Point(xPos, 5)

            ' Left accent bar
            Dim bar As New Panel()
            bar.BackColor = def.accent
            bar.Dock = DockStyle.Left
            bar.Width = 5
            card.Controls.Add(bar)

            ' Icon + title
            Dim lblTitle As New Label()
            lblTitle.Text = def.icon & "  " & def.title
            lblTitle.Font = New Font("Segoe UI", 8.5F, FontStyle.Bold)
            lblTitle.ForeColor = Color.FromArgb(140, 170, 220)
            lblTitle.Location = New Point(16, 10)
            lblTitle.AutoSize = True
            card.Controls.Add(lblTitle)

            ' Big value
            Dim lblValue As New Label()
            lblValue.Text = "..."
            lblValue.Font = New Font("Segoe UI", 22, FontStyle.Bold)
            lblValue.ForeColor = Color.White
            lblValue.Location = New Point(16, 34)
            lblValue.Size = New Size(cardWidth - 30, 46)
            lblValue.AutoEllipsis = True
            card.Controls.Add(lblValue)

            ' Subtitle
            Dim lblSub As New Label()
            lblSub.Text = def.subtitle
            lblSub.Font = New Font("Segoe UI", 7.5F)
            lblSub.ForeColor = Color.FromArgb(90, 120, 170)
            lblSub.Location = New Point(16, 82)
            lblSub.AutoSize = True
            card.Controls.Add(lblSub)

            _kpiCards(def.key) = lblValue
            PanelKPIs.Controls.Add(card)
            xPos += cardWidth + 15
        Next
    End Sub

    Private Async Function RefreshKpisAsync() As Task
        For Each lbl In _kpiCards.Values
            lbl.Text = "..."
        Next

        Try
            Dim kpi = Await Task.Run(Function()
                Dim r As New Dictionary(Of String, String)()
                Using conn As New SqlConnection(ConnStr)
                    conn.Open()

                    Using cmd As New SqlCommand(
                        "SELECT COUNT(*) FROM dbo.PolicyInForceSnapshot WHERE SnapshotMonthEnd=(SELECT MAX(SnapshotMonthEnd) FROM dbo.PolicyInForceSnapshot)", conn)
                        Dim v = cmd.ExecuteScalar()
                        r("total_policies") = If(v IsNot Nothing, CInt(v).ToString("N0"), "—")
                    End Using

                    Using cmd As New SqlCommand(
                        "SELECT MAX(SnapshotMonthEnd) FROM dbo.PolicyInForceSnapshot", conn)
                        Dim v = cmd.ExecuteScalar()
                        r("last_snapshot") = If(v IsNot Nothing AndAlso Not IsDBNull(v), CDate(v).ToString("MM/dd/yyyy"), "—")
                    End Using

                    Using cmd As New SqlCommand(
                        "SELECT COUNT(*) FROM dbo.PolicyInForceSnapshot WHERE SnapshotMonthEnd=(SELECT MAX(SnapshotMonthEnd) FROM dbo.PolicyInForceSnapshot) AND PolicyStatus='A'", conn)
                        Dim v = cmd.ExecuteScalar()
                        r("active_policies") = If(v IsNot Nothing, CInt(v).ToString("N0"), "—")
                    End Using

                    Using cmd As New SqlCommand(
                        "SELECT COUNT(DISTINCT SnapshotMonthEnd) FROM dbo.PolicyInForceSnapshot", conn)
                        Dim v = cmd.ExecuteScalar()
                        r("total_snapshots") = If(v IsNot Nothing, CInt(v).ToString("N0"), "—")
                    End Using
                End Using
                Return r
            End Function)

            For Each kvp In kpi
                If _kpiCards.ContainsKey(kvp.Key) Then
                    _kpiCards(kvp.Key).Text = kvp.Value
                End If
            Next

            LblLastUpdated.Text = "Actualizado: " & DateTime.Now.ToString("HH:mm:ss")

        Catch ex As Exception
            For Each lbl In _kpiCards.Values
                lbl.Text = "N/A"
            Next
            LblLastUpdated.Text = "❌ Error: " & ex.Message
        End Try
    End Function

    ' ─────────────────────────────────────────────────────────────────────────
    '  View selection → load DataGridView
    ' ─────────────────────────────────────────────────────────────────────────
    Private Async Sub ViewButton_Click(sender As Object, e As EventArgs)
        Dim btn As Button = CType(sender, Button)
        Dim viewName As String = btn.Tag.ToString()

        ' Highlight selected
        If _activeBtn IsNot Nothing Then
            _activeBtn.BackColor = Color.Transparent
            _activeBtn.ForeColor = Color.FromArgb(190, 215, 255)
        End If
        btn.BackColor = Color.FromArgb(37, 99, 235)
        btn.ForeColor = Color.White
        _activeBtn = btn

        _filterCol = ""
        _filterVal = ""
        If CboFilterValue IsNot Nothing Then CboFilterValue.Text = ""

        Await LoadViewDataAsync(viewName, 1)
    End Sub

    Private Async Function LoadViewDataAsync(viewName As String, Optional page As Integer = 1) As Task
        _currentView = viewName
        _currentPage = page
        
        If LblPageInfo IsNot Nothing Then LblPageInfo.Text = $"Pág. {_currentPage}"
        If BtnPrevPage IsNot Nothing Then BtnPrevPage.Enabled = (_currentPage > 1)

        LblViewName.Text = "📋  " & viewName
        DgvData.DataSource = Nothing
        LblRowCount.Text = ""
        PbLoading.Visible = True
        Cursor = Cursors.WaitCursor

        Dim offset As Integer = (_currentPage - 1) * _pageSize

        ' Store current filter locally for the Task to prevent cross-thread issues
        Dim currentFilterCol As String = _filterCol
        Dim currentFilterVal As String = _filterVal

        Try
            Dim dt As DataTable = Await Task.Run(Function()
                Using conn As New SqlConnection(ConnStr)
                    conn.Open()
                    
                    Dim whereClause As String = ""
                    If Not String.IsNullOrEmpty(currentFilterCol) AndAlso Not String.IsNullOrEmpty(currentFilterVal) Then
                        whereClause = $"WHERE [{currentFilterCol}] LIKE @filter"
                    End If

                    Dim sql As String = $"SELECT * FROM [{viewName}] {whereClause} ORDER BY 1 OFFSET {offset} ROWS FETCH NEXT {_pageSize} ROWS ONLY"
                    Using adapter As New SqlDataAdapter(sql, conn)
                        adapter.SelectCommand.CommandTimeout = 120
                        If Not String.IsNullOrEmpty(whereClause) Then
                            adapter.SelectCommand.Parameters.AddWithValue("@filter", $"%{currentFilterVal}%")
                        End If
                        Dim table As New DataTable()
                        adapter.Fill(table)
                        Return table
                    End Using
                End Using
            End Function)

            _dtCurrent = dt
            DgvData.SuspendLayout()
            DgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
            DgvData.DataSource = dt
            DgvData.ResumeLayout()
            
            ' Populate Dropdown if it's new view or filter was cleared
            If _currentPage = 1 AndAlso String.IsNullOrEmpty(_filterVal) AndAlso CboFilterColumn IsNot Nothing Then
                CboFilterColumn.Items.Clear()
                For Each col As DataColumn In dt.Columns
                    CboFilterColumn.Items.Add(col.ColumnName)
                Next
                If CboFilterColumn.Items.Count > 0 Then CboFilterColumn.SelectedIndex = 0
            End If

            If BtnNextPage IsNot Nothing Then BtnNextPage.Enabled = (dt.Rows.Count = _pageSize)

            LblRowCount.Text = $"Mostrando {dt.Rows.Count:N0} filas (Página {_currentPage})"
            If Not String.IsNullOrEmpty(_filterVal) Then
                LblRowCount.Text &= $" [Filtrado]"
            End If

        Catch ex As Exception
            LblViewName.Text = "❌  " & viewName
            LblRowCount.Text = "Error: " & ex.Message
        Finally
            PbLoading.Visible = False
            Cursor = Cursors.Default
        End Try

        If CartesianChart IsNot Nothing AndAlso CartesianChart.Visible Then
            Call LoadChartDataAsync()
        End If
    End Function

    ' ─────────────────────────────────────────────────────────────────────────
    '  Refresh button
    ' ─────────────────────────────────────────────────────────────────────────
    Private Async Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles BtnRefresh.Click
        Await RefreshKpisAsync()
        If Not String.IsNullOrEmpty(_currentView) Then
            Await LoadViewDataAsync(_currentView)
        End If
    End Sub

    ' ─────────────────────────────────────────────────────────────────────────
    '  Resize: rebuild KPI cards proportionally
    ' ─────────────────────────────────────────────────────────────────────────
    Private Sub FormDashboard_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        If PanelKPIs.Controls.Count = 0 Then Return
        Dim cards As List(Of Panel) = PanelKPIs.Controls.OfType(Of Panel)().ToList()
        If cards.Count = 0 Then Return
        Dim totalGap As Integer = 15 * (cards.Count - 1)
        Dim cardWidth As Integer = (PanelKPIs.ClientSize.Width - totalGap) \ cards.Count
        Dim xPos As Integer = 0
        For Each card As Panel In cards
            card.Width = cardWidth
            card.Location = New Point(xPos, 5)
            ' Resize value label inside
            For Each ctrl As Control In card.Controls
                If TypeOf ctrl Is Label AndAlso CType(ctrl, Label).Font.Size >= 20 Then
                    ctrl.Size = New Size(cardWidth - 30, 46)
                End If
            Next
            xPos += cardWidth + 15
        Next
    End Sub

    ' ─────────────────────────────────────────────────────────────────────────
    '  DataGridView Styling
    ' ─────────────────────────────────────────────────────────────────────────
    Private Sub StyleDataGridView(dgv As DataGridView)
        dgv.BackgroundColor = Color.FromArgb(22, 36, 68)
        dgv.BorderStyle = BorderStyle.None
        dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        dgv.GridColor = Color.FromArgb(35, 55, 95)

        dgv.DefaultCellStyle.BackColor = Color.FromArgb(26, 42, 78)
        dgv.DefaultCellStyle.ForeColor = Color.White
        dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(37, 99, 235)
        dgv.DefaultCellStyle.SelectionForeColor = Color.White
        dgv.DefaultCellStyle.Font = New Font("Segoe UI", 9)
        dgv.DefaultCellStyle.Padding = New Padding(5)

        dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(20, 33, 62)
        dgv.AlternatingRowsDefaultCellStyle.ForeColor = Color.White

        dgv.EnableHeadersVisualStyles = False
        dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(12, 20, 44)
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgv.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI Semibold", 10, FontStyle.Bold)
        dgv.ColumnHeadersDefaultCellStyle.Padding = New Padding(5)
        dgv.ColumnHeadersHeight = 38

        dgv.RowHeadersVisible = False
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgv.ReadOnly = True
        dgv.AllowUserToAddRows = False
        dgv.AllowUserToDeleteRows = False
    End Sub

    ' ─────────────────────────────────────────────────────────────────────────
    '  New Dashboard Features: Toolbar, Search, Export, Pagination, Formatting
    ' ─────────────────────────────────────────────────────────────────────────
    Private Sub BuildToolbar()
        PanelToolbar = New Panel()
        PanelToolbar.Dock = DockStyle.Top
        PanelToolbar.Height = 45
        PanelToolbar.BackColor = Color.FromArgb(20, 34, 65) ' Match content background
        PanelToolbar.Padding = New Padding(0, 5, 0, 5)

        CboFilterColumn = New ComboBox()
        CboFilterColumn.Location = New Point(5, 10)
        CboFilterColumn.Width = 160
        CboFilterColumn.DropDownStyle = ComboBoxStyle.DropDownList
        CboFilterColumn.BackColor = Color.FromArgb(26, 42, 78)
        CboFilterColumn.ForeColor = Color.White
        CboFilterColumn.FlatStyle = FlatStyle.Flat
        CboFilterColumn.Font = New Font("Segoe UI", 9)

        CboFilterValue = New ComboBox()
        CboFilterValue.Location = New Point(175, 10)
        CboFilterValue.Width = 150
        CboFilterValue.Font = New Font("Segoe UI", 9)
        CboFilterValue.BackColor = Color.FromArgb(26, 42, 78)
        CboFilterValue.ForeColor = Color.White
        CboFilterValue.FlatStyle = FlatStyle.Flat
        CboFilterValue.DropDownStyle = ComboBoxStyle.DropDown ' Permite escribir y seleccionar

        BtnSearch = New FontAwesome.Sharp.IconButton()
        BtnSearch.Text = ""
        BtnSearch.IconChar = FontAwesome.Sharp.IconChar.MagnifyingGlass
        BtnSearch.IconSize = 18
        BtnSearch.IconColor = Color.White
        BtnSearch.BackColor = Color.FromArgb(37, 99, 235)
        BtnSearch.FlatStyle = FlatStyle.Flat
        BtnSearch.FlatAppearance.BorderSize = 0
        BtnSearch.Width = 35
        BtnSearch.Height = 25
        BtnSearch.Location = New Point(335, 8)
        BtnSearch.Cursor = Cursors.Hand

        BtnToggleChart = New FontAwesome.Sharp.IconButton()
        BtnToggleChart.Text = "  Ver Gráfico"
        BtnToggleChart.IconChar = FontAwesome.Sharp.IconChar.ChartLine
        BtnToggleChart.IconSize = 18
        BtnToggleChart.IconColor = Color.White
        BtnToggleChart.ForeColor = Color.White
        BtnToggleChart.BackColor = Color.FromArgb(180, 80, 0)
        BtnToggleChart.FlatStyle = FlatStyle.Flat
        BtnToggleChart.FlatAppearance.BorderSize = 0
        BtnToggleChart.Width = 110
        BtnToggleChart.Height = 35
        BtnToggleChart.Location = New Point(390, 5)
        BtnToggleChart.Cursor = Cursors.Hand
        BtnToggleChart.TextImageRelation = TextImageRelation.ImageBeforeText

        BtnExport = New FontAwesome.Sharp.IconButton()
        BtnExport.Text = "  Exportar CSV"
        BtnExport.IconChar = FontAwesome.Sharp.IconChar.FileCsv
        BtnExport.IconSize = 20
        BtnExport.IconColor = Color.White
        BtnExport.ForeColor = Color.White
        BtnExport.BackColor = Color.FromArgb(16, 130, 58)
        BtnExport.FlatStyle = FlatStyle.Flat
        BtnExport.FlatAppearance.BorderSize = 0
        BtnExport.Width = 140
        BtnExport.Height = 35
        BtnExport.Location = New Point(510, 5)
        BtnExport.Cursor = Cursors.Hand
        BtnExport.TextImageRelation = TextImageRelation.ImageBeforeText

        BtnNextPage = New FontAwesome.Sharp.IconButton()
        BtnNextPage.IconChar = FontAwesome.Sharp.IconChar.ChevronRight
        BtnNextPage.IconSize = 16
        BtnNextPage.IconColor = Color.White
        BtnNextPage.BackColor = Color.FromArgb(37, 99, 235)
        BtnNextPage.FlatStyle = FlatStyle.Flat
        BtnNextPage.FlatAppearance.BorderSize = 0
        BtnNextPage.Size = New Size(40, 35)
        BtnNextPage.Cursor = Cursors.Hand
        BtnNextPage.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        BtnNextPage.Location = New Point(PanelContent.Width - 60, 5)

        LblPageInfo = New Label()
        LblPageInfo.Text = "Pág. 1"
        LblPageInfo.ForeColor = Color.White
        LblPageInfo.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        LblPageInfo.AutoSize = False
        LblPageInfo.Size = New Size(80, 35)
        LblPageInfo.TextAlign = ContentAlignment.MiddleCenter
        LblPageInfo.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        LblPageInfo.Location = New Point(PanelContent.Width - 145, 5)

        BtnPrevPage = New FontAwesome.Sharp.IconButton()
        BtnPrevPage.IconChar = FontAwesome.Sharp.IconChar.ChevronLeft
        BtnPrevPage.IconSize = 16
        BtnPrevPage.IconColor = Color.White
        BtnPrevPage.BackColor = Color.FromArgb(37, 99, 235)
        BtnPrevPage.FlatStyle = FlatStyle.Flat
        BtnPrevPage.FlatAppearance.BorderSize = 0
        BtnPrevPage.Size = New Size(40, 35)
        BtnPrevPage.Cursor = Cursors.Hand
        BtnPrevPage.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        BtnPrevPage.Location = New Point(PanelContent.Width - 190, 5)

        PanelToolbar.Controls.Add(CboFilterColumn)
        PanelToolbar.Controls.Add(CboFilterValue)
        PanelToolbar.Controls.Add(BtnSearch)
        PanelToolbar.Controls.Add(BtnToggleChart)
        PanelToolbar.Controls.Add(BtnExport)
        PanelToolbar.Controls.Add(BtnNextPage)
        PanelToolbar.Controls.Add(LblPageInfo)
        PanelToolbar.Controls.Add(BtnPrevPage)

        PanelContent.Controls.Add(PanelToolbar)
        PanelContent.Controls.SetChildIndex(PanelToolbar, PanelContent.Controls.GetChildIndex(PbLoading))

        ' Inicializar Gráfico interactivo
        CartesianChart = New CartesianChart()
        CartesianChart.Dock = DockStyle.Top
        CartesianChart.Height = 300
        CartesianChart.BackColor = Color.FromArgb(34, 45, 60)
        CartesianChart.Visible = False
        PanelContent.Controls.Add(CartesianChart)
        PanelContent.Controls.SetChildIndex(CartesianChart, PanelContent.Controls.GetChildIndex(PanelToolbar) + 1)
        
        Call LoadChartDataAsync()
    End Sub

    Private Async Function LoadChartDataAsync() As Task
        If String.IsNullOrEmpty(_currentView) Then Return

        Dim xLabels As New List(Of String)()
        Dim val6Month As New List(Of Double)()
        Dim val12Month As New List(Of Double)()

        Try
            Dim whereClause = ""
            If Not String.IsNullOrEmpty(_filterCol) AndAlso Not String.IsNullOrEmpty(_filterVal) Then
                whereClause = $" WHERE [{_filterCol}] LIKE @FilterParam "
            End If

            ' SQL Dinámico para calcular Cancel Rate de 6 y 12 meses agrupados por MonthStart
            Dim sql = $"SELECT 
                            FORMAT(MonthStart, 'MMM-yy') AS MonthLabel,
                            MonthStart,
                            ISNULL((SUM(CASE WHEN DATEDIFF(month, EffectiveDate, ExpirationDate) <= 6 AND CancelDate IS NOT NULL THEN 1.0 ELSE 0 END) / 
                             NULLIF(SUM(CASE WHEN DATEDIFF(month, EffectiveDate, ExpirationDate) <= 6 THEN 1.0 ELSE 0 END), 0)) * 100, 0) AS Rate6,
                            
                            ISNULL((SUM(CASE WHEN DATEDIFF(month, EffectiveDate, ExpirationDate) > 6 AND CancelDate IS NOT NULL THEN 1.0 ELSE 0 END) / 
                             NULLIF(SUM(CASE WHEN DATEDIFF(month, EffectiveDate, ExpirationDate) > 6 THEN 1.0 ELSE 0 END), 0)) * 100, 0) AS Rate12
                        FROM {_currentView}
                        {whereClause}
                        GROUP BY MonthStart
                        ORDER BY MonthStart"

            Using conn As New SqlConnection(ConnStr)
                Await conn.OpenAsync()
                Using cmd As New SqlCommand(sql, conn)
                    If whereClause <> "" Then
                        cmd.Parameters.AddWithValue("@FilterParam", $"%{_filterVal}%")
                    End If

                    Using reader = Await cmd.ExecuteReaderAsync()
                        While Await reader.ReadAsync()
                            If Not IsDBNull(reader("MonthLabel")) Then
                                xLabels.Add(reader("MonthLabel").ToString())
                                val6Month.Add(Convert.ToDouble(reader("Rate6")))
                                val12Month.Add(Convert.ToDouble(reader("Rate12")))
                            End If
                        End While
                    End Using
                End Using
            End Using

            CartesianChart.XAxes = New Axis() {
                New Axis With {
                    .Labels = xLabels.ToArray(),
                    .LabelsPaint = New LiveChartsCore.SkiaSharpView.Painting.SolidColorPaint(SkiaSharp.SKColors.LightGray),
                    .LabelsRotation = 45
                }
            }
            
            CartesianChart.YAxes = New Axis() {
                New Axis With {
                    .Labeler = Function(value) Math.Round(value, 1).ToString() & "%",
                    .LabelsPaint = New LiveChartsCore.SkiaSharpView.Painting.SolidColorPaint(SkiaSharp.SKColors.LightGray)
                }
            }

            CartesianChart.Series = New ISeries() {
                New LineSeries(Of Double) With {
                    .Values = val6Month.ToArray(),
                    .Name = "6 Month Cancellation Rate %",
                    .GeometrySize = 10,
                    .Stroke = New LiveChartsCore.SkiaSharpView.Painting.SolidColorPaint(SkiaSharp.SKColors.MediumOrchid, 3),
                    .Fill = Nothing
                },
                New LineSeries(Of Double) With {
                    .Values = val12Month.ToArray(),
                    .Name = "12 Month Cancellation Rate %",
                    .GeometrySize = 10,
                    .Stroke = New LiveChartsCore.SkiaSharpView.Painting.SolidColorPaint(SkiaSharp.SKColors.DarkOrange, 3),
                    .Fill = Nothing
                }
            }

        Catch ex As Exception
            ' Muestra el error de incompatibilidad en consola
            Console.WriteLine("Chart error: " & ex.Message)
        End Try
    End Function

    Private Async Sub BtnToggleChart_Click(sender As Object, e As EventArgs) Handles BtnToggleChart.Click
        CartesianChart.Visible = Not CartesianChart.Visible
        If CartesianChart.Visible Then
            BtnToggleChart.BackColor = Color.FromArgb(37, 99, 235)
            Await LoadChartDataAsync()
        Else
            BtnToggleChart.BackColor = Color.FromArgb(180, 80, 0)
        End If
    End Sub

    Private Async Sub BtnSearch_Click(sender As Object, e As EventArgs) Handles BtnSearch.Click
        If String.IsNullOrEmpty(_currentView) Then Return
        If CboFilterColumn.SelectedItem IsNot Nothing Then
            _filterCol = CboFilterColumn.SelectedItem.ToString()
        End If
        _filterVal = CboFilterValue.Text.Trim()
        Await LoadViewDataAsync(_currentView, 1)
    End Sub

    Private Async Sub CboFilterColumn_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CboFilterColumn.SelectedIndexChanged
        If CboFilterColumn.SelectedItem Is Nothing OrElse String.IsNullOrEmpty(_currentView) Then Return
        
        Dim colName As String = CboFilterColumn.SelectedItem.ToString()
        CboFilterValue.Items.Clear()
        CboFilterValue.Text = ""

        Try
            Dim values As List(Of String) = Await Task.Run(Function()
                Dim lst As New List(Of String)()
                Using conn As New SqlConnection(ConnStr)
                    conn.Open()
                    ' Busca hasta 500 valores únicos sin sobrecargar la memoria
                    Dim sql As String = $"SELECT DISTINCT TOP 500 [{colName}] FROM [{_currentView}] WHERE [{colName}] IS NOT NULL ORDER BY [{colName}]"
                    Using cmd As New SqlCommand(sql, conn)
                        cmd.CommandTimeout = 30
                        Using reader = cmd.ExecuteReader()
                            While reader.Read()
                                lst.Add(reader(0).ToString())
                            End While
                        End Using
                    End Using
                End Using
                Return lst
            End Function)

            For Each v In values
                CboFilterValue.Items.Add(v)
            Next
        Catch
            ' Ignorar silenciosamente si ocurre un error (ej. columna que no permite DISTINCT)
        End Try
    End Sub

    Private Async Sub ChangePage(direction As Integer)
        If String.IsNullOrEmpty(_currentView) Then Return
        _currentPage += direction
        If _currentPage < 1 Then _currentPage = 1
        Await LoadViewDataAsync(_currentView, _currentPage)
    End Sub

    Private Sub BtnNextPage_Click(sender As Object, e As EventArgs) Handles BtnNextPage.Click
        ChangePage(1)
    End Sub

    Private Sub BtnPrevPage_Click(sender As Object, e As EventArgs) Handles BtnPrevPage.Click
        ChangePage(-1)
    End Sub

    Private Sub BtnExport_Click(sender As Object, e As EventArgs) Handles BtnExport.Click
        If _dtCurrent Is Nothing OrElse _dtCurrent.Rows.Count = 0 Then
            MessageBox.Show("No hay datos para exportar.", "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Using sfd As New SaveFileDialog()
            sfd.Filter = "CSV File|*.csv"
            sfd.Title = "Guardar vista como CSV"
            sfd.FileName = $"{_currentView}_{DateTime.Now:yyyyMMdd_HHmm}.csv"
            If sfd.ShowDialog() = DialogResult.OK Then
                Try
                    Dim sb As New System.Text.StringBuilder()
                    Dim headers = _dtCurrent.Columns.Cast(Of DataColumn)().Select(Function(c) $"""{c.ColumnName}""")
                    sb.AppendLine(String.Join(",", headers))
                    
                    For Each viewRow As DataRowView In _dtCurrent.DefaultView
                        Dim fields = viewRow.Row.ItemArray.Select(Function(f) $"""{If(f IsNot Nothing, f.ToString().Replace("""", """"""), "")}""")
                        sb.AppendLine(String.Join(",", fields))
                    Next
                    
                    System.IO.File.WriteAllText(sfd.FileName, sb.ToString())
                    MessageBox.Show("Datos exportados correctamente.", "Exportar CSV", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    MessageBox.Show("Error al exportar: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Using
    End Sub

    ' Old memory filter removed

    Private Sub DgvData_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        If e.Value IsNot Nothing AndAlso Not IsDBNull(e.Value) Then
            Dim colName As String = DgvData.Columns(e.ColumnIndex).Name.ToLower()
            If TypeOf e.Value Is DateTime Then
                e.Value = CType(e.Value, DateTime).ToString("MM/dd/yyyy")
                e.FormattingApplied = True
            ElseIf colName.Contains("premium") OrElse colName.Contains("amount") OrElse colName.Contains("fee") OrElse colName.Contains("price") OrElse colName.Contains("cost") Then
                If IsNumeric(e.Value) Then
                    e.Value = CDec(e.Value).ToString("C")
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    e.FormattingApplied = True
                End If
            End If
        End If
    End Sub

    Private Sub DgvData_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = DgvData.Rows(e.RowIndex)
            Dim msg As New System.Text.StringBuilder()
            For i As Integer = 0 To row.Cells.Count - 1
                Dim colName As String = DgvData.Columns(i).HeaderText
                Dim val As String = If(row.Cells(i).Value IsNot Nothing AndAlso Not IsDBNull(row.Cells(i).Value), row.Cells(i).Value.ToString(), "N/A")
                msg.AppendLine($"{colName}: {val}")
            Next
            MessageBox.Show(msg.ToString(), "Detalles del Registro", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

End Class
