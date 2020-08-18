<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmCashRegisterDetails
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
        Me.components = New System.ComponentModel.Container()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.PanelControl = New System.Windows.Forms.Panel()
        Me.BtnMaximize = New System.Windows.Forms.Button()
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.BtnMinimize = New System.Windows.Forms.Button()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.TxtSearch = New System.Windows.Forms.TextBox()
        Me.BtnFind = New System.Windows.Forms.Button()
        Me.LblName = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.BtnPrint = New System.Windows.Forms.Button()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.BtnEdit = New System.Windows.Forms.Button()
        Me.BtnPayment = New System.Windows.Forms.Button()
        Me.BtnReceipt = New System.Windows.Forms.Button()
        Me.LblReceipt = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.CmbSearchBy = New System.Windows.Forms.ComboBox()
        Me.BtnExport = New System.Windows.Forms.Button()
        Me.SaveDlg = New System.Windows.Forms.SaveFileDialog()
        Me.DtpFrom = New System.Windows.Forms.DateTimePicker()
        Me.DtpTo = New System.Windows.Forms.DateTimePicker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.CmbBranch = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmb_Type = New System.Windows.Forms.ComboBox()
        Me.ImportBtn = New System.Windows.Forms.Button()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.PanelLoading = New System.Windows.Forms.Panel()
        Me.LblStatus = New System.Windows.Forms.Label()
        Me.Lblimporting = New System.Windows.Forms.Label()
        Me.LblRecDtls = New System.Windows.Forms.Label()
        Me.LblPayDtls = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.TimerFileSearch = New System.Windows.Forms.Timer(Me.components)
        Me.BgWrkSearch_File = New System.ComponentModel.BackgroundWorker()
        Me.RbtnActive = New System.Windows.Forms.RadioButton()
        Me.RbtnAll = New System.Windows.Forms.RadioButton()
        Me.PanelControl.SuspendLayout()
        Me.PanelLoading.SuspendLayout()
        Me.SuspendLayout()
        '
        'ListView1
        '
        Me.ListView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ListView1.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListView1.FullRowSelect = True
        Me.ListView1.GridLines = True
        Me.ListView1.Location = New System.Drawing.Point(12, 82)
        Me.ListView1.MultiSelect = False
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(1316, 426)
        Me.ListView1.TabIndex = 29
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'PanelControl
        '
        Me.PanelControl.BackColor = System.Drawing.Color.Transparent
        Me.PanelControl.Controls.Add(Me.BtnMaximize)
        Me.PanelControl.Controls.Add(Me.BtnClose)
        Me.PanelControl.Controls.Add(Me.BtnMinimize)
        Me.PanelControl.Location = New System.Drawing.Point(1239, 3)
        Me.PanelControl.Name = "PanelControl"
        Me.PanelControl.Size = New System.Drawing.Size(97, 26)
        Me.PanelControl.TabIndex = 28
        '
        'BtnMaximize
        '
        Me.BtnMaximize.Enabled = False
        Me.BtnMaximize.FlatAppearance.BorderSize = 0
        Me.BtnMaximize.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnMaximize.Font = New System.Drawing.Font("Wingdings", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.BtnMaximize.ForeColor = System.Drawing.Color.White
        Me.BtnMaximize.Location = New System.Drawing.Point(42, 3)
        Me.BtnMaximize.Name = "BtnMaximize"
        Me.BtnMaximize.Size = New System.Drawing.Size(19, 23)
        Me.BtnMaximize.TabIndex = 2
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
        Me.BtnClose.Location = New System.Drawing.Point(66, 2)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(31, 22)
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
        Me.BtnMinimize.Location = New System.Drawing.Point(18, 5)
        Me.BtnMinimize.Name = "BtnMinimize"
        Me.BtnMinimize.Size = New System.Drawing.Size(19, 23)
        Me.BtnMinimize.TabIndex = 1
        Me.BtnMinimize.Text = "-"
        Me.BtnMinimize.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.BackColor = System.Drawing.Color.Transparent
        Me.Label10.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label10.ForeColor = System.Drawing.Color.White
        Me.Label10.Location = New System.Drawing.Point(1074, 35)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(80, 19)
        Me.Label10.TabIndex = 13
        Me.Label10.Text = "Search for :"
        '
        'TxtSearch
        '
        Me.TxtSearch.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TxtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtSearch.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold)
        Me.TxtSearch.Location = New System.Drawing.Point(1077, 53)
        Me.TxtSearch.MaxLength = 8
        Me.TxtSearch.Name = "TxtSearch"
        Me.TxtSearch.Size = New System.Drawing.Size(138, 26)
        Me.TxtSearch.TabIndex = 14
        '
        'BtnFind
        '
        Me.BtnFind.BackColor = System.Drawing.Color.White
        Me.BtnFind.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnFind.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnFind.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnFind.Location = New System.Drawing.Point(1221, 54)
        Me.BtnFind.Name = "BtnFind"
        Me.BtnFind.Size = New System.Drawing.Size(106, 24)
        Me.BtnFind.TabIndex = 15
        Me.BtnFind.Text = "&Find"
        Me.BtnFind.UseVisualStyleBackColor = False
        '
        'LblName
        '
        Me.LblName.AutoSize = True
        Me.LblName.BackColor = System.Drawing.Color.Transparent
        Me.LblName.Font = New System.Drawing.Font("Segoe UI Symbol", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblName.ForeColor = System.Drawing.Color.White
        Me.LblName.Location = New System.Drawing.Point(6, 5)
        Me.LblName.Name = "LblName"
        Me.LblName.Size = New System.Drawing.Size(190, 23)
        Me.LblName.TabIndex = 0
        Me.LblName.Text = "Cash Register Details"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.BackColor = System.Drawing.Color.Transparent
        Me.Label14.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.Color.White
        Me.Label14.Location = New System.Drawing.Point(12, 32)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(48, 19)
        Me.Label14.TabIndex = 1
        Me.Label14.Text = "From :"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.BackColor = System.Drawing.Color.Transparent
        Me.Label13.ForeColor = System.Drawing.Color.White
        Me.Label13.Location = New System.Drawing.Point(136, 33)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(31, 19)
        Me.Label13.TabIndex = 3
        Me.Label13.Text = "To :"
        '
        'BtnPrint
        '
        Me.BtnPrint.BackColor = System.Drawing.Color.White
        Me.BtnPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnPrint.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnPrint.Location = New System.Drawing.Point(11, 538)
        Me.BtnPrint.Name = "BtnPrint"
        Me.BtnPrint.Size = New System.Drawing.Size(85, 24)
        Me.BtnPrint.TabIndex = 21
        Me.BtnPrint.Text = "&Print"
        Me.BtnPrint.UseVisualStyleBackColor = False
        '
        'BtnCancel
        '
        Me.BtnCancel.BackColor = System.Drawing.Color.White
        Me.BtnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnCancel.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(1239, 538)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(85, 24)
        Me.BtnCancel.TabIndex = 19
        Me.BtnCancel.Text = "&Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = False
        '
        'BtnEdit
        '
        Me.BtnEdit.BackColor = System.Drawing.Color.White
        Me.BtnEdit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnEdit.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnEdit.Location = New System.Drawing.Point(1148, 539)
        Me.BtnEdit.Name = "BtnEdit"
        Me.BtnEdit.Size = New System.Drawing.Size(85, 24)
        Me.BtnEdit.TabIndex = 18
        Me.BtnEdit.Text = "&Edit"
        Me.BtnEdit.UseVisualStyleBackColor = False
        '
        'BtnPayment
        '
        Me.BtnPayment.BackColor = System.Drawing.Color.White
        Me.BtnPayment.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnPayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnPayment.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnPayment.Location = New System.Drawing.Point(1057, 539)
        Me.BtnPayment.Name = "BtnPayment"
        Me.BtnPayment.Size = New System.Drawing.Size(85, 24)
        Me.BtnPayment.TabIndex = 17
        Me.BtnPayment.Text = "&Payment"
        Me.BtnPayment.UseVisualStyleBackColor = False
        '
        'BtnReceipt
        '
        Me.BtnReceipt.BackColor = System.Drawing.Color.White
        Me.BtnReceipt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnReceipt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnReceipt.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnReceipt.Location = New System.Drawing.Point(966, 540)
        Me.BtnReceipt.Name = "BtnReceipt"
        Me.BtnReceipt.Size = New System.Drawing.Size(85, 24)
        Me.BtnReceipt.TabIndex = 16
        Me.BtnReceipt.Text = "&Receipt"
        Me.BtnReceipt.UseVisualStyleBackColor = False
        '
        'LblReceipt
        '
        Me.LblReceipt.AutoSize = True
        Me.LblReceipt.BackColor = System.Drawing.Color.Transparent
        Me.LblReceipt.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblReceipt.ForeColor = System.Drawing.Color.White
        Me.LblReceipt.Location = New System.Drawing.Point(712, 513)
        Me.LblReceipt.Name = "LblReceipt"
        Me.LblReceipt.Size = New System.Drawing.Size(63, 19)
        Me.LblReceipt.TabIndex = 20
        Me.LblReceipt.Text = "Receipt :"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.Color.Transparent
        Me.Label11.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label11.ForeColor = System.Drawing.Color.White
        Me.Label11.Location = New System.Drawing.Point(932, 35)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(77, 19)
        Me.Label11.TabIndex = 11
        Me.Label11.Text = "Search by :"
        '
        'CmbSearchBy
        '
        Me.CmbSearchBy.BackColor = System.Drawing.Color.WhiteSmoke
        Me.CmbSearchBy.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold)
        Me.CmbSearchBy.FormattingEnabled = True
        Me.CmbSearchBy.Location = New System.Drawing.Point(935, 53)
        Me.CmbSearchBy.Name = "CmbSearchBy"
        Me.CmbSearchBy.Size = New System.Drawing.Size(135, 27)
        Me.CmbSearchBy.TabIndex = 12
        '
        'BtnExport
        '
        Me.BtnExport.BackColor = System.Drawing.Color.White
        Me.BtnExport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnExport.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnExport.Location = New System.Drawing.Point(104, 538)
        Me.BtnExport.Name = "BtnExport"
        Me.BtnExport.Size = New System.Drawing.Size(85, 24)
        Me.BtnExport.TabIndex = 22
        Me.BtnExport.Text = "&Export"
        Me.BtnExport.UseVisualStyleBackColor = False
        '
        'SaveDlg
        '
        Me.SaveDlg.Filter = """Excel files (*.xls)|*.xls|All files (*.*)|*.*"""
        '
        'DtpFrom
        '
        Me.DtpFrom.CustomFormat = "dd-MMM-yyyy "
        Me.DtpFrom.Font = New System.Drawing.Font("Segoe UI Symbol", 8.25!)
        Me.DtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DtpFrom.Location = New System.Drawing.Point(15, 50)
        Me.DtpFrom.Name = "DtpFrom"
        Me.DtpFrom.Size = New System.Drawing.Size(116, 26)
        Me.DtpFrom.TabIndex = 2
        '
        'DtpTo
        '
        Me.DtpTo.CalendarMonthBackground = System.Drawing.Color.WhiteSmoke
        Me.DtpTo.CustomFormat = "dd-MMM-yyyy "
        Me.DtpTo.Font = New System.Drawing.Font("Segoe UI Symbol", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DtpTo.Location = New System.Drawing.Point(139, 50)
        Me.DtpTo.Name = "DtpTo"
        Me.DtpTo.Size = New System.Drawing.Size(116, 26)
        Me.DtpTo.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(679, 34)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(59, 19)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Branch :"
        '
        'CmbBranch
        '
        Me.CmbBranch.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold)
        Me.CmbBranch.FormattingEnabled = True
        Me.CmbBranch.Location = New System.Drawing.Point(681, 53)
        Me.CmbBranch.Name = "CmbBranch"
        Me.CmbBranch.Size = New System.Drawing.Size(121, 27)
        Me.CmbBranch.TabIndex = 8
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(806, 35)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(82, 19)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Entry Type :"
        '
        'cmb_Type
        '
        Me.cmb_Type.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold)
        Me.cmb_Type.FormattingEnabled = True
        Me.cmb_Type.Location = New System.Drawing.Point(808, 54)
        Me.cmb_Type.Name = "cmb_Type"
        Me.cmb_Type.Size = New System.Drawing.Size(121, 27)
        Me.cmb_Type.TabIndex = 10
        '
        'ImportBtn
        '
        Me.ImportBtn.BackColor = System.Drawing.Color.White
        Me.ImportBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ImportBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ImportBtn.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ImportBtn.Location = New System.Drawing.Point(195, 538)
        Me.ImportBtn.Name = "ImportBtn"
        Me.ImportBtn.Size = New System.Drawing.Size(84, 24)
        Me.ImportBtn.TabIndex = 23
        Me.ImportBtn.Text = "&Import"
        Me.ImportBtn.UseVisualStyleBackColor = False
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(30, 52)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(347, 23)
        Me.ProgressBar1.TabIndex = 2
        '
        'PanelLoading
        '
        Me.PanelLoading.BackColor = System.Drawing.Color.Transparent
        Me.PanelLoading.BackgroundImage = Global.RE_X.My.Resources.Resources.formbg_copy
        Me.PanelLoading.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PanelLoading.Controls.Add(Me.LblStatus)
        Me.PanelLoading.Controls.Add(Me.ProgressBar1)
        Me.PanelLoading.Controls.Add(Me.Lblimporting)
        Me.PanelLoading.Location = New System.Drawing.Point(466, 237)
        Me.PanelLoading.Name = "PanelLoading"
        Me.PanelLoading.Size = New System.Drawing.Size(408, 119)
        Me.PanelLoading.TabIndex = 26
        Me.PanelLoading.Visible = False
        '
        'LblStatus
        '
        Me.LblStatus.AutoSize = True
        Me.LblStatus.BackColor = System.Drawing.Color.Transparent
        Me.LblStatus.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblStatus.ForeColor = System.Drawing.Color.White
        Me.LblStatus.Location = New System.Drawing.Point(211, 87)
        Me.LblStatus.Name = "LblStatus"
        Me.LblStatus.Size = New System.Drawing.Size(55, 19)
        Me.LblStatus.TabIndex = 2
        Me.LblStatus.Text = "Status :"
        '
        'Lblimporting
        '
        Me.Lblimporting.AutoSize = True
        Me.Lblimporting.BackColor = System.Drawing.Color.Transparent
        Me.Lblimporting.Font = New System.Drawing.Font("Segoe UI Symbol", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lblimporting.ForeColor = System.Drawing.Color.White
        Me.Lblimporting.Location = New System.Drawing.Point(20, 10)
        Me.Lblimporting.Name = "Lblimporting"
        Me.Lblimporting.Size = New System.Drawing.Size(110, 23)
        Me.Lblimporting.TabIndex = 1
        Me.Lblimporting.Text = "Importing..."
        '
        'LblRecDtls
        '
        Me.LblRecDtls.AutoSize = True
        Me.LblRecDtls.BackColor = System.Drawing.Color.Transparent
        Me.LblRecDtls.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblRecDtls.ForeColor = System.Drawing.Color.White
        Me.LblRecDtls.Location = New System.Drawing.Point(609, 535)
        Me.LblRecDtls.Name = "LblRecDtls"
        Me.LblRecDtls.Size = New System.Drawing.Size(45, 19)
        Me.LblRecDtls.TabIndex = 31
        Me.LblRecDtls.Text = "Recpt"
        Me.LblRecDtls.Visible = False
        '
        'LblPayDtls
        '
        Me.LblPayDtls.AutoSize = True
        Me.LblPayDtls.BackColor = System.Drawing.Color.Transparent
        Me.LblPayDtls.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblPayDtls.ForeColor = System.Drawing.Color.White
        Me.LblPayDtls.Location = New System.Drawing.Point(606, 554)
        Me.LblPayDtls.Name = "LblPayDtls"
        Me.LblPayDtls.Size = New System.Drawing.Size(48, 19)
        Me.LblPayDtls.TabIndex = 31
        Me.LblPayDtls.Text = "Paymt"
        Me.LblPayDtls.Visible = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(526, 535)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(79, 19)
        Me.Label5.TabIndex = 30
        Me.Label5.Text = "Receipt     :"
        Me.Label5.Visible = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.Location = New System.Drawing.Point(527, 555)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(78, 19)
        Me.Label6.TabIndex = 31
        Me.Label6.Text = "Payment   :"
        Me.Label6.Visible = False
        '
        'TimerFileSearch
        '
        '
        'BgWrkSearch_File
        '
        Me.BgWrkSearch_File.WorkerSupportsCancellation = True
        '
        'RbtnActive
        '
        Me.RbtnActive.AutoSize = True
        Me.RbtnActive.BackColor = System.Drawing.Color.Transparent
        Me.RbtnActive.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold)
        Me.RbtnActive.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.RbtnActive.Location = New System.Drawing.Point(600, 54)
        Me.RbtnActive.Name = "RbtnActive"
        Me.RbtnActive.Size = New System.Drawing.Size(69, 23)
        Me.RbtnActive.TabIndex = 33
        Me.RbtnActive.Text = "Active"
        Me.RbtnActive.UseVisualStyleBackColor = False
        '
        'RbtnAll
        '
        Me.RbtnAll.AutoSize = True
        Me.RbtnAll.BackColor = System.Drawing.Color.Transparent
        Me.RbtnAll.Checked = True
        Me.RbtnAll.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold)
        Me.RbtnAll.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.RbtnAll.Location = New System.Drawing.Point(542, 54)
        Me.RbtnAll.Name = "RbtnAll"
        Me.RbtnAll.Size = New System.Drawing.Size(47, 23)
        Me.RbtnAll.TabIndex = 34
        Me.RbtnAll.TabStop = True
        Me.RbtnAll.Text = "All"
        Me.RbtnAll.UseVisualStyleBackColor = False
        '
        'FrmCashRegisterDetails
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.BackgroundImage = Global.RE_X.My.Resources.Resources.formbg_copy
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(1340, 584)
        Me.ControlBox = False
        Me.Controls.Add(Me.RbtnAll)
        Me.Controls.Add(Me.RbtnActive)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.LblPayDtls)
        Me.Controls.Add(Me.LblRecDtls)
        Me.Controls.Add(Me.PanelLoading)
        Me.Controls.Add(Me.ImportBtn)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.CmbBranch)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmb_Type)
        Me.Controls.Add(Me.DtpTo)
        Me.Controls.Add(Me.BtnExport)
        Me.Controls.Add(Me.DtpFrom)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.CmbSearchBy)
        Me.Controls.Add(Me.LblReceipt)
        Me.Controls.Add(Me.BtnReceipt)
        Me.Controls.Add(Me.BtnPayment)
        Me.Controls.Add(Me.BtnEdit)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.BtnPrint)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.LblName)
        Me.Controls.Add(Me.BtnFind)
        Me.Controls.Add(Me.TxtSearch)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.PanelControl)
        Me.Controls.Add(Me.ListView1)
        Me.Font = New System.Drawing.Font("Segoe UI Emoji", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.ImeMode = System.Windows.Forms.ImeMode.[On]
        Me.Name = "FrmCashRegisterDetails"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.PanelControl.ResumeLayout(False)
        Me.PanelLoading.ResumeLayout(False)
        Me.PanelLoading.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents PanelControl As System.Windows.Forms.Panel
    Friend WithEvents BtnMaximize As System.Windows.Forms.Button
    Friend WithEvents BtnClose As System.Windows.Forms.Button
    Friend WithEvents BtnMinimize As System.Windows.Forms.Button
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents TxtSearch As System.Windows.Forms.TextBox
    Friend WithEvents BtnFind As System.Windows.Forms.Button
    Friend WithEvents LblName As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents BtnPrint As System.Windows.Forms.Button
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents BtnEdit As System.Windows.Forms.Button
    Friend WithEvents BtnPayment As System.Windows.Forms.Button
    Friend WithEvents BtnReceipt As System.Windows.Forms.Button
    Friend WithEvents LblReceipt As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents CmbSearchBy As System.Windows.Forms.ComboBox
    Friend WithEvents BtnExport As System.Windows.Forms.Button
    Friend WithEvents SaveDlg As System.Windows.Forms.SaveFileDialog
    Friend WithEvents DtpFrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents DtpTo As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents CmbBranch As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmb_Type As System.Windows.Forms.ComboBox
    Friend WithEvents ImportBtn As System.Windows.Forms.Button
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents PanelLoading As System.Windows.Forms.Panel
    Friend WithEvents Lblimporting As System.Windows.Forms.Label
    Friend WithEvents LblStatus As System.Windows.Forms.Label
    Friend WithEvents LblRecDtls As System.Windows.Forms.Label
    Friend WithEvents LblPayDtls As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents TimerFileSearch As System.Windows.Forms.Timer
    Friend WithEvents BgWrkSearch_File As System.ComponentModel.BackgroundWorker
    Friend WithEvents RbtnActive As RadioButton
    Friend WithEvents RbtnAll As RadioButton
End Class
