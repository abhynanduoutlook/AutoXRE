<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmTallySparePur
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
        Me.LblName = New System.Windows.Forms.Label()
        Me.PanelControl = New System.Windows.Forms.Panel()
        Me.BtnMaximize = New System.Windows.Forms.Button()
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.BtnMinimize = New System.Windows.Forms.Button()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.BtnFind = New System.Windows.Forms.Button()
        Me.BtnImport = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TxtSearch = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.CmbSearchBy = New System.Windows.Forms.ComboBox()
        Me.RbtAll = New System.Windows.Forms.RadioButton()
        Me.RbtActive = New System.Windows.Forms.RadioButton()
        Me.RbtExcluded = New System.Windows.Forms.RadioButton()
        Me.PanelPaint = New System.Windows.Forms.Panel()
        Me.LblProgressBar = New System.Windows.Forms.Label()
        Me.LblPaint = New System.Windows.Forms.Label()
        Me.LblStatusImport = New System.Windows.Forms.Label()
        Me.RbtImported = New System.Windows.Forms.RadioButton()
        Me.DtpTo = New System.Windows.Forms.DateTimePicker()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.DtpFrom = New System.Windows.Forms.DateTimePicker()
        Me.BtnExport = New System.Windows.Forms.Button()
        Me.SaveDlg = New System.Windows.Forms.SaveFileDialog()
        Me.RbtCancelled = New System.Windows.Forms.RadioButton()
        Me.ListView2 = New System.Windows.Forms.ListView()
        Me.PnlImport = New System.Windows.Forms.Panel()
        Me.RbtMissing = New System.Windows.Forms.RadioButton()
        Me.PanelSparePurchase = New System.Windows.Forms.Panel()
        Me.BtnClosePanel = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Btnupdate = New System.Windows.Forms.Button()
        Me.Txtupdate = New System.Windows.Forms.TextBox()
        Me.PanelPlsWait = New System.Windows.Forms.Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.PanelControl.SuspendLayout()
        Me.PanelPaint.SuspendLayout()
        Me.PnlImport.SuspendLayout()
        Me.PanelSparePurchase.SuspendLayout()
        Me.PanelPlsWait.SuspendLayout()
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
        Me.LblName.Size = New System.Drawing.Size(140, 23)
        Me.LblName.TabIndex = 0
        Me.LblName.Text = "Tally Importing"
        '
        'PanelControl
        '
        Me.PanelControl.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PanelControl.BackColor = System.Drawing.Color.Transparent
        Me.PanelControl.Controls.Add(Me.BtnMaximize)
        Me.PanelControl.Controls.Add(Me.BtnClose)
        Me.PanelControl.Controls.Add(Me.BtnMinimize)
        Me.PanelControl.Location = New System.Drawing.Point(1244, 3)
        Me.PanelControl.Name = "PanelControl"
        Me.PanelControl.Size = New System.Drawing.Size(111, 24)
        Me.PanelControl.TabIndex = 27
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
        Me.BtnCancel.Location = New System.Drawing.Point(1272, 627)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(75, 30)
        Me.BtnCancel.TabIndex = 24
        Me.BtnCancel.Text = "&Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = False
        '
        'ListView1
        '
        Me.ListView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ListView1.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListView1.FullRowSelect = True
        Me.ListView1.GridLines = True
        Me.ListView1.Location = New System.Drawing.Point(7, 83)
        Me.ListView1.MultiSelect = False
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(1349, 335)
        Me.ListView1.TabIndex = 16
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'BtnFind
        '
        Me.BtnFind.BackColor = System.Drawing.Color.White
        Me.BtnFind.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnFind.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnFind.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnFind.Location = New System.Drawing.Point(1169, 49)
        Me.BtnFind.Name = "BtnFind"
        Me.BtnFind.Size = New System.Drawing.Size(90, 24)
        Me.BtnFind.TabIndex = 14
        Me.BtnFind.Text = "&Find"
        Me.BtnFind.UseVisualStyleBackColor = False
        '
        'BtnImport
        '
        Me.BtnImport.BackColor = System.Drawing.Color.White
        Me.BtnImport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnImport.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnImport.Location = New System.Drawing.Point(0, 0)
        Me.BtnImport.Name = "BtnImport"
        Me.BtnImport.Size = New System.Drawing.Size(127, 30)
        Me.BtnImport.TabIndex = 21
        Me.BtnImport.Text = "&Import to tally"
        Me.BtnImport.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(1021, 35)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(80, 19)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Search for :"
        '
        'TxtSearch
        '
        Me.TxtSearch.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TxtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtSearch.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold)
        Me.TxtSearch.Location = New System.Drawing.Point(1022, 50)
        Me.TxtSearch.MaxLength = 15
        Me.TxtSearch.Name = "TxtSearch"
        Me.TxtSearch.Size = New System.Drawing.Size(141, 26)
        Me.TxtSearch.TabIndex = 13
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label8.ForeColor = System.Drawing.Color.White
        Me.Label8.Location = New System.Drawing.Point(874, 34)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(77, 19)
        Me.Label8.TabIndex = 10
        Me.Label8.Text = "Search by :"
        '
        'CmbSearchBy
        '
        Me.CmbSearchBy.BackColor = System.Drawing.Color.WhiteSmoke
        Me.CmbSearchBy.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold)
        Me.CmbSearchBy.FormattingEnabled = True
        Me.CmbSearchBy.Items.AddRange(New Object() {"Supplier_Invoice_No", "GRN_No"})
        Me.CmbSearchBy.Location = New System.Drawing.Point(877, 50)
        Me.CmbSearchBy.Name = "CmbSearchBy"
        Me.CmbSearchBy.Size = New System.Drawing.Size(139, 27)
        Me.CmbSearchBy.TabIndex = 11
        '
        'RbtAll
        '
        Me.RbtAll.AutoSize = True
        Me.RbtAll.BackColor = System.Drawing.Color.Transparent
        Me.RbtAll.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RbtAll.ForeColor = System.Drawing.Color.White
        Me.RbtAll.Location = New System.Drawing.Point(13, 50)
        Me.RbtAll.Margin = New System.Windows.Forms.Padding(4)
        Me.RbtAll.Name = "RbtAll"
        Me.RbtAll.Size = New System.Drawing.Size(48, 24)
        Me.RbtAll.TabIndex = 0
        Me.RbtAll.Text = "&All"
        Me.RbtAll.UseVisualStyleBackColor = False
        '
        'RbtActive
        '
        Me.RbtActive.AutoSize = True
        Me.RbtActive.BackColor = System.Drawing.Color.Transparent
        Me.RbtActive.Checked = True
        Me.RbtActive.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RbtActive.ForeColor = System.Drawing.Color.White
        Me.RbtActive.Location = New System.Drawing.Point(158, 50)
        Me.RbtActive.Margin = New System.Windows.Forms.Padding(4)
        Me.RbtActive.Name = "RbtActive"
        Me.RbtActive.Size = New System.Drawing.Size(72, 24)
        Me.RbtActive.TabIndex = 2
        Me.RbtActive.TabStop = True
        Me.RbtActive.Text = "&Active"
        Me.RbtActive.UseVisualStyleBackColor = False
        '
        'RbtExcluded
        '
        Me.RbtExcluded.AutoSize = True
        Me.RbtExcluded.BackColor = System.Drawing.Color.Transparent
        Me.RbtExcluded.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RbtExcluded.ForeColor = System.Drawing.Color.White
        Me.RbtExcluded.Location = New System.Drawing.Point(158, 13)
        Me.RbtExcluded.Margin = New System.Windows.Forms.Padding(4)
        Me.RbtExcluded.Name = "RbtExcluded"
        Me.RbtExcluded.Size = New System.Drawing.Size(93, 24)
        Me.RbtExcluded.TabIndex = 3
        Me.RbtExcluded.Text = "&Warranty"
        Me.RbtExcluded.UseVisualStyleBackColor = False
        Me.RbtExcluded.Visible = False
        '
        'PanelPaint
        '
        Me.PanelPaint.BackColor = System.Drawing.Color.Transparent
        Me.PanelPaint.BackgroundImage = Global.RE_X.My.Resources.Resources.formbg_copy
        Me.PanelPaint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PanelPaint.Controls.Add(Me.LblProgressBar)
        Me.PanelPaint.Controls.Add(Me.LblPaint)
        Me.PanelPaint.Location = New System.Drawing.Point(421, 331)
        Me.PanelPaint.Name = "PanelPaint"
        Me.PanelPaint.Size = New System.Drawing.Size(481, 156)
        Me.PanelPaint.TabIndex = 26
        Me.PanelPaint.Visible = False
        '
        'LblProgressBar
        '
        Me.LblProgressBar.AutoSize = True
        Me.LblProgressBar.ForeColor = System.Drawing.SystemColors.Control
        Me.LblProgressBar.Location = New System.Drawing.Point(44, 78)
        Me.LblProgressBar.Name = "LblProgressBar"
        Me.LblProgressBar.Size = New System.Drawing.Size(0, 19)
        Me.LblProgressBar.TabIndex = 1
        '
        'LblPaint
        '
        Me.LblPaint.AutoSize = True
        Me.LblPaint.BackColor = System.Drawing.Color.Transparent
        Me.LblPaint.Font = New System.Drawing.Font("Segoe UI Symbol", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblPaint.ForeColor = System.Drawing.Color.White
        Me.LblPaint.Location = New System.Drawing.Point(3, 3)
        Me.LblPaint.Name = "LblPaint"
        Me.LblPaint.Size = New System.Drawing.Size(127, 23)
        Me.LblPaint.TabIndex = 0
        Me.LblPaint.Text = "Import Status"
        '
        'LblStatusImport
        '
        Me.LblStatusImport.AutoSize = True
        Me.LblStatusImport.BackColor = System.Drawing.Color.Transparent
        Me.LblStatusImport.ForeColor = System.Drawing.Color.White
        Me.LblStatusImport.Location = New System.Drawing.Point(23, 627)
        Me.LblStatusImport.Name = "LblStatusImport"
        Me.LblStatusImport.Size = New System.Drawing.Size(55, 19)
        Me.LblStatusImport.TabIndex = 22
        Me.LblStatusImport.Text = "Status :"
        '
        'RbtImported
        '
        Me.RbtImported.AutoSize = True
        Me.RbtImported.BackColor = System.Drawing.Color.Transparent
        Me.RbtImported.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RbtImported.ForeColor = System.Drawing.Color.White
        Me.RbtImported.Location = New System.Drawing.Point(66, 50)
        Me.RbtImported.Margin = New System.Windows.Forms.Padding(4)
        Me.RbtImported.Name = "RbtImported"
        Me.RbtImported.Size = New System.Drawing.Size(93, 24)
        Me.RbtImported.TabIndex = 1
        Me.RbtImported.Text = "&Imported"
        Me.RbtImported.UseVisualStyleBackColor = False
        '
        'DtpTo
        '
        Me.DtpTo.CalendarMonthBackground = System.Drawing.Color.WhiteSmoke
        Me.DtpTo.CustomFormat = "dd-MMM-yyyy"
        Me.DtpTo.Font = New System.Drawing.Font("Segoe UI Symbol", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DtpTo.Location = New System.Drawing.Point(758, 49)
        Me.DtpTo.Name = "DtpTo"
        Me.DtpTo.Size = New System.Drawing.Size(113, 26)
        Me.DtpTo.TabIndex = 9
        Me.DtpTo.Visible = False
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.BackColor = System.Drawing.Color.Transparent
        Me.Label13.ForeColor = System.Drawing.Color.White
        Me.Label13.Location = New System.Drawing.Point(729, 53)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(31, 19)
        Me.Label13.TabIndex = 8
        Me.Label13.Text = "To :"
        Me.Label13.Visible = False
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.BackColor = System.Drawing.Color.Transparent
        Me.Label14.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.Color.White
        Me.Label14.Location = New System.Drawing.Point(565, 53)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(48, 19)
        Me.Label14.TabIndex = 6
        Me.Label14.Text = "From :"
        Me.Label14.Visible = False
        '
        'DtpFrom
        '
        Me.DtpFrom.CalendarMonthBackground = System.Drawing.Color.WhiteSmoke
        Me.DtpFrom.CustomFormat = "dd-MMM-yyyy"
        Me.DtpFrom.Font = New System.Drawing.Font("Segoe UI Symbol", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DtpFrom.Location = New System.Drawing.Point(610, 49)
        Me.DtpFrom.Name = "DtpFrom"
        Me.DtpFrom.Size = New System.Drawing.Size(113, 26)
        Me.DtpFrom.TabIndex = 7
        Me.DtpFrom.Visible = False
        '
        'BtnExport
        '
        Me.BtnExport.BackColor = System.Drawing.Color.White
        Me.BtnExport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnExport.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnExport.Location = New System.Drawing.Point(1264, 49)
        Me.BtnExport.Name = "BtnExport"
        Me.BtnExport.Size = New System.Drawing.Size(90, 24)
        Me.BtnExport.TabIndex = 15
        Me.BtnExport.Text = "&Export"
        Me.BtnExport.UseVisualStyleBackColor = False
        '
        'SaveDlg
        '
        Me.SaveDlg.Filter = """Excel files (*.xls)|*.xls|All files (*.*)|*.*"""
        '
        'RbtCancelled
        '
        Me.RbtCancelled.AutoSize = True
        Me.RbtCancelled.BackColor = System.Drawing.Color.Transparent
        Me.RbtCancelled.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RbtCancelled.ForeColor = System.Drawing.Color.White
        Me.RbtCancelled.Location = New System.Drawing.Point(246, 13)
        Me.RbtCancelled.Margin = New System.Windows.Forms.Padding(4)
        Me.RbtCancelled.Name = "RbtCancelled"
        Me.RbtCancelled.Size = New System.Drawing.Size(96, 24)
        Me.RbtCancelled.TabIndex = 4
        Me.RbtCancelled.Text = "&Cancelled"
        Me.RbtCancelled.UseVisualStyleBackColor = False
        Me.RbtCancelled.Visible = False
        '
        'ListView2
        '
        Me.ListView2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ListView2.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListView2.FullRowSelect = True
        Me.ListView2.GridLines = True
        Me.ListView2.Location = New System.Drawing.Point(6, 424)
        Me.ListView2.MultiSelect = False
        Me.ListView2.Name = "ListView2"
        Me.ListView2.Size = New System.Drawing.Size(1349, 197)
        Me.ListView2.TabIndex = 30
        Me.ListView2.UseCompatibleStateImageBehavior = False
        Me.ListView2.View = System.Windows.Forms.View.Details
        '
        'PnlImport
        '
        Me.PnlImport.BackColor = System.Drawing.Color.Transparent
        Me.PnlImport.Controls.Add(Me.BtnImport)
        Me.PnlImport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.PnlImport.Location = New System.Drawing.Point(1132, 627)
        Me.PnlImport.Name = "PnlImport"
        Me.PnlImport.Size = New System.Drawing.Size(127, 30)
        Me.PnlImport.TabIndex = 31
        '
        'RbtMissing
        '
        Me.RbtMissing.AutoSize = True
        Me.RbtMissing.BackColor = System.Drawing.Color.Transparent
        Me.RbtMissing.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RbtMissing.ForeColor = System.Drawing.Color.White
        Me.RbtMissing.Location = New System.Drawing.Point(227, 50)
        Me.RbtMissing.Margin = New System.Windows.Forms.Padding(4)
        Me.RbtMissing.Name = "RbtMissing"
        Me.RbtMissing.Size = New System.Drawing.Size(82, 24)
        Me.RbtMissing.TabIndex = 5
        Me.RbtMissing.Text = "&Missing"
        Me.RbtMissing.UseVisualStyleBackColor = False
        '
        'PanelSparePurchase
        '
        Me.PanelSparePurchase.BackgroundImage = Global.RE_X.My.Resources.Resources.formbgwp
        Me.PanelSparePurchase.Controls.Add(Me.BtnClosePanel)
        Me.PanelSparePurchase.Controls.Add(Me.Label3)
        Me.PanelSparePurchase.Controls.Add(Me.Label1)
        Me.PanelSparePurchase.Controls.Add(Me.Btnupdate)
        Me.PanelSparePurchase.Controls.Add(Me.Txtupdate)
        Me.PanelSparePurchase.Location = New System.Drawing.Point(520, 209)
        Me.PanelSparePurchase.Name = "PanelSparePurchase"
        Me.PanelSparePurchase.Size = New System.Drawing.Size(294, 100)
        Me.PanelSparePurchase.TabIndex = 33
        Me.PanelSparePurchase.Visible = False
        '
        'BtnClosePanel
        '
        Me.BtnClosePanel.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.BtnClosePanel.FlatAppearance.BorderSize = 0
        Me.BtnClosePanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnClosePanel.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClosePanel.ForeColor = System.Drawing.Color.White
        Me.BtnClosePanel.Location = New System.Drawing.Point(257, 2)
        Me.BtnClosePanel.Name = "BtnClosePanel"
        Me.BtnClosePanel.Size = New System.Drawing.Size(35, 21)
        Me.BtnClosePanel.TabIndex = 4
        Me.BtnClosePanel.Text = "x"
        Me.BtnClosePanel.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.BtnClosePanel.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(14, 27)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(163, 19)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Please Enter Invoice No :"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(19, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(0, 19)
        Me.Label1.TabIndex = 2
        '
        'Btnupdate
        '
        Me.Btnupdate.Location = New System.Drawing.Point(203, 55)
        Me.Btnupdate.Name = "Btnupdate"
        Me.Btnupdate.Size = New System.Drawing.Size(75, 23)
        Me.Btnupdate.TabIndex = 1
        Me.Btnupdate.Text = "OK"
        Me.Btnupdate.UseVisualStyleBackColor = True
        '
        'Txtupdate
        '
        Me.Txtupdate.Location = New System.Drawing.Point(19, 54)
        Me.Txtupdate.Name = "Txtupdate"
        Me.Txtupdate.Size = New System.Drawing.Size(177, 26)
        Me.Txtupdate.TabIndex = 0
        '
        'PanelPlsWait
        '
        Me.PanelPlsWait.BackgroundImage = Global.RE_X.My.Resources.Resources.formbgwp
        Me.PanelPlsWait.Controls.Add(Me.Label4)
        Me.PanelPlsWait.Location = New System.Drawing.Point(557, 331)
        Me.PanelPlsWait.Name = "PanelPlsWait"
        Me.PanelPlsWait.Size = New System.Drawing.Size(228, 68)
        Me.PanelPlsWait.TabIndex = 34
        Me.PanelPlsWait.Visible = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(64, 25)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(94, 19)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Please Wait ..."
        '
        'FrmTallySparePur
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.BackgroundImage = Global.RE_X.My.Resources.Resources.formbg_copy
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(1359, 669)
        Me.Controls.Add(Me.PanelPlsWait)
        Me.Controls.Add(Me.PanelSparePurchase)
        Me.Controls.Add(Me.ListView2)
        Me.Controls.Add(Me.RbtCancelled)
        Me.Controls.Add(Me.RbtMissing)
        Me.Controls.Add(Me.BtnExport)
        Me.Controls.Add(Me.DtpTo)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.DtpFrom)
        Me.Controls.Add(Me.RbtImported)
        Me.Controls.Add(Me.LblStatusImport)
        Me.Controls.Add(Me.RbtExcluded)
        Me.Controls.Add(Me.RbtActive)
        Me.Controls.Add(Me.RbtAll)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TxtSearch)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.CmbSearchBy)
        Me.Controls.Add(Me.BtnFind)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.ListView1)
        Me.Controls.Add(Me.PanelControl)
        Me.Controls.Add(Me.LblName)
        Me.Controls.Add(Me.PanelPaint)
        Me.Controls.Add(Me.PnlImport)
        Me.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.ImeMode = System.Windows.Forms.ImeMode.[On]
        Me.KeyPreview = True
        Me.Name = "FrmTallySparePur"
        Me.Opacity = 0.98R
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "-"
        Me.TransparencyKey = System.Drawing.Color.Fuchsia
        Me.PanelControl.ResumeLayout(False)
        Me.PanelPaint.ResumeLayout(False)
        Me.PanelPaint.PerformLayout()
        Me.PnlImport.ResumeLayout(False)
        Me.PanelSparePurchase.ResumeLayout(False)
        Me.PanelSparePurchase.PerformLayout()
        Me.PanelPlsWait.ResumeLayout(False)
        Me.PanelPlsWait.PerformLayout()
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
    Friend WithEvents BtnFind As System.Windows.Forms.Button
    Friend WithEvents BtnImport As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TxtSearch As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents CmbSearchBy As System.Windows.Forms.ComboBox
    Friend WithEvents RbtAll As System.Windows.Forms.RadioButton
    Friend WithEvents RbtActive As System.Windows.Forms.RadioButton
    Friend WithEvents RbtExcluded As System.Windows.Forms.RadioButton
    Friend WithEvents PanelPaint As System.Windows.Forms.Panel
    Friend WithEvents LblPaint As System.Windows.Forms.Label
    Friend WithEvents LblStatusImport As System.Windows.Forms.Label
    Friend WithEvents RbtImported As System.Windows.Forms.RadioButton
    Friend WithEvents DtpTo As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents DtpFrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents BtnExport As System.Windows.Forms.Button
    Friend WithEvents SaveDlg As System.Windows.Forms.SaveFileDialog
    Friend WithEvents RbtCancelled As System.Windows.Forms.RadioButton
    Friend WithEvents ListView2 As ListView
    Friend WithEvents LblProgressBar As Label
    Friend WithEvents PnlImport As Panel
    Friend WithEvents RbtMissing As RadioButton
    Friend WithEvents PanelSparePurchase As Panel
    Friend WithEvents BtnClosePanel As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Btnupdate As Button
    Friend WithEvents Txtupdate As TextBox
    Friend WithEvents PanelPlsWait As Panel
    Friend WithEvents Label4 As Label
End Class
