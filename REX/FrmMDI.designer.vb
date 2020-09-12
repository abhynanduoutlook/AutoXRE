<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmMDI
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
        Me.components = New System.ComponentModel.Container()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.PLblUser = New System.Windows.Forms.Label()
        Me.BgWrkSysJobs = New System.ComponentModel.BackgroundWorker()
        Me.BgWorkerLoad = New System.ComponentModel.BackgroundWorker()
        Me.PanelService = New System.Windows.Forms.Panel()
        Me.LblServiceStatus = New System.Windows.Forms.Label()
        Me.LblServiceInfo = New System.Windows.Forms.Label()
        Me.BgWrkApp_Request = New System.ComponentModel.BackgroundWorker()
        Me.TimerApp_Req = New System.Windows.Forms.Timer(Me.components)
        Me.MenuStripMain = New System.Windows.Forms.MenuStrip()
        Me.BtnMenuTemplate = New System.Windows.Forms.Button()
        Me.PanelVmenu = New System.Windows.Forms.Panel()
        Me.BtnMin = New System.Windows.Forms.Button()
        Me.BtnExit = New System.Windows.Forms.Button()
        Me.TimerSendMail = New System.Windows.Forms.Timer(Me.components)
        Me.BgWrkSendMail = New System.ComponentModel.BackgroundWorker()
        Me.TimerSysJobs = New System.Windows.Forms.Timer(Me.components)
        Me.BGWorker = New System.ComponentModel.BackgroundWorker()
        Me.pnelimportStatus = New System.Windows.Forms.Panel()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        Me.PanelService.SuspendLayout()
        Me.pnelimportStatus.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.Teal
        Me.Panel1.Controls.Add(Me.PLblUser)
        Me.Panel1.Location = New System.Drawing.Point(1, 368)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(418, 18)
        Me.Panel1.TabIndex = 0
        '
        'PLblUser
        '
        Me.PLblUser.AutoSize = True
        Me.PLblUser.ForeColor = System.Drawing.Color.White
        Me.PLblUser.Location = New System.Drawing.Point(3, 1)
        Me.PLblUser.Name = "PLblUser"
        Me.PLblUser.Size = New System.Drawing.Size(39, 15)
        Me.PLblUser.TabIndex = 0
        Me.PLblUser.Text = "User : "
        '
        'BgWrkSysJobs
        '
        Me.BgWrkSysJobs.WorkerSupportsCancellation = True
        '
        'BgWorkerLoad
        '
        Me.BgWorkerLoad.WorkerSupportsCancellation = True
        '
        'PanelService
        '
        Me.PanelService.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PanelService.BackColor = System.Drawing.Color.Teal
        Me.PanelService.Controls.Add(Me.LblServiceStatus)
        Me.PanelService.Controls.Add(Me.LblServiceInfo)
        Me.PanelService.Location = New System.Drawing.Point(587, 368)
        Me.PanelService.Name = "PanelService"
        Me.PanelService.Size = New System.Drawing.Size(603, 18)
        Me.PanelService.TabIndex = 7
        '
        'LblServiceStatus
        '
        Me.LblServiceStatus.AutoSize = True
        Me.LblServiceStatus.ForeColor = System.Drawing.Color.White
        Me.LblServiceStatus.Location = New System.Drawing.Point(3, -2)
        Me.LblServiceStatus.Name = "LblServiceStatus"
        Me.LblServiceStatus.Size = New System.Drawing.Size(46, 15)
        Me.LblServiceStatus.TabIndex = 0
        Me.LblServiceStatus.Text = "Status :"
        '
        'LblServiceInfo
        '
        Me.LblServiceInfo.AutoSize = True
        Me.LblServiceInfo.ForeColor = System.Drawing.Color.White
        Me.LblServiceInfo.Location = New System.Drawing.Point(3, 1)
        Me.LblServiceInfo.Name = "LblServiceInfo"
        Me.LblServiceInfo.Size = New System.Drawing.Size(69, 15)
        Me.LblServiceInfo.TabIndex = 0
        Me.LblServiceInfo.Text = "SeviceInfo :"
        Me.LblServiceInfo.Visible = False
        '
        'BgWrkApp_Request
        '
        Me.BgWrkApp_Request.WorkerSupportsCancellation = True
        '
        'TimerApp_Req
        '
        Me.TimerApp_Req.Interval = 10000
        '
        'MenuStripMain
        '
        Me.MenuStripMain.AutoSize = False
        Me.MenuStripMain.BackColor = System.Drawing.Color.DarkCyan
        Me.MenuStripMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.MenuStripMain.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MenuStripMain.GripMargin = New System.Windows.Forms.Padding(0)
        Me.MenuStripMain.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuStripMain.Location = New System.Drawing.Point(0, 0)
        Me.MenuStripMain.Name = "MenuStripMain"
        Me.MenuStripMain.Padding = New System.Windows.Forms.Padding(0)
        Me.MenuStripMain.Size = New System.Drawing.Size(1193, 36)
        Me.MenuStripMain.TabIndex = 2
        Me.MenuStripMain.Text = "MenuStrip1"
        '
        'BtnMenuTemplate
        '
        Me.BtnMenuTemplate.BackColor = System.Drawing.Color.Teal
        Me.BtnMenuTemplate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.BtnMenuTemplate.Enabled = False
        Me.BtnMenuTemplate.FlatAppearance.BorderSize = 0
        Me.BtnMenuTemplate.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkCyan
        Me.BtnMenuTemplate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnMenuTemplate.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnMenuTemplate.ForeColor = System.Drawing.Color.White
        Me.BtnMenuTemplate.Location = New System.Drawing.Point(447, 0)
        Me.BtnMenuTemplate.Name = "BtnMenuTemplate"
        Me.BtnMenuTemplate.Size = New System.Drawing.Size(147, 36)
        Me.BtnMenuTemplate.TabIndex = 3
        Me.BtnMenuTemplate.Text = "Template"
        Me.BtnMenuTemplate.UseVisualStyleBackColor = False
        Me.BtnMenuTemplate.Visible = False
        '
        'PanelVmenu
        '
        Me.PanelVmenu.BackColor = System.Drawing.Color.Teal
        Me.PanelVmenu.Location = New System.Drawing.Point(616, 0)
        Me.PanelVmenu.Name = "PanelVmenu"
        Me.PanelVmenu.Size = New System.Drawing.Size(200, 83)
        Me.PanelVmenu.TabIndex = 4
        Me.PanelVmenu.Visible = False
        '
        'BtnMin
        '
        Me.BtnMin.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnMin.BackColor = System.Drawing.Color.DarkCyan
        Me.BtnMin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.BtnMin.FlatAppearance.BorderSize = 0
        Me.BtnMin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.BtnMin.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnMin.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnMin.ForeColor = System.Drawing.Color.White
        Me.BtnMin.Location = New System.Drawing.Point(1109, 0)
        Me.BtnMin.Margin = New System.Windows.Forms.Padding(0)
        Me.BtnMin.Name = "BtnMin"
        Me.BtnMin.Size = New System.Drawing.Size(30, 36)
        Me.BtnMin.TabIndex = 5
        Me.BtnMin.Text = "__"
        Me.BtnMin.UseVisualStyleBackColor = False
        '
        'BtnExit
        '
        Me.BtnExit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnExit.BackColor = System.Drawing.Color.DarkCyan
        Me.BtnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.BtnExit.FlatAppearance.BorderSize = 0
        Me.BtnExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.BtnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnExit.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnExit.ForeColor = System.Drawing.Color.White
        Me.BtnExit.Location = New System.Drawing.Point(1134, 0)
        Me.BtnExit.Margin = New System.Windows.Forms.Padding(0)
        Me.BtnExit.Name = "BtnExit"
        Me.BtnExit.Size = New System.Drawing.Size(59, 36)
        Me.BtnExit.TabIndex = 6
        Me.BtnExit.Text = "EXIT"
        Me.BtnExit.UseVisualStyleBackColor = False
        '
        'TimerSendMail
        '
        Me.TimerSendMail.Interval = 180000
        '
        'BgWrkSendMail
        '
        Me.BgWrkSendMail.WorkerReportsProgress = True
        Me.BgWrkSendMail.WorkerSupportsCancellation = True
        '
        'TimerSysJobs
        '
        Me.TimerSysJobs.Interval = 1000
        '
        'BGWorker
        '
        '
        'pnelimportStatus
        '
        Me.pnelimportStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnelimportStatus.BackColor = System.Drawing.Color.Teal
        Me.pnelimportStatus.Controls.Add(Me.Label1)
        Me.pnelimportStatus.Controls.Add(Me.TextBox1)
        Me.pnelimportStatus.Location = New System.Drawing.Point(1034, 222)
        Me.pnelimportStatus.Name = "pnelimportStatus"
        Me.pnelimportStatus.Size = New System.Drawing.Size(156, 140)
        Me.pnelimportStatus.TabIndex = 1
        Me.pnelimportStatus.Visible = False
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.Color.Teal
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox1.Location = New System.Drawing.Point(5, 3)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox1.Size = New System.Drawing.Size(142, 134)
        Me.TextBox1.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Location = New System.Drawing.Point(132, 4)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(14, 15)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "X"
        '
        'FrmMDI
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.BackgroundImage = Global.RE_X.My.Resources.Resources.formbgwp
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(1193, 389)
        Me.ControlBox = False
        Me.Controls.Add(Me.pnelimportStatus)
        Me.Controls.Add(Me.BtnMin)
        Me.Controls.Add(Me.BtnExit)
        Me.Controls.Add(Me.PanelVmenu)
        Me.Controls.Add(Me.BtnMenuTemplate)
        Me.Controls.Add(Me.MenuStripMain)
        Me.Controls.Add(Me.PanelService)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.IsMdiContainer = True
        Me.KeyPreview = True
        Me.Name = "FrmMDI"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Tag = "Car Showroom Management System"
        Me.Text = "CSMS"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.PanelService.ResumeLayout(False)
        Me.PanelService.PerformLayout()
        Me.pnelimportStatus.ResumeLayout(False)
        Me.pnelimportStatus.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents PLblUser As System.Windows.Forms.Label
    Friend WithEvents BgWrkSysJobs As System.ComponentModel.BackgroundWorker
    Friend WithEvents BgWorkerLoad As System.ComponentModel.BackgroundWorker
    Friend WithEvents PanelService As System.Windows.Forms.Panel
    Friend WithEvents LblServiceInfo As System.Windows.Forms.Label
    Friend WithEvents LblServiceStatus As System.Windows.Forms.Label
    Friend WithEvents BgWrkApp_Request As System.ComponentModel.BackgroundWorker
    Friend WithEvents TimerApp_Req As System.Windows.Forms.Timer
    Friend WithEvents MenuStripMain As System.Windows.Forms.MenuStrip
    Friend WithEvents BtnMenuTemplate As System.Windows.Forms.Button
    Friend WithEvents PanelVmenu As System.Windows.Forms.Panel
    Friend WithEvents BtnMin As System.Windows.Forms.Button
    Friend WithEvents BtnExit As System.Windows.Forms.Button
    Friend WithEvents TimerSendMail As System.Windows.Forms.Timer
    Friend WithEvents BgWrkSendMail As System.ComponentModel.BackgroundWorker
    Friend WithEvents TimerSysJobs As System.Windows.Forms.Timer
    Friend WithEvents BGWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents pnelimportStatus As Panel
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Label1 As Label
End Class
