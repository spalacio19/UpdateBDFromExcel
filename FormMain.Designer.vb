<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormMain
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        btnExportExcel = New Button()
        btnExportTxt = New Button()
        SuspendLayout()
        ' 
        ' btnExportExcel
        ' 
        btnExportExcel.Location = New Point(100, 50)
        btnExportExcel.Name = "btnExportExcel"
        btnExportExcel.Size = New Size(200, 50)
        btnExportExcel.TabIndex = 0
        btnExportExcel.Text = "Export Excel to DB"
        btnExportExcel.UseVisualStyleBackColor = True
        ' 
        ' btnExportTxt
        ' 
        btnExportTxt.Location = New Point(100, 120)
        btnExportTxt.Name = "btnExportTxt"
        btnExportTxt.Size = New Size(200, 50)
        btnExportTxt.TabIndex = 1
        btnExportTxt.Text = "Import Do Not Call List"
        btnExportTxt.UseVisualStyleBackColor = True
        ' 
        ' FormMain
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(400, 250)
        Controls.Add(btnExportTxt)
        Controls.Add(btnExportExcel)
        Name = "FormMain"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Main Menu"
        ResumeLayout(False)
    End Sub

    Friend WithEvents btnExportExcel As Button
    Friend WithEvents btnExportTxt As Button

End Class
