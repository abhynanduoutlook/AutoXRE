Imports AutoXDS
Imports AutoXDA
Imports AutoXRPT
Imports System.Threading
Imports System.Xml
Imports System.Net
Imports System.Text
Imports System.Data.Odbc
Imports System.Text.RegularExpressions

Public Class FrmCashRegister
    Dim ds As New BkDS
    Dim dr As BkDS.Cash_RegisterRow
    Dim DrH As BkDS.Cash_RegisterRow
    Dim htp As New Hashtable
    Dim ObjDA As New BkDA
    Dim Entry_No As Integer
    Dim objds As New BkDS
    Dim tempds As New BkDS
    Dim openings As Integer = 0
    Dim frmdate As Date
    Dim todate As Date
    Dim drag As Integer
    Dim mousex As Integer
    Dim mousey As Integer
    Dim Id As Integer = 0
    Dim BkId As Integer = 0
    Dim PayId As Integer = 0

    Private Sub Initlz()
        load_Entry_sub_Head()
        load_Entry_Head()
        load_party()
        Load_Bank()
        Load_Prefix()
        Fill()
    End Sub

    Public Sub New(ByVal HeaderId As Integer, ByVal Entry_type As String, ByVal IsEnable As Boolean)
        InitializeComponent()
        CmbEntType.Text = Entry_type
        LblName.Text = Entry_type
        Id = HeaderId
        If IsEnable = True Then
            CmbEntType.Enabled = True
        Else
            CmbEntType.Enabled = False
        End If
    End Sub

    Private Sub BtnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Save()
    End Sub

    Private Sub Save()

        Dim Prefix As String = ""
        Dim Htp As New Hashtable

        If Validation() = False Then
            Exit Sub
        End If

        If MsgBox("Entry Type : " & CmbEntType.Text & vbLf & "Amount : " & TxtAmount.Text & vbCrLf & "Are you sure want to save?  ", MsgBoxStyle.YesNo, "Save") = MsgBoxResult.No Then
            Exit Sub
        End If

        Dim Add As Boolean = IIf(Id = 0, True, False)

        ds.Cash_Register.Rows.Clear()
        dr = ds.Cash_Register.Rows.Add
        dr.Entry_No = TxtEntryNo.Text
        dr.Entry_Type = CmbEntType.Text
        dr.Entry_Date = DtpEntry_Date.Text
        dr.Party = CmbParty.Text
        dr.Ref_No = TxtrefNo.Text
        dr.Entry_Head = CmbEntHead.Text
        dr.Entry_sub_head = CmbEntSubHead.Text
        dr.Prefix_RefNo = CmbPrifix.Text
        dr.Amount = TxtAmount.Text
        dr.Remarks = TxtRemarks.Text
        dr.Entryhead_ID = Val(CmbEntHead.SelectedValue)
        dr.EntrySubHead_Id = Val(CmbEntSubHead.SelectedValue)
        dr.JobCardNo = CmbPrifix.Text + TxtrefNo.Text
        dr.Branch_Id = PublicShared.Branch_Id
        dr.BkId = BkId
        dr.PayId = PayId
        dr.Payment_Mode = CmbPayModeRecpt.Text
        dr.Bank = CmbBank.Text
        dr.User_Id = PublicShared.User_Id
        dr.User_Name = PublicShared.User_Name

        ObjDA = New BkDA

        If dr.Entry_Type = "Receipt" Then Prefix = Read_Settings("Prefix_Cash_Receipt") Else Prefix = Read_Settings("Prefix_Cash_Payment")

        Dim IsReceipt As Boolean = False
        Dim IsCredit_Sales As Boolean = False

        Dim Receipts As String() = Read_Settings("CashRegister_Receipts").Split(","c)
        If Receipts.Contains(CmbEntSubHead.Text.Trim) Then
            IsReceipt = True
        End If

        Dim Payments As String() = Read_Settings("Credit_Sales").Split(","c)
        If Payments.Contains(CmbEntHead.Text.Trim) Then
            IsCredit_Sales = True
        End If

        Htp = ObjDA.Cash_Register(Id, dr, Prefix, IsReceipt, IsCredit_Sales)

        If Htp Is Nothing Then
            'Enq_HeaderId = 0
        Else
            Id = Htp("Id")
            TxtEntryNo.Text = Htp("Entry_No")
        End If
     

        If Id > 0 Then
            If Add Then
                MsgBox("Saved successfully...", MsgBoxStyle.Information, "Save")
                Clear()
                DtpEntry_Date.Focus()
            Else
                MsgBox("Saved successfully...", MsgBoxStyle.Information, "Save")
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Dispose()
            End If

        Else
            MsgBox("Error found!", MsgBoxStyle.Critical, "Error")
        End If

    End Sub

    Private Sub Load_Bank()

        Dim bkds As New BkDS
        Dim objDA As New BkDA
        Dim TempDs As New BkDS
        Dim BankName As String = ""

        BankName = CmbBank.Text
        bkds = objDA.Get_BankName(BankName)

        CmbBank.DataSource = bkds.Cash_Register.DefaultView
        CmbBank.ValueMember = "Bank"
        CmbBank.DisplayMember = "Bank"

        CmbBank.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        CmbBank.AutoCompleteSource = AutoCompleteSource.ListItems
        CmbBank.SelectedIndex = -1

    End Sub

    Private Sub load_Entry_Head()

        Dim bkds As New BkDS
        Dim TempDs As New BkDS
        Dim objDA As New BkDA

        bkds = objDA.Get_entryhead

        If CmbEntType.Text = "Receipt" Then
            TempDs.Merge(bkds.Entry_Head.Select("Entry_Type='R'"))
        Else
            TempDs.Merge(bkds.Entry_Head.Select("Entry_Type='P'"))
        End If

        CmbEntHead.DataSource = TempDs.Entry_Head.DefaultView
        CmbEntHead.ValueMember = "Entryhead_Id"
        CmbEntHead.DisplayMember = "Entry_Head"

        CmbEntHead.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        CmbEntHead.AutoCompleteSource = AutoCompleteSource.ListItems
        CmbEntHead.SelectedIndex = -1

    End Sub

    Private Sub load_Entry_sub_Head()

        Dim bkds As New BkDS
        Dim objDA As New BkDA
        Dim TempDs As New BkDS

        Dim EntryHead As String = ""

        EntryHead = CmbEntHead.Text

        bkds = objDA.Get_Entrysubhead(EntryHead)

        If CmbEntType.Text = "Receipt" Then
            TempDs.Merge(bkds.Entry_Head.Select("Entry_Type='R'"))
        Else
            TempDs.Merge(bkds.Entry_Head.Select("Entry_Type='P'"))
        End If

        CmbEntSubHead.DataSource = TempDs.Entry_Head.DefaultView
        CmbEntSubHead.ValueMember = "EntryHead_Id"
        CmbEntSubHead.DisplayMember = "Entry_sub_head"

        CmbEntSubHead.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        CmbEntSubHead.AutoCompleteSource = AutoCompleteSource.ListItems

        CmbEntSubHead.SelectedIndex = -1

    End Sub

    Private Sub load_party()

        Dim bkds As New BkDS
        Dim objda As New BkDA
        bkds = objda.Get_Distict_Party

        CmbParty.DataSource = bkds.Cash_Register.DefaultView
        CmbParty.ValueMember = "Party"
        CmbParty.DisplayMember = "Party"

        CmbParty.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        CmbParty.AutoCompleteSource = AutoCompleteSource.ListItems

        CmbParty.SelectedIndex = -1

    End Sub

    Private Sub Clear()

        Id = 0
        ' TxtEntryNo.Text = openings + 1
        If CmbEntType.Text = "Receipt" Then
            TxtEntryNo.Text = ObjDA.Get_Next_Entry_No(CmbEntType.Text, Read_Settings("Prefix_Cash_Receipt"))
        Else : CmbEntType.Text = "Payment"
            TxtEntryNo.Text = ObjDA.Get_Next_Entry_No(CmbEntType.Text, Read_Settings("Prefix_Cash_Payment"))
        End If

        DtpEntry_Date.Text = Date.Now

        ' CmbEntType.SelectedIndex = -1
        CmbParty.SelectedIndex = -1
        CmbPrifix.SelectedIndex = -1
        CmbParty.Text = ""
        TxtrefNo.Text = ""
        CmbEntHead.SelectedIndex = -1
        CmbEntSubHead.SelectedIndex = -1
        TxtAmount.Text = ""
        TxtRemarks.Text = ""

        CmbBank.Text = ""
        CmbPayModeRecpt.Text = ""

        TxtAmount.BackColor = Color.WhiteSmoke
        TxtrefNo.BackColor = Color.WhiteSmoke
        CmbEntType.BackColor = Color.WhiteSmoke
        CmbParty.BackColor = Color.WhiteSmoke
        TxtRemarks.BackColor = Color.WhiteSmoke
        CmbEntHead.BackColor = Color.WhiteSmoke
        CmbEntSubHead.BackColor = Color.WhiteSmoke
        BtnSave.Enabled = True

        If Focus() Then
            DtpEntry_Date.Focus()
        End If

    End Sub
    Private Sub Load_Prefix()
        CmbPrifix.Items.Clear()
        Dim Prifix As String() = Read_Settings("JobCard_Prefix").Split(",")
        For i As Integer = 0 To Prifix.Length - 1
            CmbPrifix.Items.Add(Prifix(i))
        Next
    End Sub
    Private Function Validation() As Boolean
        ObjDA = New BkDA
        If PublicShared.Branch_Id = 0 Then
            MsgBox("Branch not specified...", MsgBoxStyle.Information, "Validation")
            Return False
        End If

        If CmbEntType.Text = "" Then
            CmbEntType.BackColor = Color.Yellow
            CmbEntType.Focus()
            CmbEntType.DroppedDown = True
            Return False
        Else
            CmbEntType.BackColor = Color.WhiteSmoke
        End If

        If ObjDA.Check_Prefix(TxtrefNo.Text, CmbPrifix.Text) > 0 Then
            ' MsgBox("Already Exist", MsgBoxStyle.Information, "Validation")\
            If MessageBox.Show("Already Exist" & vbCrLf & "Do you want to continue?", "Validation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
                Return False
            End If
        End If


        'If CmbEntHead.SelectedIndex <> "" Then
        '    CmbEntHead.BackColor = Color.Yellow
        '    CmbEntHead.Focus()
        '    CmbEntHead.DroppedDown = True
        '    Return False
        'Else
        '    CmbEntHead.BackColor = Color.WhiteSmoke
        'End If

        'If CmbEntSubHead.SelectedIndex <> "" Then
        '    CmbEntSubHead.BackColor = Color.Yellow
        '    CmbEntSubHead.Focus()
        '    CmbEntSubHead.DroppedDown = True
        '    Return False
        'Else
        '    CmbEntSubHead.BackColor = Color.WhiteSmoke
        'End If


        'If Val(TxtAmount.Text) = 0 Then
        '    TxtAmount.BackColor = Color.Yellow
        '    TxtAmount.Focus()
        '    Return False
        'Else
        '    TxtAmount.BackColor = Color.WhiteSmoke
        'End If

        If Not IsNumeric(TxtAmount.Text) Then
            Return False
        End If

        Return True
    End Function

    Private Sub FrmCashReciept_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ObjDA = New BkDA
        Initlz()
        TxtEntryNo.ReadOnly = False
        If Id = 0 Then
            If CmbEntType.Text = "Receipt" Then
                TxtEntryNo.Text = ObjDA.Get_Next_Entry_No(CmbEntType.Text, Read_Settings("Prefix_Cash_Receipt"))
            Else : CmbEntType.Text = "Payment"
                TxtEntryNo.Text = ObjDA.Get_Next_Entry_No(CmbEntType.Text, Read_Settings("Prefix_Cash_Payment"))
            End If
        End If
    End Sub

    Public Function Read_Settings(ByVal S_Key As String) As String
        Dim Value As String = ""
        Try
            Value = PublicShared.Settings_Dt.Select("S_Key='" & S_Key & "'").First.Item("S_Value").ToString
        Catch ex As Exception
        End Try
        Return Value
    End Function

    Private Sub BtnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        Me.Dispose()
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Dispose()
    End Sub

    Private Sub FrmCashRegister_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseDown
        drag = True 'Sets the variable drag to true.
        mousex = Windows.Forms.Cursor.Position.X - Me.Left 'Sets variable mousex
        mousey = Windows.Forms.Cursor.Position.Y - Me.Top 'Sets variable mousey
    End Sub

    Private Sub FrmCashRegister_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseMove
        If drag Then
            Me.Top = Windows.Forms.Cursor.Position.Y - mousey
            Me.Left = Windows.Forms.Cursor.Position.X - mousex
        End If
    End Sub

    Private Sub FrmCashRegister_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseUp
        drag = False
    End Sub

    'Private Sub TxtAmount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Validation()
    'End Sub

    Private Sub FrmCashRegister_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then

        End If
    End Sub

    Private Sub TxtAmount_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TxtAmount.KeyPress

        If Regex.Match(TxtAmount.Text, "^(\$)?((\d{1,9})|(\d{1,3})(\,\d{3})*)(\.\d{0,2})?$").Success Then
            TxtAmount.ForeColor = Color.Black
        Else
            ' TxtAmount.BackColor = Color.Yellow
            TxtAmount.ForeColor = Color.Red

        End If

    End Sub

    Private Sub CmbEntType_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CmbEntType.KeyDown
        CmbEntType.BackColor = Color.WhiteSmoke
        If e.KeyCode = Keys.Enter Then
            If CmbEntType.Text <> " " Then
                DtpEntry_Date.Focus()
            End If
        End If
    End Sub

    Private Sub TxtAmount_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TxtAmount.KeyDown
        If e.KeyCode = Keys.Enter Then
            '  Validation()
            CmbPayModeRecpt.Focus()

        End If
    End Sub

    Private Sub CmbEntHead_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CmbEntHead.KeyDown
        If e.KeyCode = Keys.Enter Then
            If CmbEntHead.Text <> " " Then
                CmbEntSubHead.Focus()
            End If
        End If
    End Sub

    Private Sub CmbEntSubHead_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CmbEntSubHead.KeyDown
        If e.KeyCode = Keys.Enter Then
            CmbPrifix.Focus()
        End If
    End Sub

    Private Sub TxtRemarks_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TxtRemarks.KeyDown
        If e.KeyCode = Keys.Enter Then
            If TxtRemarks.Text <> " " Then
                BtnSave.Focus()
            End If
        End If
    End Sub

    Private Sub TxtrefNo_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TxtrefNo.KeyDown
        If e.KeyCode = Keys.Enter Then
            If TxtrefNo.Text <> " " Then
                TxtAmount.Focus()
            End If
        End If
    End Sub

    Private Sub CmbParty_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CmbParty.KeyDown
        If e.KeyCode = Keys.Enter Then
            If CmbParty.Text <> " " Then
                TxtRemarks.Focus()
                TxtRemarks.SelectionStart = TxtRemarks.Text.Length
            End If
        End If
    End Sub

    Private Sub CmbEntHead_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbEntHead.SelectedIndexChanged
        If CmbEntHead.CanFocus Then

            TxtrefNo.Enabled = True
            CmbParty.Enabled = True


            If CmbEntHead.SelectedIndex > -1 And CmbEntType.Text = "Payment" Then
                If CmbEntHead.SelectedValue > 0 Then
                    Dim Receipts As String() = Read_Settings("CashRegister_Payments").Split(","c)
                    If Receipts.Contains(CmbEntHead.Text.Trim) Then
                        TxtrefNo.Enabled = False
                        CmbParty.Enabled = False
                    End If
                End If

            End If


            CmbEntSubHead.Text = ""

            load_Entry_sub_Head()

        End If
       
    End Sub

    Private Sub CmbEntType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbEntType.SelectedIndexChanged
        If TxtEntryNo.Text = " " Then
            Me.Cursor = Cursors.WaitCursor
            If CmbEntType.Text = "Receipt" Then
                TxtEntryNo.Text = ObjDA.Get_Next_Entry_No(CmbEntType.Text, Read_Settings("Prefix_Cash_Receipt"))
            Else : CmbEntType.Text = "Payment"
                TxtEntryNo.Text = ObjDA.Get_Next_Entry_No(CmbEntType.Text, Read_Settings("Prefix_Cash_Payment"))
            End If
            Me.Cursor = Cursors.Default
        End If
    End Sub

    Private Sub CmbEntType_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles CmbEntType.MouseClick
        CmbEntType.BackColor = Color.WhiteSmoke
    End Sub

    Private Sub DtpEntry_Date_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DtpEntry_Date.KeyDown
        If e.KeyCode = Keys.Enter Then
            CmbEntHead.Focus()
        End If
    End Sub

    Private Sub TxtAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtAmount.TextChanged
        If Regex.Match(TxtAmount.Text, "^(\$)?((\d{1,9})|(\d{1,3})(\,\d{3})*)(\.\d{0,2})?$").Success Then
            TxtAmount.ForeColor = Color.Black
            TxtAmount.BackColor = Color.WhiteSmoke
        Else

            TxtAmount.BackColor = Color.Yellow
            TxtAmount.ForeColor = Color.Red
        End If
    End Sub

    Private Sub Fill()

        If Id > 0 Then

            ObjDA = New BkDA
            ds = New BkDS
            ds = ObjDA.Get_cash_Reg(Id, frmdate, todate)

            If ds.Cash_Register.Rows.Count > 0 Then

                DrH = ds.Cash_Register.Rows(0)

                BkId = DrH.BkId
                PayId = DrH.PayId
                TxtEntryNo.Text = DrH.Entry_No
                CmbEntType.Text = DrH.Entry_Type
                DtpEntry_Date.Text = DrH.Entry_Date
                CmbParty.Text = DrH.Party
                TxtAmount.Text = DrH.Amount
                CmbPrifix.Text = DrH.Prefix_RefNo
                TxtrefNo.Text = DrH.Ref_No
                CmbEntHead.Text = DrH.Entry_Head
                'load_Entry_sub_Head()

                CmbEntSubHead.Text = DrH.Entry_sub_head
                TxtRemarks.Text = DrH.Remarks
                CmbBank.Text = DrH.Bank
                CmbPayModeRecpt.Text = DrH.Payment_Mode

                If CmbEntType.Text = "Receipt" Then
                    Dim Receipts As String() = Read_Settings("CashRegister_Receipts").Split(","c)
                    If Receipts.Contains(DrH.Entry_sub_head) Then
                        TxtrefNo.Enabled = False
                        CmbParty.Enabled = False
                    Else
                        TxtrefNo.Enabled = True
                        CmbParty.Enabled = True
                    End If
                End If

                If CmbEntType.Text = "Payment" Then
                    Dim Receipts As String() = Read_Settings("CashRegister_Payments").Split(","c)
                    If Receipts.Contains(DrH.Entry_Head) Then
                        TxtrefNo.Enabled = False
                        CmbParty.Enabled = False
                    Else
                        TxtrefNo.Enabled = True
                        CmbParty.Enabled = True
                    End If
                End If

            End If

        Else
            CmbPayModeRecpt.Text = "Cash"
            ObjDA = New BkDA
            ' TxtEntryNo.Text = BkDA.Get_Next_Entry_No(Entry_No, Read_Settings("Prefix_Purchase"))
        End If

    End Sub

    Private Sub BtnMinimize_Click(sender As System.Object, e As System.EventArgs) Handles BtnMinimize.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub BtnMaximize_Click(sender As System.Object, e As System.EventArgs) Handles BtnMaximize.Click
        If Me.WindowState = FormWindowState.Maximized Then
            Me.WindowState = FormWindowState.Normal
        Else
            Me.WindowState = FormWindowState.Maximized
        End If
    End Sub

    Private Sub CmbEntSubHead_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbEntSubHead.SelectedIndexChanged

        If CmbEntSubHead.CanFocus And CmbEntSubHead.SelectedIndex > -1 And CmbEntType.Text = "Receipt" Then
            If CmbEntSubHead.SelectedValue > 0 Then
                Dim Receipts As String() = Read_Settings("CashRegister_Receipts").Split(","c)
                If Receipts.Contains(CmbEntSubHead.Text.Trim) Then
                    TxtrefNo.Enabled = False
                    CmbParty.Enabled = False
                Else
                    TxtrefNo.Enabled = True
                    CmbParty.Enabled = True
                End If
            End If

        End If
       
    End Sub

    Private Sub CmbPayModeRecpt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CmbPayModeRecpt.KeyDown
        If e.KeyCode = Keys.Enter Then
            If CmbPayModeRecpt.Text <> "Cash" Then
                CmbBank.Focus()
            Else
                CmbParty.Focus()
            End If
        End If
    End Sub

    Private Sub CmbPayModeRecpt_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmbPayModeRecpt.SelectedIndexChanged
        If CmbPayModeRecpt.Text = "Cash" Then
            CmbBank.SelectedIndex = -1
            CmbBank.Enabled = False
        Else
            CmbBank.Enabled = True
        End If
    End Sub

    Private Sub CmbBank_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CmbBank.KeyDown
        If e.KeyCode = Keys.Enter Then
            If CmbBank.Text <> "" Then
                CmbParty.Focus()
            Else
                CmbParty.Focus()
            End If
        End If
    End Sub

    Private Sub CmbPrifix_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles CmbPrifix.KeyDown
        If e.KeyCode = Keys.Enter Then
            TxtrefNo.Focus()
        End If
    End Sub

    Private Sub CmbBank_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbBank.SelectedIndexChanged
        '  Load_Bank()
    End Sub
End Class