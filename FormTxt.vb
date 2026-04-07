Imports System.IO

Public Class FormTxt

    Private pbLoading As ProgressBar

    Private Sub FormTxt_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        pbLoading = New ProgressBar()
        pbLoading.Style = ProgressBarStyle.Marquee
        pbLoading.MarqueeAnimationSpeed = 30
        pbLoading.Size = New Size(200, 15)
        pbLoading.Location = New Point(PanelFooter.Width \ 2 - 100, 10)
        pbLoading.Anchor = AnchorStyles.Bottom Or AnchorStyles.Top
        pbLoading.Visible = False
        PanelFooter.Controls.Add(pbLoading)

        ' Hover effects for Upload button (blue)
        AddHandler btnUpload.MouseEnter, Sub(s, ev) btnUpload.BackColor = Color.FromArgb(29, 78, 187)
        AddHandler btnUpload.MouseLeave, Sub(s, ev) btnUpload.BackColor = Color.FromArgb(37, 99, 235)

        ' Hover effects for Preview button (blue)
        AddHandler BtnPreview.MouseEnter, Sub(s, ev) BtnPreview.BackColor = Color.FromArgb(29, 78, 187)
        AddHandler BtnPreview.MouseLeave, Sub(s, ev) BtnPreview.BackColor = Color.FromArgb(37, 99, 235)

        ' Hover effects for Insert button (green)
        AddHandler BtnInsert.MouseEnter, Sub(s, ev) BtnInsert.BackColor = Color.FromArgb(16, 130, 58)
        AddHandler BtnInsert.MouseLeave, Sub(s, ev) BtnInsert.BackColor = Color.FromArgb(22, 163, 74)

        ' Apply modern styling to DataGridViews
        StyleDataGridView(DtGV1)
        StyleDataGridView(DtGV2)
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

    Private Sub UpdateStatus(message As String)
        LblStatus.Text = message
        Application.DoEvents()
    End Sub

    Private Sub UpdateRowCount()
        Dim c1 As Integer = If(DtGV1.DataSource IsNot Nothing, CType(DtGV1.DataSource, DataTable).Rows.Count, 0)
        Dim c2 As Integer = If(DtGV2.DataSource IsNot Nothing, CType(DtGV2.DataSource, DataTable).Rows.Count, 0)
        LblRowCount.Text = $"Raw: {c1:N0} | Preview: {c2:N0}"
    End Sub

    ' ─────────────────────────────────────────────────────────────────────────
    ' btnUpload – Upload TXT/CSV → show raw data in DtGV1 (Async)
    ' ─────────────────────────────────────────────────────────────────────────
    Private Async Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click
        Using ofd As New OpenFileDialog()
            ofd.Filter = "Text/CSV files|*.txt;*.csv|All files|*.*"
            If ofd.ShowDialog() <> DialogResult.OK Then Return

            Dim file = ofd.FileName
            LblFilePath.Text = Path.GetFileName(file)

            DtGV1.SuspendLayout()
            DtGV2.DataSource = Nothing
            BtnPreview.Enabled = False
            BtnInsert.Enabled = False
            Cursor = Cursors.WaitCursor
            pbLoading.Visible = True
            btnUpload.Enabled = False
            UpdateStatus("Loading TXT/CSV file...")

            Try
                Dim dt As DataTable = Await Task.Run(Function() As DataTable
                                                         Dim resultTable As New DataTable()
                                                         Dim isFirstLine As Boolean = True

                                                         Using sr As New IO.StreamReader(file, System.Text.Encoding.UTF8)
                                                             While Not sr.EndOfStream
                                                                 Dim line As String = sr.ReadLine()
                                                                 If String.IsNullOrWhiteSpace(line) Then Continue While

                                                                 Dim fields() As String = SplitCsvLine(line)

                                                                 If isFirstLine Then
                                                                     For Each header As String In fields
                                                                         resultTable.Columns.Add(header.Trim().Trim(""""c))
                                                                     Next
                                                                     isFirstLine = False
                                                                 Else
                                                                     Dim row As DataRow = resultTable.NewRow()
                                                                     For i As Integer = 0 To Math.Min(fields.Length, resultTable.Columns.Count) - 1
                                                                         row(i) = fields(i).Trim().Trim(""""c)
                                                                     Next
                                                                     resultTable.Rows.Add(row)
                                                                 End If
                                                             End While
                                                         End Using
                                                         Return resultTable
                                                     End Function)

                ' Use default or DisplayedCells width to match DtGV2
                DtGV1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
                DtGV1.DataSource = dt
                SetDoubleBuffered(DtGV1)
                BtnPreview.Enabled = True
                LblMapped.Text = $"TXT Data (raw) — {dt.Rows.Count:N0} rows:"
                UpdateStatus($"✅ File loaded: {dt.Rows.Count:N0} rows")

            Catch ex As Exception
                MessageBox.Show("Failed to read TXT file: " & ex.Message)
                UpdateStatus("❌ Error loading file")
            Finally
                DtGV1.ResumeLayout()
                UpdateRowCount()
                pbLoading.Visible = False
                btnUpload.Enabled = True
                Cursor = Cursors.Default
            End Try
        End Using
    End Sub

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
    ' BtnPreview – Map columns, check duplicates in DB, show in DtGV2 (Async)
    ' ─────────────────────────────────────────────────────────────────────────
    Private Async Sub BtnPreview_Click(sender As Object, e As EventArgs) Handles BtnPreview.Click
        UpdateStatus("🔍 Generating preview and checking duplicates...")
        BtnPreview.Enabled = False
        pbLoading.Visible = True
        Cursor = Cursors.WaitCursor
        Try
            Await UpdatePreviewDataAsync()
        Finally
            BtnPreview.Enabled = True
            pbLoading.Visible = False
            Cursor = Cursors.Default
            UpdateRowCount()
        End Try
    End Sub

    Private Async Function UpdatePreviewDataAsync() As Task
        If DtGV1.DataSource Is Nothing Then
            MessageBox.Show("Please upload a file first.")
            Return
        End If

        Dim dtExcel As DataTable = CType(DtGV1.DataSource, DataTable)

        For Each col As String In {"PhoneNumber", "AddedDate", "Status"}
            If Not HasColumn(dtExcel, col) Then
                MessageBox.Show($"Required column '{col}' not found in the file." & vbCrLf &
                                "Make sure the header names match exactly.")
                Return
            End If
        Next

        Dim defaultUser As String = Environment.UserName
        Dim defaultDate As DateTime = DateTime.Now
        Dim skipped As Integer = 0

        Dim previewDt As New DataTable()
        previewDt.Columns.Add("Estado", GetType(String))
        previewDt.Columns.Add("AreaCode", GetType(String))
        previewDt.Columns.Add("LocalNumber", GetType(String))
        previewDt.Columns.Add("State", GetType(String))
        previewDt.Columns.Add("AddedDate", GetType(DateTime))
        previewDt.Columns.Add("AddedByUser", GetType(String))
        previewDt.Columns.Add("Source", GetType(String))
        previewDt.Columns.Add("Notes", GetType(String))
        previewDt.Columns.Add("PhoneNumber", GetType(String))
        previewDt.Columns.Add("Status", GetType(String))

        Dim dupCount As Integer = 0
        Dim skipCount As Integer = 0
        Dim updateCount As Integer = 0

        Try
            Dim resultObj = Await Task.Run(Function()
                                               For Each row As DataRow In dtExcel.Rows
                                                   Dim PhoneNumber As String = GetStr(row, "PhoneNumber")

                                                   If String.IsNullOrWhiteSpace(PhoneNumber) Then
                                                       skipped += 1
                                                       Continue For
                                                   End If

                                                   Dim newRow As DataRow = previewDt.NewRow()
                                                   newRow("Estado") = "✔ Nuevo"

                                                   Dim cleanPhone As String = PhoneNumber.Trim()
                                                   If cleanPhone.Length >= 10 Then
                                                       newRow("AreaCode") = cleanPhone.Substring(0, 3)
                                                       newRow("LocalNumber") = cleanPhone.Substring(3)
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
                                                   Return New With {.success = False, .msg = "No valid rows to preview."}
                                               End If

                                               Dim existingKeys As New Dictionary(Of String, Boolean)(StringComparer.OrdinalIgnoreCase)
                                               Dim stateMap As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
                                               Dim statusMap As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)

                                               Dim builder As New SqlConnectionStringBuilder()
                                               builder.DataSource = "172.190.120.3"
                                               builder.InitialCatalog = "DNC"
                                               builder.UserID = "sa"
                                               builder.Password = "Dell2014#"
                                               builder.TrustServerCertificate = True

                                               Dim keysDt As New DataTable()
                                               keysDt.Columns.Add("PhoneNumber", GetType(String))
                                               For Each row As DataRow In previewDt.Rows
                                                   keysDt.Rows.Add(row("PhoneNumber").ToString().Trim())
                                               Next

                                               Using conn As New SqlConnection(builder.ConnectionString)
                                                   conn.Open()

                                                   Dim createTmp As String = "CREATE TABLE #tmpCheck (PhoneNumber VARCHAR(20));"
                                                   Using cmd As New SqlCommand(createTmp, conn)
                                                       cmd.ExecuteNonQuery()
                                                   End Using

                                                   Using bc As New System.Data.SqlClient.SqlBulkCopy(conn)
                                                       bc.DestinationTableName = "#tmpCheck"
                                                       bc.ColumnMappings.Add("PhoneNumber", "PhoneNumber")
                                                       bc.BulkCopyTimeout = 120
                                                       bc.WriteToServer(keysDt)
                                                   End Using

                                                   Dim checkSql As String =
                                                       "SELECT t.PhoneNumber, d.State AS DbState, d.Status AS DbStatus, s.StateCode AS NewState " &
                                                       "FROM #tmpCheck t " &
                                                       "LEFT JOIN DNC.dbo.DoNotCallNumbers d WITH (NOLOCK) ON d.PhoneNumber = t.PhoneNumber " &
                                                       "LEFT JOIN DNC.dbo.AreaCodeByState s WITH (NOLOCK) ON s.AreaCode = LEFT(t.PhoneNumber, 3);"

                                                   Using cmd As New SqlCommand(checkSql, conn)
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

                                               Return New With {.success = True, .msg = ""}
                                           End Function)

            If Not resultObj.success Then
                MessageBox.Show(resultObj.msg)
                BtnInsert.Enabled = False
                UpdateStatus("⚠️ No valid rows")
                Return
            End If

            DtGV2.DataSource = previewDt
            AddHandler DtGV2.CellFormatting, AddressOf DtGV2_CellFormatting
            BtnInsert.Enabled = True

            Dim summary As String = $"Preview listo: {previewDt.Rows.Count} filas totales." & vbCrLf &
                                    $"   ✔ Nuevas a insertar: {previewDt.Rows.Count - dupCount}" & vbCrLf &
                                    $"   🔄 Existentes a actualizar Status: {updateCount}" & vbCrLf &
                                    $"   ☑ Existentes sin cambios (se omiten): {skipCount}"
            If skipped > 0 Then summary &= $"{vbCrLf}   ⛔ Saltadas (sin AreaCode/LocalNumber): {skipped}"
            MessageBox.Show(summary, "Preview", MessageBoxButtons.OK, MessageBoxIcon.Information)

            UpdateStatus($"✅ Preview ready: {previewDt.Rows.Count - dupCount} new, {updateCount} to update")

        Catch ex As Exception
            MessageBox.Show("Error checking duplicates: " & ex.Message)
            DtGV2.DataSource = previewDt
            BtnInsert.Enabled = True
            UpdateStatus("⚠️ Preview generated with errors")
        End Try
    End Function

    Private Sub DtGV2_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        If e.RowIndex < 0 Then Return
        Dim grid As DataGridView = CType(sender, DataGridView)
        If grid.Rows(e.RowIndex).DataBoundItem Is Nothing Then Return

        Dim rowView As DataRowView = CType(grid.Rows(e.RowIndex).DataBoundItem, DataRowView)
        Dim status As String = rowView("Estado").ToString()

        If status.Contains("Sin cambios") Then
            e.CellStyle.BackColor = Color.FromArgb(22, 35, 65)
            e.CellStyle.ForeColor = Color.DimGray
        ElseIf status.Contains("A actualizar") Then
            e.CellStyle.BackColor = Color.FromArgb(50, 40, 20)
            e.CellStyle.ForeColor = Color.FromArgb(255, 200, 80)
        ElseIf status.Contains("Nuevo") Then
            e.CellStyle.ForeColor = Color.FromArgb(100, 255, 100)
        End If
    End Sub

    ' ─────────────────────────────────────────────────────────────────────────
    ' BtnInsert – Insert ONLY "✔ Nuevo" rows, log in DoNotCallExportLog (Async)
    ' ─────────────────────────────────────────────────────────────────────────
    Private Async Sub BtnInsert_Click(sender As Object, e As EventArgs) Handles BtnInsert.Click
        If DtGV2.DataSource Is Nothing Then
            MessageBox.Show("Nothing to insert. Please click Preview first.")
            Return
        End If

        Dim previewDt As DataTable = CType(DtGV2.DataSource, DataTable)

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

        pbLoading.Visible = True
        BtnInsert.Enabled = False
        Cursor = Cursors.WaitCursor
        UpdateStatus("💾 Updating database...")

        Try
            Dim lblPath As String = If(String.IsNullOrWhiteSpace(LblFilePath.Text) OrElse LblFilePath.Text = "No file selected", "", LblFilePath.Text)
            Dim usrName As String = Environment.UserName

            Dim result = Await Task.Run(Function() As Tuple(Of Integer, Integer)
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
                                                nr("Source") = "National Do Not Call Registry"
                                                nr("Notes") = row("Notes")
                                                nr("PhoneNumber") = row("PhoneNumber")
                                                nr("Status") = row("Status")
                                                bulkDt.Rows.Add(nr)
                                            Next

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

                                            Dim builder As New SqlConnectionStringBuilder()
                                            builder.DataSource = "172.190.120.3"
                                            builder.InitialCatalog = "DNC"
                                            builder.UserID = "sa"
                                            builder.Password = "Dell2014#"
                                            builder.TrustServerCertificate = True

                                            Using conn As New SqlConnection(builder.ConnectionString)
                                                conn.Open()

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

                                                If updateDt.Rows.Count > 0 Then
                                                    Dim createTmp As String = "CREATE TABLE #tmpUpdateStatus (PhoneNumber VARCHAR(20), Status CHAR(1), AddedDate DATETIME);"
                                                    Using cmd As New SqlCommand(createTmp, conn)
                                                        cmd.ExecuteNonQuery()
                                                    End Using

                                                    Using bc As New SqlBulkCopy(conn)
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
                                                        "INNER JOIN #tmpUpdateStatus t ON d.PhoneNumber = t.PhoneNumber " &
                                                        "WHERE d.Status <> t.Status;"

                                                    Using cmd As New SqlCommand(updateSql, conn)
                                                        cmd.CommandTimeout = 300
                                                        cmd.ExecuteNonQuery()
                                                    End Using
                                                End If

                                                Dim logSql As String =
                                                    "INSERT INTO DNC.dbo.DoNotCallExportLog " &
                                                    "(ExportDate, ExportedByUser, SourceFileName, RecordsExported, RecordsInserted, RecordsUpdated) " &
                                                    "VALUES (@ExportDate, @ExportedByUser, @SourceFileName, @RecordsExported, @RecordsInserted, @RecordsUpdated);"

                                                Using cmdLog As New SqlCommand(logSql, conn)
                                                    cmdLog.Parameters.AddWithValue("@ExportDate", DateTime.Now)
                                                    cmdLog.Parameters.AddWithValue("@ExportedByUser", usrName)
                                                    cmdLog.Parameters.AddWithValue("@SourceFileName",
                                                        If(String.IsNullOrWhiteSpace(lblPath), CObj(DBNull.Value), CObj(lblPath)))
                                                    cmdLog.Parameters.AddWithValue("@RecordsExported", previewDt.Rows.Count)
                                                    cmdLog.Parameters.AddWithValue("@RecordsInserted", bulkDt.Rows.Count)
                                                    cmdLog.Parameters.AddWithValue("@RecordsUpdated", updateDt.Rows.Count)
                                                    cmdLog.ExecuteNonQuery()
                                                End Using
                                            End Using

                                            Return New Tuple(Of Integer, Integer)(bulkDt.Rows.Count, updateDt.Rows.Count)
                                        End Function)

            For Each row As DataRow In newRows
                row("Estado") = "✅ Insertado"
            Next
            For Each row As DataRow In updateRows
                row("Estado") = "✅ Actualizado"
            Next
            DtGV2.Refresh()

            MessageBox.Show(
                $"✔ Se procesaron {result.Item1 + result.Item2} filas en DoNotCallNumbers." & vbCrLf &
                $"Insertados: {result.Item1}" & vbCrLf &
                $"Actualizados: {result.Item2}",
                "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)

            UpdateStatus($"✅ Successfully processed {result.Item1 + result.Item2} rows in DB")

        Catch ex As Exception
            BtnInsert.Enabled = True
            UpdateStatus("❌ Database update failed")
            MessageBox.Show("Error processing data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            pbLoading.Visible = False
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