Imports System.IO

Public Class FormTxt
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
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

            Cursor = Cursors.WaitCursor


            DtGV1.SuspendLayout()

            Try
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
                        MessageBox.Show("No valid worksheets found.")
                        Return
                    End If

                    Dim selectCmd As String = "SELECT * FROM [" & sheetName & "]"

                    Using adapter As New OleDbDataAdapter(selectCmd, conn)
                        Dim dt As New DataTable()
                        adapter.Fill(dt)


                        DtGV1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
                        DtGV1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing

                        DtGV1.DataSource = dt
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show("Failed to read Excel file: " & ex.Message)
            Finally

                SetDoubleBuffered(DtGV1)


                DtGV1.ResumeLayout()
                Cursor = Cursors.Default
            End Try
        End Using
    End Sub


    Private Sub SetDoubleBuffered(control As Control)
        Try
            Dim dgvType As Type = GetType(Control)
            Dim pi As System.Reflection.PropertyInfo = dgvType.GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.NonPublic)
            pi.SetValue(control, True, Nothing)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Dim policyNumbers As New HashSet(Of String)

        If DtGV1.DataSource IsNot Nothing Then
            Dim dtExcel As DataTable = CType(DtGV1.DataSource, DataTable)


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

        ' Connection Azure SQL Database

        Dim builder As New System.Data.SqlClient.SqlConnectionStringBuilder()
        builder.DataSource = "univista-azure-sql.database.windows.net"
        builder.UserID = "dbadmin"
        builder.Password = "jtr<wjF[{2nsf*rU<D0!"
        builder.InitialCatalog = "Unisoft"

        Dim connString As String = builder.ConnectionString

        ' Query to execute 
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

        Cursor = Cursors.WaitCursor
        Try
            Using adapter As New SqlDataAdapter(query, connString)
                Dim dtAzure As New DataTable()
                adapter.Fill(dtAzure)

                ' Check if we have an existing DataTable to merge into
                Dim dtExcel As DataTable = Nothing
                If TypeOf DtGV1.DataSource Is DataTable Then
                    dtExcel = CType(DtGV1.DataSource, DataTable)
                End If

                If dtExcel IsNot Nothing Then

                    DtGV1.SuspendLayout()

                    For Each col As DataColumn In dtAzure.Columns
                        If Not dtExcel.Columns.Contains(col.ColumnName) Then
                            dtExcel.Columns.Add(col.ColumnName, col.DataType)
                        End If
                    Next


                    ' Key: ExternalPolicyNumber (from Azure), Value: The entire row
                    Dim azureDataMap As New Dictionary(Of String, DataRow)
                    For Each row As DataRow In dtAzure.Rows
                        Dim keyVal As String = row("ExternalPolicyNumber").ToString()
                        If Not azureDataMap.ContainsKey(keyVal) Then
                            azureDataMap(keyVal) = row
                        End If
                    Next

                    '  Iterate through Excel rows and populate the new columns
                    Dim excelMatchCol As String = "Policy number" ' The column from Excel

                    If dtExcel.Columns.Contains(excelMatchCol) Then
                        For Each row As DataRow In dtExcel.Rows
                            Dim policyKey As String = row(excelMatchCol).ToString()

                            If azureDataMap.ContainsKey(policyKey) Then
                                Dim azureRow As DataRow = azureDataMap(policyKey)
                                ' Update values for common/new columns
                                For Each col As DataColumn In dtAzure.Columns
                                    row(col.ColumnName) = azureRow(col.ColumnName)
                                Next
                            End If
                        Next
                    End If


                    DtGV1.ResumeLayout()
                    DtGV1.Refresh()
                Else

                    DtGV1.DataSource = dtAzure
                End If
            End Using
            'End Using
            MessageBox.Show("Data loaded and merged from Azure successfully!")
        Catch ex As Exception
            MessageBox.Show("Error connecting to Azure: " & ex.Message)
        Finally
            Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If DtGV1.DataSource Is Nothing Then
            MessageBox.Show("No data to insert. Please load data first.")
            Return
        End If

        Dim dt As DataTable = CType(DtGV1.DataSource, DataTable)
        If dt.Rows.Count = 0 Then
            MessageBox.Show("No rows to insert.")
            Return
        End If

        ' Connection string for the target database
        Dim builder As New System.Data.SqlClient.SqlConnectionStringBuilder()
        builder.DataSource = "172.190.120.3"
        builder.InitialCatalog = "ReportDB"
        builder.UserID = "sa"
        builder.Password = "Dell2014#"
        builder.TrustServerCertificate = True

        '  DataTable for Bulk Copy
        Dim bulkDt As New DataTable()
        bulkDt.Columns.Add("SnapshotMonthEnd", GetType(Date))
        bulkDt.Columns.Add("PolicyNumber", GetType(String))
        bulkDt.Columns.Add("InsuredName", GetType(String))
        bulkDt.Columns.Add("City", GetType(String))
        bulkDt.Columns.Add("Zipcode", GetType(String))
        bulkDt.Columns.Add("EffectiveDate", GetType(Object)) ' Object to handle DBNull easily
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

        ' SnapshotMonthEnd
        Dim snapshotDate As Date = New Date(2026, 1, 31)
        Dim currentLoadDate As Date = DateTime.Now

        Cursor = Cursors.WaitCursor
        Try
            ' Bulk DataTable in memory (much faster than DB calls)
            For Each row As DataRow In dt.Rows
                ' We only insert if we have the critical keys (coming from the Azure merge)
                If row.Table.Columns.Contains("KeyPolicy") AndAlso Not IsDBNull(row("KeyPolicy")) Then
                    Dim newRow As DataRow = bulkDt.NewRow()

                    newRow("SnapshotMonthEnd") = snapshotDate

                    ' Map columns 
                    newRow("PolicyNumber") = If(row.Table.Columns.Contains("ExternalPolicyNumber"), row("ExternalPolicyNumber"), DBNull.Value)

                    ' Check if InsuredName exists in Excel or Azure columns
                    Dim insuredName As Object = DBNull.Value
                    If row.Table.Columns.Contains("InsuredName") Then
                        insuredName = row("InsuredName")
                    ElseIf row.Table.Columns.Contains("UnderwriterUserName") Then
                        insuredName = row("UnderwriterUserName")
                    End If
                    newRow("InsuredName") = insuredName

                    newRow("City") = If(row.Table.Columns.Contains("City"), row("City"), DBNull.Value)
                    newRow("Zipcode") = If(row.Table.Columns.Contains("Zipcode"), row("Zipcode"), DBNull.Value)
                    ' Safe Date Parsing using helper
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
                MessageBox.Show("No valid rows matched to insert (missing KeyPolicy).")
                Return
            End If

            '  Execute Bulk Copy
            Using conn As New System.Data.SqlClient.SqlConnection(builder.ConnectionString)
                conn.Open()
                Using bulkCopy As New System.Data.SqlClient.SqlBulkCopy(conn)
                    bulkCopy.DestinationTableName = "dbo.PolicyInForceSnapshot"

                    ' Verify column mappings
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

                    ' Set batch size for very large datasets (e.g., 5000)
                    bulkCopy.BatchSize = 5000
                    ' Set timeout if it takes long
                    bulkCopy.BulkCopyTimeout = 600

                    bulkCopy.WriteToServer(bulkDt)
                End Using

                ' insert in ReportDB.dbo.PolicyInForceSnapshotLog
                Dim logQuery As String = "INSERT INTO dbo.PolicyInForceSnapshotLog (LogDate, InsertedRows, InForceSnapshot) VALUES (@LogDate, @InsertedRows, @InForceSnapshot)"
                Dim snapshotDatex As Date = New Date(2026, 1, 31)
                Using cmdLog As New SqlCommand(logQuery, conn)
                    cmdLog.Parameters.AddWithValue("@LogDate", DateTime.Now)
                    cmdLog.Parameters.AddWithValue("@InsertedRows", bulkDt.Rows.Count)
                    cmdLog.Parameters.AddWithValue("@InForceSnapshot", snapshotDatex)
                    cmdLog.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show($"Successfully bulk inserted {bulkDt.Rows.Count} rows into ReportDB.")

        Catch ex As Exception
            MessageBox.Show("Error inserting data: " & ex.Message)
        Finally
            Cursor = Cursors.Default
        End Try
    End Sub
    ' Helper function to safely parse dates from a DataRow
    Private Function GetSafeDate(row As DataRow, colName As String) As Object
        ' Check if column exists
        If Not row.Table.Columns.Contains(colName) Then
            Return DBNull.Value
        End If

        Dim rawVal = row(colName)

        ' Check for Null or Empty
        If IsDBNull(rawVal) OrElse rawVal Is Nothing OrElse String.IsNullOrWhiteSpace(rawVal.ToString()) Then
            Return DBNull.Value
        End If

        ' If it's already a Date, return it
        If TypeOf rawVal Is Date Then
            Return rawVal
        End If

        ' Try to parse String to Date
        Dim parsedDate As Date
        If Date.TryParse(rawVal.ToString(), parsedDate) Then
            Return parsedDate
        End If

        ' If parsing fails, return DBNull
        Return DBNull.Value
    End Function

End Class