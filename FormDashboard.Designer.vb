<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormDashboard
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
        ' ── Declare all controls ────────────────────────────────────────────
        PanelHeader = New Panel()
        LblTitle = New Label()
        LblLastUpdated = New Label()
        BtnRefresh = New FontAwesome.Sharp.IconButton()

        PanelFooter = New Panel()
        LblFooter = New Label()

        PanelSidebar = New Panel()
        LblViewsTitle = New Label()
        PanelViewsList = New Panel()
        PbSidebar = New ProgressBar()

        PanelContent = New Panel()
        PanelKPIs = New Panel()
        LblViewName = New Label()
        LblRowCount = New Label()
        PbLoading = New ProgressBar()
        DgvData = New DataGridView()

        ' ── Suspend layouts ─────────────────────────────────────────────────
        PanelHeader.SuspendLayout()
        PanelFooter.SuspendLayout()
        PanelSidebar.SuspendLayout()
        PanelContent.SuspendLayout()
        CType(DgvData, System.ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()

        ' ════════════════════════════════════════════════════════════════════
        '  Header
        ' ════════════════════════════════════════════════════════════════════
        PanelHeader.BackColor = Color.FromArgb(CByte(10), CByte(18), CByte(42))
        PanelHeader.Dock = DockStyle.Top
        PanelHeader.Height = 65
        PanelHeader.Padding = New Padding(20, 0, 20, 0)
        PanelHeader.Name = "PanelHeader"
        PanelHeader.Controls.Add(BtnRefresh)
        PanelHeader.Controls.Add(LblLastUpdated)
        PanelHeader.Controls.Add(LblTitle)

        LblTitle.Text = "📊  ReportDB  Dashboard"
        LblTitle.Font = New Font("Segoe UI", 16, FontStyle.Bold)
        LblTitle.ForeColor = Color.FromArgb(CByte(192), CByte(240), CByte(255))
        LblTitle.Location = New Point(20, 16)
        LblTitle.AutoSize = True
        LblTitle.Name = "LblTitle"

        LblLastUpdated.Text = "Actualizado: —"
        LblLastUpdated.Font = New Font("Segoe UI", 8.5F)
        LblLastUpdated.ForeColor = Color.FromArgb(CByte(100), CByte(130), CByte(175))
        LblLastUpdated.Location = New Point(355, 24)
        LblLastUpdated.AutoSize = True
        LblLastUpdated.Name = "LblLastUpdated"

        BtnRefresh.Text = "  Refresh"
        BtnRefresh.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        BtnRefresh.ForeColor = Color.White
        BtnRefresh.BackColor = Color.FromArgb(CByte(37), CByte(99), CByte(235))
        BtnRefresh.FlatStyle = FlatStyle.Flat
        BtnRefresh.FlatAppearance.BorderSize = 0
        BtnRefresh.IconChar = FontAwesome.Sharp.IconChar.RotateRight
        BtnRefresh.IconColor = Color.White
        BtnRefresh.IconFont = FontAwesome.Sharp.IconFont.Auto
        BtnRefresh.IconSize = 16
        BtnRefresh.ImageAlign = ContentAlignment.MiddleLeft
        BtnRefresh.TextImageRelation = TextImageRelation.ImageBeforeText
        BtnRefresh.Size = New Size(115, 36)
        BtnRefresh.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        BtnRefresh.Location = New Point(1060, 15)
        BtnRefresh.Cursor = Cursors.Hand
        BtnRefresh.Name = "BtnRefresh"

        ' ════════════════════════════════════════════════════════════════════
        '  Footer
        ' ════════════════════════════════════════════════════════════════════
        PanelFooter.BackColor = Color.FromArgb(CByte(10), CByte(18), CByte(42))
        PanelFooter.Dock = DockStyle.Bottom
        PanelFooter.Height = 30
        PanelFooter.Name = "PanelFooter"
        PanelFooter.Controls.Add(LblFooter)

        LblFooter.Dock = DockStyle.Fill
        LblFooter.Text = "@2025 spalacio  —  ReportDB Dashboard"
        LblFooter.Font = New Font("Segoe UI", 8)
        LblFooter.ForeColor = Color.FromArgb(CByte(70), CByte(95), CByte(140))
        LblFooter.TextAlign = ContentAlignment.MiddleCenter
        LblFooter.Name = "LblFooter"

        ' ════════════════════════════════════════════════════════════════════
        '  Sidebar
        ' ════════════════════════════════════════════════════════════════════
        PanelSidebar.BackColor = Color.FromArgb(CByte(14), CByte(24), CByte(50))
        PanelSidebar.Dock = DockStyle.Left
        PanelSidebar.Width = 220
        PanelSidebar.Name = "PanelSidebar"
        PanelSidebar.Controls.Add(PanelViewsList)
        PanelSidebar.Controls.Add(PbSidebar)
        PanelSidebar.Controls.Add(LblViewsTitle)

        LblViewsTitle.Text = "   VISTAS  (ReportDB)"
        LblViewsTitle.Font = New Font("Segoe UI", 7.5F, FontStyle.Bold)
        LblViewsTitle.ForeColor = Color.FromArgb(CByte(90), CByte(120), CByte(170))
        LblViewsTitle.Dock = DockStyle.Top
        LblViewsTitle.Height = 38
        LblViewsTitle.TextAlign = ContentAlignment.MiddleLeft
        LblViewsTitle.Padding = New Padding(6, 0, 0, 0)
        LblViewsTitle.Name = "LblViewsTitle"

        PbSidebar.Dock = DockStyle.Top
        PbSidebar.Height = 3
        PbSidebar.Style = ProgressBarStyle.Marquee
        PbSidebar.MarqueeAnimationSpeed = 30
        PbSidebar.Visible = False
        PbSidebar.Name = "PbSidebar"

        PanelViewsList.Dock = DockStyle.Fill
        PanelViewsList.AutoScroll = True
        PanelViewsList.BackColor = Color.FromArgb(CByte(14), CByte(24), CByte(50))
        PanelViewsList.Padding = New Padding(0, 4, 0, 4)
        PanelViewsList.Name = "PanelViewsList"

        ' ════════════════════════════════════════════════════════════════════
        '  Content
        ' ════════════════════════════════════════════════════════════════════
        PanelContent.BackColor = Color.FromArgb(CByte(20), CByte(34), CByte(65))
        PanelContent.Dock = DockStyle.Fill
        PanelContent.Padding = New Padding(18, 12, 18, 12)
        PanelContent.Name = "PanelContent"
        PanelContent.Controls.Add(DgvData)
        PanelContent.Controls.Add(LblRowCount)
        PanelContent.Controls.Add(PbLoading)
        PanelContent.Controls.Add(LblViewName)
        PanelContent.Controls.Add(PanelKPIs)

        ' — KPI strip ———————————————————————————————————————────————————————
        PanelKPIs.Dock = DockStyle.Top
        PanelKPIs.Height = 115
        PanelKPIs.BackColor = Color.Transparent
        PanelKPIs.Padding = New Padding(0, 0, 0, 8)
        PanelKPIs.Name = "PanelKPIs"

        ' — View name label ——————————————————————————————————————————————————
        LblViewName.Text = "  Selecciona una vista del panel izquierdo"
        LblViewName.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        LblViewName.ForeColor = Color.FromArgb(CByte(170), CByte(210), CByte(255))
        LblViewName.Dock = DockStyle.Top
        LblViewName.Height = 38
        LblViewName.TextAlign = ContentAlignment.MiddleLeft
        LblViewName.Name = "LblViewName"

        ' — Progress bar (loading) ——————————————————————————————————————————
        PbLoading.Dock = DockStyle.Top
        PbLoading.Height = 3
        PbLoading.Style = ProgressBarStyle.Marquee
        PbLoading.MarqueeAnimationSpeed = 30
        PbLoading.Visible = False
        PbLoading.Name = "PbLoading"

        ' — Row count ———————————————————————————————————————————————————————
        LblRowCount.Text = ""
        LblRowCount.Font = New Font("Segoe UI", 8.5F)
        LblRowCount.ForeColor = Color.FromArgb(CByte(100), CByte(140), CByte(200))
        LblRowCount.Dock = DockStyle.Bottom
        LblRowCount.Height = 22
        LblRowCount.TextAlign = ContentAlignment.MiddleRight
        LblRowCount.Name = "LblRowCount"

        ' — DataGridView ————————————————————————————————————————————————————
        DgvData.Dock = DockStyle.Fill
        DgvData.ReadOnly = True
        DgvData.AllowUserToAddRows = False
        DgvData.AllowUserToDeleteRows = False
        DgvData.Name = "DgvData"

        ' ════════════════════════════════════════════════════════════════════
        '  Form
        ' ════════════════════════════════════════════════════════════════════
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(20), CByte(34), CByte(65))
        ClientSize = New Size(1190, 700)
        MinimumSize = New Size(900, 580)
        Name = "FormDashboard"
        Text = "ReportDB — Dashboard"

        ' Controls order matters for dock layout (last added = outermost priority)
        Controls.Add(PanelContent)
        Controls.Add(PanelSidebar)
        Controls.Add(PanelFooter)
        Controls.Add(PanelHeader)

        ' ── Resume layouts ─────────────────────────────────────────────────
        CType(DgvData, System.ComponentModel.ISupportInitialize).EndInit()
        PanelContent.ResumeLayout(False)
        PanelSidebar.ResumeLayout(False)
        PanelFooter.ResumeLayout(False)
        PanelHeader.ResumeLayout(False)
        PanelHeader.PerformLayout()
        ResumeLayout(False)
    End Sub

    ' ── Control declarations ────────────────────────────────────────────────
    Friend PanelHeader As Panel
    Friend LblTitle As Label
    Friend LblLastUpdated As Label
    Friend WithEvents BtnRefresh As FontAwesome.Sharp.IconButton

    Friend PanelFooter As Panel
    Friend LblFooter As Label

    Friend PanelSidebar As Panel
    Friend LblViewsTitle As Label
    Friend PanelViewsList As Panel
    Friend PbSidebar As ProgressBar

    Friend PanelContent As Panel
    Friend PanelKPIs As Panel
    Friend LblViewName As Label
    Friend LblRowCount As Label
    Friend PbLoading As ProgressBar
    Friend DgvData As DataGridView

End Class
