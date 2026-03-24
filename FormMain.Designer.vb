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
        PanelHeader = New Panel()
        LblTitle = New Label()
        LblSubtitle = New Label()
        PanelContent = New Panel()
        btnExportExcel = New Button()
        LblExcelDesc = New Label()
        btnExportTxt = New Button()
        LblTxtDesc = New Label()
        PanelFooter = New Panel()
        LblFooter = New Label()
        PanelHeader.SuspendLayout()
        PanelContent.SuspendLayout()
        PanelFooter.SuspendLayout()
        SuspendLayout()
        '
        ' PanelHeader
        '
        PanelHeader.BackColor = System.Drawing.Color.FromArgb(18, 30, 58)
        PanelHeader.Controls.Add(LblTitle)
        PanelHeader.Controls.Add(LblSubtitle)
        PanelHeader.Dock = DockStyle.Top
        PanelHeader.Height = 90
        PanelHeader.Name = "PanelHeader"
        PanelHeader.Padding = New Padding(20, 15, 20, 10)
        '
        ' LblTitle
        '
        LblTitle.AutoSize = True
        LblTitle.Font = New System.Drawing.Font("Segoe UI", 18, System.Drawing.FontStyle.Bold)
        LblTitle.ForeColor = System.Drawing.Color.White
        LblTitle.Location = New System.Drawing.Point(20, 12)
        LblTitle.Name = "LblTitle"
        LblTitle.Text = "📊  Data Manager        v1.0"
        '
        ' LblSubtitle
        '
        LblSubtitle.AutoSize = True
        LblSubtitle.Font = New System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Italic)
        LblSubtitle.ForeColor = System.Drawing.Color.FromArgb(160, 180, 220)
        LblSubtitle.Location = New System.Drawing.Point(22, 55)
        LblSubtitle.Name = "LblSubtitle"
        LblSubtitle.Text = "By Sarahi"
        '
        ' PanelContent
        '
        PanelContent.BackColor = System.Drawing.Color.FromArgb(26, 40, 74)
        PanelContent.Controls.Add(btnExportExcel)
        PanelContent.Controls.Add(LblExcelDesc)
        PanelContent.Controls.Add(btnExportTxt)
        PanelContent.Controls.Add(LblTxtDesc)
        PanelContent.Dock = DockStyle.Fill
        PanelContent.Name = "PanelContent"
        PanelContent.Padding = New Padding(30, 25, 30, 10)
        '
        ' btnExportExcel
        '
        btnExportExcel.BackColor = System.Drawing.Color.FromArgb(37, 99, 235)
        btnExportExcel.FlatAppearance.BorderSize = 0
        btnExportExcel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(29, 78, 187)
        btnExportExcel.FlatStyle = FlatStyle.Flat
        btnExportExcel.Font = New System.Drawing.Font("Segoe UI", 13, System.Drawing.FontStyle.Bold)
        btnExportExcel.ForeColor = System.Drawing.Color.White
        btnExportExcel.Location = New System.Drawing.Point(30, 25)
        btnExportExcel.Name = "btnExportExcel"
        btnExportExcel.Size = New System.Drawing.Size(440, 65)
        btnExportExcel.TabIndex = 0
        btnExportExcel.Text = "📂   Export Excel to DB"
        btnExportExcel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        btnExportExcel.Padding = New Padding(15, 0, 0, 0)
        btnExportExcel.UseVisualStyleBackColor = False
        btnExportExcel.Cursor = Cursors.Hand
        '
        ' LblExcelDesc
        '
        LblExcelDesc.AutoSize = True
        LblExcelDesc.Font = New System.Drawing.Font("Segoe UI", 8.5F)
        LblExcelDesc.ForeColor = System.Drawing.Color.FromArgb(160, 180, 220)
        LblExcelDesc.Location = New System.Drawing.Point(45, 96)
        LblExcelDesc.Name = "LblExcelDesc"
        LblExcelDesc.Text = "Importa y actualiza registros desde un archivo Excel hacia la base de datos"
        '
        ' btnExportTxt
        '
        btnExportTxt.BackColor = System.Drawing.Color.FromArgb(5, 150, 105)
        btnExportTxt.FlatAppearance.BorderSize = 0
        btnExportTxt.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(4, 120, 84)
        btnExportTxt.FlatStyle = FlatStyle.Flat
        btnExportTxt.Font = New System.Drawing.Font("Segoe UI", 13, System.Drawing.FontStyle.Bold)
        btnExportTxt.ForeColor = System.Drawing.Color.White
        btnExportTxt.Location = New System.Drawing.Point(30, 125)
        btnExportTxt.Name = "btnExportTxt"
        btnExportTxt.Size = New System.Drawing.Size(440, 65)
        btnExportTxt.TabIndex = 1
        btnExportTxt.Text = "📋   Import Do Not Call List"
        btnExportTxt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        btnExportTxt.Padding = New Padding(15, 0, 0, 0)
        btnExportTxt.UseVisualStyleBackColor = False
        btnExportTxt.Cursor = Cursors.Hand
        '
        ' LblTxtDesc
        '
        LblTxtDesc.AutoSize = True
        LblTxtDesc.Font = New System.Drawing.Font("Segoe UI", 8.5F)
        LblTxtDesc.ForeColor = System.Drawing.Color.FromArgb(160, 180, 220)
        LblTxtDesc.Location = New System.Drawing.Point(45, 196)
        LblTxtDesc.Name = "LblTxtDesc"
        LblTxtDesc.Text = "Importa la lista Do Not Call desde un archivo TXT/CSV al sistema DNC"
        '
        ' PanelFooter
        '
        PanelFooter.BackColor = System.Drawing.Color.FromArgb(12, 20, 44)
        PanelFooter.Controls.Add(LblFooter)
        PanelFooter.Dock = DockStyle.Bottom
        PanelFooter.Height = 36
        PanelFooter.Name = "PanelFooter"
        '
        ' LblFooter
        '
        LblFooter.Dock = DockStyle.Fill
        LblFooter.Font = New System.Drawing.Font("Segoe UI", 8.5F)
        LblFooter.ForeColor = System.Drawing.Color.FromArgb(120, 140, 180)
        LblFooter.Name = "LblFooter"
        LblFooter.Text = "@2025 spalacio"
        LblFooter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        ' FormMain
        '
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = System.Drawing.Color.FromArgb(26, 40, 74)
        ClientSize = New System.Drawing.Size(500, 370)
        Controls.Add(PanelContent)
        Controls.Add(PanelFooter)
        Controls.Add(PanelHeader)
        FormBorderStyle = FormBorderStyle.FixedSingle
        MaximizeBox = False
        Name = "FormMain"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Data Manager"
        PanelHeader.ResumeLayout(False)
        PanelHeader.PerformLayout()
        PanelContent.ResumeLayout(False)
        PanelContent.PerformLayout()
        PanelFooter.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents btnExportExcel As Button
    Friend WithEvents btnExportTxt As Button
    Friend PanelHeader As Panel
    Friend LblTitle As Label
    Friend LblSubtitle As Label
    Friend PanelContent As Panel
    Friend LblExcelDesc As Label
    Friend LblTxtDesc As Label
    Friend PanelFooter As Panel
    Friend LblFooter As Label

End Class
