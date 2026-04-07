Imports System.IO
Imports System.Data.OleDb
Imports System.Data.SqlClient

Public Class Form1

    Private pbLoading As ProgressBar

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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

        ' Hover effects for Read Azure button (blue)
        AddHandler btnReadAzure.MouseEnter, Sub(s, ev)
                                                btnReadAzure.BackColor = Color.FromArgb(29, 78, 187)
                                            End Sub
        AddHandler btnReadAzure.MouseLeave, Sub(s, ev)
                                                btnReadAzure.BackColor = Color.FromArgb(37, 99, 235)
                                            End Sub

        ' Hover effects for Insert Snapshot button (green)
        AddHandler btnInsertSnapshot.MouseEnter, Sub(s, ev)
                                                     btnInsertSnapshot.BackColor = Color.FromArgb(16, 130, 58)
                                                 End Sub
        AddHandler btnInsertSnapshot.MouseLeave, Sub(s, ev)
                                                     btnInsertSnapshot.BackColor = Color.FromArgb(22, 163, 74)
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
    '  BUTTON 2: Read from Azure (Async)
    ' ─────────────────────────────────────────────
    Private Async Sub btnReadAzure_Click(sender As Object, e As EventArgs) Handles btnReadAzure.Click
        Dim policyNumbers As New HashSet(Of String)
        Dim dtExcel As DataTable = Nothing

        If DtGV1.DataSource IsNot Nothing Then
            dtExcel = CType(DtGV1.DataSource, DataTable)
            Dim colName As String = "Policy number"

            If Not dtExcel.Columns.Contains(colName) Then
                MessageBox.Show($"Column '{colName}' not found in the uploaded Excel. Please check the header name.")
                Return
            End If

            For Each row As DataRow In dtExcel.Rows
                Dim policyVal = row(colName).ToString()
                If Not String.IsNullOrWhiteSpace(policyVal) Then
                    policyNumbers.Add("'" & policyVal.Replace("'", "''") & "'")
                End If
            Next
        Else
            MessageBox.Show("Please upload an Excel file first.")
            Return
        End If

        If policyNumbers.Count = 0 Then
            MessageBox.Show("No valid policy numbers found to search.")
            Return
        End If

        Dim policyFilter As String = String.Join(",", policyNumbers)
        UpdateStatus($"🔍 Querying Azure for {policyNumbers.Count:N0} policies...")
        
        btnReadAzure.Enabled = False
        pbLoading.Visible = True
        Cursor = Cursors.WaitCursor
        DtGV1.SuspendLayout()

        Try
            Dim dtAzure As DataTable = CType(Await Task.Run(Function() As DataTable
                Dim builder As New SqlConnectionStringBuilder()
                builder.DataSource = "univista-azure-sql.database.windows.net"
                builder.UserID = "dbadmin"
                builder.Password = "jtr<wjF[{2nsf*rU<D0!"
                builder.InitialCatalog = "Unisoft"

                Dim query As String = "SELECT
                    p.ExternalPolicyNumber,
                    p.PolicyTerm,
                    p.KeyCompany,
                    p.KeyMga,
                    p.KeyLine,
                    p.KeyPrefix,
                    p.KeyPolicy,
                    p.PolicyStatus 
                FROM dbo.MSTPOLD AS p
                WHERE p.KeyRevision = 99
                AND p.ExternalPolicyNumber IN (" & policyFilter & ")"

                Using adapter As New SqlDataAdapter(query, builder.ConnectionString)
                    Dim localDt As New DataTable()
                    adapter.Fill(localDt)
                    Return localDt
                End Using
            End Function), DataTable)

            ' Merging the two DataTables locally
            Await Task.Run(Sub()
                               For Each col As DataColumn In dtAzure.Columns
                                   If Not dtExcel.Columns.Contains(col.ColumnName) Then
                                       dtExcel.Columns.Add(col.ColumnName, col.DataType)
                                   End If
                               Next

                               Dim azureDataMap As New Dictionary(Of String, DataRow)
                               For Each row As DataRow In dtAzure.Rows
                                   Dim keyVal As String = row("ExternalPolicyNumber").ToString()
                                   If Not azureDataMap.ContainsKey(keyVal) Then
                                       azureDataMap(keyVal) = row
                                   End If
                               Next

                               Dim excelMatchCol As String = "Policy number"
                               If dtExcel.Columns.Contains(excelMatchCol) Then
                                   For Each row As DataRow In dtExcel.Rows
                                       Dim policyKey As String = row(excelMatchCol).ToString()
                                       If azureDataMap.ContainsKey(policyKey) Then
                                           Dim azureRow As DataRow = azureDataMap(policyKey)
                                           For Each col As DataColumn In dtAzure.Columns
                                               row(col.ColumnName) = azureRow(col.ColumnName)
                                           Next
                                       End If
                                   Next
                               End If
                           End Sub)

            DtGV1.ResumeLayout()
            DtGV1.Refresh()
            UpdateStatus($"✅ Azure data merged — {policyNumbers.Count:N0} policies matched")
            UpdateRowCount()
            MessageBox.Show("Data loaded and merged from Azure successfully!")

        Catch ex As Exception
            DtGV1.ResumeLayout()
            UpdateStatus("❌ Azure connection error")
            MessageBox.Show("Error connecting to Azure: " & ex.Message)
        Finally
            btnReadAzure.Enabled = True
            pbLoading.Visible = False
            Cursor = Cursors.Default
        End Try
    End Sub

    ' ─────────────────────────────────────────────
    '  BUTTON 3: Insert Snapshot (Bulk Copy Async)
    ' ─────────────────────────────────────────────
    Private Async Sub btnInsertSnapshot_Click(sender As Object, e As EventArgs) Handles btnInsertSnapshot.Click
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
        btnInsertSnapshot.Enabled = False
        Cursor = Cursors.WaitCursor
        UpdateStatus("💾 Preparing bulk insert...")

        Try
            Dim totalInserted As Integer = CType(Await Task.Run(Function() As Integer
                Dim builder As New SqlConnectionStringBuilder()
                builder.DataSource = "172.190.120.3"
                builder.InitialCatalog = "ReportDB"
                builder.UserID = "sa"
                builder.Password = "Dell2014#"
                builder.TrustServerCertificate = True

                Dim bulkDt As New DataTable()
                bulkDt.Columns.Add("SnapshotMonthEnd", GetType(Date))
                bulkDt.Columns.Add("PolicyNumber", GetType(String))
                bulkDt.Columns.Add("InsuredName", GetType(String))
                bulkDt.Columns.Add("City", GetType(String))
                bulkDt.Columns.Add("Zipcode", GetType(String))
                bulkDt.Columns.Add("EffectiveDate", GetType(Object))
                bulkDt.Columns.Add("ExpirationDate", GetType(Object))
                bulkDt.Columns.Add("CancellationDate", GetType(Object))
                bulkDt.Columns.Add("PolicyTerm", GetType(Object))
                bulkDt.Columns.Add("KeyCompany", GetType(String))
                bulkDt.Columns.Add("KeyMga", GetType(String))
                bulkDt.Columns.Add("KeyLine", GetType(String))
                bulkDt.Columns.Add("KeyPrefix", GetType(String))
                bulkDt.Columns.Add("KeyPolicy", GetType(String))
                bulkDt.Columns.Add("PolicyStatus", GetType(String))
                bulkDt.Columns.Add("LoadDate", GetType(Date))

                Dim snapshotDate As Date = New Date(2026, 3, 31)
                Dim currentLoadDate As Date = DateTime.Now

                For Each row As DataRow In dt.Rows
                    If row.Table.Columns.Contains("KeyPolicy") AndAlso Not IsDBNull(row("KeyPolicy")) Then
                        Dim newRow As DataRow = bulkDt.NewRow()

                        newRow("SnapshotMonthEnd") = snapshotDate
                        newRow("PolicyNumber") = If(row.Table.Columns.Contains("ExternalPolicyNumber"), row("ExternalPolicyNumber"), DBNull.Value)

                        Dim insuredName As Object = DBNull.Value
                        If row.Table.Columns.Contains("InsuredName") Then
                            insuredName = row("InsuredName")
                        ElseIf row.Table.Columns.Contains("UnderwriterUserName") Then
                            insuredName = row("UnderwriterUserName")
                        End If
                        newRow("InsuredName") = insuredName

                        newRow("City") = If(row.Table.Columns.Contains("City"), row("City"), DBNull.Value)
                        newRow("Zipcode") = If(row.Table.Columns.Contains("Zipcode"), row("Zipcode"), DBNull.Value)
                        newRow("EffectiveDate") = GetSafeDate(row, "EffectiveDate")
                        newRow("ExpirationDate") = GetSafeDate(row, "ExpirationDate")
                        newRow("CancellationDate") = GetSafeDate(row, "CancellationDate")
                        newRow("PolicyTerm") = If(row.Table.Columns.Contains("PolicyTerm"), row("PolicyTerm"), DBNull.Value)
                        newRow("KeyCompany") = If(row.Table.Columns.Contains("KeyCompany"), row("KeyCompany"), DBNull.Value)
                        newRow("KeyMga") = If(row.Table.Columns.Contains("KeyMga"), row("KeyMga"), DBNull.Value)
                        newRow("KeyLine") = If(row.Table.Columns.Contains("KeyLine"), row("KeyLine"), DBNull.Value)
                        newRow("KeyPrefix") = If(row.Table.Columns.Contains("KeyPrefix"), row("KeyPrefix"), DBNull.Value)
                        newRow("KeyPolicy") = If(row.Table.Columns.Contains("KeyPolicy"), row("KeyPolicy"), DBNull.Value)
                        newRow("PolicyStatus") = If(row.Table.Columns.Contains("PolicyStatus"), row("PolicyStatus"), DBNull.Value)
                        newRow("LoadDate") = currentLoadDate

                        bulkDt.Rows.Add(newRow)
                    End If
                Next

                If bulkDt.Rows.Count = 0 Then
                    Return 0
                End If

                Using conn As New SqlConnection(builder.ConnectionString)
                    conn.Open()
                    Using bulkCopy As New SqlBulkCopy(conn)
                        bulkCopy.DestinationTableName = "dbo.PolicyInForceSnapshot"
                        bulkCopy.ColumnMappings.Add("SnapshotMonthEnd", "SnapshotMonthEnd")
                        bulkCopy.ColumnMappings.Add("PolicyNumber", "PolicyNumber")
                        bulkCopy.ColumnMappings.Add("InsuredName", "InsuredName")
                        bulkCopy.ColumnMappings.Add("City", "City")
                        bulkCopy.ColumnMappings.Add("Zipcode", "Zipcode")
                        bulkCopy.ColumnMappings.Add("EffectiveDate", "EffectiveDate")
                        bulkCopy.ColumnMappings.Add("ExpirationDate", "ExpirationDate")
                        bulkCopy.ColumnMappings.Add("CancellationDate", "CancellationDate")
                        bulkCopy.ColumnMappings.Add("PolicyTerm", "PolicyTerm")
                        bulkCopy.ColumnMappings.Add("KeyCompany", "KeyCompany")
                        bulkCopy.ColumnMappings.Add("KeyMga", "KeyMga")
                        bulkCopy.ColumnMappings.Add("KeyLine", "KeyLine")
                        bulkCopy.ColumnMappings.Add("KeyPrefix", "KeyPrefix")
                        bulkCopy.ColumnMappings.Add("KeyPolicy", "KeyPolicy")
                        bulkCopy.ColumnMappings.Add("PolicyStatus", "PolicyStatus")
                        bulkCopy.ColumnMappings.Add("LoadDate", "LoadDate")
                        
                        bulkCopy.BatchSize = 5000
                        bulkCopy.BulkCopyTimeout = 600
                        bulkCopy.WriteToServer(bulkDt)
                    End Using

                    Dim logQuery As String = "INSERT INTO dbo.PolicyInForceSnapshotLog (LogDate, InsertedRows, InForceSnapshot) VALUES (@LogDate, @InsertedRows, @InForceSnapshot)"
                    Using cmdLog As New SqlCommand(logQuery, conn)
                        cmdLog.Parameters.AddWithValue("@LogDate", DateTime.Now)
                        cmdLog.Parameters.AddWithValue("@InsertedRows", bulkDt.Rows.Count)
                        cmdLog.Parameters.AddWithValue("@InForceSnapshot", snapshotDate)
                        cmdLog.ExecuteNonQuery()
                    End Using
                End Using

                Return bulkDt.Rows.Count
            End Function), Integer)

            If totalInserted = 0 Then
                MessageBox.Show("No valid rows matched to insert (missing KeyPolicy).")
                UpdateStatus("⚠️ No valid rows to insert")
            Else
                UpdateStatus($"✅ Inserted {totalInserted:N0} rows into PolicyInForceSnapshot")
                MessageBox.Show($"Successfully bulk inserted {totalInserted} rows into ReportDB.")
            End If

        Catch ex As Exception
            UpdateStatus("❌ Insert error")
            MessageBox.Show("Error inserting data: " & ex.Message)
        Finally
            pbLoading.Visible = False
            btnInsertSnapshot.Enabled = True
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