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
        If DtGV1.DataSource Is Nothing Then
            MessageBox.Show("Please upload an  file first.")
            Return
        End If

        Dim dtExcel As DataTable = CType(DtGV1.DataSource, DataTable)

        For Each col As String In {"AreaCode", "PhoneNumber"}
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
        previewDt.Columns.Add("PhoneNumber", GetType(String))
        previewDt.Columns.Add("State", GetType(String))
        previewDt.Columns.Add("AddedDate", GetType(DateTime))
        previewDt.Columns.Add("AddedByUser", GetType(String))
        previewDt.Columns.Add("Source", GetType(String))
        previewDt.Columns.Add("Notes", GetType(String))

        For Each row As DataRow In dtExcel.Rows
            Dim areaCode As String = GetStr(row, "AreaCode")
            Dim phoneNumber As String = GetStr(row, "PhoneNumber")

            If String.IsNullOrWhiteSpace(areaCode) OrElse String.IsNullOrWhiteSpace(phoneNumber) Then
                skipped += 1
                Continue For
            End If

            Dim newRow As DataRow = previewDt.NewRow()
            newRow("Estado") = "✔ Nuevo"           ' default — will update after DB check
            newRow("AreaCode") = areaCode.Trim()
            newRow("PhoneNumber") = phoneNumber.Trim()

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
            MessageBox.Show("No valid rows to preview (AreaCode and PhoneNumber are required in every row).")
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
            keysDt.Columns.Add("PhoneNumber", GetType(String))
            For Each row As DataRow In previewDt.Rows
                keysDt.Rows.Add(row("AreaCode").ToString().Trim(),
                                row("PhoneNumber").ToString().Trim())
            Next

            Using conn As New System.Data.SqlClient.SqlConnection(builder.ConnectionString)
                conn.Open()

                ' 1) Create a temp table for the incoming keys
                Dim createTmp As String =
                    "CREATE TABLE #tmpCheck (AreaCode VARCHAR(10), PhoneNumber VARCHAR(20));"
                Using cmd As New System.Data.SqlClient.SqlCommand(createTmp, conn)
                    cmd.ExecuteNonQuery()
                End Using

                ' 2) Bulk-insert the incoming keys into the temp table (fast, no query-size limit)
                Using bc As New System.Data.SqlClient.SqlBulkCopy(conn)
                    bc.DestinationTableName = "#tmpCheck"
                    bc.ColumnMappings.Add("AreaCode", "AreaCode")
                    bc.ColumnMappings.Add("PhoneNumber", "PhoneNumber")
                    bc.BulkCopyTimeout = 120
                    bc.WriteToServer(keysDt)
                End Using

                ' 3) JOIN to find which keys already exist in the real table
                Dim checkSql As String =
                    "SELECT t.AreaCode, t.PhoneNumber " &
                    "FROM #tmpCheck t " &
                    "INNER JOIN DNC.dbo.DoNotCallNumbers d " &
                    "  ON d.AreaCode = t.AreaCode AND d.PhoneNumber = t.PhoneNumber;"

                Using cmd As New System.Data.SqlClient.SqlCommand(checkSql, conn)
                    cmd.CommandTimeout = 300
                    Using reader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim key As String = reader("AreaCode").ToString().Trim() & "|" &
                                                reader("PhoneNumber").ToString().Trim()
                            existingKeys.Add(key)
                        End While
                    End Using
                End Using
            End Using

            ' Mark duplicates in preview table
            Dim dupCount As Integer = 0
            For Each row As DataRow In previewDt.Rows
                Dim key As String = row("AreaCode").ToString().Trim() & "|" &
                                    row("PhoneNumber").ToString().Trim()
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
            If skipped > 0 Then summary &= $"{vbCrLf}   ⛔ Saltadas (sin AreaCode/PhoneNumber): {skipped}"
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
        Dim dupRows As Integer = previewDt.Select("Estado = '⚠ Ya existe'").Length

        If newRows.Length = 0 Then
            MessageBox.Show($"No hay filas nuevas para insertar." & vbCrLf &
                            $"Todas las {dupRows} filas ya existen en la BD.",
                            "Sin datos nuevos", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim confirm As DialogResult = MessageBox.Show(
            $"Se insertarán {newRows.Length} filas nuevas en DNC.dbo.DoNotCallNumbers." & vbCrLf &
            If(dupRows > 0, $"Se omitirán {dupRows} filas que ya existen." & vbCrLf, "") &
            vbCrLf & "¿Continuar?",
            "Confirmar inserción", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If confirm <> DialogResult.Yes Then Return

        ' Build bulk DataTable from new rows only
        Dim bulkDt As New DataTable()
        bulkDt.Columns.Add("AreaCode", GetType(String))
        bulkDt.Columns.Add("PhoneNumber", GetType(String))
        bulkDt.Columns.Add("State", GetType(String))
        bulkDt.Columns.Add("AddedDate", GetType(DateTime))
        bulkDt.Columns.Add("AddedByUser", GetType(String))
        bulkDt.Columns.Add("Source", GetType(String))
        bulkDt.Columns.Add("Notes", GetType(String))

        For Each row As DataRow In newRows
            Dim nr As DataRow = bulkDt.NewRow()
            nr("AreaCode") = row("AreaCode")
            nr("PhoneNumber") = row("PhoneNumber")
            nr("State") = "TX" 'row("State") esto hay que agregarlo manual
            nr("AddedDate") = row("AddedDate")
            nr("AddedByUser") = row("AddedByUser")
            nr("Source") = "National Do Not Call Registry" ' row("Source") esto hay que agregarlo manual
            nr("Notes") = row("Notes")
            bulkDt.Rows.Add(nr)
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
                Using bulkCopy As New System.Data.SqlClient.SqlBulkCopy(conn)
                    bulkCopy.DestinationTableName = "DNC.dbo.DoNotCallNumbers"
                    bulkCopy.BatchSize = 5000
                    bulkCopy.BulkCopyTimeout = 600
                    bulkCopy.ColumnMappings.Add("AreaCode", "AreaCode")
                    bulkCopy.ColumnMappings.Add("PhoneNumber", "PhoneNumber")
                    bulkCopy.ColumnMappings.Add("State", "State")
                    bulkCopy.ColumnMappings.Add("AddedDate", "AddedDate")
                    bulkCopy.ColumnMappings.Add("AddedByUser", "AddedByUser")
                    bulkCopy.ColumnMappings.Add("Source", "Source")
                    bulkCopy.ColumnMappings.Add("Notes", "Notes")
                    bulkCopy.WriteToServer(bulkDt)
                End Using

                ' ── Log the export ────────────────────────────────────────
                Dim logSql As String =
                    "INSERT INTO DNC.dbo.DoNotCallExportLog " &
                    "(ExportDate, ExportedByUser, SourceFileName, RecordsExported) " &
                    "VALUES (@ExportDate, @ExportedByUser, @SourceFileName, @RecordsExported)"

                Using cmdLog As New System.Data.SqlClient.SqlCommand(logSql, conn)
                    cmdLog.Parameters.AddWithValue("@ExportDate", DateTime.Now)
                    cmdLog.Parameters.AddWithValue("@ExportedByUser", Environment.UserName)
                    cmdLog.Parameters.AddWithValue("@SourceFileName",
                        If(String.IsNullOrWhiteSpace(Label1.Text) OrElse Label1.Text = "No file selected",
                           CObj(DBNull.Value), CObj(Label1.Text)))
                    cmdLog.Parameters.AddWithValue("@RecordsExported", bulkDt.Rows.Count)
                    cmdLog.ExecuteNonQuery()
                End Using
            End Using

            ' Mark inserted rows as done in the grid
            For Each row As DataRow In newRows
                row("Estado") = "✅ Insertado"
            Next
            DtGV2.Refresh()

            MessageBox.Show(
                $"✔ Se insertaron {bulkDt.Rows.Count} filas en dbo.DoNotCallNumbers." & vbCrLf &
                If(dupRows > 0, $"Se omitieron {dupRows} duplicados.", ""),
                "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)

            BtnInsert.Enabled = False

        Catch ex As Exception
            MessageBox.Show("Error inserting data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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