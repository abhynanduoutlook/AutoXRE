<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmLogin
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmLogin))
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.TxtPw = New System.Windows.Forms.TextBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.BtnGo = New System.Windows.Forms.Button()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.TxtUname = New System.Windows.Forms.TextBox()
        Me.BtnUser = New System.Windows.Forms.Button()
        Me.BtnSwitch = New System.Windows.Forms.Button()
        Me.BtnLogin = New System.Windows.Forms.Button()
        Me.LinkLabelUpdate = New System.Windows.Forms.LinkLabel()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BtnCancel
        '
        Me.BtnCancel.BackColor = System.Drawing.Color.Transparent
        Me.BtnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BtnCancel.FlatAppearance.BorderSize = 0
        Me.BtnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red
        Me.BtnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red
        Me.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnCancel.Font = New System.Drawing.Font("Segoe UI Symbol", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.ForeColor = System.Drawing.Color.White
        Me.BtnCancel.Location = New System.Drawing.Point(526, 290)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(75, 37)
        Me.BtnCancel.TabIndex = 5
        Me.BtnCancel.Text = "&Exit"
        Me.BtnCancel.UseVisualStyleBackColor = False
        '
        'TxtPw
        '
        Me.TxtPw.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TxtPw.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtPw.Font = New System.Drawing.Font("Segoe UI Symbol", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtPw.ForeColor = System.Drawing.Color.DarkGray
        Me.TxtPw.Location = New System.Drawing.Point(237, 170)
        Me.TxtPw.Name = "TxtPw"
        Me.TxtPw.Size = New System.Drawing.Size(261, 34)
        Me.TxtPw.TabIndex = 2
        Me.TxtPw.Text = "Password"
        Me.TxtPw.Visible = False
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(39, 93)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(164, 138)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 25
        Me.PictureBox1.TabStop = False
        '
        'Timer1
        '
        Me.Timer1.Interval = 1
        '
        'BtnGo
        '
        Me.BtnGo.BackColor = System.Drawing.Color.White
        Me.BtnGo.BackgroundImage = CType(resources.GetObject("BtnGo.BackgroundImage"), System.Drawing.Image)
        Me.BtnGo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.BtnGo.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.BtnGo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnGo.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnGo.Location = New System.Drawing.Point(461, 169)
        Me.BtnGo.Name = "BtnGo"
        Me.BtnGo.Size = New System.Drawing.Size(37, 29)
        Me.BtnGo.TabIndex = 3
        Me.BtnGo.UseVisualStyleBackColor = False
        Me.BtnGo.Visible = False
        '
        'ImageList1
        '
        Me.ImageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.ImageList1.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        '
        'TxtUname
        '
        Me.TxtUname.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TxtUname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtUname.Font = New System.Drawing.Font("Segoe UI Symbol", 12.0!)
        Me.TxtUname.ForeColor = System.Drawing.Color.DarkGray
        Me.TxtUname.Location = New System.Drawing.Point(237, 202)
        Me.TxtUname.Name = "TxtUname"
        Me.TxtUname.Size = New System.Drawing.Size(261, 34)
        Me.TxtUname.TabIndex = 1
        Me.TxtUname.Text = "User Name "
        Me.TxtUname.Visible = False
        '
        'BtnUser
        '
        Me.BtnUser.BackColor = System.Drawing.Color.Transparent
        Me.BtnUser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnUser.FlatAppearance.BorderSize = 0
        Me.BtnUser.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.BtnUser.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.BtnUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnUser.Font = New System.Drawing.Font("Segoe UI Symbol", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnUser.ForeColor = System.Drawing.Color.White
        Me.BtnUser.Image = CType(resources.GetObject("BtnUser.Image"), System.Drawing.Image)
        Me.BtnUser.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnUser.Location = New System.Drawing.Point(237, 130)
        Me.BtnUser.Name = "BtnUser"
        Me.BtnUser.Size = New System.Drawing.Size(261, 30)
        Me.BtnUser.TabIndex = 0
        Me.BtnUser.Tag = ""
        Me.BtnUser.Text = "&USER NAME"
        Me.BtnUser.UseVisualStyleBackColor = True
        Me.BtnUser.Visible = False
        '
        'BtnSwitch
        '
        Me.BtnSwitch.BackColor = System.Drawing.Color.Transparent
        Me.BtnSwitch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnSwitch.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BtnSwitch.FlatAppearance.BorderSize = 0
        Me.BtnSwitch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.BtnSwitch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.BtnSwitch.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnSwitch.Font = New System.Drawing.Font("Segoe UI Symbol", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSwitch.ForeColor = System.Drawing.Color.White
        Me.BtnSwitch.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnSwitch.Location = New System.Drawing.Point(395, 288)
        Me.BtnSwitch.Name = "BtnSwitch"
        Me.BtnSwitch.Size = New System.Drawing.Size(125, 39)
        Me.BtnSwitch.TabIndex = 4
        Me.BtnSwitch.Text = "&Switch User"
        Me.BtnSwitch.UseVisualStyleBackColor = False
        Me.BtnSwitch.Visible = False
        '
        'BtnLogin
        '
        Me.BtnLogin.BackColor = System.Drawing.Color.Transparent
        Me.BtnLogin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnLogin.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BtnLogin.FlatAppearance.BorderSize = 0
        Me.BtnLogin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnLogin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.BtnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnLogin.Font = New System.Drawing.Font("Segoe UI Symbol", 20.25!)
        Me.BtnLogin.ForeColor = System.Drawing.Color.White
        Me.BtnLogin.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnLogin.Location = New System.Drawing.Point(4, 5)
        Me.BtnLogin.Name = "BtnLogin"
        Me.BtnLogin.Size = New System.Drawing.Size(101, 53)
        Me.BtnLogin.TabIndex = 6
        Me.BtnLogin.Text = "Login"
        Me.BtnLogin.UseVisualStyleBackColor = False
        Me.BtnLogin.Visible = False
        '
        'LinkLabelUpdate
        '
        Me.LinkLabelUpdate.AutoSize = True
        Me.LinkLabelUpdate.BackColor = System.Drawing.Color.Transparent
        Me.LinkLabelUpdate.LinkColor = System.Drawing.Color.Yellow
        Me.LinkLabelUpdate.Location = New System.Drawing.Point(13, 305)
        Me.LinkLabelUpdate.Name = "LinkLabelUpdate"
        Me.LinkLabelUpdate.Size = New System.Drawing.Size(114, 19)
        Me.LinkLabelUpdate.TabIndex = 26
        Me.LinkLabelUpdate.TabStop = True
        Me.LinkLabelUpdate.Text = "Update Software"
        '
        'FrmLogin
        '
        Me.AcceptButton = Me.BtnGo
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.BackgroundImage = Global.RE_X.My.Resources.Resources.formbg_copy
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(603, 330)
        Me.Controls.Add(Me.LinkLabelUpdate)
        Me.Controls.Add(Me.BtnLogin)
        Me.Controls.Add(Me.BtnSwitch)
        Me.Controls.Add(Me.BtnUser)
        Me.Controls.Add(Me.BtnGo)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.TxtPw)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.TxtUname)
        Me.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "FrmLogin"
        Me.Opacity = 0.98R
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Login"
        Me.TransparencyKey = System.Drawing.Color.Fuchsia
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents TxtPw As System.Windows.Forms.TextBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents BtnGo As System.Windows.Forms.Button
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents TxtUname As System.Windows.Forms.TextBox
    Friend WithEvents BtnUser As System.Windows.Forms.Button
    Friend WithEvents BtnSwitch As System.Windows.Forms.Button
    Friend WithEvents BtnLogin As System.Windows.Forms.Button
    Friend WithEvents LinkLabelUpdate As System.Windows.Forms.LinkLabel
End Class
