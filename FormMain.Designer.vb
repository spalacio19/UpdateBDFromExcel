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
        PanelContent = New Panel()
        btnExportExcel = New FontAwesome.Sharp.IconButton()
        LblExcelDesc = New Label()
        btnExportTxt = New FontAwesome.Sharp.IconButton()
        LblTxtDesc = New Label()
        btnPolicySnapshot = New FontAwesome.Sharp.IconButton()
        LblPolicyDesc = New Label()
        btnFrancoNet = New FontAwesome.Sharp.IconButton()
        LblFrancoNetDesc = New Label()
        PanelFooter = New Panel()
        LblFooter = New Label()
        PanelHeader.SuspendLayout()
        PanelContent.SuspendLayout()
        PanelFooter.SuspendLayout()
        SuspendLayout()
        ' 
        ' PanelHeader
        ' 
        PanelHeader.BackColor = Color.FromArgb(CByte(18), CByte(30), CByte(58))
        PanelHeader.Controls.Add(LblTitle)
        PanelHeader.Dock = DockStyle.Top
        PanelHeader.Location = New Point(0, 0)
        PanelHeader.Name = "PanelHeader"
        PanelHeader.Padding = New Padding(20, 15, 20, 10)
        PanelHeader.Size = New Size(500, 90)
        PanelHeader.TabIndex = 2
        ' 
        ' LblTitle
        ' 
        LblTitle.AutoSize = True
        LblTitle.Font = New Font("Segoe UI", 18F, FontStyle.Bold)
        LblTitle.ForeColor = Color.FromArgb(CByte(192), CByte(255), CByte(255))
        LblTitle.Location = New Point(83, 22)
        LblTitle.Name = "LblTitle"
        LblTitle.Size = New Size(328, 32)
        LblTitle.TabIndex = 0
        LblTitle.Text = "📊  Data Manager        v1.0"
        ' 
        ' PanelContent
        ' 
        PanelContent.BackColor = Color.FromArgb(CByte(26), CByte(40), CByte(74))
        PanelContent.Controls.Add(btnExportExcel)
        PanelContent.Controls.Add(LblExcelDesc)
        PanelContent.Controls.Add(btnExportTxt)
        PanelContent.Controls.Add(LblTxtDesc)
        PanelContent.Controls.Add(btnPolicySnapshot)
        PanelContent.Controls.Add(LblPolicyDesc)
        PanelContent.Controls.Add(btnFrancoNet)
        PanelContent.Controls.Add(LblFrancoNetDesc)
        PanelContent.Dock = DockStyle.Fill
        PanelContent.Location = New Point(0, 90)
        PanelContent.Name = "PanelContent"
        PanelContent.Padding = New Padding(30, 25, 30, 10)
        PanelContent.Size = New Size(500, 454)
        PanelContent.TabIndex = 0
        ' 
        ' btnExportExcel
        ' 
        btnExportExcel.BackColor = SystemColors.Highlight
        btnExportExcel.Cursor = Cursors.Hand
        btnExportExcel.FlatAppearance.BorderSize = 0
        btnExportExcel.FlatAppearance.MouseOverBackColor = Color.FromArgb(CByte(29), CByte(78), CByte(187))
        btnExportExcel.FlatStyle = FlatStyle.Flat
        btnExportExcel.Font = New Font("Segoe UI", 13F, FontStyle.Bold)
        btnExportExcel.ForeColor = Color.FromArgb(CByte(224), CByte(224), CByte(224))
        btnExportExcel.IconChar = FontAwesome.Sharp.IconChar.FileExcel
        btnExportExcel.IconColor = Color.FromArgb(CByte(224), CByte(224), CByte(224))
        btnExportExcel.IconFont = FontAwesome.Sharp.IconFont.Auto
        btnExportExcel.IconSize = 32
        btnExportExcel.ImageAlign = ContentAlignment.MiddleLeft
        btnExportExcel.Location = New Point(30, 223)
        btnExportExcel.Name = "btnExportExcel"
        btnExportExcel.Padding = New Padding(15, 0, 0, 0)
        btnExportExcel.Size = New Size(440, 65)
        btnExportExcel.TabIndex = 5
        btnExportExcel.Text = "  Export Excel to DB (TempData_Debug)"
        btnExportExcel.TextAlign = ContentAlignment.MiddleLeft
        btnExportExcel.TextImageRelation = TextImageRelation.ImageBeforeText
        btnExportExcel.UseVisualStyleBackColor = False
        ' 
        ' LblExcelDesc
        ' 
        LblExcelDesc.AutoSize = True
        LblExcelDesc.Font = New Font("Segoe UI", 8.5F)
        LblExcelDesc.ForeColor = Color.FromArgb(CByte(160), CByte(180), CByte(220))
        LblExcelDesc.Location = New Point(45, 294)
        LblExcelDesc.Name = "LblExcelDesc"
        LblExcelDesc.Size = New Size(395, 15)
        LblExcelDesc.TabIndex = 6
        LblExcelDesc.Text = "Importa y actualiza registros desde un archivo Excel hacia la base de datos"
        ' 
        ' btnExportTxt
        ' 
        btnExportTxt.BackColor = SystemColors.Highlight
        btnExportTxt.Cursor = Cursors.Hand
        btnExportTxt.FlatAppearance.BorderSize = 0
        btnExportTxt.FlatAppearance.MouseOverBackColor = Color.FromArgb(CByte(29), CByte(78), CByte(187))
        btnExportTxt.FlatStyle = FlatStyle.Flat
        btnExportTxt.Font = New Font("Segoe UI", 13F, FontStyle.Bold)
        btnExportTxt.ForeColor = Color.FromArgb(CByte(224), CByte(224), CByte(224))
        btnExportTxt.IconChar = FontAwesome.Sharp.IconChar.ClipboardList
        btnExportTxt.IconColor = Color.FromArgb(CByte(224), CByte(224), CByte(224))
        btnExportTxt.IconFont = FontAwesome.Sharp.IconFont.Auto
        btnExportTxt.IconSize = 32
        btnExportTxt.ImageAlign = ContentAlignment.MiddleLeft
        btnExportTxt.Location = New Point(30, 15)
        btnExportTxt.Name = "btnExportTxt"
        btnExportTxt.Padding = New Padding(15, 0, 0, 0)
        btnExportTxt.Size = New Size(440, 65)
        btnExportTxt.TabIndex = 1
        btnExportTxt.Text = "  Import Do Not Call List"
        btnExportTxt.TextAlign = ContentAlignment.MiddleLeft
        btnExportTxt.TextImageRelation = TextImageRelation.ImageBeforeText
        btnExportTxt.UseVisualStyleBackColor = False
        ' 
        ' LblTxtDesc
        ' 
        LblTxtDesc.AutoSize = True
        LblTxtDesc.Font = New Font("Segoe UI", 8.5F)
        LblTxtDesc.ForeColor = Color.FromArgb(CByte(160), CByte(180), CByte(220))
        LblTxtDesc.Location = New Point(45, 86)
        LblTxtDesc.Name = "LblTxtDesc"
        LblTxtDesc.Size = New Size(375, 15)
        LblTxtDesc.TabIndex = 2
        LblTxtDesc.Text = "Importa la lista Do Not Call desde un archivo TXT/CSV al sistema DNC"
        ' 
        ' btnPolicySnapshot
        ' 
        btnPolicySnapshot.BackColor = SystemColors.Highlight
        btnPolicySnapshot.Cursor = Cursors.Hand
        btnPolicySnapshot.FlatAppearance.BorderSize = 0
        btnPolicySnapshot.FlatAppearance.MouseOverBackColor = Color.FromArgb(CByte(29), CByte(78), CByte(187))
        btnPolicySnapshot.FlatStyle = FlatStyle.Flat
        btnPolicySnapshot.Font = New Font("Segoe UI", 13F, FontStyle.Bold)
        btnPolicySnapshot.ForeColor = Color.FromArgb(CByte(224), CByte(224), CByte(224))
        btnPolicySnapshot.IconChar = FontAwesome.Sharp.IconChar.Database
        btnPolicySnapshot.IconColor = Color.FromArgb(CByte(224), CByte(224), CByte(224))
        btnPolicySnapshot.IconFont = FontAwesome.Sharp.IconFont.Auto
        btnPolicySnapshot.IconSize = 32
        btnPolicySnapshot.ImageAlign = ContentAlignment.MiddleLeft
        btnPolicySnapshot.Location = New Point(30, 115)
        btnPolicySnapshot.Name = "btnPolicySnapshot"
        btnPolicySnapshot.Padding = New Padding(15, 0, 0, 0)
        btnPolicySnapshot.Size = New Size(440, 65)
        btnPolicySnapshot.TabIndex = 3
        btnPolicySnapshot.Text = "  Policy In Force Snapshot"
        btnPolicySnapshot.TextAlign = ContentAlignment.MiddleLeft
        btnPolicySnapshot.TextImageRelation = TextImageRelation.ImageBeforeText
        btnPolicySnapshot.UseVisualStyleBackColor = False
        ' 
        ' LblPolicyDesc
        ' 
        LblPolicyDesc.AutoSize = True
        LblPolicyDesc.Font = New Font("Segoe UI", 8.5F)
        LblPolicyDesc.ForeColor = Color.FromArgb(CByte(160), CByte(180), CByte(220))
        LblPolicyDesc.Location = New Point(45, 186)
        LblPolicyDesc.Name = "LblPolicyDesc"
        LblPolicyDesc.Size = New Size(392, 15)
        LblPolicyDesc.TabIndex = 4
        LblPolicyDesc.Text = "Inserta datos de polizas vigentes en ReportDB.dbo.PolicyInForceSnapshot"
        ' 
        ' btnFrancoNet
        ' 
        btnFrancoNet.BackColor = Color.Teal
        btnFrancoNet.Cursor = Cursors.Hand
        btnFrancoNet.FlatAppearance.BorderSize = 0
        btnFrancoNet.FlatAppearance.MouseOverBackColor = Color.FromArgb(CByte(16), CByte(130), CByte(58))
        btnFrancoNet.FlatStyle = FlatStyle.Flat
        btnFrancoNet.Font = New Font("Segoe UI", 13F, FontStyle.Bold)
        btnFrancoNet.ForeColor = Color.White
        btnFrancoNet.IconChar = FontAwesome.Sharp.IconChar.NetworkWired
        btnFrancoNet.IconColor = Color.White
        btnFrancoNet.IconFont = FontAwesome.Sharp.IconFont.Auto
        btnFrancoNet.IconSize = 32
        btnFrancoNet.ImageAlign = ContentAlignment.MiddleLeft
        btnFrancoNet.Location = New Point(30, 325)
        btnFrancoNet.Name = "btnFrancoNet"
        btnFrancoNet.Padding = New Padding(15, 0, 0, 0)
        btnFrancoNet.Size = New Size(440, 65)
        btnFrancoNet.TabIndex = 7
        btnFrancoNet.Text = "  Franco Net (Append Mode)"
        btnFrancoNet.TextAlign = ContentAlignment.MiddleLeft
        btnFrancoNet.TextImageRelation = TextImageRelation.ImageBeforeText
        btnFrancoNet.UseVisualStyleBackColor = False
        ' 
        ' LblFrancoNetDesc
        ' 
        LblFrancoNetDesc.AutoSize = True
        LblFrancoNetDesc.Font = New Font("Segoe UI", 8.5F)
        LblFrancoNetDesc.ForeColor = Color.FromArgb(CByte(160), CByte(180), CByte(220))
        LblFrancoNetDesc.Location = New Point(45, 396)
        LblFrancoNetDesc.Name = "LblFrancoNetDesc"
        LblFrancoNetDesc.Size = New Size(391, 15)
        LblFrancoNetDesc.TabIndex = 8
        LblFrancoNetDesc.Text = "Importa y acumula datos en la tabla FrancoNet_Temp (sin borrar previos)"
        ' 
        ' PanelFooter
        ' 
        PanelFooter.BackColor = Color.FromArgb(CByte(12), CByte(20), CByte(44))
        PanelFooter.Controls.Add(LblFooter)
        PanelFooter.Dock = DockStyle.Bottom
        PanelFooter.Location = New Point(0, 544)
        PanelFooter.Name = "PanelFooter"
        PanelFooter.Size = New Size(500, 36)
        PanelFooter.TabIndex = 1
        ' 
        ' LblFooter
        ' 
        LblFooter.Dock = DockStyle.Fill
        LblFooter.Font = New Font("Segoe UI", 8.5F)
        LblFooter.ForeColor = Color.FromArgb(CByte(120), CByte(140), CByte(180))
        LblFooter.Location = New Point(0, 0)
        LblFooter.Name = "LblFooter"
        LblFooter.Size = New Size(500, 36)
        LblFooter.TabIndex = 0
        LblFooter.Text = "@2025 spalacio"
        LblFooter.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' FormMain
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(26), CByte(40), CByte(74))
        ClientSize = New Size(500, 580)
        Controls.Add(PanelContent)
        Controls.Add(PanelFooter)
        Controls.Add(PanelHeader)
        FormBorderStyle = FormBorderStyle.FixedSingle
        MaximizeBox = False
        Name = "FormMain"
        Text = "Data Manager"
        PanelHeader.ResumeLayout(False)
        PanelHeader.PerformLayout()
        PanelContent.ResumeLayout(False)
        PanelContent.PerformLayout()
        PanelFooter.ResumeLayout(False)
        ResumeLayout(False)
    End Sub
    Friend WithEvents btnExportTxt As FontAwesome.Sharp.IconButton
    Friend WithEvents btnPolicySnapshot As FontAwesome.Sharp.IconButton
    Friend PanelHeader As Panel
    Friend LblTitle As Label
    Friend PanelContent As Panel
    Friend LblTxtDesc As Label
    Friend LblPolicyDesc As Label
    Friend PanelFooter As Panel
    Friend LblFooter As Label
    Friend WithEvents btnExportExcel As FontAwesome.Sharp.IconButton
    Friend WithEvents LblExcelDesc As Label
    Friend WithEvents btnFrancoNet As FontAwesome.Sharp.IconButton
    Friend WithEvents LblFrancoNetDesc As Label

End Class
