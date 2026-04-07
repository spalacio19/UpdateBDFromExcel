Imports System.Data.SqlClient

Public Class FormDashboard

    Private ReadOnly ConnStr As String
    Private _currentView As String = ""
    Private _activeBtn As Button = Nothing
    Private _kpiCards As New Dictionary(Of String, Label)()

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
            LblLastUpdated.Text = "❌ Error al cargar KPIs"
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

        Await LoadViewDataAsync(viewName)
    End Sub

    Private Async Function LoadViewDataAsync(viewName As String) As Task
        _currentView = viewName
        LblViewName.Text = "📋  " & viewName
        DgvData.DataSource = Nothing
        LblRowCount.Text = ""
        PbLoading.Visible = True
        Cursor = Cursors.WaitCursor

        Try
            Dim dt As DataTable = Await Task.Run(Function()
                Using conn As New SqlConnection(ConnStr)
                    conn.Open()
                    Dim sql As String = $"SELECT TOP 5000 * FROM [{viewName}]"
                    Using adapter As New SqlDataAdapter(sql, conn)
                        adapter.SelectCommand.CommandTimeout = 120
                        Dim table As New DataTable()
                        adapter.Fill(table)
                        Return table
                    End Using
                End Using
            End Function)

            DgvData.SuspendLayout()
            DgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
            DgvData.DataSource = dt
            DgvData.ResumeLayout()
            LblRowCount.Text = $"Mostrando {dt.Rows.Count:N0} filas  (máx. 5,000)"

        Catch ex As Exception
            LblViewName.Text = "❌  " & viewName
            LblRowCount.Text = "Error: " & ex.Message
        Finally
            PbLoading.Visible = False
            Cursor = Cursors.Default
        End Try
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

End Class
