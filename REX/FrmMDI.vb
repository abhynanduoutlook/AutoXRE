
Imports System.Configuration
Imports System.Threading
Imports System.IO
Imports System.Xml
Imports Microsoft.Office.Interop
Imports System.Net.Mail
Imports System.Reflection
Imports ComponentAce.Compression.ZipForge
Imports System.ComponentModel
Imports System.Text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.pdf.parser

Public Class FrmMDI

    Dim ObjLogin As FrmLogin
    Dim ObjWaiting As FrmWait
    Dim ObjLoading As New FrmLoading
    Dim ObjFrmSpareStock As FrmTallySpareStock
    Dim ObjFrmVehicleStock As FrmVehicleStockGrp

    Dim ObjFrmTallyService As FrmTallyService
    Dim ObjFrmCashRegister As FrmCashRegisterDetails
    Dim objEntryHead As FrmEntryHead
    Dim ObjFrmTallyServiceOnTime As FrmTallyServiceOnTime
    Dim ObjFrmServiceLedgers As FrmServiceLedgers
    Dim ObjFrmTallySparePur As FrmTallySparePur
    Dim ObjFrmSettings As FrmSettings
    Dim ObjFrmtallyVehiclePur As FrmTallyVehiclePur
    Dim MainMenuDt As New CompDS.FormsDataTable
    Public Last_Login_time As DateTime
    Dim SubMenuDS As New CompDS
    Dim DrM As CompDS.FormsRow
    Dim Refresh_Time_InMin As Double = 0.00
    Private WithEvents InvisibleForm As New Form
    Private invisibleReady As Boolean
    Public AppRequest_Remider As Integer = 1
    Dim Current_MainMenu_Index As Integer
    Dim Timer1 As New System.Timers.Timer(6000)
    Dim SR_Settings_Dt As New CompDS.SettingsDataTable
    Dim Path_Not_FoundCount As Integer = 0
    Public ServiePathDt As New DataTable
    Public MultiBranch As Boolean = False
    Dim OnTimeServiceFilePath As String = ""
    Dim OnTimeServiceFIleExt As String = ""
    Dim LastMovedFile As String = ""
    Dim ServiceFilePath As String = ""
    Dim PendingTask_FileName As String = ""
    Dim PendingTaskMatter As String = ""
    Dim SendMail_Date As DateTime
    Dim SendMail_Time As DateTime
    Dim FromEmail As String = ""
    Dim EmailPW As String = ""
    Dim ToEmail1 As String = ""
    Dim ToEmail2 As String = ""
    Dim ToEmail3 As String = ""
    Public App_Ver As String = ""
    Dim FilePaths() As String


    Public Sub PleaseWait(ByVal Show As Boolean)
        If Show Then
            FrmWait.LblProgress.Text = "Please Wait..."
            FrmWait.Show(Me)
            FrmWait.Refresh()
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


    Private Sub Login()

        ObjLogin = New FrmLogin
        ObjLogin.ShowDialog()
        If ObjLogin.DialogResult = DialogResult.Cancel Then
            End
        End If

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
        If Not BGWorker.IsBusy Then
            BGWorker.RunWorkerAsync()
        End If
    End Sub

    Private Sub BGWorker_DoWork(sender As Object, e As DoWorkEventArgs) Handles BGWorker.DoWork
        File_Search()
    End Sub

    Private Sub File_Search()
        Dim Status As String = ""
        Dim BackupPath As String = ""
        Dim Exe As Boolean = True

        Try


            For Each items In FilePaths : If Not Directory.Exists(items) Then : Continue For : End If

                For Each File In Directory.GetFiles(items)

                    If UCase(IO.Path.GetExtension(File)) = ".XLS" Or UCase(IO.Path.GetExtension(File)) = ".XLSX" Then

                        If items.Contains("Service") Then
                            LblStatus(True, "Reading Service Bills")
                            ImportService(File, False)
                        ElseIf items.Contains("Spare") Then
                            LblStatus(True, "Reading Spare Purchase")
                            ImportSparePur(File)
                        ElseIf items.Contains("Vehicle") Then
                            ImportVehiclePur(File)
                        ElseIf items.Contains("CashRegister") Then
                            ImportCashRegister(File)
                        End If

                    End If

                Next

            Next

        Catch ex As Exception
            'Nothing
        End Try


    End Sub


    Public Function LblStatus(ByVal visible As Boolean, ByVal Txt As String)

        If visible Then
            LblServiceStatus.Visible = True
            LblServiceStatus.Text = Txt
            LblServiceStatus.Refresh()
        Else
            LblServiceStatus.Text = Txt
            LblServiceStatus.Visible = False
            LblServiceStatus.Refresh()
        End If


        Return Nothing
    End Function


    Private Function ImportService(ByVal FileName As String, ByVal IsOn_Time As Boolean) As String

        Dim TempDs As New DataSet
        Dim SaveDs As New DataSet
        Dim Status As String = ""
        Dim NewFileName As String = ""

        LblStatus(True, "Reading Service file :" & FileName & "")

        TempDs = ReadFile(FileName, "ServiceData")

        LblStatus(True, "Inserting Data...")

        If Read_Settings("On_Time_ServiceBills") = "Y" And IsOn_Time Then

            Status = CommonDA.Insert_Service_CSV(FileName)

            ' Return Status
        Else
            If TempDs.Tables("ServiceData").Columns.Contains("Invoice Number") And TempDs.Tables("ServiceData").Columns.Contains("Job Card ") Then

                'JobCard
                SaveDs.Merge(TempDs.Tables("ServiceData").Select("", "[Delivered Date Time],[Invoice Number]"))
                LblStatus(True, "Inserting Jobcard Statement...")

                Status = CommonDA.Insert_Service(SaveDs, True, LblServiceStatus)

                If Status Then
                    MsgBox("Job Card Imported Successfully...", MsgBoxStyle.Information, "Import")
                    Try
                        NewFileName = Application.StartupPath & "\BackUp\Serv_" & Format(Date.Now, "ddMMMyy_HHmmss") & ".xls"
                        IO.File.Move(FileName, NewFileName)
                    Catch ex As Exception
                        CommonDA.Create_Log("Import to Tally", "Failed to move " & FileName, "")
                        Return Status

                    End Try

                    LblStatus(True, FileName & "Imported SuccesFully ")
                    LblServiceStatus.Tag = NewFileName
                    Return Status
                Else
                    Try
                        NewFileName = Application.StartupPath & "\BackUp\Serv_Err" & Format(Date.Now, "ddMMMyy_HHmmss") & ".xls"
                        IO.File.Move(FileName, NewFileName)
                    Catch ex As Exception
                        CommonDA.Create_Log("Import to Tally", "Failed to move " & FileName, "")
                        Return Status

                    End Try

                    LblStatus(True, FileName & "Import Error ")
                    LblServiceStatus.Tag = NewFileName
                    Return Status
                End If

                Return Status

            ElseIf TempDs.Tables("ServiceData").Columns.Contains("Invoice") And TempDs.Tables("ServiceData").Columns.Contains("Invoice Date") Then

                'Invoice Details
                SaveDs.Merge(TempDs)
                ' Status = CommonDA.RunQuery("Truncate service_header")
                LblStatus(True, "Inserting Invoice Summary...")

                Status = CommonDA.Insert_Service_Header(SaveDs, LblServiceStatus)

                If Status Then

                    MsgBox("Invoice Details Imported Successfully...", MsgBoxStyle.Information, "Import")
                    Try
                        NewFileName = Application.StartupPath & "\BackUp\Serv_InvSum" & Format(Date.Now, "ddMMMyy_HHmmss") & ".xls"
                        IO.File.Move(FileName, NewFileName)
                    Catch ex As Exception
                        CommonDA.Create_Log("Import to Tally", "Failed to move " & FileName, "")
                    End Try
                    Return Status
                Else
                    MsgBox("Invoice Details Imported Error...", MsgBoxStyle.Critical, "Import")

                    Try
                        NewFileName = Application.StartupPath & "\BackUp\Serv_InvSum_Err" & Format(Date.Now, "ddMMMyy_HHmmss") & ".xls"
                        IO.File.Move(FileName, NewFileName)
                    Catch ex As Exception
                        CommonDA.Create_Log("Import to Tally", "Failed to move " & FileName, "")
                        Return Status

                    End Try

                    LblStatus(True, FileName & "Import Error ")
                    LblServiceStatus.Tag = NewFileName
                    Return Status
                End If

                Return Status

            ElseIf TempDs.Tables("ServiceData").Columns.Contains("Document Date") And TempDs.Tables("ServiceData").Columns.Contains("IGST%") Then

                SaveDs.Merge(TempDs)
                LblStatus(True, "Inserting Warranty  Statement...")
                ' Status = CommonDA.RunQuery("Truncate service_header")
                Status = CommonDA.Insert_Service_WLP(SaveDs, LblServiceStatus)


                If Status Then
                    MsgBox("Warranty  Statement Imported Successfully...", MsgBoxStyle.Information, "Import")
                    Try
                        NewFileName = Application.StartupPath & "\BackUp\Serv_WLP_" & Format(Date.Now, "ddMMMyy_HHmmss") & ".xls"
                        IO.File.Move(FileName, NewFileName)
                    Catch ex As Exception
                        CommonDA.Create_Log("Import to Tally", "Failed to move " & FileName, "")
                    End Try

                    LblStatus(True, FileName & "Imported ")
                    LblServiceStatus.Tag = NewFileName
                Else
                    MsgBox("Warranty  Statement Imported Error...", MsgBoxStyle.Critical, "Import")

                    Try
                        NewFileName = Application.StartupPath & "\BackUp\Serv_WLP_Err" & Format(Date.Now, "ddMMMyy_HHmmss") & ".xls"
                        IO.File.Move(FileName, NewFileName)
                    Catch ex As Exception
                        CommonDA.Create_Log("Import to Tally", "Failed to move " & FileName, "")
                        Return Status

                    End Try

                    LblStatus(True, FileName & "Import Error ")
                    LblServiceStatus.Tag = NewFileName
                    Return Status
                End If
                Return Status


            ElseIf TempDs.Tables("ServiceData").Columns.Contains("Invoice Date") Then

                SaveDs.Merge(TempDs)
                LblStatus(True, "Inserting Sales Statement...")
                ' Status = CommonDA.RunQuery("Truncate service_header")
                Status = CommonDA.Insert_Service_SSI(SaveDs, LblServiceStatus)


                If Status Then
                    MsgBox("Sales Statement Imported Successfully...", MsgBoxStyle.Information, "Import")
                    Try
                        NewFileName = Application.StartupPath & "\BackUp\Serv_SSI_" & Format(Date.Now, "ddMMMyy_HHmmss") & ".xls"
                        IO.File.Move(FileName, NewFileName)
                    Catch ex As Exception
                        CommonDA.Create_Log("Import to Tally", "Failed to move " & FileName, "")
                    End Try
                    LblStatus(True, FileName & "Imported ")
                    LblServiceStatus.Tag = NewFileName
                Else
                    MsgBox("Sales  Statement Imported Error...", MsgBoxStyle.Critical, "Import")

                    Try
                        NewFileName = Application.StartupPath & "\BackUp\Serv_SSI_Err_" & Format(Date.Now, "ddMMMyy_HHmmss") & ".xls"
                        IO.File.Move(FileName, NewFileName)
                    Catch ex As Exception
                        CommonDA.Create_Log("Import to Tally", "Failed to move " & FileName, "")
                        Return Status

                    End Try

                    LblStatus(True, FileName & "Import Error ")
                    LblServiceStatus.Tag = NewFileName

                End If
                Return Status

            ElseIf TempDs.Tables("ServiceData").Columns.Contains("Document Name") And TempDs.Tables("ServiceData").Columns.Contains("Document Date") Then

                SaveDs.Merge(TempDs)
                LblStatus(True, "Inserting Insurance Bills...")
                ' Status = CommonDA.RunQuery("Truncate service_header")
                Status = CommonDA.Insert_Service_ILP(SaveDs, LblServiceStatus)


                If Status Then
                    MsgBox("Insurance Bills Imported Successfully...", MsgBoxStyle.Information, "Import")
                    Try
                        NewFileName = Application.StartupPath & "\BackUp\Serv_ILP_" & Format(Date.Now, "ddMMMyy_HHmmss") & ".xls"
                        IO.File.Move(FileName, NewFileName)
                    Catch ex As Exception
                        CommonDA.Create_Log("Import to Tally", "Failed to move " & FileName, "")
                    End Try
                    LblStatus(True, FileName & "Imported ")
                    LblServiceStatus.Tag = NewFileName
                    Return Status
                Else
                    MsgBox("Insurance  Bills Imported Error...", MsgBoxStyle.Critical, "Import")

                    Try
                        NewFileName = Application.StartupPath & "\BackUp\Serv_ILP_Err_" & Format(Date.Now, "ddMMMyy_HHmmss") & ".xls"
                        IO.File.Move(FileName, NewFileName)
                    Catch ex As Exception
                        CommonDA.Create_Log("Import to Tally", "Failed to move " & FileName, "")
                        Return Status

                    End Try

                    LblStatus(True, FileName & "Import Error ")
                    LblServiceStatus.Tag = NewFileName
                End If
                Return Status

            Else

                ' MsgBox("File Error...", MsgBoxStyle.Critical, "Import")


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

    'ImportCashRegister

    Private Function ImportCashRegister(ByVal FileName As String) As String

        Dim SaveDs As New TallyDs
        Dim Status As String = ""

        LblStatus(True, "Reading File..... ")

        SaveDs = ReadFile(FileName, "Cash_Register")

        'SaveDs.Merge(TempDs)
        LblStatus(True, "Importing File  " & FileName & " ")

        Status = CommonDA.Insert_CashRegister(SaveDs, LblServiceStatus)
        LastMovedFile = ""

        If Status = "True" Then

            Try
                LastMovedFile = Application.StartupPath & "\BackUp\CashReg_" & Format(Date.Now, "ddMMMyy_HHmmss") & ".xls"
                IO.File.Move(FileName, LastMovedFile)
            Catch ex As Exception
                LastMovedFile = ""
                CommonDA.Create_Log("Import To Tally", "Failed To move CashReg_" & FileName, "")
            End Try

            LblStatus(True, FileName & "Import Success ")
            LblServiceStatus.Tag = LastMovedFile


        Else
            Try
                LastMovedFile = Application.StartupPath & "\BackUp\CashReg_Err_" & Format(Date.Now, "ddMMMyy_HHmmss") & ".xls"
                IO.File.Move(FileName, LastMovedFile)
            Catch ex As Exception
                CommonDA.Create_Log("Import To Tally", "Failed To move CashReg_" & FileName, "")
            End Try
            LblStatus(True, FileName & "Import Error ")
            LblServiceStatus.Tag = LastMovedFile
        End If
        Return Status

    End Function

    Private Function ProcessDataSetColumns(Dsimport As DataSet, RowValues As String) As DataSet

        Dim ClNameRow As Boolean = False
        Dim Rowvals() = RowValues.ToString.Split(",")
        Dim TpDs As DataSet = Dsimport
        For Each dr As DataRow In TpDs.Tables(0).Rows

            For Each cl As DataColumn In TpDs.Tables(0).Columns

                For Each col In Rowvals
                    If dr(cl).ToString = col Then
                        cl.ColumnName = col
                        ClNameRow = True
                        Exit For
                    End If
                Next

            Next

            If ClNameRow Then
                GoTo end_of_for
            End If
        Next
