
Imports MySql.Data.MySqlClient
Imports System.Configuration
Imports Microsoft.Office.Interop
Imports System.Text.RegularExpressions

Public Class CommonDA

    Public Shared ReadOnly Property ConnectionString() As String
        Get
            Dim strConn As String
            strConn = ConfigurationManager.ConnectionStrings("DB").ToString
            strConn = Replace(strConn, "*", "")
            Return strConn
        End Get
    End Property

    Public Shared Function ReplaceQuote(ByVal str As String) As String

        Return Replace(str, "'", "''")
    End Function

    Public Shared Function ReplaceRightSlash(ByVal str As String) As String
        If str <> "" Then
            Return Replace(str, "\", "/")
        Else
            Return ""
        End If
    End Function


    Public Shared Function Remove_Null(ByVal Ds As DataSet, Optional ByVal Trim As Boolean = False) As DataSet

        For Each Dt As DataTable In Ds.Tables
            For Each dr As DataRow In Dt.Rows
                For Each DtColmn As DataColumn In Dt.Columns
                    If IsDBNull(dr.Item(DtColmn)) Then
                        If DtColmn.DataType.FullName = "System.DateTime" Then
                            ' dr.Item(DtColmn) = Date.MaxValue
                        ElseIf DtColmn.DataType.FullName = "System.Double" Then
                            dr.Item(DtColmn) = 0
                        ElseIf DtColmn.DataType.FullName = "System.Decimal" Then
                            dr.Item(DtColmn) = 0
                        ElseIf DtColmn.DataType.FullName = "System.String" Then
                            dr.Item(DtColmn) = ""
                        ElseIf DtColmn.DataType.FullName = "System.Int16" Or DtColmn.DataType.FullName = "System.Int32" Or DtColmn.DataType.FullName = "System.Int64" Then
                            dr.Item(DtColmn) = 0
                        ElseIf DtColmn.DataType.FullName = "System.Byte[]" Then
                            dr.Item(DtColmn) = 0
                        ElseIf DtColmn.DataType.FullName = "System.Boolean" Then
                            dr.Item(DtColmn) = False
                        Else
                            dr.Item(DtColmn) = ""
                        End If
                    ElseIf Trim Then
                        dr.Item(DtColmn) = dr.Item(DtColmn).ToString.Trim
                    End If
                Next
            Next
        Next

        Return Ds
    End Function

    Public Shared Function RunQuery(ByVal strQuery As String) As Boolean

        Dim cn As New MySqlConnection(ConnectionString)
        Dim cmd As New MySqlCommand
        Dim Tr As MySqlTransaction
        Dim Status As Boolean

        cmd.Connection = cn
        cn.Open()

        Tr = cn.BeginTransaction(IsolationLevel.ReadCommitted)
        cmd.Transaction = Tr

        Try

            cmd.CommandText = strQuery
            cmd.ExecuteNonQuery()

            Tr.Commit()
            Tr.Dispose()

            Status = True

            cn.Close()
            cn.Dispose()
            cn = Nothing

        Catch ex As Exception
            Status = False
            If Not Tr Is Nothing Then
                Tr.Rollback()
            End If
            cn.Close()
            cn.Dispose()
            cn = Nothing
            Return False
        End Try

        Return Status

    End Function

    Public Sub DB_Updates()

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        Dim strQuery As String = ""

        cmd.Connection = cn
        cn.Open()

        'Try
        '    strQuery = " Alter Table enquiry_header Add column BookedOk tinyint(3) Default '0'; "
        '    cmd.CommandText = strQuery
        '    cmd.ExecuteNonQuery()
        'Catch ex As Exception
        'End Try

        cn.Close()
        cn = Nothing

    End Sub

    Public Function Get_branch(ByVal branchId As Integer, ByVal branch_Code As String, ByVal branch_Name As String, ByVal Active As Boolean) As CompDS

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim da As New MySqlDataAdapter
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Ds As New CompDS
        Dim Id As Integer = 0

        cmd.Connection = cn
        cn.Open()

        strQuery = " select branchId,branch_Code,branch_Name,branch_Address,branch_Order,Active  from branch where 1=1 "


        If branchId > 0 Then
            strQuery = strQuery & " And branchId = '" & branchId & "'"
        End If

        If branch_Name.Trim <> "" Then
            strQuery = strQuery & " And branch_Name = '" & ReplaceQuote(branch_Name) & "'"
        End If

        If branch_Code.Trim <> "" Then
            strQuery = strQuery & " And branch_Code = '" & ReplaceQuote(branch_Code) & "'"
        End If

        If Active Then
            strQuery = strQuery & " And Active = 1"
        End If

        strQuery = strQuery & " Order by branch_Order "

        da.SelectCommand = cmd
        cmd.CommandText = strQuery
        da.Fill(Ds, "branch")


        cn.Close()
        cn = Nothing
        Return Ds

    End Function

    Public Function Get_FinYear(ByVal Fin_YearId As Integer, ByVal Fin_Desc As String, ByVal Active As Boolean) As CompDS

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim da As New MySqlDataAdapter
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Ds As New CompDS
        Dim Id As Integer = 0

        cmd.Connection = cn
        cn.Open()

        strQuery = " select Fin_YearId,Fin_Start,Fin_End,Fin_Desc,Active  from fin_year where 1=1 "


        If Fin_YearId > 0 Then
            strQuery = strQuery & " And Fin_YearId = '" & Fin_YearId & "'"
        End If

        If Fin_Desc.Trim <> "" Then
            strQuery = strQuery & " And Fin_YearName = '" & Fin_Desc & "'"
        End If

        If Active Then
            strQuery = strQuery & " And Active = 1"
        End If

        da.SelectCommand = cmd
        cmd.CommandText = strQuery
        da.Fill(Ds, "fin_year")


        cn.Close()
        cn = Nothing
        Return Ds

    End Function

    Public Function Get_Company() As CompDS

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim da As New MySqlDataAdapter
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Ds As New CompDS
        Dim Id As Integer = 0

        cmd.Connection = cn
        cn.Open()

        strQuery = " select Name,Place,Address,Phone,Desc1,Desc2,Code,District  from company "


        da.SelectCommand = cmd
        cmd.CommandText = strQuery
        da.Fill(Ds, "company")


        cn.Close()
        cn = Nothing
        Return Ds

    End Function

    'Public Function Get_Insur_Company(ByVal Insur_CompId As Integer) As ServDS

    '    Dim cn As New MySqlConnection(CommonDA.ConnectionString)
    '    Dim da As New MySqlDataAdapter
    '    Dim cmd As New MySqlCommand
    '    Dim strQuery As String
    '    Dim Ds As New ServDS
    '    Dim Id As Integer = 0

    '    cmd.Connection = cn
    '    cn.Open()

    '    strQuery = " select Insur_CompId,Insur_Comp_Name,Address from insur_company Where 1 = 1 "

    '    If Insur_CompId > 0 Then
    '        strQuery = strQuery & " And Insur_CompId = '" & Insur_CompId & "'"
    '    End If

    '    da.SelectCommand = cmd
    '    cmd.CommandText = strQuery
    '    da.Fill(Ds, "serv_claims")


    '    cn.Close()
    '    cn = Nothing
    '    Return Ds

    'End Function Public Function Get_Settings(ByVal htp As Hashtable, Is_Ledger As Boolean) As CompDS

    Public Shared Function Get_LastId(ByVal Prefix As String) As TallyDs.Service_HeaderRow


        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim da As New MySqlDataAdapter
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Ds As New TallyDs
        Dim Dr As TallyDs.Service_HeaderRow
        Dim Id As Integer = 0

        Try
            cmd.Connection = cn
            cn.Open()






            strQuery = "select JobCard_No as Job_Card,Inv_Number as Invoice_Number,JobCard_Date as Invoice_Date from service_bills where Id =(select max(Id) from service_bills)"
            'where left(Inv_Number,9 )='" & Prefix & "'"
            da.SelectCommand = cmd
            cmd.CommandText = strQuery

            da.Fill(Ds, "Service_Header")
            Dr = Ds.Service_Header.Rows(0)
            Return Dr


        Catch ex As Exception

            Dr = Ds.Service_Header.Rows.Add
            cn = Nothing
            Return Dr
        End Try

        cn.Close()
        cn = Nothing

        Return Nothing

    End Function

    Public Shared Function Get_NextId(ByVal Prefix As String) As String


        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim da As New MySqlDataAdapter
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Ds As New TallyDs
        Dim Dr As TallyDs.Service_HeaderRow
        Dim Id As Integer = 0
        Dim NextId As String = ""

        Try
            cmd.Connection = cn
            cn.Open()


            strQuery = "Select ifnull(Right(Inv_Number, Length(Inv_Number) - 9), 0) As Invoice_No from service_bills where left(Inv_Number,9 )='" & Prefix & "' order by Invoice_No desc"
            da.SelectCommand = cmd
            cmd.CommandText = strQuery
            NextId = cmd.ExecuteScalar()

            NextId = Prefix & Format(Val(NextId) + 1, "00000")

        Catch ex As Exception
            cn.Dispose()
            cn = Nothing
        End Try

        Return NextId

    End Function



    Public Function Get_Settings(ByVal htp As Hashtable, Is_Ledger As Boolean) As CompDS

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim da As New MySqlDataAdapter
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Ds As New CompDS
        Dim Id As Integer = 0

        cmd.Connection = cn
        cn.Open()

        Try
            If Is_Ledger <> True Then

                strQuery = " select Id,S_Key,ifnull(S_Value,0) as S_Value,Active_Value,Branch from settings Where 1 = 1 "
                If Not htp Is Nothing Then
                    For Each Key As Object In htp.Keys
                        Select Case Key
                            Case CompDS.SearchSettingsBy.S_Key
                                strQuery = strQuery & " And S_Key like '%" & CommonDA.ReplaceQuote(htp(Key)) & "%'"
                            Case CompDS.SearchSettingsBy.S_Value
                                strQuery = strQuery & " And S_Value like '%" & CommonDA.ReplaceQuote(htp(Key)) & "%'"
                        End Select
                    Next
                End If

                da.SelectCommand = cmd
                cmd.CommandText = strQuery
                da.Fill(Ds, "settings")
            Else

                strQuery = " select Id,S_Key,S_Value,Active_Value,Branch from ledgers Where 1 = 1 "
                If Not htp Is Nothing Then
                    For Each Key As Object In htp.Keys
                        Select Case Key
                            Case CompDS.SearchSettingsBy.S_Key
                                strQuery = strQuery & " And S_Key like '%" & CommonDA.ReplaceQuote(htp(Key)) & "%'"
                            Case CompDS.SearchSettingsBy.S_Value
                                strQuery = strQuery & " And S_Value like '%" & CommonDA.ReplaceQuote(htp(Key)) & "%'"
                        End Select
                    Next
                End If


                da.SelectCommand = cmd
                cmd.CommandText = strQuery
                da.Fill(Ds, "ledgers")

            End If

        Catch ex As Exception
            cn.Close()
            cn = Nothing
            Return Nothing

        End Try

        cn.Close()
        cn = Nothing
        Return Ds

    End Function

    Public Function Get_Serv_Labour_Settings(ByVal htp As Hashtable) As TallyDs

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim da As New MySqlDataAdapter
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Ds As New TallyDs
        Dim Id As Integer = 0

        cmd.Connection = cn
        cn.Open()


        Try

            strQuery = " select Id,PartNo,PartDescription,LedgerName from Service_Ledgers Where 1 = 1 "
            If Not htp Is Nothing Then
                For Each Key As Object In htp.Keys
                    Select Case Key
                        Case TallyDs.SearchServLabourSettingsBy.LedgerName
                            strQuery = strQuery & " And LedgerName like '%" & CommonDA.ReplaceQuote(htp(Key)) & "%'"
                        Case TallyDs.SearchServLabourSettingsBy.PartDescription
                            strQuery = strQuery & " And PartDescription like '%" & CommonDA.ReplaceQuote(htp(Key)) & "%'"
                        Case TallyDs.SearchServLabourSettingsBy.PartNo
                            strQuery = strQuery & " And PartNo like '%" & CommonDA.ReplaceQuote(htp(Key)) & "%'"
                    End Select
                Next
            End If

            da.SelectCommand = cmd
            cmd.CommandText = strQuery
            da.Fill(Ds, "Service_Ledgers")


        Catch ex As Exception
            cn = Nothing
        End Try

        cn.Close()
        cn = Nothing
        Return Ds

    End Function


    Public Function Update_Settings_Active_Value(ByVal Key As String, ByVal Active_Value As String) As Boolean

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Id As Integer = 0

        cmd.Connection = cn
        cn.Open()

        Try
            strQuery = " Update settings Set Active_Value = '" & Active_Value & "' where S_Key = '" & Key & "'"
            cmd.CommandText = strQuery
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            cn = Nothing
        End Try

        cn.Close()
        cn = Nothing

        Return True
    End Function

    Public Function Load_Inilz_Settings(SysMode As String, Branch_Code As String) As CompDS


        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim da As New MySqlDataAdapter
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Ds As New CompDS
        Dim Db As String = ""
        Try

            cmd.Connection = cn
            cn.Open()
            'Dim Dv() As String = CommonDA.ConnectionString.Split(";")
            'Db = Dv(2).Replace("DATABASE=", "")
            'Db = "CREATE DATABASE IF NOT EXISTS "
            'cmd = New MySqlCommand(Db, cn)
            'cmd.ExecuteNonQuery()


            'Company
            strQuery = " select Name,Place,Address,Phone,Desc1,Desc2,Code,District  from company "
            cmd.CommandText = strQuery
            da.SelectCommand = cmd
            da.Fill(Ds, "company")

            'branch
            strQuery = " select branchId,branch_Code,branch_Name,branch_Address,branch_Order,Active,Sales_Branch  from branch Order by branch_Order,branch_Name "
            cmd.CommandText = strQuery
            da.SelectCommand = cmd
            da.Fill(Ds, "branch")

            'Settings
            strQuery = " select S_Key,S_Value,Branch,Active_Value from settings Where 1 = 1 "
            cmd.CommandText = strQuery
            da.SelectCommand = cmd
            da.Fill(Ds, "settings")

            'Fin Year
            strQuery = " select Fin_YearId,Fin_Start,Fin_End,Fin_Desc,Active  from fin_year where Active = 1 "
            cmd.CommandText = strQuery
            da.SelectCommand = cmd
            da.Fill(Ds, "fin_year")

            'Users
            strQuery = " select U.UserId,U.U_Name,cast(uncompress(U.U_Pw) as Char) as U_Pw,U.Active,U.Admin,U.Type,U.Last_Login,U.Branch,U.Reports,U.IsSingleUser from users U " &
                        " left join branch B on B.BranchId = U.Branch where U.Active = 1 "
            If SysMode <> "" Then
                strQuery = strQuery & " And U.Type = '" & SysMode & "'"
            End If
            If Branch_Code <> "" Then
                strQuery = strQuery & " And B.Branch_Code = '" & Branch_Code & "'"
            End If

            cmd.CommandText = strQuery
            da.SelectCommand = cmd
            da.Fill(Ds, "users")

            'Menu
            strQuery = " select * from forms order by UserType,Orders,Frm_Text"
            cmd.CommandText = strQuery
            da.SelectCommand = cmd
            da.Fill(Ds, "forms")

            strQuery = " select * from ledgers "
            cmd.CommandText = strQuery
            da.SelectCommand = cmd
            da.Fill(Ds, "ledgers")

            ''Mfd Year
            'strQuery = " select * from mfd_year_month order by YM,Mfd_Year_Month"
            'cmd.CommandText = strQuery
            'da.SelectCommand = cmd
            'da.Fill(Ds, "mfd_year_month")

            cn.Close()
            cn = Nothing
        Catch ex As Exception
            cn = Nothing
        End Try

        Return Ds

    End Function


    Public Function Get_Forms() As CompDS


        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim da As New MySqlDataAdapter
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Ds As New CompDS

        Try

            cmd.Connection = cn
            cn.Open()

            'Menu
            strQuery = " select * from forms order by UserType,Orders,Frm_Text "
            cmd.CommandText = strQuery
            da.SelectCommand = cmd
            da.Fill(Ds, "forms")

            cn.Close()
            cn = Nothing
        Catch ex As Exception
            cn = Nothing
        End Try

        Return Ds

    End Function

    Public Shared Function Chk_Is_In_Use(ByVal Id_Field As String, ByVal Id_Value As Integer,
                                  ByVal Field As String, ByVal Field_Value As String,
                                  ByVal Table As String) As Boolean

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Result As String = ""

        cmd.Connection = cn
        cn.Open()

        strQuery = " Select " & Field & " from " & Table & " where 1 = 1 "

        If Id_Field.Trim <> "" Then
            strQuery = strQuery & " And " & Id_Field & " <> '" & Id_Value & "'"
        End If
        If Field.Trim <> "" Then
            strQuery = strQuery & " And " & Field & " = '" & Field_Value & "'"
        End If

        cmd.CommandText = strQuery
        Result = cmd.ExecuteScalar

        cn.Close()
        cn = Nothing

        If Result = Nothing Then
            Return False
        Else
            Return True
        End If

    End Function

    Public Shared Function Get_Server_Time(ByVal AddTime As String) As DateTime

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Dt As DateTime = Date.Now

        cmd.Connection = cn
        Try
            cn.Open()

        Catch ex As Exception

        End Try

        AddTime = IIf(AddTime = "", "+00:00", AddTime)

        Try

            strQuery = " SELECT CONVERT_TZ(NOW(),'+00:00','" & AddTime & "');  "
            cmd.CommandText = strQuery
            Dt = cmd.ExecuteScalar

        Catch ex As Exception

        End Try

        cn.Close()
        cn = Nothing
        Return Dt

    End Function

    Public Shared Function Get_Excel_Data(ByVal TableName As String, ByVal File As String) As DataSet

        If Not IO.File.Exists(File) Then
            ' MdiChildren'MyMessageBox("File not exists..")
            Return Nothing
            Exit Function
        End If

        Dim status As Boolean = False
        Dim Ds As New DataSet
        Dim MyExcel As New Excel.Application
        Dim MyWorkBook As Excel.Workbook
        Dim WorkSheet As Excel.Worksheet
        Dim da As New OleDb.OleDbDataAdapter

        MyWorkBook = MyExcel.Workbooks.Open(File, True, True, , , , True, True)
        WorkSheet = MyWorkBook.ActiveSheet

        If TableName = "" Then TableName = WorkSheet.Name

        Try
            Dim cnn As New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + File + ";Extended Properties=""Excel 12.0;HDR=YES;IMEX=1;""")
            da = New OleDb.OleDbDataAdapter("Select * from [" & WorkSheet.Name & "$]", cnn)
            da.Fill(Ds, TableName)

        Catch ex As Exception
            MsgBox("Import failed!" & vbCrLf & ex.Message)

        Finally
            da.Dispose()
            da = Nothing
        End Try

        MyExcel.Workbooks.Close()
        MyExcel.Quit()

        Return Ds

    End Function

    Public Shared Function Get_Summary_ServiceOnTime(ByVal FrmDate As Date, ByVal ToDate As Date) As TallyDs

        Dim cn As New MySqlConnection(ConnectionString)
        Dim cmd As New MySqlCommand
        Dim Da As New MySqlDataAdapter
        Dim strQuery As String = ""
        Dim Ds As New TallyDs

        Try
            cmd.Connection = cn
            cn.Open()

            'strQuery = "select b.Id,a.Invoice_Number,a.Invoice_Date,a.Invoice_Amount,a.updated as To_Tally,a.type,b.type,a.JobCard_No,b.JobCard_Date,Reg_No,Customer_Name,Customer_Address," &
            '            "Sum(Discount) As Discount,sum(Taxable) As Taxable,LastSeen," &
            '            "Sum(CGST) as CGST,Sum(SGST) as SGST,Sum(PaiseRoundingOff) as PaiseRoundingOff,Sum(Total_Amount) as Total_Amount " &
            '            "From service_bills_head a left Join service_bills  b on a.Invoice_Number = b.Inv_Number "

            strQuery = "Select Invoice_No ,Invoice_Date,Customer_Name,Inv_Number as Invoice_Number,JobCard_No,LastSeen,Chassis_No,Type,Locked as Updated,invoice_Date,Sum(Discount) As Discount,sum(Taxable) As Taxable,Sum(CGST) As CGST,Sum(SGST) As SGST,Sum(PaiseRoundingOff) As PaiseRoundingOff," &
                       "Sum(Total_Amount) As Invoice_Amount  from service_bills  "


            If FrmDate <> Nothing Then
                strQuery = strQuery & "where Invoice_Date between '" & Format(FrmDate, "yyyy-MM-dd") & "' AND '" & Format(ToDate, "yyyy-MM-dd") & "'"

            Else
                strQuery = strQuery & "where 1=1  "
            End If

            ' strQuery = strQuery & "group by a.Invoice_Number order by  a.Invoice_Date desc, right( a.Invoice_Number,11) desc"
            strQuery = strQuery & " group by Inv_Number , JobCard_No"

            If FrmDate = Nothing Then
                strQuery += " Limit 100"
            End If

            cmd.CommandText = strQuery
            Da.SelectCommand = cmd
            Da.Fill(Ds, "Service_Bills_Heads")


            '  strQuery = " SELECT * FROM  Service_Bills  "
            strQuery = "SELECT a.*,b.* FROM service_bills_head a INNER JOIN Service_bills b ON a.Invoice_Number = b.Inv_Number AND a.type=b.type"

            If FrmDate <> Nothing Then
                strQuery = strQuery & " where a.Invoice_Date between '" & Format(FrmDate, "yyyy-MM-dd") & "' AND '" & Format(ToDate, "yyyy-MM-dd") & "'"

            Else
                strQuery = strQuery & "where 1=1  order by Invoice_Number"

            End If



            If FrmDate = Nothing Then
                strQuery += " Limit 5000"
            End If

            cmd.CommandText = strQuery
            Da.SelectCommand = cmd
            Da.Fill(Ds, "Service_Bills")


            cn.Close()
            cn = Nothing

        Catch ex As Exception
            cn.Dispose()
            cn = Nothing
        End Try

        Return Ds

    End Function


    Public Shared Function Get_Summary_Services(ByVal FrmDate As Date, ByVal ToDate As Date, ByVal Invoice_Number As String) As TallyDs

        Dim cn As New MySqlConnection(ConnectionString)
        Dim cmd As New MySqlCommand
        Dim Da As New MySqlDataAdapter
        Dim strQuery As String = ""
        Dim Ds As New TallyDs

        Try
            cmd.Connection = cn
            cn.Open()

            'strQuery = "select b.Id,a.Invoice_Number,a.Invoice_Date,a.Invoice_Amount,a.updated as To_Tally,a.type,b.type,a.JobCard_No,b.JobCard_Date,Reg_No,Customer_Name,Customer_Address," &
            '            "Sum(Discount) As Discount,sum(Taxable) As Taxable,LastSeen," &
            '            "Sum(CGST) as CGST,Sum(SGST) as SGST,Sum(PaiseRoundingOff) as PaiseRoundingOff,Sum(Total_Amount) as Total_Amount " &
            '            "From service_bills_head a left Join service_bills  b on a.Invoice_Number = b.Inv_Number "

            strQuery = "Select Invoice_No ,Invoice_Date,Customer_Name,Inv_Number as Invoice_Number,JobCard_No,LastSeen,Chassis_No,Type,Locked,invoice_Date,Sum(Discount) As Discount,CGST_Per,SGST_Per,sum(Taxable) As Taxable,Sum(CGST) As CGST,Sum(SGST) As SGST,Sum(PaiseRoundingOff) As PaiseRoundingOff," &
                       "Sum(Total_Amount) As Invoice_Amount  from service_bills  "


            If FrmDate <> Nothing Then

                If (Invoice_Number) <> Nothing Then
                    strQuery = strQuery & " where 1=1 and JobCard_No='" & Invoice_Number & "'"
                Else
                    strQuery = strQuery & "where Invoice_Date between '" & Format(FrmDate, "yyyy-MM-dd") & "' AND '" & Format(ToDate, "yyyy-MM-dd") & "'"

                End If
            Else
                strQuery = strQuery & "where 1=1  "
            End If


            ' strQuery = strQuery & "group by a.Invoice_Number order by  a.Invoice_Date desc, right( a.Invoice_Number,11) desc"
            strQuery = strQuery & " group by Inv_Number , JobCard_No"

            If FrmDate = Nothing Then
                strQuery += " Limit 100"
            End If

            cmd.CommandText = strQuery
            Da.SelectCommand = cmd
            Da.Fill(Ds, "Service_Bills_Heads")


            '  strQuery = " SELECT * FROM  Service_Bills  "
            strQuery = "SELECT a.*,b.* FROM service_bills_head a INNER JOIN Service_bills b ON a.Invoice_Number = b.Inv_Number AND a.type=b.type"

            If FrmDate <> Nothing Then
                If (Invoice_Number) <> Nothing Then
                    strQuery = strQuery & " where 1=1 and b.JobCard_No='" & Invoice_Number & "'"
                Else
                    strQuery = strQuery & " where a.Invoice_Date between '" & Format(FrmDate, "yyyy-MM-dd") & "' AND '" & Format(ToDate, "yyyy-MM-dd") & "'"

                End If

            Else
                strQuery = strQuery & "where 1=1  order by Invoice_Number"

            End If



            If FrmDate = Nothing Then
                strQuery += " Limit 5000"
            End If

            cmd.CommandText = strQuery
            Da.SelectCommand = cmd
            Da.Fill(Ds, "Service_Bills")


            cn.Close()
            cn = Nothing

        Catch ex As Exception
            cn.Dispose()
            cn = Nothing
        End Try

        Return Ds

    End Function


    Public Shared Function Get_Summary_Service(ByVal FrmDate As Date, ByVal ToDate As Date) As TallyDs

        Dim cn As New MySqlConnection(ConnectionString)
        Dim cmd As New MySqlCommand
        Dim Da As New MySqlDataAdapter
        Dim strQuery As String = ""
        Dim Ds As New TallyDs

        Try
            cmd.Connection = cn
            cn.Open()

            strQuery = " select ifnull(b.Job_Card,0) as Job_Card ,b.Part_Labour_Description,b.Customer_Name,b.Chassis_No,b.Service_Advisor,b.Insurance_Provider,b.Insurance_Policy_No,b.Insurance_Start_Date,b.Insurance_Expiry_Date," &
            "a.* from service_header a left join service b on a.Invoice_Number=b.Invoice_Number where 1=1"

            If FrmDate <> Nothing Then
                strQuery = strQuery & " And  a.Invoice_Date between '" & Format(FrmDate, "yyyy-MM-dd") & "' AND '" & Format(ToDate, "yyyy-MM-dd") & "'"
            Else
                strQuery = strQuery & ""
            End If

            strQuery += " group by a.Invoice_Number order by  a.Invoice_Date desc"

            If FrmDate = Nothing Then
                strQuery += " Limit 200"
            End If

            If Read_Settings("Service_Limit") <> "" Then
                strQuery += "Limit " & Read_Settings("Service_Limit") & ""

            End If

            cmd.CommandText = strQuery
            Da.SelectCommand = cmd
            Da.Fill(Ds, "Service")


            strQuery = " select a.*,b.* from service_header a inner join Service b on a.Invoice_Number = b.Invoice_Number  "
            If FrmDate <> Nothing Then
                strQuery = strQuery & "where a.Invoice_Date between '" & Format(FrmDate, "yyyy-MM-dd") & "' AND '" & Format(ToDate, "yyyy-MM-dd") & "'"
            Else
                strQuery = strQuery & "where 1=1  order by a.Invoice_Number"
            End If


            If FrmDate = Nothing Then
                strQuery += " Limit 5000"
            End If


            cmd.CommandText = strQuery
            Da.SelectCommand = cmd
            Da.Fill(Ds, "Service_Detail")


            cn.Close()
            cn = Nothing

        Catch ex As Exception
            MsgBox(ex.Message)
            CommonDA.Create_Log("Get_Summary_Service", "Select Qry:" & strQuery & " Error :" & ex.Message, ex.StackTrace)
            cn.Dispose()
            cn = Nothing
        End Try

        Return Ds

    End Function
    Public Shared Function Insert_Service_Ontime(ByVal Ds As DataSet) As String

        Dim Qry As String = ""
        Dim Status As String = ""
        Dim invdate As String = ""
        Dim x As Integer = 0
        Dim InvNo As String = ""
        Dim Type As String = ""
        ' Dim Total As Integer = Ds.Tables("ServiceOn").Rows.Count
        Dim Labour_SACCode As String = Read_Ledgers("Labour_SACCode")
        Dim Value As String = ""
        Dim Taxable As Decimal = 0.0
        'Dim Qty As Decimal = 0.0
        Dim iGST As Decimal = 0.0
        Dim cn As New MySqlConnection(ConnectionString)
        Dim cmd As New MySqlCommand
        Dim BillType As String = ""
        Dim ContinueFor As Boolean = False
        Dim Dr As TallyDs.PartDetailsRow
        Dim PartDs As New TallyDs
        Dim Grand_Total As Decimal = 0.00
        Dim Details As String = ""
        Dim JobCardNo As String = ""
        Dim jDate As Date
        Dim iDate As Date
        Dim Drc As TallyDs.CustomerDetailsRow
        Dim JobCardDate As String = ""
        Dim CustAddress As String = ""
        Dim InvoiceNo As String = ""
        Dim InvoiceDate As String = ""
        Dim TabelEntry As Boolean = False
        Dim Name = "", address = "", mobile = "", email As String = ""
        Dim RegNo = "", Chaseno = "", engNo = "", model = "", modelName As String = ""
        Dim OrderAlloc = "", SalePerson = "", SupGST = "", RecGST = "", POS = "", HSN = "", UOM = "", AMtAfDisc = "", TotTax = "", RndOff As String = ""
        Dim sno = -1, labourcode = -1, labourdesc = -1, partcode = -1, partdesc = -1, qty = -1, P_rate = -1, L_rate = -1, P_disc = -1, L_disc As Integer = -1
        Dim sgst = -1, sgst_per = -1, cgst = -1, cgst_per = -1, P_amount = -1, L_amount = -1, L_totalAmt = -1, P_totalAmt = -1, kfc = -1, kfc_per As Integer = -1
        Dim CustDetails = -1, JobCNo = -1, JobCDate = -1, ServTaxNo = -1, InvNum = -1, InvoDate = -1, Tempval As Integer = -1
        Dim OrdrAlloc = -1, SalePer = -1, SpGST = -1, ReGST = -1, Ps = -1, HSN_C = -1, UO_M = -1, AftrDisc = -1, Tot_Tax = -1, rndof As Integer = -1

        Dim QryHead = "", QryData = "", QryDelete As String = ""
        Dim TypeOfInv As String = ""
        Dim is_SSI As Boolean = False
        Try


            cmd.Connection = cn
            cn.Open()

            x = 0
            Dim i As Integer = 0
            Dim MaxCol As Integer = Ds.Tables("ServiceOn").Columns.Count

            For Each Drd In Ds.Tables("ServiceOn").Rows


                For i = 0 To MaxCol - 1

                    If Drd(i).ToString <> "" And is_SSI <> True Then
                        If Drd(i).ToString.Contains("TAX INVOICE - PARTS") Then
                            is_SSI = True
                        End If
                    End If

                    If is_SSI = True Then

                        If Val(OrderAlloc) > 0 And Drd(i) <> "" And Drd(i) <> ":" Then
                            OrderAlloc = ""
                            OrdrAlloc = i
                        End If
                        If Val(CustAddress) > 0 And Drd(i) <> "" And Drd(i) <> ":" Then
                            CustAddress = ""
                            CustDetails = i
                        End If
                        If Val(InvoiceNo) > 0 And Drd(i) <> "" And Drd(i) <> ":" Then
                            InvoiceNo = ""
                            InvNum = i
                        End If
                        If Val(InvoiceDate) > 0 And Drd(i) <> "" And Drd(i) <> ":" Then
                            InvoiceDate = ""
                            InvoDate = i
                        End If
                        If Val(SalePerson) > 0 And Drd(i) <> "" And Drd(i) <> ":" Then
                            SalePerson = ""
                            SalePer = i
                        End If

                        If Val(SupGST) > 0 And Drd(i) <> "" And Drd(i) <> ":" Then
                            SupGST = ""
                            SpGST = i
                        End If

                        If Val(RecGST) > 0 And Drd(i) <> "" And Drd(i) <> ":" Then
                            RecGST = ""
                            ReGST = i
                        End If

                        If Val(POS) > 0 And Drd(i) <> "" And Drd(i) <> ":" Then
                            POS = ""
                            Ps = i
                        End If

                        If Val(TotTax) > 0 And Drd(i) <> "" And P_amount > 0 Then
                            TotTax = ""
                            Tot_Tax = i
                        End If

                        If Val(RndOff) > 0 And Drd(i) <> "" And Tot_Tax > 0 Then
                            RndOff = ""
                            rndof = i
                        End If

                        If Val(Grand_Total) > 0 And Drd(i) <> "" And rndof > 0 Then
                            Grand_Total = 0.0
                            P_totalAmt = i
                        End If


                        If Drd(i).ToString.Contains("Order/Allocation") And OrdrAlloc < 0 Then
                            OrderAlloc = i
                        ElseIf Drd(i).ToString.Contains("Customer Name") And CustDetails < 0 Then
                            CustAddress = i
                        ElseIf Drd(i).ToString.Contains("Invoice No") And InvNum < 0 Then
                            InvoiceNo = i
                        ElseIf Drd(i).ToString.Contains("Invoice Date") And InvoDate < 0 Then
                            InvoiceDate = i
                        ElseIf Drd(i).ToString.Contains("Sales Person") And SalePer < 0 Then
                            SalePerson = i
                        ElseIf Drd(i).ToString.Contains("Supplier GSTIN") And SpGST < 0 Then
                            SupGST = i
                        ElseIf Drd(i).ToString.Contains("Recipient GSTIN") And ReGST < 0 Then
                            RecGST = i
                        ElseIf Drd(i).ToString.Contains("POS") And Ps < 0 Then
                            POS = i
                        ElseIf Drd(i).ToString.Contains("DESCRIPTION") Then
                            partdesc = i
                        ElseIf Drd(i).ToString.Contains("PART NO") Then
                            partcode = i
                        ElseIf Drd(i).ToString.Contains("S No") Then
                            sno = i
                        ElseIf Drd(i).ToString.Contains("HSN No.") Then
                            HSN_C = i
                        ElseIf Drd(i).ToString.Contains("UOM") Then
                            UO_M = i
                        ElseIf Drd(i).ToString.Contains("QTY") Then
                            qty = i
                        ElseIf Drd(i).ToString.Contains("RATE") Then
                            P_rate = i
                        ElseIf Drd(i).ToString.Contains("DISCOUNT") And P_disc < 0 Then
                            P_disc = i
                        ElseIf Drd(i).ToString.Contains("AMOUNT AFTER DISCOUNT") Then
                            AftrDisc = i
                        ElseIf Drd(i).ToString.Contains("SGST%") Then
                            sgst_per = i
                        ElseIf Drd(i).ToString.Contains("SGST") Then
                            sgst = i
                        ElseIf Drd(i).ToString.Contains("CGST%") Then
                            cgst_per = i
                        ElseIf Drd(i).ToString.Contains("CGST") Then
                            cgst = i
                        ElseIf Drd(i).ToString.Contains("KFC") Then
                            kfc = i
                        ElseIf Drd(i).ToString.Contains("KFC %") Then
                            kfc_per = i
                        ElseIf Drd(i).ToString.Contains("AMOUNT") Then
                            P_amount = i
                        ElseIf Drd(i).ToString.Contains("TOTAL TAX") And P_amount > 0 Then
                            TotTax = i
                        ElseIf Drd(i).ToString.Contains("ROUND OFF") And Tot_Tax > 0 Then
                            RndOff = i
                        ElseIf Drd(i).ToString.Contains("TOTAL") And rndof > 0 And P_totalAmt < 0 Then
                            Grand_Total = i
                        End If

                    Else

                        If Val(CustAddress) > 0 And Drd(i).ToString <> "" Then
                            CustAddress = ""
                            CustDetails = i
                        End If

                        If Val(JobCardNo) > 0 And Drd(i).ToString <> "" Then
                            JobCardNo = ""
                            JobCNo = i
                        End If

                        If Val(JobCardDate) > 0 And Drd(i).ToString <> "" Then
                            JobCardDate = ""
                            JobCDate = i
                        End If

                        If Val(Details) > 0 And Drd(i).ToString <> "" Then
                            Details = ""
                            ServTaxNo = i
                        End If
                        If Val(InvoiceDate) > 0 And Drd(i).ToString <> "" Then
                            InvoiceDate = ""
                            InvoDate = i
                        End If

                        If Val(InvoiceNo) > 0 And Drd(i).ToString <> "" Then
                            InvoiceNo = ""
                            InvNum = i
                        End If

                        If Val(Grand_Total) > 0 And Drd(i).ToString <> "" And partdesc < 0 Then
                            Grand_Total = 0.0
                            L_totalAmt = i
                        ElseIf Val(Grand_Total) > 0 And Drd(i).ToString <> "" And partdesc > 0 Then
                            Grand_Total = 0.0
                            P_totalAmt = i
                        End If

                        If Drd(i).ToString.Contains("S No") Then
                            sno = i
                        ElseIf Drd(i).ToString.Contains("Labour Code") Then
                            labourcode = i
                        ElseIf Drd(i).ToString.Contains("LABOUR DESCRIPTION") Then
                            labourdesc = i
                        ElseIf Drd(i).ToString.Contains("PART  DESCRIPTION") Then
                            partdesc = i
                        ElseIf Drd(i).ToString.Contains("Part Code") Then
                            partcode = i
                        ElseIf Drd(i).ToString.Contains("RATE(Rs)") And labourdesc > 0 And L_disc < 0 Then
                            L_rate = i
                        ElseIf Drd(i).ToString.Contains("RATE(Rs)") And partdesc > 0 Then
                            P_rate = i
                        ElseIf Drd(i).ToString.Contains("QTY") Then
                            qty = i
                        ElseIf Drd(i).ToString.Contains("DISCOUNT") And labourdesc > 0 And L_disc < 0 Then
                            L_disc = i
                        ElseIf Drd(i).ToString.Contains("DISCOUNT") And partdesc > 0 Then
                            P_disc = i
                        ElseIf Drd(i).ToString.Contains("SGST%") Then
                            sgst_per = i
                        ElseIf Drd(i).ToString.Contains("SGST") Then
                            sgst = i
                        ElseIf Drd(i).ToString.Contains("CGST%") Then
                            cgst_per = i
                        ElseIf Drd(i).ToString.Contains("CGST") Then
                            cgst = i
                        ElseIf Drd(i).ToString.Contains("TOTAL AMOUNT(Rs)") Then
                            L_amount = i
                        ElseIf Drd(i).ToString.Contains("AMOUNT(Rs)") Then
                            P_amount = i
                        ElseIf Drd(i).ToString.Contains("TOTAL") And partdesc < 0 Then
                            Grand_Total = i
                        ElseIf Drd(i).ToString.Contains("GRAND TOTAL") And partdesc > 0 Then
                            Grand_Total = i
                        ElseIf Drd(i).ToString.Contains("CUSTOMER NAME & ADDRESS") And CustDetails < 0 Then
                            CustAddress = i
                        ElseIf Drd(i).ToString.Contains("JOB CARD NO") And JobCNo < 0 Then
                            JobCardNo = i
                        ElseIf Drd(i).ToString.Contains("JOB CARD DATE") And JobCDate < 0 Then
                            JobCardDate = i
                        ElseIf Drd(i).ToString.Contains("SERV TAX NO.") And ServTaxNo < 0 Then
                            Details = i
                        ElseIf Drd(i).ToString.Contains("Registration No :") And ServTaxNo < 0 Then
                            ServTaxNo = i
                        ElseIf Drd(i).ToString.Contains("INVOICE NO") And InvNum < 0 Then
                            InvoiceNo = i
                        ElseIf Drd(i).ToString.Contains("INVOICE DATE") And InvoDate < 0 Then
                            InvoiceDate = i
                        End If

                    End If

                Next

            Next

            Dim Datepat As String = "^(((19|20)(([0][48])|([2468][048])|([13579][26]))|2000)[\-](([0][13578]|[1][02])[\-]([012][0-9]|[3][01])|([0][469]|11)[\-]([012][0-9]|30)|02[\-]([012][0-9]))|((19|20)(([02468][1235679])|([13579][01345789]))|1900)[\-](([0][13578]|[1][02])[\-]([012][0-9]|[3][01])|([0][469]|11)[\-]([012][0-9]|30)|02[\-]([012][0-8])))$"
            InvoiceDate = "" : InvoiceNo = "" : Details = "" : CustAddress = "" : JobCardDate = "" : JobCardNo = ""

            If is_SSI Then
                labourcode = 0 : labourdesc = 0 : L_amount = 0 : L_disc = 0 : L_rate = 0 : L_totalAmt = 0
            End If

            For Each DrD In Ds.Tables("ServiceOn").Rows

                'If BillType = "CLI" Or BillType = "CPI" Then
                '    Continue For 'warranty
                'End If
                If is_SSI <> True Then


                    If DrD(InvNum) <> "" And InvoiceNo = "" Then
                        InvoiceNo = IIf(DrD(InvNum).ToString.Trim <> "", DrD(InvNum).ToString.Trim.Replace(":", ""), "")
                        BillType = Strings.Left(InvoiceNo, 4)
                    ElseIf DrD(InvoDate) <> "" And InvoiceDate = "" Then
                        InvoiceDate = IIf(DrD(InvoDate).ToString.Trim <> "", DrD(InvoDate).ToString.Trim.Replace(":", ""), "")
                    ElseIf DrD(ServTaxNo) <> "" And Details = "" Then
                        Details = IIf(DrD(ServTaxNo).ToString.Trim <> "", DrD(ServTaxNo).ToString.Trim.Replace(":", ""), "")

                        Dim SplitDetails As String() = Details.Split(New String() {vbCr & vbLf, vbLf}, StringSplitOptions.None)

                        For Each line In SplitDetails

                            If RegNo = "" Then
                                RegNo = line.ToString.Trim.Replace("Registration No  ", "")
                            ElseIf line.Contains("Chassis No") Then
                                Chaseno = line.ToString.Trim.Replace("Chassis No", "").Trim
                            ElseIf line.Contains("Engine No") Then
                                engNo = line.ToString.Trim.Replace("Engine No", "").Trim
                            ElseIf line.Contains("Model") And model = "" Then
                                model = line.ToString.Trim.Replace("Model", "").Trim
                            ElseIf line.Contains("Model Name") Then
                                modelName = line.ToString.Trim.Replace("Model Name", "").Trim
                            End If

                        Next

                    ElseIf DrD(JobCNo) <> "" And JobCardNo = "" Then
                        JobCardNo = IIf(DrD(JobCNo).ToString.Trim <> "", DrD(JobCNo).ToString.Trim.Replace(":", ""), "")
                    ElseIf DrD(JobCDate) <> "" And JobCardDate = "" Then
                        JobCardDate = IIf(DrD(JobCDate).ToString.Trim <> "", DrD(JobCDate).ToString.Trim.Replace(":", ""), "")
                    ElseIf DrD(CustDetails) <> "" And CustAddress = "" Then
                        CustAddress = IIf(DrD(CustDetails).ToString.Trim <> "", DrD(CustDetails).ToString.Trim.Replace(":", ""), "")
                        Dim SplitAdsress As String() = CustAddress.Split(New String() {vbCr & vbLf, vbLf}, StringSplitOptions.None)

                        For Each line In SplitAdsress

                            If Name = "" Then
                                Name = line.ToString.Trim
                                Dim Names() As String = Name.Split({"("}, StringSplitOptions.RemoveEmptyEntries)
                                Name = Names.First.ToString.Trim
                            ElseIf line.Contains("Mobile") Then
                                mobile = line.ToString.Trim.Replace("Mobile", "").Trim
                            ElseIf line.Contains("Email") Then
                                email = line.ToString.Trim.Replace("Email", "").Trim
                            Else
                                address += line.ToString.Trim
                            End If
                        Next
                    End If

                    If DrD(sno) <> "S No" And (DrD(partcode) <> "" Or DrD(labourcode) <> "") And (DrD(partdesc) <> "" Or DrD(labourdesc) <> "") And DrD(sgst) <> "" And (DrD(L_disc) <> "" Or DrD(P_disc) <> "") Then
                        If DrD(sgst_per) <> "" And DrD(cgst) <> "" And DrD(cgst_per) <> "" And (DrD(P_amount) <> "" Or DrD(L_amount) <> "") And (DrD(L_rate) <> "" Or DrD(P_rate) <> "") Then
                            TabelEntry = True
                        Else
                            TabelEntry = False
                        End If
                    Else
                        TabelEntry = False
                    End If

                    If PartDs.PartDetails.Rows.Count > 0 And (DrD(L_totalAmt) <> "" Or DrD(P_totalAmt) <> "") Then


                        Grand_Total = IIf(Val(DrD(L_totalAmt)) > 0, Val(DrD(L_totalAmt)), Val(DrD(P_totalAmt)))

                    End If

                Else

                    If DrD(InvNum) <> "" And InvoiceNo = "" Then
                        InvoiceNo = IIf(DrD(InvNum).ToString.Trim <> "", DrD(InvNum).ToString.Trim.Replace(":", ""), "")
                        BillType = Strings.Left(InvoiceNo, 4)
                    ElseIf DrD(InvoDate) <> "" And InvoiceDate = "" Then
                        InvoiceDate = IIf(DrD(InvoDate).ToString.Trim <> "", DrD(InvoDate).ToString.Trim.Replace(":", ""), "")
                        InvoiceDate = InvoiceDate.Substring(0, 10)
                    ElseIf DrD(CustDetails) <> "" And CustAddress = "" Then
                        CustAddress = IIf(DrD(CustDetails).ToString.Trim <> "", DrD(CustDetails).ToString.Trim.Replace(":", ""), "")
                        Dim SplitAdsress As String() = CustAddress.Split(New String() {vbCr & vbLf, vbLf}, StringSplitOptions.None)

                        For Each line In SplitAdsress

                            If Name = "" Then
                                Name = line.ToString.Trim
                                Name = Name.Replace("Mr.", "")
                                ' Name = line.ToString.Trim.Replace(" (", "").Trim

                            ElseIf line.Contains("Mobile") Then
                                mobile = line.ToString.Trim.Replace("Mobile", "").Trim

                                mobile = mobile.Split("Phone")(0)

                            ElseIf line.Contains("Email") Then
                                email = line.ToString.Trim.Replace("Email", "").Trim
                            Else
                                address += line.ToString.Trim
                            End If
                        Next
                    ElseIf DrD(SalePer) <> "" And SalePerson = "" Then
                        SalePerson = IIf(DrD(SalePer).ToString.Trim <> "", DrD(SalePer).ToString.Trim.Replace(":", ""), "")
                    ElseIf DrD(SpGST) <> "" And SupGST = "" Then
                        SupGST = IIf(DrD(SpGST).ToString.Trim <> "", DrD(SpGST).ToString.Trim.Replace(":", ""), "")
                    ElseIf DrD(Ps) <> "" And POS = "" Then
                        POS = IIf(DrD(Ps).ToString.Trim <> "", DrD(Ps).ToString.Trim.Replace(":", ""), "")
                    ElseIf DrD(ReGST) <> "" And RecGST = "" Then
                        RecGST = IIf(DrD(ReGST).ToString.Trim <> "", DrD(ReGST).ToString.Trim.Replace(":", ""), "")
                    End If



                    If DrD(sno) <> "S No" And DrD(partcode) <> "" And DrD(partdesc) <> "" And DrD(sgst) <> "" And DrD(P_disc) <> "" Then
                        If DrD(sgst_per) <> "" And DrD(cgst) <> "" And DrD(cgst_per) <> "" And DrD(P_amount) <> "" And DrD(P_rate) <> "" Then
                            TabelEntry = True
                        Else
                            TabelEntry = False
                        End If
                    Else
                        TabelEntry = False
                    End If


                    If PartDs.PartDetails.Rows.Count > 0 And (DrD(P_totalAmt) <> "") Then


                        Grand_Total = IIf(Val(DrD(P_totalAmt)) > 0, Val(DrD(P_totalAmt)), Val(DrD(P_totalAmt)))
                        If Grand_Total < DrD(P_totalAmt) Then
                            Grand_Total = DrD(P_totalAmt)

                        End If
                    End If

                End If

                If TabelEntry Then

                    Dr = PartDs.PartDetails.Rows.Add
                    Dr.S_No = IIf(DrD(sno).ToString.Trim <> "", Val(DrD(sno).ToString.Trim), "")
                    Dr.Part_Labour_Code = IIf(DrD(partcode).ToString.Trim <> "", DrD(partcode).ToString.Trim, DrD(labourcode).ToString.Trim)
                    Dr.Part_Labour_Description = IIf(DrD(partdesc).ToString.Trim <> "", DrD(partdesc).ToString.Trim, DrD(labourdesc).ToString.Trim)
                    Dr.Qty = IIf(DrD(partdesc).ToString.Trim <> "", DrD(qty), "1")
                    Dr.Value = IIf(DrD(partdesc).ToString.Trim <> "", DrD(P_rate), DrD(L_rate))

                    'Dr.Qty = IIf(qty <> L_rate, Val(DrD(qty).ToString.Trim), "1")
                    'Dr.Rate = IIf(Val(DrD(P_rate)) < 0, Val(DrD(P_rate).ToString.Trim), Val(DrD(L_rate).ToString.Trim))
                    If is_SSI Then
                        Dr.Discount = IIf(DrD(P_disc).ToString.Trim <> "", Val(DrD(P_disc).ToString.Trim), 0)
                    Else
                        Dr.Discount = IIf(DrD(L_disc).ToString.Trim <> "", Val(DrD(L_disc).ToString.Trim), Val(DrD(P_disc).ToString.Trim))
                    End If
                    Dr.SGST = IIf(DrD(sgst).ToString.Trim <> "", Val(DrD(sgst).ToString.Trim), "")
                    Dr.SGST_Per = IIf(DrD(sgst_per).ToString.Trim <> "", Val(DrD(sgst_per).ToString.Trim), "")
                    Dr.CGST = IIf(DrD(cgst).ToString.Trim <> "", Val(DrD(cgst).ToString.Trim), "")
                    Dr.CGST_Per = IIf(DrD(cgst_per).ToString.Trim <> "", Val(DrD(cgst_per).ToString.Trim), "")
                    Dr.Amount = IIf(DrD(P_amount).ToString.Trim <> "", (DrD(P_amount)), DrD(L_amount))
                    Try
                        Dr.KFC = IIf(DrD(kfc).ToString.Trim <> "", Val(DrD(kfc).ToString.Trim), 0.00)
                        Dr.KFC_Per = IIf(DrD(kfc_per).ToString.Trim <> "", Val(DrD(kfc_per).ToString.Trim), 0.00)

                    Catch ex As Exception

                    End Try


                    If is_SSI Then
                        Dr.HSN_No = IIf(is_SSI = True, DrD(HSN_C), "")
                        Dr.UOM = IIf(is_SSI = True, DrD(UO_M), "")
                        Dr.Taxable_Amount = Val(DrD(AftrDisc))
                    Else
                        HSN_C = 0
                        UO_M = 0
                        Dr.HSN_No = ""
                        Dr.UOM = ""
                        'If Dr.Qty < 1 Then
                        '    Dr.Taxable_Amount = (Dr.Rate / Dr.Qty)
                        'Else
                        Dr.Taxable_Amount = (Dr.Value * Dr.Qty)

                        'End If

                    End If

                    Try
                        Dr.Taxable_Amount = Dr.Taxable_Amount.ToString.Replace(",", "")

                        Dr.Value = Dr.Value.Replace(",", "")
                        Dr.Amount = Dr.Amount.ToString.Replace(",", "")

                    Catch ex As Exception

                    End Try

                    PartDs.AcceptChanges()

                End If

            Next

            If BillType.Contains("SP") Then
                TypeOfInv = "Part"
            ElseIf BillType.Contains("SL") Then
                TypeOfInv = "Labour"
            ElseIf BillType.Contains("SSI") Then
                TypeOfInv = "Item"
            Else
                TypeOfInv = ""
            End If

            '    iDate = Convert.ToDateTime(InvoiceDate)

            '    If is_SSI Then
            '        jDate = iDate
            '        JobCardNo = InvoiceNo
            '    Else
            '        jDate = Convert.ToDateTime(JobCardDate)

            '    End If



            Dim Drs As TallyDs.Service_HeaderRow
            Dim CashDs As New BkDS
            Dim dcr As BkDS.Cash_RegisterRow
            Dim StrQuery As String = ""
            Dim prefix As String = ""
            Dim NextId As String = ""
            Dim StrQry As String = ""
            Dim CashReg As String = ""
            Dim Servicebills As Integer = 0
            Dim ServiceBill As String = ""
            Dim Id As Integer = 0
            Dim invsDate As Date
            Dim JcDate As Date

            invsDate = Convert.ToDateTime(InvoiceDate)
            JcDate = Convert.ToDateTime(JobCardDate)


            CashDs.Clear()
            dcr = CashDs.Cash_Register.Rows.Add
            dcr.Branch_Id = PublicShared.Branch_Id
            dcr.BkId = 0
            dcr.PayId = 0
            dcr.User_Id = PublicShared.User_Id
            dcr.User_Name = PublicShared.User_Name
            CashDs.AcceptChanges()

            If is_SSI Then
                prefix = Read_Settings("Prefix_CS")
            Else
                prefix = Read_Settings("Prefix_PL")
            End If
            Try

                Drs = Get_LastId(prefix)


                If Drs.Job_Card.Trim = JobCardNo.Trim Then


                Else

                    Drs.Invoice_Number = ""
                    Try

                        StrQry = " Select Inv_Number from service_bills where `JobCard_No` = '" & JobCardNo.Trim & "' ; "
                        cmd.CommandText = StrQry
                        ServiceBill = cmd.ExecuteScalar()

                        Drs.Invoice_Number = ServiceBill

                        If Drs.Invoice_Number = "" Then
                            NextId = Get_NextId(prefix)
                            Drs.Invoice_Number = NextId
                        End If

                    Catch ex As Exception
                        NextId = Get_NextId(prefix)
                        Drs.Invoice_Number = NextId

                    End Try

                End If
            Catch

                StrQuery = " Select Count(Id) from service_bills  "
                cmd.CommandText = StrQuery
                NextId = cmd.ExecuteScalar()

                NextId = prefix & Format(Val(NextId) + 1, "00000")
                Drs.Invoice_Number = NextId.Trim

            End Try
            '    cmd.CommandText = StrQuery
            '    Id = cmd.ExecuteScalar()
            'Catch ex As Exception
            '    Id = 0
            'End Try



            Try
                StrQry = " Select Id from service_bills where `JobCard_No` = '" & JobCardNo.Trim & "' and Type ='" & TypeOfInv.Trim & "' ; "
                cmd.CommandText = StrQry
                Servicebills = cmd.ExecuteScalar()
            Catch ex As Exception
                Servicebills = 0
            End Try


            If Servicebills <> 0 Then
                '  Drc.Invoice_No = ServiceBill
            Else

                QryHead += "INSERT INTO service_bills (Inv_Number,JobCard_No,Invoice_Date,Invoice_No,JobCard_Date,Reg_No,Chassis_No,Vehicle_Details," &
                    "Customer_Name,Customer_Address,Type," &
                    "Part_Labour_Code,Part_Labour_Description,HSN_No,Rate," &
                    "Qty,Discount,PaiseRoundingOff,CGST_Per,CGST,SGST_Per," &
                    "SGST,Total_Amount,LastSeen,Taxable) VALUES "

                For Each Dr In PartDs.PartDetails.Rows

                    If Dr.Value <> "TOTAL" Then

                        QryData += IIf(QryData = "", "", ",")

                        'Dr.JobCard_No
                        QryData += " (" & "'" & Drs.Invoice_Number.Trim & "'," &
                "'" & JobCardNo.Trim & "','" & Format(invsDate, "yyyy-MM-dd") & "','" & InvoiceNo.Trim & "','" & Format(JcDate, "yyyy-MM-dd") & "','" & RegNo & "','" & Chaseno & "','" & Details & "'," &
                "'" & Name & "','" & address & "','" & TypeOfInv.Trim & "'," &
                "'" & Dr.Part_Labour_Code & "','" & Dr.Part_Labour_Description & "','" & Dr.HSN_No & "','" & Dr.Value & "'," &
                "'" & Dr.Qty & "','" & Dr.Discount & "','0.00','" & Dr.CGST_Per & "','" & Dr.CGST & "','" & Dr.SGST_Per & "'," &
                "'" & Dr.SGST & "','" & Dr.Amount & "','" & Format(DateTime.Now, "yyyy-MM-dd") & "','" & Dr.Taxable_Amount & "')"

                    Else

                    End If


                Next

                QryData += ";"

            End If


            Try
                Qry = "SELECT Id FROM service_bills_head WHERE Invoice_Date = '" & Format(invsDate, "yyyy-MM-dd") & "' AND `Invoice_Number` = '" & Drs.Invoice_Number.Trim & "' AND Type ='" & TypeOfInv.Trim & "';"
                cmd.CommandText = Qry
                Id = cmd.ExecuteScalar()
            Catch ex As Exception
                Id = 0
            End Try

            If Id <> 0 Then

            Else

                Qry += " INSERT INTO service_bills_head (Invoice_Number,JobCard_No,Invoice_Date,Invoice_Amount,Type,LastSeen) VALUES (" &
                        "'" & Drs.Invoice_Number.Trim & "','" & JobCardNo.Trim & "','" & Format(invsDate, "yyyy-MM-dd") & "'," &
                        "'" & Grand_Total & "','" & TypeOfInv & "','" & Format(Date.Now, "yyyy-MM-dd HH:mm:ss") & "')" 'Val(Grand_Total)

                'Drc.JobCard_No

                If QryData <> "" Then
                    cmd.CommandText = QryHead + QryData + Qry
                    cmd.ExecuteNonQuery()

                End If

            End If

            Try
                StrQry = "Select id from cash_register where JobCardNo='" & JobCardNo.Trim & "' and Type = '" & TypeOfInv & "'" &
                 " and Payment_Mode=''"
                cmd.CommandText = StrQry
                CashReg = cmd.ExecuteScalar
            Catch ex As Exception
                CashReg = 0
            End Try

            If CashReg > 0 Then

            Else
                StrQry = "Insert into cash_register (Entry_Date,Entry_Type,Entry_Head,Entry_Sub_Head,Ref_No,Amount,Type,Party,JobCardNo,Branch_Id,User_Id,Chassis_No," &
                      "User_Name,Created_On) Values('" & Format(invsDate, "yyyy-MM-dd") & "','Receipt','Service','Service Invoice','" & Drs.Invoice_Number & "','" & Grand_Total & "'," &
                      "'" & TypeOfInv & "','" & Name & "','" & JobCardNo.Trim & "','" & dcr.Branch_Id & "','" & dcr.User_Id & "','" & Chaseno & "','" & dcr.User_Name & "'," &
                      "'" & Format(Date.Now, "yyyy-MM-dd HH:mm:ss") & "')"


                cmd.CommandText = StrQry
                cmd.ExecuteNonQuery()
            End If

            'Tr.Commit()
            'Tr.Dispose()
            cn.Close()
            cn.Dispose()
            cn = Nothing

            Status = Drs.Invoice_Number.Replace("/", "_") + "_" + TypeOfInv + "True"
            Return Status

        Catch ex As Exception
            'If Not Tr Is Nothing Then
            '    Tr.Rollback()

            'End If
            cn.Close()
            cn.Dispose()

            Try
                Status = Drc.Invoice_No.Replace("/", "_") + "_" + TypeOfInv

            Catch eex As Exception
                Status = TypeOfInv + "Error"
            End Try
            Return Status

            MsgBox("Error Found : " & ex.Message & vbCrLf & ex.StackTrace, vbCritical)
        End Try

        Return Status

    End Function

    Public Shared Function Insert_Service_Ontime1(ByVal Ds As DataSet, ByVal OverWrite As Boolean) As String

        Dim Qry As String = ""
        Dim Status As String = ""
        Dim invdate As String = ""
        Dim x As Integer = 0
        Dim InvNo As String = ""
        Dim Type As String = ""
        Dim Total As Integer = Ds.Tables(0).Rows.Count
        Dim Labour_SACCode As String = Read_Settings("Labour_SACCode")
        Dim Value As String = ""
        Dim Taxable As Decimal = 0.0
        'Dim Qty As Decimal = 0.0
        Dim iGST As Decimal = 0.0
        Dim cn As New MySqlConnection(ConnectionString)
        Dim cmd As New MySqlCommand
        Dim BillType As String = ""
        Dim ContinueFor As Boolean = False
        Dim Dr As TallyDs.PartDetailsRow
        Dim PartDs As New TallyDs
        Dim Grand_Total As Decimal = 0.00
        Dim Details As String = ""
        Dim JobCardNo As String = ""
        Dim jDate As Date
        Dim iDate As Date
        Dim JobCardDate As String = ""
        Dim CustAddress As String = ""
        Dim InvoiceNo As String = ""
        Dim InvoiceDate As String = ""
        Dim TabelEntry As Boolean = False
        Dim Name = "", address = "", mobile = "", email As String = ""
        Dim RegNo = "", Chaseno = "", engNo = "", model = "", modelName As String = ""
        Dim OrderAlloc = "", SalePerson = "", SupGST = "", RecGST = "", POS = "", HSN = "", UOM = "", AMtAfDisc = "", TotTax = "", RndOff As String = ""
        Dim sno = -1, labourcode = -1, labourdesc = -1, partcode = -1, partdesc = -1, qty = -1, P_rate = -1, L_rate = -1, P_disc = -1, L_disc As Integer = -1
        Dim sgst = -1, sgst_per = -1, cgst = -1, cgst_per = -1, P_amount = -1, L_amount = -1, L_totalAmt = -1, P_totalAmt As Integer = -1
        Dim CustDetails = -1, JobCNo = -1, JobCDate = -1, ServTaxNo = -1, InvNum = -1, InvoDate = -1, Tempval As Integer = -1
        Dim OrdrAlloc = -1, SalePer = -1, SpGST = -1, ReGST = -1, Ps = -1, HSN_C = -1, UO_M = -1, AftrDisc = -1, Tot_Tax = -1, rndof As Integer = -1
        Dim TypeOfInv As String = ""
        Dim is_SSI As Boolean = False
        Try


            cmd.Connection = cn
            cn.Open()

            x = 0
            Dim i As Integer = 0
            Dim MaxCol As Integer = Ds.Tables(0).Columns.Count



            For Each Drd In Ds.Tables(0).Rows


                For i = 0 To MaxCol - 1

                    If Drd(i).ToString <> "" And is_SSI <> True Then
                        If Drd(i).ToString.Contains("TAX INVOICE - PARTS") Then
                            is_SSI = True
                        End If
                    End If

                    If is_SSI = True Then

                        If Val(OrderAlloc) > 0 And Drd(i) <> "" And Drd(i) <> ":" Then
                            OrderAlloc = ""
                            OrdrAlloc = i
                        End If
                        If Val(CustAddress) > 0 And Drd(i) <> "" And Drd(i) <> ":" Then
                            CustAddress = ""
                            CustDetails = i
                        End If
                        If Val(InvoiceNo) > 0 And Drd(i) <> "" And Drd(i) <> ":" Then
                            InvoiceNo = ""
                            InvNum = i
                        End If
                        If Val(InvoiceDate) > 0 And Drd(i) <> "" And Drd(i) <> ":" Then
                            InvoiceDate = ""
                            InvoDate = i
                        End If
                        If Val(SalePerson) > 0 And Drd(i) <> "" And Drd(i) <> ":" Then
                            SalePerson = ""
                            SalePer = i
                        End If

                        If Val(SupGST) > 0 And Drd(i) <> "" And Drd(i) <> ":" Then
                            SupGST = ""
                            SpGST = i
                        End If

                        If Val(RecGST) > 0 And Drd(i) <> "" And Drd(i) <> ":" Then
                            RecGST = ""
                            ReGST = i
                        End If

                        If Val(POS) > 0 And Drd(i) <> "" And Drd(i) <> ":" Then
                            POS = ""
                            Ps = i
                        End If

                        If Val(TotTax) > 0 And Drd(i) <> "" And P_amount > 0 Then
                            TotTax = ""
                            Tot_Tax = i
                        End If

                        If Val(RndOff) > 0 And Drd(i) <> "" And Tot_Tax > 0 Then
                            RndOff = ""
                            rndof = i
                        End If

                        If Val(Grand_Total) > 0 And Drd(i) <> "" And rndof > 0 Then
                            Grand_Total = 0.0
                            P_totalAmt = i
                        End If

                        If Drd(i).ToString.Contains("Order/Allocation") And OrdrAlloc < 0 Then
                            OrderAlloc = i
                        ElseIf Drd(i).ToString.Contains("Customer Name") And CustDetails < 0 Then
                            CustAddress = i
                        ElseIf Drd(i).ToString.Contains("Invoice No") And InvNum < 0 Then
                            InvoiceNo = i
                        ElseIf Drd(i).ToString.Contains("Invoice Date") And InvoDate < 0 Then
                            InvoiceDate = i
                        ElseIf Drd(i).ToString.Contains("Sales Person") And SalePer < 0 Then
                            SalePerson = i
                        ElseIf Drd(i).ToString.Contains("Supplier GSTIN") And SpGST < 0 Then
                            SupGST = i
                        ElseIf Drd(i).ToString.Contains("Recipient GSTIN") And ReGST < 0 Then
                            RecGST = i
                        ElseIf Drd(i).ToString.Contains("POS") And Ps < 0 Then
                            POS = i
                        ElseIf Drd(i).ToString.Contains("DESCRIPTION") Then
                            partdesc = i
                        ElseIf Drd(i).ToString.Contains("PART NO") Then
                            partcode = i
                        ElseIf Drd(i).ToString.Contains("S No") Then
                            sno = i
                        ElseIf Drd(i).ToString.Contains("HSN No.") Then
                            HSN_C = i
                        ElseIf Drd(i).ToString.Contains("UOM") Then
                            UO_M = i
                        ElseIf Drd(i).ToString.Contains("QTY") Then
                            qty = i
                        ElseIf Drd(i).ToString.Contains("RATE") Then
                            P_rate = i
                        ElseIf Drd(i).ToString.Contains("DISCOUNT") And P_disc < 0 Then
                            P_disc = i
                        ElseIf Drd(i).ToString.Contains("AMOUNT AFTER DISCOUNT") Then
                            AftrDisc = i
                        ElseIf Drd(i).ToString.Contains("SGST%") Then
                            sgst_per = i
                        ElseIf Drd(i).ToString.Contains("SGST") Then
                            sgst = i
                        ElseIf Drd(i).ToString.Contains("CGST%") Then
                            cgst_per = i
                        ElseIf Drd(i).ToString.Contains("CGST") Then
                            cgst = i
                        ElseIf Drd(i).ToString.Contains("AMOUNT") Then
                            P_amount = i
                        ElseIf Drd(i).ToString.Contains("TOTAL TAX") And P_amount > 0 Then
                            TotTax = i
                        ElseIf Drd(i).ToString.Contains("ROUND OFF") And Tot_Tax > 0 Then
                            RndOff = i
                        ElseIf Drd(i).ToString.Contains("TOTAL") And rndof > 0 And P_totalAmt < 0 Then
                            Grand_Total = i
                        End If

                    Else

                        If Val(CustAddress) > 0 And Drd(i).ToString <> "" Then
                            CustAddress = ""
                            CustDetails = i
                        End If

                        If Val(JobCardNo) > 0 And Drd(i).ToString <> "" Then
                            JobCardNo = ""
                            JobCNo = i
                        End If

                        If Val(JobCardDate) > 0 And Drd(i).ToString <> "" Then
                            JobCardDate = ""
                            JobCDate = i
                        End If

                        If Val(Details) > 0 And Drd(i).ToString <> "" Then
                            Details = ""
                            ServTaxNo = i
                        End If
                        If Val(InvoiceDate) > 0 And Drd(i).ToString <> "" Then
                            InvoiceDate = ""
                            InvoDate = i
                        End If

                        If Val(InvoiceNo) > 0 And Drd(i).ToString <> "" Then
                            InvoiceNo = ""
                            InvNum = i
                        End If

                        If Val(Grand_Total) > 0 And Drd(i).ToString <> "" And partdesc < 0 Then
                            Grand_Total = 0.0
                            L_totalAmt = i
                        ElseIf Val(Grand_Total) > 0 And Drd(i).ToString <> "" And partdesc > 0 Then
                            Grand_Total = 0.0
                            P_totalAmt = i
                        End If

                        If Drd(i).ToString.Contains("S No") Then
                            sno = i
                        ElseIf Drd(i).ToString.Contains("Labour Code") Then
                            labourcode = i
                        ElseIf Drd(i).ToString.Contains("LABOUR DESCRIPTION") Then
                            labourdesc = i
                        ElseIf Drd(i).ToString.Contains("PART  DESCRIPTION") Then
                            partdesc = i
                        ElseIf Drd(i).ToString.Contains("Part Code") Then
                            partcode = i
                        ElseIf Drd(i).ToString.Contains("RATE(Rs)") And labourdesc > 0 And L_disc < 0 Then
                            L_rate = i
                        ElseIf Drd(i).ToString.Contains("RATE(Rs)") And partdesc > 0 Then
                            P_rate = i
                        ElseIf Drd(i).ToString.Contains("QTY") Then
                            qty = i
                        ElseIf Drd(i).ToString.Contains("DISCOUNT") And labourdesc > 0 And L_disc < 0 Then
                            L_disc = i
                        ElseIf Drd(i).ToString.Contains("DISCOUNT") And partdesc > 0 Then
                            P_disc = i
                        ElseIf Drd(i).ToString.Contains("SGST%") Then
                            sgst_per = i
                        ElseIf Drd(i).ToString.Contains("SGST") Then
                            sgst = i
                        ElseIf Drd(i).ToString.Contains("CGST%") Then
                            cgst_per = i
                        ElseIf Drd(i).ToString.Contains("CGST") Then
                            cgst = i
                        ElseIf Drd(i).ToString.Contains("TOTAL AMOUNT(Rs)") Then
                            L_amount = i
                        ElseIf Drd(i).ToString.Contains("AMOUNT(Rs)") Then
                            P_amount = i
                        ElseIf Drd(i).ToString.Contains("TOTAL") And partdesc < 0 Then
                            Grand_Total = i
                        ElseIf Drd(i).ToString.Contains("GRAND TOTAL") And partdesc > 0 Then
                            Grand_Total = i
                        ElseIf Drd(i).ToString.Contains("CUSTOMER NAME & ADDRESS") And CustDetails < 0 Then
                            CustAddress = i
                        ElseIf Drd(i).ToString.Contains("JOB CARD NO") And JobCNo < 0 Then
                            JobCardNo = i
                        ElseIf Drd(i).ToString.Contains("JOB CARD DATE") And JobCDate < 0 Then
                            JobCardDate = i
                        ElseIf Drd(i).ToString.Contains("SERV TAX NO.") And ServTaxNo < 0 Then
                            Details = i
                        ElseIf Drd(i).ToString.Contains("Registration No :") And ServTaxNo < 0 Then
                            ServTaxNo = i
                        ElseIf Drd(i).ToString.Contains("INVOICE NO") And InvNum < 0 Then
                            InvoiceNo = i
                        ElseIf Drd(i).ToString.Contains("INVOICE DATE") And InvoDate < 0 Then
                            InvoiceDate = i
                        End If

                    End If

                Next

            Next

            Dim Datepat As String = "^(((19|20)(([0][48])|([2468][048])|([13579][26]))|2000)[\-](([0][13578]|[1][02])[\-]([012][0-9]|[3][01])|([0][469]|11)[\-]([012][0-9]|30)|02[\-]([012][0-9]))|((19|20)(([02468][1235679])|([13579][01345789]))|1900)[\-](([0][13578]|[1][02])[\-]([012][0-9]|[3][01])|([0][469]|11)[\-]([012][0-9]|30)|02[\-]([012][0-8])))$"
            InvoiceDate = "" : InvoiceNo = "" : Details = "" : CustAddress = "" : JobCardDate = "" : JobCardNo = ""

            If is_SSI Then
                labourcode = 0 : labourdesc = 0 : L_amount = 0 : L_disc = 0 : L_rate = 0 : L_totalAmt = 0
            End If

            For Each DrD In Ds.Tables(0).Rows

                'If BillType = "CLI" Or BillType = "CPI" Then
                '    Continue For 'warranty
                'End If
                If is_SSI <> True Then


                    If DrD(InvNum) <> "" And InvoiceNo = "" Then
                        InvoiceNo = IIf(DrD(InvNum).ToString.Trim <> "", DrD(InvNum).ToString.Trim.Replace(":", ""), "")
                        BillType = Strings.Left(InvoiceNo, 4)
                    ElseIf DrD(InvoDate) <> "" And InvoiceDate = "" Then
                        InvoiceDate = IIf(DrD(InvoDate).ToString.Trim <> "", DrD(InvoDate).ToString.Trim.Replace(":", ""), "")
                    ElseIf DrD(ServTaxNo) <> "" And Details = "" Then
                        Details = IIf(DrD(ServTaxNo).ToString.Trim <> "", DrD(ServTaxNo).ToString.Trim.Replace(":", ""), "")

                        Dim SplitDetails As String() = Details.Split(New String() {vbCr & vbLf, vbLf}, StringSplitOptions.None)

                        For Each line In SplitDetails

                            If RegNo = "" Then
                                RegNo = line.ToString.Trim.Replace("Registration No  ", "")
                            ElseIf line.Contains("Chassis No") Then
                                Chaseno = line.ToString.Trim.Replace("Chassis No", "").Trim
                            ElseIf line.Contains("Engine No") Then
                                engNo = line.ToString.Trim.Replace("Engine No", "").Trim
                            ElseIf line.Contains("Model") And model = "" Then
                                model = line.ToString.Trim.Replace("Model", "").Trim
                            ElseIf line.Contains("Model Name") Then
                                modelName = line.ToString.Trim.Replace("Model Name", "").Trim
                            End If

                        Next

                    ElseIf DrD(JobCNo) <> "" And JobCardNo = "" Then
                        JobCardNo = IIf(DrD(JobCNo).ToString.Trim <> "", DrD(JobCNo).ToString.Trim.Replace(":", ""), "")
                    ElseIf DrD(JobCDate) <> "" And JobCardDate = "" Then
                        JobCardDate = IIf(DrD(JobCDate).ToString.Trim <> "", DrD(JobCDate).ToString.Trim.Replace(":", ""), "")
                    ElseIf DrD(CustDetails) <> "" And CustAddress = "" Then
                        CustAddress = IIf(DrD(CustDetails).ToString.Trim <> "", DrD(CustDetails).ToString.Trim.Replace(":", ""), "")
                        Dim SplitAdsress As String() = CustAddress.Split(New String() {vbCr & vbLf, vbLf}, StringSplitOptions.None)

                        For Each line In SplitAdsress

                            If Name = "" Then
                                Name = line.ToString.Trim
                            ElseIf line.Contains("Mobile") Then
                                mobile = line.ToString.Trim.Replace("Mobile", "").Trim
                            ElseIf line.Contains("Email") Then
                                email = line.ToString.Trim.Replace("Email", "").Trim
                            Else
                                address += line.ToString.Trim
                            End If
                        Next
                    End If

                    If DrD(sno) <> "S No" And (DrD(partcode) <> "" Or DrD(labourcode) <> "") And (DrD(partdesc) <> "" Or DrD(labourdesc) <> "") And DrD(sgst) <> "" And (DrD(L_disc) <> "" Or DrD(P_disc) <> "") Then
                        If DrD(sgst_per) <> "" And DrD(cgst) <> "" And DrD(cgst_per) <> "" And (DrD(P_amount) <> "" Or DrD(L_amount) <> "") And (DrD(L_rate) <> "" Or DrD(P_rate) <> "") Then
                            TabelEntry = True
                        Else
                            TabelEntry = False
                        End If
                    Else
                        TabelEntry = False
                    End If

                    If PartDs.PartDetails.Rows.Count > 0 And (DrD(L_totalAmt) <> "" Or DrD(P_totalAmt) <> "") Then


                        Grand_Total = IIf(Val(DrD(L_totalAmt)) > 0, Val(DrD(L_totalAmt)), Val(DrD(P_totalAmt)))

                    End If

                Else

                    If DrD(InvNum) <> "" And InvoiceNo = "" Then
                        InvoiceNo = IIf(DrD(InvNum).ToString.Trim <> "", DrD(InvNum).ToString.Trim.Replace(":", ""), "")
                        BillType = Strings.Left(InvoiceNo, 4)
                    ElseIf DrD(InvoDate) <> "" And InvoiceDate = "" Then
                        InvoiceDate = IIf(DrD(InvoDate).ToString.Trim <> "", DrD(InvoDate).ToString.Trim.Replace(":", ""), "")
                        InvoiceDate = InvoiceDate.Substring(0, 10)
                    ElseIf DrD(CustDetails) <> "" And CustAddress = "" Then
                        CustAddress = IIf(DrD(CustDetails).ToString.Trim <> "", DrD(CustDetails).ToString.Trim.Replace(":", ""), "")
                        Dim SplitAdsress As String() = CustAddress.Split(New String() {vbCr & vbLf, vbLf}, StringSplitOptions.None)

                        For Each line In SplitAdsress

                            If Name = "" Then
                                Name = line.ToString.Trim
                                Name = Name.Replace("Mr.", "")
                            ElseIf line.Contains("Mobile") Then
                                mobile = line.ToString.Trim.Replace("Mobile", "").Trim

                                mobile = mobile.Split("Phone")(0)

                            ElseIf line.Contains("Email") Then
                                email = line.ToString.Trim.Replace("Email", "").Trim
                            Else
                                address += line.ToString.Trim
                            End If
                        Next
                    ElseIf DrD(SalePer) <> "" And SalePerson = "" Then
                        SalePerson = IIf(DrD(SalePer).ToString.Trim <> "", DrD(SalePer).ToString.Trim.Replace(":", ""), "")
                    ElseIf DrD(SpGST) <> "" And SupGST = "" Then
                        SupGST = IIf(DrD(SpGST).ToString.Trim <> "", DrD(SpGST).ToString.Trim.Replace(":", ""), "")
                    ElseIf DrD(Ps) <> "" And POS = "" Then
                        POS = IIf(DrD(Ps).ToString.Trim <> "", DrD(Ps).ToString.Trim.Replace(":", ""), "")
                    ElseIf DrD(ReGST) <> "" And RecGST = "" Then
                        RecGST = IIf(DrD(ReGST).ToString.Trim <> "", DrD(ReGST).ToString.Trim.Replace(":", ""), "")
                    End If



                    If DrD(sno) <> "S No" And DrD(partcode) <> "" And DrD(partdesc) <> "" And DrD(sgst) <> "" And DrD(P_disc) <> "" Then
                        If DrD(sgst_per) <> "" And DrD(cgst) <> "" And DrD(cgst_per) <> "" And DrD(P_amount) <> "" And DrD(P_rate) <> "" Then
                            TabelEntry = True
                        Else
                            TabelEntry = False
                        End If
                    Else
                        TabelEntry = False
                    End If


                    If PartDs.PartDetails.Rows.Count > 0 And (DrD(P_totalAmt) <> "") Then


                        Grand_Total = IIf(Val(DrD(P_totalAmt)) > 0, Val(DrD(P_totalAmt)), Val(DrD(P_totalAmt)))
                        If Grand_Total < DrD(P_totalAmt) Then
                            Grand_Total = DrD(P_totalAmt)

                        End If
                    End If

                End If

                If TabelEntry Then

                    Dr = PartDs.PartDetails.Rows.Add
                    Dr.S_No = IIf(DrD(sno).ToString.Trim <> "", Val(DrD(sno).ToString.Trim), "")
                    Dr.Part_Labour_Code = IIf(DrD(partcode).ToString.Trim <> "", DrD(partcode).ToString.Trim, DrD(labourcode).ToString.Trim)
                    Dr.Part_Labour_Description = IIf(DrD(partdesc).ToString.Trim <> "", DrD(partdesc).ToString.Trim, DrD(labourdesc).ToString.Trim)
                    Dr.Qty = IIf(DrD(partdesc).ToString.Trim <> "", DrD(qty), "1")
                    Dr.Value = IIf(DrD(partdesc).ToString.Trim <> "", DrD(P_rate), DrD(L_rate))
                    'Dr.Qty = IIf(qty <> L_rate, Val(DrD(qty).ToString.Trim), "1")
                    'Dr.Rate = IIf(Val(DrD(P_rate)) < 0, Val(DrD(P_rate).ToString.Trim), Val(DrD(L_rate).ToString.Trim))
                    If is_SSI Then
                        Dr.Discount = IIf(DrD(P_disc).ToString.Trim <> "", Val(DrD(P_disc).ToString.Trim), 0)
                    Else
                        Dr.Discount = IIf(DrD(L_disc).ToString.Trim <> "", Val(DrD(L_disc).ToString.Trim), Val(DrD(P_disc).ToString.Trim))
                    End If
                    Dr.SGST = IIf(DrD(sgst).ToString.Trim <> "", Val(DrD(sgst).ToString.Trim), "")
                    Dr.SGST_Per = IIf(DrD(sgst_per).ToString.Trim <> "", Val(DrD(sgst_per).ToString.Trim), "")
                    Dr.CGST = IIf(DrD(cgst).ToString.Trim <> "", Val(DrD(cgst).ToString.Trim), "")
                    Dr.CGST_Per = IIf(DrD(cgst_per).ToString.Trim <> "", Val(DrD(cgst_per).ToString.Trim), "")
                    Dr.Amount = IIf(DrD(P_amount).ToString.Trim <> "", (DrD(P_amount)), DrD(L_amount))
                    If is_SSI Then
                        Dr.HSN_No = IIf(is_SSI = True, DrD(HSN_C), "")
                        Dr.UOM = IIf(is_SSI = True, DrD(UO_M), "")
                        Dr.Taxable_Amount = Val(DrD(AftrDisc))
                    Else
                        HSN_C = 0
                        UO_M = 0
                        Dr.HSN_No = ""
                        Dr.UOM = ""
                        'If Dr.Qty < 1 Then
                        '    Dr.Taxable_Amount = (Dr.Rate / Dr.Qty)
                        'Else
                        Dr.Taxable_Amount = (Dr.Value * Dr.Qty)

                        'End If

                    End If

                    PartDs.AcceptChanges()

                End If

            Next

            If BillType.Contains("SP") Then
                TypeOfInv = "Item"
            ElseIf BillType.Contains("SL") Then
                TypeOfInv = "Labour"
            ElseIf BillType.Contains("SSI") Then
                TypeOfInv = "Item"
            Else
                TypeOfInv = ""
            End If

            iDate = Convert.ToDateTime(InvoiceDate)

            If is_SSI Then
                jDate = iDate
                JobCardNo = InvoiceNo
            Else
                jDate = Convert.ToDateTime(JobCardDate)

            End If

            Qry = " Delete from service where `Invoice_Number` = '" & InvoiceNo.Trim & "' ; "

            For Each Dr In PartDs.PartDetails.Rows



                Qry += "INSERT INTO Service (" &
                       "Job_Card,Invoice_Number,Job_Card_Date," &
                       "Reg_No,Chassis_No,Engine_No,Model_Code,Model_Description," &
                       "Customer_Name,Mobile_No,Customer_Voice,Type,HSN_SAC_code," &
                       "Part_Labour_Code,Part_Labour_Description," &
                       "Rate,Issued_Qty,Discount,Taxable," &
                       "CGST_Per,CGST,SGST_Per,SGST,Total_Amount) Values (" &
                        "'" & JobCardNo & "','" & InvoiceNo.Trim & "','" & Format(jDate, "yyyy-MM-dd") & "'," &
                        "'" & RegNo & "','" & Chaseno & "','" & engNo & "','" & model & "','" & modelName & "'," &
                        "'" & Name & "','" & "" & "','" & CustDetails & "','" & TypeOfInv & "','" & Dr.HSN_No & "'," &
                        "'" & Dr.Part_Labour_Code & "','" & Dr.Part_Labour_Description & "'," &
                        "'" & Dr.Value & "','" & Dr.Qty & "'," &
                        "'" & Dr.Discount & "','" & Dr.Taxable_Amount & "'," &
                        "'" & Dr.CGST_Per & "','" & Dr.CGST & "'," &
                        "'" & Dr.SGST_Per & "','" & Dr.SGST & "','" & Dr.Amount & "" &
                         "'); "

            Next

            Qry += " Delete from service_header where Invoice_Date = '" & Format(jDate, "yyyy-MM-dd") & "' and `Invoice_Number` = '" & InvoiceNo.Trim & "' ; "
            Qry += " INSERT INTO Service_Header (Invoice_Number,Invoice_Date,Invoice_Amount) Values (" &
                            "'" & InvoiceNo.Trim & "','" & Format(iDate, "yyyy-MM-dd") & "'," &
                            "'" & Val(Grand_Total) & "'); "


            If Qry <> "" Then
                cmd.CommandText = Qry
                cmd.ExecuteNonQuery()
            End If

            cn.Close()
            cn = Nothing

            Status = InvoiceNo + "TRUE"

        Catch ex As Exception
            Status = InvoiceNo
            MsgBox("Error Found : " & ex.Message, vbCritical)
        End Try

        Return Status

    End Function


    Public Shared Function Insert_PurchaseVehicle(ByVal Ds As TallyDs, ByRef LblStat As Label) As String

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim da As New MySqlDataAdapter
        Dim cmd As New MySqlCommand
        Dim Objds As New TallyDs
        Dim Id As Integer = 0
        Dim Qry = "", Qry_Checking = "", Qry_Check = "", Qry_Insert = "", Qry_Delete As String = ""
        Dim Status As Boolean

        Dim invdate As String = ""

        Dim Doc_Date As Date
        Dim InvNo As String = ""

        Dim Model As String = ""
        Dim Freight As Double = Val(Read_Ledgers("Freight_Amount"))
        Dim No_Freight_Depo As String = Read_Ledgers("Freight_Depo")
        Dim sgst_p = 0, cgst_p = 0, freigt = 0, cess_p As Double = 0
        Dim sgst = 0, cgst = 0, cess As Double = 0

        Dim TDS As New TallyDs
        Dim TepDS As New TallyDs
        Dim BillType As String = ""
        Dim DrD As TallyDs.PurchaseVehicleRow
        Dim dr As TallyDs.veh_masterRow
        Dim Taxable_amt As Double = 0
        Dim Total_amt As Decimal
        Dim Missing_Stock As String = ""
        cmd.Connection = cn
        cn.Open()

        Qry = "select * FROM veh_master"
        da.SelectCommand = cmd
        cmd.CommandText = Qry
        da.Fill(TDS, "veh_master")

        cn.Close()
        cn = Nothing

        Dim total = Ds.PurchaseVehicle.Rows.Count

        Dim y = 100 / total
        Dim x = 100 / total
        x -= y
        Dim z As String = ""
        Dim StatusString As String = "...................................................................................................."
        Dim Regex = New Regex(".")



        Try



            For Each DrD In Ds.PurchaseVehicle.Rows




                If DrD("Model Description") <> "" And DrD("RE Invoice No") <> "" And DrD("Chassis No") <> "" Then

                    TepDS = New TallyDs
                    TepDS.Merge(TDS.veh_master.Select("ModelFamily = '" & DrD("Model Description") & "' "))

                    If TepDS.veh_master.Rows.Count = 0 Then
                        Missing_Stock = Missing_Stock & DrD("Model Description") & vbCrLf
                    ElseIf TepDS.veh_master.Select("SGST_Per > 0 and CGST_Per > 0").Count = 0 Then

                        Missing_Stock = Missing_Stock & DrD("Model Description") & "Tax Values Not Found In Vehicle Master" & vbCrLf


                    Else

                    End If

                End If
            Next

            If Missing_Stock <> "" Then
                Return Missing_Stock
            End If

            '   Dim Total As Integer = Ds.PurchaseVehicle.Select("`Chassis No`<>'' and `Model Code`<>'' ").Count


            ' RunQuery("TRUNCATE `re_spare_purchase`")





            For Each DrD In Ds.PurchaseVehicle.Rows




                x = x + y
                If Math.Round(x, 0) > Math.Round(Val(z), 0) Then
                    '  StatusString += "|"
                    StatusString = Regex.Replace(StatusString, "|", CInt(Val(x)))
                    ' StatusString = Replace(StatusString, ".", "", , 4)
                Else

                End If
                z = (Math.Round(x, 0)).ToString

                LblStat.Text = "Inserting  " + StatusString + "  " + z + "/100"
                ' LblStat.Text = LblStat.Text.Replace("-", "\")
                LblStat.Refresh()


                BillType = Strings.Left(DrD("F2"), 3)

                If BillType = "DPR" Then
                    Continue For
                End If

                If (DrD("Document Name") <> "") Then
                    Doc_Date = DateSerial((Right(DrD("Document Name"), 4)), (Mid(DrD("Document Name"), 4, 2)), (Left(DrD("Document Name"), 2)))
                End If

                If InvNo <> DrD("RE Invoice No") Then

                    Qry_Delete = " Delete from PurchaseVehicle where `Doc_Date` = '" & Format(Doc_Date, "yyyy-MM-dd") & "' and `Invoice_No` = '" & DrD("RE Invoice No") & "'  and PostedToTally=0 ; "
                    Status = RunQuery(Qry_Delete)
                    InvNo = DrD("RE Invoice No")

                End If


                If Model <> DrD("Model Description") Then

                    sgst_p = 0
                    cgst_p = 0
                    cess_p = 0

                    TepDS = New TallyDs
                    TepDS.Merge(TDS.veh_master.Select("ModelFamily = '" & DrD("Model Description") & "' "))

                    If TepDS.veh_master.Rows.Count > 0 Then

                        dr = TepDS.veh_master.Rows(0)

                        sgst_p = dr.SGST_Per
                        cgst_p = dr.CGST_Per
                        cess_p = dr.CESS_Per

                    End If

                    Model = DrD("Model Description")

                End If

                Taxable_amt = 0
                Total_amt = 0

                Try
                    Dim PId = "0"
                    Qry_Check = " select Invoice_Number from PurchaseVehicle where `Doc_Date` = '" & Format(Doc_Date, "yyyy-MM-dd") & "' and `Invoice_No` = '" & DrD("RE Invoice No") & "' and PostedToTally=1 ; "
                    cmd.CommandText = Qry_Check
                    PId = cmd.ExecuteScalar()
                    If PId = "0" Then
                    Else
                        Continue For
                    End If
                Catch ex As Exception
                    Create_Log("", ex.Message, "")
                End Try


                If DrD("RE Invoice No") <> "" And DrD("Chassis No") <> "" Then





                    If No_Freight_Depo.Contains(DrD("Depot")) Then
                        DrD.Freight = Freight
                        Taxable_amt = Freight + Val(DrD("Rate"))
                    Else
                        DrD.Freight = 0
                        Taxable_amt = Val(DrD("Rate"))
                    End If
                    DrD.Freight = Format(Val(DrD("FreightCharge")), "0.00")
                    DrD.SGST = Format(Val(DrD.SGST), "0.00")
                    DrD.CGST = Format(Val(DrD.CGST), "0.00")
                    DrD.CESS = Format(Val(DrD.CESS), "0.00")
                    Total_amt = DrD.Freight + Val(DrD("Rate")) + DrD.SGST + DrD.CGST + DrD.CESS

                    If Total_amt <> Val(DrD("Total Amount")) Then
                        Create_Log("Total Mismatch in Bill", DrD("RE Invoice No").ToString, "Sum Of:" & Total_amt.ToString & "<> Bill Total" & Val(DrD("Total Amount")))
                    End If




                    Qry_Insert = "Insert into purchasevehicle(Doc_Date,Document_No,Company,Store,Depot,ModelFamily,Invoice_Date, " &
                                 " Model_Code,Description,Chassis_No,Engine_No," &
                                 " MFG_Date,Invoice_No,Remarks,Rate,Amount,Total_Amount,SGST_Per,CGST_Per,CESS_Per,SGST,CGST,CESS,Freight) Values (" &
                            "'" & Format(Doc_Date, "yyyy-MM-dd") & "','" & DrD("F2") & "','" & DrD("Company") & "', " &
                            "'" & DrD("Store") & "','" & DrD("Depot") & "','" & DrD("Model Family") & "' ,'" & Format(DrD("RE Invoice Date"), "yyyy-MM-dd") & "', " &
                            "'" & DrD("Model Code") & "','" & CommonDA.ReplaceQuote(DrD("Model Description")) & "','" & DrD("Chassis No") & "', " &
                            "'" & DrD("Engine No") & "','" & Format(DrD("Mfg Month"), "yyyy-MM-dd") & "','" & DrD("RE Invoice No") & "','" & DrD("Remarks") & "'," &
                            "'" & Val(DrD("Rate")) & "','" & Val(DrD("Amount")) & "','" & Val(DrD("Total Amount")) & "', " &
                            "'" & sgst_p & "','" & cgst_p & "','" & cess_p & "','" & DrD.SGST & "','" & DrD.CGST & "','" & DrD.CESS & "','" & DrD.Freight & "' ); "

                    Status = RunQuery(Qry_Insert)

                    DrD.To_Tally = 1
                    sgst = 0
                    cgst = 0
                    cess = 0
                    freigt = 0

                    'LblStatus.Text = "Import Status : " & Total & "/" & x
                    'LblStatus.Refresh()

                End If

                ' Status = RunQuery(Qry_Delete + Qry_Insert)

            Next

        Catch ex As Exception

        End Try
        'LblStatus.Text = "Ending ..."
        'LblStatus.Refresh()

        Return "True"

    End Function

    Public Shared Function Insert_CashRegister(ByVal Ds As TallyDs, ByRef LblStat As Label) As String

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim da As New MySqlDataAdapter
        Dim cmd As New MySqlCommand
        Dim Objds As New TallyDs
        Dim Id As Integer = 0
        Dim Qry = "", Qry_Check = "", Qry_Insert = "", Qry_Delete As String = ""


        'Dim dr As TallyDs.Cash_RegisterRow



        Dim total = Ds.Tables("Cash_Register").Rows.Count

        Dim y = 100 / total
        Dim x = 100 / total
        x -= y
        Dim z As String = ""
        Dim StatusString As String = "...................................................................................................."
        Dim Regex = New Regex(".")

        Try

            cmd.Connection = cn
            cn.Open()

            For Each dr In Ds.Tables("Cash_Register").Rows

                x = x + y
                If Math.Round(x, 0) > Math.Round(Val(z), 0) Then
                    '  StatusString += "|"
                    StatusString = Regex.Replace(StatusString, "|", CInt(Val(x)))
                    ' StatusString = Replace(StatusString, ".", "", , 4)
                Else

                End If
                z = (Math.Round(x, 0)).ToString

                LblStat.Text = "Inserting  " + StatusString + "  " + z + "/100"
                ' LblStat.Text = LblStat.Text.Replace("-", "\")
                LblStat.Refresh()

                Try

                    Id = 0
                    Qry_Check = "select id from Cash_Register where Id='" & dr("Id") & "'"
                    cmd.CommandText = Qry_Check
                    Id = cmd.ExecuteScalar()

                Catch ex As Exception
                    Id = 0
                End Try

                Dim ModifiedOn As String = ""

                Dim a = dr("Modified_On")

                Try
                    If Format(dr("Modified_On"), "yyyy-MM-dd") = "yyyy-MM-dd" Then
                        ModifiedOn = Format(dr("Created_On"), "yyyy-MM-dd")
                    Else
                        ModifiedOn = Format(dr("Modified_On"), "yyyy-MM-dd")
                    End If
                Catch
                    ModifiedOn = Format(DateAndTime.Now, "yyyy-MM-dd")
                End Try


                If Id = 0 Then

                    Qry_Insert = "insert Into Cash_Register (Id,Entry_No,Entry_Date,Entry_Type,Amount,Ref_No,Entry_Head,Entry_sub_head,Remarks,Party,EntryHead_Id," &
                          "EntrySubHead_Id,Branch_Id,BkId,PayId,Payment_Mode,Bank,User_Id,User_Name,Created_On,Modified_On,Prefix_RefNo,JobCardNo)" &
                          "values("

                    Qry = "'" & dr("Id") & "','" & dr("Entry_No") & "','" & Format(dr("Entry_Date"), "yyyy-MM-dd") & "','" & dr("Entry_Type") & "','" & dr("Amount") & "'," &
                          "'" & dr("Ref_No") & "','" & dr("Entry_Head") & "','" & dr("Entry_sub_head") & "','" & dr("Remarks") & "','" & dr("Party") & "','" & dr("EntryHead_Id") & "'," &
                          "'" & dr("EntrySubHead_Id") & "','" & dr("Branch_Id") & "','" & dr("BkId") & "','" & dr("PayId") & "','" & dr("Payment_Mode") & "'," &
                          "'" & dr("Bank") & "','" & dr("User_Id") & "','" & dr("User_Name") & "','" & Format(dr("Created_On"), "yyyy-MM-dd") & "','" & ModifiedOn & "'," &
                          "'" & dr("Prefix Ref No") & "','" & dr("JobCardNo") & "')"


                    cmd.CommandText = Qry_Insert & Qry
                    cmd.ExecuteNonQuery()

                End If






            Next

        Catch ex As Exception

        End Try
        'LblStatus.Text = "Ending ..."
        'LblStatus.Refresh()

        Return "True"

    End Function









    'Public Shared Function Get_App_Request(ByVal Htp As Hashtable, New_Entries As Boolean) As EnquiryDS

    '    Dim cn As New MySqlConnection(CommonDA.ConnectionString)
    '    Dim cmd As New MySqlCommand
    '    Dim Da As New MySqlDataAdapter
    '    Dim strQuery As String
    '    Dim Ds As New EnquiryDS

    '    cmd.Connection = cn
    '    cn.Open()

    '    Try
    '        strQuery = " Select a.*, b.Prospect_No from app_request a left join enquiry_header b On a.Enq_HeaderId = b.Enq_HeaderId where 1 = 1 "

    '        If New_Entries Then
    '            strQuery = strQuery & " And Req_Accepted = 0 "
    '        End If

    '        If Not Htp Is Nothing Then
    '            For Each Key As Object In Htp.Keys
    '                Select Case Key
    '                    Case EnquiryDS.SearchAppRequestBy.Enq_HeaderId
    '                        strQuery = strQuery & " And a.Enq_HeaderId = '" & Htp(Key).ToString & "'"
    '                    Case EnquiryDS.SearchAppRequestBy.Req_Accepted
    '                        strQuery = strQuery & " And a.Req_Accepted = '" & Htp(Key).ToString & "'"
    '                    Case EnquiryDS.SearchAppRequestBy.Req_AcceptedBy
    '                        strQuery = strQuery & " And a.Req_AcceptedBy = '" & Htp(Key).ToString & "'"
    '                    Case EnquiryDS.SearchAppRequestBy.Req_AcceptedOn
    '                        strQuery = strQuery & " And a.Req_AcceptedOn = '" & Htp(Key).ToString & "'"
    '                    Case EnquiryDS.SearchAppRequestBy.Req_ContactNo
    '                        strQuery = strQuery & " And a.Req_ContactNo Like '%" & CommonDA.ReplaceQuote(Htp(Key).ToString) & "%'"
    '                    Case EnquiryDS.SearchAppRequestBy.Req_DateTime
    '                        strQuery = strQuery & " And a.Req_DateTime = '" & Htp(Key).ToString & "'"
    '                    Case EnquiryDS.SearchAppRequestBy.Req_Id
    '                        strQuery = strQuery & " And a.Req_Id = '" & Htp(Key).ToString & "'"
    '                    Case EnquiryDS.SearchAppRequestBy.Req_Name
    '                        strQuery = strQuery & " And a.Req_Name Like '%" & Htp(Key).ToString & "%'"
    '                    Case EnquiryDS.SearchAppRequestBy.Req_Vehicle
    '                        strQuery = strQuery & " And a.Req_Vehicle Like '%" & Htp(Key).ToString & "%'"
    '                    Case EnquiryDS.SearchAppRequestBy.Req_Vehicle_No
    '                        strQuery = strQuery & " And a.Req_Vehicle_No Like '%" & Htp(Key).ToString & "%'"
    '                    Case EnquiryDS.SearchAppRequestBy.Req_Type
    '                        strQuery = strQuery & " And a.Req_Type = '" & Htp(Key).ToString & "'"
    '                    Case EnquiryDS.SearchAppRequestBy.Req_Place
    '                        strQuery = strQuery & " And a.Req_Place Like '%" & Htp(Key).ToString & "%'"
    '                End Select
    '            Next
    '        End If


    '        strQuery = strQuery & " Order by Req_Id Desc "

    '        cmd.CommandText = strQuery
    '        Da.SelectCommand = cmd
    '        Da.Fill(Ds, "App_Request")

    '    Catch ex As Exception
    '        cn = Nothing
    '    End Try

    '    cn.Close()
    '    cn = Nothing
    '    Return Ds

    'End Function

    Public Shared Function Accept_App_Request(ByVal Req_Id As Integer, ByVal Req_AcceptedBy As Integer,
                                              ByVal Req_AcceptedUserType As String, ByVal Req_AcceptedUserName As String) As Boolean

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        Dim Da As New MySqlDataAdapter
        Dim strQuery As String
        Dim Status As Boolean = True

        cmd.Connection = cn
        cn.Open()

        Try
            strQuery = " Update app_request set Req_Accepted = '1'," &
                        " Req_AcceptedBy = '" & Req_AcceptedBy & "'," &
                        " Req_AcceptedUserType = '" & Req_AcceptedUserType & "'," &
                        " Req_AcceptedUserName = '" & Req_AcceptedUserName & "'," &
                        " Req_AcceptedOn = '" & Format(Date.Now, "yyyy-MM-dd HH:mm:ss") & "'" &
                        " where Req_Id = '" & Req_Id & "'"
            cmd.CommandText = strQuery
            cmd.ExecuteNonQuery()

            strQuery = " update app_request a inner join enquiry_header b on a.Req_ContactNo = b.Cust_Tel_Mob " &
                        " set a.Enq_HeaderId = b.Enq_HeaderId " &
                        " where a.Enq_HeaderId = 0 and a.Req_Id = '" & Req_Id & "'"
            cmd.CommandText = strQuery
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Status = False
        End Try

        cn.Close()
        cn = Nothing
        Return Status

    End Function


    Public Shared Function Merge_App_Request_With_Enquiry() As Boolean

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        Dim Da As New MySqlDataAdapter
        Dim strQuery As String
        Dim Status As Boolean = True

        cmd.Connection = cn
        cn.Open()

        Try
            strQuery = " update app_request a inner join enquiry_header b on a.Req_ContactNo = b.Cust_Tel_Mob " &
                        " set a.Enq_HeaderId = b.Enq_HeaderId " &
                        " where a.Enq_HeaderId = 0 "
            cmd.CommandText = strQuery
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Status = False
        End Try

        cn.Close()
        cn = Nothing
        Return Status

    End Function

    Public Function Load_Settings() As CompDS


        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim da As New MySqlDataAdapter
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Ds As New CompDS

        Try

            cmd.Connection = cn
            cn.Open()

            'Settings
            strQuery = " select S_Key,S_Value,Branch from settings Where 1 = 1 "
            cmd.CommandText = strQuery
            da.SelectCommand = cmd
            da.Fill(Ds, "settings")

            'Settings
            strQuery = " select S_Key,S_Value,Branch from ledgers Where 1 = 1 "
            cmd.CommandText = strQuery
            da.SelectCommand = cmd
            da.Fill(Ds, "ledgers")

            cn.Close()
            cn = Nothing

        Catch ex As Exception
            cn = Nothing
        End Try

        Return Ds

    End Function
    Public Function Save_Serv_Labour_Settings(ByVal Dr As TallyDs.Service_LedgersRow) As Boolean
        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        Dim strQuery As String

        Try

            cmd.Connection = cn
            cn.Open()


            If Dr.Id > 0 Then
                strQuery = " Update service_ledgers Set PartNo = '" & Dr.PartNo & "'," &
                                " PartDescription = '" & CommonDA.ReplaceRightSlash(Dr.PartDescription) & "'," &
                                " LedgerName = '" & Dr.LedgerName & "' " &
                                " Where Id = '" & Dr.Id & "'"
                cmd.CommandText = strQuery
                cmd.ExecuteNonQuery()
            Else

                strQuery = " Insert into service_ledgers (PartNo,PartDescription,LedgerName) " &
                               " Values(" &
                               "'" & Dr.PartNo & "'," &
                               "'" & CommonDA.ReplaceRightSlash(Dr.PartDescription) & "'," &
                               "'" & Dr.LedgerName & "')"
                cmd.CommandText = strQuery
                Dr.Id = cmd.ExecuteScalar
            End If


            cn.Close()
            cn = Nothing
        Catch ex As Exception
            cn = Nothing
            Return False
        End Try

        Return True
    End Function

    Public Function Save_Settings(ByVal Dr As CompDS.SettingsRow, ByVal Is_Leger As Boolean) As Boolean

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        Dim strQuery As String

        Try

            cmd.Connection = cn
            cn.Open()
            If Is_Leger <> True Then

                If Dr.Id > 0 Then
                    strQuery = " Update settings Set S_Key = '" & Dr.S_Key & "'," &
                                " S_Value = '" & CommonDA.ReplaceRightSlash(Dr.S_Value) & "'," &
                                " Branch = '" & Dr.Branch & "' " &
                                " Where Id = '" & Dr.Id & "'"
                    cmd.CommandText = strQuery
                    cmd.ExecuteNonQuery()
                Else
                    strQuery = " Insert into settings (S_Key,S_Value,Branch) " &
                               " Values(" &
                               "'" & Dr.S_Key & "'," &
                               "'" & CommonDA.ReplaceRightSlash(Dr.S_Value) & "'," &
                               "'" & Dr.Branch & "')"
                    cmd.CommandText = strQuery
                    Dr.Id = cmd.ExecuteScalar
                End If

            Else

                If Dr.Id > 0 Then
                    strQuery = " Update ledgers Set S_Key = '" & Dr.S_Key & "'," &
                                " S_Value = '" & CommonDA.ReplaceRightSlash(Dr.S_Value) & "'," &
                                " Branch = '" & Dr.Branch & "' " &
                                " Where Id = '" & Dr.Id & "'"
                    cmd.CommandText = strQuery
                    cmd.ExecuteNonQuery()
                Else

                    strQuery = " Insert into ledgers (S_Key,S_Value,Branch) " &
                               " Values(" &
                               "'" & Dr.S_Key & "'," &
                               "'" & CommonDA.ReplaceRightSlash(Dr.S_Value) & "'," &
                               "'" & Dr.Branch & "')"
                    cmd.CommandText = strQuery
                    Dr.Id = cmd.ExecuteScalar
                End If

            End If
            cn.Close()
            cn = Nothing
        Catch ex As Exception
            cn = Nothing
            Return False
        End Try

        Return True

    End Function


    Public Function Delete_Settings(ByVal Id As Integer, ByVal Is_Leger As Boolean) As Boolean

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim da As New MySqlDataAdapter
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Dt As New DataTable
        Dim Deleted As Boolean = False

        cmd.Connection = cn
        cn.Open()

        Try
            If Is_Leger Then
                strQuery = " Delete from ledgers where Id = '" & Id & "' "
            Else
                strQuery = " Delete from settings where Id = '" & Id & "' "

            End If

            cmd.CommandText = strQuery
            cmd.ExecuteNonQuery()
            Deleted = True

        Catch ex As Exception
            Deleted = False
        End Try

        cn.Close()
        cn = Nothing
        Return Deleted

    End Function


    Public Function Delete_Serv_Labour_Settings(ByVal Id As Integer) As Boolean

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim da As New MySqlDataAdapter
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Dt As New DataTable
        Dim Deleted As Boolean = False

        cmd.Connection = cn
        cn.Open()

        Try

            strQuery = " Delete from service_ledgers where Id = '" & Id & "' "



            cmd.CommandText = strQuery
            cmd.ExecuteNonQuery()
            Deleted = True

        Catch ex As Exception
            Deleted = False
        End Try

        cn.Close()
        cn = Nothing
        Return Deleted

    End Function


    'Public Shared Function ReplaceQuote(ByVal str As String) As String
    '    If str <> "" Then
    '        Return Replace(str, "'", "''")
    '    Else
    '        Return ""
    '    End If
    'End Function



    Public Shared Function Read_Settings(ByVal S_Key As String) As String
        Dim Value As String = ""
        Try
            Value = PublicShared.Settings_Dt.Select("S_Key='" & S_Key & "'").First.Item("S_Value").ToString
        Catch ex As Exception
        End Try
        Return Value
    End Function

    Public Shared Function Update_Service(ByVal Dr As TallyDs.ServiceRow) As Boolean

        Dim Qry_Update = ""
        Dim Status As Boolean
        Dim invdate As String = ""
        Dim x As Integer = 0
        Dim InvNo As String = ""
        Dim Model As String = ""

        Qry_Update = " update Service set Locked=1 where Invoice_Number = '" & Dr.Invoice_Number & "' ; " &
            " update service_header set Updated=1 where Invoice_Date = '" & Format(Dr.Invoice_Date, "yyyy-MM-dd") & "'" &
            " and Invoice_Number = '" & Dr.Invoice_Number.Trim & "' ; "
        Status = RunQuery(Qry_Update)

        Return Status

    End Function

    Public Shared Function Update_Service_VchId(ByVal Dr As TallyDs.ServiceRow, ByVal Vchid As String) As Boolean

        Dim Qry_Update = ""
        Dim Status As Boolean
        Dim invdate As String = ""
        Dim x As Integer = 0
        Dim InvNo As String = ""
        Dim Model As String = ""

        Qry_Update = " update Service set Locked=1 where Invoice_Number = '" & Dr.Invoice_Number & "' ; " &
            " update service_header set Updated=1,Vchid='" & Vchid & "' where Invoice_Date = '" & Format(Dr.Invoice_Date, "yyyy-MM-dd") & "'" &
            " and Invoice_Number = '" & Dr.Invoice_Number.Trim & "' ; "
        Status = RunQuery(Qry_Update)

        Return Status

    End Function

    Public Shared Sub Create_Log(ByVal Screen As String, ByVal Activity As String, ByVal Activity_Detail As String)
        Dim Qry As String = ""

        Qry = "Insert into system_log(ActivityTime,Screen,Activity,Activity_Detail) Values (" &
                "'" & Format(Date.Now, "yyyy-MM-dd HH:mm:ss") & "'," &
                "'" & Screen & "'," &
                "'" & Activity & "'," &
                "'" & Activity_Detail & "')"
        RunQuery(Qry)
    End Sub


    Public Shared Sub Create_Settings(ByVal isLedger As Boolean, ByVal S_key As String, ByVal S_Value As String)
        Dim Qry As String = ""

        Try
            If isLedger Then
                Qry = PublicShared.Ledgers_Dt.Select("S_Key='" & S_key & "'").First.Item(0).ToString()
            Else
                Qry = PublicShared.Settings_Dt.Select("S_Key='" & S_key & "'").First.Item(0).ToString()

            End If
            If Qry <> "" Then
                MsgBox("Please Enter Value In Ledger")


            End If

        Catch ex As Exception

            If isLedger Then

                Qry = "Insert into ledgers (S_Key,S_Value) Values (" &
                   "'" & S_key & "'," &
                   "'" & S_Value & "')"
            Else
                Qry = "Insert into settings (S_Key,S_Value) Values (" &
                   "'" & S_key & "'," &
                   "'" & S_Value & "')"
            End If

            RunQuery(Qry)
        End Try


    End Sub

    Public Shared Function Read_Ledgers(ByVal S_Key As String) As String
        Dim Value As String = ""
        Try
            Value = PublicShared.Ledgers_Dt.Select("S_Key='" & S_Key & "'").First.Item("S_Value").ToString
        Catch ex As Exception
        End Try
        Return Value
    End Function

    Public Shared Function Insert_Service(ByVal Ds As DataSet, ByVal OverWrite As Boolean, ByRef LblStat As Label) As Boolean

        Dim Qry As String = ""
        Dim Status As Boolean = False
        Dim invdate As String = ""
        Dim InvNo As String = ""
        Dim Model As String = ""
        Dim Type As String = ""
        Dim Total As Integer = Ds.Tables("ServiceData").Rows.Count
        Dim Labour_SACCode As String = Read_Ledgers("Labour_SACCode")
        Dim Value As String = ""
        Dim Taxable As Decimal = 0.0
        Dim Qty_Fraction As Decimal = 0.0
        Dim Qry_Insert As String = ""
        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim BillType As String = ""
        Dim Qty As Decimal = 0.0
        Dim Qry_Delete As String = ""
        Dim Inv_date As Date
        Dim Rate = 0.0, Disc_per = 0.0, IGST_per = 0.0, IGST = 0.0, Total_Amt = 0.0, Dsic As Double = 0.0
        Dim GrandTotal As Decimal
        Dim PartType As String = "Item"
        Dim Accepte_Bills As String = Read_Settings("Service_Accepted_Bills")


        Try


            cmd.Connection = cn
            cn.Open()


            Dim y = 100 / Total
            Dim x = 100 / Total
            x -= y
            Dim z As String = ""
            Dim StatusString As String = "...................................................................................................."
            Dim Regex = New Regex(".")

            For Each DrD In Ds.Tables("ServiceData").Rows
                'Try
                '    Inv_date = DrD("Delivered Date Time")
                'Catch ex As Exception
                '    Continue For
                'End Try
                x = x + y
                If Math.Round(x, 0) > Math.Round(Val(z), 0) Then
                    '  StatusString += "|"
                    StatusString = Regex.Replace(StatusString, "|", CInt(Val(x)))
                    ' StatusString = Replace(StatusString, ".", "", , 4)
                Else

                End If
                z = (Math.Round(x, 0)).ToString

                LblStat.Text = "Inserting  " + StatusString + "  " + z + "/100"
                ' LblStat.Text = LblStat.Text.Replace("-", "\")
                LblStat.Refresh()

                If InvNo <> DrD("Invoice Number") Then

                    BillType = Strings.Left(DrD("Invoice Number").ToString, 3)

                    If BillType = "SSI" Then 'Or BillType = "CPI" Then ' Or BillType = "CLI" Then
                        Continue For
                    End If



                    If Accepte_Bills.Contains(BillType) Then
                    Else

                        Continue For
                    End If


                    If Qry <> "" Then
                        cmd.CommandText = Qry
                        cmd.ExecuteNonQuery()
                    End If

                    If OverWrite Then
                        Qry = " Delete from service where `Invoice_Number` = '" & DrD("Invoice Number") & "' ; "
                    Else
                        'strQuery = " Select Locked from service where `Invoice_Number` = '" & DrD("Invoice Number") & "' ; "
                        'cmd.CommandText = strQuery
                        'Value = cmd.ExecuteScalar
                        'If Value = 1 Then
                        '    Qry = ""
                        '    Continue For
                        'ElseIf Value = 0 Then
                        '    Qry = " Delete from service where `Invoice_Number` = '" & DrD("Invoice Number") & "' ; "
                        'End If
                    End If

                    InvNo = DrD("Invoice Number")

                End If


                If DrD("Invoice Number") <> "" And DrD("Part/Labour Code") <> "" Then

                    DrD("Customer Name") = DrD("Customer Name").ToString.Replace("Mr.", "")
                    DrD("Customer Name") = UCase(DrD("Customer Name").ToString.Trim)

                    If Labour_SACCode.Contains(DrD("HSN/SAC code").trim) Then
                        Type = "Labour"
                    Else
                        Type = "Item"
                    End If

                    If DrD("Part_LabourGroup").ToString = "Labour" Then
                        Type = "Labour"
                    Else
                        Type = "Item"
                    End If


                    Rate = 0.0 : Disc_per = 0.0 : IGST_per = 0.0 : IGST = 0.0 : Total_Amt = 0.0 : Dsic = 0.0 : Qty = 0

                    Qty = Val(DrD("Issued Qty")) - Val(DrD("Returned Qty"))
                    Taxable = Val(DrD("Rate")) * Qty
                    Taxable = Taxable - Val(DrD("Discount"))
                    InvNo = DrD("Invoice Number").trim
                    Inv_date = DrD("Job Card Date")

                    If BillType = "ELI" Then

                        DrD("CGST") = Math.Round(Taxable * Val(Read_Settings("CGST_Labour_Per")) / 100, 2)
                        DrD("SGST") = Math.Round(Taxable * Val(Read_Settings("SGST_Labour_Per")) / 100, 2)
                        DrD("Total Amount") = Taxable + Val(DrD("CGST")) + Val(DrD("SGST"))
                        DrD("SGST%") = Val(Read_Settings("SGST_Labour_Per"))
                        DrD("CGST%") = Val(Read_Settings("CGST_Labour_Per"))




                    End If



                    Qry += "INSERT INTO Service (" &
                                "Job_Card,Invoice_Number,Company_Name,Selling_Dealer_Name,Job_Card_Date,Reg_No,Chassis_No," &
                                "Engine_No,Model_Code,Model_Description,Customer_Name,Mobile_No,Customer_Voice,KM_Reading,Service_Advisor," &
                                "Type,Part_Labour_Code,Part_Labour_Description," &
                                "HSN_SAC_code,Header_Job_Type,Job_Type,Rate,Issued_Qty,Discount,Status,Mechanic," &
                                "Insurance_Provider,Insurance_Policy_No,PaiseRoundingOff," &
                                "Taxable,CGST_Per,CGST,SGST_Per,SGST,KFC,KFC_Per,Total_Amount,IGST_Per,IGST) Values (" &
                                "'" & DrD("Job Card ").trim & "','" & DrD("Invoice Number").trim & "','" & DrD("Company Name").trim & "', " &
                                "'" & ReplaceQuote(DrD("Selling Dealer Name")) & "','" & Format(DrD("Job Card Date"), "yyyy-MM-dd") & "','" & DrD("Reg No").trim & "', " &
                                "'" & DrD("Chassis No") & "','" & DrD("Engine No") & "','" & DrD("Model Code") & "','" & DrD("Model Description") & "','" & ReplaceQuote(DrD("Customer Name").trim) & "'," &
                                "'" & DrD("Mobile No") & "','" & ReplaceRightSlash(ReplaceQuote(DrD("Customer Voice"))) & "'," &
                                "'" & Val(DrD("KM Reading")) & "','" & DrD("Service Advisor") & "','" & Type & "','" & ReplaceQuote(DrD("Part/Labour Code")) & "','" & DrD("Part/Labour Description") & "'," &
                                "'" & DrD("HSN/SAC code") & "','" & DrD("Header Job Type") & "','" & DrD("Job Type") & "'," &
                                "'" & Val(DrD("Rate")) & "', '" & Qty & "', '" & Val(DrD("Discount")) & "'," &
                                "'" & DrD("Status") & "', '" & DrD("Mechanic") & "', '" & DrD("Insurance Provider") & "'," &
                                "'" & DrD("Insurance Policy No#") & "'," &
                                "'" & Val(DrD("PaiseRoundingOff")) & "','" & Taxable & "', '" & Val(DrD("CGST%")) & "', '" & Val(DrD("CGST")) & "'," &
                                "'" & Val(DrD("SGST%")) & "','" & Val(DrD("SGST")) & "'," &
                                "'" & Val(DrD("KFC1%")) & "','" & Val(DrD("KFC%")) & "'," &
                                "'" & Val(DrD("Total Amount")) & "'," & IGST_per & "," & IGST & "); "





                End If




            Next

            If Qry <> "" Then
                cmd.CommandText = Qry
                cmd.ExecuteNonQuery()
            End If

            cn.Close()
            cn = Nothing

            Status = True


        Catch ex As Exception
            Status = False
            MsgBox("Error Found : " & ex.Message)
        End Try

        Return Status

    End Function

    Public Shared Function Insert_Service_CSV(ByVal FilePath As String) As String

        Dim cn As New MySqlConnection(ConnectionString)
        Dim cmd As New MySqlCommand
        Dim Tr As MySqlTransaction
        Dim Dr As TallyDs.PartDetailsRow
        Dim Drc As TallyDs.CustomerDetailsRow
        Dim PartDs As New TallyDs
        Dim LastId As String = ""
        Dim lines = IO.File.ReadAllLines(FilePath)

        Dim Count As Integer = 0
        Dim TableEntry As Boolean = False
        Dim CustEntry = False, is_SSI = False, SSI_Entry As Boolean = False
        Dim QryHead = "", QryHeadDelete = "", QryData = "", Qry = "", QryDelete As String = ""

        Dim PlCode = -1, PlDesc = -1, Disc = -1, Qty = -1, Hsn = -1, cgst = -1, sgst = -1, cgstper = -1, sgstper = -1, Rate = -1, kfc = -1, kfcper = -1, total = -1, taxable = -1, value As Integer = -1
        Dim TypeOfInv = "", Status As String = ""
        Dim iDate, jDate As Date
        Dim Grand_Total As Decimal = 0.00

        Drc = PartDs.CustomerDetails.Rows.Add
        Drc.Vehicle_Details = ""
        Drc.Address = ""

        cmd.Connection = cn
        cn.Open()
        'Tr = cn.BeginTransaction(IsolationLevel.ReadCommitted)
        'cmd.Transaction = Tr

        Try
            Dim S As String = ""
            For Each line In lines
                If line.Contains("Order/Allocation") And line.Contains("Invoice No") And line.Contains("SSI") Then
                    is_SSI = True
                    CustEntry = True
                    SSI_Entry = True
                End If
                S += line + vbCrLf
            Next





            For Each line In lines


                If TableEntry Then

                    If line.Contains("TOTAL") Then
                        TableEntry = False

                        'Dim Totals() As String = line.Split({""""c}, StringSplitOptions.RemoveEmptyEntries)
                        ''MsgBox(Totals(0))

                        'Dr = PartDs.PartDetails.Rows.Add
                        'Dr.Value = "TOTAL"

                        'If Totals.Length < 2 Then
                        '    Totals = line.Split({","c}, StringSplitOptions.RemoveEmptyEntries)
                        'End If

                        'For Each Content In Totals

                        '    If TypeOfInv = "Item" Then

                        '        If is_SSI = True Then
                        '            If Not Content.Contains("TOTAL") And Dr.Amount = 0.00 And Dr.CGST = 0.00 And Dr.SGST = 0.00 And Dr.CGST_Per = 0.00 Then
                        '                Dr.CGST_Per = Totals(2)
                        '                Dr.CGST = Totals(1)
                        '                Dr.SGST_Per = Totals(4)
                        '                Dr.SGST = Totals(3)
                        '                Dr.Amount = Totals(5)
                        '                Exit For
                        '            End If

                        '        Else


                        '            If Not Content.Contains("TOTAL") And Dr.Taxable_Amount = 0.00 Then
                        '                Dr.Taxable_Amount = Content.Trim
                        '            ElseIf Not Dr.Taxable_Amount = 0.00 And (Dr.Discount = 0.00 And Dr.CGST = 0.00 And Dr.SGST = 0.00) Then
                        '                Dim GSTotals() As String = Content.Split({","c}, StringSplitOptions.RemoveEmptyEntries)
                        '                '   MsgBox(Content(0))
                        '                If Totals.Length = 8 Then
                        '                    Dr.Discount = Totals(2)
                        '                    Dr.CGST_Per = Totals(3)
                        '                    Dr.CGST = Totals(4)
                        '                    Dr.SGST_Per = Totals(5)
                        '                    Dr.SGST = Totals(6)
                        '                    Dr.Amount = Totals(7)
                        '                Else

                        '                End If
                        '            ElseIf Not Dr.Taxable_Amount = 0.00 And Dr.Amount = 0.00 And Not (Dr.Discount = 0.00 And Dr.CGST = 0.00 And Dr.SGST = 0.00) Then
                        '                Dr.Amount = Content.Trim
                        '                Dr.Value = "TOTAL"
                        '            End If

                        '        End If

                        '    Else
                        '        If Not Content.Contains("TOTAL") And Dr.Amount = 0.00 Then
                        '            Dr.Amount = Content.Trim
                        '            Dr.Value = "TOTAL"

                        '        End If
                        '    End If

                        'Next

                        'Continue For
                    End If



                    Dim TableDetails() As String = line.Split({","c}, StringSplitOptions.RemoveEmptyEntries)


                    Dim temp() As String = line.Split({""""c}, StringSplitOptions.RemoveEmptyEntries)
                    Dim newline As String = ""
                    Dim objDa As New CommonDA
                    If temp.Length > 2 Then
                        For Each tpline In temp
                            If objDa.CountCharacter(tpline, ",") = 1 Then
                                tpline = tpline.Replace(",", "")
                            End If
                            newline += tpline
                        Next

                    End If

                    Dim LineNew() As String = newline.Split({","c}, StringSplitOptions.RemoveEmptyEntries)

                    If LineNew.Length > 1 Then
                        If TableDetails.Length = LineNew.Length + 1 Then
                            TableDetails = LineNew
                        End If
                    End If


                    If line.Contains("") Then

                    End If

                    If TableDetails.Length = 12 Then

                        Dr = PartDs.PartDetails.Rows.Add

                        Dr.S_No = TableDetails(0)
                        Dr.Part_Labour_Code = TableDetails(PlCode)
                        Dr.Part_Labour_Description = TableDetails(PlDesc)


                        Dr.Value = Val(TableDetails(Rate).Replace("""", "").Replace(",", ""))
                        Dr.KFC_Per = TableDetails(kfcper)
                        Dr.KFC = TableDetails(kfc)
                        Dr.Discount = Val(TableDetails(Disc).Replace(",", ""))
                        Dr.CGST_Per = TableDetails(cgstper)
                        Dr.CGST = TableDetails(cgst)
                        Dr.SGST_Per = TableDetails(sgstper)
                        Dr.SGST = TableDetails(sgst)
                        Dr.Amount = TableDetails(total)

                        If Qty = -1 Then
                            Dr.Qty = 1
                        Else
                            Dr.Qty = TableDetails(Qty)
                        End If

                        If taxable = -1 Then
                            Dr.Taxable_Amount = Val(Dr.Value) * Val(Dr.Qty)
                        Else
                            Dr.Taxable_Amount = TableDetails(taxable)
                        End If


                        PartDs.AcceptChanges()
                    ElseIf TableDetails.Length = 13 Then

                        Dr = PartDs.PartDetails.Rows.Add

                        Dr.S_No = TableDetails(0)
                        Dr.Part_Labour_Code = TableDetails(PlCode)
                        Dr.Part_Labour_Description = TableDetails(PlDesc)


                        Dr.Value = Val(TableDetails(Rate).Replace("""", "").Replace(",", ""))
                        Dr.KFC_Per = TableDetails(kfcper)
                        Dr.KFC = TableDetails(kfc)
                        Dr.Discount = Val(TableDetails(Disc).Replace(",", ""))
                        Dr.CGST_Per = TableDetails(cgstper)
                        Dr.CGST = TableDetails(cgst)
                        Dr.SGST_Per = TableDetails(sgstper)
                        Dr.SGST = TableDetails(sgst)
                        Dr.Amount = TableDetails(total)

                        If Qty = -1 Then
                            Dr.Qty = 1
                        Else
                            Dr.Qty = TableDetails(Qty)
                        End If

                        If taxable = -1 Then
                            Dr.Taxable_Amount = Val(Dr.Value) * Val(Dr.Qty)
                        Else
                            Dr.Taxable_Amount = TableDetails(taxable)
                        End If
                        PartDs.AcceptChanges()

                    End If

                ElseIf CustEntry <> True Then

                    Dim temp As String = line.Replace(",", "|")
                    Dim Details() As String = temp.Split({"|"c}, StringSplitOptions.RemoveEmptyEntries)
                    CustEntry = True
                    For Each Cols In PartDs.CustomerDetails.Columns
                        If Drc(Cols) = "" Then
                            CustEntry = False
                            Exit For
                        End If
                    Next

                    Dim j As Integer = 0
                    For Each items In Details

                        If items.Contains("INVOICE NO.") And Drc.Invoice_No = "" Then
                            Try
                                Drc.Invoice_No = Details(j + 1).Replace(":", "").Trim
                                Exit For
                            Catch ex As Exception
                                Create_Log("Import", "CSV file Read", "Drc.Invoice_No = Details(j + 1)")
                            End Try



                        ElseIf items.Contains("INVOICE DATE") And Drc.Invoice_No <> "" And Drc.Invoice_Date = "" Then
                            Try
                                Drc.Invoice_Date = Details(j + 1).Replace(":", "").Trim
                                Exit For
                            Catch ex As Exception
                                Create_Log("Import", "CSV file Read", " Drc.Invoice_Date = Details(j + 1)")
                            End Try


                        ElseIf items.Contains("Registration No") And Drc.Invoice_Date <> "" Then
                            Try
                                Drc.Reg_No = items.Replace("Registration No :", "").Trim
                                Exit For
                            Catch ex As Exception
                                Create_Log("Import", "CSV file Read", " Drc.Reg_No = items.Replace(Registration No :)")
                            End Try


                        ElseIf items.Contains("Chassis No") And Drc.Invoice_Date <> "" Then
                            Try
                                Drc.Chassis_No = items.Replace("Chassis No", "").Replace(":", "").Trim
                                Exit For
                            Catch ex As Exception
                                Create_Log("Import", "CSV file Read", "Drc.Chassis_No = items.Replace(Chassis No")
                            End Try

                        ElseIf items.Contains("Engine No") And Drc.Chassis_No <> "" Then
                            Try
                                Drc.Engine_No = items.Replace("Engine No", "").Replace(":", "").Trim
                                Exit For
                            Catch ex As Exception
                                Create_Log("Import", "CSV file Read", "Drc.Engine_No = items.Replace(Chassis No")
                            End Try


                        ElseIf items.Contains("Model :") And Drc.Invoice_Date <> "" Then
                            Try
                                Drc.Model = items.Replace("Model :", "").Replace(":", "").Trim
                                Exit For
                            Catch ex As Exception
                                Create_Log("Import", "CSV file Read", "Drc.Model = items.Replace(Chassis No")
                            End Try


                        ElseIf items.Contains("Model Name") And Drc.Invoice_Date <> "" Then
                            Try
                                Drc.Model_Name = items.Replace("Model Name", "").Replace(":", "").Trim
                                Exit For
                            Catch ex As Exception
                                Create_Log("Import", "CSV file Read", "Drc.Model_Name = items.Replace(Chassis No")
                            End Try




                        ElseIf items.Contains("JOB CARD NO") And Drc.Invoice_Date <> "" And Drc.JobCard_No = "" Then
                            Try
                                Drc.JobCard_No = Details(j + 1).Replace(":", "").Trim
                                Exit For
                            Catch ex As Exception
                                Create_Log("Import", "CSV file Read", "Drc.JOB CARD NO = items.Replace(Chassis No")
                            End Try




                        ElseIf items.Contains("JOB CARD DATE") And Drc.JobCard_No <> "" Then
                            Try
                                Drc.JobCard_Date = Details(j + 1).Replace(":", "").Replace(":", "").Trim
                                Exit For
                            Catch ex As Exception
                                Create_Log("Import", "CSV file Read", "Drc.JobCard_Date = items.Replace(Chassis No")
                            End Try

                        ElseIf items.Contains("KM READING") And Drc.Address <> "" Then
                            Try
                                Drc.KM_Reading = Details(j + 1).Replace(":", "").Replace(":", "").Trim
                                If Drc.KM_Reading = "0" Then
                                    Drc.KM_Reading = "00000"
                                End If
                                Exit For
                            Catch ex As Exception
                                Create_Log("Import", "CSV file Read", "Drc.KM_Reading = items.Replace(Chassis No")
                            End Try

                        ElseIf Drc.Customer_Name <> "" And Drc.KM_Reading = "0" Then
                            Try
                                Drc.Address += items.Replace("CUSTOMER NAME & ADDRESS", "").Replace(":", "").Trim + " "

                            Catch ex As Exception
                                Create_Log("Import", "CSV file Read", "Drc.Address = items.Replace(Chassis No")
                            End Try


                        ElseIf items.Contains("CUSTOMER NAME & ADDRESS") Or Drc.JobCard_Date <> "" And Drc.KM_Reading = "0" Then
                            Try
                                Drc.Customer_Name = items.Replace("CUSTOMER NAME & ADDRESS", "").Replace(":", "").Trim

                            Catch ex As Exception
                                Create_Log("Import", "CSV file Read", "Drc.Address = items.Replace(Chassis No")
                            End Try




                        End If



                        j = +1
                    Next

                ElseIf SSI_Entry Then


                    Dim Details() As String = line.Split({","c}, StringSplitOptions.RemoveEmptyEntries)

                    For Each items In Details
                        items = items.Replace(":", "")
                        If items = "" Then
                            Continue For
                        End If


                        If line.Contains("Order/Allocation") And Drc.JobCard_No = "" Then
                            If Not items.Contains("Order/Allocation") Then
                                Drc.JobCard_No = items.Trim
                            End If
                        ElseIf line.Contains("Invoice No") And Drc.Invoice_No = "" Then
                            If Not items.Contains("Invoice No") Then
                                Drc.Invoice_No = items.Trim
                            End If
                        ElseIf line.Contains("Invoice Date") Then
                            If Not items.Contains("Invoice Date") Then
                                Drc.Invoice_Date = IIf(items.Length > 11, items.Substring(0, 10), items)
                            End If
                        ElseIf line.Contains("Sales Person") Then
                            If Not items.Contains("Sales Person") Then
                                '      Drc.Customer_Name = items.Trim
                            End If
                        ElseIf line.Contains("Supplier GSTIN") And Drc.Sup_GSTIN = "" Then
                            If Not items.Contains("Supplier GSTIN") Then
                                Drc.Sup_GSTIN = items.Trim
                            End If
                        ElseIf line.Contains("POS") And Drc.POS = "" Then
                            If Not items.Contains("POS") Then
                                Drc.POS = items.Replace("""", "").Trim
                            End If
                        ElseIf line.Contains("Recipient GSTIN") And Drc.Recpt_GSTIN = "" Then
                            If Not items.Contains("Recipient GSTIN") Then
                                Drc.Recpt_GSTIN = Drc.Recpt_GSTIN.Replace("""", "") & items.Replace("""", "").Trim
                            End If


                        Else
                            If Drc.Recpt_GSTIN <> "" And Drc.POS <> "" And Drc.Sup_GSTIN <> "" And Drc.Invoice_No <> "" And Drc.JobCard_No <> "" And Drc.Invoice_Date <> "" Then
                                SSI_Entry = False
                            End If

                            Continue For
                        End If



                    Next



                Else
                    PartDs.AcceptChanges()
                    Exit For
                End If

                If line.Contains("S No") = True And TableEntry <> True Then
                    TableEntry = True
                    If line.Contains("Part Code") Or line.Contains("PART NO") Then
                        TypeOfInv = "Part"
                    ElseIf line.Contains("Labour Code") Then
                        TypeOfInv = "Labour"
                    End If

                    Dim Oder() As String = line.Split({","c}, StringSplitOptions.RemoveEmptyEntries)
                    Dim j = 0
                    For Each odr In Oder

                        If odr = "Labour Code" Then
                            PlCode = j
                        ElseIf odr = "LABOUR DESCRIPTION" Then
                            PlDesc = j
                        ElseIf odr = "Part Code" Then
                            PlCode = j
                        ElseIf odr = "PART  DESCRIPTION" Then
                            PlDesc = j
                        ElseIf odr = "QTY" Then
                            Qty = j
                        ElseIf odr = "AMOUNT(Rs)" Then
                            total = j
                        ElseIf odr = "RATE(Rs)" Then
                            Rate = j
                        ElseIf odr = "DISCOUNT" Then
                            Disc = j
                        ElseIf odr = "SGST" Then
                            sgst = j
                        ElseIf odr = "SGST%" Then
                            sgstper = j
                        ElseIf odr = "CGST" Then
                            cgst = j
                        ElseIf odr = "CGST%" Then
                            cgstper = j
                            'ElseIf odr = "SGST%" Then

                        ElseIf odr = "KFC" Then
                            kfc = j
                        ElseIf odr = "KFC%" Then
                            kfcper = j
                        ElseIf odr = "TOTAL AMOUNT(Rs)" Then
                            total = j
                        End If



                        j += 1
                    Next





                End If
                PartDs.AcceptChanges()


            Next


            PartDs.AcceptChanges()

            Drc = PartDs.CustomerDetails.Rows(0)

            iDate = Convert.ToDateTime(Drc.Invoice_Date)

            'If is_SSI Then
            '    jDate = iDate
            '    JobCardNo = InvoiceNo
            'Else
            If is_SSI Then
                Drc.JobCard_Date = Drc.Invoice_Date
                Drc.JobCard_No = Drc.Invoice_No
            End If
            jDate = Convert.ToDateTime(Drc.JobCard_Date)

            'End If
            Dim ErrorString As String = ""

            For Each Dr In PartDs.PartDetails.Rows



                If Not Dr.Value = " ThenTOTAL" Then
                    If Dr.S_No = 0 Then
                        ErrorString += " Not Found  :S_No  :" + IIf(Drc.Invoice_No = "", "Null", Drc.Invoice_No) + vbCrLf
                    End If
                    If Dr.Part_Labour_Code = "" Then
                        ErrorString += " Not Found  :Part_Labour_Code   :" + IIf(Drc.Invoice_No = "", "Null", Drc.Invoice_No) + vbCrLf

                    End If
                    If Dr.Part_Labour_Description = "" Then
                        ErrorString += " Not Found  :Part_Labour_Description   : " + IIf(Drc.Invoice_No = "", "Null", Drc.Invoice_No) + vbCrLf

                    End If
                    If Dr.CGST = 0.00 Then
                        ErrorString += " Not Found  :CGST   :" + IIf(Drc.Invoice_No = "", "Null", Drc.Invoice_No) + vbCrLf

                    End If
                    If Dr.CGST_Per = 0.00 Then
                        ErrorString += " Not Found  :CGST_Per   : " + IIf(Drc.Invoice_No = "", "Null", Drc.Invoice_No) + vbCrLf

                    End If
                    If Dr.Discount = 0.00 Then
                        '   ErrorString += " Not Found  :Discount" + IIf(Drc.Invoice_No = "", "Null", Drc.Invoice_No) + vbCrLf

                    End If
                    If Dr.Taxable_Amount = 0.00 Then
                        ErrorString += " Not Found  :Taxable_Amount   :" + IIf(Drc.Invoice_No = "", "Null", Drc.Invoice_No) + vbCrLf

                    End If
                    If Dr.Value = 0.00 Then
                        ErrorString += " Not Found  :Value    :" + IIf(Drc.Invoice_No = "", "Null", Drc.Invoice_No) + vbCrLf

                    End If
                    If Dr.HSN_No = "" Then
                        'ErrorString += " Not Found  :HSN_No    :" + IIf(Drc.Invoice_No = "", "Null", Drc.Invoice_No) + vbCrLf

                    End If
                    If Dr.Qty = 0.00 Then
                        ErrorString += " Not Found  :Qty    :" + IIf(Drc.Invoice_No = "", "Null", Drc.Invoice_No) + vbCrLf

                    End If
                    If Dr.SGST = 0.00 Then
                        ErrorString += " Not Found  :SGST   : " + IIf(Drc.Invoice_No = "", "Null", Drc.Invoice_No) + vbCrLf

                    End If
                    If Dr.Amount = 0.00 Then
                        ErrorString += " Not Found  :Amount   :" + IIf(Drc.Invoice_No = "", "Null", Drc.Invoice_No) + vbCrLf
                    Else
                        Grand_Total += Dr.Amount
                    End If


                End If
            Next


            If ErrorString <> "" Then
                MessageBox.Show("Error On File ", "Value Missing", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification)
                If MsgBox("Error on File Could Not Find Value" & vbCrLf & "Do you Want to Continue" & ErrorString, MsgBoxStyle.YesNo, "Error") = vbNo Then
                    Status = Drc.Invoice_No + "_" + TypeOfInv
                    Exit Function
                End If
            End If



            'Dim SplitDetails As String() = Drc.Vehicle_Details.Split(New String() {vbCr & vbLf, vbLf}, StringSplitOptions.None)
            'Dim RegNo = "", Chaseno = "", engNo = "", model = "", modelName = ""
            'Dim A As String = ""
            'For Each line In SplitDetails

            '    If RegNo = "" Then
            '        RegNo = line.ToString.Trim.Replace("Registration No  ", "")
            '        If line.Contains("Chassis No") Then
            '            Chaseno = line.ToString.Trim.Replace("Chassis No", "").Trim
            '            Dim chassis() As String = Chaseno.Split({","}, StringSplitOptions.RemoveEmptyEntries)
            '            Chaseno = chassis.First.ToString.Trim
            '        End If
            '    End If

            'Next

            Dim Drs As TallyDs.Service_HeaderRow
            Dim CashDs As New BkDS
            Dim dcr As BkDS.Cash_RegisterRow
            Dim StrQuery As String = ""
            Dim prefix As String = ""
            Dim NextId As String = ""
            Dim StrQry As String = ""
            Dim CashReg As String = ""
            Dim Servicebills As Integer = 0
            Dim ServiceBill As String = ""
            Dim Id As Integer = 0

            CashDs.Clear()
            dcr = CashDs.Cash_Register.Rows.Add
            dcr.Branch_Id = PublicShared.Branch_Id
            dcr.BkId = 0
            dcr.PayId = 0
            dcr.User_Id = PublicShared.User_Id
            dcr.User_Name = PublicShared.User_Name
            CashDs.AcceptChanges()

            If is_SSI Then
                prefix = Read_Settings("Prefix_CS")
            Else
                prefix = Read_Settings("Prefix_PL")
            End If
            Try

                Drs = Get_LastId(prefix)
                If Drs.Job_Card.Trim = Drc.JobCard_No.Trim Then

                Else

                    Drs.Invoice_Number = ""
                    Try

                        StrQry = " Select Inv_Number from service_bills where `JobCard_No` = '" & Drc.JobCard_No.Trim & "' ; "
                        cmd.CommandText = StrQry
                        ServiceBill = cmd.ExecuteScalar()

                        Drs.Invoice_Number = ServiceBill

                        If Drs.Invoice_Number = "" Then
                            NextId = Get_NextId(prefix)
                            Drs.Invoice_Number = NextId
                        End If

                    Catch ex As Exception
                        NextId = Get_NextId(prefix)
                        Drs.Invoice_Number = NextId

                    End Try


                End If
            Catch

                StrQuery = " Select Count(Id) from service_bills  "
                cmd.CommandText = StrQuery
                NextId = cmd.ExecuteScalar()

                NextId = prefix & Format(Val(NextId) + 1, "00000")
                Drs.Invoice_Number = NextId.Trim

            End Try
            '    cmd.CommandText = StrQuery
            '    Id = cmd.ExecuteScalar()
            'Catch ex As Exception
            '    Id = 0
            'End Try



            Try
                StrQry = " Select Id from service_bills where `JobCard_No` = '" & Drc.JobCard_No.Trim & "' and Type ='" & TypeOfInv.Trim & "' ; "
                cmd.CommandText = StrQry
                Servicebills = cmd.ExecuteScalar()
            Catch ex As Exception
                Servicebills = 0
            End Try


            If Servicebills <> 0 Then
                '  Drc.Invoice_No = ServiceBill
            Else

                QryHead += "INSERT INTO service_bills (Inv_Number,JobCard_No,Invoice_Date,Invoice_No,JobCard_Date,Reg_No,Chassis_No,Vehicle_Details," &
                        "Customer_Name,Customer_Address,KM_Reading,Sales_Person,Type," &
                        "Part_Labour_Code,Part_Labour_Description,HSN_No,Rate," &
                        "Qty,Discount,PaiseRoundingOff,CGST_Per,CGST,SGST_Per," &
                        "SGST,Total_Amount,LastSeen,Taxable) VALUES "

                For Each Dr In PartDs.PartDetails.Rows

                    If Dr.Value <> "TOTAL" Then

                        QryData += IIf(QryData = "", "", ",")


                        QryData += " (" & "'" & Drs.Invoice_Number.Trim & "'," &
                    "'" & Drc.JobCard_No & "','" & Format(iDate, "yyyy-MM-dd") & "','" & Drc.Invoice_No.Trim & "','" & Format(jDate, "yyyy-MM-dd") & "','" & Drc.Reg_No & "','" & Drc.Chassis_No & "','" & Drc.Vehicle_Details & "'," &
                    "'" & Drc.Customer_Name & "','" & Drc.Address & "','" & Drc.KM_Reading & "','','" & TypeOfInv.Trim & "'," &
                    "'" & Dr.Part_Labour_Code & "','" & Dr.Part_Labour_Description & "','" & Dr.HSN_No & "','" & Dr.Value & "'," &
                    "'" & Dr.Qty & "','" & Dr.Discount & "','0.00','" & Dr.CGST_Per & "','" & Dr.CGST & "','" & Dr.SGST_Per & "'," &
                    "'" & Dr.SGST & "','" & Dr.Amount & "','" & Format(DateTime.Now, "yyyy-MM-dd") & "','" & Dr.Taxable_Amount & "')"

                    Else

                    End If


                Next

                QryData += ";"

            End If


            Try
                Qry = "SELECT Id FROM service_bills_head WHERE Invoice_Date = '" & Format(iDate, "yyyy-MM-dd") & "' AND `JobCard_No` = '" & Drc.JobCard_No.Trim & "' AND Type ='" & TypeOfInv.Trim & "';"
                cmd.CommandText = Qry
                Id = cmd.ExecuteScalar()
            Catch ex As Exception
                Id = 0
            End Try

            If Id <> 0 Then

            Else

                Qry += " INSERT INTO service_bills_head (Invoice_Number,JobCard_No,Invoice_Date,Invoice_Amount,Type,LastSeen) VALUES (" &
                            "'" & Drs.Invoice_Number.Trim & "','" & Drc.JobCard_No & "','" & Format(iDate, "yyyy-MM-dd") & "'," &
                            "'" & Grand_Total & "','" & TypeOfInv & "',now())" 'Val(Grand_Total)



                If QryData <> "" Then
                    cmd.CommandText = QryHead + QryData + Qry
                    cmd.ExecuteNonQuery()

                End If

            End If

            Try
                StrQry = "Select id from cash_register where JobCardNo='" & Drc.JobCard_No & "' and Type = '" & TypeOfInv & "'" &
                     " and Payment_Mode=''"
                cmd.CommandText = StrQry
                CashReg = cmd.ExecuteScalar
            Catch ex As Exception
                CashReg = 0
            End Try

            If CashReg > 0 Then

            Else
                StrQry = "Insert into cash_register (Entry_Date,Entry_Type,Entry_Head,Entry_Sub_Head,Ref_No,Amount,Type,Party,JobCardNo,Branch_Id,User_Id,Chassis_No," &
                          "User_Name,Created_On) Values('" & Format(iDate, "yyyy-MM-dd") & "','Receipt','Service','Service Invoice','" & Drs.Invoice_Number & "','" & Grand_Total & "'," &
                          "'" & TypeOfInv & "','" & Drc.Customer_Name & "','" & Drc.JobCard_No & "','" & dcr.Branch_Id & "','" & dcr.User_Id & "','" & Drc.Chassis_No & "','" & dcr.User_Name & "'," &
                          "'" & Format(Date.Now, "yyyy-MM-dd HH:mm:ss") & "')"


                cmd.CommandText = StrQry
                cmd.ExecuteNonQuery()
            End If

            'Tr.Commit()
            'Tr.Dispose()
            cn.Close()
            cn.Dispose()
            cn = Nothing

            Status = Drs.Invoice_Number.Replace("/", "_") + "_" + TypeOfInv + "True"
            Return Status


        Catch ex As Exception
            'If Not Tr Is Nothing Then
            '    Tr.Rollback()

            'End If
            cn.Close()
            cn.Dispose()
            Status = Drc.Invoice_No.Replace("/", "_") + "_" + TypeOfInv
            Return Status

            MsgBox("Error Found : " & ex.Message, vbCritical)
        End Try

        Return Status
    End Function

    Public Function CountCharacter(ByVal value As String, ByVal ch As Char) As Integer
        Return value.Count(Function(c As Char) c = ch)
    End Function

    Public Sub Show_ClipBoard(ByVal Title As String, ByVal Msg As String)
        Dim ObjCl As FrmClipBoard
        ObjCl = New FrmClipBoard(Title, Msg)
        ObjCl.StartPosition = FormStartPosition.CenterScreen
        ObjCl.ShowDialog(Me)
    End Sub


    Public Shared Function Insert_Service_CSV_Old(ByVal FilePath As String) As String

        Dim cn As New MySqlConnection(ConnectionString)
        Dim cmd As New MySqlCommand
        Dim Tr As MySqlTransaction
        Dim Dr As TallyDs.PartDetailsRow
        Dim Drc As TallyDs.CustomerDetailsRow
        Dim PartDs As New TallyDs
        Dim LastId As String = ""
        Dim lines = IO.File.ReadAllLines(FilePath)

        Dim Count As Integer = 0
        Dim TableEntry As Boolean = False
        Dim CustEntry = False, is_SSI = False, SSI_Entry As Boolean = False
        Dim QryHead = "", QryHeadDelete = "", QryData = "", Qry = "", QryDelete As String = ""


        Dim TypeOfInv = "", Status As String = ""
        Dim iDate, jDate As Date
        Dim Grand_Total As Decimal = 0.00

        Drc = PartDs.CustomerDetails.Rows.Add
        Drc.Vehicle_Details = ""
        Drc.Address = ""

        cmd.Connection = cn
        cn.Open()
        'Tr = cn.BeginTransaction(IsolationLevel.ReadCommitted)
        'cmd.Transaction = Tr

        Try
            Dim S As String = ""
            For Each line In lines
                If line.Contains("Order/Allocation") And line.Contains("Invoice No") And line.Contains("SSI") Then
                    is_SSI = True
                    CustEntry = True
                    SSI_Entry = True
                End If
                S += line + vbCrLf
            Next





            For Each line In lines


                If TableEntry Then

                    If line.Contains("TOTAL") Then
                        TableEntry = False

                        'Dim Totals() As String = line.Split({""""c}, StringSplitOptions.RemoveEmptyEntries)
                        ''MsgBox(Totals(0))

                        'Dr = PartDs.PartDetails.Rows.Add
                        'Dr.Value = "TOTAL"

                        'If Totals.Length < 2 Then
                        '    Totals = line.Split({","c}, StringSplitOptions.RemoveEmptyEntries)
                        'End If

                        'For Each Content In Totals

                        '    If TypeOfInv = "Item" Then

                        '        If is_SSI = True Then
                        '            If Not Content.Contains("TOTAL") And Dr.Amount = 0.00 And Dr.CGST = 0.00 And Dr.SGST = 0.00 And Dr.CGST_Per = 0.00 Then
                        '                Dr.CGST_Per = Totals(2)
                        '                Dr.CGST = Totals(1)
                        '                Dr.SGST_Per = Totals(4)
                        '                Dr.SGST = Totals(3)
                        '                Dr.Amount = Totals(5)
                        '                Exit For
                        '            End If

                        '        Else


                        '            If Not Content.Contains("TOTAL") And Dr.Taxable_Amount = 0.00 Then
                        '                Dr.Taxable_Amount = Content.Trim
                        '            ElseIf Not Dr.Taxable_Amount = 0.00 And (Dr.Discount = 0.00 And Dr.CGST = 0.00 And Dr.SGST = 0.00) Then
                        '                Dim GSTotals() As String = Content.Split({","c}, StringSplitOptions.RemoveEmptyEntries)
                        '                '   MsgBox(Content(0))
                        '                If Totals.Length = 8 Then
                        '                    Dr.Discount = Totals(2)
                        '                    Dr.CGST_Per = Totals(3)
                        '                    Dr.CGST = Totals(4)
                        '                    Dr.SGST_Per = Totals(5)
                        '                    Dr.SGST = Totals(6)
                        '                    Dr.Amount = Totals(7)
                        '                Else

                        '                End If
                        '            ElseIf Not Dr.Taxable_Amount = 0.00 And Dr.Amount = 0.00 And Not (Dr.Discount = 0.00 And Dr.CGST = 0.00 And Dr.SGST = 0.00) Then
                        '                Dr.Amount = Content.Trim
                        '                Dr.Value = "TOTAL"
                        '            End If

                        '        End If

                        '    Else
                        '        If Not Content.Contains("TOTAL") And Dr.Amount = 0.00 Then
                        '            Dr.Amount = Content.Trim
                        '            Dr.Value = "TOTAL"

                        '        End If
                        '    End If

                        'Next

                        'Continue For
                    End If

                    Dim TableDetails() As String = line.Split({","c}, StringSplitOptions.RemoveEmptyEntries)

                    If TableDetails.Length = 13 Then
                        Dr = PartDs.PartDetails.Rows.Add
                        Dr.S_No = TableDetails(0)
                        Dr.Part_Labour_Code = TableDetails(1)
                        Dr.Part_Labour_Description = TableDetails(2)
                        Dr.HSN_No = TableDetails(3)
                        Dr.Qty = TableDetails(4)
                        Dr.Value = TableDetails(5)
                        Dr.Taxable_Amount = TableDetails(6)
                        Dr.Discount = TableDetails(7)
                        Dr.CGST_Per = TableDetails(8)
                        Dr.CGST = TableDetails(9)
                        Dr.SGST_Per = TableDetails(10)
                        Dr.SGST = TableDetails(11)
                        Dr.Amount = TableDetails(12)

                        PartDs.AcceptChanges()
                    ElseIf TableDetails.Length = 14 Then

                        Dr = PartDs.PartDetails.Rows.Add
                        Dr.S_No = TableDetails(0)
                        Dr.Part_Labour_Code = TableDetails(1)
                        Dr.Part_Labour_Description = TableDetails(2)
                        Dr.HSN_No = TableDetails(3)
                        Dr.UOM = TableDetails(4)
                        Dr.Qty = TableDetails(5)
                        Dr.Value = TableDetails(6)
                        Dr.Taxable_Amount = TableDetails(8)
                        Dr.Discount = TableDetails(7)
                        Dr.CGST_Per = TableDetails(9)
                        Dr.CGST = TableDetails(10)
                        Dr.SGST_Per = TableDetails(11)
                        Dr.SGST = TableDetails(12)
                        Dr.Amount = TableDetails(13)

                    End If

                ElseIf CustEntry <> True Then

                    line = line.Replace(":", "")
                    Dim Details() As String = line.Split({","c}, StringSplitOptions.RemoveEmptyEntries)
                    CustEntry = True
                    For Each Cols In PartDs.CustomerDetails.Columns
                        If Drc(Cols) = "" Then
                            CustEntry = False
                            Exit For
                        End If
                    Next


                    For Each items In Details

                        items = items.Trim
                        If items = "" Then
                            Continue For
                        End If

                        If line.Contains("JOB CARD NO") And Drc.JobCard_No = "" Then
                            If Not items.Contains("JOB CARD NO") Then
                                Drc.JobCard_No = items.Trim
                            End If

                        ElseIf line.Contains("INVOICE NO") And Drc.Invoice_No = "" Then
                            If Not items.Contains("INVOICE NO") Then
                                Drc.Invoice_No = items.Trim
                            End If
                        End If

                        If line.Contains("INVOICE DATE") And Drc.Invoice_Date = "" Then
                            If Not items.Contains("INVOICE DATE") Then
                                Drc.Invoice_Date = items.Trim
                            End If
                        End If

                        If line.Contains("JOB CARD DATE") And Drc.JobCard_Date = "" Then
                            If Not items.Contains("JOB CARD DATE") Then
                                Drc.JobCard_Date = items.Trim
                            End If
                        End If

                        If line.Contains("Registration No") And Drc.Reg_No = "" Then
                            If items.Contains("Registration No :") Then
                                Drc.Reg_No = items.Replace("Registration No :", "").Trim.Replace("""", "").Trim
                            End If
                        End If

                        If line.Contains("Chassis No") Or line.Contains("Engine No") Or line.Contains("Model ") Then
                            If Drc.Vehicle_Details.Contains(items) Then
                                Continue For
                            End If
                            Drc.Vehicle_Details += items.Trim & " , "

                        End If

                        If line.Contains("KM READING") And Drc.KM_Reading = "" Then
                            items = items.Replace(":", " ")
                            If Not items.Contains("KM READING") Then
                                Drc.KM_Reading = items.Replace("""", "").Trim
                            End If

                        End If

                        If Drc.Customer_Name <> "" And Drc.KM_Reading = "" And items.Contains("KM READING") <> True Then
                            Drc.Address += items.Replace("""", "").Trim & ","
                        End If

                        If line.Contains("CUSTOMER NAME & ADDRESS") And Drc.Customer_Name = "" Then
                            If Not items.Contains("CUSTOMER NAME & ADDRESS") Then
                                Drc.Customer_Name = items.Replace("""", "").Trim
                            End If
                        End If

                        If line.Contains("Observation") And Drc.Observation = "" Then
                            If Not items.Contains("Observation") Then
                                Drc.Observation = items.Trim
                            Else
                            End If

                        ElseIf line.Contains("Supplier GSTIN") And Drc.Sup_GSTIN = "" Then
                            If Not items.Contains("Supplier GSTIN") Then
                                Drc.Sup_GSTIN = items.Trim
                            End If
                        End If

                        If line.Contains("POS") And Drc.POS = "" Then
                            If Not items.Contains("POS") Then
                                Drc.POS = items.Replace("""", "").Trim
                            End If
                        End If

                        If line.Contains("Recipient GSTIN") And Drc.Recpt_GSTIN = "" Then
                            If Not items.Contains("Recipient GSTIN") Then
                                Drc.Recpt_GSTIN = Drc.Recpt_GSTIN.Replace("""", "") & items.Replace("""", "").Trim
                            Else
                                If Details.Length < 3 Then
                                    Drc.Recpt_GSTIN = """"
                                End If
                            End If
                        End If

                    Next

                ElseIf SSI_Entry Then


                    Dim Details() As String = line.Split({","c}, StringSplitOptions.RemoveEmptyEntries)

                    For Each items In Details
                        items = items.Replace(":", "")
                        If items = "" Then
                            Continue For
                        End If


                        If line.Contains("Order/Allocation") And Drc.JobCard_No = "" Then
                            If Not items.Contains("Order/Allocation") Then
                                Drc.JobCard_No = items.Trim
                            End If
                        ElseIf line.Contains("Invoice No") And Drc.Invoice_No = "" Then
                            If Not items.Contains("Invoice No") Then
                                Drc.Invoice_No = items.Trim
                            End If
                        ElseIf line.Contains("Invoice Date") Then
                            If Not items.Contains("Invoice Date") Then
                                Drc.Invoice_Date = IIf(items.Length > 11, items.Substring(0, 10), items)
                            End If
                        ElseIf line.Contains("Sales Person") Then
                            If Not items.Contains("Sales Person") Then
                                '      Drc.Customer_Name = items.Trim
                            End If
                        ElseIf line.Contains("Supplier GSTIN") And Drc.Sup_GSTIN = "" Then
                            If Not items.Contains("Supplier GSTIN") Then
                                Drc.Sup_GSTIN = items.Trim
                            End If
                        ElseIf line.Contains("POS") And Drc.POS = "" Then
                            If Not items.Contains("POS") Then
                                Drc.POS = items.Replace("""", "").Trim
                            End If
                        ElseIf line.Contains("Recipient GSTIN") And Drc.Recpt_GSTIN = "" Then
                            If Not items.Contains("Recipient GSTIN") Then
                                Drc.Recpt_GSTIN = Drc.Recpt_GSTIN.Replace("""", "") & items.Replace("""", "").Trim
                            End If


                        Else
                            If Drc.Recpt_GSTIN <> "" And Drc.POS <> "" And Drc.Sup_GSTIN <> "" And Drc.Invoice_No <> "" And Drc.JobCard_No <> "" And Drc.Invoice_Date <> "" Then
                                SSI_Entry = False
                            End If

                            Continue For
                        End If



                    Next



                Else
                    PartDs.AcceptChanges()
                    Exit For
                End If

                If line.Contains("S No") = True And TableEntry <> True Then
                    TableEntry = True
                    If line.Contains("Part Code") Or line.Contains("PART NO") Then
                        TypeOfInv = "Part"
                    ElseIf line.Contains("Labour Code") Then
                        TypeOfInv = "Labour"

                    End If
                End If
                PartDs.AcceptChanges()


            Next


            Drc = PartDs.CustomerDetails.Rows(0)

            iDate = Convert.ToDateTime(Drc.Invoice_Date)

            'If is_SSI Then
            '    jDate = iDate
            '    JobCardNo = InvoiceNo
            'Else
            If is_SSI Then
                Drc.JobCard_Date = Drc.Invoice_Date
                Drc.JobCard_No = Drc.Invoice_No
            End If
            jDate = Convert.ToDateTime(Drc.JobCard_Date)

            'End If
            Dim ErrorString As String = ""

            For Each Dr In PartDs.PartDetails.Rows



                If Not Dr.Value = "TOTAL" Then
                    If Dr.S_No = 0 Then
                        ErrorString += " Not Found  :S_No  :" + IIf(Drc.Invoice_No = "", "Null", Drc.Invoice_No) + vbCrLf
                    End If
                    If Dr.Part_Labour_Code = "" Then
                        ErrorString += " Not Found  :Part_Labour_Code   :" + IIf(Drc.Invoice_No = "", "Null", Drc.Invoice_No) + vbCrLf

                    End If
                    If Dr.Part_Labour_Description = "" Then
                        ErrorString += " Not Found  :Part_Labour_Description   : " + IIf(Drc.Invoice_No = "", "Null", Drc.Invoice_No) + vbCrLf

                    End If
                    If Dr.CGST = 0.00 Then
                        ErrorString += " Not Found  :CGST   :" + IIf(Drc.Invoice_No = "", "Null", Drc.Invoice_No) + vbCrLf

                    End If
                    If Dr.CGST_Per = 0.00 Then
                        ErrorString += " Not Found  :CGST_Per   : " + IIf(Drc.Invoice_No = "", "Null", Drc.Invoice_No) + vbCrLf

                    End If
                    If Dr.Discount = 0.00 Then
                        '   ErrorString += " Not Found  :Discount" + IIf(Drc.Invoice_No = "", "Null", Drc.Invoice_No) + vbCrLf

                    End If
                    If Dr.Taxable_Amount = 0.00 Then
                        ErrorString += " Not Found  :Taxable_Amount   :" + IIf(Drc.Invoice_No = "", "Null", Drc.Invoice_No) + vbCrLf

                    End If
                    If Dr.Value = 0.00 Then
                        ErrorString += " Not Found  :Value    :" + IIf(Drc.Invoice_No = "", "Null", Drc.Invoice_No) + vbCrLf

                    End If
                    If Dr.HSN_No = "" Then
                        ErrorString += " Not Found  :HSN_No    :" + IIf(Drc.Invoice_No = "", "Null", Drc.Invoice_No) + vbCrLf

                    End If
                    If Dr.Qty = 0.00 Then
                        ErrorString += " Not Found  :Qty    :" + IIf(Drc.Invoice_No = "", "Null", Drc.Invoice_No) + vbCrLf

                    End If
                    If Dr.SGST = 0.00 Then
                        ErrorString += " Not Found  :SGST   : " + IIf(Drc.Invoice_No = "", "Null", Drc.Invoice_No) + vbCrLf

                    End If
                    If Dr.Amount = 0.00 Then
                        ErrorString += " Not Found  :Amount   :" + IIf(Drc.Invoice_No = "", "Null", Drc.Invoice_No) + vbCrLf
                    Else
                        Grand_Total += Dr.Amount
                    End If


                End If
            Next


            If ErrorString <> "" Then
                MessageBox.Show("Error On File ", "Value Missing", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification)
                If MsgBox("Error on File Could Not Find Value" & vbCrLf & "Do you Want to Continue" & ErrorString, MsgBoxStyle.YesNo, "Error") = vbNo Then
                    Status = Drc.Invoice_No + "_" + TypeOfInv
                    Exit Function
                End If
            End If



            Dim SplitDetails As String() = Drc.Vehicle_Details.Split(New String() {vbCr & vbLf, vbLf}, StringSplitOptions.None)
            Dim RegNo = "", Chaseno = "", engNo = "", model = "", modelName = ""
            Dim A As String = ""
            For Each line In SplitDetails

                If RegNo = "" Then
                    RegNo = line.ToString.Trim.Replace("Registration No  ", "")
                    If line.Contains("Chassis No") Then
                        Chaseno = line.ToString.Trim.Replace("Chassis No", "").Trim
                        Dim chassis() As String = Chaseno.Split({","}, StringSplitOptions.RemoveEmptyEntries)
                        Chaseno = chassis.First.ToString.Trim
                    End If
                End If

            Next

            Dim Drs As TallyDs.Service_HeaderRow
            Dim CashDs As New BkDS
            Dim dcr As BkDS.Cash_RegisterRow
            Dim StrQuery As String = ""
            Dim prefix As String = ""
            Dim NextId As String = ""
            Dim StrQry As String = ""
            Dim CashReg As String = ""
            Dim Servicebills As Integer = 0
            Dim ServiceBill As String = ""
            Dim Id As Integer = 0

            CashDs.Clear()
            dcr = CashDs.Cash_Register.Rows.Add
            dcr.Branch_Id = PublicShared.Branch_Id
            dcr.BkId = 0
            dcr.PayId = 0
            dcr.User_Id = PublicShared.User_Id
            dcr.User_Name = PublicShared.User_Name
            CashDs.AcceptChanges()

            If is_SSI Then
                prefix = Read_Settings("Prefix_CS")
            Else
                prefix = Read_Settings("Prefix_PL")
            End If
            Try

                Drs = Get_LastId(prefix)


                If Drs.Job_Card.Trim = Drc.JobCard_No.Trim Then


                Else

                    Drs.Invoice_Number = ""
                    Try

                        StrQry = " Select Inv_Number from service_bills where `JobCard_No` = '" & Drc.JobCard_No.Trim & "' ; "
                        cmd.CommandText = StrQry
                        ServiceBill = cmd.ExecuteScalar()

                        Drs.Invoice_Number = ServiceBill

                        If Drs.Invoice_Number = "" Then
                            NextId = Get_NextId(prefix)
                            Drs.Invoice_Number = NextId
                        End If

                    Catch ex As Exception
                        NextId = Get_NextId(prefix)
                        Drs.Invoice_Number = NextId

                    End Try






                End If
            Catch

                StrQuery = " Select Count(Id) from service_bills  "
                cmd.CommandText = StrQuery
                NextId = cmd.ExecuteScalar()

                NextId = prefix & Format(Val(NextId) + 1, "00000")
                Drs.Invoice_Number = NextId.Trim

            End Try
            '    cmd.CommandText = StrQuery
            '    Id = cmd.ExecuteScalar()
            'Catch ex As Exception
            '    Id = 0
            'End Try



            Try
                StrQry = " Select Id from service_bills where `JobCard_No` = '" & Drc.JobCard_No.Trim & "' and Type ='" & TypeOfInv.Trim & "' ; "
                cmd.CommandText = StrQry
                Servicebills = cmd.ExecuteScalar()
            Catch ex As Exception
                Servicebills = 0
            End Try


            If Servicebills <> 0 Then
                '  Drc.Invoice_No = ServiceBill
            Else

                QryHead += "INSERT INTO service_bills (Inv_Number,JobCard_No,Invoice_Date,Invoice_No,JobCard_Date,Reg_No,Chassis_No,Vehicle_Details," &
                        "Customer_Name,Customer_Address,KM_Reading,Sales_Person,Type," &
                        "Part_Labour_Code,Part_Labour_Description,HSN_No,Rate," &
                        "Qty,Discount,PaiseRoundingOff,CGST_Per,CGST,SGST_Per," &
                        "SGST,Total_Amount,LastSeen,Taxable) VALUES "

                For Each Dr In PartDs.PartDetails.Rows

                    If Dr.Value <> "TOTAL" Then

                        QryData += IIf(QryData = "", "", ",")


                        QryData += " (" & "'" & Drs.Invoice_Number.Trim & "'," &
                    "'" & Drc.JobCard_No & "','" & Format(iDate, "yyyy-MM-dd") & "','" & Drc.Invoice_No.Trim & "','" & Format(jDate, "yyyy-MM-dd") & "','" & Drc.Reg_No & "','" & Chaseno & "','" & Drc.Vehicle_Details & "'," &
                    "'" & Drc.Customer_Name & "','" & Drc.Address & "','" & Drc.KM_Reading & "','','" & TypeOfInv.Trim & "'," &
                    "'" & Dr.Part_Labour_Code & "','" & Dr.Part_Labour_Description & "','" & Dr.HSN_No & "','" & Dr.Value & "'," &
                    "'" & Dr.Qty & "','" & Dr.Discount & "','0.00','" & Dr.CGST_Per & "','" & Dr.CGST & "','" & Dr.SGST_Per & "'," &
                    "'" & Dr.SGST & "','" & Dr.Amount & "','" & Format(DateTime.Now, "yyyy-MM-dd") & "','" & Dr.Taxable_Amount & "')"

                    Else

                    End If


                Next

                QryData += ";"

            End If


            Try
                Qry = "SELECT Id FROM service_bills_head WHERE Invoice_Date = '" & Format(iDate, "yyyy-MM-dd") & "' AND `Invoice_Number` = '" & Drc.JobCard_No.Trim & "' AND Type ='" & TypeOfInv.Trim & "';"
                cmd.CommandText = Qry
                Id = cmd.ExecuteScalar()
            Catch ex As Exception
                Id = 0
            End Try

            If Id <> 0 Then

            Else

                Qry += " INSERT INTO service_bills_head (Invoice_Number,JobCard_No,Invoice_Date,Invoice_Amount,Type,LastSeen) VALUES (" &
                            "'" & Drs.Invoice_Number.Trim & "','" & Drc.JobCard_No & "','" & Format(iDate, "yyyy-MM-dd") & "'," &
                            "'" & Grand_Total & "','" & TypeOfInv & "',now())" 'Val(Grand_Total)



                If QryData <> "" Then
                    cmd.CommandText = QryHead + QryData + Qry
                    cmd.ExecuteNonQuery()

                End If

            End If

            Try
                StrQry = "Select id from cash_register where JobCardNo='" & Drc.JobCard_No & "' and Type = '" & TypeOfInv & "'" &
                     " and Payment_Mode=''"
                cmd.CommandText = StrQry
                CashReg = cmd.ExecuteScalar
            Catch ex As Exception
                CashReg = 0
            End Try

            If CashReg > 0 Then

            Else
                StrQry = "Insert into cash_register (Entry_Date,Entry_Type,Entry_Head,Entry_Sub_Head,Ref_No,Amount,Type,Party,JobCardNo,Branch_Id,User_Id,Chassis_No," &
                          "User_Name,Created_On) Values('" & Format(iDate, "yyyy-MM-dd") & "','Receipt','Service','Service Invoice','" & Drs.Invoice_Number & "','" & Grand_Total & "'," &
                          "'" & TypeOfInv & "','" & Drc.Customer_Name & "','" & Drc.JobCard_No & "','" & dcr.Branch_Id & "','" & dcr.User_Id & "','" & Chaseno & "','" & dcr.User_Name & "'," &
                          "'" & Format(Date.Now, "yyyy-MM-dd HH:mm:ss") & "')"


                cmd.CommandText = StrQry
                cmd.ExecuteNonQuery()
            End If

            'Tr.Commit()
            'Tr.Dispose()
            cn.Close()
            cn.Dispose()
            cn = Nothing

            Status = Drs.Invoice_Number.Replace("/", "_") + "_" + TypeOfInv + "True"
            Return Status


        Catch ex As Exception
            'If Not Tr Is Nothing Then
            '    Tr.Rollback()

            'End If
            cn.Close()
            cn.Dispose()
            Status = Drc.Invoice_No.Replace("/", "_") + "_" + TypeOfInv
            Return Status

            MsgBox("Error Found : " & ex.Message, vbCritical)
        End Try

        Return Status
    End Function


    Public Function LblStatus(ByRef LblServiceStatus As Label, ByVal visible As Boolean, ByVal Txt As String)

        If visible Then
            LblServiceStatus.Visible = True
            LblServiceStatus.Text = Txt
            LblServiceStatus.Refresh()


            Txt = Txt.Replace("_", "-")

            LblServiceStatus.Refresh()

            Txt = Txt.Replace("-", "\")

            LblServiceStatus.Refresh()

            Txt = Txt.Replace("\", "|")

            LblServiceStatus.Refresh()

            Txt = Txt.Replace("|", "/")

            LblServiceStatus.Refresh()
            Txt = Txt.Replace("/", ".")

            LblServiceStatus.Refresh()
        Else
            LblServiceStatus.Text = Txt
            LblServiceStatus.Visible = False
            LblServiceStatus.Refresh()
        End If


        Return Nothing
    End Function

    Public Shared Function Insert_Service_Header(ByVal Ds As DataSet, ByRef LblStat As Label) As Boolean

        Dim Qry As String = ""
        Dim Status As Boolean
        Dim invdate As String = ""

        Dim Inv_date As Date
        Dim InvNo As String = ""
        Dim Model As String = ""
        Dim Type As String = ""
        Dim Total As Integer = Ds.Tables("ServiceData").Rows.Count
        Dim Labour_SACCode As String = Read_Settings("Labour_SACCode")
        Dim Value As String = ""
        ' Dim Taxable18, Taxable28 As Decimal
        Dim Qty_Fraction As Decimal = 0.0
        Dim BillType As String = ""
        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        'Dim strQuery As String
        Dim PartType As String = "Item"

        Try

            cmd.Connection = cn
            cn.Open()

            ' IGST18%, IGST18, IGST28%, IGST28




            If Not Ds.Tables("ServiceData").Columns.Contains("IGST28") Then
                'Ds.Tables("ServiceData").Columns.Add("IGST28")
                Dim Dc As New DataColumn
                Dc.DataType = System.Type.GetType("System.Decimal")
                Dc.DefaultValue = 0.00
                Dc.ColumnName = "IGST28"
                Ds.Tables("ServiceData").Columns.Add(Dc)

            End If
            If Not Ds.Tables("ServiceData").Columns.Contains("IGST28%") Then
                'Ds.Tables("ServiceData").Columns.Add("IGST28%")
                Dim Dc As New DataColumn
                Dc.DataType = System.Type.GetType("System.Decimal")
                Dc.DefaultValue = 0.00
                Dc.ColumnName = "IGST28%"
                Ds.Tables("ServiceData").Columns.Add(Dc)
            End If
            If Not Ds.Tables("ServiceData").Columns.Contains("IGST18") Then
                'Ds.Tables("ServiceData").Columns.Add("IGST18")
                Dim Dc As New DataColumn
                Dc.DataType = System.Type.GetType("System.Decimal")
                Dc.DefaultValue = 0.00
                Dc.ColumnName = "IGST18"
                Ds.Tables("ServiceData").Columns.Add(Dc)
            End If
            If Not Ds.Tables("ServiceData").Columns.Contains("IGST18%") Then
                ' Ds.Tables("ServiceData").Columns.Add("IGST18%")
                Dim Dc As New DataColumn
                Dc.DataType = System.Type.GetType("System.Decimal")
                Dc.DefaultValue = 0.00
                Dc.ColumnName = "IGST18%"
                Ds.Tables("ServiceData").Columns.Add(Dc)
            End If


            Dim y = 100 / Total
            Dim x = 100 / Total
            x -= y
            Dim z As String = ""
            Dim Taxable As Decimal = 0.0
            Dim StatusString As String = "...................................................................................................."
            Dim Regex = New Regex(".")
            Dim Accepte_Bills As String = Read_Settings("Service_Accepted_Bills")
            For Each DrD In Ds.Tables("ServiceData").Rows

                x = x + y
                If Math.Round(x, 0) > Math.Round(Val(z), 0) Then
                    '  StatusString += "|"
                    StatusString = Regex.Replace(StatusString, "|", CInt(Val(x)))
                    ' StatusString = Replace(StatusString, ".", "", , 4)
                Else

                End If
                z = (Math.Round(x, 0)).ToString

                LblStat.Text = "Inserting  " + StatusString + "  " + z + "/100"
                ' LblStat.Text = LblStat.Text.Replace("-", "\")
                LblStat.Refresh()
                Taxable = 0.00

                Try
                    Inv_date = DrD("Invoice Date")
                Catch ex As Exception
                    Continue For
                End Try


                BillType = Strings.Left(DrD("Invoice").ToString, 3)

                If Accepte_Bills.Contains(BillType) Then
                Else
                    Continue For
                End If

                If DrD("Invoice") <> "" Then

                    Taxable = Val(DrD("Parts Amount")) + Val(DrD("Service Amount")) + Val(DrD("OilAmount"))

                    Qry = " Delete from service_header where Invoice_Date = '" & Format(Inv_date, "yyyy-MM-dd") & "' and `Invoice_Number` = '" & DrD("Invoice") & "' ; "
                    Qry += " INSERT INTO Service_Header (Invoice_Number,Invoice_Date,Invoice_Amount,`CGST14_Per`,`CGST14`,`SGST14_Per`,`SGST14`," &
                            "`CGST9_Per`,`CGST9`,`SGST9_Per`,`SGST9`,`KFC1_Per`,`KFC1`,`IGST18_Per`,`IGST18`,`IGST28_Per`,`IGST28`,`Taxable_Amount` ,PaiseRound) Values (" &
                                "'" & DrD("Invoice").trim & "','" & Format(Inv_date, "yyyy-MM-dd") & "'," &
                                "'" & Val(DrD("Invoice Amount")) & "','" & Val(DrD("CGST14%")) & "','" & Val(DrD("CGST14")) & "','" & Val(DrD("SGST14%")) & "','" & Val(DrD("SGST14")) & "'," &
                                "'" & Val(DrD("CGST9%")) & "','" & Val(DrD("CGST9")) & "','" & Val(DrD("SGST9%")) & "','" & Val(DrD("SGST9")) & "'," &
                    "'" & Val(DrD("KFC%")) & "','" & Val(DrD("KFC1%")) & "','" & Val(DrD("IGST18%")) & "','" & Val(DrD("IGST18")) & "','" & Val(DrD("IGST28%")) & "','" & Val(DrD("IGST28")) & "'," &
                    "'" & Taxable & "','" & Val(DrD("PaiseRoundingOff")) & "'); "

                    cmd.CommandText = Qry
                    cmd.ExecuteNonQuery()

                End If

            Next

            cn.Close()
            cn = Nothing

            Status = True


        Catch ex As Exception
            Status = False
            MsgBox("Error Found : " & ex.Message)
        End Try


        Return Status

    End Function


    Public Shared Function Insert_Service_SSI(ByVal Ds As DataSet, ByRef LblStat As Label) As String

        Dim Qry = "", strQuery = "", Qry_Insert = "", Qry_Delete As String = ""
        Dim Status As Boolean
        Dim invdate As String = ""

        Dim Inv_date As Date
        Dim InvNo As String = ""
        Dim Model As String = ""
        Dim Stck_Ds As New TallyDs
        Dim Stemp As New TallyDs
        Dim Drs As TallyDs.Stock_masterRow
        Dim Rate = 0.0, Disc_per = 0.0, IGST_per = 0.0, IGST = 0.0, Taxable = 0.0, Total_Amt = 0.0, Dsic As Double = 0.0

        '   Dim Total As Integer = Ds.Tables("ServiceData").Rows.Count
        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim da As New MySqlDataAdapter
        Dim cmd As New MySqlCommand
        Dim Missing_Stock As String = ""
        Dim CGST = 0.0, CGST_per = 0.0, SGST = 0.0, Tax_per = 0.0, SGST_per = 0.0, KFC = 0.0, KFC_per As Double = 0.0
        Dim TDS As New DataSet
        Dim old_Inv As String = ""
        Dim Inser_Header As Boolean = False
        Dim Inser_Details As Boolean = False
        Dim Grand_tot As Double = 0
        Dim BillType As String = ""
        Dim Sum_CGST14 As Double = 0
        Dim Sum_SGST14 As Double = 0
        Dim Sum_CGST9 As Double = 0
        Dim Sum_IGST28 As Double = 0
        Dim Sum_IGST18 As Double = 0
        Dim Sum_SGST9 As Double = 0
        Dim sum_KFC As Double = 0


        InvNo = ""

        Dim Total As Integer = Ds.Tables("ServiceData").Rows.Count
        Dim y = 100 / Total
        Dim x = 100 / Total
        x -= y
        Dim z As String = ""
        Dim StatusString As String = "...................................................................................................."
        Dim Regex = New Regex(".")

        Try

            For Each DrD In Ds.Tables("ServiceData").Rows

                x = x + y
                If Math.Round(x, 0) > Math.Round(Val(z), 0) Then
                    '  StatusString += "|"
                    StatusString = Regex.Replace(StatusString, "|", CInt(Val(x)))
                    ' StatusString = Replace(StatusString, ".", "", , 4)
                Else

                End If
                z = (Math.Round(x, 0)).ToString

                LblStat.Text = "Inserting  " + StatusString + "  " + z + "/100"
                ' LblStat.Text = LblStat.Text.Replace("-", "\")
                LblStat.Refresh()

                If DrD("Invoice Date") <> "" Then

                    If DrD("Invoice Date").ToString.Contains("SSI") Or
                        DrD("Invoice Date").ToString.Contains("IPI") Then


                        Qry_Delete = " Delete from service where `Invoice_Number` =  '" & DrD("Invoice Date") & "' ; " &
                         " Delete from service_header where `Invoice_Number` =  '" & DrD("Invoice Date") & "' ; "



                        Status = RunQuery(Qry_Delete)
                        If InvNo = "" Then

                        Else
                            old_Inv = InvNo
                        End If
                        InvNo = DrD("Invoice Date")
                        Inser_Header = True
                        Inser_Details = True

                    Else



                        If InvNo <> "" Then
                            old_Inv = InvNo
                        End If

                        InvNo = ""


                        Inser_Header = False
                        Continue For
                    End If

                End If


                If Inser_Details = True And DrD("Part No") <> "" And InvNo <> "" Then

                    Stemp = New TallyDs
                    Disc_per = 0 : IGST_per = 0 : IGST = 0 : Taxable = 0 : Dsic = 0 : Total_Amt = 0 : Rate = 0 : CGST_per = 0 : CGST = 0 : SGST_per = 0 : SGST = 0 : KFC = 0 : KFC_per = 0

                    Rate = Val(DrD("Rate"))
                    Dsic = Val(DrD("Discount"))

                    Taxable = Val(DrD("Qty")) * Val(DrD("Rate"))
                    Total_Amt = Val(DrD("Total Amount"))

                    If Dsic > 0 Then
                        Disc_per = Math.Round((Dsic / Taxable) * 100)
                    End If

                    Taxable = Format(Taxable - Dsic, "0.00")

                    IGST = Val(DrD("Tax Amount"))



                    If IGST > 0 Then

                        IGST_per = Math.Round((IGST / Taxable) * 100)
                        CGST_per = IGST_per / 2
                        SGST_per = IGST_per / 2

                        CGST = Format(IGST / 2, "0.00")
                        SGST = Format(IGST / 2, "0.00")

                    Else

                        CGST_per = Val(DrD("CGST%"))
                        SGST_per = Val(DrD("SGST%"))
                        KFC_per = Val(DrD("KFC%"))
                        CGST = Val(DrD("CGST"))
                        SGST = Val(DrD("SGST"))
                        KFC = Val(DrD("KFC1%"))

                        If Ds.Tables("ServiceData").Columns.Contains("IGST%") Then
                            IGST_per = Val(DrD("IGST%"))
                            IGST = Val(DrD("IGST"))
                        End If

                    End If
                    If Ds.Tables("ServiceData").Columns.Contains("IGST%") Then
                        If Val(DrD("IGST%")) > 0 Then
                            IGST_per = Val(DrD("IGST%"))
                            IGST = Val(DrD("IGST"))
                        End If
                    End If

                    BillType = Left(InvNo, 3)
                        Taxable = Format(Total_Amt - (CGST + SGST), "0.00")

                        Qry_Insert = " INSERT INTO service (Job_Card,Job_Card_Date,Invoice_Number,Company_Name,Service_Advisor,Header_Job_Type,Job_Type,Customer_Name, " &
                                " Mobile_No,Type,Part_Labour_Code,Part_Labour_Description,HSN_SAC_code,Issued_Qty,Rate,Discount,Discount_per, " &
                                " CGST_Per,CGST,SGST_Per,SGST,Taxable,KFC,KFC_Per,IGST_Per,IGST,Total_Amount) Values (" &
                                "'" & IIf(BillType = "IPI", "", InvNo.Trim) & "','" & Format(DrD("F2"), "yyyy-MM-dd") & "','" & InvNo.Trim & "','" & ReplaceQuote(DrD("Company")) & "', " &
                                "'" & ReplaceQuote(DrD("Salesperson")) & "','" & ReplaceQuote(DrD("Transaction Type")) & "', " &
                                " '" & ReplaceQuote(DrD("Payment Type")) & "', '" & ReplaceQuote(DrD("Customer")) & "','" & ReplaceQuote(DrD("Mobile No")) & "', " &
                                " 'Item','" & ReplaceQuote(DrD("Part No")) & "','" & ReplaceQuote(DrD("Description")) & "','" & ReplaceQuote(DrD("HSN Code")) & "', " &
                                " '" & Val(DrD("Qty")) & "','" & Val(DrD("Rate")) & "','" & Dsic & "','" & Disc_per & "'," &
                                " '" & CGST_per & "','" & CGST & "', '" & SGST_per & "','" & SGST & "','" & Taxable & "','" & KFC & "','" & KFC_per & "','" & IGST_per & "','" & IGST & "','" & Total_Amt & "' ); "

                        Status = RunQuery(Qry_Insert)
                        Grand_tot = Grand_tot + Total_Amt

                        If CGST_per.ToString.Contains("14") Then
                            Sum_CGST14 += CGST
                        ElseIf CGST_per.ToString.Contains("9") Then
                            Sum_CGST9 += CGST
                        End If

                        If SGST_per.ToString.Contains("14") Then
                            Sum_SGST14 += SGST
                        ElseIf SGST_per.ToString.Contains("9") Then
                            Sum_SGST9 += SGST
                        End If

                        If IGST_per.ToString.Contains("28") Then
                            Sum_IGST28 += IGST
                        ElseIf IGST_per.ToString.Contains("18") Then
                            Sum_IGST18 += IGST
                        End If


                        If KFC > 0 Then
                            sum_KFC += KFC
                        End If


                    ElseIf Inser_Header = True And old_Inv <> "" Then

                        Qry_Insert = " Insert into service_header (Invoice_Number,Invoice_Date,Invoice_Amount,`CGST14_Per`,`CGST14`,`SGST14_Per`,`SGST14`," &
                                "`CGST9_Per`,`CGST9`,`SGST9_Per`,`SGST9`,`KFC1_Per`,`KFC1`,IGST18_Per,IGST18,IGST28_Per,IGST28) " &
                      " Select '" & old_Inv.Trim & "',Job_Card_Date,'" & Grand_tot & "','" & IIf(Sum_CGST14 > 0, "14.00", "0.00") & "','" & Sum_CGST14 & "'," &
                      "'" & IIf(Sum_SGST14 > 0, "14.00", "0.00") & "','" & Sum_CGST14 & "','" & IIf(Sum_CGST9 > 0, "9.00", "0.00") & "','" & Sum_CGST9 & "'," &
                      "'" & IIf(Sum_SGST9 > 0, "9.00", "0.00") & "','" & Sum_CGST9 & "','" & IIf(sum_KFC > 0, "1.00", "0.00") & "','" & sum_KFC & "'," &
                       "'" & IIf(Sum_IGST18 > 0, "18.00", "0.00") & "','" & Sum_IGST18 & "','" & IIf(Sum_IGST28 > 0, "28.00", "0.00") & "','" & Sum_IGST28 & "'" &
                    "from service where invoice_number = '" & old_Inv.Trim & "' group by  '" & old_Inv.Trim & "'"
                    Status = RunQuery(Qry_Insert)
                    Grand_tot = 0
                    sum_KFC = 0 : Sum_CGST14 = 0 : Sum_CGST9 = 0 : Sum_SGST14 = 0 : Sum_CGST14 = 0 : Sum_IGST18 = 0 : Sum_IGST28 = 0
                    Inser_Header = False
                End If

            Next

            If Inser_Header = False And InvNo <> "" Then

                'Qry_Insert = " Insert into service_header (Invoice_Number,Invoice_Date,Invoice_Amount) " &
                '       " Select '" & InvNo.Trim & "',Job_Card_Date,'" & Grand_tot & "' from service where invoice_number = '" & InvNo.Trim & "' group by  '" & InvNo.Trim & "'"
                Qry_Insert = " Insert into service_header (Invoice_Number,Invoice_Date,Invoice_Amount,`CGST14_Per`,`CGST14`,`SGST14_Per`,`SGST14`," &
                                "`CGST9_Per`,`CGST9`,`SGST9_Per`,`SGST9`,`KFC1_Per`,`KFC1`,IGST18_Per,IGST18,IGST28_Per,IGST28) " &
                      " Select '" & InvNo.Trim & "',Job_Card_Date,'" & Grand_tot & "','" & IIf(Sum_CGST14 > 0, "14.00", "0.00") & "','" & Sum_CGST14 & "'," &
                      "'" & IIf(Sum_SGST14 > 0, "14.00", "0.00") & "','" & Sum_CGST14 & "','" & IIf(Sum_CGST9 > 0, "9.00", "0.00") & "','" & Sum_CGST9 & "'," &
                      "'" & IIf(Sum_SGST9 > 0, "9.00", "0.00") & "','" & Sum_CGST9 & "','" & IIf(sum_KFC > 0, "1.00", "0.00") & "','" & sum_KFC & "'," &
                       "'" & IIf(Sum_IGST18 > 0, "18.00", "0.00") & "','" & Sum_IGST18 & "','" & IIf(Sum_IGST28 > 0, "28.00", "0.00") & "','" & Sum_IGST28 & "'" &
                    "from service where invoice_number = '" & InvNo.Trim & "' group by  '" & InvNo.Trim & "'"
                Status = RunQuery(Qry_Insert)
                Grand_tot = 0
                sum_KFC = 0 : Sum_CGST14 = 0 : Sum_CGST9 = 0 : Sum_SGST14 = 0 : Sum_CGST14 = 0
                Inser_Header = False
            End If

        Catch ex As Exception

        End Try

        Return "True"

    End Function

    Public Shared Function Insert_Service_ILP(ByVal Ds As DataSet, ByRef LblStat As Label) As String

        Dim Qry = "", strQuery = "", Qry_Insert = "", Qry_Delete As String = ""
        Dim Status As Boolean
        Dim invdate As String = ""

        Dim Inv_date As Date
        Dim InvNo As String = ""
        Dim RegNo As String = ""
        Dim Model As String = ""
        Dim Stck_Ds As New TallyDs
        Dim Stemp As New TallyDs
        Dim Drs As TallyDs.Stock_masterRow
        Dim Rate = 0.0, Disc_per = 0.0, IGST_per = 0.0, IGST = 0.0, Taxable = 0.0, Total_Amt = 0.0, Dsic As Double = 0.0

        '   Dim Total As Integer = Ds.Tables("ServiceData").Rows.Count
        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim da As New MySqlDataAdapter
        Dim cmd As New MySqlCommand
        Dim Missing_Stock As String = ""
        Dim CGST = 0.0, CGST_per = 0.0, SGST = 0.0, Tax_per = 0.0, SGST_per = 0.0, KFC = 0.0, KFC_per As Double = 0.0
        Dim TDS As New DataSet
        Dim old_Inv As String = ""
        Dim Inser_Header As Boolean = False
        Dim Inser_Details As Boolean = False
        Dim Grand_tot As Double = 0
        Dim BillType As String = ""
        Dim Sum_IGST18 As Double = 0
        Dim Sum_IGST28 As Double = 0
        Dim sum_KFC As Double = 0


        InvNo = ""

        Dim Total As Integer = Ds.Tables("ServiceData").Rows.Count
        Dim y = 100 / Total
        Dim x = 100 / Total
        x -= y
        Dim z As String = ""
        Dim StatusString As String = "...................................................................................................."
        Dim Regex = New Regex(".")
        Dim InvType As String = ""

        Try


            cmd.Connection = cn
            cn.Open()


            For Each DrD In Ds.Tables("ServiceData").Rows

                x = x + y
                If Math.Round(x, 0) > Math.Round(Val(z), 0) Then
                    '  StatusString += "|"
                    StatusString = Regex.Replace(StatusString, "|", CInt(Val(x)))
                    ' StatusString = Replace(StatusString, ".", "", , 4)
                Else

                End If
                z = (Math.Round(x, 0)).ToString

                LblStat.Text = "Inserting  " + StatusString + "  " + z + "/100"
                ' LblStat.Text = LblStat.Text.Replace("-", "\")
                LblStat.Refresh()

                'If DrD("Document Name") <> "" Then

                If DrD("Document Name").ToString.Contains("ILI") Or
                    DrD("Document Name").ToString.Contains("IPI") Then

                    BillType = Left(DrD("Document Name"), 3)

                    If DrD("Document Name").ToString.Trim <> InvNo Then

                        RegNo = " "
                        Try
                            Qry = "select Reg_No from service where Chassis_No ='" & DrD("Serial No").ToString.Trim & "'"
                            cmd.CommandText = Qry
                            RegNo = cmd.ExecuteScalar()

                        Catch ex As Exception
                            RegNo = " "
                        End Try

                        If BillType = "ILI" Then
                            Qry_Delete = " Delete from service where `Invoice_Number` =  '" & DrD("Document Name") & "' ; "
                        Else
                            Qry_Delete = " Update service set Job_Card='" & DrD("JobCardNo").trim & "',Reg_No='" & RegNo & "',Chassis_No='" & DrD("Serial No").ToString.Trim & "' where `Invoice_Number` =  '" & DrD("Document Name") & "' ; "
                            ' Qry_Insert = " Update serviceheader set Job_Card='" & DrD("JobCardNo").trim & "' where `Invoice_Number` =  '" & DrD("Document Name") & "' ; "

                        End If


                        Status = RunQuery(Qry_Delete)



                        InvNo = DrD("Document Name")

                        If BillType = "ILI" Then

                            Total_Amt = Math.Round(Val(DrD("Grand Total").ToString.Replace(",", "")), 2)
                            Qry_Insert = " insert into service (Job_Card ,Invoice_Number,Customer_Name,Chassis_No,Reg_No," &
                                    "Total_Amount) values (" &
                                        "'" & DrD("JobCardNo").ToString.Trim & "','" & InvNo & "','" & DrD("Customer").ToString.Trim & "','" & DrD("Serial No").ToString.Trim & "','" & RegNo & "'," &
                                     "'" & Total_Amt & "')"
                            Status = RunQuery(Qry_Insert)

                        End If


                    End If


                End If



            Next



        Catch ex As Exception

            cn.Close()

        End Try


        cn.Close()
        cn = Nothing
        Return "True"

    End Function


    Public Shared Function Insert_Service_WLP(ByVal Ds As DataSet, ByRef LblStat As Label) As String

        Dim Qry = "", strQuery = "", Qry_Insert = "", Qry_Delete As String = ""
        Dim Status As Boolean
        Dim invdate As String = ""

        Dim Inv_date As Date
        Dim InvNo As String = ""
        Dim Model As String = ""
        Dim Stck_Ds As New TallyDs
        Dim Stemp As New TallyDs
        Dim Drs As TallyDs.Stock_masterRow
        Dim Rate = 0.0, Disc_per = 0.0, IGST_per = 0.0, IGST = 0.0, Taxable = 0.0, Total_Amt = 0.0, Dsic As Double = 0.0

        '   Dim Total As Integer = Ds.Tables("ServiceData").Rows.Count
        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim da As New MySqlDataAdapter
        Dim cmd As New MySqlCommand
        Dim Missing_Stock As String = ""
        Dim CGST = 0.0, CGST_per = 0.0, SGST = 0.0, Tax_per = 0.0, SGST_per = 0.0, KFC = 0.0, KFC_per As Double = 0.0
        Dim TDS As New DataSet
        Dim old_Inv As String = ""
        Dim Inser_Header As Boolean = False
        Dim Inser_Details As Boolean = False
        Dim Grand_tot As Double = 0

        Dim Sum_IGST18 As Double = 0
        Dim Sum_IGST28 As Double = 0
        Dim sum_KFC As Double = 0


        InvNo = ""

        Dim Total As Integer = Ds.Tables("ServiceData").Rows.Count
        Dim y = 100 / Total
        Dim x = 100 / Total
        x -= y
        Dim z As String = ""
        Dim StatusString As String = "...................................................................................................."
        Dim Regex = New Regex(".")
        Dim InvType As String = ""

        Try




            For Each DrD In Ds.Tables("ServiceData").Rows

                x = x + y
                If Math.Round(x, 0) > Math.Round(Val(z), 0) Then
                    '  StatusString += "|"
                    StatusString = Regex.Replace(StatusString, "|", CInt(Val(x)))
                    ' StatusString = Replace(StatusString, ".", "", , 4)
                Else

                End If
                z = (Math.Round(x, 0)).ToString

                LblStat.Text = "Inserting  " + StatusString + "  " + z + "/100"
                ' LblStat.Text = LblStat.Text.Replace("-", "\")
                LblStat.Refresh()

                'If DrD("Document Name") <> "" Then

                If DrD("Document Name").ToString.Contains("WLI") Or
                    DrD("Document Name").ToString.Contains("WPI") Then

                    If DrD("Document Name").ToString.Trim <> InvNo Then

                        Qry_Delete = " Delete from service where `Invoice_Number` =  '" & DrD("Document Name") & "' ; " &
                         " Delete from service_header where `Invoice_Number` =  '" & DrD("Document Name") & "' ; "

                        Status = RunQuery(Qry_Delete)

                        If InvNo = "" Then

                        Else
                            old_Inv = InvNo
                        End If



                        InvNo = DrD("Document Name")
                        If InvNo.Contains("WLI") Then
                            InvType = "Labour"
                        ElseIf InvNo.Contains("WPI") Then
                            InvType = "Item"
                        Else
                            InvType = ""
                        End If



                        Inser_Header = True
                        Inser_Details = True

                        If Inser_Header = True And old_Inv <> "" Then

                            Qry_Insert = " Insert into service_header (Invoice_Number,Invoice_Date,Invoice_Amount,IGST18_Per,IGST28_Per,IGST18,IGST28) " &
                              " Select '" & old_Inv.Trim & "',Job_Card_Date,'" & Grand_tot & "','" & IIf(Sum_IGST18 > 0, "18.00", "0.00") & "','" & IIf(Sum_IGST28 > 0, "28.00", "0.00") & "'," &
                            "'" & Sum_IGST18 & "','" & Sum_IGST28 & "' from service where invoice_number = '" & old_Inv.Trim & "' group by  '" & old_Inv.Trim & "'"
                            Status = RunQuery(Qry_Insert)
                            Grand_tot = 0
                            ' sum_KFC = 0 : Sum_CGST14 = 0 : Sum_CGST9 = 0 : Sum_SGST14 = 0 : Sum_CGST14 = 0
                            old_Inv = ""
                            Sum_IGST18 = 0.0 : Sum_IGST28 = 0.0
                        End If

                    End If

                    IGST = 0.00
                    IGST = Format(Val(DrD("IGST").ToString.Replace(",", "")), "0.00")
                    Total_Amt = Format(Val(DrD("Total Amount").ToString.Replace(",", "")), "0.00")

                    Qry_Insert = " insert into service (Job_Card ,Invoice_Number,Company_Name,Job_Card_Date,Reg_No,Model_Code,Model_Description,Customer_Name,Type,Part_Labour_Code," &
                                    "Part_Labour_Description,Issued_Qty,Rate,HSN_SAC_code,IGST,IGST_Per,Total_Amount) values (" &
                                        "'" & InvNo & "','" & InvNo & "','" & DrD("Company") & "','" & Format(DrD("Document Date"), "yyyy-MM-dd") & "','" & DrD("Registration No") & "'," &
                                     "'" & DrD("Model Code") & "','" & DrD("Model Name") & "','" & DrD("Customer") & "','" & InvType & "','" & DrD("Part/Labour Code") & "'," &
                                     "'" & DrD("Part/Labour Name") & "','" & DrD("Qty") & "','" & DrD("Rate") & "','" & DrD("HSN Code") & "','" & IGST & "','" & DrD("IGST%") & "'," &
                                     "'" & Total_Amt & "')"


                    Status = RunQuery(Qry_Insert)
                    Grand_tot = Grand_tot + Total_Amt

                    If DrD("IGST%").ToString.Contains("18") Then
                        Sum_IGST18 += IGST
                    ElseIf DrD("IGST%").ToString.Contains("28") Then
                        Sum_IGST28 += IGST
                    End If






                End If









            Next

            If Inser_Header = True And old_Inv = "" And InvNo <> "" Then
                old_Inv = InvNo

                Qry_Insert = " Insert into service_header (Invoice_Number,Invoice_Date,Invoice_Amount,IGST18%,IGST28%,IGST18,IGST28) " &
                  " Select '" & old_Inv.Trim & "',Job_Card_Date,'" & Grand_tot & "','" & IIf(Sum_IGST18 > 0, "'18.00'", "'0.00") & "','" & IIf(Sum_IGST28 > 0, "'28.00'", "'0.00") & "'," &
                "'" & Sum_IGST18 & "','" & Sum_IGST28 & "' from service where invoice_number = '" & old_Inv.Trim & "' group by  '" & old_Inv.Trim & "'"
                Status = RunQuery(Qry_Insert)
                Grand_tot = 0
                ' sum_KFC = 0 : Sum_CGST14 = 0 : Sum_CGST9 = 0 : Sum_SGST14 = 0 : Sum_CGST14 = 0
                old_Inv = ""
                Sum_IGST18 = 0.0 : Sum_IGST28 = 0.0
            End If

        Catch ex As Exception

        End Try

        Return "True"

    End Function

    Public Shared Function Get_Summary_purchasevehicle() As TallyDs

        Dim cn As New MySqlConnection(ConnectionString)
        Dim cmd As New MySqlCommand
        Dim Da As New MySqlDataAdapter
        Dim strQuery As String = ""

        Dim Ds As New TallyDs

        Try

            cmd.Connection = cn
            cn.Open()

            strQuery = " SELECT Invoice_No,PostedToTally as To_Tally,Invoice_Number,Invoice_Date,sum(Rate) Rate,sum(Total_Amount) Total_Amount,count(Invoice_No) as Total_Qty,ifnull(sum(SGST),0) SGST, " &
                      " ifnull(sum(CGST),0) CGST,ifnull(sum(CESS),0) CESS,ifnull(sum(Freight),0) Freight,Doc_Date as Document_Date   " &
                      " from purchasevehicle where 1=1 group by Invoice_No,Invoice_Number"

            cmd.CommandText = strQuery
            Da.SelectCommand = cmd
            Da.Fill(Ds, "PurchaseVehicle")

            strQuery = " SELECT Invoice_No,Invoice_Number,Invoice_Date,Rate+ifnull(Freight,0) AS Rate,ModelFamily,Description,'1' qty,Chassis_No,Engine_No,SGST,CGST,CESS,SGST_Per,CGST_Per,CESS_Per,Total_Amount " &
                        ",Doc_Date as Document_Da from purchasevehicle where 1=1 "
            cmd.CommandText = strQuery
            Da.SelectCommand = cmd
            Da.Fill(Ds, "PurchaseVehicle_Detail")


            strQuery = " SELECT distinct `Description`,`Invoice_No` from purchasevehicle  where 1=1 " &
                       " group by Description,Invoice_No "
            cmd.CommandText = strQuery
            Da.SelectCommand = cmd
            Da.Fill(Ds, "Invoice_no")

            cn.Close()
            cn = Nothing

        Catch ex As Exception
            cn.Dispose()
            cn = Nothing
        End Try

        Return Ds

    End Function



    Public Shared Function Update_SparePurchase(ByVal Dr As DataRow) As Boolean

        Dim Qry_Update = ""
        Dim Status As Boolean
        Dim invdate As String = ""
        Dim x As Integer = 0
        Dim InvNo As String = ""
        Dim Model As String = ""


        Qry_Update = " update re_spare_purchase set Locked=1 where `GRN Date` = '" & Format(Dr("GRN Date"), "yyyy-MM-dd") & "'" &
            "and `Supplier Invoice No` = '" & Dr("Supplier_Invoice_No") & "' ; "
        Status = RunQuery(Qry_Update)


        Return Status

    End Function


    Public Shared Function Update_Purchase(ByVal Dr As TallyDs.PurchaseVehicleRow) As Boolean

        Dim Qry_Update = ""
        Dim Status As Boolean
        Dim invdate As String = ""
        Dim x As Integer = 0
        Dim InvNo As String = ""
        Dim Model As String = ""


        Qry_Update = " update purchasevehicle set PostedToTally=1 where Invoice_Date = '" & Format(Dr.Invoice_Date, "yyyy-MM-dd") & "'" &
            " and Invoice_No = '" & Dr.Invoice_No & "' ; "

        Status = RunQuery(Qry_Update)

        Return Status

    End Function

    Public Shared Function CustomRounding(ByVal Amount As Decimal, ByVal Round As Integer) As Decimal
        If Amount <> 0 Then
            Dim IntValue As Integer = 0
            Dim DecValue As Decimal = 0

            IntValue = Int(Amount)
            DecValue = Amount - Int(Amount)
            If Right(DecValue, 1) > 5 Then
                DecValue = Math.Round(DecValue, 2)
            Else
                DecValue = Left(DecValue, Round + 2)
            End If
            Amount = IntValue + DecValue
        End If
        Return Amount
    End Function


    Public Shared Function Get_Summary_SparePurchase(ByVal FrmDate As Date, ByVal ToDate As Date) As TallyDs

        Dim cn As New MySqlConnection(ConnectionString)
        Dim cmd As New MySqlCommand
        Dim Da As New MySqlDataAdapter
        Dim strQuery As String = ""
        Dim Ds As New TallyDs
        '  Dim Filter As String = IIf(FrmDate = Nothing, "", "Where `GRN Date` between ")

        Try
            cmd.Connection = cn
            cn.Open()


            strQuery = " select `GRN Date` ,`GRN No`,Invoice_No,`Supplier Invoice No` as Supplier_Invoice_No,locked as to_tally,sum(RATE) as RATE ,Sum(Taxable_amount) as Taxable_amount,sum(IGST) as IGST ,`Supplier Invoice Date` as Supplier_Invoice_Date,sum(`Total Amount`) Total_Amount,sum(Freight) as Freight " &
            " from re_spare_purchase where  `Supplier Invoice Date` <= now() group by `Supplier Invoice No` "

            cmd.CommandText = strQuery
            Da.SelectCommand = cmd
            Da.Fill(Ds, "re_spare_purchase")

            strQuery = " SELECT `GRN Date` ,`GRN No` ,Invoice_No,locked as to_tally,`Supplier Invoice No` as Supplier_Invoice_No,`Supplier Invoice Date` as Supplier_Invoice_Date,Gross_Amt as Rate,(taxable_amount+Freight) as taxable_amount,`total amount` as total_amount, Description,Model,S.qty,UOM,IGST,s.IGST_per,Discount,Discount_per,m.`HSN_code`,m.hsn_desc " &
            " from re_spare_purchase s left join stock_master m on s.Model=m.`Part_no` "

            cmd.CommandText = strQuery
            Da.SelectCommand = cmd
            Da.Fill(Ds, "re_spare_purchase_Detail")

            strQuery = " select Distinct model,model as Part_no,0 as id,'' as HSN_code from re_spare_purchase " &
            " where model not in (select part_no from stock_master) " &
            " union all " &
            " select Distinct Model, cast(concat('Part no: ', p.model,' Arrived:',p.igst_per,' Given: ',s.igst_per) as char) as Part_no , " &
            " 1 as id ,'' as HSN_code from re_spare_purchase p " &
            " inner join stock_master s on s.part_no=p.model where p.igst_per<>s.igst_per AND P.LOCKED=0 " &
            " union all " &
            " SELECT Distinct Model,Model as Part_no,2,m.`HSN_code` " &
            " from re_spare_purchase s left join stock_master m on s.Model=m.`Part_no` where Locked=0 order by id,model "

            cmd.CommandText = strQuery
            Da.SelectCommand = cmd
            Da.Fill(Ds, "stock_master")
            Remove_Null(Ds)
            cn.Close()
            cn = Nothing

        Catch ex As Exception
            MsgBox("Get Data : " & ex.Message & strQuery)
            cn.Dispose()
            cn = Nothing
        End Try

        Return Ds

    End Function

    Public Function Get_GST_Details(ByVal GroupBy_Tally As Boolean, ByVal Acc_Type As String) As TallyDs

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim da As New MySqlDataAdapter
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Objds As New TallyDs
        Dim Id As Integer = 0

        cmd.Connection = cn
        cn.Open()

        strQuery = " SELECT * from gst_details " &
                    " Where 1 = 1 "

        If Acc_Type <> "" Then
            strQuery = strQuery & " And Acc_Type = '" & Acc_Type & "' "
        End If


        strQuery = strQuery & " Order by GST_Per,GST_Name "

        da.SelectCommand = cmd
        cmd.CommandText = strQuery
        da.Fill(Objds, "gst_details")


        strQuery = "select * from service_ledgers order by ledgername"

        da.SelectCommand = cmd
        cmd.CommandText = strQuery
        da.Fill(Objds, "service_ledgers")



        cn.Close()
        cn = Nothing
        Return Objds

    End Function

    Public Function Get_GST_Details(ByVal GroupBy_Tally As String, ByVal Acc_Type As String) As TallyDs

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim da As New MySqlDataAdapter
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Objds As New TallyDs
        Dim Id As Integer = 0

        cmd.Connection = cn
        cn.Open()

        strQuery = " SELECT * from gst_details " &
                    " Where 1 = 1 "

        If Acc_Type <> "" Then
            strQuery = strQuery & " And Acc_Type = '" & Acc_Type & "' "
        End If


        strQuery = strQuery & " Order by GST_Per,GST_Name "

        da.SelectCommand = cmd
        cmd.CommandText = strQuery
        da.Fill(Objds, "gst_details")


        cn.Close()
        cn = Nothing
        Return Objds

    End Function

    Public Shared Function Insert_SparePurchase(ByVal Ds As TallyDs, ByRef LblStat As Label) As Boolean

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim da As New MySqlDataAdapter
        Dim cmd As New MySqlCommand

        Dim Qry = "", Qry_Insert = "", Qry_Delete As String = ""

        Dim Status As Boolean
        Dim invdate As String = ""
        Dim Grn_date As Date
        Dim InvNo As String = ""
        Dim Model As String = ""

        Dim f_bill As Boolean = False
        Dim d_bill As Boolean = False
        Dim TaxOnly As Boolean = True
        Dim Freight_per As Double = 5
        Dim Freight As Double = 0
        Dim Disc_per As Double = 1
        Dim Disc As Double = 0
        Dim IGST As Double = 0
        Dim IGST_per As Double = 0
        Dim taxable As Double = 0
        Dim Given_Tax As Double = 0
        Dim Total_Amount As Double = 0
        Dim rate As Double = 0
        Dim qty As Double = 0
        Dim Gross_amount As Double = 0
        Dim Arrived_tax As Double = 0
        Dim TDS As New TallyDs
        Dim Net_with_Disc As Double = 0
        Dim Insert As Boolean = False
        Dim Delid As Integer = 0
        Dim exist As Boolean = False
        Dim BillType As String = ""
        cmd.Connection = cn
        cn.Open()
        Dim F_per = val(Read_Settings("Freight_spare_per")), D_per As Double = Val(Read_Settings("Disc_spare_per"))

        Qry = " select * FROM gst_details "
        da.SelectCommand = cmd
        cmd.CommandText = Qry
        da.Fill(TDS, "gst_details")



        ' RunQuery("TRUNCATE `re_spare_purchase`")

        ' Dim Total As Integer = Ds.re_spare_purchase.Select("`Supplier Invoice No`<>'' and `Model`<>'' ").Count
        Dim Total As Integer = Ds.re_spare_purchase.Rows.Count


        Try
            Dim y = 100 / Total
            Dim x = 100 / Total
            x -= y
            Dim z As String = ""
            Dim StatusString As String = "...................................................................................................."
            Dim Regex = New Regex(".")


            For Each DrD In Ds.re_spare_purchase.Rows



                x = x + y
                If Math.Round(x, 0) > Math.Round(Val(z), 0) Then
                    '  StatusString += "|"
                    StatusString = Regex.Replace(StatusString, "|", CInt(Val(x)))
                    ' StatusString = Replace(StatusString, ".", "", , 4)
                Else

                End If
                z = (Math.Round(x, 0)).ToString

                LblStat.Text = "Inserting  " + StatusString + "  " + z + "/100"
                ' LblStat.Text = LblStat.Text.Replace("-", "\")
                LblStat.Refresh()



                If (DrD("GRN No") <> "") Then
                    Grn_date = DateSerial((Right(DrD("GRN No"), 4)), (Mid(DrD("GRN No"), 4, 2)), (Left(DrD("GRN No"), 2)))
                End If

                If DrD("Supplier Invoice No") = "42271322" Or DrD("Supplier Invoice No") = "42276300" Then
                    Dim ssss As String = ""
                End If

                If exist = True And InvNo = DrD("Supplier Invoice No") Then
                    x += 1

                    'LblStatus.Text = "Import Status : " & Total & "/" & x
                    'LblStatus.Refresh()

                    Status = True
                    Continue For

                End If

                If InvNo <> DrD("Supplier Invoice No") Then

                    Qry = "Select myid from re_spare_purchase where locked=1 and `GRN Date` = '" & Format(Grn_date, "yyyy-MM-dd") & "' and `Supplier Invoice No` = '" & DrD("Supplier Invoice No") & "' ; "
                    da.SelectCommand = cmd
                    cmd.CommandText = Qry
                    Delid = cmd.ExecuteScalar

                    InvNo = DrD("Supplier Invoice No")
                    TaxOnly = True

                    If Delid > 0 Then
                        exist = True
                        Status = True
                        x += 1

                        'LblStatus.Text = "Import Status : " & Total & "/" & x
                        'LblStatus.Refresh()

                        Continue For
                    Else
                        Qry_Delete = " Delete from re_spare_purchase where `GRN Date` = '" & Format(Grn_date, "yyyy-MM-dd") & "' and `Supplier Invoice No` = '" & DrD("Supplier Invoice No") & "' ; "
                        Status = RunQuery(Qry_Delete)
                        exist = False

                    End If

                End If

                If DrD("Supplier Invoice No") <> "" And DrD("Model") <> "" Then

                    Insert = False
                    taxable = 0
                    Freight = 0
                    Disc = 0
                    IGST = 0
                    IGST_per = 0

                    Given_Tax = 0
                    Total_Amount = 0
                    rate = 0
                    qty = 0
                    Gross_amount = 0
                    Arrived_tax = 0

                    Net_with_Disc = 0

                    rate = Val(DrD("Rate"))
                    qty = Val(DrD("Qty"))
                    Gross_amount = rate * qty
                    Total_Amount = Math.Round(Val(DrD("Total Amount")), 2)
                    Given_Tax = Math.Round(Val(DrD("Tax Amount")), 2)

                    If Given_Tax = 0 Then
                        taxable = Gross_amount
                        Freight = 0
                        Freight_per = 0
                        Disc_per = 0
                        Disc = 0
                        IGST = 0
                        IGST_per = 0
                        Insert = True
                        TaxOnly = False
                        f_bill = False
                        d_bill = False
                    End If

                    If TaxOnly = True Then
                        IGST_per = Math.Round((Given_Tax / Gross_amount) * 100)

                        If TDS.GST_Details.Select("GST_Name = 'IGST' and GST_per = '" & IGST_per & "'").Count > 0 Then
                            IGST = Given_Tax
                            taxable = Gross_amount

                            Disc = 0
                            Disc_per = 0
                            Freight = 0
                            Freight_per = 0

                            Insert = True
                            f_bill = False
                            d_bill = False
                            TaxOnly = True

                        Else

                            d_bill = True
                            f_bill = True
                            TaxOnly = False
                        End If


                    End If

                    If f_bill = True Then
                        Freight_per = F_per
                        Freight = Math.Round((Gross_amount * Freight_per) / 100, 2)
                        taxable = Math.Round(Freight + Gross_amount, 2)
                        IGST_per = Math.Round((Given_Tax / taxable) * 100)

                        Arrived_tax = Math.Round((taxable * IGST_per) / 100, 2)
                        IGST = Arrived_tax


                        If TDS.GST_Details.Select("GST_Name = 'IGST' and GST_per = '" & IGST_per & "'").Count > 0 And Arrived_tax = Given_Tax Then

                            taxable = Gross_amount
                            Total_Amount = Math.Round(taxable + IGST + Freight, 2)

                            Disc = 0
                            Disc_per = 0

                            Insert = True
                            f_bill = True
                            d_bill = False
                            TaxOnly = False

                        Else
                            d_bill = True
                            TaxOnly = True
                            f_bill = False
                        End If

                    End If

                    If d_bill = True Then
                        Disc_per = D_per
                        Net_with_Disc = Math.Round((Total_Amount * 100) / (100 - Disc_per), 2)
                        IGST = Math.Round(Net_with_Disc - Gross_amount, 2)
                        IGST_per = Math.Round((IGST / Gross_amount) * 100)
                        taxable = Gross_amount
                        Disc = Math.Round((Net_with_Disc * Disc_per) / 100, 2)
                        Arrived_tax = Math.Round(IGST - Disc, 2)

                        If TDS.GST_Details.Select("GST_Name = 'IGST' and GST_per = '" & IGST_per & "'").Count > 0 And Arrived_tax = Given_Tax Then

                            Freight = 0
                            Disc = 0
                            taxable = 0
                            Freight_per = 0

                            Disc = Math.Round((Gross_amount * Disc_per) / 100, 2)
                            taxable = Math.Round(Gross_amount - Disc, 2)
                            IGST = Math.Round((taxable * IGST_per) / 100, 2)
                            IGST = Format(Total_Amount - taxable, "0.00")
                            TaxOnly = False
                            f_bill = False
                            d_bill = True
                            Insert = True
                        Else
                            d_bill = False
                            f_bill = True
                            TaxOnly = True
                        End If

                    End If


                    If Insert = False Then
                        IGST_per = Math.Round((Given_Tax / Gross_amount) * 100)
                        taxable = Gross_amount
                        IGST = Given_Tax

                        Disc = 0
                        Disc_per = 0
                        Freight = 0
                        Freight_per = 0
                        TaxOnly = True
                    End If
                    BillType = Strings.Left(DrD("F2"), 3)
                    If BillType = "DRP" Then
                        Continue For
                    End If
                    Dim LrDate As Date = DrD("LR Date")



                    Qry_Insert += " INSERT INTO re_spare_purchase (`GRN Date`,`GRN No`,Company,Branch," &
                    " Supplier,`Supplier Invoice No`,`Supplier Invoice Date`,Model,Description,`Part Catalog`,`Part Category`,UOM,Qty,Rate," &
                    " `Tax Amount`,LineTaxAmount,Remarks,`LR No`,`LR Date`,`Transporter No`,`Road Permit No`,`Header Tax Amount`,`Header Tax Authority`,Company1, " &
                    "branchcontactattachment,IGST_per,IGST,Freight_per,Freight,Discount_per,Discount,Gross_amt,taxable_amount,`Total Amount`) Values (" &
                    "'" & Format(Grn_date, "yyyy-MM-dd") & "','" & DrD("F2") & "','" & DrD("Company") & "', " &
                    "'" & DrD("Branch") & "','" & DrD("Supplier") & "','" & DrD("Supplier Invoice No") & "' ,'" & Format(DrD("Supplier Invoice Date"), "yyyy-MM-dd") & "', " &
                    "'" & DrD("Model") & "','" & CommonDA.ReplaceQuote(DrD("Description")) & "','" & DrD("Part Catalog") & "','" & DrD("Part Category") & "','" & DrD("UOM") & "','" & Val(DrD("Qty")) & "','" & Val(DrD("Rate")) & "'," &
                    "'" & Val(DrD("Tax Amount")) & "','" & Val(DrD("LineTaxAmount")) & "','" & DrD("Remarks") & "','" & DrD("LR No") & "'," &
                    "'" & Format(LrDate, "yyyy-MM-dd") & "','" & DrD("Transporter No") & "','" & DrD("Road Permit No") & "','" & DrD("Header Tax Amount") & "'," &
                    "'" & DrD("Header Tax Authority") & "', '" & DrD("Company1") & "', " &
                    "'" & DrD("branchcontactattachment") & "','" & IGST_per & "','" & IGST & "','" & Freight_per & "','" & Freight & "','" & Disc_per & "', " &
                    "'" & Disc & "'," & Gross_amount & "," & taxable & ",'" & Total_Amount & "'); "



                End If

                ' Status = RunQuery(Qry_Delete + Qry_Insert)


                'LblStatus.Text = "Import Status : " & Total & "/" & x
                'LblStatus.Refresh()

            Next

            Status = RunQuery(Qry_Insert)


            Qry_Insert = " insert into stock_master (part_no,Stock_Name,IGST_PER,MRP) " &
            " select distinct model,`Description`,`IGST_Per`,`Rate` from re_spare_purchase " &
            " where model not in (select part_no from stock_master) "

            Status = RunQuery(Qry_Insert)


            'LblStatus.Text = "Ending ..."
            'LblStatus.Refresh()
        Catch ex As Exception

            cn.Close()
            cn = Nothing
            Return Status

        End Try

        cn.Close()
        cn = Nothing


        Return Status

    End Function

    Public Function Get_vehile(ByVal id As Integer, ByVal Htp As Hashtable) As TallyDs

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim da As New MySqlDataAdapter
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Objds As New TallyDs

        cmd.Connection = cn
        cn.Open()

        strQuery = " SELECT * from veh_master where 1=1 "


        If id > 0 Then
            strQuery = strQuery & " And veh_id = '" & id & "'"
        End If

        If Not Htp Is Nothing Then
            For Each Key As Object In Htp.Keys
                Select Case Key
                    Case TallyDs.SearchVehicleGrpBy.ModelName
                        strQuery = strQuery & " And ModelFamily like '%" & CommonDA.ReplaceQuote(Htp(Key)) & "%'"
                End Select
            Next
        End If

        strQuery = strQuery & " Order by veh_id "

        da.SelectCommand = cmd
        cmd.CommandText = strQuery
        da.Fill(Objds, "veh_master")


        cn.Close()
        cn = Nothing
        Return Objds

    End Function


    Public Function Save_Vehicle(ByVal Id As Integer, ByVal Dr As TallyDs.veh_masterRow) As Boolean

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        '  Dim Dr As TallyDs.veh_masterRow
        Dim VehId As Integer = 0
        Try

            cmd.Connection = cn
            cn.Open()

            'Dr = Ds.veh_master.Rows(0)

            If Id = 0 Then
                strQuery = " Insert into veh_master (ModelFamily,SGST_Per,CGST_Per,CESS_Per,HSN_Code)" &
                            " Values ( " &
                           "'" & CommonDA.ReplaceQuote(Dr.ModelFamily) & "'," &
                           "'" & CommonDA.ReplaceQuote(Dr.SGST_Per) & "'," &
                           "'" & CommonDA.ReplaceQuote(Dr.CGST_Per) & "'," &
                           "'" & CommonDA.ReplaceQuote(Dr.CESS_Per) & "'," &
                           "'" & CommonDA.ReplaceQuote(Dr.HSN_Code) & "');Select LAST_INSERT_ID();"
                cmd.CommandText = strQuery
                VehId = cmd.ExecuteScalar
            Else
                strQuery = " Update veh_master Set " &
                           " ModelFamily='" & CommonDA.ReplaceQuote(Dr.ModelFamily) & "'," &
                           " SGST_Per='" & CommonDA.ReplaceQuote(Dr.SGST_Per) & "'," &
                           " CGST_Per='" & CommonDA.ReplaceQuote(Dr.CGST_Per) & "'," &
                           " CESS_Per='" & CommonDA.ReplaceQuote(Dr.CESS_Per) & "'," &
                           " HSN_Code='" & CommonDA.ReplaceQuote(Dr.HSN_Code) & "'"
                strQuery = strQuery & " Where veh_id = '" & Id & "'"
                cmd.CommandText = strQuery
                cmd.ExecuteNonQuery()
            End If


            cn.Close()
            cn = Nothing

        Catch ex As Exception
            cn.Dispose()
            cn = Nothing
            Return False
        End Try

        Return True

    End Function

    Public Function Delete_Vehicle(ByVal Id As Integer) As Boolean

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim da As New MySqlDataAdapter
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Dt As New DataTable
        Dim Deleted As Boolean = False

        cmd.Connection = cn
        cn.Open()

        Try
            strQuery = " Delete from veh_master where veh_id = '" & Id & "' "
            cmd.CommandText = strQuery
            cmd.ExecuteNonQuery()

            Deleted = True

        Catch ex As Exception
            Deleted = False
        End Try

        cn.Close()
        cn = Nothing
        Return Deleted

    End Function



    Public Function Get_SpareStock(ByVal id As Integer, ByVal Htp As Hashtable) As TallyDs

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim da As New MySqlDataAdapter
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Objds As New TallyDs

        cmd.Connection = cn
        cn.Open()

        strQuery = " SELECT * from Stock_master where 1=1 "


        If id > 0 Then
            strQuery = strQuery & " And id = '" & id & "'"
        End If

        If Not Htp Is Nothing Then
            For Each Key As Object In Htp.Keys
                Select Case Key
                    Case TallyDs.SearchSpareBy.PartName
                        strQuery = strQuery & " And stock_name like '%" & CommonDA.ReplaceQuote(Htp(Key)) & "%'"
                    Case TallyDs.SearchSpareBy.PartCode
                        strQuery = strQuery & " And Part_no like '%" & CommonDA.ReplaceQuote(Htp(Key)) & "%'"

                End Select
            Next
        End If

        strQuery = strQuery & " Order by Stock_Name "

        da.SelectCommand = cmd
        cmd.CommandText = strQuery
        da.Fill(Objds, "Stock_master")


        cn.Close()
        cn = Nothing
        Return Objds

    End Function

    Public Function Save_SpareStock(ByVal Id As Integer, ByVal Dr As TallyDs.Stock_masterRow) As Boolean

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        'Dim Dr As TallyDs.Stock_masterRow
        Dim StockId As Integer = 0
        Try

            cmd.Connection = cn
            cn.Open()

            '  Dr = Ds.Stock_master.Rows(0)

            If Id = 0 Then
                strQuery = " Insert into stock_master (Stock_Name,Part_no,HSN_code,NDP,IGST_Per,MRP)" &
                            " Values ( " &
                           "'" & CommonDA.ReplaceQuote(Dr.Stock_Name) & "'," &
                           "'" & CommonDA.ReplaceQuote(Dr.Part_no) & "'," &
                           "'" & CommonDA.ReplaceQuote(Dr.HSN_code) & "'," &
                           "'" & CommonDA.ReplaceQuote(Dr.NDP) & "'," &
                           "'" & CommonDA.ReplaceQuote(Dr.IGST_Per) & "'," &
                           "'" & CommonDA.ReplaceQuote(Dr.MRP) & "');Select LAST_INSERT_ID();"
                cmd.CommandText = strQuery
                StockId = cmd.ExecuteScalar

            Else
                strQuery = " Update stock_master Set " &
                           " Stock_Name='" & CommonDA.ReplaceQuote(Dr.Stock_Name) & "'," &
                           " Part_no='" & CommonDA.ReplaceQuote(Dr.Part_no) & "'," &
                           " HSN_code='" & CommonDA.ReplaceQuote(Dr.HSN_code) & "'," &
                           " NDP='" & CommonDA.ReplaceQuote(Dr.NDP) & "'," &
                           " IGST_Per='" & CommonDA.ReplaceQuote(Dr.IGST_Per) & "'," &
                           " MRP='" & CommonDA.ReplaceQuote(Dr.MRP) & "'"
                strQuery = strQuery & " Where id = '" & Id & "'"
                cmd.CommandText = strQuery
                cmd.ExecuteNonQuery()
            End If


            cn.Close()
            cn = Nothing

        Catch ex As Exception
            cn.Dispose()
            cn = Nothing
            Return False
        End Try

        Return True

    End Function


    Public Function Delete_SpareStock(ByVal Id As Integer) As Boolean

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim da As New MySqlDataAdapter
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Dt As New DataTable
        Dim Deleted As Boolean = False

        cmd.Connection = cn
        cn.Open()

        Try
            strQuery = " Delete from Stock_master where id = '" & Id & "' "
            cmd.CommandText = strQuery
            cmd.ExecuteNonQuery()

            Deleted = True

        Catch ex As Exception
            Deleted = False
        End Try

        cn.Close()
        cn = Nothing
        Return Deleted

    End Function



End Class
