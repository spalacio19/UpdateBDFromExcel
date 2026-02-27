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
        DtGV1 = New DataGridView()
        DtGV2 = New DataGridView()
        Button1 = New Button()
        Label1 = New Label()
        BtnPreview = New Button()
        BtnInsert = New Button()
        LblMapped = New Label()
        lblPreview = New Label()
        CType(DtGV1, ComponentModel.ISupportInitialize).BeginInit()
        CType(DtGV2, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' DtGV1
        ' 
        DtGV1.AllowUserToAddRows = False
        DtGV1.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        DtGV1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DtGV1.Location = New Point(12, 68)
        DtGV1.Name = "DtGV1"
        DtGV1.ReadOnly = True
        DtGV1.Size = New Size(776, 190)
        DtGV1.TabIndex = 0
        ' 
        ' DtGV2
        ' 
        DtGV2.AllowUserToAddRows = False
        DtGV2.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DtGV2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DtGV2.Location = New Point(12, 288)
        DtGV2.Name = "DtGV2"
        DtGV2.ReadOnly = True
        DtGV2.Size = New Size(776, 230)
        DtGV2.TabIndex = 5
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(12, 12)
        Button1.Name = "Button1"
        Button1.Size = New Size(100, 27)
        Button1.TabIndex = 1
        Button1.Text = "📂 Upload Txt"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.ForeColor = Color.Gray
        Label1.Location = New Point(120, 18)
        Label1.Name = "Label1"
        Label1.Size = New Size(450, 15)
        Label1.TabIndex = 2
        Label1.Text = "No file selected"
        ' 
        ' BtnPreview
        ' 
        BtnPreview.Enabled = False
        BtnPreview.Location = New Point(580, 12)
        BtnPreview.Name = "BtnPreview"
        BtnPreview.Size = New Size(90, 27)
        BtnPreview.TabIndex = 3
        BtnPreview.Text = "👁 Preview"
        BtnPreview.UseVisualStyleBackColor = True
        ' 
        ' BtnInsert
        ' 
        BtnInsert.BackColor = Color.FromArgb(CByte(0), CByte(120), CByte(212))
        BtnInsert.Enabled = False
        BtnInsert.FlatStyle = FlatStyle.Flat
        BtnInsert.ForeColor = Color.White
        BtnInsert.Location = New Point(678, 12)
        BtnInsert.Name = "BtnInsert"
        BtnInsert.Size = New Size(110, 27)
        BtnInsert.TabIndex = 4
        BtnInsert.Text = "✔ Insert into DB"
        BtnInsert.UseVisualStyleBackColor = False
        ' 
        ' LblMapped
        ' 
        LblMapped.AutoSize = True
        LblMapped.Font = New Font("Segoe UI", 8.5F, FontStyle.Bold)
        LblMapped.Location = New Point(12, 48)
        LblMapped.Name = "LblMapped"
        LblMapped.Size = New Size(100, 15)
        LblMapped.TabIndex = 5
        LblMapped.Text = "Excel Data (raw):"
        ' 
        ' lblPreview
        ' 
        lblPreview.AutoSize = True
        lblPreview.Font = New Font("Segoe UI", 8.5F, FontStyle.Bold)
        lblPreview.Location = New Point(12, 268)
        lblPreview.Name = "lblPreview"
        lblPreview.Size = New Size(370, 15)
        lblPreview.TabIndex = 6
        lblPreview.Text = "Mapped Data — ready to insert into DNC.dbo.DoNotCallNumbers:"
        ' 
        ' FormTxt
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(800, 530)
        Controls.Add(Button1)
        Controls.Add(Label1)
        Controls.Add(BtnPreview)
        Controls.Add(BtnInsert)
        Controls.Add(LblMapped)
        Controls.Add(DtGV1)
        Controls.Add(lblPreview)
        Controls.Add(DtGV2)
        Name = "FormTxt"
        Text = "Do Not Call — Import Wizard"
        CType(DtGV1, ComponentModel.ISupportInitialize).EndInit()
        CType(DtGV2, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents DtGV1 As DataGridView
    Friend WithEvents DtGV2 As DataGridView
    Friend WithEvents Button1 As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents BtnPreview As Button
    Friend WithEvents BtnInsert As Button
    Friend WithEvents LblMapped As Label
    Friend WithEvents lblPreview As Label

End Class
