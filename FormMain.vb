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
                                                btnExportTxt.BackColor = System.Drawing.Color.FromArgb(4, 120, 84)
                                            End Sub
        AddHandler btnExportTxt.MouseLeave, Sub(s, ev)
                                                btnExportTxt.BackColor = System.Drawing.Color.FromArgb(5, 150, 105)
                                            End Sub
    End Sub

    Private Sub btnExportExcel_Click(sender As Object, e As EventArgs) Handles btnExportExcel.Click
        Dim f As New Form2()
        f.Show()
    End Sub

    Private Sub btnExportTxt_Click(sender As Object, e As EventArgs) Handles btnExportTxt.Click
        Dim f As New FormTxt()
        f.Show()
    End Sub

End Class
