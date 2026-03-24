<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormTxt
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
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As DataGridViewCellStyle = New DataGridViewCellStyle()
        PanelHeader = New Panel()
        LblTitle = New Label()
        PanelToolbar = New Panel()
        btnUpload = New FontAwesome.Sharp.IconButton()
        BtnPreview = New FontAwesome.Sharp.IconButton()
        BtnInsert = New FontAwesome.Sharp.IconButton()
        LblFilePath = New Label()
        PanelContent = New Panel()
        SplitContainer1 = New SplitContainer()
        DtGV1 = New DataGridView()
        LblMapped = New Label()
        DtGV2 = New DataGridView()
        lblPreview = New Label()
        PanelFooter = New Panel()
        LblStatus = New Label()
        LblRowCount = New Label()
        PanelHeader.SuspendLayout()
        PanelToolbar.SuspendLayout()
        PanelContent.SuspendLayout()
        CType(SplitContainer1, ComponentModel.ISupportInitialize).BeginInit()
        SplitContainer1.Panel1.SuspendLayout()
        SplitContainer1.Panel2.SuspendLayout()
        SplitContainer1.SuspendLayout()
        CType(DtGV1, ComponentModel.ISupportInitialize).BeginInit()
        CType(DtGV2, ComponentModel.ISupportInitialize).BeginInit()
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
        PanelHeader.Padding = New Padding(20, 12, 20, 8)
        PanelHeader.Size = New Size(1000, 60)
        PanelHeader.TabIndex = 0
        ' 
        ' LblTitle
        ' 
        LblTitle.AutoSize = True
        LblTitle.Font = New Font("Segoe UI", 16F, FontStyle.Bold)
        LblTitle.ForeColor = Color.FromArgb(CByte(255), CByte(255), CByte(192))
        LblTitle.Location = New Point(20, 14)
        LblTitle.Name = "LblTitle"
        LblTitle.Size = New Size(361, 30)
        LblTitle.TabIndex = 0
        LblTitle.Text = "Do Not Call — Import Wizard"
        ' 
        ' PanelToolbar
        ' 
        PanelToolbar.BackColor = Color.FromArgb(CByte(22), CByte(35), CByte(65))
        PanelToolbar.Controls.Add(btnUpload)
        PanelToolbar.Controls.Add(BtnPreview)
        PanelToolbar.Controls.Add(BtnInsert)
        PanelToolbar.Controls.Add(LblFilePath)
        PanelToolbar.Dock = DockStyle.Top
        PanelToolbar.Location = New Point(0, 60)
        PanelToolbar.Name = "PanelToolbar"
        PanelToolbar.Padding = New Padding(15, 10, 15, 10)
        PanelToolbar.Size = New Size(1000, 60)
        PanelToolbar.TabIndex = 1
        ' 
        ' btnUpload
        ' 
        btnUpload.BackColor = SystemColors.Highlight
        btnUpload.Cursor = Cursors.Hand
        btnUpload.FlatAppearance.BorderSize = 0
        btnUpload.FlatAppearance.MouseOverBackColor = Color.FromArgb(CByte(29), CByte(78), CByte(187))
        btnUpload.FlatStyle = FlatStyle.Flat
        btnUpload.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        btnUpload.ForeColor = Color.White
        btnUpload.IconChar = FontAwesome.Sharp.IconChar.FolderOpen
        btnUpload.IconColor = Color.White
        btnUpload.IconFont = FontAwesome.Sharp.IconFont.Auto
        btnUpload.IconSize = 24
        btnUpload.ImageAlign = ContentAlignment.MiddleLeft
        btnUpload.Location = New Point(15, 12)
        btnUpload.Name = "btnUpload"
        btnUpload.Size = New Size(150, 36)
        btnUpload.TabIndex = 0
        btnUpload.Text = "  Upload Txt"
        btnUpload.TextImageRelation = TextImageRelation.ImageBeforeText
        btnUpload.UseVisualStyleBackColor = False
        ' 
        ' BtnPreview
        ' 
        BtnPreview.BackColor = SystemColors.Highlight
        BtnPreview.Cursor = Cursors.Hand
        BtnPreview.Enabled = False
        BtnPreview.FlatAppearance.BorderSize = 0
        BtnPreview.FlatAppearance.MouseOverBackColor = Color.FromArgb(CByte(29), CByte(78), CByte(187))
        BtnPreview.FlatStyle = FlatStyle.Flat
        BtnPreview.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        BtnPreview.ForeColor = Color.White
        BtnPreview.IconChar = FontAwesome.Sharp.IconChar.Eye
        BtnPreview.IconColor = Color.White
        BtnPreview.IconFont = FontAwesome.Sharp.IconFont.Auto
        BtnPreview.IconSize = 24
        BtnPreview.ImageAlign = ContentAlignment.MiddleLeft
        BtnPreview.Location = New Point(180, 12)
        BtnPreview.Name = "BtnPreview"
        BtnPreview.Size = New Size(150, 36)
        BtnPreview.TabIndex = 1
        BtnPreview.Text = "  Preview"
        BtnPreview.TextImageRelation = TextImageRelation.ImageBeforeText
        BtnPreview.UseVisualStyleBackColor = False
        ' 
        ' BtnInsert
        ' 
        BtnInsert.BackColor = Color.FromArgb(CByte(224), CByte(224), CByte(224))
        BtnInsert.Cursor = Cursors.Hand
        BtnInsert.Enabled = False
        BtnInsert.FlatAppearance.BorderSize = 0
        BtnInsert.FlatAppearance.MouseOverBackColor = Color.FromArgb(CByte(16), CByte(130), CByte(58))
        BtnInsert.FlatStyle = FlatStyle.Flat
        BtnInsert.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        BtnInsert.ForeColor = Color.Black
        BtnInsert.IconChar = FontAwesome.Sharp.IconChar.Check
        BtnInsert.IconColor = Color.Black
        BtnInsert.IconFont = FontAwesome.Sharp.IconFont.Auto
        BtnInsert.IconSize = 24
        BtnInsert.ImageAlign = ContentAlignment.MiddleLeft
        BtnInsert.Location = New Point(345, 12)
        BtnInsert.Name = "BtnInsert"
        BtnInsert.Size = New Size(170, 36)
        BtnInsert.TabIndex = 2
        BtnInsert.Text = "  Insert into DB"
        BtnInsert.TextImageRelation = TextImageRelation.ImageBeforeText
        BtnInsert.UseVisualStyleBackColor = False
        ' 
        ' LblFilePath
        ' 
        LblFilePath.AutoSize = True
        LblFilePath.Font = New Font("Segoe UI", 9F)
        LblFilePath.ForeColor = Color.FromArgb(CByte(160), CByte(180), CByte(220))
        LblFilePath.Location = New Point(535, 20)
        LblFilePath.Name = "LblFilePath"
        LblFilePath.Size = New Size(88, 15)
        LblFilePath.TabIndex = 3
        LblFilePath.Text = "No file selected"
        ' 
        ' PanelContent
        ' 
        PanelContent.Controls.Add(SplitContainer1)
        PanelContent.Dock = DockStyle.Fill
        PanelContent.Location = New Point(0, 120)
        PanelContent.Name = "PanelContent"
        PanelContent.Padding = New Padding(15)
        PanelContent.Size = New Size(1000, 500)
        PanelContent.TabIndex = 2
        ' 
        ' SplitContainer1
        ' 
        SplitContainer1.Dock = DockStyle.Fill
        SplitContainer1.Location = New Point(15, 15)
        SplitContainer1.Name = "SplitContainer1"
        SplitContainer1.Orientation = Orientation.Horizontal
        ' 
        ' SplitContainer1.Panel1
        ' 
        SplitContainer1.Panel1.Controls.Add(DtGV1)
        SplitContainer1.Panel1.Controls.Add(LblMapped)
        SplitContainer1.Panel1.Padding = New Padding(0, 0, 0, 10)
        ' 
        ' SplitContainer1.Panel2
        ' 
        SplitContainer1.Panel2.Controls.Add(DtGV2)
        SplitContainer1.Panel2.Controls.Add(lblPreview)
        SplitContainer1.Panel2.Padding = New Padding(0, 10, 0, 0)
        SplitContainer1.Size = New Size(970, 470)
        SplitContainer1.SplitterDistance = 200
        SplitContainer1.TabIndex = 0
        ' 
        ' DtGV1
        ' 
        DtGV1.AllowUserToAddRows = False
        DtGV1.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = Color.FromArgb(CByte(22), CByte(35), CByte(65))
        DataGridViewCellStyle1.ForeColor = Color.FromArgb(CByte(200), CByte(210), CByte(235))
        DataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(CByte(37), CByte(99), CByte(235))
        DataGridViewCellStyle1.SelectionForeColor = Color.White
        DtGV1.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        DtGV1.BackgroundColor = Color.FromArgb(CByte(20), CByte(33), CByte(61))
        DtGV1.BorderStyle = BorderStyle.None
        DataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = Color.FromArgb(CByte(18), CByte(30), CByte(58))
        DataGridViewCellStyle2.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        DataGridViewCellStyle2.ForeColor = Color.FromArgb(CByte(200), CByte(215), CByte(245))
        DataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(CByte(37), CByte(99), CByte(235))
        DataGridViewCellStyle2.SelectionForeColor = Color.White
        DtGV1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        DtGV1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = Color.FromArgb(CByte(26), CByte(40), CByte(74))
        DataGridViewCellStyle3.Font = New Font("Segoe UI", 9F)
        DataGridViewCellStyle3.ForeColor = Color.FromArgb(CByte(200), CByte(210), CByte(235))
        DataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(CByte(37), CByte(99), CByte(235))
        DataGridViewCellStyle3.SelectionForeColor = Color.White
        DataGridViewCellStyle3.WrapMode = DataGridViewTriState.False
        DtGV1.DefaultCellStyle = DataGridViewCellStyle3
        DtGV1.Dock = DockStyle.Fill
        DtGV1.EnableHeadersVisualStyles = False
        DtGV1.GridColor = Color.FromArgb(CByte(40), CByte(55), CByte(90))
        DtGV1.Location = New Point(0, 25)
        DtGV1.Name = "DtGV1"
        DtGV1.ReadOnly = True
        DtGV1.RowHeadersVisible = False
        DtGV1.Size = New Size(970, 165)
        DtGV1.TabIndex = 0
        ' 
        ' LblMapped
        ' 
        LblMapped.Dock = DockStyle.Top
        LblMapped.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        LblMapped.ForeColor = Color.FromArgb(CByte(200), CByte(215), CByte(245))
        LblMapped.Location = New Point(0, 0)
        LblMapped.Name = "LblMapped"
        LblMapped.Padding = New Padding(0, 0, 0, 5)
        LblMapped.Size = New Size(970, 25)
        LblMapped.TabIndex = 1
        LblMapped.Text = "TXT Data (raw):"
        ' 
        ' DtGV2
        ' 
        DtGV2.AllowUserToAddRows = False
        DtGV2.AllowUserToDeleteRows = False
        DataGridViewCellStyle4.BackColor = Color.FromArgb(CByte(22), CByte(35), CByte(65))
        DataGridViewCellStyle4.ForeColor = Color.FromArgb(CByte(200), CByte(210), CByte(235))
        DataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(CByte(37), CByte(99), CByte(235))
        DataGridViewCellStyle4.SelectionForeColor = Color.White
        DtGV2.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle4
        DtGV2.BackgroundColor = Color.FromArgb(CByte(20), CByte(33), CByte(61))
        DtGV2.BorderStyle = BorderStyle.None
        DataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = Color.FromArgb(CByte(18), CByte(30), CByte(58))
        DataGridViewCellStyle5.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        DataGridViewCellStyle5.ForeColor = Color.FromArgb(CByte(200), CByte(215), CByte(245))
        DataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(CByte(37), CByte(99), CByte(235))
        DataGridViewCellStyle5.SelectionForeColor = Color.White
        DtGV2.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle5
        DtGV2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = Color.FromArgb(CByte(26), CByte(40), CByte(74))
        DataGridViewCellStyle6.Font = New Font("Segoe UI", 9F)
        DataGridViewCellStyle6.ForeColor = Color.FromArgb(CByte(200), CByte(210), CByte(235))
        DataGridViewCellStyle6.SelectionBackColor = Color.FromArgb(CByte(37), CByte(99), CByte(235))
        DataGridViewCellStyle6.SelectionForeColor = Color.White
        DataGridViewCellStyle6.WrapMode = DataGridViewTriState.False
        DtGV2.DefaultCellStyle = DataGridViewCellStyle6
        DtGV2.Dock = DockStyle.Fill
        DtGV2.EnableHeadersVisualStyles = False
        DtGV2.GridColor = Color.FromArgb(CByte(40), CByte(55), CByte(90))
        DtGV2.Location = New Point(0, 35)
        DtGV2.Name = "DtGV2"
        DtGV2.ReadOnly = True
        DtGV2.RowHeadersVisible = False
        DtGV2.Size = New Size(970, 231)
        DtGV2.TabIndex = 0
        ' 
        ' lblPreview
        ' 
        lblPreview.Dock = DockStyle.Top
        lblPreview.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        lblPreview.ForeColor = Color.FromArgb(CByte(200), CByte(215), CByte(245))
        lblPreview.Location = New Point(0, 10)
        lblPreview.Name = "lblPreview"
        lblPreview.Padding = New Padding(0, 0, 0, 5)
        lblPreview.Size = New Size(970, 25)
        lblPreview.TabIndex = 1
        lblPreview.Text = "Mapped Data — ready to insert into DNC.dbo.DoNotCallNumbers:"
        ' 
        ' PanelFooter
        ' 
        PanelFooter.BackColor = Color.FromArgb(CByte(12), CByte(20), CByte(44))
        PanelFooter.Controls.Add(LblStatus)
        PanelFooter.Controls.Add(LblRowCount)
        PanelFooter.Dock = DockStyle.Bottom
        PanelFooter.Location = New Point(0, 620)
        PanelFooter.Name = "PanelFooter"
        PanelFooter.Size = New Size(1000, 36)
        PanelFooter.TabIndex = 3
        ' 
        ' LblStatus
        ' 
        LblStatus.Font = New Font("Segoe UI", 9F)
        LblStatus.ForeColor = Color.FromArgb(CByte(120), CByte(140), CByte(180))
        LblStatus.Location = New Point(15, 0)
        LblStatus.Name = "LblStatus"
        LblStatus.Size = New Size(600, 36)
        LblStatus.TabIndex = 0
        LblStatus.Text = "Ready"
        LblStatus.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' LblRowCount
        ' 
        LblRowCount.Font = New Font("Segoe UI", 9F)
        LblRowCount.ForeColor = Color.FromArgb(CByte(120), CByte(140), CByte(180))
        LblRowCount.Location = New Point(750, 0)
        LblRowCount.Name = "LblRowCount"
        LblRowCount.Size = New Size(235, 36)
        LblRowCount.TabIndex = 1
        LblRowCount.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' FormTxt
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(26), CByte(40), CByte(74))
        ClientSize = New Size(1000, 656)
        Controls.Add(PanelContent)
        Controls.Add(PanelFooter)
        Controls.Add(PanelToolbar)
        Controls.Add(PanelHeader)
        Name = "FormTxt"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Do Not Call — Import Wizard"
        PanelHeader.ResumeLayout(False)
        PanelHeader.PerformLayout()
        PanelToolbar.ResumeLayout(False)
        PanelToolbar.PerformLayout()
        PanelContent.ResumeLayout(False)
        SplitContainer1.Panel1.ResumeLayout(False)
        SplitContainer1.Panel2.ResumeLayout(False)
        CType(SplitContainer1, ComponentModel.ISupportInitialize).EndInit()
        SplitContainer1.ResumeLayout(False)
        CType(DtGV1, ComponentModel.ISupportInitialize).EndInit()
        CType(DtGV2, ComponentModel.ISupportInitialize).EndInit()
        PanelFooter.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents PanelHeader As Panel
    Friend WithEvents LblTitle As Label
    Friend WithEvents PanelToolbar As Panel
    Friend WithEvents btnUpload As FontAwesome.Sharp.IconButton
    Friend WithEvents BtnPreview As FontAwesome.Sharp.IconButton
    Friend WithEvents BtnInsert As FontAwesome.Sharp.IconButton
    Friend WithEvents LblFilePath As Label
    Friend WithEvents PanelContent As Panel
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents LblMapped As Label
    Friend WithEvents DtGV1 As DataGridView
    Friend WithEvents lblPreview As Label
    Friend WithEvents DtGV2 As DataGridView
    Friend WithEvents PanelFooter As Panel
    Friend WithEvents LblStatus As Label
    Friend WithEvents LblRowCount As Label

End Class