end_of_for:




        Return TpDs
    End Function

    Private Function ImportVehiclePur(ByVal FileName As String) As String


        Dim SaveDs As New TallyDs
        Dim Status As String = ""

        LblStatus(True, "Reading File..... ")

        SaveDs = ReadFile(FileName, "PurchaseVehicle")
        'SaveDs.Merge(TempDs)
        LblStatus(True, "Importing File  " & FileName & " ")
        SaveDs = ProcessDataSetColumns(SaveDs, Read_Settings("VehiclePurchase_ColumnOrder"))
        Status = CommonDA.Insert_PurchaseVehicle(SaveDs, LblServiceStatus)
        LastMovedFile = ""
        If Status = "True" Then
            Try
                LastMovedFile = Application.StartupPath & "\BackUp\VehiclePur_" & Format(Date.Now, "ddMMMyy_HHmmss") & ".xls"
                IO.File.Move(FileName, LastMovedFile)
            Catch ex As Exception
                LastMovedFile = ""
                CommonDA.Create_Log("Import To Tally", "Failed To move VehiclePur_" & FileName, "")
            End Try
            LblStatus(True, FileName & "Import Success ")
            LblServiceStatus.Tag = LastMovedFile
        ElseIf Status.Contains(vbCrLf) Then

            Show_ClipBoard("Missing Stock Group", Status)
            Dim MissingGroups As String = ""
            Dim ExistingFlag As Boolean = False
            Status = Status.Replace(vbCrLf, ",")
            Dim StatusArray() As String = Status.Split(",")

            For Each item In StatusArray
                ExistingFlag = False

                If item.Contains("Tax Values Not Found In Vehicle Master") Then
                    Continue For
                End If


                For Each items In MissingGroups.Split(",")
                    If items = item Then
                        ExistingFlag = True
                        Exit For
                    End If

                Next

                If ExistingFlag Then
                Else
                    MissingGroups += item + ","
                End If

            Next


            For Each item In MissingGroups.Split(",")
                If item <> "" Then
                    CommonDA.RunQuery("insert into veh_master (ModelFamily) values('" & item & "')")

                End If
            Next

            VehStockGroup()


        Else
            Try
                LastMovedFile = Application.StartupPath & "\BackUp\VehiclePur_Err_" & Format(Date.Now, "ddMMMyy_HHmmss") & ".xls"
                IO.File.Move(FileName, LastMovedFile)
            Catch ex As Exception
                CommonDA.Create_Log("Import To Tally", "Failed To move VehiclePur_" & FileName, "")
            End Try
            LblStatus(True, FileName & "Import Error ")
            LblServiceStatus.Tag = LastMovedFile
        End If
        Return Status

    End Function

    'Public Sub VehStockGroup()
    '    Dim ObjFrmVehStock As New FrmVehicleStockGrp
    '    ObjFrmVehStock.MdiParent = Me.Parent
    '    ObjFrmVehStock.StartPosition = FormStartPosition.CenterScreen
    '    ObjFrmVehStock.WindowState = FormWindowState.Normal
    '    ObjFrmVehStock.StartPosition = TopMost
    '    ObjFrmVehStock.ShowDialog()
    'End Sub


    Public Sub VehStockGroup()

        Dim ObjFrmVehStock As FrmVehicleStockGrp

        ObjFrmVehStock = New FrmVehicleStockGrp()
        ObjFrmVehStock.ShowDialog(Me)

    End Sub

    Private Function ImportSparePur(ByVal FileName As String) As String

        Dim TempDs As New TallyDs
        Dim Status As Boolean = False
        Dim NewFileName As String = ""
        LblStatus(True, "Reading" & FileName & "")
        TempDs = ReadFile(FileName, "re_spare_purchase")

        Status = CommonDA.Insert_SparePurchase(TempDs, LblServiceStatus)

        If Status Then
            Try
                NewFileName = Application.StartupPath & "\BackUp\Spare_Pur" & Format(Date.Now, "ddMMMyy_HHmmss") & ".Xls"
                IO.File.Move(FileName, NewFileName)
            Catch ex As Exception
                LastMovedFile = ""
                CommonDA.Create_Log("Import To Tally", "Failed To move Spare_Pur " & FileName, "")
            End Try
            MsgBox("Import Spare Purchase Successfully" & vbCrLf & FileName, vbInformation, "Import")

            LblStatus(True, FileName & "   Imported SuccesFully ")
            LblServiceStatus.Tag = NewFileName
        Else
            Try
                NewFileName = Application.StartupPath & "\BackUp\Error_Spare_Pur_" & Format(Date.Now, "ddMMMyy_HHmmss") & ".Xls"
                IO.File.Move(FileName, NewFileName)
            Catch ex As Exception
                CommonDA.Create_Log("Import To Tally", "Failed To move Spare_Pur" & FileName, "")
            End Try
            LblStatus(True, "Error Importing Spare Purchase")
            LblServiceStatus.Tag = NewFileName


        End If

        Return Status.ToString

    End Function


    Private Function ReadFile(ByVal File_Name As String, ByVal Table_Name As String) As DataSet

        Dim Ds As New TallyDs
        Dim wait As FrmWait
        PublicShared.DSt = Ds
        wait = New FrmWait(Table_Name, File_Name)

        Ds = CommonDA.Remove_Null(PublicShared.DSt)

        If Ds.Tables.Count = 0 Then
            'MDI.MyMessageBox("Data not found!")
            Return Nothing
        Else
            Return Ds
        End If

    End Function

    'Public Function Get_Excel_Data(ByVal TableName As String, ByVal File As String) As DataSet

    '    If Not IO.File.Exists(File) Then
    '        ' MdiChildren'MyMessageBox("File not exists..")
    '        Return Nothing
    '        Exit Function
    '    End If

    '    Dim status As Boolean = False
    '    Dim Ds As New DataSet
    '    Dim MyExcel As New Excel.Application
    '    Dim MyWorkBook As Excel.Workbook
    '    Dim WorkSheet As Excel.Worksheet
    '    Dim da As New OleDb.OleDbDataAdapter

    '    MyWorkBook = MyExcel.Workbooks.Open(File, True, True, , , , True, True)
    '    WorkSheet = MyWorkBook.ActiveSheet

    '    If TableName = "" Then TableName = WorkSheet.Name

    '    Try
    '        Dim cnn As New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + File + ";Extended Properties=""Excel 12.0;HDR=YES;IMEX=1;""")
    '        da = New OleDb.OleDbDataAdapter("Select * from [" & WorkSheet.Name & "$]", cnn)
    '        da.Fill(Ds, TableName)
    '        Me.WindowState = FormWindowState.Normal

    '    Catch ex As Exception
    '        MsgBox("Import failed!" & vbCrLf & ex.Message)
    '        Me.WindowState = FormWindowState.Maximized

    '    Finally
    '        da.Dispose()
    '        da = Nothing
    '    End Try

    '    MyExcel.Workbooks.Close()
    '    MyExcel.Quit()

    '    Return Ds

    'End Function

    Private Sub Initlz()
        OnTimeServiceFilePath = Read_Settings("OnTimeServiceFilePath")
        FilePaths = {Read_Settings("ServiceFilePath"), Read_Settings("SparePurFilePath"), Read_Settings("VehiclePurFilePath"), Read_Settings("CashRegisterFilePath")}
        OnTimeServiceFIleExt = Read_Settings("OnTimeServiceFIleExt")


        Reset_Timer()
        ' Dim ObjFrm As New FrmTallyService
        '  ObjFrm.Initlz()
    End Sub



    Private Sub FrmMDI_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If MsgBox("Are you sure want to exit?", MsgBoxStyle.YesNo) <> MsgBoxResult.Yes Then
            e.Cancel = True
        End If
    End Sub


    Private Sub Mouse_focus()
        PanelVmenu.Visible = False
    End Sub

    Private Sub Set_Invisible_Form()

        Me.Size = Screen.PrimaryScreen.WorkingArea.Size
        Me.Location = Screen.PrimaryScreen.WorkingArea.Location

        With InvisibleForm
            .FormBorderStyle = Windows.Forms.FormBorderStyle.None
            .TransparencyKey = .BackColor
            .WindowState = FormWindowState.Maximized
            .Owner = Me
            .ShowInTaskbar = False
            .Show()
        End With

        Me.Activate()
        invisibleReady = True

    End Sub

    Private Sub ReportWorkingAreaChanged() Handles InvisibleForm.Layout
        If invisibleReady Then
            Me.Size = Screen.PrimaryScreen.WorkingArea.Size
            Me.Location = Screen.PrimaryScreen.WorkingArea.Location
        End If
    End Sub


    Private Sub FrmMDI_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        DoubleBuffered = True
        Control.CheckForIllegalCrossThreadCalls = False

        For Each Obj As Control In Me.Controls
            If TypeOf Obj Is MdiClient Then
                AddHandler Obj.MouseHover, AddressOf Mouse_focus
            End If
        Next

        Set_Invisible_Form()
        Get_App_Ver()
        Load_Update(False)

        Create_BkUpFolder()

        '  Dim Ds As New CompDS
        ' Dim ObjDa As New CommonDA
        ' Dim DrF As CompDS.Fin_YearRow
        Dim BranchCode As String = ""

        'DoubleBuffered = True
        'Control.CheckForIllegalCrossThreadCalls = False

        'For Each Obj As Control In Me.Controls
        '    If TypeOf Obj Is MdiClient Then
        '        AddHandler Obj.MouseHover, AddressOf Mouse_focus
        '    End If
        'Next

        'Set_Invisible_Form()

        Dim Ds As CompDS
        Ds = New CompDS
        Dim ObjDa As New CommonDA
        Dim DrF As CompDS.Fin_YearRow
        Dim Dsr As CompDS.UsersRow
        Dim Dbr As CompDS.BranchRow
        'Dim BranchCode As String = ""

        PublicShared.Sys_Mode = ConfigurationManager.AppSettings.Item("Mode").ToString
        PublicShared.Branch_Code = ConfigurationManager.AppSettings.Item("Branch").ToString

        Ds = ObjDa.Load_Inilz_Settings(PublicShared.Sys_Mode, BranchCode)
        PublicShared.Branch_Dt = Ds.Branch
        PublicShared.Settings_Dt = Ds.Settings
        PublicShared.Ledgers_Dt = Ds.Ledgers
        PublicShared.User_Dt = Ds.Users
        PublicShared.Forms_Dt = Ds.Forms
        PublicShared.Mfd_Year_Month_Dt = Ds.Mfd_Year_Month

        If Ds.Users.Rows.Count > 0 Then
            ' DrF = Ds.Fin_Year.Rows(0)
            Dsr = Ds.Users.Rows(0)
            PublicShared.User_Name = Dsr.U_Name
            PublicShared.User_Id = Dsr.UserId
            PublicShared.User_Type = Dsr.Type
        End If

        If Ds.Branch.Rows.Count > 0 Then
            Dbr = Ds.Branch.Rows(0)
            PublicShared.Branch_Id = Dbr.BranchId
            PublicShared.Branch_Code = Dbr.Branch_Code
        End If


        If PublicShared.IsAdmin Or PublicShared.Sys_Mode = "ERA" Or PublicShared.Sys_Mode = "SLC" Then
            PublicShared.Branch_Id = 0
        Else
            Try
                PublicShared.Branch_Id = PublicShared.Branch_Dt.Select("Branch_Code = '" & PublicShared.Branch_Code & "'").First.Item("BranchId")
            Catch ex As Exception
                PublicShared.Branch_Id = 0
            End Try
        End If

        Try
            PublicShared.Company_Info_Dr = Ds.Company.Rows(0)
        Catch ex As Exception
            MsgBox("Failed to load settings...", MsgBoxStyle.Critical, "Load failed")
            End
        End Try

        TimerSysJobs.Enabled = True
        TimerSysJobs.Start()


        'If Ds.Users.Rows(0).Item("Reports").ToString <> "" Then
        '    PublicShared.Settings_Reports = Split(Ds.Users.Rows(0).Item("Reports"), ",")
        'End If

        If Ds.Fin_Year.Rows.Count > 0 Then
            DrF = Ds.Fin_Year.Rows(0)
            PublicShared.Fin_Id = DrF.Fin_YearId
            PublicShared.Fin_Name = DrF.Fin_YearId
            PublicShared.Fin_Start = DrF.Fin_Start
            PublicShared.Fin_End = DrF.Fin_End
        End If


        BgWorkerLoad.RunWorkerAsync()

        Login()
        Initlz_ToolStrip()

        If Not ObjLoading.IsDisposed Then
            ObjLoading.Show(Me)
            ObjLoading.Refresh()
        End If

        Initlz()

        If PublicShared.Sys_Mode = "ER" Or PublicShared.Sys_Mode = "ERA" Then
            '  Load_Enquiry_Rpt()
        ElseIf PublicShared.Sys_Mode = "MAIL" Then
            ' Load_Email_Settings()
        ElseIf PublicShared.Sys_Mode = "ASM" Or PublicShared.Sys_Mode = "SM" Then
            Me.MultiBranch = True
        Else
            'TimerApp_Req.Enabled = True
            'TimerApp_Req.Interval = AppRequest_Remider * 10000
        End If

        PLblUser.Text = "User : " & PublicShared.User_Name & "             " &
                           "Last Login : " & Format(Me.Last_Login_time, "dd-MM-yyyy hh:mm:ss tt")

        ObjDa = Nothing
        Ds = Nothing

    End Sub

    Private Sub Create_BkUpFolder()
        Dim Path = New DirectoryInfo(Application.StartupPath & "\BackUp")
        If (Not Path.Exists) Then
            Path.Create()
        End If

    End Sub

    Private Sub Get_App_Ver()
        App_Ver = Assembly.GetExecutingAssembly().GetName().Version.ToString
    End Sub

    Private Sub CopyAllItems(ByVal fromPath As String, ByVal toPath As String)
        ''Create the target directory if necessary
        Dim toPathInfo = New DirectoryInfo(toPath)
        If (Not toPathInfo.Exists) Then
            toPathInfo.Create()
        End If
        Dim fromPathInfo = New DirectoryInfo(fromPath)
        ''move all files
        For Each file As FileInfo In fromPathInfo.GetFiles()
            file.CopyTo(IO.Path.Combine(toPath, file.Name), True)
        Next
        ''move all folders
        'For Each dir As DirectoryInfo In fromPathInfo.GetDirectories()
        '    dir.MoveTo(Path.Combine(toPath, dir.Name))
        'Next
    End Sub

    Private Function Extract_UpdateZipFile(ByVal ZipFile As String) As Boolean

        Try

            Dim ZipDir As String = Application.StartupPath & "\UpdateX " & Format(Date.Now, "ddMMMyyyy_HHmmss")
            Dim archiver As New ZipForge()

            If File.Exists(ZipFile) Then

                ' The name of the ZIP file to unzip
                archiver.FileName = ZipFile
                ' Open an existing archive
                archiver.OpenArchive(System.IO.FileMode.Open)
                ' Default path for all operations
                archiver.BaseDir = ZipDir
                ' Extract all files from the archive to C:\Temp folder
                archiver.ExtractFiles("*.*")
                ' Close archive
                archiver.CloseArchive()

                CopyAllItems(ZipDir, Application.StartupPath)

                File.Delete(ZipFile)
                Return True

            End If
        Catch ex As Exception
            MsgBox("Error while extracting the UpdateX.zip file.." & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Update")
            Return False
        End Try

        Return Nothing
    End Function

    Private Sub Load_Update(ByVal ShowUptoDate As Boolean)
        Dim UpdateZipFile As String = Application.StartupPath & "\UpdateX.zip"
        Dim Path As String = ""
        If IO.File.Exists(UpdateZipFile) Then
            Extract_UpdateZipFile(UpdateZipFile)
        Else
            Try
                Path = Application.StartupPath & "\UpdateX.exe " & ShowUptoDate
                Microsoft.VisualBasic.Interaction.Shell(Path, AppWinStyle.NormalFocus)
            Catch ex As Exception
                MsgBox("Path error ! UpdateX not found. " & Path, MsgBoxStyle.Critical, "CSMS")
            End Try
        End If

    End Sub


    Public Function IsFileLocked(ByVal FileName As String)
        Dim Locked As Boolean = False
        Try
            Dim fs As FileStream = File.Open(FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None)
            fs.Close()
            fs.Dispose()
        Catch ex As Exception
            Locked = True
        End Try
        Return Locked
    End Function

    Public Sub ShowServiceNotification()
        'Dim ObjFrmNotif As New FrmNotification("Service", "New service entry found...", 10)
        'ObjFrmNotif.Show()
    End Sub

    Private Function Get_First_Value(ByVal Line As String) As String
        Dim Str As String = ""
        Dim Letter As String
        Dim Pos As Integer = 1
        Line = Line.Trim
        Line = Replace(Line, ",", "")

        Do
            Letter = Strings.Mid(Line, Pos, 1)
            Str += Letter
            Pos += 1
            If Pos > Line.Length Then
                Pos = Pos
                Exit Do
            End If
        Loop Until Letter = " "
        Return Str.Trim
    End Function

    Private Function Get_First_Amt(ByVal Line As String) As Double
        Dim Amt As Double
        Dim Amt_Str As String = ""
        Dim Letter As String
        Dim Pos As Integer = 1
        Line = Line.Trim
        Line = Replace(Line, ",", "")

        Do
            Letter = Strings.Mid(Line, Pos, 1)
            Amt_Str += Letter
            Pos += 1
            If Pos > Line.Length Then
                Pos = Pos
                Exit Do
            End If
        Loop Until Letter = " "
        Amt = Val(Amt_Str)
        Return Amt
    End Function

    Private Function Get_Last_Amt(ByVal Line As String) As Double
        Dim Amt As Double
        Dim Amt_Str As String = ""
        Dim Letter As String
        Dim Pos As Integer
        Line = Line.Trim
        Line = Replace(Line, ",", "")

        Pos = Line.Length
        Do
            Letter = Strings.Mid(Line, Pos, 1)
            Amt_Str += Letter
            Pos -= 1
            If Pos = 0 Then
                Pos = Pos
                Exit Do
            End If
        Loop Until Letter = " "
        Amt_Str = Strings.StrReverse(Amt_Str)
        Amt = Val(Amt_Str)
        Return Amt
    End Function

    Private Sub releaseObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub

    Private Sub BtnServiceReg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        '  Load_Service_Reg()
    End Sub

    Private Sub BtnSC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Load_ScDetails()
    End Sub

    Private Sub BackupToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        '  Load_Backup()
    End Sub

    Private Sub BtnPendingTasks_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Load_Pending_Tasks()
    End Sub

    Private Sub BgWorkerLoad_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BgWorkerLoad.DoWork
        'Initlz()
    End Sub

    Private Sub UsersToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' Load_Users()
    End Sub

    Private Sub BtnServiceReg_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs)
        '' ShowServiceNotification()
    End Sub

    Private Sub BgWorkerLoad_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BgWorkerLoad.RunWorkerCompleted
        ObjLoading.Dispose()
    End Sub


    Private Sub BgWrkApp_Request_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BgWrkApp_Request.DoWork

        Try
            'If ObjApp_Request.Visible = False Then
            '    AppReqDt.Clear()
            '    ' AppReqDt = CommonDA.Get_App_Request(Nothing, True).App_Request
            'End If
        Catch ex As Exception
        End Try

    End Sub

    Private Sub TimerApp_Req_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerApp_Req.Tick
        TimerApp_Req.Interval = AppRequest_Remider * 10000
        'If ObjApp_Request.Visible = False And AppReqDt.Rows.Count > 0 Then
        '    App_request_Notification()
        'End If
        If Not BgWrkApp_Request.IsBusy Then
            BgWrkApp_Request.RunWorkerAsync()
        End If
    End Sub

    Private Sub Initlz_ToolStrip()
        SubMenuDS.Merge(PublicShared.Forms_Dt.Select("UserType = '" & PublicShared.User_Type & "' And Active = 1", "Orders"))
        MainMenuDt.Merge(SubMenuDS.Forms.DefaultView.ToTable(True, "Frm_Groups"))
        Cleate_MainMenu()
    End Sub


    Public Sub Menu_Click(ByVal s As Object, ByVal e As EventArgs)


        If s.name = "FrmTallyServiceOnTime" Then
            Load_TallyServiceOnTime()
        ElseIf s.name = "FrmTallyService" Then
            Load_TallyService()
        ElseIf s.name = "FrmTallyVehiclePur" Then
            Load_TallyVehiclePur()
        ElseIf s.name = "FrmTallySparePur" Then
            Load_TallySparePur()
        ElseIf s.name = "FrmSettings" Then
            Load_Settings()
        ElseIf s.name = "FrmLedgers" Then
            Load_Ledgers()
        ElseIf s.name = "FrmServiceLedgers" Then
            Load_ServiceLedgers()
        ElseIf s.name = "FrmVehicleStockGrp" Then
            Load_VehicleStockGrp()
        ElseIf s.name = "FrmTallySpareStock" Then
            Load_SpareStock()
        ElseIf s.name = "FrmCashRegisterDetails" Then
            Load_CashRegister()
        ElseIf s.name = "FrmEntryHead" Then
            Load_EntryHead()
        ElseIf s.Name = "AboutUs" Then
            Process.Start(Read_Settings("ReviewLink"))
        End If


    End Sub

    Private Sub Load_SpareStock()
        If ObjFrmSpareStock Is Nothing Then
            ObjFrmSpareStock = New FrmTallySpareStock()
            ObjFrmSpareStock.MdiParent = Me
            ObjFrmSpareStock.Show()
        ElseIf ObjFrmSpareStock.IsDisposed Then
            ObjFrmSpareStock = New FrmTallySpareStock()
            ObjFrmSpareStock.MdiParent = Me
            ObjFrmSpareStock.Show()
        Else
            ObjFrmSpareStock.BringToFront()
        End If
    End Sub

    Private Sub Load_VehicleStockGrp()
        If ObjFrmVehicleStock Is Nothing Then
            ObjFrmVehicleStock = New FrmVehicleStockGrp()
            ObjFrmVehicleStock.MdiParent = Me
            ObjFrmVehicleStock.Show()
        ElseIf ObjFrmVehicleStock.IsDisposed Then
            ObjFrmVehicleStock = New FrmVehicleStockGrp()
            ObjFrmVehicleStock.MdiParent = Me
            ObjFrmVehicleStock.Show()
        Else
            ObjFrmVehicleStock.BringToFront()
        End If
    End Sub

    Private Sub Load_ServiceLedgers()
        If ObjFrmServiceLedgers Is Nothing Then
            ObjFrmServiceLedgers = New FrmServiceLedgers()
            ObjFrmServiceLedgers.MdiParent = Me
            ObjFrmServiceLedgers.Show()
        ElseIf ObjFrmServiceLedgers.IsDisposed Then
            ObjFrmServiceLedgers = New FrmServiceLedgers()
            ObjFrmServiceLedgers.MdiParent = Me
            ObjFrmServiceLedgers.Show()
        Else
            ObjFrmServiceLedgers.BringToFront()
        End If
    End Sub

    Private Sub Load_Ledgers()
        If ObjFrmSettings Is Nothing Then
            ObjFrmSettings = New FrmSettings(True)
            ObjFrmSettings.MdiParent = Me
            ObjFrmSettings.Show()
        ElseIf ObjFrmSettings.IsDisposed Then
            ObjFrmSettings = New FrmSettings(True)
            ObjFrmSettings.MdiParent = Me
            ObjFrmSettings.Show()
        Else
            ObjFrmSettings.BringToFront()
        End If
    End Sub

    Private Sub Load_Settings()
        If ObjFrmSettings Is Nothing Then
            ObjFrmSettings = New FrmSettings(False)
            ObjFrmSettings.MdiParent = Me
            ObjFrmSettings.Show()
        ElseIf ObjFrmSettings.IsDisposed Then
            ObjFrmSettings = New FrmSettings(False)
            ObjFrmSettings.MdiParent = Me
            ObjFrmSettings.Show()
        Else
            ObjFrmSettings.BringToFront()
        End If
    End Sub

    Private Sub Load_TallySparePur()
        If ObjFrmtallysparepur Is Nothing Then
            ObjFrmtallysparepur = New FrmTallySparePur()
            ObjFrmtallysparepur.MdiParent = Me
            ObjFrmtallysparepur.Show()
        ElseIf ObjFrmtallysparepur.IsDisposed Then
            ObjFrmtallysparepur = New FrmTallySparePur()
            ObjFrmtallysparepur.MdiParent = Me
            ObjFrmtallysparepur.Show()
        Else
            ObjFrmtallysparepur.BringToFront()
        End If
    End Sub


    Private Sub Load_TallyVehiclePur()
        If ObjFrmtallyVehiclePur Is Nothing Then
            ObjFrmtallyVehiclePur = New FrmTallyVehiclePur()
            ObjFrmtallyVehiclePur.MdiParent = Me
            ObjFrmtallyVehiclePur.Show()
        ElseIf ObjFrmtallyVehiclePur.IsDisposed Then
            ObjFrmtallyVehiclePur = New FrmTallyVehiclePur
            ObjFrmtallyVehiclePur.MdiParent = Me
            ObjFrmtallyVehiclePur.Show()
        Else
            ObjFrmtallyVehiclePur.BringToFront()
        End If
    End Sub


    Private Sub Load_TallyService()
        If ObjFrmTallyService Is Nothing Then
            ObjFrmTallyService = New FrmTallyService(LblServiceStatus)
            ObjFrmTallyService.MdiParent = Me
            ObjFrmTallyService.Show()
        ElseIf ObjFrmTallyService.IsDisposed Then
            ObjFrmTallyService = New FrmTallyService(LblServiceStatus)
            ObjFrmTallyService.MdiParent = Me
            ObjFrmTallyService.Show()
        Else
            ObjFrmTallyService.BringToFront()
        End If
    End Sub


    Private Sub Load_TallyServiceOnTime()
        If ObjFrmTallyServiceOnTime Is Nothing Then
            ObjFrmTallyServiceOnTime = New FrmTallyServiceOnTime()
            ObjFrmTallyServiceOnTime.MdiParent = Me
            ObjFrmTallyServiceOnTime.Show()
        ElseIf ObjFrmTallyServiceOnTime.IsDisposed Then
            ObjFrmTallyServiceOnTime = New FrmTallyServiceOnTime
            ObjFrmTallyServiceOnTime.MdiParent = Me
            ObjFrmTallyServiceOnTime.Show()
        Else
            ObjFrmTallyServiceOnTime.BringToFront()
        End If
    End Sub

    Private Sub Load_CashRegister()
        If ObjFrmCashRegister Is Nothing Then
            ObjFrmCashRegister = New FrmCashRegisterDetails()
            ObjFrmCashRegister.MdiParent = Me
            ObjFrmCashRegister.Show()
        ElseIf ObjFrmCashRegister.IsDisposed Then
            ObjFrmCashRegister = New FrmCashRegisterDetails
            ObjFrmCashRegister.MdiParent = Me
            ObjFrmCashRegister.Show()
        Else
            ObjFrmCashRegister.BringToFront()
        End If
    End Sub

    Private Sub Load_EntryHead()
        If objEntryHead Is Nothing Then
            objEntryHead = New FrmEntryHead()
            objEntryHead.MdiParent = Me
            objEntryHead.Show()
        ElseIf objEntryHead.IsDisposed Then
            objEntryHead = New FrmEntryHead()
            objEntryHead.MdiParent = Me
            objEntryHead.Show()
        Else
            objEntryHead.BringToFront()
        End If
    End Sub



    'Private Sub Load_BkReceiptApproval()
    '    If ObjFrmBkReceiptApproval Is Nothing Then
    '        ObjFrmBkReceiptApproval = New FrmBkReceiptApproval("", "", "")
    '        ObjFrmBkReceiptApproval.MdiParent = Me
    '        ObjFrmBkReceiptApproval.Show()
    '    ElseIf ObjFrmBkReceiptApproval.IsDisposed Then
    '        ObjFrmBkReceiptApproval = New FrmBkReceiptApproval("", "", "")
    '        ObjFrmBkReceiptApproval.MdiParent = Me
    '        ObjFrmBkReceiptApproval.Show()
    '    Else
    '        ObjFrmBkReceiptApproval.BringToFront()
    '    End If
    'End Sub

    Private Sub V_MouseEnter(ByVal sender As Object, ByVal e As EventArgs)
        Open_Btn_H(sender)
    End Sub

    Private Sub Cleate_MainMenu()
        MenuStripMain.Items.Clear()
        Dim max_lenght As Integer = 0
        Dim i As Integer = 0
        For Each Dr As DataRow In MainMenuDt.Rows
            Dim ObjMain As New ToolStripMenuItem(UCase(Dr("Frm_Groups")), Nothing)
            ObjMain.AutoSize = False
            ObjMain.Size = New Size(BtnMenuTemplate.Size)
            ObjMain.Name = Dr("Frm_Groups")
            ObjMain.Tag = i
            MenuStripMain.Items.Add(ObjMain)
            AddHandler ObjMain.MouseEnter, AddressOf V_MouseEnter
            AddHandler ObjMain.Click, AddressOf V_MouseEnter
            'AddHandler ObjMain.MouseHover, AddressOf V_MouseEnter

            ' ObjMain.GotFocus()
            i += 1
        Next

        MenuStripMain.Renderer = New MyRenderer()
    End Sub

    Private Sub Open_Btn_H(ByRef Obj_H_Button As ToolStripMenuItem)

        PanelVmenu.Size = New Size(0, 0)
        PanelVmenu.Controls.Clear()
        PanelVmenu.Refresh()
        PanelVmenu.Visible = True
        PanelVmenu.Location = New Point(Obj_H_Button.Bounds.X, Obj_H_Button.Bounds.Y + Obj_H_Button.Bounds.Height)

        Dim Location As New Point(0, 0)
        Dim Height As Integer = 0
        Dim i As Integer

        For Each Me.DrM In SubMenuDS.Forms.Select("Frm_Groups = '" & Obj_H_Button.Name & "'", "")
            Dim ObjButton As New Button
            'UCase(DrM("Frm_Text"))
            MyMenuButton(ObjButton, DrM("Frm_Name"), UCase(DrM("Frm_Text")), i)
            PanelVmenu.Controls.Add(ObjButton)
            ObjButton.Location = New Point(Location.X, Location.Y)
            AddHandler ObjButton.Click, AddressOf Menu_Click
            AddHandler ObjButton.KeyUp, AddressOf Button_KeyDown
            Location = New Point(ObjButton.Location.X, ObjButton.Location.Y + ObjButton.Height)
            Height += ObjButton.Height
            ' PanelVmenu.Size = New Size(PanelVmenu.Size.Width, Height)
        Next

        PanelVmenu.Size = New Size(PanelVmenu.PreferredSize.Width - 2, Height)

        'For i = 0 To Height
        '    PanelVmenu.Size = New Size(PanelVmenu.PreferredSize.Width - 2, i)
        '    PanelVmenu.Refresh()
        '    'Threading.Thread.Sleep(0.5)
        '    'i += 3
        'Next

        Current_MainMenu_Index = Val(Obj_H_Button.Tag)

        PanelVmenu.Focus()
        PanelVmenu.Controls(0).Focus()

    End Sub

    Private Sub Button_Got_Focus(ByVal sender As Object, ByVal e As EventArgs)
        sender.backcolor = BtnMenuTemplate.BackColor
    End Sub

    Private Sub Button_Lost_Focus(ByVal sender As Object, ByVal e As EventArgs)
        sender.backcolor = BtnMenuTemplate.FlatAppearance.MouseOverBackColor
        Try
            If Me.ActiveMdiChild.Name <> "" And PanelVmenu.ContainsFocus = False Then
                PanelVmenu.Visible = False
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button_KeyDown(ByVal sender As Object, ByVal e As EventArgs)
        Dim obj As Object = e
        If obj.KeyCode = Keys.Right Or obj.KeyCode = Keys.Left Then
            PanelVmenu.Visible = False
            MenuStripMain.Focus()
            If obj.KeyCode = Keys.Right Then
                If Current_MainMenu_Index >= MenuStripMain.Items.Count - 1 Then
                    Current_MainMenu_Index = 0
                    MenuStripMain.Items(0).Select()
                Else
                    Current_MainMenu_Index += 1
                    MenuStripMain.Items(Current_MainMenu_Index).Select()
                End If
            ElseIf obj.KeyCode = Keys.Left Then
                If Current_MainMenu_Index = 0 Then
                    Current_MainMenu_Index = MenuStripMain.Items.Count - 1
                    MenuStripMain.Items(Current_MainMenu_Index).Select()
                Else
                    Current_MainMenu_Index += -1
                    MenuStripMain.Items(Current_MainMenu_Index).Select()
                End If
            End If
        End If

        obj = Nothing
    End Sub

    Private Sub MyMenuButton(ByRef ObjButton As Button, ByVal Name As String, ByVal Text As String, ByVal Index As Integer)
        ObjButton.AutoSize = False
        ObjButton.Name = Name
        ObjButton.Text = Text
        ObjButton.Tag = Name
        ObjButton.Font = BtnMenuTemplate.Font
        ObjButton.ForeColor = BtnMenuTemplate.ForeColor
        ObjButton.BackColor = BtnMenuTemplate.FlatAppearance.MouseOverBackColor
        ObjButton.FlatAppearance.BorderSize = BtnMenuTemplate.FlatAppearance.BorderSize
        ObjButton.FlatAppearance.MouseOverBackColor = BtnMenuTemplate.BackColor
        ObjButton.FlatStyle = BtnMenuTemplate.FlatStyle
        ObjButton.TextAlign = ContentAlignment.MiddleLeft
        ObjButton.Size = BtnMenuTemplate.Size
        AddHandler ObjButton.GotFocus, AddressOf Button_Got_Focus
        AddHandler ObjButton.LostFocus, AddressOf Button_Lost_Focus
    End Sub

    Public Class MyRenderer
        Inherits ToolStripProfessionalRenderer
        Protected Overloads Overrides Sub OnRenderMenuItemBackground(ByVal e As ToolStripItemRenderEventArgs)
            Try
                Dim rc As New Rectangle(Point.Empty, e.Item.Size)
                Dim c As Color
                'Color.FromArgb(159, 216, 222)
                c = IIf(e.Item.Selected, Color.Teal, Color.DarkCyan)
                Using brush As New SolidBrush(c)
                    e.Graphics.FillRectangle(brush, rc)
                End Using
                e.Item.Padding = New Padding(0, 5, 0, 5)
                e.Item.TextAlign = ContentAlignment.MiddleCenter
                e.Item.ForeColor = Color.White
            Catch ex As Exception
            End Try
        End Sub
    End Class


    Private Sub PanelVmenu_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles PanelVmenu.MouseLeave
        PanelVmenu.Visible = False
    End Sub

    Private Sub FrmMDI_MdiChildActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.MdiChildActivate
        PanelVmenu.Visible = False
    End Sub

    Private Sub BtnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnExit.Click
        Me.Close()
    End Sub

    Private Sub BtnMin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnMin.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub MenuStripMain_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MenuStripMain.KeyDown
        If e.KeyCode = Keys.Down Then

        End If
    End Sub

    Private Sub TimerSendMail_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerSendMail.Tick
        If Not BgWrkSendMail.IsBusy Then
            BgWrkSendMail.RunWorkerAsync()
        End If
    End Sub




    Private Sub TimerSysJobs_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerSysJobs.Tick
        If Not BgWrkSysJobs.IsBusy Then
            TimerSysJobs.Interval = 30000
            BgWrkSysJobs.RunWorkerAsync()
        End If
    End Sub


    Private Sub BgWrkSysJobs_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BgWrkSysJobs.DoWork
        PublicShared.Server_Date = CommonDA.Get_Server_Time(Read_Settings("UTC_Add_Time"))
    End Sub


    Public Function Read_Settings(ByVal S_Key As String) As String
        Dim Value As String = ""
        Try
            Value = PublicShared.Settings_Dt.Select("S_Key='" & S_Key & "'").First.Item("S_Value").ToString
        Catch ex As Exception
        End Try
        Return Value
    End Function

    Private Sub PanelVmenu_Paint(sender As Object, e As PaintEventArgs) Handles PanelVmenu.Paint

    End Sub

    Private Sub FrmMDI_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd

    End Sub

    Private Sub FrmMDI_GotFocus(sender As Object, e As EventArgs) Handles Me.GotFocus

    End Sub

    Private Sub LblServiceStatus_Click(sender As Object, e As EventArgs) Handles LblServiceStatus.Click
        Try
            If File.Exists(LblServiceStatus.Tag) Then
                Process.Start(LblServiceStatus.Tag)
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
