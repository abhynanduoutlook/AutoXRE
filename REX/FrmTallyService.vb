Imports System.ComponentModel
Imports System.Data.Odbc
Imports System.IO
Imports System.Net
Imports System.Data
Imports System.Linq
Imports System.Text
Imports System.Xml

Public Class FrmTallyService
    Dim ServiceDs As TallyDs

    Dim StatusLabel As Label
    Public Sub New(ByRef LblStatus As Label)
        ServiceDs = CommonDA.Get_Summary_ServiceOnTime(Now, Now)
        StatusLabel = LblStatus
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Dim ImportDs As TallyDs
    Dim lvItem As New ListViewItem
    Dim Prev_fromDate, Prev_ToDate As Date
    Dim TallyHost As String = ""
    Dim Import_Log As String = ""
    Dim End_Process As Boolean
    Dim FrmMDI As New FrmMDI
    Dim DsLabourLedger As New DataSet
    Dim PublicTallyDS As New DataSet
    Dim PartDs As New DataSet
    Dim DsLedger As New DataSet
    Dim GST_Ds As New TallyDs
    Dim GstDs_Temp As New TallyDs
    Dim Dr As TallyDs.ServiceRow
    Dim drd As TallyDs.Service_DetailRow
    Dim Htp As New Hashtable
    Dim Refresh_Time_InMin As Double = 0.00
    Dim Existing_Entry_Checked As Boolean
    Dim BranchName As String = ""
    Dim Timer1 As New System.Timers.Timer(6000)
    Private pValue As Integer

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

    Public Sub Initlz()

        Create_Header()
        Create_Heads()
        TallyHost = Read_Settings("TallyHost")
        BranchName = Read_Settings("TallyCompanyBranch")
        Load_Gst_Details()
        Prev_fromDate = DtpFrom.Value
        Prev_ToDate = DtpTo.Value
        Load_CmbSearchBy()

        If Read_Settings("Auto_PostTo_Tally") = "Y" Then
            Reset_Timer()
            Timer1.Start()
        End If

    End Sub

    Private Sub Load_CmbSearchBy()

    End Sub

    Private Sub Create_Header()
        ListView1.Clear()
        ListView1.Columns.Add("id", 0)
        ListView1.Columns.Add("SL.NO", 40)
        ListView1.Columns.Add("INVOICE NO.", 150)
        ListView1.Columns.Add("JOB CARD NO.", 120, HorizontalAlignment.Center)
        ListView1.Columns.Add("INVOICE DATE", 120, HorizontalAlignment.Center)
        ListView1.Columns.Add("CUSTOMER", 200)
        ListView1.Columns.Add("CGST9", 70, HorizontalAlignment.Right)
        ListView1.Columns.Add("SGST9", 70, HorizontalAlignment.Right)
        ListView1.Columns.Add("CGST14", 70, HorizontalAlignment.Right)
        ListView1.Columns.Add("SGST14", 70, HorizontalAlignment.Right)
        ListView1.Columns.Add("KFC", 60, HorizontalAlignment.Right)
        ListView1.Columns.Add("TAXABLE", 70, HorizontalAlignment.Right)
        ListView1.Columns.Add("INVOICE AMOUNT", 100, HorizontalAlignment.Right)
        ListView1.Columns.Add("TALLY", 60, HorizontalAlignment.Center)
        ListView1.Columns.Add("GSTIN", 60, HorizontalAlignment.Center)
        ListView1.Columns.Add("LAST UPDATED ON", 150, HorizontalAlignment.Center)

    End Sub

    Private Sub Create_Heads()
        ListView2.Clear()
        ListView2.Columns.Add("ID", 0)
        ListView2.Columns.Add("SL.NO", 40)
        ListView2.Columns.Add("PART NO.", 110)
        ListView2.Columns.Add("DESCRIPTION", 250, HorizontalAlignment.Left)
        ListView2.Columns.Add("HSN NO", 80)
        ListView2.Columns.Add("QTY", 50)
        ListView2.Columns.Add("RATE", 88, HorizontalAlignment.Right)
        ' ListView2.Columns.Add("GROSS PRICE", 90, HorizontalAlignment.Right)
        ' ListView2.Columns.Add("DIS %", 60, HorizontalAlignment.Center)
        ListView2.Columns.Add("DIS. AMT.", 80, HorizontalAlignment.Right)
        ListView2.Columns.Add("TAXABLE", 88, HorizontalAlignment.Right)
        ListView2.Columns.Add("SGST", 70, HorizontalAlignment.Right)
        ListView2.Columns.Add("SGST%", 60, HorizontalAlignment.Center)
        ListView2.Columns.Add("CGST", 70, HorizontalAlignment.Right)
        ListView2.Columns.Add("CGST%", 60, HorizontalAlignment.Center)
        ListView2.Columns.Add("AMOUNT (RS)", 88, HorizontalAlignment.Right)

    End Sub
    Public Function LblStatus(ByVal visible As Boolean, ByVal Txt As String)

        If visible Then
            StatusLabel.Visible = True
            StatusLabel.Text = Txt
            StatusLabel.Refresh()
        Else
            StatusLabel.Text = Txt
            StatusLabel.Visible = False
            StatusLabel.Refresh()
        End If


        Return Nothing
    End Function
    Private Sub BtnFind_Click(sender As Object, e As EventArgs) Handles BtnFind.Click
        PleaseWait(True)
        find()
        PleaseWait(False)

    End Sub

    Private Sub find()

        LblStatus(True, "Please Wait .....")


        If DtpFrom.Value <> Prev_fromDate Or DtpTo.Value <> Prev_ToDate Then

            ServiceDs = CommonDA.Get_Summary_Service(DtpFrom.Value, DtpTo.Value)

            Prev_fromDate = DtpFrom.Value
            Prev_ToDate = DtpTo.Value
        ElseIf ServiceDs.Service.Rows.Count = 0 Then
            ServiceDs = CommonDA.Get_Summary_Service(DtpFrom.Value, DtpTo.Value)

        End If


        ImportDs = New TallyDs
        Dim TempDs As New TallyDs
        Dim i As Integer = 0

        If RbtActive.Checked = True Then
            ImportDs.Merge(ServiceDs.Service.Select("Updated=0 and Job_Card >='1'"))
            ' ImportDs.Merge(ServiceDs.Service_Detail.Select("Updated=0 and JobCard_Date is not null"))
        ElseIf RbtImported.Checked = True Then
            ImportDs.Merge(ServiceDs.Service.Select("Updated=1 and Job_Card >='1'"))
        ElseIf RbtMissing.Checked = True Then
            ImportDs.Merge(ServiceDs.Service.Select("Job_Card ='0' or Job_Card =''", "Invoice_Amount"))
        ElseIf RbtAll.Checked = True Then
            ImportDs = ServiceDs
        End If

        If CmbSearchBy.Text = "JobCardNumber" Then
            TempDs.Merge(ImportDs.Service.Select("Job_Card like '%" & TxtSearch.Text.Trim & "%'"))
            ImportDs = TempDs
        ElseIf CmbSearchBy.Text = "InvoiceNumber" Then
            TempDs.Merge(ImportDs.Service.Select("Invoice_Number like '%" & TxtSearch.Text.Trim & "%'"))
            ImportDs = TempDs
        Else

        End If

        ImportDs = CommonDA.Remove_Null(ImportDs)
        ListView1.Items.Clear()
        For Each Dr In ImportDs.Service.Rows
            i += 1
            lvItem = ListView1.Items.Add(Dr.Id)
            lvItem.SubItems.Add(i)
            lvItem.SubItems.Add(Dr.Invoice_Number)
            lvItem.SubItems.Add(Dr.Job_Card)
            lvItem.SubItems.Add(IIf(IsDBNull(Dr.Invoice_Date) = True, "", Format(Dr.Invoice_Date, "yyyy-MM-dd")))
            lvItem.SubItems.Add(Dr.Customer_Name)
            lvItem.SubItems.Add(Dr.CGST9)
            lvItem.SubItems.Add(Dr.SGST9)
            lvItem.SubItems.Add(Dr.CGST14)
            lvItem.SubItems.Add(Dr.SGST14)
            lvItem.SubItems.Add(Dr.KFC1)
            lvItem.SubItems.Add(Dr.Taxable_Amount)
            lvItem.SubItems.Add(Dr.Invoice_Amount)
            lvItem.SubItems.Add(IIf(Dr.Updated = "1", "Yes", "No"))
            lvItem.SubItems.Add(Dr.GSTIN)
            ' lvItem.SubItems.Add(Dr.LastSeen)

        Next

        LblStatus(False, "")


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
        Dim InvoiceType As String = ListView1.SelectedItems.Item(0).SubItems(3).Text

        DetailedDs.Merge(ServiceDs.Service_Detail.Select("Invoice_Number = '" & InvoiceNum & "'"))

        ListView2.Items.Clear()
        For Each drd In DetailedDs.Service_Detail.Rows

            i += 1
            lvItem = ListView2.Items.Add(drd.Id)
            lvItem.SubItems.Add(i)
            lvItem.SubItems.Add(drd.Part_Labour_Code)
            lvItem.SubItems.Add(drd.Part_Labour_Description)
            lvItem.SubItems.Add(drd.HSN_SAC_code)
            lvItem.SubItems.Add(drd.Issued_Qty)
            lvItem.SubItems.Add(drd.Rate)
            'lvItem.SubItems.Add(Val(Val(drd.Issued_Qty) * Val(drd.Rate)))
            lvItem.SubItems.Add(drd.Discount)
            lvItem.SubItems.Add(drd.Taxable)
            lvItem.SubItems.Add(drd.SGST)
            lvItem.SubItems.Add(drd.SGST_Per)
            lvItem.SubItems.Add(drd.CGST)
            lvItem.SubItems.Add(drd.CGST_Per)
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

    Private Sub Reset_Timer()

        AddHandler Timer1.Elapsed, AddressOf Timer1_Tick
        AddHandler Timer1.Disposed, AddressOf Timer1_Disposed
        If Val(Read_Settings("Refresh_Time_InMin")) > 0 Then
            Refresh_Time_InMin = 60 * 1000 * Val(Read_Settings("Refresh_Time_InMin"))
        Else
            Refresh_Time_InMin = 10 * 1000
        End If

        If PublicShared.Sys_Mode <> "AD" Then
            Timer1.Interval = Refresh_Time_InMin
            Timer1.Enabled = True
            Timer1.Start()
        Else
            Timer1.Enabled = False
        End If
    End Sub


    Private Sub Timer1_Disposed(ByVal sender As Object, ByVal e As System.EventArgs)
        Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Not BgWorkerImportToTally.IsBusy Then
            BgWorkerImportToTally.RunWorkerAsync()
        End If
    End Sub

    Private Sub BgWorkerImportToTally_DoWork(sender As Object, e As DoWorkEventArgs) Handles BgWorkerImportToTally.DoWork

        Try
            ServiceDs = IIf(ServiceDs.Tables("Service").Rows.Count > 0 And ServiceDs.Tables("Service_Detail").Rows.Count > 0, ServiceDs, CommonDA.Get_Summary_ServiceOnTime(Nothing, Nothing))
            If ServiceDs.Service.Select("Tally=0 and Job_Card_Date is not null ").First().Table.Rows.Count > 0 Then
                Import_To_Tally(False)
            End If
        Catch ex As Exception

        End Try


    End Sub

    Public Sub PleaseWait(ByVal Show As Boolean)
        If Show Then
            'FrmWait.LblProgress.Text = "Please Wait..."
            'FrmWait.Show(Me)
            'FrmWait.Refresh()

            If FrmWait.Visible Then

                FrmWait.LblProgress.Text = "                         Please Wait....."
                FrmWait.Refresh()
            ElseIf FrmWait.IsDisposed Then
                FrmWait = New FrmWait()
                FrmWait.LblProgress.Text = "                         Please Wait....."
                FrmWait.Show(Me)
                FrmWait.Refresh()
            Else
                FrmWait.LblProgress.Text = "                         Please Wait....."
                FrmWait.Show(Me)
                FrmWait.Refresh()
            End If


        Else
            FrmWait.Visible = False
        End If


    End Sub
    Public Sub PleaseWait_Progress(ByVal Text As String)
        If FrmWait.Visible Then
            FrmWait.LblProgress.Text = Text
            FrmWait.LblProgress.Refresh()
        End If
    End Sub

    Private Sub BtnImport_Click(sender As Object, e As EventArgs) Handles BtnImport.Click
        PleaseWait(True)
        Process_Import()
        PleaseWait(False)
    End Sub

    Public Sub Process_Import()

        PleaseWait(True)

        If RbtActive.Checked = True Then

            If ImportDs.Service.Select("Updated=0 and Job_Card >='1'").Count > 0 Then
                BtnImport.Width = 0
                Import_To_Tally(True)
                BtnImport.Width = PnlImport.Width
            Else
                MsgBox("No Bills Found To Import")
            End If

        ElseIf RbtAll.Checked = True Then
            If ImportDs.Service.Select("Job_Card >='1'").Count > 0 Then
                BtnImport.Width = 0
                Import_To_Tally(False)
                BtnImport.Width = PnlImport.Width
            Else
                MsgBox("No Bills Found To Import")
            End If

        ElseIf RbtMissing.Checked = True Then

            MsgBox("Missing Bills Cannot be Imported !", vbInformation)

        End If
        PleaseWait(False)

    End Sub

    Private Sub Load_Gst_Details()
        GST_Ds = New TallyDs
        Dim Objda As New CommonDA
        GST_Ds = objda.Get_GST_Details(True, "O")
    End Sub


    Public Sub Import_To_Tally(ByVal status As Boolean)

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
        Dim DsDetails As New TallyDs
        Dim tempDS As New TallyDs
        Dim ErrorLine As String = ""
        Dim SalesVoucherType As String = ""
        Dim BillType As String = ""
        Dim RefNo As String = ""
        Dim PrevSalesVoucherType As String = ""

        If status Then

        Else
            'ImportDs = New TallyDs
            'ImportDs.Merge(ServiceDs.Service.Select("Updated=0 and Job_Card >='1'"))
            '  ImportDs.Merge(ServiceDs.Service_Bills.Select("Updated=0 and JobCard_Date is not null"))
        End If

        If ImportDs.Service.Rows.Count > 0 Then
            Load_PartName()

            If CheckPartNames() = False Then
                Exit Sub
            End If

            'Load_Labour_Ledgers()

            'If CheckLabourLedgers() = False Then
            '    Exit Sub
            'End If

            Get_Ledgers("")

            Dim totalCount As Integer = ImportDs.Service.Rows.Count
            For Each Dr In ImportDs.Service.Rows

                If End_Process Then
                    Exit For
                End If

                ProgresBar(totalCount, BtnImport, PnlImport)

                BillType = Strings.Left(Dr.Invoice_Number, 3)

                'If BillType = "SIW" Then
                '    SalesVoucherType = Read_Settings("SalesSpare_VoucherType_SIW")
                'ElseIf BillType = "PBV" Then
                '    SalesVoucherType = Read_Settings("SalesSpare_VoucherType_PBVPS")
                'Else
                '    SalesVoucherType = Read_Settings("SalesSpare_VoucherType")
                'End If

                PleaseWait_Progress("Importing.. Invoice Number:" & Dr.Invoice_Number & "")

                If BillType = "SSI" Then
                    SalesVoucherType = Read_Ledgers("Service_VT_SSI")
                ElseIf BillType = "SPI" Then
                    SalesVoucherType = Read_Ledgers("Service_VT_SPI")
                ElseIf BillType = "SLI" Then
                    SalesVoucherType = Read_Ledgers("Service_VT_SLI")
                Else
                    SalesVoucherType = Read_Ledgers("Service_VT_" & BillType & "")
                End If

                If SalesVoucherType = "" Then
                    SalesVoucherType = Read_Ledgers("Service_VT")
                End If



                If CurrentDate.Date <> Dr.Invoice_Date.Date And PrevSalesVoucherType <> SalesVoucherType Then
                    PrevSalesVoucherType = SalesVoucherType

                    CurrentDate = Dr.Invoice_Date
                    Export_Data_Status = Export_Xml(Format(CurrentDate.Date, "yyyyMMdd"), Format(CurrentDate.Date, "yyyyMMdd"), SalesVoucherType)
                    If Export_Data_Status = False Then
                        If MsgBox("Warnings : Failed to check existing entries..." & vbCrLf &
                               "Do you want to continue?") = Windows.Forms.DialogResult.No Then
                            CommonDA.Create_Log(Me.Text, "Import-Aborted", "Failed to check existing entries in tally...")
                            Exit For
                        Else
                            CommonDA.Create_Log(Me.Text, "Import to Tally - Continue", "Failed to check existing entries in tally...")
                        End If
                    End If
                End If

                If Is_Already_Exist(Dr.Invoice_Number, SalesVoucherType) Then
                    AlreadyExist_Int += 1
                    AlreadyExist_String += Dr.Invoice_Number & " , "
                    CommonDA.Update_Service(Dr)
                    'Skip
                Else
                    'Do
                    Try
                        ObjXml.open("POST", TallyHost, False)

                        DsDetails.Merge(ImportDs.Service.Select("Invoice_Number = '" & Dr.Invoice_Number & "'", "Id"))

                        If DsDetails.Service_Detail.Rows.Count = 0 Then
                            Import_Log += vbCrLf & " Job Card details not found for : " & Dr.Invoice_Number
                        End If
                        If Dr.Job_Card <> "" Then


                            StrXmldata = XmlFormat_Sales_WithOut_Stock(Dr, DsDetails.Tables("Service_Detail"), SalesVoucherType)
                        End If
                        DsDetails.Service_Detail.Rows.Clear()

                        If Strings.Left(StrXmldata, 10) <> "<ENVELOPE>" Then
                            ErrorList += StrXmldata
                        End If



                        If Read_Settings("Show_Request_XML") = "Y" Then
                            Show_ClipBoard(Dr.Invoice_Number.ToString, StrXmldata)
                        End If


                        'StrXmldata = Test_Xml()
                        If StrXmldata = "" Then
                            Import_Log += vbCrLf & Dr.Invoice_Number & " - Unable to create tally request.."
                        Else
                            Try
                                ObjXml.send(StrXmldata)
                                Responsestr = ObjXml.responseText
                            Catch ex As Exception
                            End Try

                            If Read_Settings("Show_Responds_XML") = "Y" Then
                                Show_ClipBoard("", Responsestr)
                            End If

                            If Responsestr <> Nothing Then

                                XMLDOM.loadXML(Responsestr)
                                Stream = New StringReader(Responsestr)
                                Reader = New XmlTextReader(Stream)
                                XmlDS.Clear()
                                XmlDS.ReadXml(Reader)

                                If Responsestr.Contains("Stock Item") Then

                                End If

                                If XmlDS.Tables.Contains("response") Then
                                    Created += Val(XmlDS.Tables("response").Rows(0).Item("CREATED").ToString)
                                    Altered += Val(XmlDS.Tables("response").Rows(0).Item("ALTERED").ToString)
                                    Errors += Val(XmlDS.Tables("response").Rows(0).Item("ERRORS").ToString)

                                    If XmlDS.Tables("response").Rows(0).Item(0).ToString.Contains("already exists") Then
                                        Import_Log += Dr.Invoice_Number & " - " &
                                           "Created :" & XmlDS.Tables("response").Rows(0).Item("CREATED").ToString &
                                           ",Skipped :" & XmlDS.Tables("response").Rows(0).Item("ERRORS").ToString &
                                            ",Altered :" & XmlDS.Tables("response").Rows(0).Item("ALTERED").ToString
                                        'CommonDA.Update_Service(Dr)
                                        AlreadyExist_Int += 1
                                        Errors -= 1

                                    Else


                                        Import_Log += Dr.Invoice_Number & " - " &
                                            "Created :" & XmlDS.Tables("response").Rows(0).Item("CREATED").ToString &
                                            ",Altered :" & XmlDS.Tables("response").Rows(0).Item("ALTERED").ToString &
                                            ",Errors :" & XmlDS.Tables("response").Rows(0).Item("ERRORS").ToString

                                    End If

                                    If Val(XmlDS.Tables("response").Rows(0).Item("ERRORS").ToString) > 0 Then
                                        If XmlDS.Tables("response").Columns.Contains("LINEERROR") Then
                                            Import_Log += " ," & XmlDS.Tables("response").Rows(0).Item("LINEERROR").ToString
                                            ErrorList += XmlDS.Tables("response").Rows(0).Item("LINEERROR").ToString
                                        End If
                                    End If
                                    If Val(XmlDS.Tables("response").Rows(0).Item("CREATED").ToString) > 0 Then
                                        Try
                                            Dim VchId As String = XmlDS.Tables("response").Rows(0).Item("LASTVCHID").ToString
                                            CommonDA.Update_Service_VchId(Dr, VchId)
                                        Catch ex As Exception
                                            CommonDA.Update_Service(Dr)

                                        End Try

                                    End If
                                ElseIf XmlDS.Tables.Contains("LINEERROR") Then
                                    Import_Log += Dr.Invoice_Number & " - " &
                                                XmlDS.Tables("LINEERROR").Rows(0).Item("LINEERROR_Text").ToString
                                    ErrorList += Dr.Invoice_Number & " - " &
                                                XmlDS.Tables("LINEERROR").Rows(0).Item("LINEERROR_Text").ToString
                                    Errors += 1
                                End If

                                Import_Log += vbCrLf
                            Else
                                Import_Log += vbCrLf & Dr.Invoice_Number & " - Tally not responding..."
                                Errors += 1
                                ErrorList += "Tally not responding..." & vbCrLf
                            End If

                        End If

                    Catch ex As Exception
                        Import_Log += vbCrLf & Dr.Invoice_Number & " -Error- " & ex.Message
                        CommonDA.Create_Log("Import To Tally Service", "Import Error", ex.Message & vbCrLf & ex.StackTrace)

                        LblStatus(True, Import_Log)
                        If XmlDS.Tables.Count > 0 Then
                            If XmlDS.Tables(0).Rows.Count > 0 Then
                                ErrorList += XmlDS.Tables(0).Rows(0).Item(0).ToString & vbCrLf
                            End If
                        End If
                        Errors += 1
                    End Try
                End If

                LblImportStatus.Text = "Import Status = " & i & "/" & ImportDs.Service.Rows.Count & vbCrLf &
                            "Created : " & Created & " , Altered : " & Altered & " , Skipped : " & AlreadyExist_Int & " , Errors : " & Errors
                LblImportStatus.Refresh()

                ' LblStatus(True, LblImportStatus.Text)

                i += 1


            Next
            If AlreadyExist_String <> "" Then
                ' Show_ClipBoard("Skipped entries", "Skipped entries ( " & AlreadyExist_Int & " ) : " & vbCrLf & AlreadyExist_String)
            End If

            If ErrorList = "" Then
                ImportDs.Clear()
                Me.BtnRefresh.PerformClick()
                'Show_ClipBoard("Completed successfully.", "" & vbCrLf & LblImportStatus.Text)
                ' CommonDA.Create_Log("Import to Tally", "Import", LblImportStatus.Text & vbCrLf & Import_Log)
                '* 'If Created > 0 Then
                '    Try
                '        IO.File.Move(TxtPath.Text, Application.StartupPath & "\BackUp\Serv_" & Format(Date.Now, "ddMMMyy_HHmmss") & ".xls")
                '    Catch ex As Exception
                '        CommonDA.Create_Log("Import to Tally", "Failed to move " & TxtPath.Text, ErrorList)
                '    End Try
                'End If
                ' */
                ''''''TxtPath.Text = ""
                ''''''LblFileInfo.Text = "File Info :"
                ''''''LblImportStatus.Text = "Import Status :"
            Else
                Show_ClipBoard("", LblImportStatus.Text & vbCrLf & ErrorList)
                Me.BtnRefresh.PerformClick()
                ' CommonDA.Create_Log("Import to Tally", "Import-Error", LblImportStatus.Text & vbCrLf & Import_Log & ErrorList)
            End If
        End If



        LblStatus(False, "")

    End Sub

    Public Sub Show_ClipBoard(ByVal Title As String, ByVal Msg As String)
        Dim ObjCl As FrmClipBoard
        ObjCl = New FrmClipBoard(Title, Msg)
        ObjCl.StartPosition = FormStartPosition.CenterScreen
        ObjCl.ShowDialog(Me)
    End Sub

    Public Shared Function ProgresBar(ByVal Count As Integer, ByRef buton As Button, ByRef pnl As Panel)


        If buton.Width <= pnl.Width Then
            buton.Width += pnl.Width / Count
        Else
            buton.Width += pnl.Width / Count
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

    Public Shared Function Read_Ledgers(ByVal S_Key As String) As String
        Dim Value As String = ""
        Try
            Value = PublicShared.Ledgers_Dt.Select("S_Key='" & S_Key & "'").First.Item("S_Value").ToString
        Catch ex As Exception
        End Try
        Return Value
    End Function



    Public Function XmlFormat_Sales_WithOut_Stock(ByVal Dr As TallyDs.ServiceRow, ByVal Dt As DataTable, ByVal SalesSpare_VoucherType As String) As String

        Dim Xml As String = ""
        Dim Additional_Ledgers As String = ""
        Dim Main_Ledgers As String = ""
        Dim VOUHCER_PARTYLEDGERNAME As String = ""
        Dim Parts_Entries As String = ""
        Dim Missing_Parts As String = ""
        Dim Narration As String = ""

        Dim InvDate As Date = Dr.Invoice_Date
        Dim InvDateString As String = Format(InvDate, "yyyyMMdd")
        Dim BillType As String = Strings.Left(Dr.Invoice_Number, 3)
        Dim ROType As String = Strings.Left(Dr.Job_Card, 3)
        Dim COST_CENTER As String = ""
        Dim PartyLedgerService As String = ""
        Dim Alloc_Ledger As String = ""
        Dim StrXmldata As String = ""
        Dim IsClaim As Boolean = False
        Dim IsEW As Boolean = False
        Dim IsRSA As Boolean = False
        Dim Total_Labour As Decimal
        Dim Is_Igst As Boolean = False
        Dim RefNo As String = Dr.Job_Card
        Dim Is_Ins As Boolean = False
        Dim DrL As TallyDs.Service_LedgersRow
        Dim Taxable18 As Decimal = 0.00
        Dim Taxable28 As Decimal = 0.00

        'If BillType = "SSI" Then
        '    PartyLedgerService = Read_Ledgers("Service_PartyLedger_SSI")
        'ElseIf BillType = "WLI" Or BillType = "WPI" Then

        '    Is_Igst = True
        '    RefNo = Dr.Job_Card
        '    PartyLedgerService = IIf(Dr.Part_Labour_Description.Contains("Free Service Coupon") Or Dr.Part_Labour_Description.Contains("free service coupon"), Read_Ledgers("Service_PartyLedger_FreeService"), Read_Ledgers("Service_PartyLedger_WLP"))

        '    ' PartyLedgerService = Read_Ledgers("Service_PartyLedger_WLP")

        'ElseIf BillType = "ILI" Or BillType = "IPI" Then
        '    '  PartyLedgerService = Read_Ledgers("Service_PartyLedger_ILIP")
        '    PartyLedgerService = Read_Ledgers("Service_PartyLedger_INS_" & Dr.Customer_Name.Trim & "")
        '    If PartyLedgerService = "" Then
        '        Show_ClipBoard("Ledger Missing", " No Ledger Found in Settings  For " & Dr.Customer_Name & vbCrLf & "")
        '        CommonDA.Create_Settings(True, "Service_PartyLedger_INS_" & Dr.Customer_Name.Trim & "", "")
        '        CommonDA.Create_Log("Importing", "Service_PartyLedger_INS_" & Dr.Customer_Name.Trim, "")
        '        Return ""
        '        Exit Function
        '    End If
        '    Is_Ins = True
        '    If Dr.IGST18 > 0 Or Dr.IGST28 > 0 Then
        '        Is_Igst = True
        '    End If

        'Else
        '    PartyLedgerService = Read_Ledgers("Service_PartyLedger_" & BillType & "")
        'End If

        'If PartyLedgerService = "" Then
        '    Show_ClipBoard("PartyLedgerService", "PartyLedgerService is Missing for " & vbCrLf & Dr.Invoice_Number)
        '    Return ""
        '    Exit Function
        'End If

        If Dr.GSTIN <> "" Then

            If DsLedger.Tables.Count > 0 Then
                If DsLedger.Tables("ledger").Select("$PARTYGSTIN = '" & Dr.GSTIN & "'").Count = 0 Then
                    ' StrXmldata = XmlFormat_Ledger(Dr.GSTIN, PartyLedgerService, Read_Settings("partyledgerservice_parent"), Dr.VEHICLENO)
                    'Export_Tally_Method1(StrXmldata)
                Else
                    PartyLedgerService = DsLedger.Tables("ledger").Select("$PARTYGSTIN = '" & Dr.GSTIN & "'").First.Item("$NAME").ToString
                End If
            End If

            Narration = Narration & ",GST-NO:" & Dr.GSTIN
        Else
            PartyLedgerService = Read_Ledgers("Service_PartyLedger")
        End If





        'If PartyLedgerService.Trim = "" Then
        '    PartyLedgerService = Dr.Customer_Name & " " & Dr.Chassis_No  ' "Mr. Gigi K  (MR7164748285)" '
        'End If

        'If DsLedger.Tables.Count > 0 Then
        '    If DsLedger.Tables("Ledger").Select("$NAME = '" & PartyLedgerService & "'").Count = 0 Then
        '        StrXmldata = XmlFormat_Ledger(PartyLedgerService, Read_Ledgers("Service_PartyLedger_Parent"))
        '        Export_Tally_Method1(StrXmldata)
        '    End If
        'Else
        '    StrXmldata = XmlFormat_Ledger(PartyLedgerService, Read_Ledgers("Service_PartyLedger_Parent"))
        '    Export_Tally_Method1(StrXmldata)
        'End If

        Dim CustVeh As String = Dr.Customer_Name & "(" & Dr.Reg_No & ")"
        Narration += "Job Card No :" & Dr.Job_Card & " , Cust Name:" & CustVeh & ""



        If Is_Igst Then


            Dim Taxable1 As Decimal = 0.0

            If Val(Dr.IGST28_Per) > 0 Then
                Taxable28 = Math.Round((Val(Dr.IGST28) / (Val(Dr.IGST28_Per))) * 100, 2)
            End If
            If Val(Dr.IGST18_Per) > 0 Then
                Taxable18 = Math.Round((Val(Dr.IGST18) / (Val(Dr.IGST18_Per))) * 100, 2)
            End If


            Dim IGST28 As Decimal = 0.0
            Dim IGST18 As Decimal = 0.0


            IGST28 = Taxable28 * (Dr.IGST28_Per) / 100
            IGST18 = Taxable18 * (Dr.IGST18_Per) / 100

            Dr.IGST18 = Math.Round(IGST18, 2)
            Dr.IGST28 = Math.Round(IGST28, 2)


            Dr.Total_Amount = Taxable28 + Taxable18 + Dr.IGST18 + Dr.IGST28



        Else



            Dim Taxable1 As Decimal = 0.0

            If Val(Dr.CGST14) > 0 Then
                Taxable28 = Math.Round(((Val(Dr.CGST14) + Val(Dr.SGST14)) / (Val(Dr.SGST14_Per) + Val(Dr.CGST14_Per))) * 100, 2)
            End If
            If Val(Dr.CGST9) > 0 Then
                Taxable18 = Math.Round(((Val(Dr.CGST9) + Val(Dr.SGST9)) / (Val(Dr.SGST9_Per) + Val(Dr.CGST9_Per))) * 100, 2)
            End If

            If Val(Dr.KFC1) > 0 Then
                Taxable1 = Math.Round(Val(Dr.KFC1) / Val(Dr.KFC1_Per) * 100, 2)
            End If

            Dim GST14 As Decimal = 0.0
            Dim GST9 As Decimal = 0.0


            Dim CGST14 As Decimal = 0.0
            Dim SGST9 As Decimal = 0.0
            Dim SGST14 As Decimal = 0.0
            Dim CGST9 As Decimal = 0.0
            Dim KFC1 As Decimal = 0.0

            GST14 = Taxable28 * (Dr.CGST14_Per + Dr.SGST14_Per) / 100
            GST9 = Taxable18 * (Dr.CGST9_Per + Dr.SGST9_Per) / 100

            KFC1 = Taxable1 * Dr.KFC1_Per / 100

            Dr.KFC1 = KFC1

            Dr.CGST14 = Math.Round(GST14 / 2, 2)
            Dr.SGST14 = Math.Round(GST14 / 2, 2)

            Dr.SGST9 = Math.Round(GST9 / 2, 2)
            Dr.CGST9 = Math.Round(GST9 / 2, 2)

            If BillType.Contains("LI") Then
                Dr.Total_Amount = Taxable28 + Dr.Taxable_Amount + Dr.CGST14 + Dr.SGST14 + Dr.CGST9 + Dr.SGST9 + Dr.KFC1

            Else
                Dr.Total_Amount = Taxable28 + Taxable18 + Dr.CGST14 + Dr.SGST14 + Dr.CGST9 + Dr.SGST9 + Dr.KFC1

            End If

        End If



        Dr.Invoice_Amount = Math.Round(Dr.Invoice_Amount)

        Main_Ledgers = Create_XML_LEDGERENTRIES_PARTYLEDGER(PartyLedgerService, -Math.Round(Dr.Invoice_Amount), RefNo, True)

        Parts_Entries = ""

        If Parts_Entries = "" Then
            Parts_Entries = "<ALLINVENTORYENTRIES.LIST></ALLINVENTORYENTRIES.LIST>"
        End If


        If BillType.Contains("LI") Then

            If Is_Igst Then
                If Taxable18 > 0 Then
                    Dim LabourLedger As String = ""
                    If Read_Ledgers("Service_PartyLedger_INS_" & Dr.Customer_Name.Trim & "") <> "" Then
                        LabourLedger = Read_Ledgers("Service_LabourLedger_INS_IGST")
                    Else
                        LabourLedger = IIf(Dr.Part_Labour_Description.Contains("Free Service Coupon") Or Dr.Part_Labour_Description.Contains("free service coupon"), Read_Ledgers("Service_LabourLedger_FreeService"), Read_Ledgers("Service_LabourLedger_WLI"))
                    End If

                    COST_CENTER = Create_XML_COSTCENTER(Read_Ledgers("Cost_Center_Labour"), Taxable18)
                    Additional_Ledgers += Create_XML_LEDGERENTRIES(LabourLedger, Taxable18, COST_CENTER)
                End If

            Else

                If Dr.Taxable_Amount > 0 Then
                    COST_CENTER = Create_XML_COSTCENTER(Read_Ledgers("Cost_Center_Labour"), Dr.Taxable_Amount)
                    Additional_Ledgers += Create_XML_LEDGERENTRIES(Read_Ledgers("Service_LabourLedger_" & BillType), Dr.Taxable_Amount, COST_CENTER)
                End If
            End If

        Else

            If Not Is_Igst Then
                If Taxable18 > 0 Then
                    COST_CENTER = Create_XML_COSTCENTER(Read_Ledgers("Cost_Center_Part"), Taxable18)
                    Additional_Ledgers += Create_XML_LEDGERENTRIES(Read_Ledgers("Service_PartsLedger_18"), Taxable18, COST_CENTER)
                End If
            Else
                COST_CENTER = Create_XML_COSTCENTER(Read_Ledgers("Cost_Center_Part"), Taxable18)
                Additional_Ledgers += Create_XML_LEDGERENTRIES(Read_Ledgers("Service_PartsLedger_18_IGST" & IIf(Is_Ins, "_INS", "")), Taxable18, COST_CENTER)
            End If

        End If


        If Taxable28 > 0 Then
            COST_CENTER = Create_XML_COSTCENTER(Read_Ledgers("Cost_Center_Part"), Taxable28)
            Additional_Ledgers += Create_XML_LEDGERENTRIES(Read_Ledgers("Service_PartsLedger_28" & IIf(Is_Igst, "_IGST" & IIf(Is_Ins, "_INS", ""), "")), Taxable28, COST_CENTER)
        End If



        Dim DrGST As TallyDs.GST_DetailsRow
        GstDs_Temp = New TallyDs
        GstDs_Temp.Merge(GST_Ds)

        ' For Each Drn As TallyDs.Service_BillsRow In Dt.Rows

        If GstDs_Temp.GST_Details.Select("GST_Name = 'CGST' And GST_Per = '" & Dr.CGST14_Per & "'", "").Count > 0 Then
            DrGST = GstDs_Temp.GST_Details.Select("GST_Name = 'CGST' And GST_Per = '" & Dr.CGST14_Per & "'", "").First
            DrGST.Amount = Dr.CGST14
        End If

        If GstDs_Temp.GST_Details.Select("GST_Name = 'SGST' And GST_Per = '" & Dr.SGST14_Per & "'", "").Count > 0 Then
            DrGST = GstDs_Temp.GST_Details.Select("GST_Name = 'SGST' And GST_Per = '" & Dr.SGST14_Per & "'", "").First
            DrGST.Amount = Dr.SGST14
        End If

        If GstDs_Temp.GST_Details.Select("GST_Name = 'CGST' And GST_Per = '" & Dr.CGST9_Per & "'", "").Count > 0 Then
            DrGST = GstDs_Temp.GST_Details.Select("GST_Name = 'CGST' And GST_Per = '" & Dr.CGST9_Per & "'", "").First
            DrGST.Amount = Dr.CGST9
        End If

        If GstDs_Temp.GST_Details.Select("GST_Name = 'SGST' And GST_Per = '" & Dr.SGST9_Per & "'", "").Count > 0 Then
            DrGST = GstDs_Temp.GST_Details.Select("GST_Name = 'SGST' And GST_Per = '" & Dr.SGST9_Per & "'", "").First
            DrGST.Amount = Dr.SGST9
        End If

        If GstDs_Temp.GST_Details.Select("GST_Name = 'IGST' And GST_Per = '" & Dr.IGST18_Per & "'", "").Count > 0 Then
            DrGST = GstDs_Temp.GST_Details.Select("GST_Name = 'IGST' And GST_Per = '" & Dr.IGST18_Per & "'", "").First
            DrGST.Amount = Dr.IGST18
        End If

        If GstDs_Temp.GST_Details.Select("GST_Name = 'IGST' And GST_Per = '" & Dr.IGST28_Per & "'", "").Count > 0 Then
            DrGST = GstDs_Temp.GST_Details.Select("GST_Name = 'IGST' And GST_Per = '" & Dr.IGST28_Per & "'", "").First
            DrGST.Amount = Dr.IGST28
        End If

        GstDs_Temp.AcceptChanges()


        ''GST Ledger
        For Each DrG As TallyDs.GST_DetailsRow In GstDs_Temp.GST_Details.Rows
            If DrG.Amount > 0 Then
                Additional_Ledgers += Create_XML_LEDGERENTRIES(DrG.Tally_Ledger, DrG.Amount, "")

            End If
        Next



        If Dr.KFC1 > 0 Then
            Additional_Ledgers += Create_XML_LEDGERENTRIES(Read_Ledgers("Ledger_KFCess"), Dr.KFC1, "")
        End If

        Dr.PaiseRoundingOff = Dr.Invoice_Amount - Dr.Total_Amount


        If Dr.Invoice_Amount = Dr.Total_Amount + Dr.PaiseRoundingOff Then

        ElseIf Dr.Invoice_Amount = Dr.Total_Amount + Dr.PaiseRound Then
            Dr.PaiseRoundingOff = Dr.PaiseRound
        Else
            ' MsgBox("Round off")
        End If


        Dim CostCentreName As String = ""

        If BillType.Contains("LI") Then
            CostCentreName = "Cost_Center_Labour"
        Else
            CostCentreName = "Cost_Center_Part"
        End If



        If Dr.PaiseRoundingOff <> 0 And Dr.PaiseRoundingOff < 1 Then
            COST_CENTER = Create_XML_COSTCENTER(Read_Ledgers(CostCentreName), Format(Dr.PaiseRoundingOff, "0.00"))
            Additional_Ledgers += Create_XML_LEDGERENTRIES(Read_Ledgers("Ledger_PaiseRoundOff"), Dr.PaiseRoundingOff, COST_CENTER, True)
        ElseIf Dr.PaiseRound <> 0 And Dr.PaiseRound < 1 And dr.PaiseRoundingOff <> 0 Then
            COST_CENTER = Create_XML_COSTCENTER(Read_Ledgers(CostCentreName), Dr.PaiseRound)
            Additional_Ledgers += Create_XML_LEDGERENTRIES(Read_Ledgers("Ledger_PaiseRoundOff"), Dr.PaiseRound, COST_CENTER, True)
        End If

        Try

            '"<STATICVARIABLES>" & _
            '    "<SVCURRENTCOMPANY>EVM CARS - (From 1-Apr-2015)</SVCURRENTCOMPANY>" & _
            '"</STATICVARIABLES>" & _

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
                              "<REQUESTDATA>" &
                                "<TALLYMESSAGE xmlns:UDF=""TallyUDF"">" &
                                      "<VOUCHER> " &
                                          "<VOUCHERTYPENAME>" & SalesSpare_VoucherType.Replace("&", "&amp;") & "</VOUCHERTYPENAME>" &
                                          "<DATE>" & InvDateString & "</DATE>" &
                                          "<EFFECTIVEDATE>" & InvDateString & "</EFFECTIVEDATE>" &
                                          "<VOUCHERNUMBER>" & Dr.Invoice_Number & "</VOUCHERNUMBER>" &
                                          "<REFERENCE>" & Dr.Job_Card & "</REFERENCE>" &
                                          "<NARRATION>" & Narration.Replace("&", "&amp;") & "</NARRATION>" &
                                          "<PARTYLEDGERNAME>" & VOUHCER_PARTYLEDGERNAME.Replace("&", "&amp;") & "</PARTYLEDGERNAME>" &
                                          "<ISINVOICE>Yes</ISINVOICE>" &
                                           Main_Ledgers & Additional_Ledgers & Parts_Entries &
                                      "</VOUCHER>" &
                                  "</TALLYMESSAGE>" &
                              "</REQUESTDATA>" &
                          "</IMPORTDATA>" &
                      "</BODY>" &
                  "</ENVELOPE>"


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Return IIf(Missing_Parts = "", Xml, Missing_Parts)

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



    Private Sub Get_Ledgers()

        Dim Qry As String
        Dim TallyCon As New OdbcConnection(Read_Settings("TallyODBC"))
        Dim Cmd As New OdbcCommand
        Dim Da As New OdbcDataAdapter
        Dim Parent As String = Read_Ledgers("Service_PartyLedger_Parent")
        Dim StrXmldata As String = ""

        Cmd.Connection = TallyCon

        Try

            TallyCon.Open()

            DsLedger.Clear()

            Qry = "Select $NAME,$LEDGERMOBILE from ListOfLedgers WHERE $PARENT = '" & Parent & "'"

            Cmd.CommandText = Qry
            Da.SelectCommand = Cmd
            Da.Fill(DsLedger, "Ledger")

            TallyCon.Close()
            TallyCon.Dispose()

        Catch ex As Exception
        End Try

    End Sub


    Private Sub Get_Ledgers(ByVal Parent As String)

        Dim Qry As String
        Dim TallyCon As New OdbcConnection(Read_Settings("TallyODBC"))
        Dim Cmd As New OdbcCommand
        Dim Da As New OdbcDataAdapter
        'Dim Parent As String = Read_Settings("PartyLedgerService_Parent")
        Dim StrXmldata As String = ""

        Cmd.Connection = TallyCon

        Try

            TallyCon.Open()

            DsLedger.Clear()

            If Parent <> "" Then
                Qry = "Select $NAME,$LEDGERMOBILE,$PARTYGSTIN from ListOfLedgers WHERE $PARENT = '" & Parent & "'"
            Else
                Qry = "Select $NAME,$LEDGERMOBILE,$PARTYGSTIN from ListOfLedgers "
            End If

            Cmd.CommandText = Qry
            Da.SelectCommand = Cmd
            Da.Fill(DsLedger, "Ledger")

            TallyCon.Close()
            TallyCon.Dispose()

        Catch ex As Exception
        End Try

    End Sub



    Private Sub Load_Labour_Ledgers()

        Dim Qry As String
        Dim TallyCon As New OdbcConnection(Read_Settings("TallyODBC"))
        Dim Cmd As New OdbcCommand
        Dim Da As New OdbcDataAdapter
        Dim Parent As String = Read_Ledgers("Service_LabourLedger_Parent")
        Dim StrXmldata As String = ""

        Cmd.Connection = TallyCon

        Try

            TallyCon.Open()

            DsLabourLedger.Clear()

            Qry = "Select $NAME,$LEDGERMOBILE from ListOfLedgers WHERE $PARENT = '" & Parent & "'"

            Cmd.CommandText = Qry
            Da.SelectCommand = Cmd
            Da.Fill(DsLabourLedger, "LabourLedger")

            TallyCon.Close()
            TallyCon.Dispose()

        Catch ex As Exception
        End Try

    End Sub

    Private Sub Load_PartName()

        PartDs = New DataSet
        PartDs.Tables.Add("PartName")
        PartDs.Tables("PartName").Columns.Add("Name", Type.GetType("System.String"))
        PartDs.Tables("PartName").Columns.Add("PartNo", Type.GetType("System.String"))
        PartDs.Tables("PartName").Columns.Add("Unit", Type.GetType("System.String"))

        Dim Qry As String
        Dim StockSpareParent As String = Read_Settings("Stock_Parent_Spare")
        Dim TallyCon As New OdbcConnection(Read_Settings("TallyODBC"))
        Dim Cmd As New OdbcCommand
        Dim Da As New OdbcDataAdapter
        '   Dim Parent As String = Read_Settings("BOOKING_ADV_PARENT")

        Cmd.Connection = TallyCon

        Try

            TallyCon.Open()

            If StockSpareParent = "" Then
                Qry = "Select $NAME as Name,$BASEUNITS as Unit,$ADDITIONALNAME as PartNo from ListofStockItems "
            Else
                Qry = "Select $NAME as Name,$BASEUNITS as Unit,$ADDITIONALNAME as PartNo from ListofStockItems WHERE $PARENT = '" & StockSpareParent & "' "
            End If

            Cmd.CommandText = Qry
            Da.SelectCommand = Cmd
            Da.Fill(PartDs, "PartName")

            TallyCon.Close()
            TallyCon.Dispose()

        Catch ex As Exception

            MsgBox("Failed to load stock items from tally... " & vbCrLf & ex.Message)

        End Try

        If PartDs.Tables("PartName").Rows.Count > 0 Then
            For Each Dr As DataRow In PartDs.Tables("PartName").Rows
                Dr("Name") = Dr("$NAME")
                Dr("Unit") = Dr("$BASEUNITS")
                Dr("PartNo") = Dr("$ADDITIONALNAME")
            Next
        End If

        PartDs.AcceptChanges()

    End Sub

    Private Function CheckPartNames() As Boolean

        Dim DsDetails As New TallyDs
        Dim PartName As String = ""
        Dim Missing_Parts As String = ""

        For Each Dr As TallyDs.Service_Bills_HeadsRow In ImportDs.Service_Bills_Heads.Rows

            DsDetails.Service_Bills.Clear()
            DsDetails.Merge(ImportDs.Service_Bills.Select("Invoice_No = '" & Dr.Invoice_Number & "'", "Id"))

            For Each DrD As TallyDs.Service_BillsRow In DsDetails.Service_Bills.Rows
                If DrD("Part_Labour_Code").ToString.Trim = "" Or DrD("Type") <> "Item" Then
                    Continue For
                End If
                PartName = Get_PartName(DrD("Part_Labour_Code").ToString.Trim)
                If PartName = "" Then
                    Missing_Parts = IIf(Missing_Parts = "", vbCrLf & "BILL:" & Dr.Invoice_Number, Missing_Parts)
                    Missing_Parts += vbCrLf & DrD("Part_Labour_Code").ToString.Trim
                End If
            Next
        Next

        If Missing_Parts <> "" Then
            ' Show_ClipBoard("Missing Parts", Missing_Parts)
            If MessageBox.Show("Do you want to continue?", "Import", MessageBoxButtons.YesNo, Nothing, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
                Return False
            End If
        End If

        Return True

    End Function

    Private Function CheckLabourLedgers() As Boolean

        Dim DsDetails As New TallyDs
        Dim LabourName As String = ""
        Dim Missing_Labour As String = ""

        For Each Dr As TallyDs.ServiceRow In ImportDs.Service.Rows

            DsDetails.Service_Detail.Clear()
            DsDetails.Merge(ImportDs.Service_Detail.Select("Invoice_Number = '" & Dr.Invoice_Number & "'", "Id"))

            For Each DrD As TallyDs.Service_DetailRow In DsDetails.Service_Detail.Rows
                If DrD("Part_Labour_Code").ToString.Trim = "" Or DrD("Type") <> "Labour" Then
                    Continue For
                End If
                Try
                    LabourName = DsLabourLedger.Tables("LabourLedger").Select("$NAME = '" & DrD("Part_Labour_Description") & "'").First.Item("$NAME").ToString
                Catch ex As Exception
                    LabourName = ""
                End Try
                If LabourName = "" Then
                    Missing_Labour = IIf(Missing_Labour = "", vbCrLf & "BILL:" & Dr.Invoice_Number, Missing_Labour)
                    Missing_Labour += vbCrLf & DrD("Part_Labour_Description").ToString.Trim
                End If
            Next
        Next

        If Missing_Labour <> "" Then
            ' Show_ClipBoard("Missing Labour Ledgers", Missing_Labour)
            If MessageBox.Show("Do you want to continue?", "Import", MessageBoxButtons.YesNo, Nothing, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
                Return False
            End If
        End If

        Return True

    End Function

    Public Function Is_Already_Exist(ByVal VoucherNo As String, ByVal VoucherType As String) As Boolean

        Dim Value As String = ""
        Dim Status As Boolean = False

        If PublicTallyDS.Tables.Count > 0 Then
            If PublicTallyDS.Tables("VOUCHER").Rows.Count > 0 Then
                Try
                    Value = PublicTallyDS.Tables("VOUCHER").Select("VOUCHERNUMBER = '" & VoucherNo & "'" &
                                                              "And VOUCHERTYPENAME = '" & VoucherType & "' And ISCANCELLED <> 'Yes'").First.Item("VOUCHERNUMBER").ToString
                    Status = True
                Catch ex As Exception
                End Try
            End If
        End If
        Return Status
    End Function

    Public Function Export_Xml(ByVal FromDate As String, ByVal ToDate As String, ByVal VoucherType As String) As Boolean

        Dim Ds As New DataSet
        Dim i As Integer = 1
        Dim XmlDS As New DataSet
        Dim StrXmldata As String
        Existing_Entry_Checked = False

        Try

            StrXmldata = XmlFormat_Voucher_Register(FromDate, ToDate, VoucherType)
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
            '     CommonDA.Create_Log("Import to Tally", "Import-Error", "Failed to check existing entries in tally...")
            Return False
        End Try

        Return True

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


    Public Function Get_PartName(ByVal PartNo As String) As String
        Dim Name As String = ""
        Try
            Name = PartDs.Tables("PartName").Select("PartNo = '" & PartNo & "'", "").First.Item("Name").ToString
        Catch ex As Exception
        End Try
        'Try
        '    Name = PartDs.Tables("PartName").Select("Name like '*" & PartNo & "*'", "").First.Item("Name").ToString
        'Catch ex As Exception
        'End Try
        Return Name
    End Function

    Public Function Get_PartUnit(ByVal PartNo As String) As String
        Dim Unit As String = ""
        Try
            Unit = PartDs.Tables("PartName").Select("PartNo = '" & PartNo & "'", "").First.Item("Unit").ToString
        Catch ex As Exception
        End Try
        Return Unit
    End Function


    Public Function XmlFormat_Ledger(ByVal LedgerName As String, ByVal Parent As String, Optional ByVal Mobile As String = "") As String

        Dim Xml As String = ""

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
                                        "<LEDGER Action = ""Create"">" &
                                            "<NAME>" & LedgerName & "</NAME>" &
                                            "<PARENT>" & Parent & "</PARENT>" &
                                            "<LEDGERMOBILE>" & Mobile & "</LEDGERMOBILE>" &
                                            "<ISBILLWISEON>Yes</ISBILLWISEON>" &
                                            XmlFormat_Ledger_GST() &
                                        "</LEDGER>" &
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

    Public Function XmlFormat_Ledger_GST() As String

        Dim Xml As String = ""

        Try

            Xml = "<COUNTRYNAME>India</COUNTRYNAME>" &
                  "<COUNTRYOFRESIDENCE>India</COUNTRYOFRESIDENCE>" &
                  "<GSTREGISTRATIONTYPE>Consumer</GSTREGISTRATIONTYPE>" &
                  "<LEDSTATENAME>Kerala</LEDSTATENAME>" &
                  "" &
                  "" &
                  "" &
                  "" &
                  ""

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

    Private Function Create_XML_LEDGERENTRIES(ByVal LEDGERNAME As String, ByVal AMOUNT As String, ByVal COST_CENTER As String, Optional ByVal Ignore_ISDEEMEDPOSITIVE As Boolean = False) As String
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
              COST_CENTER &
              "</LEDGERENTRIES.LIST>"
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

    Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles BtnRefresh.Click
        PleaseWait(True)
        ServiceDs = CommonDA.Get_Summary_Service(DtpFrom.Value, DtpTo.Value)
        find()
        PleaseWait(False)
    End Sub

    Private Sub DtpFrom_ValueChanged(sender As Object, e As EventArgs) Handles DtpFrom.ValueChanged

    End Sub

    Private Sub PrintBtn_Click(sender As Object, e As EventArgs) Handles PrintBtn.Click
        Print_Rpt()
    End Sub

    Private Sub ListView2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView2.SelectedIndexChanged

    End Sub

    Private Sub Print_Rpt()

        If ListView1.SelectedItems.Count = 0 Then
            MsgBox("Please select an item..", MsgBoxStyle.Information, "Information")
            Exit Sub
        End If
        '   id = ListView1.SelectedItems(0).Text
        ' ObjDA = New CommonDA
        Dim RptDs As New TallyDs
        Htp = New Hashtable

        'Dim ObjRptSLI As New CryRptServiceSLI
        'Dim ObjRptSSI As New CryRptServiceSSI
        'Dim ObjRptSPI As New CryRptServiceSPI

        'Dim RptView As FrmCrptRptViewer

        'Dim InvoiceNum As String = ListView1.SelectedItems.Item(0).SubItems(2).Text
        'RptDs.Merge(ServiceDs.Service.Select("Invoice_Number = '" & InvoiceNum & "'"))
        'RptDs.Merge(ServiceDs.Service_Detail.Select("Invoice_Number = '" & InvoiceNum & "'"))


        'If ListView1.SelectedItems(0).SubItems(2).ToString.Contains("SLI") Then
        '    RptView = New FrmCrptRptViewer(ObjRptSLI, RptDs, Htp)
        'ElseIf ListView1.SelectedItems(0).SubItems(2).ToString.Contains("SSI") Then
        '    RptView = New FrmCrptRptViewer(ObjRptSSI, RptDs, Htp)
        'ElseIf ListView1.SelectedItems(0).SubItems(2).ToString.Contains("SPI") Then
        '    RptView = New FrmCrptRptViewer(ObjRptSPI, RptDs, Htp)
        'Else
        '    MsgBox("Invalid Bill")
        '    Exit Sub
        'End If

        'RptView.StartPosition = FormStartPosition.CenterScreen
        'RptView.WindowState = FormWindowState.Maximized
        'RptView.Show()

        ' If ObjDA.UpdatePrintTime(ListView1.SelectedItems(0).SubItems(2).Text()) Then
        '  Else
        ' MsgBox("Failed To Update Time")
        ' End If

    End Sub

End Class
