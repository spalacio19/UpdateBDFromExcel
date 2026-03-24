<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form2
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
        PanelHeader = New Panel()
        LblTitle = New Label()
        PanelToolbar = New Panel()
        btnUpload = New Button()
        btnExportDB = New Button()
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
        LblTitle.Size = New Size(320, 30)
        LblTitle.TabIndex = 0
        LblTitle.Text = "📂  Export Excel to Database"
        '
        ' PanelToolbar
        '
        PanelToolbar.BackColor = Color.FromArgb(CByte(22), CByte(35), CByte(65))
        PanelToolbar.Controls.Add(btnUpload)
        PanelToolbar.Controls.Add(btnExportDB)
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
        btnUpload.BackColor = Color.FromArgb(CByte(37), CByte(99), CByte(235))
        btnUpload.Cursor = Cursors.Hand
        btnUpload.FlatAppearance.BorderSize = 0
        btnUpload.FlatAppearance.MouseOverBackColor = Color.FromArgb(CByte(29), CByte(78), CByte(187))
        btnUpload.FlatStyle = FlatStyle.Flat
        btnUpload.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        btnUpload.ForeColor = Color.FromArgb(CByte(224), CByte(224), CByte(224))
        btnUpload.Location = New Point(15, 12)
        btnUpload.Name = "btnUpload"
        btnUpload.Size = New Size(150, 36)
        btnUpload.TabIndex = 0
        btnUpload.Text = "📂  Upload Excel"
        btnUpload.UseVisualStyleBackColor = False
        '
        ' btnExportDB
        '
        btnExportDB.BackColor = Color.FromArgb(CByte(22), CByte(163), CByte(74))
        btnExportDB.Cursor = Cursors.Hand
        btnExportDB.FlatAppearance.BorderSize = 0
        btnExportDB.FlatAppearance.MouseOverBackColor = Color.FromArgb(CByte(16), CByte(130), CByte(58))
        btnExportDB.FlatStyle = FlatStyle.Flat
        btnExportDB.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        btnExportDB.ForeColor = Color.FromArgb(CByte(224), CByte(224), CByte(224))
        btnExportDB.Location = New Point(180, 12)
        btnExportDB.Name = "btnExportDB"
        btnExportDB.Size = New Size(180, 36)
        btnExportDB.TabIndex = 1
        btnExportDB.Text = "💾  Export directly to DB"
        btnExportDB.UseVisualStyleBackColor = False
        '
        ' LblFilePath
        '
        LblFilePath.AutoSize = True
        LblFilePath.Font = New Font("Segoe UI", 9F)
        LblFilePath.ForeColor = Color.FromArgb(CByte(160), CByte(180), CByte(220))
        LblFilePath.Location = New Point(380, 20)
        LblFilePath.Name = "LblFilePath"
        LblFilePath.Size = New Size(100, 15)
        LblFilePath.TabIndex = 2
        LblFilePath.Text = "No file loaded"
        '
        ' DtGV1
        '
        DtGV1.AllowUserToAddRows = False
        DtGV1.AllowUserToDeleteRows = False
        DtGV1.BackgroundColor = Color.FromArgb(CByte(20), CByte(33), CByte(61))
        DtGV1.BorderStyle = BorderStyle.None
        DtGV1.ColumnHeadersDefaultCellStyle = New DataGridViewCellStyle() With {
            .BackColor = Color.FromArgb(18, 30, 58),
            .ForeColor = Color.FromArgb(200, 215, 245),
            .Font = New Font("Segoe UI", 9F, FontStyle.Bold),
            .SelectionBackColor = Color.FromArgb(37, 99, 235),
            .SelectionForeColor = Color.White,
            .Alignment = DataGridViewContentAlignment.MiddleLeft
        }
        DtGV1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DtGV1.DefaultCellStyle = New DataGridViewCellStyle() With {
            .BackColor = Color.FromArgb(26, 40, 74),
            .ForeColor = Color.FromArgb(200, 210, 235),
            .Font = New Font("Segoe UI", 9F),
            .SelectionBackColor = Color.FromArgb(37, 99, 235),
            .SelectionForeColor = Color.White
        }
        DtGV1.AlternatingRowsDefaultCellStyle = New DataGridViewCellStyle() With {
            .BackColor = Color.FromArgb(22, 35, 65),
            .ForeColor = Color.FromArgb(200, 210, 235),
            .SelectionBackColor = Color.FromArgb(37, 99, 235),
            .SelectionForeColor = Color.White
        }
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
        ' Form2
        '
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(26), CByte(40), CByte(74))
        ClientSize = New Size(1000, 550)
        Controls.Add(DtGV1)
        Controls.Add(PanelFooter)
        Controls.Add(PanelToolbar)
        Controls.Add(PanelHeader)
        Name = "Form2"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Export Excel to Database"
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
    Friend WithEvents btnExportDB As Button
    Friend WithEvents LblFilePath As Label
    Friend WithEvents DtGV1 As DataGridView
    Friend WithEvents PanelFooter As Panel
    Friend WithEvents LblStatus As Label
    Friend WithEvents LblRowCount As Label

End Class
