Public Class FormMain
    Private Sub btnExportExcel_Click(sender As Object, e As EventArgs) Handles btnExportExcel.Click
        Dim f As New Form1()
        f.Show()
    End Sub

    Private Sub btnExportTxt_Click(sender As Object, e As EventArgs) Handles btnExportTxt.Click
        Dim f As New FormTxt()
        f.Show()
    End Sub
End Class
