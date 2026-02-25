<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ReadBD4
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
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

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        DtGV1 = New DataGridView()
        Button2 = New Button()
        CType(DtGV1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' DtGV1
        ' 
        DtGV1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DtGV1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DtGV1.Location = New Point(12, 72)
        DtGV1.Name = "DtGV1"
        DtGV1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
        DtGV1.Size = New Size(776, 397)
        DtGV1.TabIndex = 1
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(12, 12)
        Button2.Name = "Button2"
        Button2.Size = New Size(112, 27)
        Button2.TabIndex = 4
        Button2.Text = "Read from DB"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' ReadBD4
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(800, 532)
        Controls.Add(Button2)
        Controls.Add(DtGV1)
        Name = "ReadBD4"
        Text = "ReadBD4"
        CType(DtGV1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents DtGV1 As DataGridView
    Friend WithEvents Button2 As Button
End Class
