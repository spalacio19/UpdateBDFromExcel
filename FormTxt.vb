Imports System.IO

Public Class FormTxt

    ' ─────────────────────────────────────────────────────────────────────────
    ' Button1 – Upload Excel → show raw data in DtGV1
    ' ─────────────────────────────────────────────────────────────────────────
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Using ofd As New OpenFileDialog()
            ofd.Filter = "Text/CSV files|*.txt;*.csv|All files|*.*"
            If ofd.ShowDialog() <> DialogResult.OK Then Return

            Dim file = ofd.FileName
            Label1.Text = file
            Label1.ForeColor = System.Drawing.Color.Black

            DtGV1.SuspendLayout()
            DtGV2.DataSource = Nothing
            BtnPreview.Enabled = False
            BtnInsert.Enabled = False
            Cursor = Cursors.WaitCursor

            Try
                Dim dt As New DataTable()
                Dim isFirstLine As Boolean = True

                Using sr As New IO.StreamReader(file, System.Text.Encoding.UTF8)
                    While Not sr.EndOfStream
                        Dim line As String = sr.ReadLine()
                        If String.IsNullOrWhiteSpace(line) Then Continue While

                        ' Split by comma — handle quoted fields
                        Dim fields() As String = SplitCsvLine(line)

                        If isFirstLine Then
                            ' First row = headers
                            For Each header As String In fields
                                dt.Columns.Add(header.Trim().Trim(""""c))
                            Next
                            isFirstLine = False
                        Else
                            ' Pad or trim fields to match column count
                            Dim row As DataRow = dt.NewRow()
                            For i As Integer = 0 To Math.Min(fields.Length, dt.Columns.Count) - 1
                                row(i) = fields(i).Trim().Trim(""""c)
                            Next
                            dt.Rows.Add(row)
                        End If
                    End While
                End Using

                DtGV1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
                DtGV1.DataSource = dt
                SetDoubleBuffered(DtGV1)
                BtnPreview.Enabled = True
                LblMapped.Text = $"TXT Data (raw) — {dt.Rows.Count} rows:"

            Catch ex As Exception
                MessageBox.Show("Failed to read TXT file: " & ex.Message)
            Finally
                DtGV1.ResumeLayout()
                Cursor = Cursors.Default
            End Try
        End Using
    End Sub

    ' Splits a CSV line respecting quoted fields that may contain commas
    Private Function SplitCsvLine(line As String) As String()
        Dim result As New List(Of String)
        Dim current As New System.Text.StringBuilder()
        Dim inQuotes As Boolean = False

        For Each c As Char In line
            If c = """"c Then
                inQuotes = Not inQuotes
            ElseIf c = ","c AndAlso Not inQuotes Then
                result.Add(current.ToString())
                current.Clear()
            Else
                current.Append(c)
            End If
        Next
        result.Add(current.ToString())
        Return result.ToArray()
    End Function


    ' ─────────────────────────────────────────────────────────────────────────
    ' BtnPreview – Map columns, check duplicates in DB, show in DtGV2
    ' ─────────────────────────────────────────────────────────────────────────
    Private Sub BtnPreview_Click(sender As Object, e As EventArgs) Handles BtnPreview.Click
        UpdatePreviewData()
    End Sub

    Private Sub LoadPreviewData()
        If DtGV1.DataSource Is Nothing Then
            MessageBox.Show("Please upload an  file first.")
            Return
        End If

        Dim dtExcel As DataTable = CType(DtGV1.DataSource, DataTable)

        For Each col As String In {"AreaCode", "LocalNumber"}
            If Not HasColumn(dtExcel, col) Then
                MessageBox.Show($"Required column '{col}' not found in the Excel file." & vbCrLf &
                                "Make sure the header names match exactly.")
                Return
            End If
        Next

        Dim defaultUser As String = Environment.UserName
        Dim defaultDate As DateTime = DateTime.Now
        Dim skipped As Integer = 0

        ' ── Build in-memory preview table ─────────────────────────────────
        Dim previewDt As New DataTable()
        previewDt.Columns.Add("Estado", GetType(String))        ' NEW — duplicate flag
        previewDt.Columns.Add("AreaCode", GetType(String))
        previewDt.Columns.Add("LocalNumber", GetType(String))
        previewDt.Columns.Add("State", GetType(String))
        previewDt.Columns.Add("AddedDate", GetType(DateTime))
        previewDt.Columns.Add("AddedByUser", GetType(String))
        previewDt.Columns.Add("Source", GetType(String))
        previewDt.Columns.Add("Notes", GetType(String))

        For Each row As DataRow In dtExcel.Rows
            Dim areaCode As String = GetStr(row, "AreaCode")
            Dim localNumber As String = GetStr(row, "LocalNumber")

            If String.IsNullOrWhiteSpace(areaCode) OrElse String.IsNullOrWhiteSpace(localNumber) Then
                skipped += 1
                Continue For
            End If

            Dim newRow As DataRow = previewDt.NewRow()
            newRow("Estado") = "✔ Nuevo"           ' default — will update after DB check
            newRow("AreaCode") = areaCode.Trim()
            newRow("LocalNumber") = localNumber.Trim()

            Dim stateVal As String = GetStr(row, "State")
            newRow("State") = If(String.IsNullOrWhiteSpace(stateVal), DBNull.Value, CObj(stateVal.Trim()))

            Dim addedDateVal As Object = GetSafeDate(row, "AddedDate")
            newRow("AddedDate") = If(addedDateVal Is DBNull.Value, CObj(defaultDate), addedDateVal)

            Dim addedBy As String = GetStr(row, "AddedByUser")
            newRow("AddedByUser") = If(String.IsNullOrWhiteSpace(addedBy), defaultUser, addedBy.Trim())

            Dim sourceVal As String = GetStr(row, "Source")
            newRow("Source") = If(String.IsNullOrWhiteSpace(sourceVal), DBNull.Value, CObj(sourceVal.Trim()))

            Dim notesVal As String = GetStr(row, "Notes")
            newRow("Notes") = If(String.IsNullOrWhiteSpace(notesVal), DBNull.Value, CObj(notesVal.Trim()))

            previewDt.Rows.Add(newRow)
        Next

        If previewDt.Rows.Count = 0 Then
            MessageBox.Show("No valid rows to preview (AreaCode and LocalNumber are required in every row).")
            BtnInsert.Enabled = False
            Return
        End If

        ' ── Check duplicates against DB (temp-table + JOIN — scales to any size) ──
        Cursor = Cursors.WaitCursor
        Try
            Dim existingKeys As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

            Dim builder As New System.Data.SqlClient.SqlConnectionStringBuilder()
            builder.DataSource = "172.190.120.3"
            builder.InitialCatalog = "DNC"
            builder.UserID = "sa"
            builder.Password = "Dell2014#"
            builder.TrustServerCertificate = True

            ' Build an in-memory table with just the keys we want to check
            Dim keysDt As New DataTable()
            keysDt.Columns.Add("AreaCode", GetType(String))
            keysDt.Columns.Add("LocalNumber", GetType(String))
            For Each row As DataRow In previewDt.Rows
                keysDt.Rows.Add(row("AreaCode").ToString().Trim(),
                                row("LocalNumber").ToString().Trim())
            Next

            Using conn As New System.Data.SqlClient.SqlConnection(builder.ConnectionString)
                conn.Open()

                ' 1) Create a temp table for the incoming keys
                Dim createTmp As String =
                    "CREATE TABLE #tmpCheck (AreaCode VARCHAR(10), LocalNumber VARCHAR(20));"
                Using cmd As New System.Data.SqlClient.SqlCommand(createTmp, conn)
                    cmd.ExecuteNonQuery()
                End Using

                ' 2) Bulk-insert the incoming keys into the temp table (fast, no query-size limit)
                Using bc As New System.Data.SqlClient.SqlBulkCopy(conn)
                    bc.DestinationTableName = "#tmpCheck"
                    bc.ColumnMappings.Add("AreaCode", "AreaCode")
                    bc.ColumnMappings.Add("LocalNumber", "LocalNumber")
                    bc.BulkCopyTimeout = 120
                    bc.WriteToServer(keysDt)
                End Using

                ' 3) JOIN to find which keys already exist in the real table
                Dim checkSql As String =
                    "SELECT t.AreaCode, t.LocalNumber " &
                    "FROM #tmpCheck t " &
                    "INNER JOIN DNC.dbo.DoNotCallNumbers d " &
                    "  ON d.AreaCode = t.AreaCode AND d.LocalNumber = t.LocalNumber;"

                Using cmd As New SqlCommand(checkSql, conn)
                    cmd.CommandTimeout = 300
                    Using reader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim key As String = reader("AreaCode").ToString().Trim() & "|" &
                                                reader("LocalNumber").ToString().Trim()
                            existingKeys.Add(key)
                        End While
                    End Using
                End Using
            End Using

            ' Mark duplicates in preview table
            Dim dupCount As Integer = 0
            For Each row As DataRow In previewDt.Rows
                Dim key As String = row("AreaCode").ToString().Trim() & "|" &
                                    row("LocalNumber").ToString().Trim()
                If existingKeys.Contains(key) Then
                    row("Estado") = "⚠ Ya existe"
                    dupCount += 1
                End If
            Next

            ' Show in DtGV2
            DtGV2.DataSource = previewDt

            ' Color duplicate rows yellow/orange
            AddHandler DtGV2.CellFormatting, AddressOf DtGV2_CellFormatting

            BtnInsert.Enabled = True

            Dim summary As String = $"Preview listo: {previewDt.Rows.Count} filas totales." & vbCrLf &
                                    $"   ✔ Nuevas a insertar: {previewDt.Rows.Count - dupCount}" & vbCrLf &
                                    $"   ⚠ Ya existen en BD (no se insertarán): {dupCount}"
            If skipped > 0 Then summary &= $"{vbCrLf}   ⛔ Saltadas (sin AreaCode/LocalNumber): {skipped}"
            MessageBox.Show(summary, "Preview", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Error checking duplicates: " & ex.Message)
            ' Still show the data even if DB check fails
            DtGV2.DataSource = previewDt
            BtnInsert.Enabled = True
        Finally
            Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub UpdatePreviewData()
        If DtGV1.DataSource Is Nothing Then
            MessageBox.Show("Please upload an  file first.")
            Return
        End If

        Dim dtExcel As DataTable = CType(DtGV1.DataSource, DataTable)

        For Each col As String In {"PhoneNumber", "AddedDate", "Status"}
            If Not HasColumn(dtExcel, col) Then
                MessageBox.Show($"Required column '{col}' not found in the Excel file." & vbCrLf &
                                "Make sure the header names match exactly.")
                Return
            End If
        Next

        Dim defaultUser As String = Environment.UserName
        Dim defaultDate As DateTime = DateTime.Now
        Dim skipped As Integer = 0

        ' ── Build in-memory preview table ─────────────────────────────────
        Dim previewDt As New DataTable()
        previewDt.Columns.Add("Estado", GetType(String))        ' NEW — duplicate flag
        previewDt.Columns.Add("AreaCode", GetType(String))
        previewDt.Columns.Add("LocalNumber", GetType(String))
        previewDt.Columns.Add("State", GetType(String))
        previewDt.Columns.Add("AddedDate", GetType(DateTime))
        previewDt.Columns.Add("AddedByUser", GetType(String))
        previewDt.Columns.Add("Source", GetType(String))
        previewDt.Columns.Add("Notes", GetType(String))
        previewDt.Columns.Add("PhoneNumber", GetType(String))
        previewDt.Columns.Add("Status", GetType(String))



        For Each row As DataRow In dtExcel.Rows
            Dim PhoneNumber As String = GetStr(row, "PhoneNumber")


            If String.IsNullOrWhiteSpace(PhoneNumber) Then
                skipped += 1
                Continue For
            End If

            Dim newRow As DataRow = previewDt.NewRow()
            newRow("Estado") = "✔ Nuevo"           ' default — will update after DB check
            
            Dim cleanPhone As String = PhoneNumber.Trim()
            ' Splitting the 10-digit number into AreaCode (3 digits) and LocalNumber (7 digits)
            If cleanPhone.Length >= 10 Then
                newRow("AreaCode") = cleanPhone.Substring(0, 3)     ' e.g. "210"
                newRow("LocalNumber") = cleanPhone.Substring(3)     ' e.g. "2107788"
            Else
                newRow("AreaCode") = cleanPhone
                newRow("LocalNumber") = cleanPhone
            End If

            Dim stateVal As String = GetStr(row, "State")
            newRow("State") = If(String.IsNullOrWhiteSpace(stateVal), DBNull.Value, CObj(stateVal.Trim()))

            Dim addedDateVal As Object = GetSafeDate(row, "AddedDate")
            newRow("AddedDate") = If(addedDateVal Is DBNull.Value, CObj(defaultDate), addedDateVal)

            Dim addedBy As String = GetStr(row, "AddedByUser")
            newRow("AddedByUser") = If(String.IsNullOrWhiteSpace(addedBy), defaultUser, addedBy.Trim())

            Dim sourceVal As String = GetStr(row, "Source")
            newRow("Source") = If(String.IsNullOrWhiteSpace(sourceVal), DBNull.Value, CObj(sourceVal.Trim()))

            Dim notesVal As String = GetStr(row, "Notes")
            newRow("Notes") = If(String.IsNullOrWhiteSpace(notesVal), DBNull.Value, CObj(notesVal.Trim()))

            Dim phoneNumberVal As String = GetStr(row, "PhoneNumber")
            newRow("PhoneNumber") = If(String.IsNullOrWhiteSpace(phoneNumberVal), DBNull.Value, CObj(phoneNumberVal.Trim()))

            Dim statusVal As String = GetStr(row, "Status")
            Dim validStatus As String = If(String.IsNullOrWhiteSpace(statusVal), "A", statusVal.Trim().ToUpper())
            If validStatus <> "D" AndAlso validStatus <> "A" Then validStatus = "A"
            newRow("Status") = validStatus


            previewDt.Rows.Add(newRow)
        Next

        If previewDt.Rows.Count = 0 Then
            MessageBox.Show("No valid rows to preview (AreaCode and LocalNumber are required in every row).")
            BtnInsert.Enabled = False
            Return
        End If

        ' ── Check duplicates against DB (temp-table + JOIN — scales to any size) ──
        Cursor = Cursors.WaitCursor
        Try
            Dim existingKeys As New Dictionary(Of String, Boolean)(StringComparer.OrdinalIgnoreCase)
            Dim stateMap As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
            Dim statusMap As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)


            Dim builder As New System.Data.SqlClient.SqlConnectionStringBuilder()
            builder.DataSource = "172.190.120.3"
            builder.InitialCatalog = "DNC"
            builder.UserID = "sa"
            builder.Password = "Dell2014#"
            builder.TrustServerCertificate = True

            ' Build an in-memory table with just the keys we want to check
            Dim keysDt As New DataTable()
            keysDt.Columns.Add("PhoneNumber", GetType(String))
            For Each row As DataRow In previewDt.Rows
                keysDt.Rows.Add(row("PhoneNumber").ToString().Trim())
            Next

            Using conn As New System.Data.SqlClient.SqlConnection(builder.ConnectionString)
                conn.Open()

                ' 1) Create a temp table for the incoming keys
                Dim createTmp As String =
                    "CREATE TABLE #tmpCheck (PhoneNumber VARCHAR(20));"
                Using cmd As New System.Data.SqlClient.SqlCommand(createTmp, conn)
                    cmd.ExecuteNonQuery()
                End Using

                ' 2) Bulk-insert the incoming keys into the temp table (fast, no query-size limit)
                Using bc As New System.Data.SqlClient.SqlBulkCopy(conn)
                    bc.DestinationTableName = "#tmpCheck"
                    bc.ColumnMappings.Add("PhoneNumber", "PhoneNumber")
                    bc.BulkCopyTimeout = 120
                    bc.WriteToServer(keysDt)
                End Using

                ' 3) JOIN to find which keys already exist in the real table and get State from AreaCodeByState for new ones
                Dim checkSql As String =
                    "SELECT t.PhoneNumber, d.State AS DbState, d.Status AS DbStatus, s.StateCode AS NewState " &
                    "FROM #tmpCheck t " &
                    "LEFT JOIN DNC.dbo.DoNotCallNumbers d WITH (NOLOCK) " &
                    "  ON d.PhoneNumber = t.PhoneNumber " &
                    "LEFT JOIN DNC.dbo.AreaCodeByState s WITH (NOLOCK) " &
                    "  ON s.AreaCode = LEFT(t.PhoneNumber, 3);"

                Using cmd As New System.Data.SqlClient.SqlCommand(checkSql, conn)
                    cmd.CommandTimeout = 300
                    Using reader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim key As String = reader("PhoneNumber").ToString().Trim()
                            Dim isDup As Boolean = Not IsDBNull(reader("DbState"))
                            
                            If isDup Then
                                existingKeys(key) = True
                                stateMap(key) = reader("DbState").ToString().Trim()
                                statusMap(key) = If(IsDBNull(reader("DbStatus")), "", reader("DbStatus").ToString().Trim().ToUpper())
                            Else
                                If Not IsDBNull(reader("NewState")) Then
                                    stateMap(key) = reader("NewState").ToString().Trim()
                                End If
                            End If
                        End While
                    End Using
                End Using
            End Using

            ' Mark duplicates in preview table and apply State
            Dim dupCount As Integer = 0
            Dim skipCount As Integer = 0
            Dim updateCount As Integer = 0

            For Each row As DataRow In previewDt.Rows
                Dim key As String = row("PhoneNumber").ToString().Trim()
                
                If stateMap.ContainsKey(key) AndAlso Not String.IsNullOrWhiteSpace(stateMap(key)) Then
                    row("State") = stateMap(key)
                End If

                If existingKeys.ContainsKey(key) Then
                    dupCount += 1
                    Dim dbStatus As String = If(statusMap.ContainsKey(key), statusMap(key), "")
                    Dim newStatus As String = row("Status").ToString().Trim().ToUpper()

                    If dbStatus <> newStatus AndAlso (dbStatus = "A" OrElse dbStatus = "D") AndAlso (newStatus = "A" OrElse newStatus = "D") Then
                        row("Estado") = "🔄 A actualizar Status"
                        updateCount += 1
                    Else
                        row("Estado") = "☑ Sin cambios"
                        skipCount += 1
                    End If
                End If
            Next

            ' Show in DtGV2
            DtGV2.DataSource = previewDt

            ' Color duplicate rows yellow/orange
            AddHandler DtGV2.CellFormatting, AddressOf DtGV2_CellFormatting

            BtnInsert.Enabled = True

            Dim summary As String = $"Preview listo: {previewDt.Rows.Count} filas totales." & vbCrLf &
                                    $"   ✔ Nuevas a insertar: {previewDt.Rows.Count - dupCount}" & vbCrLf &
                                    $"   🔄 Existentes a actualizar Status: {updateCount}" & vbCrLf &
                                    $"   ☑ Existentes sin cambios (se omiten): {skipCount}"
            If skipped > 0 Then summary &= $"{vbCrLf}   ⛔ Saltadas (sin AreaCode/LocalNumber): {skipped}"
            MessageBox.Show(summary, "Preview", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Error checking duplicates: " & ex.Message)
            ' Still show the data even if DB check fails
            DtGV2.DataSource = previewDt
            BtnInsert.Enabled = True
        Finally
            Cursor = Cursors.Default
        End Try
    End Sub

    ' ─────────────────────────────────────────────────────────────────────────
    ' DtGV2 cell formatting – highlight duplicate rows in amber
    ' ─────────────────────────────────────────────────────────────────────────
    Private Sub DtGV2_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        If e.RowIndex < 0 Then Return
        Dim grid As DataGridView = CType(sender, DataGridView)
        If grid.Rows(e.RowIndex).DataBoundItem Is Nothing Then Return

        Dim rowView As DataRowView = CType(grid.Rows(e.RowIndex).DataBoundItem, DataRowView)
        If rowView("Estado").ToString() = "⚠ Ya existe" Then
            e.CellStyle.BackColor = System.Drawing.Color.FromArgb(255, 230, 153)   ' amber
            e.CellStyle.ForeColor = System.Drawing.Color.FromArgb(120, 80, 0)
            e.CellStyle.Font = New System.Drawing.Font(grid.Font, System.Drawing.FontStyle.Italic)
        End If
    End Sub

    ' ─────────────────────────────────────────────────────────────────────────
    ' BtnInsert – Insert ONLY "✔ Nuevo" rows, log in DoNotCallExportLog
    ' ─────────────────────────────────────────────────────────────────────────
    Private Sub BtnInsert_Click(sender As Object, e As EventArgs) Handles BtnInsert.Click
        If DtGV2.DataSource Is Nothing Then
            MessageBox.Show("Nothing to insert. Please click Preview first.")
            Return
        End If

        Dim previewDt As DataTable = CType(DtGV2.DataSource, DataTable)

        ' Filter only new rows
        Dim newRows As DataRow() = previewDt.Select("Estado = '✔ Nuevo'")
        Dim updateRows As DataRow() = previewDt.Select("Estado = '🔄 A actualizar Status'")

        If newRows.Length = 0 AndAlso updateRows.Length = 0 Then
            MessageBox.Show($"No hay filas nuevas ni para actualizar.",
                            "Sin datos accionar", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim confirm As DialogResult = MessageBox.Show(
            $"Se insertarán {newRows.Length} filas nuevas en DoNotCallNumbers." & vbCrLf &
            $"Se actualizarán {updateRows.Length} filas que ya existen." & vbCrLf &
            vbCrLf & "¿Continuar?",
            "Confirmar acción", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If confirm <> DialogResult.Yes Then Return

        ' Build bulk DataTable from new rows only
        Dim bulkDt As New DataTable()
        bulkDt.Columns.Add("AreaCode", GetType(String))
        bulkDt.Columns.Add("LocalNumber", GetType(String))
        bulkDt.Columns.Add("State", GetType(String))
        bulkDt.Columns.Add("AddedDate", GetType(DateTime))
        bulkDt.Columns.Add("AddedByUser", GetType(String))
        bulkDt.Columns.Add("Source", GetType(String))
        bulkDt.Columns.Add("Notes", GetType(String))
        bulkDt.Columns.Add("PhoneNumber", GetType(String))
        bulkDt.Columns.Add("Status", GetType(String))

        For Each row As DataRow In newRows
            Dim nr As DataRow = bulkDt.NewRow()
            nr("AreaCode") = row("AreaCode")
            nr("LocalNumber") = row("LocalNumber")
            nr("State") = row("State")
            nr("AddedDate") = row("AddedDate")
            nr("AddedByUser") = row("AddedByUser")
            nr("Source") = "National Do Not Call Registry" ' row("Source") esto hay que agregarlo manual
            nr("Notes") = row("Notes")
            nr("PhoneNumber") = row("PhoneNumber")
            nr("Status") = row("Status")
            bulkDt.Rows.Add(nr)
        Next

        ' Build update DataTable
        Dim updateDt As New DataTable()
        updateDt.Columns.Add("PhoneNumber", GetType(String))
        updateDt.Columns.Add("Status", GetType(String))
        updateDt.Columns.Add("AddedDate", GetType(DateTime))

        For Each row As DataRow In updateRows
            Dim nr As DataRow = updateDt.NewRow()
            nr("PhoneNumber") = row("PhoneNumber")
            nr("Status") = row("Status")
            nr("AddedDate") = row("AddedDate")
            updateDt.Rows.Add(nr)
        Next

        Dim builder As New System.Data.SqlClient.SqlConnectionStringBuilder()
        builder.DataSource = "172.190.120.3"
        builder.InitialCatalog = "DNC"
        builder.UserID = "sa"
        builder.Password = "Dell2014#"
        builder.TrustServerCertificate = True

        Cursor = Cursors.WaitCursor
        Try
            Using conn As New System.Data.SqlClient.SqlConnection(builder.ConnectionString)
                conn.Open()

                ' ── Bulk insert ───────────────────────────────────────────
                If bulkDt.Rows.Count > 0 Then
                    Using bulkCopy As New System.Data.SqlClient.SqlBulkCopy(conn)
                        bulkCopy.DestinationTableName = "DNC.dbo.DoNotCallNumbers"
                        bulkCopy.BatchSize = 5000
                        bulkCopy.BulkCopyTimeout = 600
                        bulkCopy.ColumnMappings.Add("AreaCode", "AreaCode")
                        bulkCopy.ColumnMappings.Add("LocalNumber", "LocalNumber")
                        bulkCopy.ColumnMappings.Add("State", "State")
                        bulkCopy.ColumnMappings.Add("AddedDate", "AddedDate")
                        bulkCopy.ColumnMappings.Add("AddedByUser", "AddedByUser")
                        bulkCopy.ColumnMappings.Add("Source", "Source")
                        bulkCopy.ColumnMappings.Add("Notes", "Notes")
                        bulkCopy.ColumnMappings.Add("PhoneNumber", "PhoneNumber")
                        bulkCopy.ColumnMappings.Add("Status", "Status")
                        bulkCopy.WriteToServer(bulkDt)
                    End Using
                End If

                ' ── Update Status ─────────────────────────────────────────
                If updateDt.Rows.Count > 0 Then
                    Dim createTmp As String =
                        "CREATE TABLE #tmpUpdateStatus (PhoneNumber VARCHAR(20), Status CHAR(1), AddedDate DATETIME);"
                    Using cmd As New System.Data.SqlClient.SqlCommand(createTmp, conn)
                        cmd.ExecuteNonQuery()
                    End Using

                    Using bc As New System.Data.SqlClient.SqlBulkCopy(conn)
                        bc.DestinationTableName = "#tmpUpdateStatus"
                        bc.ColumnMappings.Add("PhoneNumber", "PhoneNumber")
                        bc.ColumnMappings.Add("Status", "Status")
                        bc.ColumnMappings.Add("AddedDate", "AddedDate")
                        bc.BulkCopyTimeout = 120
                        bc.WriteToServer(updateDt)
                    End Using

                    Dim updateSql As String =
                        "UPDATE d " &
                        "SET d.Status = t.Status, d.AddedDate = t.AddedDate " &
                        "FROM DNC.dbo.DoNotCallNumbers d " &
                        "INNER JOIN #tmpUpdateStatus t " &
                        "  ON d.PhoneNumber = t.PhoneNumber " &
                        "WHERE d.Status <> t.Status;"

                    Using cmd As New System.Data.SqlClient.SqlCommand(updateSql, conn)
                        cmd.CommandTimeout = 300
                        cmd.ExecuteNonQuery()
                    End Using
                End If

                ' ── Log the export ────────────────────────────────────────
                Dim logSql As String =
                    "INSERT INTO DNC.dbo.DoNotCallExportLog " &
                    "( ExportDate, ExportedByUser, SourceFileName, RecordsExported, RecordsInserted, RecordsUpdated) " &
                    "SELECT @ExportDate, @ExportedByUser, @SourceFileName, @RecordsExported, @RecordsInserted, @RecordsUpdated " &
                    "FROM DNC.dbo.DoNotCallExportLog"

                Using cmdLog As New System.Data.SqlClient.SqlCommand(logSql, conn)
                    cmdLog.Parameters.AddWithValue("@ExportDate", DateTime.Now)
                    cmdLog.Parameters.AddWithValue("@ExportedByUser", Environment.UserName)
                    cmdLog.Parameters.AddWithValue("@SourceFileName",
                        If(String.IsNullOrWhiteSpace(Label1.Text) OrElse Label1.Text = "No file selected",
                           CObj(DBNull.Value), CObj(Label1.Text)))
                    cmdLog.Parameters.AddWithValue("@RecordsExported", previewDt.Rows.Count)
                    cmdLog.Parameters.AddWithValue("@RecordsInserted", bulkDt.Rows.Count)
                    cmdLog.Parameters.AddWithValue("@RecordsUpdated", updateDt.Rows.Count)
                    cmdLog.ExecuteNonQuery()
                End Using
            End Using

            ' Mark rows as done in the grid
            For Each row As DataRow In newRows
                row("Estado") = "✅ Insertado"
            Next
            For Each row As DataRow In updateRows
                row("Estado") = "✅ Actualizado"
            Next
            DtGV2.Refresh()

            MessageBox.Show(
                $"✔ Se procesaron {bulkDt.Rows.Count + updateDt.Rows.Count} filas en DoNotCallNumbers." & vbCrLf &
                $"Insertados: {bulkDt.Rows.Count}" & vbCrLf &
                $"Actualizados: {updateDt.Rows.Count}",
                "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)

            BtnInsert.Enabled = False

        Catch ex As Exception
            MessageBox.Show("Error processing data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Cursor = Cursors.Default
        End Try
    End Sub

    ' ─────────────────────────────────────────────────────────────────────────
    ' Helpers
    ' ─────────────────────────────────────────────────────────────────────────
    Private Sub SetDoubleBuffered(control As Control)
        Try
            Dim pi As System.Reflection.PropertyInfo =
                GetType(Control).GetProperty("DoubleBuffered",
                    System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.NonPublic)
            pi.SetValue(control, True, Nothing)
        Catch
        End Try
    End Sub

    Private Function HasColumn(dt As DataTable, colName As String) As Boolean
        For Each col As DataColumn In dt.Columns
            If String.Equals(col.ColumnName, colName, StringComparison.OrdinalIgnoreCase) Then Return True
        Next
        Return False
    End Function

    Private Function GetStr(row As DataRow, colName As String) As String
        For Each col As DataColumn In row.Table.Columns
            If String.Equals(col.ColumnName, colName, StringComparison.OrdinalIgnoreCase) Then
                Dim v = row(col.ColumnName)
                If IsDBNull(v) OrElse v Is Nothing Then Return String.Empty
                Return v.ToString()
            End If
        Next
        Return String.Empty
    End Function

    Private Function GetSafeDate(row As DataRow, colName As String) As Object
        For Each col As DataColumn In row.Table.Columns
            If String.Equals(col.ColumnName, colName, StringComparison.OrdinalIgnoreCase) Then
                Dim rawVal = row(col.ColumnName)
                If IsDBNull(rawVal) OrElse rawVal Is Nothing OrElse String.IsNullOrWhiteSpace(rawVal.ToString()) Then
                    Return DBNull.Value
                End If
                If TypeOf rawVal Is Date Then Return rawVal
                Dim parsedDate As Date
                If Date.TryParse(rawVal.ToString(), parsedDate) Then Return parsedDate
                Return DBNull.Value
            End If
        Next
        Return DBNull.Value
    End Function

End Class