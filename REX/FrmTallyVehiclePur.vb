Imports System.Data.Odbc
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Xml

Public Class FrmTallyVehiclePur
    Dim VehicleDS As TallyDs

    Public Sub New()
        VehicleDS = CommonDA.Get_Summary_purchasevehicle()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Dim VehMaster As New TallyDs
    Dim ImportDs As TallyDs
    Dim lvItem As New ListViewItem
    Dim TallyHost As String = ""
    Dim Import_Log As String = ""
    Dim End_Process As Boolean
    Dim BranchName As String = ""
    Dim Dr As TallyDs.PurchaseVehicleRow


    Dim ResponseDt As DataTable
    Dim PurchaseVehicle_Path As String
    Dim PublicTallyDS As New DataSet
    Dim Existing_Entry_Checked As Boolean
    Dim StockGroupsDS As New DataSet
    Dim InvDS As New TallyDs

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
        TallyHost = Read_Settings("TallyHost")
        BranchName = Read_Settings("TallyCompanyBranch")
        Create_Header()
        Create_Heads()
        CmbSearchBy.SelectedIndex = 0


    End Sub

    Private Sub Create_Header()

        ListView1.Columns.Add("id", 0)
        ListView1.Columns.Add("SL.NO", 40)
        ListView1.Columns.Add("INVOICE NO.", 110)
        ListView1.Columns.Add("INVOICE DATE", 110, HorizontalAlignment.Right)
        ListView1.Columns.Add("SUP. NUMBER", 120)
        ListView1.Columns.Add("SUP. DATE", 110, HorizontalAlignment.Right)
        ListView1.Columns.Add("CGST", 100, HorizontalAlignment.Right)
        ListView1.Columns.Add("SGST", 100, HorizontalAlignment.Right)
        ListView1.Columns.Add("CESS", 100, HorizontalAlignment.Right)
        ListView1.Columns.Add("TOTAL QTY", 85, HorizontalAlignment.Right)
        ListView1.Columns.Add("RATE", 100, HorizontalAlignment.Right)
        ListView1.Columns.Add("TOTAL", 100, HorizontalAlignment.Right)
        ListView1.Columns.Add("TALLY", 60, HorizontalAlignment.Center)


    End Sub

    Private Sub Create_Heads()

        ListView2.Columns.Add("ID", 0)
        ListView2.Columns.Add("SL.NO", 40)
        ListView2.Columns.Add("DESCRIPTION", 180)
        ListView2.Columns.Add("CHASE NO", 250, HorizontalAlignment.Left)
        ListView2.Columns.Add("ENFINE NO", 80)
        ListView2.Columns.Add("MODEL", 50)
        ListView2.Columns.Add("RATE", 88, HorizontalAlignment.Right)
        ListView2.Columns.Add("SGST%", 60, HorizontalAlignment.Center)
        ListView2.Columns.Add("SGST", 70, HorizontalAlignment.Right)
        ListView2.Columns.Add("CGST%", 60, HorizontalAlignment.Center)
        ListView2.Columns.Add("CGST", 70, HorizontalAlignment.Right)
        ListView2.Columns.Add("CESS%", 60, HorizontalAlignment.Center)
        ListView2.Columns.Add("CESS", 70, HorizontalAlignment.Right)
        ListView2.Columns.Add("FREIGHT", 90, HorizontalAlignment.Right)
        ListView2.Columns.Add("AMOUNT (RS)", 88, HorizontalAlignment.Right)

    End Sub

    Private Sub BtnFind_Click(sender As Object, e As EventArgs) Handles BtnFind.Click
        find()
    End Sub

    Private Sub find()


        ImportDs = New TallyDs
        Dim i As Integer = 0

        If RbtActive.Checked = True Then
            ImportDs.Merge(VehicleDS.PurchaseVehicle.Select("To_Tally=0 and Invoice_Number <>'' "))
            ImportDs.Merge(VehicleDS.PurchaseVehicle_Detail.Select("To_Tally=0 and Invoice_Number <> ''"))
        ElseIf RbtImported.Checked = True Then
            ImportDs.Merge(VehicleDS.PurchaseVehicle.Select("To_Tally=1"))
            ImportDs.Merge(VehicleDS.PurchaseVehicle_Detail.Select("To_Tally=1"))
        ElseIf RbtMissing.Checked = True Then
            ImportDs.Merge(VehicleDS.PurchaseVehicle.Select("Invoice_Number='' and To_Tally =0"))
        ElseIf RbtAll.Checked = True Then
            ImportDs = VehicleDS
        End If


        'Inv_Number

        Dim TempDs As New TallyDs


        If CmbSearchBy.Text = "Doc_Number" Then

            TempDs.Merge(ImportDs.PurchaseVehicle.Select("Invoice_No like '%" & TxtSearch.Text.Trim & "%'"))
            TempDs.Merge(ImportDs.PurchaseVehicle_Detail.Select("Invoice_No like '%" & TxtSearch.Text.Trim & "%'"))
            ImportDs = TempDs
        ElseIf CmbSearchBy.Text = "Inv_Number" Then

            TempDs.Merge(ImportDs.PurchaseVehicle.Select("Invoice_Number like '%" & TxtSearch.Text.Trim & "%'"))
            TempDs.Merge(ImportDs.PurchaseVehicle_Detail.Select("Invoice_Number like '%" & TxtSearch.Text.Trim & "%'"))
            ImportDs = TempDs
        End If

        If RbtMissing.Checked = False Then
            ImportDs = CommonDA.Remove_Null(ImportDs)
        End If
        ListView1.Items.Clear()
        For Each Dr In ImportDs.PurchaseVehicle.Rows
            i += 1
            lvItem = ListView1.Items.Add(Dr.Id)
            lvItem.SubItems.Add(i)
            lvItem.SubItems.Add(Dr.Invoice_Number)
            lvItem.SubItems.Add(Dr.Document_Date)
            lvItem.SubItems.Add(Dr.Invoice_No)
            lvItem.SubItems.Add(IIf(IsDBNull(Dr.Invoice_Date) = True, "", Dr.Invoice_Date))
            lvItem.SubItems.Add(Dr.CGST)
            lvItem.SubItems.Add(Dr.SGST)
            lvItem.SubItems.Add(Dr.CESS)
            lvItem.SubItems.Add(Dr.Total_Qty)
            lvItem.SubItems.Add(Dr.Rate)
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
        Dim InvoiceNum As String = ListView1.SelectedItems.Item(0).SubItems(4).Text
        DetailedDs.Merge(VehicleDS.PurchaseVehicle_Detail.Select("Invoice_No = '" & InvoiceNum & "'"))

        ListView2.Items.Clear()
        For Each drd In DetailedDs.PurchaseVehicle_Detail.Rows

            i += 1
            lvItem = ListView2.Items.Add(drd.Id)
            lvItem.SubItems.Add(i)
            lvItem.SubItems.Add(drd.Description)
            lvItem.SubItems.Add(drd.Chassis_No)
            lvItem.SubItems.Add(drd.Engine_No)
            lvItem.SubItems.Add(drd.ModelFamily)
            lvItem.SubItems.Add(drd.Rate)
            lvItem.SubItems.Add(drd.SGST_Per)
            lvItem.SubItems.Add(drd.SGST)
            lvItem.SubItems.Add(drd.CGST_Per)
            lvItem.SubItems.Add(drd.CGST)
            lvItem.SubItems.Add(drd.CESS_Per)
            lvItem.SubItems.Add(drd.CESS)
            lvItem.SubItems.Add(drd.Freight)
            lvItem.SubItems.Add(drd.Total_Amount)

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

        If ImportDs.PurchaseVehicle.Rows.Count > 0 Then

            If RbtActive.Checked = True Then
                If TxtSearch.Text = "" Then

                    ImportDs = New TallyDs
                    ImportDs.Merge(VehicleDS.PurchaseVehicle.Select("To_Tally=0 and Invoice_Number <>'' "))
                    ImportDs.Merge(VehicleDS.PurchaseVehicle_Detail.Select("To_Tally=0 and Invoice_Number <> ''"))
                    '  BtnImport.Width = 0
                Else
                    'ImportDs.PurchaseVehicle.Rows.Clear()
                    'ImportDs.Merge(VehicleDS.PurchaseVehicle_Detail.Select("Invoice_Number like '%" & TxtSearch.Text.Trim & "%'"))


                End If
                PanelPlsWait.Visible = True
                Import_To_Tally(ImportDs)
                PanelPlsWait.Visible = False

                'BtnImport.Width = PnlImport.Width
            Else
                MsgBox("No Active Items Found To Import")
            End If


        Else

        End If
        PanelPlsWait.Visible = False

    End Sub

    'Public Shared Function ProgresBar(ByVal Count As Integer, ByRef buton As Button, ByRef pnl As Panel)


    '    If buton.Width <= pnl.Width Then
    '        buton.Width += pnl.Width / Count
    '    Else
    '        buton.Width = pnl.Width
    '    End If


    '    Return Nothing
    'End Function

    Public Function Import_To_Tally(ByVal SaveDs_T As TallyDs) As Boolean

        Dim i As Integer = 1
        Dim Errors As Integer = 0
        Dim Created As Integer = 0
        Dim Altered As Integer = 0
        Dim ErrorList As String = ""
        Dim CurrentDate As Date
        Dim AlreadyExist_Int As Integer = 0
        Dim AlreadyExist_String As String = ""
        LblStatusImport.Visible = True
        Dim Responsestr As String = ""
        Dim XMLDOM As New MSXML2.DOMDocument30
        Dim ObjXml As New MSXML2.ServerXMLHTTP
        Dim StrXmldata As String = ""
        Dim Export_Data_Status As Boolean = False
        Dim TpvDS As New TallyDs
        Dim XmlDS As New DataSet
        Dim err_str As String = ""
        Dim Stream As StringReader
        Dim Reader As XmlTextReader
        Dim TempInv As New TallyDs
        Import_Log = ""

        Dim obj As New CommonDA
        StockGroupsDS = Export_Stock_Groups_Xml()
        Dim totalCount As Integer = SaveDs_T.PurchaseVehicle.Rows.Count


        If SaveDs_T.PurchaseVehicle.Rows.Count > 0 Then

            ' ProgresBar(totalCount, BtnImport, PnlImport)
            VehMaster = obj.Get_vehile(0, Nothing)
            err_str = Check_VehGrp_not_exist()

            If err_str <> "" Then
                Show_ClipBoard("Missing Group ", err_str)
                If MsgBox("Do you want to skip these bills and continue ?", MsgBoxStyle.YesNo) = Windows.Forms.DialogResult.No Then
                    Return False
                End If
            End If

            Dim dr As TallyDs.PurchaseVehicleRow

            For Each dr In SaveDs_T.PurchaseVehicle.Rows

                If dr.Invoice_Number = "" Then
                    Continue For
                End If

                TpvDS = New TallyDs
                TpvDS.Merge(SaveDs_T.PurchaseVehicle_Detail.Select("Invoice_No='" & dr.Invoice_No & "'"))

                'InvDate = dr.Invoice_Date '

                If CurrentDate.Date <> dr.Document_Date.Date Then
                    CurrentDate = dr.Document_Date.Date 'dr.Invoice_Date
                    Export_Data_Status = Export_Xml(Format(CurrentDate.Date, "yyyyMMdd"), Format(CurrentDate.Date, "yyyyMMdd"))
                    If Existing_Entry_Checked = False Then 'And TxtPw.Visible = False Then
                        If MsgBox("Warnings : Failed to check existing entries..." & vbCrLf &
                                                "Do you want to continue?") = Windows.Forms.DialogResult.No Then
                            Return False
                            Exit Function
                        End If
                    End If
                End If

                If Is_Already_Exist(dr.Invoice_No) Then

                    AlreadyExist_Int += 1
                    AlreadyExist_String += dr.Invoice_No & " , "
                    CommonDA.Update_Purchase(dr)
                    'Skip
                Else
                    'Do
                    Try
                        ObjXml.open("POST", TallyHost, False)
                        TempInv = New TallyDs

                        TempInv.Merge(InvDS.Invoice_no.Select("Invoice_No='" & dr.Invoice_No & "'"))
                        If TempInv.Invoice_no.Rows.Count > 0 Then
                            'MDI.ShowClipBoard(StrXmldata)
                            StrXmldata = ""
                            '  StrXmldata = XmlFormat_purchase_Vehicle_With_Stock(dr, TpvDS)
                        Else

                            StrXmldata = XmlFormat_purchase_Vehicle_With_Stock(dr, TpvDS)
                        End If

                        If Read_Settings("ShowRequestXML_YN") = "Y" Then
                            Show_ClipBoard("", StrXmldata)
                        End If


                        If StrXmldata = "" Then
                            Import_Log += vbCrLf & dr.Invoice_No & " - Unable to create tally request.."
                        Else

                            Try
                                ObjXml.send(StrXmldata)
                                Responsestr = ObjXml.responseText
                            Catch ex As Exception
                                Import_Log += vbCrLf & ex.Message
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


                                    If XmlDS.Tables("response").Rows(0).Item(0).ToString.Contains("already exists") Then
                                        Import_Log += dr.Invoice_Number & " - " &
                                           "Created :" & XmlDS.Tables("response").Rows(0).Item("CREATED").ToString &
                                           ",Skipped :" & XmlDS.Tables("response").Rows(0).Item("ERRORS").ToString &
                                            ",Altered :" & XmlDS.Tables("response").Rows(0).Item("ALTERED").ToString
                                        CommonDA.Update_Purchase(dr)
                                        AlreadyExist_Int += 1
                                        Errors -= 1

                                    Else


                                        Import_Log += dr.Invoice_Number & " - " &
                                            "Created :" & XmlDS.Tables("response").Rows(0).Item("CREATED").ToString &
                                            ",Altered :" & XmlDS.Tables("response").Rows(0).Item("ALTERED").ToString &
                                            ",Errors :" & XmlDS.Tables("response").Rows(0).Item("ERRORS").ToString

                                    End If


                                    'Import_Log += dr.Invoice_No & " - " &
                                    '            "Created :" & XmlDS.Tables("response").Rows(0).Item("CREATED").ToString &
                                    '            ",Altered :" & XmlDS.Tables("response").Rows(0).Item("ALTERED").ToString &
                                    '            ",Errors :" & XmlDS.Tables("response").Rows(0).Item("ERRORS").ToString

                                    If Val(XmlDS.Tables("response").Rows(0).Item("ERRORS").ToString) > 0 Then
                                        Import_Log += " ," & XmlDS.Tables("response").Rows(0).Item("LINEERROR").ToString
                                        ErrorList += XmlDS.Tables("response").Rows(0).Item("LINEERROR").ToString
                                    End If

                                    If Val(XmlDS.Tables("response").Rows(0).Item("CREATED").ToString) > 0 Then
                                        CommonDA.Update_Purchase(dr)
                                    End If

                                ElseIf XmlDS.Tables.Contains("LINEERROR") Then

                                    Import_Log += dr.Invoice_No & " - " &
                                                    XmlDS.Tables("LINEERROR").Rows(0).Item("LINEERROR_Text").ToString
                                    ErrorList += dr.Invoice_No & " - " &
                                                    XmlDS.Tables("LINEERROR").Rows(0).Item("LINEERROR_Text").ToString
                                    Errors += 1

                                End If

                                Import_Log += vbCrLf
                            Else
                                Import_Log += vbCrLf & dr.Invoice_No & " - Tally not responding..."
                                Errors += 1
                                ErrorList += "Tally not responding..." & vbCrLf
                            End If

                        End If

                    Catch ex As Exception
                        Import_Log += vbCrLf & dr.Invoice_No & " -Error- " & ex.Message
                        If XmlDS.Tables.Count > 0 Then
                            ErrorList += XmlDS.Tables(0).Rows(0).Item(0).ToString & vbCrLf
                        End If
                        Errors += 1
                    End Try
                End If

                LblStatusImport.Text = "Import Status = " & i & "/" & ImportDs.PurchaseVehicle.Rows.Count & vbCrLf &
        "Created : " & Created & " , Altered : " & Altered & " , Skipped : " & AlreadyExist_Int & " , Errors : " & Errors
                LblStatusImport.Refresh()
                i += 1
            Next

            If AlreadyExist_String <> "" Then
                Show_ClipBoard("Skipped entries", "Skipped entries ( " & AlreadyExist_Int & " ) : " & vbCrLf & AlreadyExist_String)
            End If
            If ErrorList = "" Then

                Show_ClipBoard("Completed successfully.", "" & vbCrLf & LblStatusImport.Text)
                CommonDA.Create_Log("Import to Tally", "Import", LblStatusImport.Text & vbCrLf & Import_Log)
                'If Created > 0 Then
                '    Try
                '        IO.File.Move(TxtPath.Text, Application.StartupPath & "\BackUp\Purch_Vehcle_" & Format(Date.Now, "ddMMMyy_HHmmss") & ".xls")
                '    Catch ex As Exception
                '        Common.Create_Log("Import to Tally", "Failed to move " & TxtPath.Text, ErrorList)
                '    End Try
                'End If
                'TxtPath.Text = ""
                'LblFileInfo.Text = "File Info :"
                LblStatusImport.Text = "Import Status :"
            Else
                Show_ClipBoard("Error List", ErrorList)
                CommonDA.Create_Log("Import to Tally", "Import-Error", ErrorList)
            End If
            ImportDs.Clear()
            ImportDs = CommonDA.Get_Summary_purchasevehicle
        End If

        Return True

    End Function


    Public Function Check_VehGrp_not_exist() As String

        Dim err_str As String = ""
        Dim DsDetails As New TallyDs
        Dim dr As TallyDs.PurchaseVehicleRow
        Dim Not_exist As Boolean = False

        InvDS = New TallyDs


        If StockGroupsDS.Tables.Contains("STOCKGROUP") Then

            For Each dr In ImportDs.PurchaseVehicle.Rows

                Not_exist = False
                DsDetails.Invoice_no.Clear()
                DsDetails.Merge(ImportDs.Invoice_no.Select("`Invoice_Number` = '" & dr.Invoice_Number & "'", ""))
                For Each drt In DsDetails.Invoice_no.Rows
                    If StockGroupsDS.Tables(0).Select("[NAME]='" & drt.Description & "'").Count = 0 Then
                        '    err_str = drt.Model & vbCrLf
                        err_str = IIf(err_str = "", vbCrLf & "BILL:" & dr.Invoice_Number, err_str)
                        err_str += vbCrLf & drt.Description.ToString.Trim
                        Not_exist = True
                    End If
                Next

                If Not_exist = True Then
                    Dim R As DataRow = InvDS.Invoice_no.NewRow
                    R("Invoice_No") = dr.Invoice_No
                    InvDS.Invoice_no.Rows.Add(R)
                End If

            Next

        End If

        Return err_str


    End Function


    Public Function Check_Vehicle_Groups(ByVal dt As TallyDs.PurchaseVehicleDataTable) As Boolean

        Dim Status As Boolean = True
        Dim SubGroup As String = ""

        For Each Dr As TallyDs.PurchaseVehicleRow In dt.Rows
            SubGroup = Dr.Description
            Dr.GroupOk = 0
            Dr.PartyLedgerVehicle = Read_Ledgers("Purchase_PartyLedger_Vehicle")
            Dr.LedgerVehicle = Read_Ledgers("LedgerVehicle")
            Dr.Vehicle_VoucherType = Read_Ledgers("Purchase_VT_Vehicle")
            If StockGroupsDS.Tables.Count > 0 Then
                For Each DrStock As DataRow In StockGroupsDS.Tables("STOCKGROUP").Rows
                    If SubGroup.Contains(DrStock("NAME")) Then
                        Dr.StockGroupVehMain = DrStock("PARENT")
                        Dr.StockGroupVehSub = DrStock("NAME")
                        Dr.GroupOk = 1
                    End If
                Next
            End If
        Next

        If dt.Select("GroupOk = 0", "").Count > 0 Then
            MsgBox("Stock Group not found for " & vbCrLf & dt.Select("GroupOk = 0", "").First.Item("Description").ToString)
            Status = False
        End If

        Return Status

    End Function

    Public Function Is_Already_Exist(ByVal VoucherNo As String) As Boolean
        Dim Value As String = ""
        Dim Status As Boolean = False
        Dim Vehicle_VoucherType As String = Read_Settings("Purchase_VT_Vehicle")
        If PublicTallyDS.Tables.Count > 0 Then
            If PublicTallyDS.Tables("VOUCHER").Rows.Count > 0 Then
                Try
                    Value = PublicTallyDS.Tables("VOUCHER").Select("REFERENCE = '" & VoucherNo & "'" &
                                                              "And VOUCHERTYPENAME = '" & Vehicle_VoucherType & "'").First.Item("REFERENCE").ToString
                    Status = True
                Catch ex As Exception
                End Try
            End If
        End If
        Return Status
    End Function

    Public Function Export_Xml(ByVal FromDate As String, ByVal ToDate As String) As Boolean

        Dim Ds As New DataSet
        Dim i As Integer = 1
        Dim XmlDS As New DataSet
        Dim StrXmldata As String
        Existing_Entry_Checked = False
        Dim Voucher_Type = Read_Ledgers("Purchase_VT_Vehicle").Replace("&", "&amp;")
        Try

            StrXmldata = XmlFormat_Voucher_Register(FromDate, ToDate, Voucher_Type)
            XmlDS = Export_Tally_Method1(StrXmldata)
            If XmlDS.Tables.Count = 0 Then
                XmlDS = Export_Tally_Method2(StrXmldata)
            End If

            If Not XmlDS Is Nothing Then
                If XmlDS.Tables.Contains("VOUCHER") Then
                    PublicTallyDS.Merge(XmlDS.Tables("VOUCHER"))
                    Existing_Entry_Checked = True
                ElseIf XmlDS.Tables.Count >= 4 Then
                    Existing_Entry_Checked = True
                End If
            End If

        Catch ex As Exception
            ' MsgBox("Failed to check existing entries in tally...")
            CommonDA.Create_Log("Import to Tally", "Import-Error", "Failed to check existing entries in tally...")
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

    Public Function Export_Stock_Groups_Xml() As DataSet

        Dim Ds As New DataSet
        Dim i As Integer = 1

        Dim Responsestr As String
        Dim XMLDOM As New MSXML2.DOMDocument30
        Dim ObjXml As New MSXML2.ServerXMLHTTP
        Dim StrXmldata As String = ""

        Dim XmlDS As New DataSet
        Dim Stream As StringReader
        Dim Reader As XmlTextReader

        Dim StockGroupVehMain As String = Read_Ledgers("Stock_Parent_Vehicle")

        Try

            ObjXml.open("POST", TallyHost, False)
            StrXmldata = XmlExport_Stock_Groups("Stock Groups")
            ObjXml.send(StrXmldata)
            Responsestr = ObjXml.responseText

            If Responsestr <> Nothing Then
                XMLDOM.loadXML(Responsestr)
                Stream = New StringReader(Responsestr)
                Reader = New XmlTextReader(Stream)
                XmlDS.Clear()
                XmlDS.ReadXml(Reader)

                If XmlDS.Tables.Contains("STOCKGROUP") Then
                    Ds.Merge(XmlDS.Tables("STOCKGROUP").Select("[PARENT] = '" & StockGroupVehMain & "'", ""))
                End If
            End If

        Catch ex As Exception
            ' MsgBox("Failed to check existing entries in tally...")
            CommonDA.Create_Log("Export StockGroup", "Import-Error", ex.Message)
        End Try

        Return Ds

    End Function

    Public Function XmlExport_Stock_Groups(ByVal RptType As String) As String

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
                   " <REPORTNAME>List of Accounts</REPORTNAME>" &
                   " <STATICVARIABLES>" &
                   " <SVEXPORTFORMAT>$$SysName:XML</SVEXPORTFORMAT>" &
                   " <ACCOUNTTYPE>" & RptType & "</ACCOUNTTYPE>" &
                   " </STATICVARIABLES>" &
                   " </REQUESTDESC>" &
                   " </EXPORTDATA>" &
                   " </BODY>" &
                   " </ENVELOPE>"


        Catch ex As Exception
            MsgBox(ex.Message)
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

    Public Function XmlFormat_Stock_Groups(ByVal dr As DataRow) As String

        'PARENT,SUBGROUP

        Dim Xmlstring As String = ""

        Try

            Xmlstring = "<ENVELOPE>" &
                            "<HEADER>" &
                                "<TALLYREQUEST>Import Data</TALLYREQUEST>" &
                            "</HEADER>" &
                            "<BODY>" &
                                "<IMPORTDATA>" &
                                    "<REQUESTDESC>" &
                                        "<REPORTNAME>All Masters</REPORTNAME>" &
                                    "</REQUESTDESC>" &
                                    "<REQUESTDATA>" &
                                        "<TALLYMESSAGE xmlns:UDF=""TallyUDF"">" &
                                            "<STOCKGROUP NAME=""" & dr("SUBGROUP") & """ ACTION=""Create"">" &
                                                "<NAME>" & dr("SUBGROUP") & "</NAME>" &
                                                "<PARENT>" & dr("PARENT") & "</PARENT>" &
                                                "<ISADDABLE>Yes</ISADDABLE>" &
                                            "</STOCKGROUP>" &
                                        "</TALLYMESSAGE>" &
                                    "</REQUESTDATA>" &
                                "</IMPORTDATA>" &
                            "</BODY>" &
                        "</ENVELOPE>"

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Return Xmlstring


    End Function

    Public Function XmlFormat_Items(ByVal dr As DataRow) As String

        'PARTNO,PARTNAME,OLDPRTNO,LC,LCNAME,COLOR,MOQ,MRP,NDP,PARENT

        Dim Xmlstring As String = ""

        Try

            Xmlstring = "<ENVELOPE>" &
                            "<HEADER>" &
                                "<TALLYREQUEST>Import Data</TALLYREQUEST>" &
                            "</HEADER>" &
                            "<BODY>" &
                                "<IMPORTDATA>" &
                                    "<REQUESTDESC>" &
                                        "<REPORTNAME>All Masters</REPORTNAME>" &
                                    "</REQUESTDESC>" &
                                    "<REQUESTDATA>" &
                                        "<TALLYMESSAGE xmlns:UDF=""TallyUDF"">" &
                                            "<STOCKITEM NAME=""" & dr("PARTNAME") & """ ACTION=""Create"">" &
                                                "<NAME>" & dr("PARTNAME") & "</NAME>" &
                                                "<PARENT>" & dr("PARENT") & "</PARENT>" &
                                                "<BASEUNITS>Nos</BASEUNITS>" &
                                                "<ADDITIONALNAME>" & dr("PARTNO") & "</ADDITIONALNAME>" &
                                                "<STANDARDPRICELIST.LIST>" &
                                                    "<RATE>" & dr("MRP") & "</RATE>" &
                                                "</STANDARDPRICELIST.LIST>" &
                                                "<STANDARDCOSTLIST.LIST>" &
                                                    "<RATE>" & dr("NDP") & "</RATE>" &
                                                "</STANDARDCOSTLIST.LIST>" &
                                                "<OPENINGBALANCE>0.000 NOS</OPENINGBALANCE>" &
                                                "<OPENINGVALUE>0.000</OPENINGVALUE>" &
                                                "<OPENINGRATE>0.000/NOS</OPENINGRATE>" &
                                            "</STOCKITEM>" &
                                        "</TALLYMESSAGE>" &
                                    "</REQUESTDATA>" &
                                "</IMPORTDATA>" &
                            "</BODY>" &
                        "</ENVELOPE>"

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Return Xmlstring


    End Function

    Public Function XmlFormat_purchase(ByVal dr As DataRow) As String

        'INVDATE,INVNO,PARTNO,PARTNOORDERD,PARTNAME,ORDERNO,QTY,UNITRATE,NDPAMT,UNITMRP,MRPAMT,CASENO,PARTYNAME,LEDGERNAME

        Dim Xml As String = ""
        Dim InvDate As Date = dr("INVDATE")
        Dim InvDateString As String = Format(InvDate, "yyyyMMdd")

        Try
            Xml = "<ENVELOPE>" &
                      "<HEADER>" &
                          "<TALLYREQUEST>Import Data</TALLYREQUEST>" &
                      "</HEADER>" &
                      "<BODY>" &
                          "<IMPORTDATA>" &
                              "<REQUESTDESC>" &
                                  "<REPORTNAME>All Masters</REPORTNAME>" &
                              "</REQUESTDESC>" &
                              "<REQUESTDATA>" &
                                "<TALLYMESSAGE xmlns:UDF=""TallyUDF"">" &
                                      "<VOUCHER>" &
                                          "<VOUCHERTYPENAME>Purchase Interstate Spare</VOUCHERTYPENAME>" &
                                          "<DATE>" & InvDateString & "</DATE>" &
                                          "<EFFECTIVEDATE>" & InvDateString & "</EFFECTIVEDATE>" &
                                          "<ISCANCELLED>No</ISCANCELLED>" &
                                          "<USETRACKINGNUMBER>No</USETRACKINGNUMBER>" &
                                          "<ISPOSTDATED>No</ISPOSTDATED>" &
                                          "<ISINVOICE>Yes</ISINVOICE>" &
                                          "<REFERENCE>" & dr("INVNO") & "</REFERENCE>" &
                                          "<NARRATION>" & dr("CASENO") & "</NARRATION>" &
                                          "<DIFFACTUALQTY>No</DIFFACTUALQTY>" &
                                          "<VOUCHERNUMBER>1</VOUCHERNUMBER>" &
                                          "<PARTYNAME>" & dr("PARTYNAME") & "</PARTYNAME>" &
                                          "<PARTYLEDGERNAME>" & dr("PARTYNAME") & "</PARTYLEDGERNAME>" &
                                          "<ASORIGINAL>Yes</ASORIGINAL>" &
                                          "<CSTFORMISSUETYPE>C Form</CSTFORMISSUETYPE>" &
                                          "<BASICBUYERSSALESTAXNO></BASICBUYERSSALESTAXNO>" &
                                          "<ALLLEDGERENTRIES.LIST>" &
                                          "</ALLLEDGERENTRIES.LIST>" &
                                          "<ALLLEDGERENTRIES.LIST>" &
                                          "</ALLLEDGERENTRIES.LIST>" &
                                          "<LEDGERENTRIES.LIST>" &
                                              "<LEDGERNAME>" & dr("PARTYNAME") & "</LEDGERNAME>" &
                                              "<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>" &
                                              "<ISPARTYLEDGER>Yes</ISPARTYLEDGER>" &
                                              "<AMOUNT>" & dr("NDPAMT") & "</AMOUNT>" &
                                          "</LEDGERENTRIES.LIST>" &
                                          "<ALLINVENTORYENTRIES.LIST>" &
                                              "<STOCKITEMNAME>" & dr("PARTNAME") & "</STOCKITEMNAME>" &
                                              "<ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" &
                                              "<RATE>" & dr("UNITRATE") & "/Nos</RATE>" &
                                              "<AMOUNT>-" & dr("NDPAMT") & "</AMOUNT>" &
                                              "<ACTUALQTY>" & dr("QTY") & " Nos</ACTUALQTY>" &
                                              "<BILLEDQTY>" & dr("QTY") & " Nos</BILLEDQTY>" &
                                              "<ACCOUNTINGALLOCATIONS.LIST>" &
                                                  "<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>" &
                                                  "<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>" &
                                                  "<LEDGERFROMITEM>No</LEDGERFROMITEM>" &
                                                  "<TAXCLASSIFICATIONNAME>Interstate Purchases @ 2% Against Form C</TAXCLASSIFICATIONNAME>" &
                                                  "<LEDGERNAME>" & dr("LEDGERNAME") & "</LEDGERNAME>" &
                                                  "<AMOUNT>" & dr("NDPAMT") & "</AMOUNT>" &
                                              "</ACCOUNTINGALLOCATIONS.LIST>" &
                                              "<BATCHALLOCATIONS.LIST>" &
                                                  "<GODOWNNAME>Main Location</GODOWNNAME>" &
                                                  "<BATCHNAME>Primary Batch</BATCHNAME>" &
                                                  "<AMOUNT>-" & dr("NDPAMT") & "</AMOUNT>" &
                                                  "<ACTUALQTY>" & dr("QTY") & " Nos</ACTUALQTY>" &
                                                  "<BILLEDQTY>" & dr("QTY") & " Nos</BILLEDQTY>" &
                                              "</BATCHALLOCATIONS.LIST>" &
                                          "</ALLINVENTORYENTRIES.LIST>" &
                                      "</VOUCHER>" &
                                  "</TALLYMESSAGE>" &
                              "</REQUESTDATA>" &
                          "</IMPORTDATA>" &
                      "</BODY>" &
                  "</ENVELOPE>"


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Return Xml

    End Function


    Public Function XmlFormat_purchase_With_Alter_StockItem(ByVal dr As DataRow) As String

        'INVDATE,INVNO,PARTNO,PARTNOORDERD,PARTNAME,ORDERNO,QTY,UNITRATE,NDPAMT,UNITMRP,MRPAMT,CASENO,PARTYNAME,LEDGERNAME

        Dim Xml As String = ""
        Dim InvDate As Date = dr("INVDATE")
        Dim InvDateString As String = Format(InvDate, "yyyyMMdd")

        Try
            Xml = "<ENVELOPE>" &
                      "<HEADER>" &
                          "<TALLYREQUEST>Import Data</TALLYREQUEST>" &
                      "</HEADER>" &
                      "<BODY>" &
                          "<IMPORTDATA>" &
                              "<REQUESTDESC>" &
                                  "<REPORTNAME>All Masters</REPORTNAME>" &
                              "</REQUESTDESC>" &
                              "<REQUESTDATA>" &
                                "<TALLYMESSAGE xmlns:UDF=""TallyUDF"">" &
                                  "<STOCKITEM NAME=""" & dr("PARTNAME") & """ ACTION=""Create"">" &
                                      "<NAME>" & dr("PARTNAME") & "</NAME>" &
                                      "<PARENT>SPARE</PARENT>" &
                                      "<BASEUNITS>Nos</BASEUNITS>" &
                                      "<OPENINGBALANCE>0.000 NOS</OPENINGBALANCE>" &
                                      "<OPENINGVALUE>0.000</OPENINGVALUE>" &
                                      "<OPENINGRATE>0.000/NOS</OPENINGRATE>" &
                                      "<STANDARDPRICELIST.LIST>" &
                                          "<RATE>" & dr("UNITMRP") & "</RATE>" &
                                          "<DATE>20150101</DATE>" &
                                      "</STANDARDPRICELIST.LIST>" &
                                      "<STANDARDCOSTLIST.LIST>" &
                                          "<RATE>" & dr("UNITMRP") & "</RATE>" &
                                          "<DATE>20150101</DATE>" &
                                      "</STANDARDCOSTLIST.LIST>" &
                                      "<REORDERBASE>0.000</REORDERBASE>" &
                                      "<MINIMUMORDERBASE>0.000</MINIMUMORDERBASE>" &
                                  "</STOCKITEM>" &
                                "</TALLYMESSAGE>" &
                                "<TALLYMESSAGE xmlns:UDF=""TallyUDF"">" &
                                        "<VOUCHER>" &
                                          "<VOUCHERTYPENAME>Purchase Interstate Spare</VOUCHERTYPENAME>" &
                                          "<DATE>" & InvDateString & "</DATE>" &
                                          "<EFFECTIVEDATE>" & InvDateString & "</EFFECTIVEDATE>" &
                                          "<ISCANCELLED>No</ISCANCELLED>" &
                                          "<USETRACKINGNUMBER>No</USETRACKINGNUMBER>" &
                                          "<ISPOSTDATED>No</ISPOSTDATED>" &
                                          "<ISINVOICE>Yes</ISINVOICE>" &
                                          "<REFERENCE>" & dr("INVNO") & "</REFERENCE>" &
                                          "<NARRATION>" & dr("CASENO") & "</NARRATION>" &
                                          "<DIFFACTUALQTY>No</DIFFACTUALQTY>" &
                                          "<VOUCHERNUMBER>1</VOUCHERNUMBER>" &
                                          "<PARTYNAME>" & dr("PARTYNAME") & "</PARTYNAME>" &
                                          "<PARTYLEDGERNAME>" & dr("PARTYNAME") & "</PARTYLEDGERNAME>" &
                                          "<ASORIGINAL>Yes</ASORIGINAL>" &
                                          "<CSTFORMISSUETYPE>C Form</CSTFORMISSUETYPE>" &
                                          "<BASICBUYERSSALESTAXNO></BASICBUYERSSALESTAXNO>" &
                                          "<ALLLEDGERENTRIES.LIST>" &
                                          "</ALLLEDGERENTRIES.LIST>" &
                                          "<ALLLEDGERENTRIES.LIST>" &
                                          "</ALLLEDGERENTRIES.LIST>" &
                                          "<LEDGERENTRIES.LIST>" &
                                              "<LEDGERNAME>" & dr("PARTYNAME") & "</LEDGERNAME>" &
                                              "<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>" &
                                              "<ISPARTYLEDGER>Yes</ISPARTYLEDGER>" &
                                              "<AMOUNT>" & dr("NDPAMT") & "</AMOUNT>" &
                                          "</LEDGERENTRIES.LIST>" &
                                          "<ALLINVENTORYENTRIES.LIST>" &
                                              "<STOCKITEMNAME>" & dr("PARTNAME") & "</STOCKITEMNAME>" &
                                              "<ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" &
                                              "<RATE>" & dr("UNITRATE") & "/Nos</RATE>" &
                                              "<AMOUNT>-" & dr("NDPAMT") & "</AMOUNT>" &
                                              "<ACTUALQTY>" & dr("QTY") & " Nos</ACTUALQTY>" &
                                              "<BILLEDQTY>" & dr("QTY") & " Nos</BILLEDQTY>" &
                                              "<ACCOUNTINGALLOCATIONS.LIST>" &
                                                  "<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>" &
                                                  "<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>" &
                                                  "<LEDGERFROMITEM>No</LEDGERFROMITEM>" &
                                                  "<TAXCLASSIFICATIONNAME>Interstate Purchases @ 2% Against Form C</TAXCLASSIFICATIONNAME>" &
                                                  "<LEDGERNAME>" & dr("LEDGERNAME") & "</LEDGERNAME>" &
                                                  "<AMOUNT>" & dr("NDPAMT") & "</AMOUNT>" &
                                              "</ACCOUNTINGALLOCATIONS.LIST>" &
                                              "<BATCHALLOCATIONS.LIST>" &
                                                  "<GODOWNNAME>Main Location</GODOWNNAME>" &
                                                  "<BATCHNAME>Primary Batch</BATCHNAME>" &
                                                  "<AMOUNT>-" & dr("NDPAMT") & "</AMOUNT>" &
                                                  "<ACTUALQTY>" & dr("QTY") & " Nos</ACTUALQTY>" &
                                                  "<BILLEDQTY>" & dr("QTY") & " Nos</BILLEDQTY>" &
                                              "</BATCHALLOCATIONS.LIST>" &
                                          "</ALLINVENTORYENTRIES.LIST>" &
                                      "</VOUCHER>" &
                                  "</TALLYMESSAGE>" &
                              "</REQUESTDATA>" &
                          "</IMPORTDATA>" &
                      "</BODY>" &
                  "</ENVELOPE>"


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try


        Return Xml

    End Function

    Public Function XmlFormat_purchase_Vehicle_With_Stock(ByVal dr As TallyDs.PurchaseVehicleRow, ByVal SaveDs_T As TallyDs) As String

        'Delivery_No,Depot,Invoice_No,Invoice_Date,Document_No,Document_Date,Total_NDP,
        'Total_Amount, Slno, Model_Code, Description, MFG_Date, Chassis_No, Engine_No, NDP, Amount

        Dim Xml As String = ""
        Dim Xml_Stock_Items As String = ""
        Dim Xml_Invn_Items As String = ""
        Dim Xml_AccAll_Items As String = ""
        Dim Xml_BatchAll_Items As String = ""
        Dim Veh_HSN_Code As String = Read_Ledgers("Veh_HSN_Code")
        Dim drd As TallyDs.PurchaseVehicle_DetailRow
        Dim InvDate As Date = dr.Invoice_Date  '
        Dim DocDate As Date = dr.Document_Date
        Dim DocDateString As String = Format(DocDate, "yyyyMMdd")
        Dim InvDateString As String = Format(InvDate, "yyyyMMdd")
        Dim Narration As String = ""
        Dim InputTaxPaid As String = Read_Ledgers("InputTaxPaid")
        '  Dim PaiseRoundingOff As String = Read_Settings("RoundOff")
        Dim PaiseRoundingOff_Amount As Double
        Dim StateCess As String = Read_Ledgers("State_Cess_Per")

        Try


            dr.PartyLedgerVehicle = Read_Ledgers("Purchase_PartyLedger_Vehicle").Replace("&", "&amp;")
            dr.StockGroupVehSub = Read_Ledgers("Stock_Parent_Vehicle").Replace("&", "&amp;")
            dr.Vehicle_VoucherType = Read_Ledgers("Purchase_VT_Vehicle").Replace("&", "&amp;")

            Narration = "INV.NO : " & dr.Invoice_Number & "   Inv Date :" & dr.Document_Date & "  " &
                "Sup  Date : " & dr.Invoice_Date & " Sup Inv No: " & dr.Invoice_No
            Dim a As Double = 0.0
            For Each drd In SaveDs_T.PurchaseVehicle_Detail.Rows

                'Narration = "INV.NO : " & SaveDs_T.PurchaseVehicle.Rows(0).Item("RE Invoice No").ToString

                'Narration += IIf(Narration <> "", "," & drd.Description, drd.ModelFamily)

                '"<TAXCLASSIFICATIONNAME>Interstate Purchases @ 2% Against Form C</TAXCLASSIFICATIONNAME>" & _

                'If drd.CESS > 0 Then
                '    dr.LedgerVehicle = Read_Settings("LedgerVehicle_Pur_CESS").Replace("&", "&amp;")
                'Else
                'End If

                '  dr.LedgerVehicle = Read_Settings("LedgerVehicle_Pur").Replace("&", "&amp;")

                If dr.CESS > 0 Then
                    dr.LedgerVehicle = Read_Ledgers("Vehicle_Ledger_Purchase_Cess").Replace("&", "&amp;")
                Else
                    dr.LedgerVehicle = Read_Ledgers("Vehicle_Ledger_Purchase").Replace("&", "&amp;")
                End If

                Try
                    drd.HSN_Code = VehMaster.veh_master.Select("ModelFamily='" & drd.Description & "'").First.Item("HSN_Code").ToString

                Catch ex As Exception

                    If MsgBox("HSN Code Not Found" & vbCrLf & "Do you Want to continue by Entering HSN Code for this Item", vbYesNo, "HSN Code Missing") = vbYes Then

                        drd.HSN_Code = InputBox("Enter HSN Code For " & drd.Description).ToString
                        Narration += ":,:"

                    Else
                        Return ""
                    End If

                End Try


                Xml_AccAll_Items = "<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>" &
                                    "<ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" &
                                    "<LEDGERFROMITEM>No</LEDGERFROMITEM>" &
                                    "<LEDGERNAME>" & dr.LedgerVehicle & "</LEDGERNAME>" &
                                     Create_XML_COSTCENTER(Read_Ledgers("Cost_Center_Vehicle"), drd.Rate) &
                                    "<AMOUNT>-" & drd.Rate & "</AMOUNT>"

                Xml_BatchAll_Items = "<GODOWNNAME>Main Location</GODOWNNAME>" &
                                                 "<BATCHNAME>Primary Batch</BATCHNAME>" &
                                                 "<AMOUNT>" & drd.Rate & "</AMOUNT>" &
                                                 "<ACTUALQTY>" & drd.qty & " Nos</ACTUALQTY>" &
                                                 "<BILLEDQTY>" & drd.qty & " Nos</BILLEDQTY>"

                'Stk_ItemName = DrT.Vehicle_Name.Replace("&", "&amp;") & " " & Strings.Right(DrT.Chs_No, 10)
                'Qty_Type = "Nos."

                'StockSpareParent_To_Create = DrT.Vehicle_Name.Replace("&", "&amp;")

                '                Xml_StockItem = Xml_StockItem & "<TALLYMESSAGE xmlns:UDF=""TallyUDF"">" & _
                '                                      "<STOCKITEM NAME=""" & Stk_ItemName.Replace("&", "&amp;") & """ ACTION=""Create"">" & _
                '                                          "<NAME>" & Stk_ItemName.Replace("&", "&amp;") & "</NAME>" & _
                '                                          "<ADDITIONALNAME>" & DrT.Chs_No & "</ADDITIONALNAME>" & _
                '"<LANGUAGENAME.LIST>" & _
                '                                          "<NAME.LIST TYPE='String'> " & _
                '                                          "<NAME>" & Stk_ItemName.Replace("&", "&amp;") & "</NAME>" & _
                '                                          "<NAME>" & DrT.Eng_No & "</NAME>" & _
                '                                          "</NAME.LIST>" & _
                '"</LANGUAGENAME.LIST>" & _
                '                                          "<BASEUNITS>" & Qty_Type & "</BASEUNITS>" & _
                '                                        "<PARENT>" & StockSpareParent_To_Create & "</PARENT>" & _
                '                                        Xml_GST_Item(DrT.GST_Perc, 0, DrT.HSN_Code, DrT.HSN_Code) &
                '                                      "</STOCKITEM>" & _
                '                                "</TALLYMESSAGE>"


                Xml_Stock_Items += "<STOCKITEM NAME=""" & drd.Description.Replace("&", "&amp;") & " " & drd.Chassis_No & """ ACTION=""Create"">" &
                                     "<NAME>" & drd.Description.Replace("&", "&amp;") & " " & drd.Chassis_No & "</NAME>" &
                                     "<ADDITIONALNAME>" & dr.Engine_No & "</ADDITIONALNAME>" &
                                     "<LANGUAGENAME.LIST>" &
                                          "<NAME.LIST TYPE='String'> " &
                                          "<NAME>" & drd.Description.Replace("&", "&amp;") & " " & drd.Chassis_No & "</NAME>" &
                                          "<NAME>" & drd.Engine_No & "</NAME>" &
                                          "</NAME.LIST>" &
                                "</LANGUAGENAME.LIST>" &
                                "<PARENT> " & drd.Description.Replace("&", "&amp;") & " </PARENT>" &
                                     "<BASEUNITS>Nos</BASEUNITS>" &
                                     "<DESCRIPTION>" & drd.Engine_No & "</DESCRIPTION>" &
                                        Xml_GST_Item(drd.SGST_Per + drd.CGST_Per, drd.CESS_Per, drd.HSN_Code, drd.HSN_Code, StateCess) &
                                 "</STOCKITEM>"

                Xml_Invn_Items += "<ALLINVENTORYENTRIES.LIST>" &
                                            "<STOCKITEMNAME>" & drd.Description.Replace("&", "&amp;") & " " & drd.Chassis_No & "</STOCKITEMNAME>" &
                                             "<ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>" &
                                             "<RATE>" & drd.Rate & "/Nos</RATE>" &
                                             "<AMOUNT>-" & drd.Rate & "</AMOUNT>" &
                                             "<ACTUALQTY>" & drd.qty & " Nos</ACTUALQTY>" &
                                             "<BILLEDQTY>" & drd.qty & " Nos</BILLEDQTY>" &
                                             "<ACCOUNTINGALLOCATIONS.LIST>" &
                                             Xml_AccAll_Items &
                                             "</ACCOUNTINGALLOCATIONS.LIST>" &
                                             "<BATCHALLOCATIONS.LIST>" &
                                             Xml_BatchAll_Items &
                                             "</BATCHALLOCATIONS.LIST>" &
                                   "</ALLINVENTORYENTRIES.LIST>"
                a += drd.Rate

            Next

            InvDateString = Format(InvDate, "yyyyMMdd")

            Dim Additional_Ledgers As String = ""

            If dr.CGST > 0 Then
                Additional_Ledgers += Create_XML_ALLLEDGERENTRIES(Read_Ledgers("Veh_Ledger_CGST"), -Val(dr("CGST")))
            End If

            If dr.SGST > 0 Then
                Additional_Ledgers += Create_XML_ALLLEDGERENTRIES(Read_Ledgers("Veh_Ledger_SGST"), -Val(dr("sGST")))
            End If

            If dr.CESS > 0 Then
                Additional_Ledgers += Create_XML_ALLLEDGERENTRIES(Read_Ledgers("Veh_Ledger_CESS"), -Val(dr("cess")))
            End If

            'If dr.Freight > 0 Then
            '    Additional_Ledgers += Create_XML_ALLLEDGERENTRIES(Read_Settings("Ledger_FRIEGHT"), -Val(dr("Freight")))
            'End If

            'dr.Total_Amount = 286159.36
            '  dr.Total_Amount = Val(dr.Total_Amount) + dr.CGST + dr.SGST + dr.CESS + dr.Freight


            'Dim Party_ledger_Amt As Decimal = Math.Round(Val(dr.Total_Amount))
            Dim Party_ledger_Amt As Decimal = Math.Round(Val(dr.Total_Amount))
            PaiseRoundingOff_Amount = Format(Party_ledger_Amt - Val(dr.Total_Amount), "0.00")

            If PaiseRoundingOff_Amount <> 0 Then
                Additional_Ledgers += Create_XML_ALLLEDGERENTRIES(Read_Ledgers("Ledger_PaiseRoundOff"), -PaiseRoundingOff_Amount)
                ' Additional_Ledgers += Create_XML_ALLLEDGERENTRIES(PaiseRoundingOff, PaiseRoundingOff_Amount)
            End If

            'If PaiseRoundingOff_Amount <> 0 Then
            '    Additional_Ledgers += Create_XML_LEDGERENTRIES(PaiseRoundingOff, PaiseRoundingOff_Amount, True)
            'End If

            '"<CSTFORMISSUETYPE>C Form</CSTFORMISSUETYPE>" & _

            '   dr.Invoice_No = dr("RE Invoice No")

            Xml = "<ENVELOPE>" &
                     "<HEADER>" &
                         "<TALLYREQUEST>Import Data</TALLYREQUEST>" &
                     "</HEADER>" &
                     "<BODY>" &
                         "<IMPORTDATA>" &
                             "<REQUESTDESC>" &
                               "<REPORTNAME>All Masters</REPORTNAME>" &
                               "</REQUESTDESC>" &
                               "<REQUESTDATA>" &
                               "<TALLYMESSAGE xmlns:UDF=""TallyUDF"">" &
                               Xml_Stock_Items &
                               "</TALLYMESSAGE>" &
                               "<TALLYMESSAGE xmlns:UDF=""TallyUDF"">" &
                                       "<VOUCHER>" &
                                         "<VOUCHERTYPENAME>" & dr.Vehicle_VoucherType & "</VOUCHERTYPENAME>" &
                                         "<DATE>" & DocDateString & "</DATE>" &
                                         "<EFFECTIVEDATE>" & DocDateString & "</EFFECTIVEDATE>" &
                                         "<ISCANCELLED>No</ISCANCELLED>" &
                                         "<USETRACKINGNUMBER>No</USETRACKINGNUMBER>" &
                                         "<ISPOSTDATED>No</ISPOSTDATED>" &
                                         "<ISINVOICE>Yes</ISINVOICE>" &
                                         "<REFERENCEDATE>" & InvDateString & "</REFERENCEDATE>" &
                                         "<REFERENCE>" & dr.Invoice_No & "</REFERENCE>" &
                                         "<NARRATION>" & Narration & "</NARRATION>" &
                                         "<DIFFACTUALQTY>No</DIFFACTUALQTY>" &
                                         "<VOUCHERNUMBER>" & dr.Invoice_Number & "</VOUCHERNUMBER>" &
                                         "<PARTYNAME>" & dr.PartyLedgerVehicle & "</PARTYNAME>" &
                                         "<PARTYLEDGERNAME>" & dr.PartyLedgerVehicle & "</PARTYLEDGERNAME>" &
                                         "<ASORIGINAL>No</ASORIGINAL>" &
                                         "<BASICBUYERSSALESTAXNO></BASICBUYERSSALESTAXNO>" &
                                         "<ALLLEDGERENTRIES.LIST>" &
                                         "</ALLLEDGERENTRIES.LIST>" &
                                          "<LEDGERENTRIES.LIST>" &
                                             "<LEDGERNAME>" & dr.PartyLedgerVehicle & "</LEDGERNAME>" &
                                             "<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>" &
                                             "<ISPARTYLEDGER>Yes</ISPARTYLEDGER>" &
                                             "<AMOUNT>" & Party_ledger_Amt & "</AMOUNT>" &
                                         "</LEDGERENTRIES.LIST>" &
                                         Additional_Ledgers &
                                         Xml_Invn_Items &
                                     "</VOUCHER>" &
                                 "</TALLYMESSAGE>" &
                             "</REQUESTDATA>" &
                         "</IMPORTDATA>" &
                     "</BODY>" &
                 "</ENVELOPE>"

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Return Xml

    End Function


    Private Function Create_XML_COSTCENTER(ByVal COSTCENTER As String, ByVal AMOUNT As String, Optional ByVal Ignore_ISDEEMEDPOSITIVE As Boolean = False) As String
        Dim Str As String = ""
        Dim CATEGORY = Read_Ledgers("CostCategory")
        Dim ISDEEMEDPOSITIVE As String = IIf(Val(AMOUNT) < 0, "Yes", "No")

        If Ignore_ISDEEMEDPOSITIVE Then
            ISDEEMEDPOSITIVE = "No"
        End If

        Str = "<CATEGORYALLOCATIONS.LIST>" &
              "<CATEGORY>" & CATEGORY & "</CATEGORY>" &
              "<ISDEEMEDPOSITIVE>" & ISDEEMEDPOSITIVE & "</ISDEEMEDPOSITIVE> " &
              "<COSTCENTREALLOCATIONS.LIST>" &
              "<NAME>" & COSTCENTER & "</NAME>" &
              "<AMOUNT>-" & AMOUNT & "</AMOUNT>" &
              "</COSTCENTREALLOCATIONS.LIST>" &
              "</CATEGORYALLOCATIONS.LIST>"
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


    Private Function Xml_GST_Item(ByVal IGST As String, ByVal Cess As String, ByVal HSN_Desc As String, ByVal HSNCode As String, ByVal State_Cess As String) As String

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
                 "<RATEDETAILS.LIST>" &
                    "<GSTRATEDUTYHEAD>State Cess</GSTRATEDUTYHEAD>" &
                    "<GSTRATEVALUATIONTYPE>Based on Value</GSTRATEVALUATIONTYPE>" &
                    "<GSTRATE>" & State_Cess & "</GSTRATE>" &
                    "<GSTRATEPERUNIT>0</GSTRATEPERUNIT>" &
                    "<TEMPGSTRATE>0</TEMPGSTRATE>" &
                "</RATEDETAILS.LIST>" &
                "<GSTSLABRATES.LIST></GSTSLABRATES.LIST>" &
               "</STATEWISEDETAILS.LIST>" &
              "</GSTDETAILS.LIST>"


        Return Xml_Str

    End Function


    Private Function Create_XML_LEDGERENTRIES(ByVal LEDGERNAME As String, ByVal AMOUNT As String, Optional ByVal Ignore_ISDEEMEDPOSITIVE As Boolean = False) As String

        Dim Str As String = ""
        Dim ISDEEMEDPOSITIVE As String = IIf(Val(AMOUNT) < 0, "Yes", "No")
        If Ignore_ISDEEMEDPOSITIVE Then
            ISDEEMEDPOSITIVE = "No"
        End If
        Str = "<LEDGERENTRIES.LIST>" &
              "<LEDGERNAME>" & LEDGERNAME.Replace("&", "&amp;") & "</LEDGERNAME>" &
              "<ISDEEMEDPOSITIVE>" & ISDEEMEDPOSITIVE & "</ISDEEMEDPOSITIVE> " &
              "<ROUNDTYPE>Normal Rounding</ROUNDTYPE>" &
              "<AMOUNT>" & AMOUNT & "</AMOUNT> " &
              "</LEDGERENTRIES.LIST>"
        Return Str

    End Function

    Public Shared Function Read_Settings(ByVal S_Key As String) As String
        Dim Value As String = ""
        Try
            Value = PublicShared.Settings_Dt.Select("S_Key='" & S_Key & "'").First.Item("S_Value").ToString
        Catch ex As Exception
        End Try
        Return Value
    End Function

    Public Function XmlFormat_Sales_WithOut_Stock(ByVal dr As DataRow) As String

        'Invoice,Invoice Date,Parts Amount,Service Amount,OilAmount,PaiseRoundingOff,Vat14.5%,ServiceTax,Invoice Amount

        'Service Customer Debtors - Party
        'Service Tax on Paid Labour
        'Service Labour Income
        'KVAT Collected - 14.5%
        'Sales Spares - 14.5% (Part/Oil)
        'Rounded Off +/-

        dr("Parts Amount") = CommonDA.CustomRounding(Val(dr("Parts Amount")), 2)
        dr("OilAmount") = CommonDA.CustomRounding(Val(dr("OilAmount")), 2)
        dr("Invoice Amount") = CommonDA.CustomRounding(Val(dr("Invoice Amount")), 2)
        dr("ServiceTax") = CommonDA.CustomRounding(Val(dr("ServiceTax")), 2)
        dr("Service Amount") = CommonDA.CustomRounding(Val(dr("Service Amount")), 2)
        dr("Vat14#5%") = CommonDA.CustomRounding(Val(dr("Vat14#5%")), 2)
        dr("PaiseRoundingOff") = CommonDA.CustomRounding(Val(dr("PaiseRoundingOff")), 2)

        Dim Xml As String = ""
        Dim Additional_Ledgers As String = ""
        Dim Main_Ledgers As String = ""
        Dim VOUHCER_PARTYLEDGERNAME As String = ""

        Dim InvDate As Date = dr("Invoice Date")
        Dim InvDateString As String = Format(InvDate, "yyyyMMdd")

        Dim SalesSpare_VoucherType As String = Read_Settings("SalesSpare_VoucherType")
        Dim PartyLedgerService As String = Read_Settings("PartyLedgerService")
        Dim PartyLedgerSpare As String = Read_Settings("PartyLedgerSpare")
        Dim PartyLedgerSpareWarranty As String = Read_Settings("PartyLedgerSpareWarranty")

        Dim Entry_Type As String = Strings.Left(dr("Invoice"), 3)
        Dim Sales_Spare As String = Val(dr("Parts Amount"))
        Dim Sales_Oil As String = Val(dr("OilAmount"))
        Dim Net_Invoice_Amt As String = dr("Invoice Amount")

        If Entry_Type = "WIN" Then
            'Warranty
            VOUHCER_PARTYLEDGERNAME = PartyLedgerSpareWarranty
            Main_Ledgers = Create_XML_ALLLEDGERENTRIES_PARTYLEDGER(PartyLedgerSpareWarranty, dr("Invoice Amount"))
        ElseIf Val(dr("Service Amount")) > 0 Then
            'Service
            VOUHCER_PARTYLEDGERNAME = PartyLedgerService
            Main_Ledgers = Create_XML_ALLLEDGERENTRIES_PARTYLEDGER(PartyLedgerService, Val(dr("Service Amount")) + Val(dr("ServiceTax")) + Val(dr("PaiseRoundingOff")))
            If Val(dr("Parts Amount")) > 0 Or Val(dr("OilAmount")) > 0 Then
                Main_Ledgers += Create_XML_ALLLEDGERENTRIES_PARTYLEDGER(PartyLedgerSpare, Val(dr("Parts Amount")) + Val(dr("OilAmount")) + Val(dr("Vat14#5%")))
            End If
        ElseIf Val(dr("Service Amount")) = 0 Then
            'Spare = Part +Oil+Vat
            VOUHCER_PARTYLEDGERNAME = PartyLedgerSpare
            Main_Ledgers = Create_XML_ALLLEDGERENTRIES_PARTYLEDGER(PartyLedgerSpare, dr("Invoice Amount"))
        Else
            Return Xml
        End If

        If Val(dr("Service Amount")) <> 0 Then
            Additional_Ledgers += Create_XML_ALLLEDGERENTRIES(Read_Settings("Service Amount"), dr("Service Amount"))
        End If
        If Val(dr("ServiceTax")) <> 0 Then
            Additional_Ledgers += Create_XML_ALLLEDGERENTRIES(Read_Settings("ServiceTax"), dr("ServiceTax"))
        End If
        If Val(dr("Parts Amount")) <> 0 Then
            Additional_Ledgers += Create_XML_ALLLEDGERENTRIES(Read_Settings("Parts Amount"), dr("Parts Amount"))
        End If
        If Val(dr("OilAmount")) <> 0 Then
            Additional_Ledgers += Create_XML_ALLLEDGERENTRIES(Read_Settings("OilAmount"), dr("OilAmount"))
        End If
        If Val(dr("Vat14#5%")) <> 0 Then
            Additional_Ledgers += Create_XML_ALLLEDGERENTRIES(Read_Settings("Vat14#5%"), dr("Vat14#5%"))
        End If
        If Val(dr("PaiseRoundingOff")) <> 0 Then
            Additional_Ledgers += Create_XML_ALLLEDGERENTRIES(Read_Settings("PaiseRoundingOff"), dr("PaiseRoundingOff"))
        End If

        Try
            Xml = "<ENVELOPE>" &
                      "<HEADER>" &
                          "<TALLYREQUEST>Import Data</TALLYREQUEST>" &
                      "</HEADER>" &
                      "<BODY>" &
                          "<IMPORTDATA>" &
                              "<REQUESTDESC>" &
                                  "<REPORTNAME>All Masters</REPORTNAME>" &
                              "</REQUESTDESC>" &
                              "<REQUESTDATA>" &
                                "<TALLYMESSAGE xmlns:UDF=""TallyUDF"">" &
                                      "<VOUCHER>" &
                                          "<VOUCHERTYPENAME>" & SalesSpare_VoucherType & "</VOUCHERTYPENAME>" &
                                          "<DATE>" & InvDateString & "</DATE>" &
                                          "<EFFECTIVEDATE>" & InvDateString & "</EFFECTIVEDATE>" &
                                          "<VOUCHERNUMBER>" & dr("Invoice") & "</VOUCHERNUMBER>" &
                                          "<PARTYLEDGERNAME>" & VOUHCER_PARTYLEDGERNAME & "</PARTYLEDGERNAME>" &
                                           Main_Ledgers & Additional_Ledgers &
                                      "</VOUCHER>" &
                                  "</TALLYMESSAGE>" &
                              "</REQUESTDATA>" &
                          "</IMPORTDATA>" &
                      "</BODY>" &
                  "</ENVELOPE>"


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Return Xml

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
            MsgBox(ex.Message)
        End Try

        Return strXmldata

    End Function


    Public Function XmlFormat_Voucher_Register(ByVal FromDate As String, ByVal ToDate As String) As String

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
                   " <SVFROMDATE TYPE='Date'>" & FromDate & "</SVFROMDATE>" &
                   " <SVTODATE TYPE='Date'>" & ToDate & "</SVTODATE>" &
                   " <SVEXPORTFORMAT>$$SysName:XML</SVEXPORTFORMAT >" &
                   " </STATICVARIABLES>" &
                   " </REQUESTDESC>" &
                   " </EXPORTDATA>" &
                   " </BODY>" &
                   "</ENVELOPE>"


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Return strXmldata

    End Function

    Private Function Create_XML_ALLLEDGERENTRIES(ByVal LEDGERNAME As String, ByVal AMOUNT As String) As String

        'Str = "<LEDGERENTRIES.LIST>" & _
        '      "<LEDGERNAME>" & LEDGERNAME & "</LEDGERNAME>" & _
        '      "<ISDEEMEDPOSITIVE>" & IIf(AMOUNT < 0, "Yes", "No") & "</ISDEEMEDPOSITIVE> " & _
        '      "<AMOUNT>" & AMOUNT & "</AMOUNT> " & _
        '      "<ISPARTYLEDGER>No</ISPARTYLEDGER> " & _
        '      "<ASORIGINAL>No</ASORIGINAL> "

        'Str += "</LEDGERENTRIES.LIST>"

        Dim Str As String = ""
        Dim ISDEEMEDPOSITIVE As String = IIf(Val(AMOUNT) < 0, "Yes", "No")
        Str = "<LEDGERENTRIES.LIST>" &
              "<LEDGERNAME>" & LEDGERNAME & "</LEDGERNAME>" &
              "<ISDEEMEDPOSITIVE>" & ISDEEMEDPOSITIVE & "</ISDEEMEDPOSITIVE> " &
              "<AMOUNT>" & AMOUNT & "</AMOUNT> " &
              "<ISPARTYLEDGER>No</ISPARTYLEDGER> " &
              "<ASORIGINAL>No</ASORIGINAL> " &
              "</LEDGERENTRIES.LIST>"
        Return Str
    End Function

    Private Function Create_XML_ALLLEDGERENTRIES_PARTYLEDGER(ByVal LEDGERNAME As String, ByVal AMOUNT As String) As String
        Dim Str As String = ""
        Str = "<ALLLEDGERENTRIES.LIST>" &
              "<LEDGERNAME>" & LEDGERNAME & "</LEDGERNAME>" &
              "<ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE> " &
              "<AMOUNT>-" & AMOUNT & "</AMOUNT> " &
              "<ISPARTYLEDGER>Yes</ISPARTYLEDGER> " &
              "<ASORIGINAL>No</ASORIGINAL> " &
              "</ALLLEDGERENTRIES.LIST>"
        Return Str
    End Function

    Public Function XmlFormat_Master(ByVal RptType As String) As String

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
                   " <REPORTNAME>" & RptType & "</REPORTNAME>" &
                   " <SVEXPORTFORMAT>$$SysName:XML</SVEXPORTFORMAT >" &
                   " </REQUESTDESC>" &
                   " </EXPORTDATA>" &
                   " </BODY>" &
                   "</ENVELOPE>"


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Return strXmldata

    End Function
    Public Sub Show_ClipBoard(ByVal Title As String, ByVal Msg As String)
        Dim ObjCl As FrmClipBoard
        ObjCl = New FrmClipBoard(Title, Msg)
        ObjCl.StartPosition = FormStartPosition.CenterScreen
        ObjCl.ShowDialog(Me)
    End Sub

    Private Sub ListView1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles ListView1.MouseDoubleClick
        EditDocNumber()
    End Sub

    Private Sub EditDocNumber()

        If ListView1.SelectedItems.Count > 0 Then

            If ListView1.SelectedItems(0).SubItems(2).Text = "" Then
                Panel1.Visible = True
            Else
                If MsgBox("Do you want to Change the Invoice Number " & ListView1.SelectedItems(0).SubItems(2).Text & "", vbYesNo, "Confirm") = vbYes Then
                    Panel1.Visible = True
                End If
            End If

        Else
            MsgBox("No items..!!")
        End If

    End Sub

    Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles BtnRefresh.Click
        VehicleDS = CommonDA.Get_Summary_purchasevehicle()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Panel1.Visible = False
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If ListView1.SelectedItems(0).SubItems(2).Text = "" Then
            '  Dim DocNumber = InputBox("Please Enter Invoice Number For  " & ListView1.SelectedItems(0).SubItems(4).Text & vbCrLf & "Sum Amount =" & ListView1.SelectedItems(0).SubItems(9).Text & vbCrLf & "Total Qty =" & ListView1.SelectedItems(0).SubItems(8).Text & vbCrLf, "Invoice Number")

            Dim DocNumber = Txtupdate.Text
            Dim Qry As String = ""
            Qry = "Update purchasevehicle set Invoice_Number = '" & DocNumber & "',Updated_On =now() " &
                      "Where Invoice_No='" & ListView1.SelectedItems(0).SubItems(4).Text & "'"

            If CommonDA.RunQuery(Qry) Then
                BtnRefresh.PerformClick()
                Txtupdate.Text = ""
                find()
                Panel1.Visible = False
            End If

        Else

            Dim DocNumber = Txtupdate.Text
            ' Dim DocNumber = InputBox("Please Enter Invoice Number For  " & ListView1.SelectedItems(0).SubItems(4).Text & vbCrLf & "Sum Amount =" & ListView1.SelectedItems(0).SubItems(9).Text & vbCrLf & "Total Qty =" & ListView1.SelectedItems(0).SubItems(8).Text & vbCrLf, "Invoice Number")
            Dim Qry As String = ""
            Qry = "Update purchasevehicle set Invoice_Number = '" & DocNumber & "',Updated_On =now() " &
                          "Where Invoice_No='" & ListView1.SelectedItems(0).SubItems(4).Text & "'"

            If CommonDA.RunQuery(Qry) Then
                BtnRefresh.PerformClick()
                Txtupdate.Text = ""
                find()
                Panel1.Visible = False
            End If
        End If

    End Sub
End Class
