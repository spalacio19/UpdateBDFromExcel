<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        PanelHeader = New Panel()
        LblTitle = New Label()
        PanelToolbar = New Panel()
        btnUpload = New Button()
        btnReadAzure = New Button()
        btnInsertSnapshot = New Button()
        LblFilePath = New Label()
        DtGV1 = New DataGridView()
        PanelFooter = New Panel()
        LblStatus = New Label()
        LblRowCount = New Label()
        PanelHeader.SuspendLayout()
        PanelToolbar.SuspendLayout()
        CType(DtGV1, ComponentModel.ISupportInitialize).BeginInit()
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
        LblTitle.Size = New Size(310, 30)
        LblTitle.TabIndex = 0
        LblTitle.Text = "📑  Policy In-Force Snapshot"
        ' 
        ' PanelToolbar
        ' 
        PanelToolbar.BackColor = Color.FromArgb(CByte(22), CByte(35), CByte(65))
        PanelToolbar.Controls.Add(btnUpload)
        PanelToolbar.Controls.Add(btnReadAzure)
        PanelToolbar.Controls.Add(btnInsertSnapshot)
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
        btnUpload.Location = New Point(15, 12)
        btnUpload.Name = "btnUpload"
        btnUpload.Size = New Size(150, 36)
        btnUpload.TabIndex = 0
        btnUpload.Text = "📂  Upload Excel"
        btnUpload.UseVisualStyleBackColor = False
        ' 
        ' btnReadAzure
        ' 
        btnReadAzure.BackColor = SystemColors.Highlight
        btnReadAzure.Cursor = Cursors.Hand
        btnReadAzure.FlatAppearance.BorderSize = 0
        btnReadAzure.FlatAppearance.MouseOverBackColor = Color.FromArgb(CByte(29), CByte(78), CByte(187))
        btnReadAzure.FlatStyle = FlatStyle.Flat
        btnReadAzure.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        btnReadAzure.ForeColor = Color.White
        btnReadAzure.Location = New Point(180, 12)
        btnReadAzure.Name = "btnReadAzure"
        btnReadAzure.Size = New Size(170, 36)
        btnReadAzure.TabIndex = 1
        btnReadAzure.Text = "🔍  Read from Azure"
        btnReadAzure.UseVisualStyleBackColor = False
        ' 
        ' btnInsertSnapshot
        ' 
        btnInsertSnapshot.BackColor = Color.FromArgb(CByte(224), CByte(224), CByte(224))
        btnInsertSnapshot.Cursor = Cursors.Hand
        btnInsertSnapshot.FlatAppearance.BorderSize = 0
        btnInsertSnapshot.FlatAppearance.MouseOverBackColor = Color.FromArgb(CByte(16), CByte(130), CByte(58))
        btnInsertSnapshot.FlatStyle = FlatStyle.Flat
        btnInsertSnapshot.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        btnInsertSnapshot.ForeColor = Color.Black
        btnInsertSnapshot.Location = New Point(365, 12)
        btnInsertSnapshot.Name = "btnInsertSnapshot"
        btnInsertSnapshot.Size = New Size(170, 36)
        btnInsertSnapshot.TabIndex = 2
        btnInsertSnapshot.Text = "💾  Insert Snapshot"
        btnInsertSnapshot.UseVisualStyleBackColor = False
        ' 
        ' LblFilePath
        ' 
        LblFilePath.AutoSize = True
        LblFilePath.Font = New Font("Segoe UI", 9F)
        LblFilePath.ForeColor = Color.FromArgb(CByte(160), CByte(180), CByte(220))
        LblFilePath.Location = New Point(555, 20)
        LblFilePath.Name = "LblFilePath"
        LblFilePath.Size = New Size(81, 15)
        LblFilePath.TabIndex = 3
        LblFilePath.Text = "No file loaded"
        ' 
        ' DtGV1
        ' 
        DtGV1.AllowUserToAddRows = False
        DtGV1.AllowUserToDeleteRows = False
        DtGV1.BackgroundColor = Color.FromArgb(CByte(20), CByte(33), CByte(61))
        DtGV1.BorderStyle = BorderStyle.None
        DtGV1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        DtGV1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DtGV1.Dock = DockStyle.Fill
        DtGV1.EnableHeadersVisualStyles = False
        DtGV1.GridColor = Color.FromArgb(CByte(40), CByte(55), CByte(90))
        DtGV1.Location = New Point(0, 120)
        DtGV1.Name = "DtGV1"
        DtGV1.ReadOnly = True
        DtGV1.RowHeadersVisible = False
        DtGV1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing
        DtGV1.Size = New Size(1000, 394)
        DtGV1.TabIndex = 2
        ' 
        ' PanelFooter
        ' 
        PanelFooter.BackColor = Color.FromArgb(CByte(12), CByte(20), CByte(44))
        PanelFooter.Controls.Add(LblStatus)
        PanelFooter.Controls.Add(LblRowCount)
        PanelFooter.Dock = DockStyle.Bottom
        PanelFooter.Location = New Point(0, 514)
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
        LblRowCount.Text = "Rows: 0"
        LblRowCount.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(26), CByte(40), CByte(74))
        ClientSize = New Size(1000, 550)
        Controls.Add(DtGV1)
        Controls.Add(PanelFooter)
        Controls.Add(PanelToolbar)
        Controls.Add(PanelHeader)
        Name = "Form1"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Policy In-Force Snapshot"
        PanelHeader.ResumeLayout(False)
        PanelHeader.PerformLayout()
        PanelToolbar.ResumeLayout(False)
        PanelToolbar.PerformLayout()
        CType(DtGV1, ComponentModel.ISupportInitialize).EndInit()
        PanelFooter.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents PanelHeader As Panel
    Friend WithEvents LblTitle As Label
    Friend WithEvents PanelToolbar As Panel
    Friend WithEvents btnUpload As Button
    Friend WithEvents btnReadAzure As Button
    Friend WithEvents btnInsertSnapshot As Button
    Friend WithEvents LblFilePath As Label
    Friend WithEvents DtGV1 As DataGridView
    Friend WithEvents PanelFooter As Panel
    Friend WithEvents LblStatus As Label
    Friend WithEvents LblRowCount As Label

End Class
