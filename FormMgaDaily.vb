Imports System.Drawing
Imports System.Windows.Forms
Imports FontAwesome.Sharp
Imports System.Data

Public Class FormMgaDaily
    Inherits Form

    Private DgvPivot As DataGridView

    Public Sub New()
        ' Configuración básica
        Me.Text = "MGA Daily KPIs"
        Me.BackColor = Color.FromArgb(20, 34, 65)
        Me.FormBorderStyle = FormBorderStyle.None
        Me.TopLevel = False
        Me.Dock = DockStyle.Fill

        BuildUI()
        LoadDummyData()
    End Sub

    Private Sub BuildUI()
        DgvPivot = New DataGridView()
        DgvPivot.Dock = DockStyle.Fill
        DgvPivot.BackgroundColor = Color.FromArgb(20, 34, 65)
        DgvPivot.BorderStyle = BorderStyle.None
        DgvPivot.AllowUserToAddRows = False
        DgvPivot.AllowUserToDeleteRows = False
        DgvPivot.ReadOnly = True
        DgvPivot.RowHeadersVisible = False
        DgvPivot.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        
        ' Estilos base
        DgvPivot.EnableHeadersVisualStyles = False
        Dim headerStyle As New DataGridViewCellStyle()
        headerStyle.BackColor = Color.FromArgb(0, 32, 96)
        headerStyle.ForeColor = Color.White
        headerStyle.Font = New Font("Segoe UI", 8.5!, FontStyle.Bold)
        headerStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DgvPivot.ColumnHeadersDefaultCellStyle = headerStyle
        DgvPivot.ColumnHeadersHeight = 45

        Dim cellStyle As New DataGridViewCellStyle()
        cellStyle.BackColor = Color.White
        cellStyle.ForeColor = Color.Black
        cellStyle.Font = New Font("Consolas", 8.5!) ' Ideal para alineación tabular numérica
        cellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        cellStyle.Format = "#,##0;(#,##0);-" ' <- Magia para los formatos (negativos con parentesis, 0 con guiones)
        DgvPivot.DefaultCellStyle = cellStyle

        AddHandler DgvPivot.CellFormatting, AddressOf DgvPivot_CellFormatting

        Me.Controls.Add(DgvPivot)
    End Sub
    
    Private Sub LoadDummyData()
        Dim dt As New DataTable()
        ' Columnas
        dt.Columns.Add("PolicyTerm", GetType(String))
        dt.Columns.Add("TransactionType", GetType(String))
        
        ' Daily Columns
        Dim days = {"2/17/2026", "2/18/2026", "2/19/2026", "2/20/2026", "2/21/2026", "2/22/2026", "2/23/2026"}
        For Each d In days
            dt.Columns.Add(d, GetType(Double))
        Next

        ' Monthly Columns
        Dim months = {"Feb-25", "Mar-25", "Apr-25", "May-25", "Jun-25", "Jul-25", "Aug-25", "Sep-25", "Oct-25", "Nov-25", "Dec-25", "Jan-26", "Feb-26"}
        For Each m In months
            dt.Columns.Add(m, GetType(Double))
        Next

        ' Insertando la fila 6 New Exactamente como el screenshot
        dt.Rows.Add("6", "New", 8312, 13068, 18360, 4138, 38244, 2996, 9191, 726174, 1785431, 1845446, 1503631, 1215027, 888551, 883838, 805537, 643050, 374578, 334437, 279486, 190168)
        dt.Rows.Add("6", "Rnw", 25328, 22704, 21810, 7806, 17864, 25164, 19001, 1251308, 1204418, 2023126, 2128564, 1339825, 1009495, 824293, 426134, 640962, 694902, 590789, 405380, 382109)
        dt.Rows.Add("6", "Rei", 0, 4082, 2094, 0, 1166, 0, 702, 238822, 185481, 237385, 271942, 283821, 255118, 223405, 151753, 111009, 104281, 92100, 48235, 28238)
        dt.Rows.Add("6", "Can", -374, -3450, -33468, -5615, -4574, -36756, -14498, -792887, -815744, -1239145, -1330215, -894341, -789838, -674981, -454870, -428023, -357057, -307372, -200450, -190286)
        dt.Rows.Add("6", "Sub-Total", 33266, 36404, 8796, 6329, 52700, -8596, 14396, 1421217, 2359586, 2867792, 2573922, 1924332, 1473324, 1256655, 928554, 966998, 816705, 709954, 530642, 410249)

        ' Espaciador
        dt.Rows.Add("", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
        
        ' Filas 12
        dt.Rows.Add("12", "New", 260072, 158570, 327432, 185132, 284322, 97014, 193658, 2153627, 321170, 348171, 4913680, 4060778, 7301486, 8306317, 7401588, 7115013, 5073729, 5298491, 4835157, 3168064)
        dt.Rows.Add("12", "Rnw", 437604, 394340, 453352, 256003, 573608, 527058, 207016, 10123409, 10311588, 9562426, 6148386, 2671743, 1216783, 1159224, 3164528, 3615615, 4271889, 6169818, 7789900, 7081248)
        dt.Rows.Add("12", "Rei", 34412, 26290, 17314, 16352, 46624, 0, 125813, 1305342, 1318264, 1098735, 961778, 820348, 927915, 1039380, 1455684, 1286747, 1195994, 1621998, 1588290, 1123895)
        dt.Rows.Add("12", "Can", -120168, -121774, -1324786, -150615, -316316, -856234, -486464, -4826767, -5641043, -5712147, -4614654, -3073614, -3056258, -3673480, -4512323, -4598360, -4722203, -5728226, -5531202, -6444002)
        dt.Rows.Add("12", "Sub-Total", 611920, 457426, -516688, 306872, 588236, -232162, 40023, 8755611, 6309879, 5297185, 7409171, 4479255, 6477906, 6833441, 7509459, 7418995, 5819409, 7382081, 8762185, 4929305)
        
        ' Espaciador
        dt.Rows.Add("", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
        
        ' Total General
        dt.Rows.Add("Total", "General", 645186, 493830, -507892, 313201, 840938, -240758, 54419, 10176828, 8689545, 8164977, 9983093, 6403587, 7951230, 8100096, 8438013, 8385993, 6636114, 8072035, 9292827, 5339554)

        DgvPivot.DataSource = dt
        DgvPivot.Columns("PolicyTerm").HeaderText = "Policy Term"
        DgvPivot.Columns("TransactionType").HeaderText = "Transaction Type"
    End Sub

    Private Sub DgvPivot_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        If e.RowIndex >= 0 Then
            Dim dgv = CType(sender, DataGridView)
            Dim isSubtotal = dgv.Rows(e.RowIndex).Cells("TransactionType").Value?.ToString().Contains("Total")
            Dim isTotal = dgv.Rows(e.RowIndex).Cells("PolicyTerm").Value?.ToString().Contains("Total")
            Dim isEmptySpacer = String.IsNullOrWhiteSpace(dgv.Rows(e.RowIndex).Cells("TransactionType").Value?.ToString())
            
            ' Centrar texto en las primeras columnas
            If e.ColumnIndex = 0 Or e.ColumnIndex = 1 Then
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End If
            
            If isSubtotal Or isTotal Then
                e.CellStyle.Font = New Font(dgv.DefaultCellStyle.Font, FontStyle.Bold)
                e.CellStyle.BackColor = Color.FromArgb(240, 245, 255)
            End If
            
            If isEmptySpacer Then
                e.CellStyle.BackColor = Color.FromArgb(20, 34, 65) ' Mimetizar con el fondo del form
                e.CellStyle.ForeColor = Color.FromArgb(20, 34, 65)
            End If
            
            ' Formatear espacios vacíos como guiones para que coincida con "-" en "Rei"
            If Not isEmptySpacer AndAlso e.ColumnIndex >= 2 AndAlso (e.Value Is Nothing OrElse e.Value.ToString() = "0" OrElse e.Value.ToString() = "0.00") Then
                If Not dgv.Rows(e.RowIndex).Cells("TransactionType").Value.ToString().Contains("Sub") Then
                    e.Value = "-"
                    e.FormattingApplied = True
                End If
            End If
        End If
    End Sub
End Class
