Imports System.IO
Imports System.Data.OleDb
Imports System.Data.SqlClient

Public Class Form2

    Private pbLoading As ProgressBar

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Create and inject Marquee ProgressBar for Loading State
        pbLoading = New ProgressBar()
        pbLoading.Style = ProgressBarStyle.Marquee
        pbLoading.MarqueeAnimationSpeed = 30
        pbLoading.Size = New Size(200, 15)
        pbLoading.Location = New Point(PanelFooter.Width \ 2 - 100, 10)
        pbLoading.Anchor = AnchorStyles.Bottom Or AnchorStyles.Top
        pbLoading.Visible = False
        PanelFooter.Controls.Add(pbLoading)

        ' Hover effects for Upload button (blue)
        AddHandler btnUpload.MouseEnter, Sub(s, ev)
                                             btnUpload.BackColor = Color.FromArgb(29, 78, 187)
                                         End Sub
        AddHandler btnUpload.MouseLeave, Sub(s, ev)
                                             btnUpload.BackColor = Color.FromArgb(37, 99, 235)
                                         End Sub

        ' Hover effects for Export DB button (green)
        AddHandler btnExportDB.MouseEnter, Sub(s, ev)
                                                 btnExportDB.BackColor = Color.FromArgb(16, 130, 58)
                                             End Sub
        AddHandler btnExportDB.MouseLeave, Sub(s, ev)
                                                 btnExportDB.BackColor = Color.FromArgb(22, 163, 74)
                                             End Sub

        ' Apply modern styling to DataGridView
        StyleDataGridView(DtGV1)
    End Sub

    Private Sub StyleDataGridView(dgv As DataGridView)
        dgv.BackgroundColor = Color.FromArgb(26, 40, 74)
        dgv.BorderStyle = BorderStyle.None
        dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        dgv.GridColor = Color.FromArgb(40, 60, 100)
        
        dgv.DefaultCellStyle.BackColor = Color.FromArgb(30, 45, 80)
        dgv.DefaultCellStyle.ForeColor = Color.White
        dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(37, 99, 235)
        dgv.DefaultCellStyle.SelectionForeColor = Color.White
        dgv.DefaultCellStyle.Font = New Font("Segoe UI", 9)
        dgv.DefaultCellStyle.Padding = New Padding(5)
        
        dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(26, 40, 74)
        dgv.AlternatingRowsDefaultCellStyle.ForeColor = Color.White
        
        dgv.EnableHeadersVisualStyles = False
        dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 30, 55)
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgv.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI Semibold", 10, FontStyle.Bold)
        dgv.ColumnHeadersDefaultCellStyle.Padding = New Padding(5)
        dgv.ColumnHeadersHeight = 35
        
        dgv.RowHeadersVisible = False
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
    End Sub

    Private Sub SetDoubleBuffered(control As Control)
        Try
            Dim dgvType As Type = GetType(Control)
            Dim pi As System.Reflection.PropertyInfo = dgvType.GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.NonPublic)
            pi.SetValue(control, True, Nothing)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub UpdateStatus(message As String)
        LblStatus.Text = message
        Application.DoEvents()
    End Sub

    Private Sub UpdateRowCount()
        If DtGV1.DataSource IsNot Nothing Then
            Dim dt As DataTable = CType(DtGV1.DataSource, DataTable)
            LblRowCount.Text = $"Rows: {dt.Rows.Count:N0}"
        Else
            LblRowCount.Text = "Rows: 0"
        End If
    End Sub

    ' ─────────────────────────────────────────────
    '  BUTTON 1: Upload Excel (Async)
    ' ─────────────────────────────────────────────
    Private Async Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click
        Using ofd As New OpenFileDialog()
            ofd.Filter = "Excel files|*.xlsx;*.xls;*.xlsm|All files|*.*"
            If ofd.ShowDialog() <> DialogResult.OK Then Return

            Dim file = ofd.FileName
            Dim ext = Path.GetExtension(file).ToLower()
            Dim connString As String

            If ext = ".xls" Then
                connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & file & ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1';"
            Else
                connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & file & ";Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1';"
            End If

            LblFilePath.Text = Path.GetFileName(file)
            Cursor = Cursors.WaitCursor
            pbLoading.Visible = True
            btnUpload.Enabled = False
            UpdateStatus("Loading Excel file...")

            DtGV1.SuspendLayout()

            Try
                Dim dt As DataTable = CType(Await Task.Run(Function() As DataTable
                    Using conn As New OleDbConnection(connString)
                        conn.Open()

                        Dim schemaTable As DataTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
                        Dim sheetName As String = Nothing

                        If schemaTable IsNot Nothing Then
                            For Each row As DataRow In schemaTable.Rows
                                Dim tableName As String = row("TABLE_NAME").ToString()
                                If tableName.EndsWith("$") OrElse tableName.EndsWith("$'") Then
                                    sheetName = tableName
                                    Exit For
                                End If
                            Next
                        End If

                        If String.IsNullOrEmpty(sheetName) Then
                            Return Nothing
                        End If

                        Dim selectCmd As String = "SELECT * FROM [" & sheetName & "]"

                        Using adapter As New OleDbDataAdapter(selectCmd, conn)
                            Dim table As New DataTable()
                            adapter.Fill(table)
                            Return table
                        End Using
                    End Using
                End Function), DataTable)

                If dt Is Nothing Then
                    MessageBox.Show("No valid worksheets found.")
                    UpdateStatus("No worksheets found")
                    Return
                End If

                ' Allow Columns to Fill Screen based on StyleDataGridView
                DtGV1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                DtGV1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing

                DtGV1.DataSource = dt
                UpdateStatus($"✅ Excel loaded: {dt.Rows.Count:N0} rows from '{Path.GetFileName(file)}'")

            Catch ex As Exception
                MessageBox.Show("Failed to read Excel file: " & ex.Message)
                UpdateStatus("❌ Error loading Excel")
            Finally
                SetDoubleBuffered(DtGV1)
                DtGV1.ResumeLayout()
                UpdateRowCount()
                pbLoading.Visible = False
                btnUpload.Enabled = True
                Cursor = Cursors.Default
            End Try
        End Using
    End Sub

    ' ─────────────────────────────────────────────
    '  BUTTON 2: Export DB (TempData_Debug Async)
    ' ─────────────────────────────────────────────
    Private Async Sub btnExportDB_Click(sender As Object, e As EventArgs) Handles btnExportDB.Click
        If DtGV1.DataSource Is Nothing Then
            MessageBox.Show("No data to insert. Please load data first.")
            Return
        End If

        Dim dt As DataTable = CType(DtGV1.DataSource, DataTable)
        If dt.Rows.Count = 0 Then
            MessageBox.Show("No rows to insert.")
            Return
        End If

        pbLoading.Visible = True
        btnExportDB.Enabled = False
        Cursor = Cursors.WaitCursor
        UpdateStatus("💾 Preparing bulk insert...")

        Try
            Dim totalInserted As Integer = CType(Await Task.Run(Function() As Integer
                Dim builder As New SqlConnectionStringBuilder()
                builder.DataSource = "172.190.120.3"
                builder.InitialCatalog = "Univista"
                builder.UserID = "sa"
                builder.Password = "Dell2014#"
                builder.TrustServerCertificate = True

                Dim bulkDt As New DataTable()
                bulkDt.Columns.Add("Location", GetType(String))
                bulkDt.Columns.Add("Customer First Name", GetType(String))
                bulkDt.Columns.Add("Customer Last Name", GetType(String))
                bulkDt.Columns.Add("Customer Phone Number", GetType(String))
                bulkDt.Columns.Add("Customer Email", GetType(String))

                For Each row As DataRow In dt.Rows
                    Dim newRow As DataRow = bulkDt.NewRow()

                    newRow("Location") = If(row.Table.Columns.Contains("Location"), row("Location"), DBNull.Value)
                    newRow("Customer First Name") = If(row.Table.Columns.Contains("Customer First Name"), row("Customer First Name"), DBNull.Value)
                    newRow("Customer Last Name") = If(row.Table.Columns.Contains("Customer Last Name"), row("Customer Last Name"), DBNull.Value)
                    newRow("Customer Phone Number") = If(row.Table.Columns.Contains("Customer Phone Number"), row("Customer Phone Number"), DBNull.Value)
                    newRow("Customer Email") = If(row.Table.Columns.Contains("Customer Email"), row("Customer Email"), DBNull.Value)

                    bulkDt.Rows.Add(newRow)
                Next

                If bulkDt.Rows.Count = 0 Then Return 0

                Using conn As New SqlConnection(builder.ConnectionString)
                    conn.Open()

                    Dim createTempTableCmd As String = "
                        IF OBJECT_ID('TempData_Debug', 'U') IS NOT NULL DROP TABLE TempData_Debug;
                        CREATE TABLE TempData_Debug (
                        [Location] NVARCHAR(100),
                        [Customer First Name] NVARCHAR(100),
                        [Customer Last Name] NVARCHAR(100),
                        [Customer Phone Number] NVARCHAR(50),
                        [Customer Email] NVARCHAR(255)
                    );"

                    Using cmd As New SqlCommand(createTempTableCmd, conn)
                        cmd.ExecuteNonQuery()
                    End Using

                    Using bulkCopy As New SqlBulkCopy(conn)
                        bulkCopy.DestinationTableName = "TempData_Debug"
                        bulkCopy.ColumnMappings.Add("Location", "Location")
                        bulkCopy.ColumnMappings.Add("Customer First Name", "Customer First Name")
                        bulkCopy.ColumnMappings.Add("Customer Last Name", "Customer Last Name")
                        bulkCopy.ColumnMappings.Add("Customer Phone Number", "Customer Phone Number")
                        bulkCopy.ColumnMappings.Add("Customer Email", "Customer Email")
                        bulkCopy.BatchSize = 5000
                        bulkCopy.BulkCopyTimeout = 600
                        bulkCopy.WriteToServer(bulkDt)
                    End Using
                End Using

                Return bulkDt.Rows.Count
            End Function), Integer)

            If totalInserted = 0 Then
                MessageBox.Show("No valid rows matched to insert.")
                UpdateStatus("⚠️ No valid rows to insert")
            Else
                UpdateStatus($"✅ Exported {totalInserted:N0} rows to TempData_Debug")
                MessageBox.Show($"Successfully bulk inserted {totalInserted} rows into table TempData_Debug in Univista database.")
            End If

        Catch ex As Exception
            UpdateStatus("❌ Export error")
            MessageBox.Show("Error inserting data: " & ex.Message)
        Finally
            pbLoading.Visible = False
            btnExportDB.Enabled = True
            Cursor = Cursors.Default
        End Try
    End Sub

    Private Function GetSafeDate(row As DataRow, colName As String) As Object
        If Not row.Table.Columns.Contains(colName) Then Return DBNull.Value
        Dim rawVal = row(colName)
        If IsDBNull(rawVal) OrElse rawVal Is Nothing OrElse String.IsNullOrWhiteSpace(rawVal.ToString()) Then Return DBNull.Value
        If TypeOf rawVal Is Date Then Return rawVal
        Dim parsedDate As Date
        If Date.TryParse(rawVal.ToString(), parsedDate) Then Return parsedDate
        Return DBNull.Value
    End Function

End Class