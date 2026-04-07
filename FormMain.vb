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


End Class
