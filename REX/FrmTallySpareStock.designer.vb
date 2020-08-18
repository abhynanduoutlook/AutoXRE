<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmTallySpareStock
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmTallySpareStock))
        Me.LblName = New System.Windows.Forms.Label()
        Me.PanelControl = New System.Windows.Forms.Panel()
        Me.BtnMaximize = New System.Windows.Forms.Button()
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.BtnMinimize = New System.Windows.Forms.Button()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.BtnSave = New System.Windows.Forms.Button()
        Me.BtnFind = New System.Windows.Forms.Button()
        Me.BtnEdit = New System.Windows.Forms.Button()
        Me.LblUserName = New System.Windows.Forms.Label()
        Me.BtnAdd = New System.Windows.Forms.Button()
        Me.TxtName = New System.Windows.Forms.TextBox()
        Me.LblType = New System.Windows.Forms.Label()
        Me.txtCode = New System.Windows.Forms.TextBox()
        Me.Delete = New System.Windows.Forms.Button()
        Me.TxtValueSearch = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtHSNdesc = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TxtHsnCode = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtMrp = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtNDP = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtIGST = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.CmbSearch = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.PanelControl.SuspendLayout()
        Me.SuspendLayout()
        '
        'LblName
        '
        Me.LblName.AutoSize = True
        Me.LblName.BackColor = System.Drawing.Color.Transparent
        Me.LblName.Font = New System.Drawing.Font("Segoe UI Symbol", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblName.ForeColor = System.Drawing.Color.White
        Me.LblName.Location = New System.Drawing.Point(3, 4)
        Me.LblName.Name = "LblName"
        Me.LblName.Size = New System.Drawing.Size(101, 23)
        Me.LblName.TabIndex = 0
        Me.LblName.Text = "Stock Item"
        '
        'PanelControl
        '
        Me.PanelControl.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PanelControl.BackColor = System.Drawing.Color.Transparent
        Me.PanelControl.Controls.Add(Me.BtnMaximize)
        Me.PanelControl.Controls.Add(Me.BtnClose)
        Me.PanelControl.Controls.Add(Me.BtnMinimize)
        Me.PanelControl.Location = New System.Drawing.Point(941, 3)
        Me.PanelControl.Name = "PanelControl"
        Me.PanelControl.Size = New System.Drawing.Size(111, 24)
        Me.PanelControl.TabIndex = 16
        '
        'BtnMaximize
        '
        Me.BtnMaximize.Enabled = False
        Me.BtnMaximize.FlatAppearance.BorderSize = 0
        Me.BtnMaximize.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnMaximize.Font = New System.Drawing.Font("Wingdings", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.BtnMaximize.ForeColor = System.Drawing.Color.White
        Me.BtnMaximize.Location = New System.Drawing.Point(53, 4)
        Me.BtnMaximize.Name = "BtnMaximize"
        Me.BtnMaximize.Size = New System.Drawing.Size(22, 22)
        Me.BtnMaximize.TabIndex = 1
        Me.BtnMaximize.Text = "p"
        Me.BtnMaximize.UseVisualStyleBackColor = True
        '
        'BtnClose
        '
        Me.BtnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.BtnClose.FlatAppearance.BorderSize = 0
        Me.BtnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnClose.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClose.ForeColor = System.Drawing.Color.White
        Me.BtnClose.Location = New System.Drawing.Point(76, 2)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(35, 21)
        Me.BtnClose.TabIndex = 2
        Me.BtnClose.Text = "x"
        Me.BtnClose.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.BtnClose.UseVisualStyleBackColor = False
        '
        'BtnMinimize
        '
        Me.BtnMinimize.FlatAppearance.BorderSize = 0
        Me.BtnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnMinimize.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnMinimize.ForeColor = System.Drawing.Color.White
        Me.BtnMinimize.Location = New System.Drawing.Point(33, 5)
        Me.BtnMinimize.Name = "BtnMinimize"
        Me.BtnMinimize.Size = New System.Drawing.Size(22, 22)
        Me.BtnMinimize.TabIndex = 0
        Me.BtnMinimize.Text = "-"
        Me.BtnMinimize.UseVisualStyleBackColor = True
        '
        'BtnCancel
        '
        Me.BtnCancel.BackColor = System.Drawing.Color.White
        Me.BtnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red
        Me.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnCancel.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(966, 532)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(75, 30)
        Me.BtnCancel.TabIndex = 12
        Me.BtnCancel.Text = "&Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = False
        '
        'ListView1
        '
        Me.ListView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ListView1.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListView1.FullRowSelect = True
        Me.ListView1.GridLines = True
        Me.ListView1.Location = New System.Drawing.Point(246, 86)
        Me.ListView1.MultiSelect = False
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(795, 440)
        Me.ListView1.TabIndex = 12
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'BtnSave
        '
        Me.BtnSave.BackColor = System.Drawing.Color.White
        Me.BtnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnSave.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSave.Location = New System.Drawing.Point(152, 496)
        Me.BtnSave.Name = "BtnSave"
        Me.BtnSave.Size = New System.Drawing.Size(75, 30)
        Me.BtnSave.TabIndex = 9
        Me.BtnSave.Text = "&Save"
        Me.BtnSave.UseVisualStyleBackColor = False
        '
        'BtnFind
        '
        Me.BtnFind.BackColor = System.Drawing.Color.White
        Me.BtnFind.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnFind.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnFind.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnFind.Location = New System.Drawing.Point(951, 53)
        Me.BtnFind.Name = "BtnFind"
        Me.BtnFind.Size = New System.Drawing.Size(90, 27)
        Me.BtnFind.TabIndex = 15
        Me.BtnFind.Text = "&Find"
        Me.BtnFind.UseVisualStyleBackColor = False
        '
        'BtnEdit
        '
        Me.BtnEdit.BackColor = System.Drawing.Color.White
        Me.BtnEdit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnEdit.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnEdit.Location = New System.Drawing.Point(246, 532)
        Me.BtnEdit.Name = "BtnEdit"
        Me.BtnEdit.Size = New System.Drawing.Size(75, 30)
        Me.BtnEdit.TabIndex = 10
        Me.BtnEdit.Text = "&Edit"
        Me.BtnEdit.UseVisualStyleBackColor = False
        '
        'LblUserName
        '
        Me.LblUserName.AutoSize = True
        Me.LblUserName.BackColor = System.Drawing.Color.Transparent
        Me.LblUserName.ForeColor = System.Drawing.Color.White
        Me.LblUserName.Location = New System.Drawing.Point(18, 121)
        Me.LblUserName.Name = "LblUserName"
        Me.LblUserName.Size = New System.Drawing.Size(86, 19)
        Me.LblUserName.TabIndex = 222
        Me.LblUserName.Text = "Part Name : "
        '
        'BtnAdd
        '
        Me.BtnAdd.BackColor = System.Drawing.Color.White
        Me.BtnAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnAdd.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAdd.Location = New System.Drawing.Point(18, 86)
        Me.BtnAdd.Name = "BtnAdd"
        Me.BtnAdd.Size = New System.Drawing.Size(75, 30)
        Me.BtnAdd.TabIndex = 1
        Me.BtnAdd.Text = "&New"
        Me.BtnAdd.UseVisualStyleBackColor = False
        '
        'TxtName
        '
        Me.TxtName.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TxtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtName.Location = New System.Drawing.Point(18, 143)
        Me.TxtName.MaxLength = 1000
        Me.TxtName.Name = "TxtName"
        Me.TxtName.Size = New System.Drawing.Size(209, 26)
        Me.TxtName.TabIndex = 2
        '
        'LblType
        '
        Me.LblType.AutoSize = True
        Me.LblType.BackColor = System.Drawing.Color.Transparent
        Me.LblType.ForeColor = System.Drawing.Color.White
        Me.LblType.Location = New System.Drawing.Point(18, 172)
        Me.LblType.Name = "LblType"
        Me.LblType.Size = New System.Drawing.Size(70, 19)
        Me.LblType.TabIndex = 4
        Me.LblType.Text = "Part Code"
        '
        'txtCode
        '
        Me.txtCode.Location = New System.Drawing.Point(18, 194)
        Me.txtCode.MaxLength = 1000
        Me.txtCode.Name = "txtCode"
        Me.txtCode.Size = New System.Drawing.Size(209, 26)
        Me.txtCode.TabIndex = 3
        '
        'Delete
        '
        Me.Delete.BackColor = System.Drawing.Color.White
        Me.Delete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Delete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Delete.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Delete.Location = New System.Drawing.Point(327, 532)
        Me.Delete.Name = "Delete"
        Me.Delete.Size = New System.Drawing.Size(75, 30)
        Me.Delete.TabIndex = 11
        Me.Delete.Text = "&Delete"
        Me.Delete.UseVisualStyleBackColor = False
        '
        'TxtValueSearch
        '
        Me.TxtValueSearch.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TxtValueSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtValueSearch.Location = New System.Drawing.Point(832, 54)
        Me.TxtValueSearch.MaxLength = 8
        Me.TxtValueSearch.Name = "TxtValueSearch"
        Me.TxtValueSearch.Size = New System.Drawing.Size(115, 26)
        Me.TxtValueSearch.TabIndex = 14
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(779, 61)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(50, 19)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "Value :"
        '
        'txtHSNdesc
        '
        Me.txtHSNdesc.Location = New System.Drawing.Point(18, 296)
        Me.txtHSNdesc.MaxLength = 1000
        Me.txtHSNdesc.Name = "txtHSNdesc"
        Me.txtHSNdesc.Size = New System.Drawing.Size(209, 26)
        Me.txtHSNdesc.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(18, 274)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(114, 19)
        Me.Label2.TabIndex = 21
        Me.Label2.Text = "HSN Description"
        '
        'TxtHsnCode
        '
        Me.TxtHsnCode.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TxtHsnCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtHsnCode.Location = New System.Drawing.Point(18, 245)
        Me.TxtHsnCode.MaxLength = 1000
        Me.TxtHsnCode.Name = "TxtHsnCode"
        Me.TxtHsnCode.Size = New System.Drawing.Size(209, 26)
        Me.TxtHsnCode.TabIndex = 4
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(18, 223)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(74, 19)
        Me.Label3.TabIndex = 19
        Me.Label3.Text = "HSN Code"
        '
        'txtMrp
        '
        Me.txtMrp.Location = New System.Drawing.Point(18, 398)
        Me.txtMrp.MaxLength = 1000
        Me.txtMrp.Name = "txtMrp"
        Me.txtMrp.Size = New System.Drawing.Size(209, 26)
        Me.txtMrp.TabIndex = 7
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(18, 376)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(39, 19)
        Me.Label5.TabIndex = 25
        Me.Label5.Text = "MRP"
        '
        'txtNDP
        '
        Me.txtNDP.BackColor = System.Drawing.Color.WhiteSmoke
        Me.txtNDP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtNDP.Location = New System.Drawing.Point(18, 347)
        Me.txtNDP.MaxLength = 1000
        Me.txtNDP.Name = "txtNDP"
        Me.txtNDP.Size = New System.Drawing.Size(209, 26)
        Me.txtNDP.TabIndex = 6
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.Location = New System.Drawing.Point(18, 325)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(38, 19)
        Me.Label6.TabIndex = 23
        Me.Label6.Text = "NDP"
        '
        'txtIGST
        '
        Me.txtIGST.Location = New System.Drawing.Point(18, 449)
        Me.txtIGST.MaxLength = 1000
        Me.txtIGST.Name = "txtIGST"
        Me.txtIGST.Size = New System.Drawing.Size(209, 26)
        Me.txtIGST.TabIndex = 8
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.Location = New System.Drawing.Point(18, 427)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(39, 19)
        Me.Label7.TabIndex = 27
        Me.Label7.Text = "IGST"
        '
        'CmbSearch
        '
        Me.CmbSearch.FormattingEnabled = True
        Me.CmbSearch.Items.AddRange(New Object() {"PartName", "PartCode"})
        Me.CmbSearch.Location = New System.Drawing.Point(660, 54)
        Me.CmbSearch.Name = "CmbSearch"
        Me.CmbSearch.Size = New System.Drawing.Size(115, 27)
        Me.CmbSearch.TabIndex = 13
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.ForeColor = System.Drawing.Color.White
        Me.Label8.Location = New System.Drawing.Point(580, 61)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(77, 19)
        Me.Label8.TabIndex = 30
        Me.Label8.Text = "Search by :"
        '
        'FrmTallySpareStock
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(1055, 570)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.CmbSearch)
        Me.Controls.Add(Me.txtIGST)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtMrp)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtNDP)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtHSNdesc)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TxtHsnCode)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TxtValueSearch)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Delete)
        Me.Controls.Add(Me.txtCode)
        Me.Controls.Add(Me.LblType)
        Me.Controls.Add(Me.TxtName)
        Me.Controls.Add(Me.BtnAdd)
        Me.Controls.Add(Me.LblUserName)
        Me.Controls.Add(Me.BtnFind)
        Me.Controls.Add(Me.BtnSave)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.BtnEdit)
        Me.Controls.Add(Me.ListView1)
        Me.Controls.Add(Me.PanelControl)
        Me.Controls.Add(Me.LblName)
        Me.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.ImeMode = System.Windows.Forms.ImeMode.[On]
        Me.KeyPreview = True
        Me.Name = "FrmTallySpareStock"
        Me.Opacity = 0.98R
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "-"
        Me.TransparencyKey = System.Drawing.Color.Fuchsia
        Me.PanelControl.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LblName As System.Windows.Forms.Label
    Friend WithEvents BtnClose As System.Windows.Forms.Button
    Friend WithEvents BtnMinimize As System.Windows.Forms.Button
    Friend WithEvents BtnMaximize As System.Windows.Forms.Button
    Friend WithEvents PanelControl As System.Windows.Forms.Panel
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents BtnSave As System.Windows.Forms.Button
    Friend WithEvents BtnFind As System.Windows.Forms.Button
    Friend WithEvents BtnEdit As System.Windows.Forms.Button
    Friend WithEvents LblUserName As System.Windows.Forms.Label
    Friend WithEvents BtnAdd As System.Windows.Forms.Button
    Friend WithEvents TxtName As System.Windows.Forms.TextBox
    Friend WithEvents LblType As System.Windows.Forms.Label
    Friend WithEvents txtCode As System.Windows.Forms.TextBox
    Friend WithEvents Delete As System.Windows.Forms.Button
    Friend WithEvents TxtValueSearch As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtHSNdesc As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents TxtHsnCode As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtMrp As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents txtNDP As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents txtIGST As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents CmbSearch As ComboBox
    Friend WithEvents Label8 As Label
End Class
