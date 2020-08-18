<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmClipBoard
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
        Me.TxtClipBoard = New System.Windows.Forms.TextBox()
        Me.BtnCopy = New System.Windows.Forms.Button()
        Me.LblTitle = New System.Windows.Forms.Label()
        Me.BtnOK = New System.Windows.Forms.Button()
        Me.BtnHide = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'TxtClipBoard
        '
        Me.TxtClipBoard.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TxtClipBoard.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TxtClipBoard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtClipBoard.Location = New System.Drawing.Point(8, 42)
        Me.TxtClipBoard.Margin = New System.Windows.Forms.Padding(4)
        Me.TxtClipBoard.MaxLength = 200
        Me.TxtClipBoard.Multiline = True
        Me.TxtClipBoard.Name = "TxtClipBoard"
        Me.TxtClipBoard.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TxtClipBoard.Size = New System.Drawing.Size(647, 334)
        Me.TxtClipBoard.TabIndex = 2
        '
        'BtnCopy
        '
        Me.BtnCopy.BackColor = System.Drawing.Color.White
        Me.BtnCopy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnCopy.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCopy.Location = New System.Drawing.Point(416, 384)
        Me.BtnCopy.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnCopy.Name = "BtnCopy"
        Me.BtnCopy.Size = New System.Drawing.Size(155, 31)
        Me.BtnCopy.TabIndex = 31
        Me.BtnCopy.Text = "&Copy To ClipBoard"
        Me.BtnCopy.UseVisualStyleBackColor = False
        '
        'LblTitle
        '
        Me.LblTitle.AutoSize = True
        Me.LblTitle.BackColor = System.Drawing.Color.Transparent
        Me.LblTitle.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblTitle.ForeColor = System.Drawing.Color.Black
        Me.LblTitle.Location = New System.Drawing.Point(8, 11)
        Me.LblTitle.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblTitle.Name = "LblTitle"
        Me.LblTitle.Size = New System.Drawing.Size(22, 23)
        Me.LblTitle.TabIndex = 32
        Me.LblTitle.Text = "..."
        '
        'BtnOK
        '
        Me.BtnOK.BackColor = System.Drawing.Color.White
        Me.BtnOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnOK.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnOK.Location = New System.Drawing.Point(580, 384)
        Me.BtnOK.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnOK.Name = "BtnOK"
        Me.BtnOK.Size = New System.Drawing.Size(76, 31)
        Me.BtnOK.TabIndex = 33
        Me.BtnOK.Text = "&OK"
        Me.BtnOK.UseVisualStyleBackColor = False
        '
        'BtnHide
        '
        Me.BtnHide.BackColor = System.Drawing.Color.White
        Me.BtnHide.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnHide.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnHide.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnHide.Location = New System.Drawing.Point(332, 384)
        Me.BtnHide.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnHide.Name = "BtnHide"
        Me.BtnHide.Size = New System.Drawing.Size(76, 31)
        Me.BtnHide.TabIndex = 34
        Me.BtnHide.Text = "&Hide"
        Me.BtnHide.UseVisualStyleBackColor = False
        Me.BtnHide.Visible = False
        '
        'FrmClipBoard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.BackgroundImage = Global.RE_X.My.Resources.Resources.formbg_copy
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(664, 425)
        Me.ControlBox = False
        Me.Controls.Add(Me.BtnHide)
        Me.Controls.Add(Me.BtnOK)
        Me.Controls.Add(Me.LblTitle)
        Me.Controls.Add(Me.BtnCopy)
        Me.Controls.Add(Me.TxtClipBoard)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmClipBoard"
        Me.Opacity = 0.97R
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TxtClipBoard As System.Windows.Forms.TextBox
    Friend WithEvents BtnCopy As System.Windows.Forms.Button
    Friend WithEvents LblTitle As System.Windows.Forms.Label
    Friend WithEvents BtnOK As System.Windows.Forms.Button
    Friend WithEvents BtnHide As Button
End Class
