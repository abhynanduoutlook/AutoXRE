Imports System.Data.Odbc
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Xml

Public Class FrmTallySparePur
    Dim SpareDs As TallyDs

    Public Sub New()
        SpareDs = CommonDA.Get_Summary_SparePurchase(Nothing, Nothing)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Dim Prev_fromDate, Prev_ToDate As Date
    Dim ImportDs As TallyDs
    Dim lvItem As New ListViewItem
    Dim TallyHost As String = ""
    Dim Import_Log As String = ""
    Dim End_Process As Boolean
    Dim PublicTallyDS As DataSet

    Dim StockGroupsDS As New DataSet
    Dim ErrStr_QtyType As String = ""
    Dim TaxDs As New DataSet
    Dim PartNo_tax As String = ""
    Dim Ds As TallyDs
    Dim GST_Ds As New TallyDs
    Dim GstDs_Temp As New TallyDs
    Dim ObjDA As New CommonDA
    Dim DsStock As New DataSet
    Dim BranchName As String

    Dim Dr As TallyDs.re_spare_purchaseRow
    Dim Drd As TallyDs.re_spare_purchase_DetailRow

    Private Sub BtnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub

    Private Sub BtnMaximize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnMaximize.Click
        If Me.WindowState = FormWindowState.Maximized Then
            Me.WindowState = FormWindowState.Normal
        Else
            Me.WindowState = FormWindowState.Maximized
        End If
    End Sub

    Private Sub BtnMinimize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnMinimize.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub FrmTallyService_Load(sender As Object, e As EventArgs) Handles Me.Load
        Initlz()
    End Sub

    Private Sub Initlz()

        Create_Header()
        Create_Heads()
        BranchName = Read_Settings("TallyCompanyBranch")
        TallyHost = Read_Settings("TallyHost")
        Load_Gst_Details()

    End Sub

    Private Sub Create_Header()

        ListView1.Columns.Add("id", 0)
        ListView1.Columns.Add("SL.NO", 40)
        ListView1.Columns.Add("GRN NO.", 180)
        ListView1.Columns.Add("Invoice No", 160)
        ListView1.Columns.Add("GRN DATE", 110, HorizontalAlignment.Right)
        ListView1.Columns.Add("Sup.NUMBER", 120)
        ListView1.Columns.Add("Sup.DATE", 160)
        ListView1.Columns.Add("RATE", 100, HorizontalAlignment.Right)
        ListView1.Columns.Add("TAXABLE", 100, HorizontalAlignment.Right)
        ListView1.Columns.Add("IGST", 85, HorizontalAlignment.Right)
        ListView1.Columns.Add("FREIGHT", 100, HorizontalAlignment.Right)
        ListView1.Columns.Add("TOTAL", 100, HorizontalAlignment.Right)
        ListView1.Columns.Add("TALLY", 60, HorizontalAlignment.Center)


    End Sub

    Private Sub Create_Heads()

        ListView2.Columns.Add("ID", 0)
        ListView2.Columns.Add("SL.NO", 40)
        ListView2.Columns.Add("MODEL DESCRIPTION", 180)
        ListView2.Columns.Add("MODEL NO", 250, HorizontalAlignment.Left)
        ListView2.Columns.Add("UOM", 80)
        ListView2.Columns.Add("QTY", 50)
        ListView2.Columns.Add("RATE", 88, HorizontalAlignment.Right)
        ListView2.Columns.Add("IGST%", 60, HorizontalAlignment.Center)
        ListView2.Columns.Add("IGST", 70, HorizontalAlignment.Right)
        ListView2.Columns.Add("FREIGHT%", 60, HorizontalAlignment.Center)
        ListView2.Columns.Add("FREIGHT", 70, HorizontalAlignment.Right)
        ListView2.Columns.Add("DISCOUNT%", 60, HorizontalAlignment.Center)
        ListView2.Columns.Add("DISCOUNT", 70, HorizontalAlignment.Right)
        ListView2.Columns.Add("TAXABLE", 90, HorizontalAlignment.Right)
        ListView2.Columns.Add("AMOUNT (RS)", 88, HorizontalAlignment.Right)

    End Sub

    Private Sub BtnFind_Click(sender As Object, e As EventArgs) Handles BtnFind.Click
        find()
    End Sub

    Private Sub find()

        'If DtpFrom.Value <> Prev_fromDate Or DtpTo.Value <> Prev_ToDate Then

        '    SpareDs = CommonDA.Get_Summary_SparePurchase(DtpFrom.Value, DtpTo.Value)

        '    Prev_fromDate = DtpFrom.Value
        '    Prev_ToDate = DtpTo.Value
        'ElseIf SpareDs.Service.Rows.Count = 0 Then
        SpareDs = CommonDA.Get_Summary_SparePurchase(DtpFrom.Value, DtpTo.Value)

        'End If

        ImportDs = New TallyDs
        Dim TempDs As New TallyDs
        Dim i As Integer = 0

        If RbtActive.Checked = True Then
            ImportDs.Merge(SpareDs.re_spare_purchase.Select("Invoice_No<>'' and to_tally=0"))
            ImportDs.Merge(SpareDs.re_spare_purchase_Detail.Select("Invoice_No<>'' and to_tally=0 "))
        ElseIf RbtImported.Checked = True Then
            ImportDs.Merge(SpareDs.re_spare_purchase.Select("to_tally=1"))
            ImportDs.Merge(SpareDs.re_spare_purchase_Detail.Select("to_tally=1"))
        ElseIf RbtMissing.Checked = True Then
            ImportDs.Merge(SpareDs.re_spare_purchase.Select("Invoice_No='' and to_tally=0"))
            ImportDs.Merge(SpareDs.re_spare_purchase_Detail.Select("Invoice_No='' and to_tally=0"))
        ElseIf RbtAll.Checked = True Then
            ImportDs = SpareDs
        End If

        If CmbSearchBy.Text = "Supplier_Invoice_No" Then
            TempDs.Merge(ImportDs.re_spare_purchase.Select("Supplier_Invoice_No Like '%" & TxtSearch.Text.Trim & "%'"))
            TempDs.Merge(ImportDs.re_spare_purchase_Detail.Select("Supplier_Invoice_No Like '%" & TxtSearch.Text.Trim & "%'"))
            ImportDs = TempDs
        ElseIf CmbSearchBy.Text = "GRN_No" Then
            TempDs.Merge(ImportDs.re_spare_purchase.Select("`GRN No` like '%" & TxtSearch.Text.Trim & "%'"))
            TempDs.Merge(ImportDs.re_spare_purchase_Detail.Select("`GRN No` like '%" & TxtSearch.Text.Trim & "%'"))
            ImportDs = TempDs
        Else

        End If


        ImportDs = CommonDA.Remove_Null(ImportDs)
        ListView1.Items.Clear()
        For Each Dr In ImportDs.re_spare_purchase.Rows
            i += 1
            lvItem = ListView1.Items.Add(Dr.MyId)
            lvItem.SubItems.Add(i)
            lvItem.SubItems.Add(Dr.GRN_No)
            lvItem.SubItems.Add(Dr.Invoice_No)
            lvItem.SubItems.Add(IIf(IsDBNull(Dr.GRN_Date) = True, "", Dr.GRN_Date))
            lvItem.SubItems.Add(Dr.Supplier_Invoice_No)
            lvItem.SubItems.Add(Dr.Supplier_Invoice_Date)
            lvItem.SubItems.Add(Dr.Rate)
            lvItem.SubItems.Add(Dr.Taxable_Amount)
            lvItem.SubItems.Add(Dr.IGST)
            lvItem.SubItems.Add(Dr.Freight)
            lvItem.SubItems.Add(Dr.Total_Amount)
            lvItem.SubItems.Add(Dr.To_Tally)
            '  lvItem.SubItems.Add(Dr.LastSeen)

        Next


    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged

        If ListView1.SelectedItems.Count > 0 Then

            find_Details()

        End If

    End Sub

    Private Sub find_Details()
        Dim i As Integer = 0
        Dim DetailedDs As New TallyDs
        Dim InvoiceNum As String = ListView1.SelectedItems.Item(0).SubItems(2).Text
        DetailedDs.Merge(SpareDs.re_spare_purchase_Detail.Select("`GRN No`= '" & InvoiceNum & "'"))

        ListView2.Items.Clear()
        For Each Drd In DetailedDs.re_spare_purchase_Detail.Rows

            i += 1
            lvItem = ListView2.Items.Add(Drd.MyId)
            lvItem.SubItems.Add(i)
            lvItem.SubItems.Add(Drd.Model)
            lvItem.SubItems.Add(Drd.Description)
            lvItem.SubItems.Add(Drd.UOM)
            lvItem.SubItems.Add(Drd.Qty)
            lvItem.SubItems.Add(Drd.Rate)
            lvItem.SubItems.Add(Drd.IGST_per)
            lvItem.SubItems.Add(Drd.IGST)
            lvItem.SubItems.Add(Drd.Freight_per)
            lvItem.SubItems.Add(Drd.Freight)
            lvItem.SubItems.Add(Drd.Discount_per)
            lvItem.SubItems.Add(Drd.Discount)
            lvItem.SubItems.Add(Drd.Taxable_Amount)
            lvItem.SubItems.Add(Drd.Total_Amount)

        Next
    End Sub

    Private Sub RbtMissing_CheckedChanged(sender As Object, e As EventArgs) Handles RbtMissing.CheckedChanged
        find()
    End Sub

    Private Sub RbtActive_CheckedChanged(sender As Object, e As EventArgs) Handles RbtActive.CheckedChanged
        If RbtActive.Checked = True Then
            find()
        End If
    End Sub

    Private Sub RbtImported_CheckedChanged(sender As Object, e As EventArgs) Handles RbtImported.CheckedChanged
        If RbtImported.Checked = True Then
            find()
        End If
    End Sub

    Private Sub RbtAll_CheckedChanged(sender As Object, e As EventArgs) Handles RbtAll.CheckedChanged
        find()
    End Sub

    Private Sub BtnImport_Click(sender As Object, e As EventArgs) Handles BtnImport.Click
        If ImportDs.re_spare_purchase.Rows.Count > 0 Then
            PanelPlsWait.Visible = True
            BtnImport.Width = 0

            ' Load_Tally_StockItems()
            Imort_To_Tally()
            BtnImport.Width = PnlImport.Width
            PanelPlsWait.Visible = False
        Else
            MsgBox("No Active Items...")
        End If
        find()
    End Sub

    Public Shared Function ProgresBar(ByVal Count As Integer, ByRef buton As Button, ByRef pnl As Panel)


        If buton.Width <= pnl.Width Then
            buton.Width += pnl.Width / Count
        Else
            buton.Width = pnl.Width
        End If


        Return Nothing
    End Function


    Public Shared Function Read_Settings(ByVal S_Key As String) As String
        Dim Value As String = ""
        Try
            Value = PublicShared.Settings_Dt.Select("S_Key='" & S_Key & "'").First.Item("S_Value").ToString
        Catch ex As Exception
        End Try
        Return Value
    End Function

    Public Function Is_Already_Exist(ByVal REFERENCE_NO As String, ByVal VOUCHER_NUMBER As String, ByVal VoucherType As String) As Boolean
        Dim Value As String = ""
        Dim Status As Boolean = False
        'Dim PurchaseVehicle_VoucherType As String = ""

        'If CmbType.Text = "Genuine" Then
        '    PurchaseVehicle_VoucherType = Read_Settings("PurchaseInterstateSpare_VoucherType")
        'Else
        '    PurchaseVehicle_VoucherType = Read_Settings("PurchaseLocalSpare_VoucherType")
        'End If

        If PublicTallyDS.Tables.Count > 0 Then
            If PublicTallyDS.Tables.Contains("VOUCHER") Then
                If PublicTallyDS.Tables("VOUCHER").Rows.Count > 0 Then
                    Try
                        Value = PublicTallyDS.Tables("VOUCHER").Select("REFERENCE = '" & REFERENCE_NO & "'" &
                                                              "And VOUCHERTYPENAME = '" & VoucherType & "'").First.Item("REFERENCE").ToString
                        Status = True
                    Catch ex As Exception
                    End Try
                End If
            End If
        End If
            Return Status

    End Function

    Public Sub Imort_To_Tally()

        Dim i As Integer = 1
        Dim Errors As Integer = 0
        Dim Created As Integer = 0
        Dim Altered As Integer = 0
        Dim ErrorList As String = ""
        Dim CurrentDate As Date
        Dim AlreadyExist_Int As Integer = 0
        Dim AlreadyExist_String As String = ""

        Dim Responsestr As String = ""
        Dim XMLDOM As New MSXML2.DOMDocument30
        Dim ObjXml As New MSXML2.ServerXMLHTTP
        Dim StrXmldata As String = ""
        Dim Export_Data_Status As Boolean = False

        Dim XmlDS As New DataSet
        Dim Stream As StringReader
        Dim Reader As XmlTextReader
        Dim Import_Log As String = ""
        'Dim status As Boolean
        Dim Stk_ItemName As String = ""
        Dim StkItems_ToCreate As String = ""
        Dim Status_Msg As String = ""
        Dim PurchaseDs As New TallyDs
        Dim PurchaseVoucherType As String = ""
        Dim PartyLedgerSpare_Transfer_Ds As New TallyDs
        Dim err_str As String = ""
        ErrStr_QtyType = ""
        Import_Log = ""
        PublicTallyDS = New TallyDs
        LblStatusImport.Text = ""
        If ImportDs.re_spare_purchase.Rows.Count > 0 Then

            '''' Load_PartName()

            'err_str = Check_Partno_not_exist()

            'If err_str <> "" Then
            '    '  Show_ClipBoard("Missing Parts ", err_str)
            '    If MsgBox("Do you want to add these parts and continue ?", MsgBoxStyle.YesNo) = Windows.Forms.DialogResult.No Then
            '        Exit Sub
            '    End If
            'End If


            For Each Me.Dr In ImportDs.re_spare_purchase.Rows

                If Dr.Supplier_Invoice_No = "" Then
                    Continue For
                End If
                Dim Bill_Type As String = Strings.Left(Dr.GRN_No, 3)
                If Bill_Type = "GPR" Then
                    PurchaseVoucherType = Read_Ledgers("Purchase_VT_Gear")
                Else
                    PurchaseVoucherType = Read_Ledgers("Purchase_VT_Spare")

                End If
                ' MsgBox(PurchaseVoucherType)
                '  Dr.Supplier_Invoice_Date = "31-07-2018"
                If CurrentDate.Date <> Dr.Supplier_Invoice_Date Then
                    CurrentDate = Dr.Supplier_Invoice_Date
                    Export_Data_Status = Export_Xml(Format(CurrentDate.Date, "yyyyMMdd"), Format(CurrentDate.Date, "yyyyMMdd"), PurchaseVoucherType)
                    If Export_Data_Status = False Then
                        MsgBox ("Please Check Tally ODBC Connection...")
                        If MsgBox("Warnings : Failed to check existing entries..." & vbCrLf &
                                   "Do you want to continue?", MsgBoxStyle.YesNo) = Windows.Forms.DialogResult.No Then
                            Exit For
                        End If
                    End If
                End If


                'Dim PurchaseReferenceNo_Prefix As String = Read_Settings("PurchaseReferenceNo_Prefix")

                Dim ReferNo As String = Dr.Supplier_Invoice_No

                Dim VchNo As String = ""
                VchNo = Dr.GRN_No
                'If PublicShared.Branch_Code = "KTM" Or PublicShared.Branch_Code = "TVM" Or PublicShared.Branch_Code = "KLM" Then
                '    VchNo = Dr.Grn_No
                'Else
                '    VchNo = Dr.Supplier_Invoice_No
                'End If

                If Dr.Supplier_Invoice_No = "0079048529" Then
                    Dim hhh As String = ""
                End If

                If Is_Already_Exist(ReferNo, VchNo, PurchaseVoucherType) Then
                    AlreadyExist_Int += 1
                    AlreadyExist_String += Dr.Supplier_Invoice_No & " , "

                    CommonDA.Update_SparePurchase(Dr)
                    'Skip
                Else
                    'Do
                    Try

                        '  MsgBox("exist check")
                        ObjXml.open("POST", TallyHost, False)
                        PurchaseDs = New TallyDs
                        PurchaseDs.Merge(ImportDs.re_spare_purchase_Detail.Select("Supplier_Invoice_No='" & Dr.Supplier_Invoice_No & "' And `GRN No`='" & Dr.GRN_No & "'", "Supplier_Invoice_No"))

                        StrXmldata = XmlFormat_purchase_spare_WithOut_Stock(Dr, PurchaseDs.re_spare_purchase_Detail, i, PurchaseVoucherType)
                        StrXmldata = StrXmldata.Replace("Â", "")

                        If Read_Settings("ShowRequestXML_YN") = "Y" Then
                            Show_ClipBoard("Xml", StrXmldata)
                        End If
                        If StrXmldata = "" Then
                            Import_Log += vbCrLf & Dr.Supplier_Invoice_No & " - Unable to create tally request.."
                        Else
                            Try
                                ObjXml.send(StrXmldata)
                                Responsestr = ObjXml.responseText
                                If Read_Settings("ShowRespondsXML_YN") = "Y" Then
                                    'Show_ClipBoard("", Responsestr)
                                End If
                            Catch ex As Exception
                                'serin
                                Me.Cursor = Cursors.Default
                                ' FrmMDI.PleaseWait(False)
                            End Try

                            If Responsestr <> Nothing Then

                                XMLDOM.loadXML(Responsestr)
                                Stream = New StringReader(Responsestr)
                                Reader = New XmlTextReader(Stream)
                                XmlDS.Clear()
                                XmlDS.ReadXml(Reader)

                                If XmlDS.Tables.Contains("response") Then

                                    Created += Val(XmlDS.Tables("response").Rows(0).Item("CREATED").ToString)
                                    Altered += Val(XmlDS.Tables("response").Rows(0).Item("ALTERED").ToString)
                                    Errors += Val(XmlDS.Tables("response").Rows(0).Item("ERRORS").ToString)

                                    Import_Log += Dr.Invoice_No & " - " &
                                                "Created :" & XmlDS.Tables("response").Rows(0).Item("CREATED").ToString &
                                                ",Altered :" & XmlDS.Tables("response").Rows(0).Item("ALTERED").ToString &
                                                ",Errors :" & XmlDS.Tables("response").Rows(0).Item("ERRORS").ToString

                                    If Val(XmlDS.Tables("response").Rows(0).Item("ERRORS").ToString) > 0 Then
                                        If XmlDS.Tables("response").Columns.Contains("LINEERROR") Then
                                            Import_Log += " ," & XmlDS.Tables("response").Rows(0).Item("LINEERROR").ToString
                                            ErrorList += XmlDS.Tables("response").Rows(0).Item("LINEERROR").ToString
                                        End If

                                    End If

                                ElseIf XmlDS.Tables.Contains("LINEERROR") Then
                                    Import_Log += Dr.Invoice_No & " - " &
                                                    XmlDS.Tables("LINEERROR").Rows(0).Item("LINEERROR_Text").ToString
                                    ErrorList += Dr.Invoice_No & " - " &
                                                    XmlDS.Tables("LINEERROR").Rows(0).Item("LINEERROR_Text").ToString
                                    Errors += 1
                                End If

                                If Val(XmlDS.Tables("response").Rows(0).Item("CREATED").ToString) > 0 And
                                    Val(XmlDS.Tables("response").Rows(0).Item("ERRORS").ToString) = 0 Then


                                    CommonDA.Update_SparePurchase(Dr)

                                End If

                                Import_Log += vbCrLf

                            Else
                                Import_Log += vbCrLf & Dr.Invoice_No & " - Tally not responding..."
                                Errors += 1
                                ErrorList += "Tally not responding..." & vbCrLf
                            End If

                        End If

                    Catch ex As Exception
                        'serin
                        Me.Cursor = Cursors.Default
                        '   FrmMDI.PleaseWait(False)
                        Import_Log += vbCrLf & Dr.Supplier_Invoice_No & " -Error- " & ex.Message
                        If XmlDS.Tables.Count > 0 Then
                            ErrorList += XmlDS.Tables(0).Rows(0).Item(0).ToString & vbCrLf
                        End If
                        Errors += 1
                    End Try
                End If


                LblStatusImport.Text = "Import Status = " & i & "/" & ImportDs.re_spare_purchase.Rows.Count & vbCrLf &
                "Created : " & Created & " , Altered : " & Altered & " , Skipped : " & AlreadyExist_Int & " , Errors : " & Errors
                LblStatusImport.Refresh()

                i += 1


            Next


            If AlreadyExist_String <> "" Then
                '  Show_ClipBoard("Skipped entries", "Skipped entries ( " & AlreadyExist_Int & " ) : " & vbCrLf & AlreadyExist_String)
            End If
            If ErrorList = "" Then
                ImportDs.Clear()
                ' Show_ClipBoard("Completed successfully.", "" & vbCrLf & LblStatusImport.Text)
                CommonDA.Create_Log("Import to Tally", "Import", LblStatusImport.Text & vbCrLf & Import_Log)
                If Created > 0 Then
                    Try
                        '  IO.File.Move(TxtPath.Text, Application.StartupPath & "\BackUp\Spare_Parts_" & Format(Date.Now, "ddMMMyy_HHmmss") & ".xls")
                    Catch ex As Exception
                        CommonDA.Create_Log("Import to Tally", "Failed to move ", ErrorList)
                    End Try
                End If

                LblStatusImport.Text = "Import Status :"
            Else
                Show_ClipBoard("Error List", ErrorList)
                CommonDA.Create_Log("Import to Tally", "Import-Error", ErrorList)
            End If



            If StkItems_ToCreate <> "" Then
                ' Status_Msg = Status_Msg & vbCrLf & "Items Not Exist : " & vbCrLf & StkItems_ToCreate
            End If

            If ErrStr_QtyType <> "" Then
                ' MsgBox(ErrStr_QtyType)

                Show_ClipBoard("Import Spare Purchase", ErrStr_QtyType)


            End If

            If Status_Msg <> "" Then
                'Show_ClipBoard("Import Spare Purchase", Status_Msg)
            End If

            '  MsgBox(Status_Msg)

        End If

    End Sub

    Public Sub Show_ClipBoard(ByVal Title As String, ByVal Msg As String)
        Dim ObjCl As FrmClipBoard
        ObjCl = New FrmClipBoard(Title, Msg)
        ObjCl.StartPosition = FormStartPosition.CenterScreen
        ObjCl.ShowDialog(Me)
    End Sub

    Public Function XmlFormat_purchase_spare_WithOut_Stock(ByVal Dr As TallyDs.re_spare_purchaseRow, ByVal Dt As TallyDs.re_spare_purchase_DetailDataTable,
                                                            ByVal No As Integer, ByVal PurchaseVehicle_VoucherType As String) As String

        Dim Xml As String = ""
        Dim Xml_StockItem As String = ""
        Dim Xml_AllItems As String = ""

        '  Dr.Supplier_Invoice_Date = "31-07-2018"
        '  Dr.GRN_Date = "31-07-2018"

        Dim InvDate As Date = Dr.Supplier_Invoice_Date
        Dim InvDateString As String = Format(InvDate, "yyyyMMdd")
        Dim GrnDateString As String = Format(Dr.GRN_Date, "yyyyMMdd")
        Dim StockSpareParent As String = Read_Ledgers("StockSpareParent")

        Dim PartyLedgerSpare As String = Read_Ledgers("Purchase_PartyLedger_Spare")
        Dim PurchaseLedgerSpare As String = "" 'Read_Settings("PurchaseLedgerSpare")
        ' Dim PurchaseReferenceNo_Prefix As String = Read_Settings("PurchaseReferenceNo_Prefix")


        Dim StockSpareParent_To_Create As String = Read_Ledgers("StockSpareParent_To_Create")

        '    MsgBox(StockSpareParent & vbCrLf & PartyLedgerSpare & vbCrLf & PurchaseLedgerSpare & vbCrLf & GSTPer & vbCrLf & StockSpareParent_To_Create)

        Dim Narration As String = "GRN : " & Dr.GRN_No & " , GRN.DATE : " & Format(Dr.GRN_Date, "dd-MMM-yyyy")
        Dim Ledger_Name As String = ""
        Dim Stk_Item As String = ""
        Dim Stk_ItemName As String = ""
        Dim StkItems_ToCreate As String = ""
        Dim DrT As TallyDs.re_spare_purchase_DetailRow
        Dim j As Integer
        Dim Max_Count As Integer = 0
        Dim Qty_Type As String = ""
        Dim IGST_18, IGST_28 As Decimal
        Dim RoundOff As Decimal = 0
        Dim COST_CENTER As String = ""
        Dim PartyLedgerSpare_Transfer_Ds As New TallyDs

        Try
            Dim Bill_Type As String = Strings.Left(Dr.GRN_No, 3)

            Xml = "<ENVELOPE>" &
                     "<HEADER>" &
                           "<TALLYREQUEST>Import Data</TALLYREQUEST>" &
                     "</HEADER>" &
                     "<BODY>" &
                         "<IMPORTDATA>" &
                             "<REQUESTDESC>" &
                                  "<REPORTNAME>All Masters</REPORTNAME>" &
                                  "<STATICVARIABLES>" &
                                    "<SVCURRENTCOMPANY>" & BranchName.Replace("&", "&amp;") & "</SVCURRENTCOMPANY>" &
                                  "</STATICVARIABLES>" &
                              "</REQUESTDESC>" &
                             "<REQUESTDATA>"

            j = 0

            Max_Count = Dt.Rows.Count

            IGST_18 = 0 : IGST_28 = 0

            Dim X As Double = 0
            Dim y As Integer = 0

            '  Spare_Purchase_SepTax_Ledger
            Dim DrSt As TallyDs.Spare_Purchase_SepTax_LedgerRow
            PartyLedgerSpare_Transfer_Ds.Spare_Purchase_SepTax_Ledger.Rows.Clear()

            For Each DrT In Dt.Rows

                DrSt = PartyLedgerSpare_Transfer_Ds.Spare_Purchase_SepTax_Ledger.Rows.Add()


                PurchaseLedgerSpare = Read_Ledgers("Purchase_Ledger_Spare_" & Val(DrT.IGST_per))

                DrSt.PurchaseLedgerSpare = PurchaseLedgerSpare

                DrSt.Taxable_Amt = Math.Round(Val(DrT.Taxable_Amount), 2)


                '    If DsStock.Tables.Contains("StockItems_Names") Then

                '        If DsStock.Tables("StockItems_Names").Select("[$ADDITIONALNAME]='" & DrT.Model & "'").Count = 0 Then


                '            If DrT.UOM = "" Then

                '                DrT.UOM = Read_Settings("Default_UOM")

                '            End If

                '            If DrT.Description = "" Then
                '                Continue For
                '            End If

                '            DrT.Description = DrT.Description.Replace("  ", " ")
                '            DrT.Description = DrT.Description.Replace(ControlChars.Quote, "")
                '            Stk_ItemName = DrT.Description.Replace("&", "&amp;") & " " & DrT.Model
                '            Qty_Type = DrT.UOM

                '            'Xml_StockItem = Xml_StockItem & "<TALLYMESSAGE xmlns:UDF=""TallyUDF"">" & _
                '            '                      "<STOCKITEM NAME=""" & Stk_ItemName.Replace("&", "&amp;") & """ ACTION=""Create"">" & _
                '            '                          "<NAME>" & Stk_ItemName.Replace("&", "&amp;") & "</NAME>" & _
                '            '                          "<ADDITIONALNAME>" & DrT.Model & "</ADDITIONALNAME>" & _
                '            '                          "<BASEUNITS>" & DrT.UOM & "</BASEUNITS>" & _
                '            '                        "<PARENT>" & StockSpareParent_To_Create & "</PARENT>" & _
                '            '                      Xml_GST_Item(Val(DrT.IGST_per), "", IIf(DrT.HSN_Desc = "", DrT.HSN_code, DrT.HSN_Desc), DrT.HSN_code) &
                '            '                      "</STOCKITEM>" & _
                '            '                "</TALLYMESSAGE>"

                '            'Xml_GST_Item(DrT.GSTPer, 0, DrT.HSNCode, DrT.HSNCode) &

                '            Dim DrN As DataRow
                '            DrN = DsStock.Tables("StockItems_Names").Rows.Add
                '            DrN("$Name") = Stk_ItemName.Replace("&", "&amp;")
                '            DrN("$BASEUNITS") = DrT.UOM
                '            DrN("$ADDITIONALNAME") = DrT.Model
                '            DsStock.AcceptChanges()

                '        Else

                '            Dim DrN As DataRow
                '            ''DrN = DsStock.Tables("StockItems_Names").Rows(0)
                '            DrN = DsStock.Tables("StockItems_Names").Select("[$ADDITIONALNAME]='" & DrT.Model & "'")(0)

                '            Stk_ItemName = DrN("$Name")
                '            Qty_Type = DrN("$BASEUNITS")

                '        End If

                '    End If

                '    If Stk_ItemName.Contains("&") Then
                '        Stk_ItemName.Replace("&", "&amp;")
                '    End If

                '    Dim DISC As Double = 0
                '    Dim DISC_YES As String = "No"

                '    If DrT.Discount > 0 Then
                '        DISC = DrT.Discount_per
                '        DISC_YES = "Yes"
                '    End If

                '    'If Bill_Type = "GPR" Then
                '    '    PurchaseLedgerSpare = Read_Settings("PurchaseLedgerSpare_GPR" & Val(DrT.IGST_per))
                '    'Else
                '    'End If


                '    'Xml_AllItems = Xml_AllItems & "<ALLINVENTORYENTRIES.LIST>" & _
                '    '        "<STOCKITEMNAME>" & Stk_ItemName.Replace("&", "&amp;") & "</STOCKITEMNAME>" & _
                '    '          "<ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" & _
                '    '          "<HASDISCOUNTS>" & DISC_YES & "</HASDISCOUNTS>" & _
                '    '          "<RATE>" & Val(DrT.Rate) & "/" & Qty_Type & "</RATE>" & _
                '    '          "<AMOUNT>-" & Val(DrT.Taxable_Amount) & "</AMOUNT>" & _
                '    '          "<DISCOUNT> " & DISC & "</DISCOUNT>" & _
                '    '          "<ACTUALQTY>" & DrT.Qty & " " & Qty_Type & "</ACTUALQTY>" & _
                '    '          "<BILLEDQTY>" & DrT.Qty & " " & Qty_Type & "</BILLEDQTY>"

                '    'Xml_AllItems = Xml_AllItems & "<ACCOUNTINGALLOCATIONS.LIST>" & _
                '    '   "<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>" & _
                '    '   "<ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" & _
                '    '   "<LEDGERFROMITEM>No</LEDGERFROMITEM>" & _
                '    '   "<TAXCLASSIFICATIONNAME></TAXCLASSIFICATIONNAME>" & _
                '    '   "<LEDGERNAME>" & PurchaseLedgerSpare.Replace("&", "&amp;") & "</LEDGERNAME>" & _
                '    '   "<AMOUNT>-" & Val(DrT.Taxable_Amount) & "</AMOUNT>" & _
                '    '   "</ACCOUNTINGALLOCATIONS.LIST>"

                '    'Xml_AllItems = Xml_AllItems & "<BATCHALLOCATIONS.LIST>" & _
                '    '              "<GODOWNNAME>Main Location</GODOWNNAME>" & _
                '    '              "<BATCHNAME></BATCHNAME>" & _
                '    '              "<AMOUNT>-" & Val(DrT.Taxable_Amount) & "</AMOUNT>" & _
                '    '              "<ACTUALQTY>" & DrT.Qty & " " & Qty_Type & "</ACTUALQTY>" & _
                '    '              "<BILLEDQTY>" & DrT.Qty & " " & Qty_Type & "</BILLEDQTY>" & _
                '    '          "</BATCHALLOCATIONS.LIST>" & _
                '    '    "</ALLINVENTORYENTRIES.LIST>"


                '    X += Val(DrT.Rate)
                '    y += 1

                PartyLedgerSpare_Transfer_Ds.AcceptChanges()


            Next

            If Xml_AllItems = "" Then
                Xml_AllItems = "<ALLINVENTORYENTRIES.LIST></ALLINVENTORYENTRIES.LIST>"
            End If

            Dim Additional_Ledgers As String = ""
            Dim Alloc_Ledger As String = ""
            Dim SumTaxable As Double = 0.00
            Dim Ds As New TallyDs
            Dim DsR As TallyDs.Spare_Purchase_SepTax_LedgerRow
            Dim dtd As New DataTable
            dtd = PartyLedgerSpare_Transfer_Ds.Spare_Purchase_SepTax_Ledger.DefaultView.ToTable(True, {"PurchaseLedgerSpare"})

            Ds.Merge(PartyLedgerSpare_Transfer_Ds.Spare_Purchase_SepTax_Ledger)

            ' SumTaxable = Val(Dt.Select("sum(Taxable_Amount)", ""))
            PartyLedgerSpare_Transfer_Ds.Spare_Purchase_SepTax_Ledger.Rows.Clear()

            For Each ledger In dtd.Rows

                DrSt = PartyLedgerSpare_Transfer_Ds.Spare_Purchase_SepTax_Ledger.Rows.Add()

                For Each DsR In Ds.Spare_Purchase_SepTax_Ledger

                    If ledger(0).ToString = DsR.PurchaseLedgerSpare Then
                        SumTaxable += Val(DsR.Taxable_Amt)
                    End If
                Next

                PurchaseLedgerSpare = ledger(0).ToString
                DrSt.PurchaseLedgerSpare = PurchaseLedgerSpare
                DrSt.Taxable_Amt = Math.Round(Val(SumTaxable), 2)
                PartyLedgerSpare_Transfer_Ds.AcceptChanges()
                PurchaseLedgerSpare = ""
                SumTaxable = 0.0

            Next

            PurchaseLedgerSpare = ""
            Dim Main_Ledgers As String = ""



            'Allocation Ledger
            For Each DrSt In PartyLedgerSpare_Transfer_Ds.Spare_Purchase_SepTax_Ledger.Rows
                If DrSt.Taxable_Amt > 0 Then
                    COST_CENTER = Create_XML_COSTCENTER(Read_Ledgers("Cost_Center_Part"), -DrSt.Taxable_Amt)
                    Additional_Ledgers += Create_XML_ALLLEDGERENTRIES(DrSt.PurchaseLedgerSpare, -DrSt.Taxable_Amt, COST_CENTER)
                End If
            Next



            Dim DrGST As TallyDs.GST_DetailsRow
            GstDs_Temp = New TallyDs
            GstDs_Temp.Merge(GST_Ds)


            For Each Drn As TallyDs.re_spare_purchase_DetailRow In Dt.Rows
                If GstDs_Temp.GST_Details.Select("GST_Name = 'IGST' And GST_Per = '" & Drn.IGST_per & "'", "").Count > 0 Then
                    DrGST = GstDs_Temp.GST_Details.Select("GST_Name = 'IGST' And GST_Per = '" & Drn.IGST_per & "'", "").First
                    DrGST.Amount += Drn.IGST
                End If
                GstDs_Temp.AcceptChanges()
            Next

            'IGST Ledger
            For Each DrG As TallyDs.GST_DetailsRow In GstDs_Temp.GST_Details.Rows
                If DrG.Amount > 0 Then
                    Additional_Ledgers += Create_XML_ALLLEDGERENTRIES(DrG.Tally_Ledger, -DrG.Amount, "")
                End If
            Next


            Xml = Xml & Xml_StockItem

            Dim RefNo As String = ""

            RefNo = Dr.Supplier_Invoice_No

            Dim PaiseRoundingOff_Amount As Double = 0
            Dim Party_ledger_Amt As Decimal = Math.Round(Val(Dr.Total_Amount))
            PaiseRoundingOff_Amount = Format(Party_ledger_Amt - Val(Dr.Total_Amount), "0.00")

            If PaiseRoundingOff_Amount = -0.5 Then
                PaiseRoundingOff_Amount = 0.5
                Party_ledger_Amt = Party_ledger_Amt + 1
            End If


            Main_Ledgers += Create_XML_LEDGERENTRIES_PARTYLEDGER(PartyLedgerSpare, Party_ledger_Amt, "", False)


            'Ledger_FRIEGHT
            'If Dr.Freight > 0 Then
            '    Additional_Ledgers += Create_XML_ALLLEDGERENTRIES(Read_Settings("Ledger_FRIEGHT"), -Val(Dr("Freight")))
            'End If

            If PaiseRoundingOff_Amount <> 0 Then
                COST_CENTER = Create_XML_COSTCENTER(Read_Ledgers("Cost_Center_Part"), -PaiseRoundingOff_Amount)
                Additional_Ledgers += Create_XML_ALLLEDGERENTRIES_RoundOff(Read_Ledgers("Ledger_PaiseRoundOff"), -PaiseRoundingOff_Amount, COST_CENTER)
            End If

            Xml = Xml & "<TALLYMESSAGE xmlns:UDF=""TallyUDF"">" &
                         "<VOUCHER>" &
                         "<VOUCHERTYPENAME>" & PurchaseVehicle_VoucherType.Replace("&", "&amp;") & "</VOUCHERTYPENAME>" &
                         "<DATE>" & GrnDateString & "</DATE>" &
                         "<EFFECTIVEDATE>" & GrnDateString & "</EFFECTIVEDATE>" &
                         "<ISCANCELLED>No</ISCANCELLED>" &
                         "<USETRACKINGNUMBER>No</USETRACKINGNUMBER>" &
                         "<ISPOSTDATED>No</ISPOSTDATED>" &
                         "<ISINVOICE>Yes</ISINVOICE>" &
                         "<REFERENCE>" & RefNo & "</REFERENCE>" &
                         "<REFERENCEDATE>" & InvDateString & "</REFERENCEDATE>" &
                         "<NARRATION>" & Narration & "</NARRATION>" &
                         "<DIFFACTUALQTY>No</DIFFACTUALQTY>" &
                         "<VOUCHERNUMBER>" & Dr.Invoice_No & "</VOUCHERNUMBER>" &
                         "<PARTYNAME>" & PartyLedgerSpare.Replace("&", "&amp;") & "</PARTYNAME>" &
                         "<PARTYLEDGERNAME>" & PartyLedgerSpare.Replace("&", "&amp;") & "</PARTYLEDGERNAME>" &
                         "<ASORIGINAL>Yes</ASORIGINAL>" &
                         "<CSTFORMISSUETYPE>C Form</CSTFORMISSUETYPE>" &
                         "<BASICBUYERSSALESTAXNO></BASICBUYERSSALESTAXNO>" &
                        Main_Ledgers & Additional_Ledgers

            Xml = Xml & Xml_AllItems

            Xml = Xml & "</VOUCHER>" &
       "</TALLYMESSAGE>"

            Xml = Xml & "</REQUESTDATA>" &
     "</IMPORTDATA>" &
     "</BODY>" &
     "</ENVELOPE>"

        Catch ex As Exception
            '  FrmMDI.PleaseWait(False)
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message)
        End Try

        Return Xml

    End Function

    Public Function XmlFormat_purchase_spare_With_Stock(ByVal Dr As TallyDs.re_spare_purchaseRow, ByVal Dt As TallyDs.re_spare_purchase_DetailDataTable,
                                                            ByVal No As Integer, ByVal PurchaseVehicle_VoucherType As String) As String

        Dim Xml As String = ""
        Dim Xml_StockItem As String = ""
        Dim Xml_AllItems As String = ""

        '  Dr.Supplier_Invoice_Date = "31-07-2018"
        ' Dr.GRN_Date = "31-07-2018"

        Dim InvDate As Date = Dr.Supplier_Invoice_Date
        Dim InvDateString As String = Format(InvDate, "yyyyMMdd")
        Dim GrnDateString As String = Format(Dr.GRN_Date, "yyyyMMdd")
        Dim StockSpareParent As String = Read_Settings("Spare_Stock_Parent")

        Dim PartyLedgerSpare As String = ""
        Dim PurchaseLedgerSpare As String = ""
        ' Dim PurchaseReferenceNo_Prefix As String = Read_Settings("PurchaseReferenceNo_Prefix")



        Dim StockSpareParent_To_Create As String = ""

        '    MsgBox(StockSpareParent & vbCrLf & PartyLedgerSpare & vbCrLf & PurchaseLedgerSpare & vbCrLf & GSTPer & vbCrLf & StockSpareParent_To_Create)

        Dim Narration As String = "GRN : " & Dr.GRN_No & " , GRN.DATE : " & Format(Dr.GRN_Date, "dd-MMM-yyyy")
        Dim Ledger_Name As String = ""
        Dim Stk_Item As String = ""
        Dim Stk_ItemName As String = ""
        Dim StkItems_ToCreate As String = ""
        Dim DrT As TallyDs.re_spare_purchase_DetailRow
        Dim j As Integer
        Dim Max_Count As Integer = 0
        Dim Qty_Type As String = ""
        Dim IGST_18, IGST_28 As Decimal
        Dim RoundOff As Decimal = 0
        Dim PartyLedgerSpare_Transfer_Ds As New TallyDs
        Dim HsnMissing As String = ""

        Try

            Dim Bill_Type As String = Strings.Left(Dr.GRN_No, 3)



            Xml = "<ENVELOPE>" &
                     "<HEADER>" &
                           "<TALLYREQUEST>Import Data</TALLYREQUEST>" &
                     "</HEADER>" &
                     "<BODY>" &
                         "<IMPORTDATA>" &
                             "<REQUESTDESC>" &
                                  "<REPORTNAME>All Masters</REPORTNAME>" &
                                  "<STATICVARIABLES>" &
                                    "<SVCURRENTCOMPANY>" & BranchName.Replace("&", "&amp;") & "</SVCURRENTCOMPANY>" &
                                  "</STATICVARIABLES>" &
                              "</REQUESTDESC>" &
                             "<REQUESTDATA>"


            j = 0

            Max_Count = Dt.Rows.Count

            IGST_18 = 0 : IGST_28 = 0


            Dim X As Double = 0

            For Each DrT In Dt.Rows


                '    FrmMDI.PleaseWait_Progress("Please Wait  " & No & "/" & Ds.Spare_Purchase.Rows.Count & " (" & Format(j / Max_Count * 100, "0") & "%)")
                'LblStatusImport.Text = "Please Wait  " & No & "/" & ImportDs.re_spare_purchase.Rows.Count & " (" & Format(j / Max_Count * 100, "0") & "%)"
                'LblStatusImport.Refresh()

                j += 1

                '' DsStock.Clear()

                '     Get_StockItem_Names(DrT.Shipped_Part, DrT("Id"), DrT.StockSpareParent, DrT.UOM)

                '  Stk_ItemName = "ADDON LEAD"

                'If DrT.GSTPer = Val(Read_Settings("IGST18")) And Dr.IsLocal = 0 Then
                '    PurchaseInterstateSpare = Read_Settings("PurchaseInterstateOil")
                '    IGST_18 += DrT.GST_Amount
                'ElseIf Dr.IsLocal = 0 Then
                '    PurchaseInterstateSpare = Read_Settings("PurchaseInterstateSpare")
                '    IGST_28 += DrT.GST_Amount
                'End If

                If DsStock.Tables.Contains("StockItems_Names") Then

                    'If DsStock.Tables("StockItems_Names").Rows.Count = 0 Then
                    If DsStock.Tables("StockItems_Names").Select("[$ADDITIONALNAME]='" & DrT.Model & "'").Count = 0 Then

                        If DrT.UOM = "" Then

                            'If ErrStr_QtyType = "" Then
                            '    ErrStr_QtyType = "Please Enter UOM for these entries : " & vbCrLf
                            'End If

                            'ErrStr_QtyType = ErrStr_QtyType & "Inv No : " & DrT.Supplier_Invoice_No & ", Part No : " & DrT.Model & vbCrLf

                            'Continue For

                            DrT.UOM = Read_Settings("Default_UOM")

                        End If

                        If DrT.Description.Trim = "" Then
                            Continue For
                        ElseIf DrT.HSN_code.Trim = "" Then
                            HsnMissing += " HSNCode Missing :" & DrT.Model
                            Continue For
                        End If

                        If Bill_Type = "GPR" Then
                            StockSpareParent_To_Create = Read_Ledgers("Stock_Parent_Gear")
                        Else
                            StockSpareParent_To_Create = Read_Ledgers("Stock_Parent_Spare")
                        End If

                        DrT.Description = DrT.Description.Replace("  ", " ")
                        DrT.Description = DrT.Description.Replace(ControlChars.Quote, "")
                        DrT.Description = DrT.Description.Replace("Â", "")
                        Stk_ItemName = DrT.Description.Replace("&", "&amp;") & " " & DrT.Model
                        Stk_ItemName = Stk_ItemName.Replace("Â", "")
                        Qty_Type = DrT.UOM

                        If DrT.Model.Contains("888414") Then
                            MsgBox("Found")
                        End If

                        Xml_StockItem = Xml_StockItem & "<TALLYMESSAGE xmlns:UDF=""TallyUDF"">" &
                                              "<STOCKITEM NAME=""" & Stk_ItemName.Replace("&", "&amp;") & """ ACTION=""Create"">" &
                                                  "<NAME>" & Stk_ItemName.Replace("&", "&amp;") & "</NAME>" &
                                                  "<ADDITIONALNAME>" & DrT.Model & "</ADDITIONALNAME>" &
                                                  "<BASEUNITS>" & DrT.UOM & "</BASEUNITS>" &
                                                "<PARENT>" & StockSpareParent_To_Create & "</PARENT>" &
                                                Xml_GST_Item(Val(DrT.IGST_per), "", IIf(DrT.HSN_Desc = "", DrT.HSN_code, DrT.HSN_Desc), DrT.HSN_code) &
                                              "</STOCKITEM>" &
                                        "</TALLYMESSAGE>"
                        'Xml_GST_Item(DrT.GSTPer, 0, DrT.HSNCode, DrT.HSNCode) &


                        Dim DrN As DataRow

                        DrN = DsStock.Tables("StockItems_Names").Rows.Add
                        DrN("$Name") = Stk_ItemName.Replace("&", "&amp;")
                        DrN("$BASEUNITS") = DrT.UOM
                        DrN("$ADDITIONALNAME") = DrT.Model
                        DsStock.AcceptChanges()



                    Else

                        Dim DrN As DataRow
                        ''DrN = DsStock.Tables("StockItems_Names").Rows(0)
                        DrN = DsStock.Tables("StockItems_Names").Select("[$ADDITIONALNAME]='" & DrT.Model & "'")(0)

                        Stk_ItemName = DrN("$Name")
                        Qty_Type = DrN("$BASEUNITS")

                    End If

                End If
                If Stk_ItemName.Contains("&") Then
                    Stk_ItemName.Replace("&", "&amp;")
                End If

                Dim DISC As Double = 0
                Dim DISC_YES As String = "No"

                If DrT.Discount > 0 Then
                    DISC = DrT.Discount_per
                    DISC_YES = "Yes"
                End If

                If Bill_Type = "GPR" Then
                    PurchaseLedgerSpare = Read_Ledgers("Purchase_Ledger_Gear" & Val(DrT.IGST_per))
                Else
                    PurchaseLedgerSpare = Read_Ledgers("Purchase_Ledger_Spare" & Val(DrT.IGST_per))
                End If
                ' Show_ClipBoard("", "PurchaseLedgerSpare_" & Val(DrT.IGST_per))
                'MsgBox(PartyLedgerSpare)

                Xml_AllItems = Xml_AllItems & "<ALLINVENTORYENTRIES.LIST>" &
                        "<STOCKITEMNAME>" & Stk_ItemName.Replace("&", "&amp;") & "</STOCKITEMNAME>" &
                          "<ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" &
                          "<HASDISCOUNTS>" & DISC_YES & "</HASDISCOUNTS>" &
                          "<RATE>" & Val(DrT.Rate) & "/" & Qty_Type & "</RATE>" &
                          "<AMOUNT>-" & Val(DrT.Taxable_Amount) & "</AMOUNT>" &
                          "<DISCOUNT> " & DISC & "</DISCOUNT>" &
                          "<ACTUALQTY>" & DrT.Qty & " " & Qty_Type & "</ACTUALQTY>" &
                          "<BILLEDQTY>" & DrT.Qty & " " & Qty_Type & "</BILLEDQTY>"

                Xml_AllItems = Xml_AllItems & "<ACCOUNTINGALLOCATIONS.LIST>" &
                   "<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>" &
                   "<ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" &
                   "<LEDGERFROMITEM>No</LEDGERFROMITEM>" &
                   "<TAXCLASSIFICATIONNAME></TAXCLASSIFICATIONNAME>" &
                   "<LEDGERNAME>" & PurchaseLedgerSpare.Replace("&", "&amp;") & "</LEDGERNAME>" &
                   "<AMOUNT>-" & Val(DrT.Taxable_Amount) & "</AMOUNT>" &
                   "</ACCOUNTINGALLOCATIONS.LIST>"

                Xml_AllItems = Xml_AllItems & "<BATCHALLOCATIONS.LIST>" &
                              "<GODOWNNAME>Main Location</GODOWNNAME>" &
                              "<BATCHNAME></BATCHNAME>" &
                              "<AMOUNT>-" & Val(DrT.Taxable_Amount) & "</AMOUNT>" &
                              "<ACTUALQTY>" & DrT.Qty & " " & Qty_Type & "</ACTUALQTY>" &
                              "<BILLEDQTY>" & DrT.Qty & " " & Qty_Type & "</BILLEDQTY>" &
                          "</BATCHALLOCATIONS.LIST>" &
                    "</ALLINVENTORYENTRIES.LIST>"


                X += Val(DrT.Rate)



            Next


            Dim Additional_Ledgers As String = ""


            Dim DrGST As TallyDs.GST_DetailsRow
            GstDs_Temp = New TallyDs
            GstDs_Temp.Merge(GST_Ds)


            For Each Drn As TallyDs.re_spare_purchase_DetailRow In Dt.Rows

                If GstDs_Temp.GST_Details.Select("GST_Name = 'IGST' And GST_Per = '" & Drn.IGST_per & "'", "").Count > 0 Then
                    DrGST = GstDs_Temp.GST_Details.Select("GST_Name = 'IGST' And GST_Per = '" & Drn.IGST_per & "'", "").First
                    DrGST.Amount += Drn.IGST
                End If

                GstDs_Temp.AcceptChanges()

            Next

            'IGST Ledger
            For Each DrG As TallyDs.GST_DetailsRow In GstDs_Temp.GST_Details.Rows
                If DrG.Amount > 0 Then
                    Additional_Ledgers += Create_XML_ALLLEDGERENTRIES(DrG.Tally_Ledger, -DrG.Amount, "")
                End If
            Next


            Xml = Xml & Xml_StockItem

            Dim RefNo As String = ""

            RefNo = Dr.Supplier_Invoice_No



            Dim PaiseRoundingOff_Amount As Double = 0
            Dim Party_ledger_Amt As Decimal = Math.Round(Val(Dr.Total_Amount))
            PaiseRoundingOff_Amount = Format(Party_ledger_Amt - Val(Dr.Total_Amount), "0.00")

            If PaiseRoundingOff_Amount = -0.5 Then
                PaiseRoundingOff_Amount = 0.5
                Party_ledger_Amt = Party_ledger_Amt + 1
            End If
            If Bill_Type = "GPR" Then
                PartyLedgerSpare = Read_Ledgers("Purchase_PartyLedger_Gear")
            Else
                PartyLedgerSpare = Read_Ledgers("Purchase_PartyLedger_Spare")
            End If

            Dim Main_Ledgers As String = ""
            Main_Ledgers = Create_XML_LEDGERENTRIES_PARTYLEDGER(PartyLedgerSpare, Party_ledger_Amt, "", False)

            'Ledger_FRIEGHT
            'If Dr.Freight > 0 Then
            '    Additional_Ledgers += Create_XML_ALLLEDGERENTRIES(Read_Settings("Ledger_FRIEGHT"), -Val(Dr("Freight")))
            'End If

            If PaiseRoundingOff_Amount <> 0 Then
                Additional_Ledgers += Create_XML_ALLLEDGERENTRIES_RoundOff(Read_Ledgers("Ledger_PaiseRoundOff"), -PaiseRoundingOff_Amount, "")
            End If

            Xml = Xml & "<TALLYMESSAGE xmlns:UDF=""TallyUDF"">" &
                       "<VOUCHER>" &
                         "<VOUCHERTYPENAME>" & PurchaseVehicle_VoucherType.Replace("&", "&amp;") & "</VOUCHERTYPENAME>" &
                         "<DATE>" & GrnDateString & "</DATE>" &
                         "<EFFECTIVEDATE>" & InvDateString & "</EFFECTIVEDATE>" &
                         "<ISCANCELLED>No</ISCANCELLED>" &
                         "<USETRACKINGNUMBER>No</USETRACKINGNUMBER>" &
                         "<ISPOSTDATED>No</ISPOSTDATED>" &
                         "<ISINVOICE>Yes</ISINVOICE>" &
                         "<REFERENCE>" & RefNo & "</REFERENCE>" &
                         "<REFERENCEDATE>" & InvDateString & "</REFERENCEDATE>" &
                         "<NARRATION>" & Narration & "</NARRATION>" &
                         "<DIFFACTUALQTY>No</DIFFACTUALQTY>" &
                         "<VOUCHERNUMBER>" & Dr.GRN_No & "</VOUCHERNUMBER>" &
                         "<PARTYNAME>" & PartyLedgerSpare.Replace("&", "&amp;") & "</PARTYNAME>" &
                         "<PARTYLEDGERNAME>" & PartyLedgerSpare.Replace("&", "&amp;") & "</PARTYLEDGERNAME>" &
                         "<ASORIGINAL>Yes</ASORIGINAL>" &
                         "<BASICBUYERSSALESTAXNO></BASICBUYERSSALESTAXNO>" &
                         "<ALLLEDGERENTRIES.LIST>" &
                         "</ALLLEDGERENTRIES.LIST>" &
                         Main_Ledgers & Additional_Ledgers


            ' "<LEDGERENTRIES.LIST>" & _
            '    "<LEDGERNAME>" & PartyLedgerSpare.Replace("&", "&amp;") & "</LEDGERNAME>" & _
            '    "<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>" & _
            '    "<ISPARTYLEDGER>Yes</ISPARTYLEDGER>" & _
            '    "<AMOUNT>" & Dr.Invoice_Amt & "</AMOUNT>" & _
            '"</LEDGERENTRIES.LIST>"

            Xml = Xml & Xml_AllItems

            Xml = Xml & "</VOUCHER>" &
       "</TALLYMESSAGE>"


            Xml = Xml & "</REQUESTDATA>" &
         "</IMPORTDATA>" &
     "</BODY>" &
 "</ENVELOPE>"

            If HsnMissing <> "" Then
                'Show_ClipBoard("", HsnMissing)
            End If

        Catch ex As Exception
            '  FrmMDI.PleaseWait(False)
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message)
        End Try


        Return Xml


    End Function

    Private Function Create_XML_COSTCENTER(ByVal COSTCENTER As String, ByVal AMOUNT As String, Optional ByVal Ignore_ISDEEMEDPOSITIVE As Boolean = False) As String
        Dim Str As String = ""
        Dim CATEGORY = Read_Settings("CostCategory")
        Dim ISDEEMEDPOSITIVE As String = IIf(Val(AMOUNT) < 0, "Yes", "No")

        If Ignore_ISDEEMEDPOSITIVE Then
            ISDEEMEDPOSITIVE = "No"
        End If

        Str = "<CATEGORYALLOCATIONS.LIST>" &
              "<CATEGORY>" & CATEGORY & "</CATEGORY>" &
              "<ISDEEMEDPOSITIVE>" & ISDEEMEDPOSITIVE & "</ISDEEMEDPOSITIVE> " &
              "<COSTCENTREALLOCATIONS.LIST>" &
              "<NAME>" & COSTCENTER & "</NAME>" &
              "<AMOUNT>" & AMOUNT & "</AMOUNT>" &
              "</COSTCENTREALLOCATIONS.LIST>" &
              "</CATEGORYALLOCATIONS.LIST>"
        Return Str
    End Function

    Private Function Create_XML_ALLLEDGERENTRIES(ByVal LEDGERNAME As String, ByVal AMOUNT As String, ByVal COST_CENTER As String) As String
        Dim Str As String = ""
        Dim ISDEEMEDPOSITIVE As String = IIf(Val(AMOUNT) < 0, "Yes", "No")
        Str = "<LEDGERENTRIES.LIST>" &
              "<LEDGERNAME>" & LEDGERNAME & "</LEDGERNAME>" &
              "<ISDEEMEDPOSITIVE>" & ISDEEMEDPOSITIVE & "</ISDEEMEDPOSITIVE> " &
              "<AMOUNT>" & AMOUNT & "</AMOUNT> " &
              COST_CENTER &
              "</LEDGERENTRIES.LIST>"
        Return Str
    End Function

    Private Function Create_XML_ALLLEDGERENTRIES_RoundOff(ByVal LEDGERNAME As String, ByVal AMOUNT As String, ByVal COST_CENTER As String) As String
        Dim Str As String = ""
        Dim ISDEEMEDPOSITIVE As String = "Yes" 'IIf(Val(AMOUNT) < 0, "Yes", "No")
        Str = "<LEDGERENTRIES.LIST>" &
              "<LEDGERNAME>" & LEDGERNAME & "</LEDGERNAME>" &
              "<ISDEEMEDPOSITIVE>" & ISDEEMEDPOSITIVE & "</ISDEEMEDPOSITIVE> " &
              "<AMOUNT>" & AMOUNT & "</AMOUNT> " &
              COST_CENTER &
              "</LEDGERENTRIES.LIST>"
        Return Str
    End Function

    Public Function Check_Partno_not_exist() As String

        Dim err_str As String = ""
        Dim DsDetails As New TallyDs
        Dim dr As TallyDs.re_spare_purchaseRow
        If DsStock.Tables.Contains("StockItems_Names") Then


            For Each dr In ImportDs.re_spare_purchase.Rows
                DsDetails.re_spare_purchase_Detail.Clear()
                DsDetails.Merge(ImportDs.re_spare_purchase_Detail.Select("`Supplier_Invoice_No` = '" & dr.Supplier_Invoice_No & "'", ""))
                For Each drt In DsDetails.re_spare_purchase_Detail

                    If drt.Model = "145754/B" Then
                        Dim cc As String = ""
                    End If

                    If DsStock.Tables("StockItems_Names").Select("[$ADDITIONALNAME]='" & drt.Model & "'").Count = 0 Then
                        '    err_str = drt.Model & vbCrLf
                        err_str = IIf(err_str = "", vbCrLf & "BILL:" & dr.Supplier_Invoice_No, err_str)
                        err_str += vbCrLf & drt.Model.ToString.Trim

                    End If
                    If drt.HSN_code = "" Then
                        err_str += "HSN CODE Not Found " & drt.Model & vbCrLf
                    End If
                Next

            Next

        End If

        Return err_str


    End Function


    Private Sub Load_Gst_Details()
        GST_Ds = New TallyDs
        GST_Ds = ObjDA.Get_GST_Details(True, "I")
    End Sub


    Private Function Create_XML_LEDGERENTRIES_PARTYLEDGER(ByVal LEDGERNAME As String, ByVal AMOUNT As String, ByVal BillNo As String, ByVal BillAllocation As Boolean) As String
        Dim Str As String = ""
        Str = "<LEDGERENTRIES.LIST>" &
              "<LEDGERNAME>" & LEDGERNAME.Replace("&", "&amp;") & "</LEDGERNAME>" &
              "<ISDEEMEDPOSITIVE>" & IIf(AMOUNT < 0, "Yes", "No") & "</ISDEEMEDPOSITIVE> " &
              "<AMOUNT>" & AMOUNT & "</AMOUNT> " &
              "<ISPARTYLEDGER>Yes</ISPARTYLEDGER> " &
              "<ASORIGINAL>No</ASORIGINAL> "
        If BillAllocation Then
            Str += "<BILLALLOCATIONS.LIST>" &
              "<NAME>" & BillNo & "</NAME>" &
              "<BILLTYPE>New Ref</BILLTYPE>" &
              "<AMOUNT>" & AMOUNT & "</AMOUNT>" &
              "</BILLALLOCATIONS.LIST>"
        End If
        Str += "</LEDGERENTRIES.LIST>"
        Return Str
    End Function


    Private Function Xml_GST_Item(ByVal IGST As String, ByVal Cess As String, ByVal HSN_Desc As String, ByVal HSNCode As String) As String

        Dim Xml_Str As String

        Dim CGST As String = Math.Round(Val(IGST / 2), 2)
        Dim SGST As String = Math.Round(Val(IGST / 2), 2)

        Xml_Str = "<GSTAPPLICABLE>&#4; Applicable</GSTAPPLICABLE>" &
            "<GSTTYPEOFSUPPLY>Goods</GSTTYPEOFSUPPLY>"


        '"<APPLICABLEFROM>20170701</APPLICABLEFROM>" & _


        Xml_Str = Xml_Str & "<GSTDETAILS.LIST>" &
                "<APPLICABLEFROM>20170701</APPLICABLEFROM>" &
               "<CALCULATIONTYPE>On Value</CALCULATIONTYPE>" &
               "<HSNCODE>" & HSNCode & "</HSNCODE>" &
               "<HSN>" & HSN_Desc & "</HSN>" &
               "<TAXABILITY>Taxable</TAXABILITY>" &
               "<STATEWISEDETAILS.LIST>" &
               "<STATENAME>&#4; Any</STATENAME>" &
               "<RATEDETAILS.LIST>" &
                 "<GSTRATEDUTYHEAD>Central Tax</GSTRATEDUTYHEAD>" &
                 "<GSTRATEVALUATIONTYPE>Based on Value</GSTRATEVALUATIONTYPE>" &
                 "<GSTRATE>" & CGST & "</GSTRATE>" &
                 "<GSTRATEPERUNIT>0</GSTRATEPERUNIT>" &
                 "<TEMPGSTRATE>0</TEMPGSTRATE>" &
                "</RATEDETAILS.LIST>" &
                "<RATEDETAILS.LIST>" &
                 "<GSTRATEDUTYHEAD>State Tax</GSTRATEDUTYHEAD>" &
                 "<GSTRATEVALUATIONTYPE>Based on Value</GSTRATEVALUATIONTYPE>" &
                 "<GSTRATE>" & SGST & "</GSTRATE>" &
                 "<GSTRATEPERUNIT>0</GSTRATEPERUNIT>" &
                 "<TEMPGSTRATE>0</TEMPGSTRATE>" &
                "</RATEDETAILS.LIST>" &
                "<RATEDETAILS.LIST>" &
                 "<GSTRATEDUTYHEAD>Integrated Tax</GSTRATEDUTYHEAD>" &
                 "<GSTRATEVALUATIONTYPE>Based on Value</GSTRATEVALUATIONTYPE>" &
                 "<GSTRATE>" & IGST & "</GSTRATE>" &
                 "<GSTRATEPERUNIT>0</GSTRATEPERUNIT>" &
                 "<TEMPGSTRATE>0</TEMPGSTRATE>" &
                "</RATEDETAILS.LIST>" &
                "<RATEDETAILS.LIST>" &
                    "<GSTRATEDUTYHEAD>Cess</GSTRATEDUTYHEAD>" &
                    "<GSTRATEVALUATIONTYPE>Based on Value</GSTRATEVALUATIONTYPE>" &
                    "<GSTRATE>" & Cess & "</GSTRATE>" &
                    "<GSTRATEPERUNIT>0</GSTRATEPERUNIT>" &
                    "<TEMPGSTRATE>0</TEMPGSTRATE>" &
                "</RATEDETAILS.LIST>" &
                "<GSTSLABRATES.LIST></GSTSLABRATES.LIST>" &
               "</STATEWISEDETAILS.LIST>" &
              "</GSTDETAILS.LIST>"


        Return Xml_Str

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
                   " <VoucherTypeName>" & VoucherTypeName & "</VoucherTypeName>" &
                   " </STATICVARIABLES>" &
                   " </REQUESTDESC>" &
                   " </EXPORTDATA>" &
                   " </BODY>" &
                   "</ENVELOPE>"


        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try

        Return strXmldata

    End Function

    Public Function Export_Tally_Method1(ByVal StrXmldata As String) As DataSet

        Dim Ds As New DataSet
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
            CommonDA.Create_Log("Import to Tally", "Import-Error", "Failed to check existing entries in tally...")
            Return Ds
        End Try

        Return Ds

    End Function

    Public Function Export_Tally_Method2(ByVal RequestXML) As DataSet

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
        Dim ResponseFromtally As String = responseFromTallyServer.ToString()
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


    Public Function Export_Xml(ByVal FromDate As String, ByVal ToDate As String, ByVal VoucherTypeName As String) As Boolean

        Dim Ds As New DataSet
        Dim i As Integer = 1
        Dim XmlDS As New DataSet
        Dim StrXmldata As String

        Try

            StrXmldata = XmlFormat_Voucher_Register(FromDate, ToDate, VoucherTypeName)
            XmlDS = Export_Tally_Method1(StrXmldata)

            If XmlDS.Tables.Count = 0 Then
                XmlDS = Export_Tally_Method2(StrXmldata)
            End If

            If XmlDS.Tables.Contains("VOUCHER") Then
                PublicTallyDS.Merge(XmlDS.Tables("VOUCHER"))
            ElseIf XmlDS.Tables.Count >= 4 Then
            Else
                MsgBox("Failed to check existing entries in tally..." & vbCrLf & " Table Count " & XmlDS.Tables.Count)
                Return False
            End If


        Catch ex As Exception
            MsgBox("Failed to check existing entries in tally...")
            Return False
        End Try

        Return True

    End Function

    Public Shared Function Read_Ledgers(ByVal S_Key As String) As String
        Dim Value As String = ""
        Try
            Value = PublicShared.Ledgers_Dt.Select("S_Key='" & S_Key & "'").First.Item("S_Value").ToString
        Catch ex As Exception
        End Try
        Return Value
    End Function

    Private Sub Load_Tally_StockItems()

        'FrmMDI.PleaseWait(True)
        Me.Cursor = Cursors.WaitCursor
        Dim Qry As String
        Dim StockSpareParent As String = Read_Ledgers("Stock_Parent_Spare")
        Dim TallyCon As New OdbcConnection(Read_Settings("TallyODBC"))
        Dim Cmd As New OdbcCommand
        Dim Da As New OdbcDataAdapter
        Dim DrD As TallyDs.re_spare_purchase_DetailRow
        Dim UOM As String = ""
        Dim StockSpareParent_To_Create As String = ""
        Dim TempDsStock As New DataSet
        Dim PartNo_Str As String = ""
        Dim Desc_Missing_Count As Integer = 0
        Dim Part_Name As String = ""

        Dim TempDs As New TallyDs
        Dim No As Integer = 0
        Dim AllItemsDS As New DataSet
        Dim ObjDA As New CommonDA
        Dim HsnMissing As String = ""
        DsStock.Clear()
        ' PartNo_tax = ""

        TempDs.Merge(ImportDs.re_spare_purchase_Detail)

        Try

            Cmd.Connection = TallyCon
            TallyCon.Open()


            Qry = "Select $NAME,$BASEUNITS,$ADDITIONALNAME from ListofStockItems "
            'If StockSpareParent = "" Then
            '    Qry = "Select $NAME,$BASEUNITS,$ADDITIONALNAME from ListofStockItems "
            '    Else
            '       Qry = "Select $NAME,$BASEUNITS,$ADDITIONALNAME from ListofStockItems WHERE  $PARENT = '" & StockSpareParent & "'"
            'End If

            Cmd.CommandText = Qry
            Da.SelectCommand = Cmd
            Da.Fill(AllItemsDS, "StockItems_Names")

            DsStock.Merge(AllItemsDS.Tables(0))
            DsStock.Tables(0).Clear()

            '    MsgBox("Tallyfill")
            TallyCon.Close()
            TallyCon.Dispose()

            For Each DrD In TempDs.re_spare_purchase_Detail.Rows

                No += 1
                '  FrmMDI.PleaseWait_Progress("Please Wait  " & No & "/" & TempDs.re_spare_purchase_Detail.Rows.Count)

                If DrD.UOM = "" Then
                    UOM = Read_Settings("Default_UOM")
                End If

                If UOM = "" Then
                    UOM = Read_Settings("Default_UOM")
                End If

                If StockSpareParent = "" Then
                    StockSpareParent_To_Create = Read_Settings("Stock_Parent_Spare")
                End If

                TempDsStock = New DataSet
                TempDsStock.Merge(AllItemsDS.Tables("StockItems_Names").Select("[$ADDITIONALNAME] = '" & DrD.Model & "'"))

                If TempDsStock.Tables.Count > 0 Then

                    DsStock.Merge(TempDsStock.Tables("StockItems_Names"))


                    ''Part_Name = ""
                    ''Part_Name = TempDsStock.Tables("StockItems_Names")(0)("$Name").ToString
                    ''Check_Tax_Mismatch(DrD.Shipped_Part, Part_Name, DrD.GSTPer)

                ElseIf TempDsStock.Tables.Count = 0 Then

                    If DrD.Description = "" Then

                        Desc_Missing_Count += 1
                        If PartNo_Str <> "" Then
                            PartNo_Str = PartNo_Str & vbCrLf
                        End If

                        PartNo_Str = PartNo_Str & DrD.Model

                    End If

                    ' ObjDA = New DmsDA
                    '  ObjDA.Save_PurchaseSpare_UOM(DrD.Id, UOM, DrD.Model, StockSpareParent_To_Create)

                End If



            Next


            If Desc_Missing_Count > 0 Then
                Dim Str_title As String = Desc_Missing_Count & " entries are found without PartName "
                '   Show_ClipBoard(Str_title, PartNo_Str)

            End If

            ''If PartNo_tax <> "" Then
            ''    Dim Str_tax_title As String = "Tax Mismatch "
            ''    Show_ClipBoard("Tax Mismatch ", PartNo_tax)
            ''End If

        Catch ex As Exception
            MsgBox("Load Items from tally : " & ex.Message, MsgBoxStyle.Critical, "CSMS")
            Me.Cursor = Cursors.Default
            ' FrmMDI.PleaseWait(False)
        End Try

        '    MsgBox("Tally completed")
        Me.Cursor = Cursors.Default
        ' FrmMDI.PleaseWait(False)

    End Sub

    Private Sub Validate_TallyGST()

        Load_Stock_GST()

        If TaxDs.Tables("PartName").Rows.Count = 0 Then
            Exit Sub
        End If




        Dim Qry As String
        Dim StockSpareParent As String = Read_Ledgers("Stock_Parent_Spare")
        Dim TallyCon As New OdbcConnection(Read_Settings("TallyODBC"))
        Dim Cmd As New OdbcCommand
        Dim Da As New OdbcDataAdapter
        Dim DrD As TallyDs.re_spare_purchase_DetailRow
        Dim UOM As String = ""
        Dim StockSpareParent_To_Create As String = ""
        Dim TempDsStock As New DataSet
        Dim PartNo_Str As String = ""
        Dim Desc_Missing_Count As Integer = 0
        Dim Part_Name As String = ""

        Dim TempDs As New TallyDs
        Dim No As Integer = 0
        Dim AllItemsDS As New DataSet
        Dim ObjDA As New CommonDA

        DsStock.Clear()
        PartNo_tax = ""
        Ds = CommonDA.Get_Summary_SparePurchase(Nothing, Nothing)
        TempDs.Merge(Ds.re_spare_purchase_Detail)

        Try

            Cmd.Connection = TallyCon
            TallyCon.Open()

            If StockSpareParent = "" Then
                Qry = "Select $NAME,$BASEUNITS,$ADDITIONALNAME from ListofStockItems "
            Else
                Qry = "Select $NAME,$BASEUNITS,$ADDITIONALNAME from ListofStockItems WHERE  $PARENT = '" & StockSpareParent & "'"
            End If

            Cmd.CommandText = Qry
            Da.SelectCommand = Cmd
            Da.Fill(AllItemsDS, "StockItems_Names")

            TallyCon.Close()
            TallyCon.Dispose()

            For Each DrD In TempDs.re_spare_purchase_Detail.Rows

                No += 1
                '     FrmMDI.PleaseWait_Progress("Please Wait  " & No & "/" & TempDs.Spare_Purchase.Rows.Count)

                TempDsStock = New DataSet
                TempDsStock.Merge(AllItemsDS.Tables("StockItems_Names").Select("[$ADDITIONALNAME] = '" & DrD.Model & "'"))

                If TempDsStock.Tables.Count > 0 Then

                    ''DsStock.Merge(TempDsStock.Tables("StockItems_Names"))

                    Part_Name = ""
                    Part_Name = TempDsStock.Tables("StockItems_Names")(0)("$Name").ToString
                    Check_Tax_Mismatch(DrD.Model, Part_Name, DrD.IGST_per)

                End If

            Next


            If PartNo_tax <> "" Then
                Dim Str_tax_title As String = "Tax Mismatch "
                ' Show_ClipBoard("Tax Mismatch ", PartNo_tax)

            Else
                MsgBox("No Items found : ", MsgBoxStyle.Information, "CSMS")
            End If

        Catch ex As Exception
            MsgBox("Load Items from tally : " & ex.Message, MsgBoxStyle.Critical, "CSMS")

        End Try


    End Sub


    Private Sub Load_Stock_GST()


        TaxDs = New DataSet
        TaxDs.Tables.Add("PartName")
        TaxDs.Tables("PartName").Columns.Add("Name", Type.GetType("System.String"))
        TaxDs.Tables("PartName").Columns.Add("PartNo", Type.GetType("System.String"))
        TaxDs.Tables("PartName").Columns.Add("Unit", Type.GetType("System.String"))
        TaxDs.Tables("PartName").Columns.Add("Tax", Type.GetType("System.String"))
        TaxDs.Tables("PartName").Columns.Add("NewTax", Type.GetType("System.String"))
        TaxDs.Tables("PartName").Columns.Add("APPLICABLEFROM", Type.GetType("System.String"))

        Dim StrXmldata As String
        Dim XDoc As New Xml.XmlDocument
        Dim XmlnodeL As XmlNodeList
        Dim Dr As DataRow
        Try

            StrXmldata = XmlFormat_Master_Inventory()
            StrXmldata = Export_Tally_Method_Return_Text(StrXmldata)
            ''StrXmldata = Get_Xml_Data()
            XDoc.Load(New StringReader(StrXmldata))
            ' XmlnodeL = XDoc.GetElementsByTagName("NAME") '"STOCKNAME.LIST"
            XmlnodeL = XDoc.GetElementsByTagName("STOCKITEM") '"STOCKNAME.LIST"

            For i As Integer = 0 To XmlnodeL.Count - 1
                Dr = TaxDs.Tables("PartName").Rows.Add
                Dr("PartNo") = XmlnodeL(i).ChildNodes.Item(0).InnerText.Trim()
                For Each ObjNode As XmlNode In XmlnodeL(i)

                    If ObjNode.Name = "BASEUNITS" Then
                        Dr("Unit") = ObjNode.InnerText.Trim()
                    ElseIf ObjNode.Name = "LANGUAGENAME.LIST" Then
                        For Each ObjSubNode As XmlNode In ObjNode.ChildNodes
                            If ObjSubNode.Name = "NAME.LIST" Then
                                Dr("Name") = ObjSubNode.InnerText.Trim()
                            End If
                        Next
                    End If


                    If ObjNode.Name = "GSTDETAILS.LIST" Then

                        For Each ObjSubNode1 As XmlNode In ObjNode.ChildNodes

                            If ObjSubNode1.Name = "APPLICABLEFROM" Then
                                Dr("APPLICABLEFROM") = ObjSubNode1.InnerText.Trim()

                            ElseIf ObjSubNode1.Name = "STATEWISEDETAILS.LIST" Then

                                For Each ObjSubNode2 As XmlNode In ObjSubNode1.ChildNodes
                                    If ObjSubNode2.Name = "RATEDETAILS.LIST" Then

                                        For Each ObjSubNode3 As XmlNode In ObjSubNode2.ChildNodes
                                            If ObjSubNode3.Name = "GSTRATEDUTYHEAD" And ObjSubNode3.InnerText.Trim() <> "Integrated Tax" Then
                                                Exit For
                                            End If

                                            If ObjSubNode3.Name = "GSTRATE" Then

                                                Dr("Tax") = ObjSubNode3.InnerText.Trim()

                                            End If
                                        Next
                                    End If
                                Next
                            End If
                        Next
                    End If
                    'GSTDETAILS.LIST

                Next
            Next
            TaxDs.AcceptChanges()

        Catch ex As Exception

            MsgBox("Failed to load stock items from tally..." & ex.Message)
        End Try

    End Sub

    Private Sub Load_Stock_GST_History()

        'FrmMDI.PleaseWait(True)

        TaxDs = New DataSet
        TaxDs.Tables.Add("PartName")
        TaxDs.Tables("PartName").Columns.Add("Name", Type.GetType("System.String"))
        TaxDs.Tables("PartName").Columns.Add("PartNo", Type.GetType("System.String"))
        TaxDs.Tables("PartName").Columns.Add("Unit", Type.GetType("System.String"))
        TaxDs.Tables("PartName").Columns.Add("APPLICABLEFROM", Type.GetType("System.String"))
        TaxDs.Tables("PartName").Columns.Add("HSNCODE", Type.GetType("System.String"))
        TaxDs.Tables("PartName").Columns.Add("CGST", Type.GetType("System.String"))
        TaxDs.Tables("PartName").Columns.Add("SGST", Type.GetType("System.String"))
        TaxDs.Tables("PartName").Columns.Add("IGST", Type.GetType("System.String"))
        TaxDs.Tables("PartName").Columns.Add("CESS", Type.GetType("System.String"))
        Dim StrXmldata As String
        Dim XDoc As New Xml.XmlDocument
        Dim XmlnodeL As XmlNodeList
        Dim Dr As DataRow

        Dim IsCGST As Boolean = False
        Dim IsSGST As Boolean = False
        Dim IsIGST As Boolean = False
        Dim IsCESS As Boolean = False

        Dim j As Integer = 0
        Dim PartNo As String = ""
        Dim Unit As String = ""
        Dim Name As String = ""


        Try

            StrXmldata = XmlFormat_Master_Inventory_History()
            StrXmldata = Export_Tally_Method_Return_Text(StrXmldata)
            ''StrXmldata = Get_Xml_Data()
            XDoc.Load(New StringReader(StrXmldata))
            ' XmlnodeL = XDoc.GetElementsByTagName("NAME") '"STOCKNAME.LIST"
            XmlnodeL = XDoc.GetElementsByTagName("STOCKITEM") '"STOCKNAME.LIST"


            For i As Integer = 0 To XmlnodeL.Count - 1

                If XmlnodeL(i).ChildNodes.Item(0).InnerText.Trim() <> "6RG845025A" Then
                    Continue For
                End If

                ' PartNo = "" : Unit = "" : Name = ""
                j = 0


                Dr = TaxDs.Tables("PartName").Rows.Add
                Dr("PartNo") = XmlnodeL(i).ChildNodes.Item(0).InnerText.Trim()
                PartNo = Dr("PartNo")

                Name = XmlnodeL(i).Attributes.ItemOf(0).InnerText
                Dr("Name") = Name

                For Each ObjNode As XmlNode In XmlnodeL(i)

                    If ObjNode.Name = "BASEUNITS" Then
                        Dr("Unit") = ObjNode.InnerText.Trim()
                        Unit = Dr("Unit")

                        'ElseIf ObjNode.Name = "LANGUAGENAME.LIST" Then
                        '    For Each ObjSubNode As XmlNode In ObjNode.ChildNodes
                        '        If ObjSubNode.Name = "NAME.LIST" Then
                        '            Dr("Name") = ObjSubNode.InnerText.Trim()
                        '            Name = Dr("Name")
                        '        End If
                        '    Next
                    End If


                    If ObjNode.Name = "GSTDETAILS.LIST" Then

                        j = j + 1
                        If j > 1 Then
                            Dr = TaxDs.Tables("PartName").Rows.Add
                            Dr("PartNo") = PartNo
                            Dr("Unit") = Unit
                            Dr("Name") = Name

                        End If


                        For Each ObjSubNode1 As XmlNode In ObjNode.ChildNodes
                            If ObjSubNode1.Name = "APPLICABLEFROM" Then
                                'If ObjSubNode1.InnerText.Trim() = "" Then
                                '    Continue FO
                                'End If
                                Dr("APPLICABLEFROM") = ObjSubNode1.InnerText.Trim()

                            ElseIf ObjSubNode1.Name = "HSNCODE" Then
                                Dr("HSNCODE") = ObjSubNode1.InnerText.Trim()


                            ElseIf ObjSubNode1.Name = "STATEWISEDETAILS.LIST" Then
                                For Each ObjSubNode2 As XmlNode In ObjSubNode1.ChildNodes
                                    If ObjSubNode2.Name = "RATEDETAILS.LIST" Then
                                        For Each ObjSubNode3 As XmlNode In ObjSubNode2.ChildNodes

                                            If ObjSubNode3.Name = "GSTRATEDUTYHEAD" Then

                                                If ObjSubNode3.InnerText.Trim() = "Central Tax" Then
                                                    IsCGST = True

                                                ElseIf ObjSubNode3.InnerText.Trim() = "State Tax" Then
                                                    IsSGST = True

                                                ElseIf ObjSubNode3.InnerText.Trim() = "Integrated Tax" Then
                                                    IsIGST = True

                                                ElseIf ObjSubNode3.InnerText.Trim() = "Cess" Then
                                                    IsCESS = True

                                                End If

                                            End If


                                            If ObjSubNode3.Name = "GSTRATE" Then
                                                If IsCGST = True Then
                                                    Dr("CGST") = ObjSubNode3.InnerText.Trim()
                                                    IsCGST = False
                                                ElseIf IsSGST = True Then
                                                    Dr("SGST") = ObjSubNode3.InnerText.Trim()
                                                    IsSGST = False
                                                ElseIf IsIGST = True Then
                                                    Dr("IGST") = ObjSubNode3.InnerText.Trim()
                                                    IsIGST = False

                                                ElseIf IsCESS = True Then
                                                    Dr("CESS") = ObjSubNode3.InnerText.Trim()
                                                    IsCESS = False

                                                End If
                                            End If
                                        Next
                                    End If
                                Next

                            End If

                        Next
                    End If
                    'GSTDETAILS.LIST

                Next


            Next
            TaxDs.AcceptChanges()

        Catch ex As Exception
            'FrmMDI.PleaseWait(False)
            MsgBox("Failed to load stock items from tally..." & ex.Message)
        End Try
        '  FrmMDI.PleaseWait(False)

    End Sub

    Public Function XmlFormat_Master_Inventory_History() As String

        Dim strXmldata As String = ""


        Try

            'strXmldata = _
            '       "<ENVELOPE>" & _
            '        "<HEADER>" & _
            '        "<TALLYREQUEST>Export Data</TALLYREQUEST>" & _
            '        "</HEADER>" & _
            '        "<BODY>" & _
            '        "<EXPORTDATA>" & _
            '        "<REQUESTDESC>" & _
            '        "<REPORTNAME>List of Accounts</REPORTNAME>" & _
            '        "<STATICVARIABLES>" & _
            '        "<SVEXPORTFORMAT>$$SysName:XML</SVEXPORTFORMAT>" & _
            '        "<SVCURRENTCOMPANY>" & BranchName.Replace("&", "&amp;") & "</SVCURRENTCOMPANY>" & _
            '        "<ACCOUNTTYPE>All Inventory Masters</ACCOUNTTYPE>" & _
            '        "<MAILINGNAME>000012147A</MAILINGNAME>" & _
            '        "</STATICVARIABLES>" & _
            '        "</REQUESTDESC>" & _
            '        "</EXPORTDATA>" & _
            '        "</BODY>" & _
            '        "</ENVELOPE>"

            'strXmldata = _
            '     "<ENVELOPE>" & _
            '      "<HEADER>" & _
            '      "<TALLYREQUEST>Export Data</TALLYREQUEST>" & _
            '      "</HEADER>" & _
            '      "<BODY>" & _
            '      "<EXPORTDATA>" & _
            '      "<REQUESTDESC>" & _
            '      "<REPORTNAME>List of Accounts</REPORTNAME>" & _
            '      "<STATICVARIABLES>" & _
            '      "<STOCKITEMNAME>000012147A[BOTTLE]</STOCKITEMNAME>" & _
            '      "</STATICVARIABLES>" & _
            '      "</REQUESTDESC>" & _
            '      "</EXPORTDATA>" & _
            '      "</BODY>" & _
            '      "</ENVELOPE>"

            strXmldata =
                  "<ENVELOPE>" &
                   "<HEADER>" &
                   "<TALLYREQUEST>Export Data</TALLYREQUEST>" &
                   "</HEADER>" &
                   "<BODY>" &
                   "<EXPORTDATA>" &
                   "<REQUESTDESC>" &
                   "<REPORTNAME>List of Accounts</REPORTNAME>" &
                   "<STATICVARIABLES>" &
                   "<SVEXPORTFORMAT>$$SysName:XML</SVEXPORTFORMAT>" &
                   "<SVCURRENTCOMPANY>" & BranchName.Replace("&", "&amp;") & "</SVCURRENTCOMPANY>" &
                   "<ACCOUNTTYPE>All Inventory Masters</ACCOUNTTYPE>" &
                   "<PARENT>Vehicle</PARENT>" &
                   "</STATICVARIABLES>" &
                   "</REQUESTDESC>" &
                   "</EXPORTDATA>" &
                   "</BODY>" &
                   "</ENVELOPE>"


        Catch ex As Exception
        End Try

        Return strXmldata

    End Function


    Private Sub Check_Tax_Mismatch(ByVal PartNo As String, ByVal Name As String, ByVal DMSTax_Perc As String)
        Try
            If TaxDs.Tables("PartName").Select("Name = '" & Name & "'").Count = 0 Then
                Exit Sub
            End If

            Dim TallyGST = Val(TaxDs.Tables("PartName").Select("Name = '" & Name & "'")(0)("Tax").trim)

            If TallyGST <> Val(DMSTax_Perc) Then

                If PartNo_tax <> "" Then
                    PartNo_tax = PartNo_tax & vbCrLf
                End If

                Dim Tax_tally_perc As Decimal = 0
                Tax_tally_perc = 0
                Tax_tally_perc = Format(TallyGST, "0.00")
                PartNo_tax = PartNo_tax & PartNo & "    GST : " & DMSTax_Perc & " % (Tally : " & Tax_tally_perc & " %)"

            End If

        Catch ex As Exception

        End Try

    End Sub

    Public Function XmlFormat_Master_Inventory() As String

        Dim strXmldata As String = ""

        Try

            strXmldata =
                   "<ENVELOPE>" &
                    "<HEADER>" &
                    "<TALLYREQUEST>Export Data</TALLYREQUEST>" &
                    "</HEADER>" &
                    "<BODY>" &
                    "<EXPORTDATA>" &
                    "<REQUESTDESC>" &
                    "<REPORTNAME>List of Accounts</REPORTNAME>" &
                    "<STATICVARIABLES>" &
                    "<SVEXPORTFORMAT>$$SysName:XML</SVEXPORTFORMAT>" &
                    "<ACCOUNTTYPE>All Inventory Masters</ACCOUNTTYPE>" &
                    "</STATICVARIABLES>" &
                    "</REQUESTDESC>" &
                    "</EXPORTDATA>" &
                    "</BODY>" &
                    "</ENVELOPE>"

        Catch ex As Exception
        End Try

        Return strXmldata

    End Function

    Private Sub Btnupdate_Click(sender As Object, e As EventArgs) Handles Btnupdate.Click
        If ListView1.SelectedItems(0).SubItems(3).Text = "" Then

            Dim DocNumber = Txtupdate.Text
            Dim Qry As String = ""
            Qry = "Update re_spare_purchase set Invoice_No = '" & DocNumber & "',Updated_On =now() " &
                      "Where `GRN No`='" & ListView1.SelectedItems(0).SubItems(2).Text & "'"

            If CommonDA.RunQuery(Qry) Then
                Txtupdate.Text = ""
                find()
                PanelSparePurchase.Visible = False
            End If

        Else

            Dim DocNumber = Txtupdate.Text

            Dim Qry As String = ""
            Qry = "Update re_spare_purchase set Invoice_No = '" & DocNumber & "',Updated_On =now() " &
                          "Where `GRN No`='" & ListView1.SelectedItems(0).SubItems(2).Text & "'"

            If CommonDA.RunQuery(Qry) Then
                Txtupdate.Text = ""
                find()
                PanelSparePurchase.Visible = False
            End If
        End If

    End Sub

    Private Sub ListView1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles ListView1.MouseDoubleClick
        EditDocNumber()
    End Sub
    Private Sub EditDocNumber()

        If ListView1.SelectedItems.Count > 0 Then

            If ListView1.SelectedItems(0).SubItems(3).Text = "" Then
                PanelSparePurchase.Visible = True
            Else
                If MsgBox("Do you want to Change the Invoice Number " & ListView1.SelectedItems(0).SubItems(3).Text & "", vbYesNo, "Confirm") = vbYes Then
                    PanelSparePurchase.Visible = True
                End If
            End If

        Else
            MsgBox("No items..!!")
        End If

    End Sub

    Private Sub BtnClosePanel_Click(sender As Object, e As EventArgs) Handles BtnClosePanel.Click
        PanelSparePurchase.Visible = False
    End Sub

    Public Function Export_Tally_Method_Return_Text(ByVal RequestXML) As String

        Dim TallyRequest As HttpWebRequest
        Dim byteArray As Byte()
        Dim dataStream As Stream
        Dim response As HttpWebResponse
        Dim ResponseStr As String
        Dim reader As StreamReader
        Dim responseFromTallyServer As String

        'RequestXML = "<ENVELOPE><HEADER><TALLYREQUEST>Export Data</TALLYREQUEST></HEADER><BODY><EXPORTDATA><REQUESTDESC><REPORTNAME>List of Accounts</REPORTNAME><STATICVARIABLES><SVEXPORTFORMAT>$$SysName:XML</SVEXPORTFORMAT><ACCOUNTTYPE>All Inventory Masters</ACCOUNTTYPE></STATICVARIABLES></REQUESTDESC></EXPORTDATA></BODY></ENVELOPE>";
        TallyRequest = WebRequest.Create(TallyHost)
        TallyRequest.UserAgent = ".NET Framework Example Client"

        TallyRequest.Timeout = 1000000
        TallyRequest.ReadWriteTimeout = 1000000


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
        responseFromTallyServer = responseFromTallyServer.Replace("", "") 'Removing Splecial Char  - Not Visible 

        'Display the content.
        'string ResponseFromtally=responseFromServer.ToString();
        ' Clean up the streams.
        reader.Close()
        dataStream.Close()
        response.Close()
        byteArray = Nothing
        response = Nothing
        response = Nothing
        dataStream = Nothing
        ' RequestClient.open("Get", "http://localhost:9000/", false, null, null);
        ' RequestClient.send(
        ' IXMLDOMNode ResponseXml = (IXMLDOMNode)RequestClient.responseXML;

        GC.Collect()
        Return responseFromTallyServer

    End Function



End Class
