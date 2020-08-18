Imports AutoXDS
Imports AutoXDA
Imports AutoXRPT
Imports System.Threading
Imports System.Xml
Imports System.Net
Imports System.Text
Imports System.Data.Odbc
Imports System.Windows.Forms
Imports Microsoft.Office.Interop
Imports System.IO
Imports System.Configuration
Imports System.Text.RegularExpressions

Public Class FrmCashRegisterDetails

    Dim ds As New BkDS
    Dim CashDs As New BkDS
    Dim dr As BkDS.Cash_RegisterRow
    Dim lvItem As ListViewItem
    Dim entry_no As String
    Dim entry_type As String
    Dim htp As Hashtable
    Dim ObjDA As New BkDA
    Dim objds As New BkDS
    Dim ObjCR As FrmCashRegister
    Dim Import_Log As String = ""
    Dim bkds As New BkDS
    Dim ReturnValue As String = ""
    Dim openings As Decimal = 0.0
    Dim Current_Balance As Decimal = 0.0
    Dim Id As Integer = 0

    Dim drag As Boolean
    Dim mousex As Integer
    Dim mousey As Integer

    Dim Receipt As Decimal
    Dim Payment As Decimal
    Dim CashRec As Decimal = 0.0
    Dim CashPay As Decimal = 0.0
    Dim Cheque As Decimal = 0.0
    Dim RTGSPay As Decimal = 0.0
    Dim RTGSRec As Decimal = 0.0
    Dim SmartCardRec As Decimal = 0.0
    Dim SmartCardPay As Decimal = 0.0
    Dim ChequeRec As Decimal = 0.0
    Dim ChequePay As Decimal = 0.0
    Dim BankRec As Decimal = 0.0
    Dim BankPay As Decimal = 0.0
    Dim DDRec As Decimal = 0.0
    Dim DDPay As Decimal = 0.0
    Dim TotalBal As Decimal
    Dim Rec As String = "Receipt"
    Dim Pay As String = "Payment"

    Dim File_Extension As String = ""
    Dim BranchName As String = ""
    Dim DsLedger As New DataSet
    Dim TallyHost As String
    Dim imp_jobcard_Path As String
    Dim File_Name As String = ""

    Private Sub Initlz()
        BranchName = Read_Settings("TallyCompanyBranch")
        TallyHost = Read_Settings("TallyHost")
        Create_Header()
        Load_SearchBy()
        Load_Type()
        Load_Branch()
        Find()
        Start_File_Search()
    End Sub

    Private Sub Create_Header1()

        ListView1.Columns.Add("Id", 0)
        ListView1.Columns.Add("SlNo", 40)
        ListView1.Columns.Add("Entry No", 85)
        ListView1.Columns.Add("Entry Type", 100)
        ListView1.Columns.Add("Entry Date", 145)
        ListView1.Columns.Add("Party", 80)
        ListView1.Columns.Add("Ref No", 100)
        ListView1.Columns.Add("Entry Head", 95)
        ListView1.Columns.Add("Entry Sub Head", 95)
        ListView1.Columns.Add("Amount", 100, HorizontalAlignment.Right)
        ListView1.Columns.Add("Remarks", 125)

    End Sub

    Private Sub Create_Header()
        ListView1.Columns.Add("Id", 0)
        ListView1.Columns.Add("SlNo", 60)
        '  ListView1.Columns.Add("Branch", 90)
        ListView1.Columns.Add("Entry No", 90)
        ListView1.Columns.Add("Entry Type", 105)
        ListView1.Columns.Add("Entry Date", 155)
        ListView1.Columns.Add("Party", 105)
        ListView1.Columns.Add("Ref No", 150)
        ListView1.Columns.Add("Entry Head", 105)
        ListView1.Columns.Add("Entry Sub Head", 140)
        ListView1.Columns.Add("Amount", 100, HorizontalAlignment.Right)
        ListView1.Columns.Add("Remarks", 120)
        ListView1.Columns.Add("Balance", 100, HorizontalAlignment.Right)
        ListView1.Columns.Add("Tally", 60)

    End Sub

    Private Sub Load_SearchBy()

        CmbSearchBy.Items.Add("Entry No")
        CmbSearchBy.Items.Add("Entry Head")
        CmbSearchBy.Items.Add("Entry Sub Head")
        CmbSearchBy.Items.Add("Ref. No")
        CmbSearchBy.Items.Add("Party")
        'CmbSearchBy.Items.Add("Payment Mode")
        CmbSearchBy.SelectedIndex = 0

    End Sub

    Private Sub Load_Type()
        cmb_Type.Items.Add("Receipt")
        cmb_Type.Items.Add("Payment")
        cmb_Type.SelectedIndex = -1
    End Sub

    Private Sub Load_Branch()

        Dim DsB As New CompDS
        Dim DsS As New CompDS

        If FrmMDI.MultiBranch Then
            DsS.Merge(PublicShared.Branch_Dt.Select("Sc_Id = '" & PublicShared.User_Id & "'"))
        ElseIf PublicShared.Branch_Id > 0 Then
            DsB.Merge(PublicShared.Branch_Dt.Select("BranchId = '" & PublicShared.Branch_Id & "'"))
        Else
            DsB.Merge(PublicShared.Branch_Dt)
        End If

        If DsB.Branch.Rows.Count > 0 Then
            CmbBranch.DataSource = DsB.Branch.DefaultView
        Else
            CmbBranch.DataSource = DsS.Sc_Branch.DefaultView
        End If

        CmbBranch.ValueMember = "BranchId"
        CmbBranch.DisplayMember = "Branch_Name"

        CmbBranch.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        CmbBranch.AutoCompleteSource = AutoCompleteSource.ListItems

        CmbBranch.SelectedValue = PublicShared.Branch_Id

    End Sub

    Private Sub Find()

        ListView1.Items.Clear()
        htp = New Hashtable
        ObjDA = New BkDA
        Receipt = 0.0
        Payment = 0.0
        TotalBal = 0.0

        'If Cmb_Head.Text <> "" Then
        '    htp.Add(bkds.searchcashRegisterBy.Entry_Head, Cmb_Head.Text.Trim)
        'End If

        If CmbBranch.SelectedIndex > -1 Then
            htp.Add(bkds.searchcashRegisterBy.Branch, CmbBranch.SelectedValue)
        ElseIf PublicShared.Branch_Id > 0 Then
            htp.Add(bkds.searchcashRegisterBy.Branch, PublicShared.Branch_Id)
        End If

        If Not PublicShared.IsAdmin Then
            htp.Add(bkds.searchcashRegisterBy.User_Id, PublicShared.User_Id)
        End If

        If TxtSearch.Text.Trim <> "" Then
            If CmbSearchBy.Text = "Entry No" Then
                htp.Add(bkds.searchcashRegisterBy.Entry_No, TxtSearch.Text.Trim)
            ElseIf CmbSearchBy.Text = "Party" Then
                htp.Add(BkDS.searchcashRegisterBy.Party, TxtSearch.Text.Trim)
            ElseIf CmbSearchBy.Text = "Entry Date" Then
                htp.Add(bkds.searchcashRegisterBy.Entry_Date, TxtSearch.Text.Trim)
            ElseIf CmbSearchBy.Text = "Amount" Then
                htp.Add(bkds.searchcashRegisterBy.Amount, TxtSearch.Text.Trim)
            ElseIf CmbSearchBy.Text = "Entry Head" Then
                htp.Add(bkds.searchcashRegisterBy.Entry_Head, TxtSearch.Text.Trim)
            ElseIf CmbSearchBy.Text = "Entry Sub Head" Then
                htp.Add(bkds.searchcashRegisterBy.Entry_sub_head, TxtSearch.Text.Trim)
            ElseIf CmbSearchBy.Text = "Ref. No" Then
                htp.Add(bkds.searchcashRegisterBy.Ref_no, TxtSearch.Text.Trim)
            End If
        End If
        If cmb_Type.Text <> "" Then
            'Entry_Type
            htp.Add(bkds.searchcashRegisterBy.Entry_Type, cmb_Type.Text.Trim)

            'If cmb_Type.Text = "Receipt" Then
            '    htp.Add(bkds.searchEntry_TypeBy.Receipt, cmb_Type.Text.Trim)
            'ElseIf cmb_Type.Text = "Payment" Then
            '    htp.Add(bkds.searchEntry_TypeBy.Payment, cmb_Type.Text.Trim)
            'End If
        End If

        CashDs = ObjDA.Get_cash_Register(entry_no, DtpFrom.Value, DtpTo.Value, htp)


        openings = objDA.Get_Opening_cash(DtpFrom.Value.Date, CmbBranch.SelectedValue)
        Dim SlNo As Integer = 1

        'LblOpening.Text = "Opening : "
        'LblBal.Text = "Closing : "
        'LblPay.Text = "Payment : "
        LblReceipt.Text = "Receipt : "
        LblRecDtls.Text = "Cash : "
        'LblSmartCard.Text = "SmartCard : "
        'LblCash.Text = "Cheque : "
        'LblSmartCard.Text = "Bank : "
        'LblCash.Text = "RTGS : "
        'LblSmartCard.Text = "DD : "1

        ds = New BkDS

        If RbtnAll.Checked = True Then
            ds = CashDs
        ElseIf RbtnActive.Checked = True Then
            ds.Merge(CashDs.Cash_Register.Select("PostedToTally='0'"))
        End If


        Current_Balance = openings

        If ds.Cash_Register.Rows.Count > 0 Then


            For Each Me.dr In ds.Cash_Register.Rows

                If dr.Entry_Type = "Receipt" Then
                    Current_Balance += Val(dr.Amount)
                    dr.Balance = Current_Balance
                Else : dr.Entry_Type = "Payment"
                    Current_Balance -= Val(dr.Amount)
                    dr.Balance = Current_Balance
                End If

                lvItem = ListView1.Items.Add(dr.Id)
                lvItem.SubItems.Add(SlNo)
                ' lvItem.SubItems.Add(Dr.Branch_Name)
                lvItem.SubItems.Add(dr.Entry_No)
                lvItem.SubItems.Add(dr.Entry_Type)
                lvItem.SubItems.Add(Format(dr.Entry_Date, "dd-MMM-yyyy h:mm tt"))
                lvItem.SubItems.Add(dr.Party)
                lvItem.SubItems.Add(IIf(dr.Prefix_RefNo = "", dr.Ref_No, dr.Prefix_RefNo + dr.Ref_No))
                lvItem.SubItems.Add(dr.Entry_Head)
                lvItem.SubItems.Add(dr.Entry_sub_head)
                lvItem.SubItems.Add(Math.Round(dr.Amount, 2))
                lvItem.SubItems.Add(dr.Remarks)
                lvItem.SubItems.Add(Math.Round(Val(dr.Balance), 2))
                lvItem.SubItems.Add((IIf(dr.PostedToTally = 1, "Yes", "No")))

                SlNo += 1

                If dr.Entry_Type = "Payment" Then
                    lvItem.BackColor = Color.Wheat
                Else
                    lvItem.BackColor = Color.White
                End If
                If dr.Amount = "0" Then
                    lvItem.BackColor = Color.LightBlue
                End If

            Next
            CashRec = 0
            CashPay = 0
            SmartCardRec = 0
            SmartCardPay = 0
            ChequeRec = 0
            ChequePay = 0
            BankRec = 0
            BankPay = 0
            DDRec = 0
            DDPay = 0
            RTGSRec = 0
            RTGSPay = 0


            If ds.Cash_Register.Select("Entry_Type = 'Receipt'").Count > 0 Then

                If ds.Cash_Register.Select("Entry_Type = 'Receipt'").Count > 0 Then
                    Receipt = ds.Cash_Register.Compute("sum(Amount) ", "Entry_Type = 'Receipt'")
                End If

                'If ds.Cash_Register.Select("Entry_Type = 'Payment'").Count > 0 Then
                '    Payment = ds.Cash_Register.Compute("sum(Amount)", "Entry_Type = 'Payment'")
                'End If

                If ds.Cash_Register.Select("Payment_Mode = 'Cash'").Count > 0 Then
                    CashRec = ds.Cash_Register.Compute("sum(Amount)", " Entry_Type = 'Receipt' and Payment_Mode = 'Cash'")
                End If

                If ds.Cash_Register.Select("Payment_Mode = 'SmartCard'").Count > 0 Then
                    SmartCardRec = ds.Cash_Register.Compute("sum(Amount)", "Entry_Type = 'Receipt' and Payment_Mode = 'SmartCard'")
                End If
                If ds.Cash_Register.Select("Payment_Mode = 'Bank'").Count > 0 Then
                    BankRec = ds.Cash_Register.Compute("sum(Amount)", "Entry_Type = 'Receipt' and Payment_Mode = 'Bank'")
                End If

                If ds.Cash_Register.Select("Payment_Mode = 'Cheque'").Count > 0 Then
                    ChequeRec = ds.Cash_Register.Compute("sum(Amount)", "Entry_Type = 'Receipt' and Payment_Mode = 'Cheque'")
                End If
                If ds.Cash_Register.Select("Payment_Mode = 'RTGS'").Count > 0 Then
                    RTGSRec = ds.Cash_Register.Compute("sum(Amount)", "Entry_Type = 'Receipt' and Payment_Mode = 'RTGS'")
                End If

                If ds.Cash_Register.Select("Payment_Mode = 'DD'").Count > 0 Then
                    DDRec = ds.Cash_Register.Compute("sum(Amount)", "Entry_Type = 'Receipt' and Payment_Mode = 'DD'")
                End If
            End If
        End If
        If ds.Cash_Register.Select("Entry_Type = 'Payment'").Count > 0 Then
            Payment = ds.Cash_Register.Compute("sum(Amount)", "Entry_Type = 'Payment'")

            If ds.Cash_Register.Select("Payment_Mode = 'Cash'").Count > 0 Then
                CashPay = ds.Cash_Register.Compute("sum(Amount)", " Entry_Type = 'Payment' and Payment_Mode = 'Cash'")
            End If
            If ds.Cash_Register.Select("Entry_Type = 'Payment' and Payment_Mode = 'SmartCard'").Count > 0 Then
                SmartCardPay = ds.Cash_Register.Compute("sum(Amount)", "Entry_Type = 'Payment' and Payment_Mode = 'SmartCard'")
            End If
            If ds.Cash_Register.Select("Entry_Type = 'Payment' and Payment_Mode = 'Bank'").Count > 0 Then
                BankPay = ds.Cash_Register.Compute("sum(Amount)", "Entry_Type = 'Payment' and Payment_Mode = 'Bank'")
            End If

            If ds.Cash_Register.Select("Payment_Mode = 'Cheque'").Count > 0 Then
                Cheque = ds.Cash_Register.Compute("sum(Amount)", "Entry_Type = 'Payment' and Payment_Mode = 'Cheque'")
            End If
            If ds.Cash_Register.Select("Payment_Mode = 'RTGS'").Count > 0 Then
                RTGSPay = ds.Cash_Register.Compute("sum(Amount)", "Entry_Type = 'Payment' and Payment_Mode = 'RTGS'")
            End If

            If ds.Cash_Register.Select("Payment_Mode = 'DD'").Count > 0 Then
                DDPay = ds.Cash_Register.Compute("sum(Amount)", "Entry_Type = 'Payment' and Payment_Mode = 'DD'")
            End If
        End If
        TotalBal = openings + Receipt - Payment

        ' LblOpening.Text = "Opening : " & openings
        ' LblBal.Text = "Closing : " & TotalBal
        ' LblReceipt.Text = "Receipt : " & Receipt
        ' LblPay.Text = "Payment : " & Payment
        ' LblSmartCard.Text = "SmartCard :" & SmartCard
        ' LblBank.Text = "Bank :" & Bank
        ' Lblcheque.Text = "Cheque :" & Cheque
        ' LblDD.Text = "DD :" & DD
        ' LblRTGS.Text = "RTGS :" & RTGS
        LblReceipt.Text = "    Opening :  " & openings & "    Receipt :  " & Receipt & "    Payment :  " & Payment & "    Closing :  " & TotalBal
        LblRecDtls.Text = "Cash :  " & CashRec & "   Bank :  " & BankRec & "   SmartCard :  " & SmartCardRec & "   Cheque :  " & ChequeRec & "  DD :  " & DDRec & "   RTGS :  " & RTGSRec
        LblPayDtls.Text = "Cash :  " & CashPay & "   Bank :  " & BankPay & "   SmartCard :  " & SmartCardPay & "   Cheque :  " & ChequePay & "  DD :  " & DDPay & "   RTGS :  " & RTGSPay

    End Sub
    'Private Sub load_Entry_Head()

    '    Dim bkds As New BkDS
    '    Dim TempDs As New BkDS

    '    Dim objDA As New BkDA

    '    bkds = objDA.Get_entryhead

    '    If Cmb_Head.Text = "Receipt" Then
    '        TempDs.Merge(bkds.Entry_Head.Select("Entry_Type='R'"))
    '    Else
    '        TempDs.Merge(bkds.Entry_Head.Select("Entry_Type='P'"))
    '    End If

    '    Cmb_Head.DataSource = TempDs.Entry_Head.DefaultView
    '    cmb_Type.ValueMember = "Entryhead_Id"
    '    cmb_Type.DisplayMember = "Entry_Head"
    '    cmb_Type.AutoCompleteMode = AutoCompleteMode.SuggestAppend
    '    cmb_Type.AutoCompleteSource = AutoCompleteSource.ListItems
    '    cmb_Type.SelectedIndex = -1

    'End Sub

    Private Sub BtnFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnFind.Click
        Find()
    End Sub

    Private Sub FrmCashRegisterDetails_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Initlz()
    End Sub

    Private Sub BtnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnEdit.Click
        Load_Edit()
    End Sub

    Private Sub Load_Edit()

        If ListView1.SelectedItems.Count = 0 Then
            MsgBox("Please select an item..", MsgBoxStyle.Information, "Information")
            Exit Sub
        End If
        Dim ObjCR As FrmCashRegister
        Dim EntryType As String

        Id = (ListView1.SelectedItems(0).Text)
        EntryType = ds.Cash_Register.Select("Id = '" & Id & "'", "").First.Item("Entry_Type").ToString

        ObjCR = New FrmCashRegister(Id, EntryType, False)
        ObjCR.StartPosition = FormStartPosition.CenterScreen
        If ObjCR.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Find()
        End If

    End Sub

    

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub BtnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        Me.Dispose()
    End Sub

    Private Sub TxtSearch_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TxtSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            Find()
        End If
    End Sub

    Private Sub BtnReceipt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnReceipt.Click
        Add_Recipt()
    End Sub

    Private Sub Add_Recipt()
        Id = 0
        Dim ObjCR As FrmCashRegister
        ObjCR = New FrmCashRegister(Id, Rec, False)
        ObjCR.StartPosition = FormStartPosition.CenterScreen
        If ObjCR.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Find()
        End If
    End Sub

    Private Sub BtnPayment_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnPayment.Click
        Add_Payment()
    End Sub

    Private Sub Add_Payment()
        Id = 0
        Dim ObjCR As FrmCashRegister
        ObjCR = New FrmCashRegister(Id, Pay, False)
        ObjCR.StartPosition = FormStartPosition.CenterScreen
        If ObjCR.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Find()
        End If
    End Sub

    Private Sub ListView1_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.DoubleClick
        Load_Edit()
    End Sub

    Private Sub FrmCashRegisterDetails_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseDown
        drag = True 'Sets the variable drag to true.
        mousex = Windows.Forms.Cursor.Position.X - Me.Left 'Sets variable mousex
        mousey = Windows.Forms.Cursor.Position.Y - Me.Top 'Sets variable mousey
    End Sub

    Private Sub FrmCashRegisterDetails_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseMove
        If drag Then
            Me.Top = Windows.Forms.Cursor.Position.Y - mousey
            Me.Left = Windows.Forms.Cursor.Position.X - mousex
        End If
    End Sub

    Private Sub FrmCashRegisterDetails_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseUp
        drag = False
    End Sub

    Private Sub FrmCashRegisterDetails_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            Find()
        End If
    End Sub

    Private Sub ChkDateFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Find()
    End Sub

    Private Sub BtnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnExport.Click
        FrmMDI.PleaseWait(True)
        Detailed_Excel()
        FrmMDI.PleaseWait(False)
    End Sub

    Private Sub Detailed_Excel()

        Dim Dr As BkDS.Cash_RegisterRow

        Dim FileSaveAs As String = ""
        Dim Max_Count As Integer = 0

        ds = ObjDA.Get_cash_RegisterExcelDs(DtpFrom.Value, DtpTo.Value)

        If SaveDlg.ShowDialog = Windows.Forms.DialogResult.OK Then
            FileSaveAs = SaveDlg.FileName
        Else
            '    Ds.Clear()
            Exit Sub
        End If

        Dim excel As New Excel.Application
        Dim wBook As Excel.Workbook
        Dim wSheet As Excel.Worksheet

        wBook = excel.Workbooks.Add()
        wSheet = wBook.ActiveSheet()

        Dim colIndex As Integer = 0
        Dim rowIndex As Integer = 11
        Dim Ci As Integer = 0
        Dim i As Integer = 1
        Dim Max_Col_Index As Integer = 0

        Max_Count = ds.Cash_Register.Rows.Count

        wSheet.Columns.Range("A:F").HorizontalAlignment = 2
        'wSheet.Columns.Range("I:I").HorizontalAlignment = 1
        wSheet.Columns.Range("J:K").HorizontalAlignment = 4
        'wSheet.PageSetup.Orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlLandscape
        Try
            wSheet.PageSetup.PrintTitleRows = "$1:$3"

        Catch ex As Exception

        End Try
        wSheet.Rows(1).Select()
        wSheet.Application.ActiveWindow.FreezePanes = False
        wSheet.Rows(1).select()
        wSheet.Rows.Range("1:1").Font.Bold = True
        ' wSheet.Rows.Range("1:1").Font.Size = 12
        'wSheet.Rows.Range("2:3").Font.Size = 11

        'excel.Cells(1, 1) = PublicShared.Company_Info_Dr.Name
        'excel.Cells(2, 1) = "CASH REGISTER DETAILED REPORT FROM " & UCase(DtpFrom.Text) & " TO " & UCase(DtpTo.Text)

        rowIndex = 1
        Ci += 1
        excel.Cells(rowIndex, Ci) = "Id" : Ci += 1
        excel.Cells(rowIndex, Ci) = "Entry_No" : Ci += 1
        excel.Cells(rowIndex, Ci) = "Entry_Type" : Ci += 1
        excel.Cells(rowIndex, Ci) = "Entry_Date" : Ci += 1
        excel.Cells(rowIndex, Ci) = "Party" : Ci += 1
        excel.Cells(rowIndex, Ci) = "Prefix Ref No" : Ci += 1
        excel.Cells(rowIndex, Ci) = "Ref_No" : Ci += 1
        excel.Cells(rowIndex, Ci) = "Amount" : Ci += 1
        excel.Cells(rowIndex, Ci) = "Entry_Head" : Ci += 1
        excel.Cells(rowIndex, Ci) = "Entry_sub_head" : Ci += 1
        excel.Cells(rowIndex, Ci) = "Remarks" : Ci += 1
        excel.Cells(rowIndex, Ci) = "EntryHead_Id" : Ci += 1
        excel.Cells(rowIndex, Ci) = "EntrySubHead_Id" : Ci += 1
        excel.Cells(rowIndex, Ci) = "Branch_Id" : Ci += 1
        excel.Cells(rowIndex, Ci) = "BkId" : Ci += 1
        excel.Cells(rowIndex, Ci) = "PayId" : Ci += 1
        excel.Cells(rowIndex, Ci) = "Payment_Mode" : Ci += 1
        excel.Cells(rowIndex, Ci) = "Bank" : Ci += 1
        excel.Cells(rowIndex, Ci) = "User_Id" : Ci += 1
        excel.Cells(rowIndex, Ci) = "User_Name" : Ci += 1
        excel.Cells(rowIndex, Ci) = "Created_On" : Ci += 1
        excel.Cells(rowIndex, Ci) = "JobcardNo" : Ci += 1
        excel.Cells(rowIndex, Ci) = "Modified_On" : Ci += 1



        rowIndex = 2

        Max_Col_Index = colIndex
        colIndex = Ci

        Dim j As Integer
        j = 0
        Ci = 1

        If ds.Cash_Register.Rows.Count > 0 Then

            For Each Dr In ds.Cash_Register.Rows

                FrmMDI.PleaseWait_Progress("Please Wait...(" & Format(j / Max_Count * 100, "0") & "%)")
                j += 1

                excel.Cells(rowIndex, Ci) = Dr.Id : Ci += 1
                excel.Cells(rowIndex, Ci) = Dr.Entry_No : Ci += 1
                excel.Cells(rowIndex, Ci) = Dr.Entry_Type.Trim : Ci += 1
                excel.Cells(rowIndex, Ci) = Dr.Entry_Date : Ci += 1
                excel.Cells(rowIndex, Ci) = Dr.Party.Trim : Ci += 1
                excel.Cells(rowIndex, Ci) = Dr.Prefix_RefNo : Ci += 1
                excel.Cells(rowIndex, Ci) = Dr.Ref_No.Trim : Ci += 1
                excel.Cells(rowIndex, Ci) = Dr.Amount : Ci += 1
                excel.Cells(rowIndex, Ci) = Dr.Entry_Head.Trim : Ci += 1
                excel.Cells(rowIndex, Ci) = Dr.Entry_sub_head.Trim : Ci += 1
                excel.Cells(rowIndex, Ci) = Dr.Remarks.Trim : Ci += 1
                excel.Cells(rowIndex, Ci) = Dr.EntryHead_ID : Ci += 1
                excel.Cells(rowIndex, Ci) = Dr.EntrySubHead_Id : Ci += 1
                excel.Cells(rowIndex, Ci) = Dr.Branch_Id : Ci += 1
                excel.Cells(rowIndex, Ci) = Dr.BkId : Ci += 1
                excel.Cells(rowIndex, Ci) = Dr.PayId : Ci += 1
                excel.Cells(rowIndex, Ci) = Dr.Payment_Mode : Ci += 1
                excel.Cells(rowIndex, Ci) = Dr.Bank : Ci += 1
                excel.Cells(rowIndex, Ci) = Dr.User_Id : Ci += 1
                excel.Cells(rowIndex, Ci) = Dr.User_Name : Ci += 1
                excel.Cells(rowIndex, Ci) = Dr.Created_On : Ci += 1
                excel.Cells(rowIndex, Ci) = Dr.JobCardNo : Ci += 1
                excel.Cells(rowIndex, Ci) = Dr.Modified_On : Ci += 1



                    'If Dr.Entry_Type = "Receipt" Then
                    '    excel.Cells(rowIndex, Ci) = Dr.Amount : Ci += 2
                    'Else : Dr.Entry_Type = "Payment"
                    '    Ci += 1
                    '    excel.Cells(rowIndex, Ci) = Dr.Amount : Ci += 1
                    'End If
                    rowIndex += 1
                Ci = 1

            Next
        End If

        'excel.Cells(rowIndex, 9).HorizontalAlignment = 4
        'excel.Cells(rowIndex, 9) = "Total :"
        'excel.Cells(rowIndex, 10) = Receipt
        'excel.Cells(rowIndex, 11) = Payment

        'rowIndex += 1

        'excel.Cells(rowIndex, 9).HorizontalAlignment = 4
        'excel.Cells(rowIndex, 9) = "Opening :"
        'excel.Cells(rowIndex, 10) = openings

        'rowIndex += 1

        'excel.Cells(rowIndex, 9).HorizontalAlignment = 4
        'excel.Cells(rowIndex, 9) = "Closing :"
        'excel.Cells(rowIndex, 10) = TotalBal

        'rowIndex += 1
        'excel.Cells(rowIndex, 1).HorizontalAlignment = 4
        'excel.Cells(rowIndex, 1) = "Rec : "

        'excel.Cells(rowIndex, 2).HorizontalAlignment = 4
        'excel.Cells(rowIndex, 2) = "Cash:"
        'excel.Cells(rowIndex, 3) = CashRec

        'excel.Cells(rowIndex, 4).HorizontalAlignment = 4
        'excel.Cells(rowIndex, 4) = "SmartCard :"
        'excel.Cells(rowIndex, 5) = SmartCardRec

        'excel.Cells(rowIndex, 6).HorizontalAlignment = 4
        'excel.Cells(rowIndex, 6) = "DD :"
        'excel.Cells(rowIndex, 7) = DDRec

        'excel.Cells(rowIndex, 8).HorizontalAlignment = 4
        'excel.Cells(rowIndex, 8) = "Cheque :"
        'excel.Cells(rowIndex, 9) = ChequeRec

        'excel.Cells(rowIndex, 10).HorizontalAlignment = 4
        'excel.Cells(rowIndex, 10) = "RTGS :"
        'excel.Cells(rowIndex, 11) = RTGSRec

        'excel.Cells(rowIndex, 12).HorizontalAlignment = 4
        'excel.Cells(rowIndex, 12) = "Bank :"
        'excel.Cells(rowIndex, 13) = BankRec

        'rowIndex += 1

        'excel.Cells(rowIndex, 1).HorizontalAlignment = 4
        'excel.Cells(rowIndex, 1) = "Pay : "

        'excel.Cells(rowIndex, 2).HorizontalAlignment = 4
        'excel.Cells(rowIndex, 2) = "Cash:"
        'excel.Cells(rowIndex, 3) = CashPay

        'excel.Cells(rowIndex, 4).HorizontalAlignment = 4
        'excel.Cells(rowIndex, 4) = "SmartCard :"
        'excel.Cells(rowIndex, 5) = SmartCardPay


        'excel.Cells(rowIndex, 6).HorizontalAlignment = 4
        'excel.Cells(rowIndex, 6) = "DD :"
        'excel.Cells(rowIndex, 7) = DDPay

        'excel.Cells(rowIndex, 8).HorizontalAlignment = 4
        'excel.Cells(rowIndex, 8) = "Cheque :"
        'excel.Cells(rowIndex, 9) = ChequePay


        'excel.Cells(rowIndex, 10).HorizontalAlignment = 4
        'excel.Cells(rowIndex, 10) = "RTGS :"
        'excel.Cells(rowIndex, 11) = RTGSPay

        'excel.Cells(rowIndex, 12).HorizontalAlignment = 4
        'excel.Cells(rowIndex, 12) = "Bank :"
        'excel.Cells(rowIndex, 13) = BankPay

        'rowIndex += 1


        wSheet.Rows.Range(rowIndex & ":" & rowIndex).Font.Bold = True


        Max_Col_Index = colIndex - 1
        rowIndex -= 5


        wSheet.Columns.AutoFit()

        wSheet.Columns.Range("A:A").ColumnWidth = 5



        Try
            'wSheet.Range("A1", excel.Cells(rowIndex, Max_Col_Index)).Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous
            ' wSheet.Range("A1", excel.Cells(rowIndex, Max_Col_Index)).Borders.Weight = 2
            wSheet.Range("A1", excel.Cells(rowIndex, Max_Col_Index)).Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous

        Catch ex As Exception
        End Try

        Try
            If System.IO.File.Exists(FileSaveAs) Then
                System.IO.File.Delete(FileSaveAs)
            End If
            wBook.SaveAs(FileSaveAs)
        Catch ex As Exception
            MessageBox.Show("Please close Excel Workbook  " & FileSaveAs & " " & vbCrLf & " and Try Again")
            Exit Sub
        End Try
        excel.Workbooks.Open(FileSaveAs)
        excel.Visible = True

    End Sub

    Private Sub ListView1_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ListView1.KeyDown
        If e.KeyCode = Keys.Enter Then
            Load_Edit()
        End If
    End Sub

    Private Sub CmbSearchBy_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CmbSearchBy.KeyDown
        If e.KeyCode = Keys.Enter Then
            TxtSearch.Focus()
        End If
    End Sub

    Private Sub DtpFrom_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DtpFrom.ValueChanged

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
    Public Function Read_Settings(ByVal S_Key As String) As String
        Dim Value As String = ""
        Try
            Value = PublicShared.Settings_Dt.Select("S_Key='" & S_Key & "'").First.Item("S_Value").ToString
        Catch ex As Exception
        End Try
        Return Value
    End Function
    Private Sub BtnPrint_Click(sender As System.Object, e As System.EventArgs) Handles BtnPrint.Click
        ' If ListView1.SelectedItems.Count > 0 Then
        ' Print(Val(ListView1.SelectedItems(0).Text))
        'End If
    End Sub

    'Public Sub Print(ByVal Id As Integer)

    '    If Id <= 0 Then
    '        Exit Sub
    '    End If

    '    Dim htp As New Hashtable
    '    Dim RptView As FrmCrptRptViewer
    '    Dim ObjRpt As New CryRptRecpt
    '    Dim Recpt_Total As Double = 0
    '    Dim TempDs As New BkDS
    '    ds = New BkDS
    '    ObjDA = New BkDA

    '    If Id <> 0 Then
    '        ds = ObjDA.Get_cash_Register(Id, DtpFrom.Value, DtpTo.Value, htp)

    '    End If

    '    TempDs = Nothing

    '    htp.Clear()
    '    htp.Add("Company", PublicShared.Company_Info_Dr.Name)
    '    'htp.Add("Place", PublicShared.Company_Info_Dr.Place)
    '    ' htp.Add("Address", PublicShared.Company_Info_Dr.Address)

    '    RptView = New FrmCrptRptViewer(ObjRpt, ds, htp)
    '    RptView.StartPosition = FormStartPosition.CenterScreen
    '    RptView.WindowState = FormWindowState.MaximizedC:\Users\mohazin\Desktop\UNNOONNY\Unnoonny Cash Register\CASHIER APPLICATION  UNNOONNY1\AutoX\FrmFinance_FollowUp.vb
    '    RptView.Show()
    'End Sub


    Private Sub Import_Click(sender As System.Object, e As System.EventArgs) Handles ImportBtn.Click

        Dim i As Integer = 0
        Dim ImportDs As New BkDS
        If ListView1.SelectedItems.Count > 0 Then
            ImportDs.Merge(ds.Cash_Register.Select("Id='" & ListView1.SelectedItems.Item(0).SubItems(0).Text & "'"))
        Else
            ImportDs = ds
        End If

        Dim MaxCount As Integer = ImportDs.Cash_Register.Rows.Count
        Dim ObjSDa As New CommonDA

        PanelLoading.Visible = True

        LblStatus.Refresh()
        Lblimporting.Refresh()

        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = 100

        Dim y = 100 / MaxCount
        Dim x = 100 / MaxCount
        x -= y
        For Each Me.dr In ImportDs.Cash_Register.Rows
            x = x + y
            '  ProgressBar1.Value = ProgressBar1.Value + (100 / MaxCount)
            ProgressBar1.Value = x
            ProgressBar1.Refresh()

            If dr.Entry_Head = "SERVICE" Then
                dr.Ref_No = dr.Prefix_RefNo + dr.Ref_No
            End If

            Post_To_Tally(dr)
            i += 1

            LblStatus.Text = "Process Executed : " & i & "/" & MaxCount
            LblStatus.Refresh()
        Next
        ProgressBar1.Value = 0
        LblStatus.Text = ""
        PanelLoading.Visible = False
        MsgBox(Import_Log, MsgBoxStyle.Information, "Auto X")
        Import_Log = ""
        ' dr = ds.Cash_Register.Select("id=").Count
        'Post_To_Tally(dr)
        ' End If
        '  If ListView1.SelectedItems.Count = 1 Then
        '  Post_To_Tally(Val(ListView1.SelectedItems(0).Text))
        'Import_Expence()

    End Sub

    Private Function Post_To_Tally(ByVal Dr As BkDS.Cash_RegisterRow) As Boolean

        Dim Status As Boolean = False
        Dim ExpDS As New DataSet
        Dim StrXmlReq As String
        Dim RespondDs As DataSet

        Dim Already_Exist As Boolean = False
        Dim Party_Ledger As String
        Dim VDetails As New DataSet
        '   Dim ObjSDa As New DmsDA

        If Dr.Entry_Type = "Receipt" Then

            ExpDS = Export_Xml(Dr.Entry_Date, Read_Ledgers(Dr.Entry_Head & "_" & Dr.Entry_Type & "_VoucherType"))
            If ExpDS.Tables.Count = 0 Then
                If MsgBox("Failed to check existing entries from tally.." & vbCrLf & "Do you want to continue...", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                    Return False
                End If
            End If

            Already_Exist = Is_Already_Exist(IIf(Dr.Entry_Head <> "Sales", Dr.Entry_No, Dr.JobCardNo), ExpDS, Read_Ledgers(Dr.Entry_Head & "_" & Dr.Entry_Type & "_VoucherType"))

            If Already_Exist Then
                MsgBox("Voucher already exist in tally!", MsgBoxStyle.Information, "Auto X")
                Return False
            End If

            'Do
            Try

                '   Dim PartyName As String = Dr.Party.Trim & " (" & Dr.JobCardNo.Trim & ")"          
                '  Get_Ledgers(PartyName, Read_Settings(Dr.Entry_Head & "_" & Dr.Entry_Type & "_PartyLedgerParent"))

                If Dr.JobCardNo.Contains("RJC") Then
                    Party_Ledger = Read_Ledgers("Service_PartyLedger_SLI")
                ElseIf Dr.JobCardNo.Contains("SSI") Then
                    Party_Ledger = Read_Ledgers("Service_PartyLedger_SSI")
                Else
                    Party_Ledger = Read_Ledgers("Service_PartyLedger_ILIP")
                End If


                If Party_Ledger = "" Then
                    Show_ClipBoard("PartyLedger", "PartyLedger is Missing for " & vbCrLf & Dr.JobCardNo)
                    Return ""
                    Exit Function
                End If

                StrXmlReq = XmlFormat_Recpt(Party_Ledger, Dr, Read_Ledgers(Dr.Entry_Head & "_" & Dr.Entry_Type & "_VoucherType"))

                If Read_Settings("ShowRequestXML_YN") = "Y" Then
                    Show_ClipBoard("XML", StrXmlReq)
                End If

                If StrXmlReq = "" Then
                    Dr.PostedToTally = 0
                Else
                    RespondDs = Request_Tally_Method1(StrXmlReq)
                    If Val(RespondDs.Tables("response").Rows(0).Item("CREATED").ToString) = 1 Then
                        Dr.PostedToTally = 1
                        Status = True

                        'Get Voucher Number
                        StrXmlReq = XmlFormat_Voucher_ID(RespondDs.Tables("response").Rows(0).Item("LASTVCHID").ToString)
                        VDetails = Request_Tally_Method1(StrXmlReq)
                        If VDetails.Tables.Contains("VOUCHERNUMBER") Then
                            'Dr.Ref_No = VDetails.Tables("VOUCHERNUMBER").Rows(0).Item("VOUCHERNUMBER_Text").ToString
                            'Dr.InvRefNo = TxtInvRefNo.Text
                            'TxtInvRefNo.Refresh()
                        End If

                    End If
                    Import_Log += Dr.Ref_No & " - " &
                                "Created :" & RespondDs.Tables("response").Rows(0).Item("CREATED").ToString &
                                ",Altered :" & RespondDs.Tables("response").Rows(0).Item("ALTERED").ToString &
                                ",Errors :" & RespondDs.Tables("response").Rows(0).Item("ERRORS").ToString

                    If Val(RespondDs.Tables("response").Rows(0).Item("CREATED").ToString) > 0 And Val(RespondDs.Tables("response").Rows(0).Item("ERRORS").ToString) = 0 Then
                        ObjDA = New BkDA
                        ObjDA.Receipt_Tally_Imported(Dr.Entry_No)
                        'Else
                        '    FailedStatus_Log += Dr.BkNo & " , "
                        '    FailedStatus_Int += 1
                    End If

                    If Val(RespondDs.Tables("response").Rows(0).Item("ERRORS").ToString) > 0 Then
                        Import_Log += " ," & RespondDs.Tables("response").Rows(0).Item("LINEERROR").ToString
                    End If
                    Import_Log += vbCrLf
                End If


            Catch ex As Exception
                Import_Log += vbCrLf & Dr.Ref_No & " -Error- " & ex.Message
            End Try
        End If
        Return Status

    End Function

    Public Function Is_Already_Exist(ByVal EntryNo As String, ByVal Ds As DataSet, ByVal VOUCHERTYPENAME As String) As Boolean
        Dim Value As String = ""
        Dim Status As Boolean = False
        'Dim Service_Receipt_VoucherType As String = Read_Settings("SERVICE_Receipt_VoucherType")
        If Ds.Tables.Count > 0 Then
            If Ds.Tables.Contains("VOUCHER") Then
                Try
                    Value = Ds.Tables("VOUCHER").Select("VOUCHERNUMBER = '" & EntryNo & "'" &
                              "And VOUCHERTYPENAME = '" & VOUCHERTYPENAME & "'").First.Item("VOUCHERNUMBER").ToString
                    Status = True
                Catch ex As Exception
                End Try
            End If
        End If
        Return Status
    End Function

    Public Sub Show_ClipBoard(ByVal Title As String, ByVal Msg As String)
        Dim ObjCl As FrmClipBoard
        ObjCl = New FrmClipBoard(Title, Msg)
        ObjCl.StartPosition = FormStartPosition.CenterScreen
        ObjCl.ShowDialog(Me)
    End Sub

    'Public Function Request_Tally_Method1(ByVal StrXmldata As String) As DataSet

    '    Dim i As Integer = 1

    '    Dim Responsestr As String
    '    Dim XMLDOM As New MSXML2.DOMDocument30
    '    Dim ObjXml As New MSXML2.ServerXMLHTTP

    '    Dim XmlDS As New DataSet
    '    Dim Stream As StringReader
    '    Dim Reader As XmlTextReader

    '    Try
    '        ObjXml.open("POST", TallyHost, False)
    '        ObjXml.send(StrXmldata)
    '        Responsestr = ObjXml.responseText

    '        If Responsestr <> Nothing Then
    '            XMLDOM.loadXML(Responsestr)
    '            Stream = New StringReader(Responsestr)
    '            Reader = New XmlTextReader(Stream)
    '            XmlDS.Clear()
    '            XmlDS.ReadXml(Reader)
    '        End If

    '    Catch ex As Exception
    '        Return XmlDS
    '    End Try

    '    Return XmlDS

    'End Function

    Public Function Validate_Ledger_Name(ByVal Ledger_Name As String) As String

        Dim Ds As New DataSet
        Dim i As Integer = 1
        Dim XmlDS As New DataSet
        Dim StrXmldata As String
        Dim PARENT As String = Read_Settings("SalesReceiptPartyLedgerParent")
        Dim Tally_ledger As String
        Try

            StrXmldata = XmlLedgerList()
            XmlDS = Request_Tally_Method2(StrXmldata)
            If XmlDS.Tables.Contains("LEDGER") Then
                If XmlDS.Tables("LEDGER").Select("[PARENT] = '" & PARENT & "' And [NAME] LIKE '%" & dr.Ref_No.Trim & "%'", "").Count > 0 Then
                    Tally_ledger = XmlDS.Tables("LEDGER").Select("[PARENT] = '" & PARENT & "' And [NAME] LIKE '%" & dr.Ref_No.Trim & "%'", "").First.Item("NAME").ToString
                    If Tally_ledger <> Ledger_Name Then
                        Ledger_Name = Tally_ledger
                    End If
                End If
            End If

        Catch ex As Exception
        End Try

        Return Ledger_Name
    End Function

    Public Shared Function Read_Ledgers(ByVal S_Key As String) As String
        Dim Value As String = ""
        Try
            Value = PublicShared.Ledgers_Dt.Select("S_Key='" & S_Key & "'").First.Item("S_Value").ToString
        Catch ex As Exception
        End Try
        Return Value
    End Function

    Public Function Get_Ledgers(ByVal Ledger_Name As String, ByVal Parent As String) As String

        Dim Qry As String
        Dim TallyCon As New OdbcConnection(Read_Settings("TallyODBC"))
        Dim Cmd As New OdbcCommand
        Dim Da As New OdbcDataAdapter
        Dim StrXmldata As String = ""
        'Dim Parent As String = Read_Settings("Sales_Receipt_VoucherType")

        Cmd.Connection = TallyCon

        Try

            TallyCon.Open()

            DsLedger.Clear()

            'Stk_Name = "VASUDEVAN .P.T 17IN02561"
            'Parent = "Test"
            'Qry = "Select $NAME,$PARENT from ListOfLedgers WHERE $NAME = '" & Stk_Name & "'"
            Qry = "Select $NAME from ListOfLedgers WHERE $PARENT = '" & Parent & "'"

            Cmd.CommandText = Qry
            Da.SelectCommand = Cmd
            Da.Fill(DsLedger, "Ledger")

            If DsLedger.Tables("Ledger").Select("$NAME Like '%" & Ledger_Name & "%'").Count = 0 Then
                Ledger_Name = ""
                StrXmldata = XmlFormat_Ledger(Ledger_Name, Parent)
                Request_Tally_Method1(StrXmldata)
            Else
                Ledger_Name = DsLedger.Tables("Ledger").Select("$NAME Like '%" & Ledger_Name & "%'").First().Item("$NAME")
            End If

            TallyCon.Close()
            TallyCon.Dispose()

        Catch ex As Exception
            Ledger_Name = ""
            Return Ledger_Name

        End Try
        Return Ledger_Name
    End Function

    Public Function XmlLedgerList() As String

        Dim strXmldata As String = ""

        Try
            strXmldata = _
                   "<ENVELOPE>" & _
                   " <HEADER>" & _
                   " <TALLYREQUEST>Export Data</TALLYREQUEST>" & _
                   " </HEADER>" & _
                   " <BODY>" & _
                   " <EXPORTDATA>" & _
                   " <REQUESTDESC>" & _
                   " <REPORTNAME>List of accounts</REPORTNAME>" & _
                   " <SVEXPORTFORMAT>$$SysName:XML</SVEXPORTFORMAT >" & _
                   " </REQUESTDESC>" & _
                   " </EXPORTDATA>" & _
                   " </BODY>" & _
                   "</ENVELOPE>"

        Catch ex As Exception
        End Try

        Return strXmldata

    End Function

    Public Function XmlFormat_Ledger(ByVal LedgerName As String, ByVal parent As String) As String
        Dim Xml As String = ""

        ' Dim parent As String = Read_Settings("SalesReceiptPartyLedgerParent")

        Try

            Xml = "<ENVELOPE>" & _
                      "<HEADER>" & _
                          "<TALLYREQUEST>Import Data</TALLYREQUEST>" & _
                      "</HEADER>" & _
                      "<BODY>" & _
                          "<IMPORTDATA>" & _
                              "<REQUESTDESC>" & _
                                  "<REPORTNAME>All Masters</REPORTNAME>" & _
                              "</REQUESTDESC>" & _
                              "<REQUESTDATA>" & _
                                "<TALLYMESSAGE xmlns:UDF=""TallyUDF"">" & _
                                        "<LEDGER Action = ""Create"">" & _
                                            "<NAME>" & LedgerName & "</NAME>" & _
                                            "<PARENT>" & parent & "</PARENT>" & _
                                        "</LEDGER>" & _
                                  "</TALLYMESSAGE>" & _
                              "</REQUESTDATA>" & _
                          "</IMPORTDATA>" & _
                      "</BODY>" & _
                  "</ENVELOPE>"


        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try

        Return Xml

    End Function

    Public Function XmlFormat_Recpt(ByVal PartyLedger As String, ByVal dr As BkDS.Cash_RegisterRow, ByVal VOUCHERTYPENAME As String) As String

        Dim Xml As String = ""
        Dim Additional_Ledgers As String = ""
        Dim Main_Ledgers As String = ""
        Dim RecptDate As Date = dr.Entry_Date
        Dim RecptDateString As String = Format(RecptDate, "yyyyMMdd")
        Dim ServRecptCostCentre As String = Read_Settings("ServRecptCostCentre")
        Dim VOUCHERNUMBER As String = dr.Entry_No
        Dim Narration As String = ""

        Narration = dr.Remarks.Trim + " ," + dr.Party + " ," + dr.JobCardNo.Trim
        ' Narration += dr.Ref_No.Trim
        'Narration += IIf(CmbBank.Text.Trim = "", "", (" , " & CmbBank.Text))
        'Narration += IIf(TxtPayDetails.Text.Trim = "", "", (" , " & TxtPayDetails.Text))
        'Narration += IIf(TxtNarration.Text.Trim = "", "", (" , " & TxtNarration.Text))
        'Narration += " , " & CmbVehName.Text

        Narration = UCase(Narration)

        Main_Ledgers = Create_XML_ALLLEDGERENTRIES_PARTYLEDGER(PartyLedger, dr.Amount, dr.JobCardNo, True)


        If dr.Payment_Mode = "SmartCard" Then
            Additional_Ledgers += Create_XML_ALLLEDGERENTRIES_PARTYLEDGER(Read_Ledgers("LedgerServBankSwip"), -dr.Amount, "", False)
        ElseIf dr.Payment_Mode = "Bank" Or dr.Payment_Mode = "DD" Or dr.Payment_Mode = "RTGS" Or dr.Payment_Mode = "Cheque" Then
            Additional_Ledgers += Create_XML_ALLLEDGERENTRIES_PARTYLEDGER(Read_Ledgers("LedgerServBank"), -dr.Amount, "", False)
        ElseIf dr.Payment_Mode = "Cash" Then
            Additional_Ledgers += Create_XML_ALLLEDGERENTRIES_PARTYLEDGER(Read_Ledgers("LedgerServiceCash"), -dr.Amount, "", False)
        End If


        Try
            Xml = "<ENVELOPE>" & _
                      "<HEADER>" & _
                          "<TALLYREQUEST>Import Data</TALLYREQUEST>" & _
                      "</HEADER>" & _
                      "<BODY>" & _
                          "<IMPORTDATA>" & _
                             "<REQUESTDESC>" & _
                             "<REPORTNAME>All Masters</REPORTNAME>" & _
                         "</REQUESTDESC>" & _
                              "<REQUESTDATA>" & _
                                 "<TALLYMESSAGE xmlns:UDF=""TallyUDF"">" & _
                                      "<VOUCHER>" & _
                                          "<VOUCHERTYPENAME>" & VOUCHERTYPENAME & "</VOUCHERTYPENAME>" & _
                                          "<DATE>" & RecptDateString & "</DATE>" & _
                                          "<EFFECTIVEDATE>" & RecptDateString & "</EFFECTIVEDATE>" & _
                                          "<VOUCHERNUMBER>" & VOUCHERNUMBER & "</VOUCHERNUMBER>" & _
                                          "<PARTYLEDGERNAME>" & PartyLedger & "</PARTYLEDGERNAME>" & _
                                          "<BASICVOUCHERCHEQUENAME>" & PartyLedger & "</BASICVOUCHERCHEQUENAME>" & _
                                          "<NARRATION>" & Narration & "</NARRATION>" & _
                                           Main_Ledgers & Additional_Ledgers & _
                                      "</VOUCHER>" & _
                                  "</TALLYMESSAGE>" & _
                              "</REQUESTDATA>" & _
                          "</IMPORTDATA>" & _
                      "</BODY>" & _
                  "</ENVELOPE>"


        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try

        Return Xml

    End Function
    Public Function XmlFormat_Voucher_ID(ByVal ID As String) As String

        Dim strXmldata As String = ""

        Try
            strXmldata = _
                    "<ENVELOPE>" & _
                    "<HEADER>" & _
                    "<VERSION>1</VERSION>" & _
                    "<TALLYREQUEST>EXPORT</TALLYREQUEST>" & _
                    "<TYPE>Object</TYPE>" & _
                    "<SUBTYPE>VOUCHER</SUBTYPE>" & _
                    "<ID TYPE=""Name"">ID:" & ID & "</ID>" & _
                    "</HEADER>" & _
                    "<BODY>" & _
                    "<DESC>" & _
                    "<STATICVARIABLES>" & _
                    "<SVEXPORTFORMAT>$$SysName:XML</SVEXPORTFORMAT>" & _
                    "</STATICVARIABLES>" & _
                    "<FETCHLIST>" & _
                        "<FETCH>Date</FETCH>" & _
                        "<FETCH>VoucherTypeName</FETCH>" & _
                        "<FETCH>VoucherNumber</FETCH>" & _
                    "</FETCHLIST>" & _
                    "</DESC>" & _
                    "</BODY>" & _
                    "</ENVELOPE>"


        Catch ex As Exception

        End Try

        Return strXmldata

    End Function

    Public Function Export_Xml(ByVal Export_Date As Date, ByVal VoucherTypeName As String) As DataSet

        Dim Ds As New DataSet
        Dim i As Integer = 1
        Dim XmlDS As New DataSet
        Dim StrXmldata As String

        Try

            StrXmldata = XmlFormat_Voucher_Register(Format(Export_Date, "yyyyMMdd"), Format(Export_Date, "yyyyMMdd"), VoucherTypeName)

            XmlDS = Request_Tally_Method1(StrXmldata)
            If XmlDS.Tables.Count = 0 Or XmlDS.Tables.Contains("VOUCHER") = False Then
                XmlDS = Request_Tally_Method2(StrXmldata)
            End If

        Catch ex As Exception

        End Try

        Return XmlDS

    End Function
    Public Function XmlFormat_Voucher_Register(ByVal FromDate As String, ByVal ToDate As String, ByVal VoucherTypeName As String) As String

        Dim strXmldata As String = ""

        Try

            strXmldata =
                  "<ENVELOPE>" &
                  " <HEADER>" &
                  " <TALLYREQUEST>Export Data</TALLYREQUEST>" &
                  " </HEADER>" &
                  " <BODY>" &
                  " <EXPORTDATA>" &
                  " <REQUESTDESC>" &
                  " <REPORTNAME>Voucher Register</REPORTNAME>" &
                  " <STATICVARIABLES>" &
                  " <SVCURRENTCOMPANY>" & BranchName.Replace("&", "&amp;") & "</SVCURRENTCOMPANY>" &
                  " <SVFROMDATE TYPE='Date'>" & FromDate & "</SVFROMDATE>" &
                  " <SVTODATE TYPE='Date'>" & ToDate & "</SVTODATE>" &
                  " <SVEXPORTFORMAT>$$SysName:XML</SVEXPORTFORMAT >" &
                  " <VOUCHERTYPENAME>" & VoucherTypeName & "</VOUCHERTYPENAME>" &
                  " </STATICVARIABLES>" &
                  " </REQUESTDESC>" &
                  " </EXPORTDATA>" &
                  " </BODY>" &
                  "</ENVELOPE>"


        Catch ex As Exception

        End Try

        Return strXmldata

    End Function
    Public Function Request_Tally_Method1(ByVal StrXmldata As String) As DataSet

        Dim i As Integer = 1

        Dim Responsestr As String
        Dim XMLDOM As New MSXML2.DOMDocument30
        Dim ObjXml As New MSXML2.ServerXMLHTTP

        Dim XmlDS As New DataSet
        Dim Stream As StringReader
        Dim Reader As XmlTextReader

        Try

            ObjXml.open("POST", TallyHost, False)
            ObjXml.send(StrXmldata)
            Responsestr = ObjXml.responseText

            If Responsestr <> Nothing Then
                XMLDOM.loadXML(Responsestr)
                Stream = New StringReader(Responsestr)
                Reader = New XmlTextReader(Stream)
                XmlDS.Clear()
                XmlDS.ReadXml(Reader)
            End If

        Catch ex As Exception
            Return XmlDS
        End Try

        Return XmlDS

    End Function

    Public Function Request_Tally_Method2(ByVal RequestXML) As DataSet

        Dim TallyRequest As HttpWebRequest
        Dim byteArray As Byte()
        Dim dataStream As Stream
        Dim response As HttpWebResponse
        Dim ResponseStr As String
        Dim reader As StreamReader
        Dim responseFromTallyServer As String
        Dim TallyResponseDataSet As DataSet

        'RequestXML = "<ENVELOPE><HEADER><TALLYREQUEST>Export Data</TALLYREQUEST></HEADER><BODY><EXPORTDATA><REQUESTDESC><REPORTNAME>List of Accounts</REPORTNAME><STATICVARIABLES><SVEXPORTFORMAT>$$SysName:XML</SVEXPORTFORMAT><ACCOUNTTYPE>All Inventory Masters</ACCOUNTTYPE></STATICVARIABLES></REQUESTDESC></EXPORTDATA></BODY></ENVELOPE>";
        TallyRequest = WebRequest.Create(TallyHost)
        TallyRequest.UserAgent = ".NET Framework Example Client"

        ' ((HttpWebRequest)TallyRequest).UserAgent = ".NET Framework Example Client"
        ' Set the Method property of the request to POST.
        TallyRequest.Method = "POST"
        ' Create POST data and convert it to a byte array.
        byteArray = Encoding.UTF8.GetBytes(RequestXML)
        ' Set the ContentType property of the WebRequest.
        TallyRequest.ContentType = "application/x-www-form-urlencoded"
        ' Set the ContentLength property of the WebRequest.
        TallyRequest.ContentLength = byteArray.Length
        ' Get the request stream.
        dataStream = TallyRequest.GetRequestStream()
        ' Write the data to the request stream.
        dataStream.Write(byteArray, 0, byteArray.Length)
        ' Close the Stream object.
        dataStream.Close()
        ' Get the response.
        response = TallyRequest.GetResponse()
        ' Display the status.
        ResponseStr = response.StatusDescription.ToString()
        ' Get the stream containing content returned by the server.
        dataStream = response.GetResponseStream()
        ' Open the stream using a StreamReader for easy access.
        reader = New StreamReader(dataStream)
        ' Read the content.
        responseFromTallyServer = reader.ReadToEnd().ToString()
        'Display the content.
        'string ResponseFromtally=responseFromServer.ToString();
        TallyResponseDataSet = New DataSet()
        TallyResponseDataSet.ReadXml(New StringReader(responseFromTallyServer))
        ' Clean up the streams.
        reader.Close()
        dataStream.Close()
        response.Close()
        byteArray = Nothing
        response = Nothing
        responseFromTallyServer = Nothing
        response = Nothing
        dataStream = Nothing
        ' RequestClient.open("Get", "http://localhost:9000/", false, null, null);
        ' RequestClient.send(
        ' IXMLDOMNode ResponseXml = (IXMLDOMNode)RequestClient.responseXML;
        Return TallyResponseDataSet

    End Function

    Private Function Create_XML_ALLLEDGERENTRIES_PARTYLEDGER(ByVal LEDGERNAME As String, ByVal AMOUNT As String, ByVal BillNo As String, ByVal BillAllocation As Boolean) As String

        Dim Str As String = ""
        Str = "<ALLLEDGERENTRIES.LIST>" & _
              "<LEDGERNAME>" & LEDGERNAME & "</LEDGERNAME>" & _
              "<ISDEEMEDPOSITIVE>" & IIf(AMOUNT < 0, "Yes", "No") & "</ISDEEMEDPOSITIVE> " & _
              "<AMOUNT>" & AMOUNT & "</AMOUNT> " & _
              "<ISPARTYLEDGER>Yes</ISPARTYLEDGER> " & _
              "<ASORIGINAL>No</ASORIGINAL> "
        If BillAllocation Then
            Str += "<BILLALLOCATIONS.LIST>" &
              "<NAME>" & BillNo & "</NAME>" &
              "<BILLTYPE>Agst Ref</BILLTYPE>" &
              "<AMOUNT>" & AMOUNT & "</AMOUNT>" &
              "</BILLALLOCATIONS.LIST>"
        End If
        Str += "</ALLLEDGERENTRIES.LIST>"
        Return Str
    End Function
    Public Function Get_JobCard_BillSummery()

        Dim MyExcel As Excel.Application
        Dim MyWorkBook As Excel.Workbook
        Dim WorkSheet As Excel.Worksheet
        'File_Name = Read_Settings("Sales_Booking_Path")

        If Not IO.File.Exists(File_Name) Then
            MsgBox("File not exists..", MsgBoxStyle.Information, "Imports")
            Return Nothing
            Exit Function
        End If

        Dim status As Boolean = False
        Dim Ds As New BkDS

        MyExcel = New Excel.Application
        MyWorkBook = MyExcel.Workbooks.Open(File_Name, True, True, , , , True, True)
        Dim da As New OleDb.OleDbDataAdapter
        WorkSheet = MyWorkBook.ActiveSheet

        'MsgBox(WorkSheet)
        Try
            Dim cnn As New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + File_Name + ";Extended Properties=""Excel 12.0;HDR=YES;IMEX=1;""")
            da = New OleDb.OleDbDataAdapter("Select * from [" & WorkSheet.Name & "$]", cnn)
            da.Fill(Ds, "bookings")

        Catch ex As Exception
            MsgBox("Import failed!" & vbCrLf & ex.Message)
        Finally
            da.Dispose()
            da = Nothing
        End Try

        CommonDA.Remove_Null(Ds)

        MyExcel.Workbooks.Close()
        MyExcel.Quit()

        CommonDA.Remove_Null(Ds)

        Return Ds

    End Function

    Public Sub Start_File_Search()

        Try
            imp_jobcard_Path = Read_Settings("Service_Path")
            If Directory.Exists(imp_jobcard_Path) Then
                If Not Directory.Exists(Application.StartupPath & "\BackUp") Then
                    Directory.CreateDirectory(Application.StartupPath & "\BackUp")
                End If
                TimerFileSearch.Enabled = True
                TimerFileSearch.Start()
            End If
        Catch ex As Exception
            'Nothing
        End Try
    End Sub

    Private Sub TimerFileSearch_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles TimerFileSearch.Tick
        If Not BgWrkSearch_File.IsBusy Then
            BgWrkSearch_File.RunWorkerAsync()
        End If
    End Sub

    Private Sub BgWrkSearch_File_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BgWrkSearch_File.DoWork
        ' TxtPath.Refresh()
        If Not IO.File.Exists(File_Name) Then
            Dim File As String = ""
            Try
                For Each File In Directory.GetFiles(imp_jobcard_Path)
                    If UCase(IO.Path.GetExtension(File)) = ".XLS" Or UCase(IO.Path.GetExtension(File)) = ".XLSX" Then
                        File_Name = File
                        'Common.Create_Log("Import to Tally", "File found", )
                    End If
                Next
            Catch ex As Exception
                'Nothing
            End Try
        End If
    End Sub

    Private Sub RbtnAll_CheckedChanged(sender As Object, e As EventArgs) Handles RbtnAll.CheckedChanged
        If RbtnAll.Checked = True Then
            Find()
        End If
    End Sub

    Private Sub RbtnActive_CheckedChanged(sender As Object, e As EventArgs) Handles RbtnActive.CheckedChanged
        If RbtnActive.Checked = True Then
            Find()
        End If
    End Sub
End Class