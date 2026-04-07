Public Class FormMain

    Private Sub FormMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Hover effects for Excel button (blue)
        AddHandler btnExportExcel.MouseEnter, Sub(s, ev)
                                                  btnExportExcel.BackColor = System.Drawing.Color.FromArgb(29, 78, 187)
                                              End Sub
        AddHandler btnExportExcel.MouseLeave, Sub(s, ev)
                                                  btnExportExcel.BackColor = System.Drawing.Color.FromArgb(37, 99, 235)
                                              End Sub

        ' Hover effects for TXT button (green)
        AddHandler btnExportTxt.MouseEnter, Sub(s, ev)
                                                btnExportTxt.BackColor = System.Drawing.Color.FromArgb(29, 78, 187)
                                            End Sub
        AddHandler btnExportTxt.MouseLeave, Sub(s, ev)
                                                btnExportTxt.BackColor = System.Drawing.Color.FromArgb(37, 99, 235)
                                            End Sub

        ' Hover effects for Policy Snapshot button (blue)
        AddHandler btnPolicySnapshot.MouseEnter, Sub(s, ev)
                                                     btnPolicySnapshot.BackColor = System.Drawing.Color.FromArgb(29, 78, 187)
                                                 End Sub
        AddHandler btnPolicySnapshot.MouseLeave, Sub(s, ev)
                                                     btnPolicySnapshot.BackColor = System.Drawing.Color.FromArgb(37, 99, 235)
                                                 End Sub

        ' Hover effects for Franco Net button (green)
        AddHandler btnFrancoNet.MouseEnter, Sub(s, ev)
                                                btnFrancoNet.BackColor = System.Drawing.Color.FromArgb(16, 130, 58)
                                            End Sub
        AddHandler btnFrancoNet.MouseLeave, Sub(s, ev)
                                                btnFrancoNet.BackColor = System.Drawing.Color.FromArgb(22, 163, 74)
                                            End Sub

        ' Hover effects for Dashboard button (purple)
        AddHandler btnDashboard.MouseEnter, Sub(s, ev)
                                                btnDashboard.BackColor = System.Drawing.Color.FromArgb(107, 33, 168)
                                            End Sub
        AddHandler btnDashboard.MouseLeave, Sub(s, ev)
                                                btnDashboard.BackColor = System.Drawing.Color.FromArgb(88, 28, 135)
                                            End Sub

        ' Injecting BtnMgaDaily Kpis Code Dynamically
        Dim BtnMgaKpis As New FontAwesome.Sharp.IconButton()
        BtnMgaKpis.BackColor = Color.FromArgb(180, 80, 0) ' Orange thematic
        BtnMgaKpis.Cursor = Cursors.Hand
        BtnMgaKpis.FlatAppearance.BorderSize = 0
        BtnMgaKpis.FlatAppearance.MouseOverBackColor = Color.FromArgb(210, 100, 0)
        BtnMgaKpis.FlatStyle = FlatStyle.Flat
        BtnMgaKpis.Font = New Font("Segoe UI", 13.0!, FontStyle.Bold)
        BtnMgaKpis.ForeColor = Color.White
        BtnMgaKpis.IconChar = FontAwesome.Sharp.IconChar.Table
        BtnMgaKpis.IconColor = Color.White
        BtnMgaKpis.IconSize = 32
        BtnMgaKpis.ImageAlign = ContentAlignment.MiddleLeft
        BtnMgaKpis.TextImageRelation = TextImageRelation.ImageBeforeText
        BtnMgaKpis.Location = New Point(30, 545)
        BtnMgaKpis.Size = New Size(440, 65)
        BtnMgaKpis.Padding = New Padding(15, 0, 0, 0)
        BtnMgaKpis.Text = "  MGA Daily KPIs (Pivot Matrix)"
        BtnMgaKpis.TextAlign = ContentAlignment.MiddleLeft
        
        Dim LblMgaDesc As New Label()
        LblMgaDesc.AutoSize = True
        LblMgaDesc.Font = New Font("Segoe UI", 8.5!)
        LblMgaDesc.ForeColor = Color.FromArgb(160, 180, 220)
        LblMgaDesc.Location = New Point(45, 616)
        LblMgaDesc.Text = "Reporte matricial avanzado de primas y cancelaciones según plazo de póliza"
        
        AddHandler BtnMgaKpis.Click, Sub(s, ev)
                                        Dim f As New FormMgaDaily()
                                        f.Show()
                                     End Sub

        Me.Height = Me.Height + 110 ' Expand the form vertically to fit the new button
        PanelContent.Controls.Add(BtnMgaKpis)
        PanelContent.Controls.Add(LblMgaDesc)
    End Sub

    Private Sub btnExportTxt_Click(sender As Object, e As EventArgs) Handles btnExportTxt.Click
        Dim f As New FormTxt()
        f.Show()
    End Sub

    Private Sub btnPolicySnapshot_Click(sender As Object, e As EventArgs) Handles btnPolicySnapshot.Click
        Dim f As New Form1()
        f.Show()
    End Sub

    Private Sub btnExportExcel_Click_1(sender As Object, e As EventArgs) Handles btnExportExcel.Click
        Dim f As New Form2()
        f.Show()
    End Sub

    Private Sub btnFrancoNet_Click(sender As Object, e As EventArgs) Handles btnFrancoNet.Click
        Dim f As New FormFrancoNet()
        f.Show()
    End Sub

    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnDashboard.Click
        Dim f As New FormDashboard()
        f.Show()
    End Sub

End Class
