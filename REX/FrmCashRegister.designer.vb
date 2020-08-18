<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmCashRegister
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
        Me.LblName = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.TxtRemarks = New System.Windows.Forms.TextBox()
        Me.CmbEntSubHead = New System.Windows.Forms.ComboBox()
        Me.BtnSave = New System.Windows.Forms.Button()
        Me.CmbEntHead = New System.Windows.Forms.ComboBox()
        Me.CmbEntType = New System.Windows.Forms.ComboBox()
        Me.TxtEntryNo = New System.Windows.Forms.TextBox()
        Me.PanelControl = New System.Windows.Forms.Panel()
        Me.BtnMaximize = New System.Windows.Forms.Button()
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.BtnMinimize = New System.Windows.Forms.Button()
        Me.TxtrefNo = New System.Windows.Forms.TextBox()
        Me.TxtAmount = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.CmbParty = New System.Windows.Forms.ComboBox()
        Me.DtpEntry_Date = New System.Windows.Forms.DateTimePicker()
        Me.CmbPayModeRecpt = New System.Windows.Forms.ComboBox()
        Me.CmbBank = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.CmbPrifix = New System.Windows.Forms.ComboBox()
        Me.PanelControl.SuspendLayout()
        Me.SuspendLayout()
        '
        'LblName
        '
        Me.LblName.AutoSize = True
        Me.LblName.BackColor = System.Drawing.Color.Transparent
        Me.LblName.Font = New System.Drawing.Font("Segoe UI Symbol", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblName.ForeColor = System.Drawing.Color.White
        Me.LblName.Location = New System.Drawing.Point(2, 4)
        Me.LblName.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblName.Name = "LblName"
        Me.LblName.Size = New System.Drawing.Size(54, 23)
        Me.LblName.TabIndex = 0
        Me.LblName.Text = "Entry"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Segoe UI Semibold", 8.5!, System.Drawing.FontStyle.Bold)
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(31, 56)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(91, 20)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Entry No    :"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Segoe UI Semibold", 8.5!, System.Drawing.FontStyle.Bold)
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(31, 93)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(90, 20)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Entry Date :"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Segoe UI Semibold", 8.5!, System.Drawing.FontStyle.Bold)
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(31, 207)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(92, 20)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Ref No        :"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Segoe UI Semibold", 8.5!, System.Drawing.FontStyle.Bold)
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.Location = New System.Drawing.Point(31, 130)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(94, 20)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Entry Head :"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Segoe UI Semibold", 8.5!, System.Drawing.FontStyle.Bold)
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.Location = New System.Drawing.Point(31, 168)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(91, 20)
        Me.Label7.TabIndex = 8
        Me.Label7.Text = "Sub Head   :"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Font = New System.Drawing.Font("Segoe UI Semibold", 8.5!, System.Drawing.FontStyle.Bold)
        Me.Label8.ForeColor = System.Drawing.Color.White
        Me.Label8.Location = New System.Drawing.Point(31, 327)
        Me.Label8.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(87, 20)
        Me.Label8.TabIndex = 22
        Me.Label8.Text = "Remarks    :"
        '
        'TxtRemarks
        '
        Me.TxtRemarks.Font = New System.Drawing.Font("Segoe UI Emoji", 8.0!)
        Me.TxtRemarks.Location = New System.Drawing.Point(122, 322)
        Me.TxtRemarks.Margin = New System.Windows.Forms.Padding(4)
        Me.TxtRemarks.Multiline = True
        Me.TxtRemarks.Name = "TxtRemarks"
        Me.TxtRemarks.Size = New System.Drawing.Size(442, 39)
        Me.TxtRemarks.TabIndex = 23
        '
        'CmbEntSubHead
        '
        Me.CmbEntSubHead.Font = New System.Drawing.Font("Segoe UI Emoji", 8.0!)
        Me.CmbEntSubHead.FormattingEnabled = True
        Me.CmbEntSubHead.Location = New System.Drawing.Point(122, 165)
        Me.CmbEntSubHead.Name = "CmbEntSubHead"
        Me.CmbEntSubHead.Size = New System.Drawing.Size(443, 25)
        Me.CmbEntSubHead.TabIndex = 9
        '
        'BtnSave
        '
        Me.BtnSave.BackColor = System.Drawing.Color.White
        Me.BtnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnSave.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSave.Location = New System.Drawing.Point(370, 387)
        Me.BtnSave.Name = "BtnSave"
        Me.BtnSave.Size = New System.Drawing.Size(85, 24)
        Me.BtnSave.TabIndex = 24
        Me.BtnSave.Text = "&Save"
        Me.BtnSave.UseVisualStyleBackColor = False
        '
        'CmbEntHead
        '
        Me.CmbEntHead.Font = New System.Drawing.Font("Segoe UI Emoji", 8.0!)
        Me.CmbEntHead.FormattingEnabled = True
        Me.CmbEntHead.Location = New System.Drawing.Point(122, 127)
        Me.CmbEntHead.Name = "CmbEntHead"
        Me.CmbEntHead.Size = New System.Drawing.Size(444, 25)
        Me.CmbEntHead.TabIndex = 7
        '
        'CmbEntType
        '
        Me.CmbEntType.Font = New System.Drawing.Font("Segoe UI Emoji", 8.0!)
        Me.CmbEntType.FormattingEnabled = True
        Me.CmbEntType.Items.AddRange(New Object() {"Receipt", "Payment"})
        Me.CmbEntType.Location = New System.Drawing.Point(364, 53)
        Me.CmbEntType.Name = "CmbEntType"
        Me.CmbEntType.Size = New System.Drawing.Size(101, 25)
        Me.CmbEntType.TabIndex = 3
        '
        'TxtEntryNo
        '
        Me.TxtEntryNo.Enabled = False
        Me.TxtEntryNo.Font = New System.Drawing.Font("Segoe UI Emoji", 8.0!)
        Me.TxtEntryNo.Location = New System.Drawing.Point(122, 52)
        Me.TxtEntryNo.Margin = New System.Windows.Forms.Padding(4)
        Me.TxtEntryNo.Name = "TxtEntryNo"
        Me.TxtEntryNo.Size = New System.Drawing.Size(196, 25)
        Me.TxtEntryNo.TabIndex = 2
        '
        'PanelControl
        '
        Me.PanelControl.BackColor = System.Drawing.Color.Transparent
        Me.PanelControl.Controls.Add(Me.BtnMaximize)
        Me.PanelControl.Controls.Add(Me.BtnClose)
        Me.PanelControl.Controls.Add(Me.BtnMinimize)
        Me.PanelControl.Location = New System.Drawing.Point(483, 4)
        Me.PanelControl.Name = "PanelControl"
        Me.PanelControl.Size = New System.Drawing.Size(97, 26)
        Me.PanelControl.TabIndex = 26
        '
        'BtnMaximize
        '
        Me.BtnMaximize.Enabled = False
        Me.BtnMaximize.FlatAppearance.BorderSize = 0
        Me.BtnMaximize.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnMaximize.Font = New System.Drawing.Font("Wingdings", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.BtnMaximize.ForeColor = System.Drawing.Color.White
        Me.BtnMaximize.Location = New System.Drawing.Point(40, 3)
        Me.BtnMaximize.Name = "BtnMaximize"
        Me.BtnMaximize.Size = New System.Drawing.Size(19, 23)
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
        Me.BtnMinimize.Location = New System.Drawing.Point(16, 5)
        Me.BtnMinimize.Name = "BtnMinimize"
        Me.BtnMinimize.Size = New System.Drawing.Size(19, 23)
        Me.BtnMinimize.TabIndex = 0
        Me.BtnMinimize.Text = "-"
        Me.BtnMinimize.UseVisualStyleBackColor = True
        '
        'TxtrefNo
        '
        Me.TxtrefNo.Font = New System.Drawing.Font("Segoe UI Emoji", 8.0!)
        Me.TxtrefNo.Location = New System.Drawing.Point(285, 204)
        Me.TxtrefNo.Margin = New System.Windows.Forms.Padding(4)
        Me.TxtrefNo.Name = "TxtrefNo"
        Me.TxtrefNo.Size = New System.Drawing.Size(108, 25)
        Me.TxtrefNo.TabIndex = 12
        '
        'TxtAmount
        '
        Me.TxtAmount.Font = New System.Drawing.Font("Segoe UI Emoji", 8.0!)
        Me.TxtAmount.Location = New System.Drawing.Point(467, 204)
        Me.TxtAmount.Margin = New System.Windows.Forms.Padding(4)
        Me.TxtAmount.Name = "TxtAmount"
        Me.TxtAmount.Size = New System.Drawing.Size(99, 25)
        Me.TxtAmount.TabIndex = 15
        Me.TxtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Segoe UI Semibold", 8.5!, System.Drawing.FontStyle.Bold)
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(396, 206)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(72, 20)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Amount :"
        '
        'BtnCancel
        '
        Me.BtnCancel.BackColor = System.Drawing.Color.White
        Me.BtnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnCancel.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(463, 387)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(85, 24)
        Me.BtnCancel.TabIndex = 25
        Me.BtnCancel.Text = "&Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = False
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Segoe UI Semibold", 8.5!, System.Drawing.FontStyle.Bold)
        Me.Label9.ForeColor = System.Drawing.Color.White
        Me.Label9.Location = New System.Drawing.Point(31, 286)
        Me.Label9.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(89, 20)
        Me.Label9.TabIndex = 20
        Me.Label9.Text = "Party          :"
        '
        'CmbParty
        '
        Me.CmbParty.Font = New System.Drawing.Font("Segoe UI Emoji", 8.0!)
        Me.CmbParty.FormattingEnabled = True
        Me.CmbParty.Location = New System.Drawing.Point(122, 283)
        Me.CmbParty.Name = "CmbParty"
        Me.CmbParty.Size = New System.Drawing.Size(443, 25)
        Me.CmbParty.TabIndex = 21
        '
        'DtpEntry_Date
        '
        Me.DtpEntry_Date.CalendarMonthBackground = System.Drawing.Color.WhiteSmoke
        Me.DtpEntry_Date.CustomFormat = "dd-MMM-yyyy hh:mm tt"
        Me.DtpEntry_Date.Font = New System.Drawing.Font("Segoe UI Symbol", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DtpEntry_Date.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DtpEntry_Date.Location = New System.Drawing.Point(122, 89)
        Me.DtpEntry_Date.Name = "DtpEntry_Date"
        Me.DtpEntry_Date.Size = New System.Drawing.Size(196, 26)
        Me.DtpEntry_Date.TabIndex = 5
        '
        'CmbPayModeRecpt
        '
        Me.CmbPayModeRecpt.Font = New System.Drawing.Font("Segoe UI Emoji", 8.0!)
        Me.CmbPayModeRecpt.FormattingEnabled = True
        Me.CmbPayModeRecpt.Items.AddRange(New Object() {"Bank", "Cash", "Cheque", "DD", "RTGS", "SmartCard"})
        Me.CmbPayModeRecpt.Location = New System.Drawing.Point(124, 245)
        Me.CmbPayModeRecpt.Name = "CmbPayModeRecpt"
        Me.CmbPayModeRecpt.Size = New System.Drawing.Size(81, 25)
        Me.CmbPayModeRecpt.TabIndex = 17
        '
        'CmbBank
        '
        Me.CmbBank.Font = New System.Drawing.Font("Segoe UI Emoji", 8.0!)
        Me.CmbBank.FormattingEnabled = True
        Me.CmbBank.Items.AddRange(New Object() {"Receipt", "Payment"})
        Me.CmbBank.Location = New System.Drawing.Point(262, 245)
        Me.CmbBank.Name = "CmbBank"
        Me.CmbBank.Size = New System.Drawing.Size(304, 25)
        Me.CmbBank.TabIndex = 19
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Segoe UI Semibold", 8.5!, System.Drawing.FontStyle.Bold)
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(31, 245)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(90, 20)
        Me.Label3.TabIndex = 16
        Me.Label3.Text = "Pay Mode  :"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.BackColor = System.Drawing.Color.Transparent
        Me.Label10.Font = New System.Drawing.Font("Segoe UI Semibold", 8.5!, System.Drawing.FontStyle.Bold)
        Me.Label10.ForeColor = System.Drawing.Color.White
        Me.Label10.Location = New System.Drawing.Point(210, 247)
        Me.Label10.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(51, 20)
        Me.Label10.TabIndex = 18
        Me.Label10.Text = "Bank :"
        '
        'CmbPrifix
        '
        Me.CmbPrifix.Font = New System.Drawing.Font("Segoe UI Emoji", 8.0!)
        Me.CmbPrifix.FormattingEnabled = True
        Me.CmbPrifix.Items.AddRange(New Object() {"Receipt", "Payment"})
        Me.CmbPrifix.Location = New System.Drawing.Point(122, 204)
        Me.CmbPrifix.Name = "CmbPrifix"
        Me.CmbPrifix.Size = New System.Drawing.Size(156, 25)
        Me.CmbPrifix.TabIndex = 11
        '
        'FrmCashRegister
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 22.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.BackgroundImage = Global.RE_X.My.Resources.Resources.formbg_copy
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(585, 434)
        Me.Controls.Add(Me.CmbPrifix)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.CmbBank)
        Me.Controls.Add(Me.CmbPayModeRecpt)
        Me.Controls.Add(Me.DtpEntry_Date)
        Me.Controls.Add(Me.CmbParty)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.TxtAmount)
        Me.Controls.Add(Me.TxtrefNo)
        Me.Controls.Add(Me.PanelControl)
        Me.Controls.Add(Me.TxtEntryNo)
        Me.Controls.Add(Me.CmbEntType)
        Me.Controls.Add(Me.CmbEntHead)
        Me.Controls.Add(Me.BtnSave)
        Me.Controls.Add(Me.CmbEntSubHead)
        Me.Controls.Add(Me.TxtRemarks)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.LblName)
        Me.Font = New System.Drawing.Font("Segoe UI Emoji", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "FrmCashRegister"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FrmCashReciept"
        Me.PanelControl.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LblName As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents TxtRemarks As System.Windows.Forms.TextBox
    Friend WithEvents CmbEntSubHead As System.Windows.Forms.ComboBox
    Friend WithEvents BtnSave As System.Windows.Forms.Button
    Friend WithEvents CmbEntHead As System.Windows.Forms.ComboBox
    Friend WithEvents CmbEntType As System.Windows.Forms.ComboBox
    Friend WithEvents TxtEntryNo As System.Windows.Forms.TextBox
    Friend WithEvents PanelControl As System.Windows.Forms.Panel
    Friend WithEvents BtnMaximize As System.Windows.Forms.Button
    Friend WithEvents BtnClose As System.Windows.Forms.Button
    Friend WithEvents BtnMinimize As System.Windows.Forms.Button
    Friend WithEvents TxtrefNo As System.Windows.Forms.TextBox
    Friend WithEvents TxtAmount As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents CmbParty As System.Windows.Forms.ComboBox
    Friend WithEvents DtpEntry_Date As System.Windows.Forms.DateTimePicker
    Friend WithEvents CmbPayModeRecpt As System.Windows.Forms.ComboBox
    Friend WithEvents CmbBank As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents CmbPrifix As System.Windows.Forms.ComboBox
End Class
