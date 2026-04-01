Imports System.IO
Imports System.Data.OleDb
Imports System.Data.SqlClient

Public Class FormFrancoNet

    Private pbLoading As ProgressBar

    Private Sub FormFrancoNet_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
    '  BUTTON 2: Export DB (FrancoNet_Temp Async)
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
                bulkDt.Columns.Add("Inquiry Date", GetType(String))
                bulkDt.Columns.Add("Salutation", GetType(String))
                bulkDt.Columns.Add("First Name", GetType(String))
                bulkDt.Columns.Add("Last Name", GetType(String))
                bulkDt.Columns.Add("Address1", GetType(String))
                bulkDt.Columns.Add("Address2", GetType(String))
                bulkDt.Columns.Add("City", GetType(String))
                bulkDt.Columns.Add("Country", GetType(String))
                bulkDt.Columns.Add("State / Province", GetType(String))
                bulkDt.Columns.Add("Zip / Postal Code", GetType(String))
                bulkDt.Columns.Add("County", GetType(String))
                bulkDt.Columns.Add("Preferred Mode of Contact", GetType(String))
                bulkDt.Columns.Add("Work Phone", GetType(String))
                bulkDt.Columns.Add("Home Phone", GetType(String))
                bulkDt.Columns.Add("Mobile", GetType(String))
                bulkDt.Columns.Add("Email", GetType(String))
                bulkDt.Columns.Add("Comments", GetType(String))
                bulkDt.Columns.Add("Email Unsubscribe Reason", GetType(String))
                bulkDt.Columns.Add("Lead Status", GetType(String))
                bulkDt.Columns.Add("Lead Owner", GetType(String))
                bulkDt.Columns.Add("Lead Reason", GetType(String))
                bulkDt.Columns.Add("Lead Reason Comment", GetType(String))
                bulkDt.Columns.Add("Lead Source Category", GetType(String))
                bulkDt.Columns.Add("Lead Source Details", GetType(String))
                bulkDt.Columns.Add("Web Site Lead", GetType(String))
                bulkDt.Columns.Add("Forecast Revenue", GetType(String))
                bulkDt.Columns.Add("First Contacted", GetType(String))
                bulkDt.Columns.Add("Last Contacted", GetType(String))
                bulkDt.Columns.Add("Last Updated On", GetType(String))
                bulkDt.Columns.Add("Group Name", GetType(String))

                For Each row As DataRow In dt.Rows
                    Dim newRow As DataRow = bulkDt.NewRow()
                    Dim cols As DataColumnCollection = row.Table.Columns

                    Dim SafeVal As Func(Of String, Object) =
                        Function(col As String) As Object
                            If cols.Contains(col) Then
                                Return If(IsDBNull(row(col)), DBNull.Value, CObj(row(col).ToString()))
                            End If
                            Return DBNull.Value
                        End Function

                    newRow("Inquiry Date") = SafeVal("Inquiry Date")
                    newRow("Salutation") = SafeVal("Salutation")
                    newRow("First Name") = SafeVal("First Name")
                    newRow("Last Name") = SafeVal("Last Name")
                    newRow("Address1") = SafeVal("Address1")
                    newRow("Address2") = SafeVal("Address2")
                    newRow("City") = SafeVal("City")
                    newRow("Country") = SafeVal("Country")
                    newRow("State / Province") = SafeVal("State / Province")
                    newRow("Zip / Postal Code") = SafeVal("Zip / Postal Code")
                    newRow("County") = SafeVal("County")
                    newRow("Preferred Mode of Contact") = SafeVal("Preferred Mode of Contact")
                    newRow("Work Phone") = SafeVal("Work Phone")
                    newRow("Home Phone") = SafeVal("Home Phone")
                    newRow("Mobile") = SafeVal("Mobile")
                    newRow("Email") = SafeVal("Email")
                    newRow("Comments") = SafeVal("Comments")
                    newRow("Email Unsubscribe Reason") = SafeVal("Email Unsubscribe Reason")
                    newRow("Lead Status") = SafeVal("Lead Status")
                    newRow("Lead Owner") = SafeVal("Lead Owner")
                    newRow("Lead Reason") = SafeVal("Lead Reason")
                    newRow("Lead Reason Comment") = SafeVal("Lead Reason Comment")
                    newRow("Lead Source Category") = SafeVal("Lead Source Category")
                    newRow("Lead Source Details") = SafeVal("Lead Source Details")
                    newRow("Web Site Lead") = SafeVal("Web Site Lead")
                    newRow("Forecast Revenue") = SafeVal("Forecast Revenue")
                    newRow("First Contacted") = SafeVal("First Contacted")
                    newRow("Last Contacted") = SafeVal("Last Contacted")
                    newRow("Last Updated On") = SafeVal("Last Updated On")
                    newRow("Group Name") = SafeVal("Group Name")

                    bulkDt.Rows.Add(newRow)
                Next

                If bulkDt.Rows.Count = 0 Then Return 0

                Using conn As New SqlConnection(builder.ConnectionString)
                    conn.Open()

                    ' Append Logic: Create only if NOT exists. Do NOT drop.
                    Dim createTempTableCmd As String = "
                        IF OBJECT_ID('FrancoNet_Temp', 'U') IS NULL
                        BEGIN
                            CREATE TABLE FrancoNet_Temp (
                                [Inquiry Date] NVARCHAR(50),
                                [Salutation] NVARCHAR(50),
                                [First Name] NVARCHAR(100),
                                [Last Name] NVARCHAR(100),
                                [Address1] NVARCHAR(255),
                                [Address2] NVARCHAR(255),
                                [City] NVARCHAR(100),
                                [Country] NVARCHAR(100),
                                [State / Province] NVARCHAR(100),
                                [Zip / Postal Code] NVARCHAR(20),
                                [County] NVARCHAR(100),
                                [Preferred Mode of Contact] NVARCHAR(100),
                                [Work Phone] NVARCHAR(50),
                                [Home Phone] NVARCHAR(50),
                                [Mobile] NVARCHAR(50),
                                [Email] NVARCHAR(255),
                                [Comments] NVARCHAR(MAX),
                                [Email Unsubscribe Reason] NVARCHAR(255),
                                [Lead Status] NVARCHAR(100),
                                [Lead Owner] NVARCHAR(100),
                                [Lead Reason] NVARCHAR(255),
                                [Lead Reason Comment] NVARCHAR(255),
                                [Lead Source Category] NVARCHAR(100),
                                [Lead Source Details] NVARCHAR(100),
                                [Web Site Lead] NVARCHAR(10),
                                [Forecast Revenue] NVARCHAR(50),
                                [First Contacted] NVARCHAR(50),
                                [Last Contacted] NVARCHAR(50),
                                [Last Updated On] NVARCHAR(50),
                                [Group Name] NVARCHAR(255),
                                [ImportDate] DATETIME DEFAULT GETDATE()
                            );
                        END"

                    Using cmd As New SqlCommand(createTempTableCmd, conn)
                        cmd.ExecuteNonQuery()
                    End Using

                    Using bulkCopy As New SqlBulkCopy(conn)
                        bulkCopy.DestinationTableName = "FrancoNet_Temp"
                        bulkCopy.ColumnMappings.Add("Inquiry Date", "Inquiry Date")
                        bulkCopy.ColumnMappings.Add("Salutation", "Salutation")
                        bulkCopy.ColumnMappings.Add("First Name", "First Name")
                        bulkCopy.ColumnMappings.Add("Last Name", "Last Name")
                        bulkCopy.ColumnMappings.Add("Address1", "Address1")
                        bulkCopy.ColumnMappings.Add("Address2", "Address2")
                        bulkCopy.ColumnMappings.Add("City", "City")
                        bulkCopy.ColumnMappings.Add("Country", "Country")
                        bulkCopy.ColumnMappings.Add("State / Province", "State / Province")
                        bulkCopy.ColumnMappings.Add("Zip / Postal Code", "Zip / Postal Code")
                        bulkCopy.ColumnMappings.Add("County", "County")
                        bulkCopy.ColumnMappings.Add("Preferred Mode of Contact", "Preferred Mode of Contact")
                        bulkCopy.ColumnMappings.Add("Work Phone", "Work Phone")
                        bulkCopy.ColumnMappings.Add("Home Phone", "Home Phone")
                        bulkCopy.ColumnMappings.Add("Mobile", "Mobile")
                        bulkCopy.ColumnMappings.Add("Email", "Email")
                        bulkCopy.ColumnMappings.Add("Comments", "Comments")
                        bulkCopy.ColumnMappings.Add("Email Unsubscribe Reason", "Email Unsubscribe Reason")
                        bulkCopy.ColumnMappings.Add("Lead Status", "Lead Status")
                        bulkCopy.ColumnMappings.Add("Lead Owner", "Lead Owner")
                        bulkCopy.ColumnMappings.Add("Lead Reason", "Lead Reason")
                        bulkCopy.ColumnMappings.Add("Lead Reason Comment", "Lead Reason Comment")
                        bulkCopy.ColumnMappings.Add("Lead Source Category", "Lead Source Category")
                        bulkCopy.ColumnMappings.Add("Lead Source Details", "Lead Source Details")
                        bulkCopy.ColumnMappings.Add("Web Site Lead", "Web Site Lead")
                        bulkCopy.ColumnMappings.Add("Forecast Revenue", "Forecast Revenue")
                        bulkCopy.ColumnMappings.Add("First Contacted", "First Contacted")
                        bulkCopy.ColumnMappings.Add("Last Contacted", "Last Contacted")
                        bulkCopy.ColumnMappings.Add("Last Updated On", "Last Updated On")
                        bulkCopy.ColumnMappings.Add("Group Name", "Group Name")
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
                UpdateStatus($"✅ Appended {totalInserted:N0} rows to FrancoNet_Temp")
                MessageBox.Show($"Successfully appended {totalInserted} rows into table FrancoNet_Temp in Univista database.")
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

End Class
