Imports AutoXDS
Imports MySql.Data.MySqlClient

Public Class BkDA


    Public Function Get_Bank_Insur_Hypt() As BkDS

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Da As New MySqlDataAdapter
        Dim Ds As New BkDS


        Try

            cmd.Connection = cn
            cn.Open()

            strQuery = " SELECT DISTINCT Bank from pay_details where Payment_Mode <> 'Cash' Order by Bank"
            cmd.CommandText = strQuery
            Da.SelectCommand = cmd
            Da.Fill(Ds, "pay_details")

            strQuery = " SELECT DISTINCT Bank from Cash_Register where Payment_Mode <> 'Cash' Order by Bank"
            cmd.CommandText = strQuery
            Da.SelectCommand = cmd
            Da.Fill(Ds, "Cash_Register")

            strQuery = " Select * from (SELECT DISTINCT Hypt_To from booking_header where ifnull(Hypt_To,'') <> ''" & _
                        " union SELECT DISTINCT Fin_Company from bk_fin  where ifnull(Fin_Company,'') <> ''" & _
                        " ) A Order by Hypt_To "
            cmd.CommandText = strQuery
            Da.SelectCommand = cmd
            Da.Fill(Ds, "Hypt_To")

            strQuery = " SELECT DISTINCT Insur_Company from booking_header where ifnull(Insur_Company,'') <> '' Order by Insur_Company"
            cmd.CommandText = strQuery
            Da.SelectCommand = cmd
            Da.Fill(Ds, "Insur_Company")

            strQuery = " SELECT Id,po,district,concat(pinno,'-',po) as pinno from pin_master where active = 1 Order by district,po,pinno"
            cmd.CommandText = strQuery
            Da.SelectCommand = cmd
            Da.Fill(Ds, "Pin_Master")

            strQuery = " SELECT DISTINCT RTO from booking_header where ifnull(RTO,'') <> '' Order by RTO "
            cmd.CommandText = strQuery
            Da.SelectCommand = cmd
            Da.Fill(Ds, "RTO")

            strQuery = " SELECT * from dealers Order by Cust_Type,Cust_Name "
            cmd.CommandText = strQuery
            Da.SelectCommand = cmd
            Da.Fill(Ds, "Dealers")


            cn.Close()
            cn = Nothing

        Catch ex As Exception
            cn.Dispose()
            cn = Nothing
        End Try

        Return Ds

    End Function

    Public Function Get_Rcpt_Bank() As BkDS

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Da As New MySqlDataAdapter
        Dim Ds As New BkDS


        Try

            cmd.Connection = cn
            cn.Open()

            strQuery = " SELECT DISTINCT Bank from pay_details where Payment_Mode <> 'Cash' Order by Bank"
            cmd.CommandText = strQuery
            Da.SelectCommand = cmd
            Da.Fill(Ds, "pay_details")

            cn.Close()
            cn = Nothing

        Catch ex As Exception
            cn.Dispose()
            cn = Nothing
        End Try

        Return Ds

    End Function

    Public Function Get_Hypt_To() As BkDS

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Da As New MySqlDataAdapter
        Dim Ds As New BkDS

        Try

            cmd.Connection = cn
            cn.Open()

            strQuery = " SELECT DISTINCT Hypt_To from booking_header where ifnull(Hypt_To,'') <> '' Order by Hypt_To"
            cmd.CommandText = strQuery
            Da.SelectCommand = cmd
            Da.Fill(Ds, "Hypt_To")

            cn.Close()
            cn = Nothing

        Catch ex As Exception
            cn.Dispose()
            cn = Nothing
        End Try

        Return Ds

    End Function

    Public Function Get_Insur_Company() As BkDS

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Da As New MySqlDataAdapter
        Dim Ds As New BkDS


        Try

            cmd.Connection = cn
            cn.Open()

            strQuery = " SELECT DISTINCT Insur_Company from booking_header where ifnull(Insur_Company,'') <> '' Order by Insur_Company"
            cmd.CommandText = strQuery
            Da.SelectCommand = cmd
            Da.Fill(Ds, "Insur_Company")

            cn.Close()
            cn = Nothing

        Catch ex As Exception
            cn.Dispose()
            cn = Nothing
        End Try

        Return Ds

    End Function


    Public Function Get_Imported_CashRegister(ByVal htp As Hashtable)
        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim da As New MySqlDataAdapter
        Dim cmd As New MySqlCommand
        Dim strQuery As String = ""
        Dim Objds As New BkDS

        Try

            cmd.Connection = cn
            cn.Open()
            strQuery = "select Id,Entry_No,Entry_Type,Entry_Date,Party,Payment_Mode,sum(Amount) as Amount,Prefix_RefNo,Chassis_No,Ref_No,Entry_Head,Entry_sub_head,Remarks," &
                " EntryHead_Id,EntrySubHead_Id,Branch_Name,PostedToTally,JobCardNo,Chassis_No from cash_register a left join branch b on a.Branch_Id=b.BranchId where Payment_Mode ='' "

            'If FromDate <> Nothing And ToDate <> Nothing Then
            '    strQuery += " and date(Entry_Date) between'" & Format(FromDate.Date, "yyyy-MM-dd") & "' And '" & Format(ToDate.Date, "yyyy-MM-dd") & "'"
            'End If

            If Not htp Is Nothing Then
                For Each key As Object In htp.Keys

                    Select Case key
                        Case BkDS.searchcashRegisterBy.User_Id
                            strQuery = strQuery & " And User_Id ='" & htp(key).ToString & "'"
                        Case BkDS.searchcashRegisterBy.Entry_No
                            strQuery = strQuery & " And Entry_No like '%" & htp(key).ToString & "%'"
                        Case BkDS.searchcashRegisterBy.Entry_Type
                            strQuery = strQuery & " And Entry_Type ='" & htp(key).ToString & "'"
                            'Case BkDS.searchcashRegisterBy.Receipt
                            '    strQuery = strQuery & " And Entry_Type ='" & htp(key).ToString & "'"
                        Case BkDS.searchcashRegisterBy.Party
                            strQuery = strQuery & " And Party like '%" & htp(key).ToString & "%'"
                            'Case BkDS.searchcashRegisterBy.Amount
                            '    strQuery = strQuery & " And Amount like '%" & htp(key).ToString & "%'"
                        Case BkDS.searchcashRegisterBy.Branch
                            strQuery = strQuery & " And Branch_Id ='" & htp(key).ToString & "'"
                        Case BkDS.searchcashRegisterBy.Entry_Head
                            strQuery = strQuery & " And Entry_Head like '%" & htp(key).ToString & "%'"
                        Case BkDS.searchcashRegisterBy.Payment_Mode
                            strQuery = strQuery & " And Payment_Mode like '%" & htp(key).ToString & "%'"
                        Case BkDS.searchcashRegisterBy.JobCardNo
                            strQuery = strQuery & " And JobCardNo like '%" & htp(key).ToString & "%'"
                        Case BkDS.searchcashRegisterBy.Entry_sub_head
                            strQuery = strQuery & " And Entry_Sub_Head like '%" & htp(key).ToString & "%'"


                    End Select

                Next

            End If

            strQuery += "Group by JobCardNo "
            strQuery += " order by Entry_Date,Entry_No,Id "
            cmd.CommandText = strQuery

            da.SelectCommand = cmd
            da.Fill(Objds, "Cash_Register")
            Objds = CommonDA.Remove_Null(Objds, False)

        Catch ex As Exception
            cn.Dispose()
            cn = Nothing

        End Try
        Return Objds

    End Function

    Public Function Get_cash_Register(ByVal id As Integer, ByVal FromDate As Date, ByVal ToDate As DateTime, ByVal htp As Hashtable)
        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim da As New MySqlDataAdapter
        Dim cmd As New MySqlCommand
        Dim strQuery As String = ""
        Dim Objds As New BkDS

        Try

            cmd.Connection = cn
            cn.Open()
            strQuery = "select Id,Entry_No,Entry_Type,Entry_Date,Party,Payment_Mode,Amount,Prefix_RefNo,Ref_No,Entry_Head,Entry_sub_head,Remarks," &
                " EntryHead_Id,EntrySubHead_Id,Branch_Name,PostedToTally,JobCardNo from cash_register a left join branch b on a.Branch_Id=b.BranchId where 1=1 "

            If FromDate <> Nothing And ToDate <> Nothing Then
                strQuery += " and date(Entry_Date) between'" & Format(FromDate.Date, "yyyy-MM-dd") & "' And '" & Format(ToDate.Date, "yyyy-MM-dd") & "'"

            End If

            If Not htp Is Nothing Then
                For Each key As Object In htp.Keys

                    Select Case key
                        Case BkDS.searchcashRegisterBy.User_Id
                            strQuery = strQuery & " And User_Id ='" & htp(key).ToString & "'"
                        Case BkDS.searchcashRegisterBy.Entry_No
                            strQuery = strQuery & " And Entry_No like '%" & htp(key).ToString & "%'"
                        Case BkDS.searchcashRegisterBy.Entry_Type
                            strQuery = strQuery & " And Entry_Type ='" & htp(key).ToString & "'"
                            'Case BkDS.searchcashRegisterBy.Receipt
                            '    strQuery = strQuery & " And Entry_Type ='" & htp(key).ToString & "'"
                        Case BkDS.searchcashRegisterBy.Party
                            strQuery = strQuery & " And Party like '%" & htp(key).ToString & "%'"
                            'Case BkDS.searchcashRegisterBy.Amount
                            '    strQuery = strQuery & " And Amount like '%" & htp(key).ToString & "%'"
                        Case BkDS.searchcashRegisterBy.Ref_no
                            strQuery = strQuery & " And Ref_no like '%'" & htp(key).ToString & "'"
                        Case BkDS.searchcashRegisterBy.Entry_Head
                            strQuery = strQuery & " And Entry_Head like '%" & htp(key).ToString & "%'"
                        Case BkDS.searchcashRegisterBy.Payment_Mode
                            strQuery = strQuery & " And Payment_Mode like '%" & htp(key).ToString & "%'"
                        Case BkDS.searchcashRegisterBy.JobCardNo
                            strQuery = strQuery & " And JobCardNo like '%" & htp(key).ToString & "%'"
                        Case BkDS.searchcashRegisterBy.Entry_sub_head
                            strQuery = strQuery & " And Entry_Sub_Head like '%" & htp(key).ToString & "%'"

                    End Select

                Next

            End If

            strQuery += " order by Entry_Date,Entry_No,Id,Ref_No "
            cmd.CommandText = strQuery

            da.SelectCommand = cmd
            da.Fill(Objds, "Cash_Register")
            Objds = CommonDA.Remove_Null(Objds, False)

        Catch ex As Exception
            cn.Dispose()
            cn = Nothing

        End Try
        Return Objds

    End Function

    Public Function Get_cash_RegisterExcelDs(ByVal FromDate As Date, ByVal ToDate As DateTime)
        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim da As New MySqlDataAdapter
        Dim cmd As New MySqlCommand
        Dim strQuery As String = ""
        Dim Objds As New BkDS

        Try

            cmd.Connection = cn
            cn.Open()
            strQuery = "select * from cash_Register where 1=1 "

            If FromDate <> Nothing And ToDate <> Nothing Then
                strQuery += " and date(Entry_Date) between'" & Format(FromDate.Date, "yyyy-MM-dd") & "' And '" & Format(ToDate.Date, "yyyy-MM-dd") & "'"

            End If

            'If Not htp Is Nothing Then
            '    For Each key As Object In htp.Keys

            '        Select Case key
            '            Case BkDS.searchcashRegisterBy.User_Id
            '                strQuery = strQuery & " And User_Id ='" & htp(key).ToString & "'"
            '            Case BkDS.searchcashRegisterBy.Entry_No
            '                strQuery = strQuery & " And Entry_No like '%" & htp(key).ToString & "%'"
            '            Case BkDS.searchcashRegisterBy.Entry_Type
            '                strQuery = strQuery & " And Entry_Type ='" & htp(key).ToString & "'"
            '                'Case BkDS.searchcashRegisterBy.Receipt
            '                '    strQuery = strQuery & " And Entry_Type ='" & htp(key).ToString & "'"
            '            Case BkDS.searchcashRegisterBy.Party
            '                strQuery = strQuery & " And Party like '%" & htp(key).ToString & "%'"
            '                'Case BkDS.searchcashRegisterBy.Amount
            '                '    strQuery = strQuery & " And Amount like '%" & htp(key).ToString & "%'"
            '            Case BkDS.searchcashRegisterBy.Ref_no
            '                strQuery = strQuery & " And Ref_no like '%" & htp(key).ToString & "'"
            '            Case BkDS.searchcashRegisterBy.Entry_Head
            '                strQuery = strQuery & " And Entry_Head like '%" & htp(key).ToString & "%'"
            '            Case BkDS.searchcashRegisterBy.Payment_Mode
            '                strQuery = strQuery & " And Payment_Mode like '%" & htp(key).ToString & "%'"
            '            Case BkDS.searchcashRegisterBy.JobCardNo
            '                strQuery = strQuery & " And JobCardNo like '%" & htp(key).ToString & "%'"
            '            Case BkDS.searchcashRegisterBy.Entry_sub_head
            '                strQuery = strQuery & " And Entry_Sub_Head like '%" & htp(key).ToString & "%'"

            '        End Select

            '    Next

            'End If

            'strQuery += "Group by JobCardNo "
            'strQuery += " order by Entry_Date,Entry_No,Id,Ref_No "



            cmd.CommandText = strQuery
            da.SelectCommand = cmd
            da.Fill(Objds, "Cash_Register")
            Objds = CommonDA.Remove_Null(Objds, False)

        Catch ex As Exception
            cn.Dispose()
            cn = Nothing

        End Try
        Return Objds

    End Function

    Public Function Get_Opening_cash(ByVal FromDate As Date, ByVal BranchId As Integer) As Decimal

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim da As New MySqlDataAdapter
        Dim cmd As New MySqlCommand
        Dim strQuery As String = ""
        Dim Objds As New BkDS
        Dim openings As Decimal = 0.0
        Dim Receipt As Decimal = 0
        Dim Payment As Decimal = 0

        Try

            cmd.Connection = cn
            cn.Open()
            If FromDate <> Nothing Then

                strQuery = "SELECT (ifnull(Receipt,0)-ifnull(payment,0)) as balance from(select sum(amount) as Receipt from cash_register " & _
                            " where entry_Type='Receipt' and date(entry_Date) < '" & Format(FromDate.Date, "yyyy-MM-dd ") & "' "
                If BranchId > 0 Then strQuery = strQuery & " and Branch_Id=" & BranchId & " "
                strQuery = strQuery & ")a " & _
                   " Left Join (select SUM(amount)as payment from cash_register where Entry_Type='Payment' " & _
                   "and Date(entry_Date)<'" & Format(FromDate.Date, "yyyy-MM-dd ") & "' "
                If BranchId > 0 Then strQuery = strQuery & " and Branch_Id=" & BranchId & " "
                strQuery = strQuery & ")b on 1=1 "

                cmd.CommandText = strQuery
                openings = cmd.ExecuteScalar

            End If

        Catch ex As Exception
            cn.Dispose()
            cn = Nothing

        End Try
        Return openings

    End Function

    Public Function Cash_Register(ByVal Id As Integer, ByVal dr As BkDS.Cash_RegisterRow, ByVal Prefix As String, ByVal IsReceipt As Boolean, ByVal IsCredit_Sales As Boolean) As Hashtable

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Duplicat_Count As Integer = 0
        Dim Htp As New Hashtable
        Dim PayAmt As Decimal = 0
        Dim da As New MySqlDataAdapter

        Dim Tr As MySqlTransaction


        cmd.Connection = cn
        cn.Open()

        Tr = cn.BeginTransaction(IsolationLevel.ReadCommitted)
        cmd.Transaction = Tr

        Try


            If Id = 0 Then

                If IsReceipt = True Or IsCredit_Sales = True Then

                    PayAmt = dr.Amount

                    If IsCredit_Sales = True And dr.Amount > 0 Then
                        PayAmt = -dr.Amount
                    End If



                    strQuery = " Insert into pay_details (BkId,PayDate,Payment_Mode,Bank,Narration,Pay_Amt,Recpt_Type,UpdatedOn)" &
                          " Values ( " &
                          "'" & dr.BkId & "'," &
                          "'" & Format(dr.Entry_Date, "yyyy-MM-dd") & "'," &
                          "'" & dr.Payment_Mode & "'," &
                          "'" & dr.Bank & "'," &
                          "'" & dr.Remarks & "', " &
                          "'" & PayAmt & "'," &
                          "'" & IIf(IsCredit_Sales = True, dr.Entry_Head, dr.Entry_sub_head) & "'," &
                          "'" & Format(Date.Now, "yyyy-MM-dd HH:mm:ss") & "'" &
                          " );Select LAST_INSERT_ID();"
                    cmd.CommandText = strQuery
                    dr.PayId = cmd.ExecuteScalar
                End If


                dr.Entry_No = Get_Next_Entry_No(dr.Entry_Type, Prefix)

                strQuery = " Insert into cash_register (Entry_No,Entry_Type,Entry_Date,Party,Amount,Prefix_RefNo,Ref_No,Entry_Head," &
                        "Entry_sub_head,Remarks,EntryHead_Id,EntrySubHead_Id,Payment_Mode,Bank,JobCardNo,BkId,PayId,Branch_Id," &
                        "User_Id,User_Name,Created_On)Values ( " &
                       "'" & dr.Entry_No & "'," &
                       "'" & dr.Entry_Type & "', " &
                       " '" & Format(dr.Entry_Date, "yyyy-MM-dd HH:mm:ss") & "'," &
                       "'" & CommonDA.ReplaceQuote(dr.Party) & "', " &
                       "'" & dr.Amount & "', " &
                       "'" & dr.Prefix_RefNo & "', " &
                       "'" & dr.Ref_No & "'," &
                       "'" & dr.Entry_Head & "'," &
                       "'" & dr.Entry_sub_head & "'," &
                       "'" & dr.Remarks & "'," &
                       "'" & dr.EntryHead_ID & "'," &
                       "'" & dr.EntrySubHead_Id & "'," &
                       "'" & dr.Payment_Mode & "'," &
                       "'" & dr.Bank & "'," &
                       "'" & dr.JobCardNo & "'," &
                       "'" & dr.BkId & "'," &
                       "'" & dr.PayId & "'," &
                       "'" & dr.Branch_Id & "'," &
                       "'" & dr.User_Id & "'," &
                       "'" & dr.User_Name & "'," &
                       "'" & Format(Date.Now, "yyyy-MM-dd HH:mm:ss") & "'" &
                       ");Select LAST_INSERT_ID();"

                cmd.CommandText = strQuery
                Id = cmd.ExecuteScalar()

            Else

                strQuery = " Update cash_register Set " &
                           " Entry_No = '" & dr.Entry_No & "'," &
                           " Entry_Type = '" & dr.Entry_Type & "'," &
                           " Entry_Date =  '" & Format(dr.Entry_Date, "yyyy-MM-dd HH:mm:ss") & "'," &
                           " Party = '" & CommonDA.ReplaceQuote(dr.Party) & "'," &
                           " Amount = '" & dr.Amount & "'," &
                           " Prefix_RefNo = '" & dr.Prefix_RefNo & "'," &
                           " Ref_No = '" & dr.Ref_No & "'," &
                           " Entry_Head = '" & dr.Entry_Head & "'," &
                           " Entry_sub_head = '" & dr.Entry_sub_head & "'," &
                           " Remarks = '" & dr.Remarks & "'," &
                           " EntryHead_Id='" & dr.EntryHead_ID & "'," &
                           " Payment_Mode='" & dr.Payment_Mode & "'," &
                           " Bank='" & dr.Bank & "'," &
                           " BkId='" & dr.BkId & "'," &
                           " PayId='" & dr.PayId & "'," &
                           " EntrySubHead_Id='" & dr.EntrySubHead_Id & "'," &
                           " User_Id='" & dr.User_Id & "'," &
                           " User_Name='" & dr.User_Name & "'," &
                           " Modified_On='" & Format(Date.Now, "yyyy-MM-dd HH:mm:ss") & "' ," &
                           " JobCardNo = '" & dr.JobCardNo & "'" &
                           " Where Entry_No = '" & dr.Entry_No & "'"

                cmd.CommandText = strQuery
                    cmd.ExecuteNonQuery()
                End If

            If IsReceipt = True Or IsCredit_Sales = True Then

                PayAmt = dr.Amount

                If IsCredit_Sales = True And dr.Amount > 0 Then
                    PayAmt = -dr.Amount
                End If

                strQuery = " Update pay_details Set " &
                     " BkId = '" & dr.BkId & "'," &
                     " Payment_Mode = '" & dr.Payment_Mode & "'," &
                     " Bank = '" & dr.Bank & "'," &
                     " PayDate = '" & Format(dr.Entry_Date, "yyyy-MM-dd") & "'," &
                     " Narration = '" & CommonDA.ReplaceQuote(dr.Remarks) & "'," &
                     " Pay_Amt = '" & PayAmt & "'," &
                     " Recpt_Type = '" & IIf(IsCredit_Sales = True, dr.Entry_Head, dr.Entry_sub_head) & "'," &
                     " UpdatedOn = '" & Format(Date.Now, "yyyy-MM-dd HH:mm:ss") & "'"
                strQuery = strQuery & " Where PayId = '" & dr.PayId & "' and Approved=0"
                cmd.CommandText = strQuery
                cmd.ExecuteNonQuery()
            End If

            Htp.Add("Id", Id)
            Htp.Add("Entry_No", dr.Entry_No)

            Tr.Commit()
            Tr.Dispose()
            cn.Close()
            cn = Nothing

        Catch ex As Exception
            If Not Tr Is Nothing Then
                Tr.Rollback()
            End If
            cn.Dispose()
            cn = Nothing
            Return Nothing
        End Try

        Return Htp

    End Function

    Public Function Get_distinct_entryhead() As BkDS

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Da As New MySqlDataAdapter
        Dim Objds As New BkDS

        Try

            cmd.Connection = cn
            cn.Open()

            strQuery = " SELECT DISTINCT  Entry_Head from cash_register where Entry_Head <> '' "
            cmd.CommandText = strQuery
            Da.SelectCommand = cmd
            Da.Fill(Objds, "cash_Register")

            cn.Close()
            cn = Nothing

        Catch ex As Exception
            cn.Dispose()
            cn = Nothing
        End Try

        Return Objds

    End Function

    Public Function Get_distinct_Entrysubhead(ByVal EntryHead As String) As BkDS

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Da As New MySqlDataAdapter
        Dim Objds As New BkDS

        Try

            cmd.Connection = cn
            cn.Open()
            strQuery = " SELECT DISTINCT  Entry_sub_head from cash_register where Entry_sub_head <> '' "

            If EntryHead <> "" Then
                strQuery = strQuery & " and Entry_head like '" & EntryHead & "' "
            End If

            cmd.CommandText = strQuery
            Da.SelectCommand = cmd
            Da.Fill(Objds, "cash_Register")

            cn.Close()
            cn = Nothing

        Catch ex As Exception
            cn.Dispose()
            cn = Nothing
        End Try

        Return Objds

    End Function

    Public Function Get_Next_Entry_No(ByVal Entry_Type As String, ByVal prefix As String) As String

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim EntNo As String = ""
        Dim Da As New MySqlDataAdapter
        ' Dim Ds As New EnquiryDS

        Try

            cmd.Connection = cn
            cn.Open()

            strQuery = "  select ifnull(Right(Entry_No,Length(Entry_No)-1),0) as Entry_No from cash_register where left(Entry_No,1)='" & prefix & "' order by Entry_no desc"

            cmd.CommandText = strQuery
            EntNo = cmd.ExecuteScalar()

            EntNo = prefix & Format(Val(EntNo) + 1, "00000")

        Catch ex As Exception
            cn.Dispose()
            cn = Nothing
        End Try

        Return EntNo

    End Function

    Public Function Get_cash_Reg(ByVal id As Integer, ByVal FromDate As Date, ByVal ToDate As DateTime)
        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim da As New MySqlDataAdapter
        Dim cmd As New MySqlCommand
        Dim strQuery As String = ""
        Dim Objds As New BkDS

        Try

            cmd.Connection = cn
            cn.Open()

            strQuery = "SELECT COUNT(JobCardNo) FROM cash_register WHERE JobCardNo=(select JobCardNo from cash_register where ID ='" & id & "')"
            cmd.CommandText = strQuery
            If cmd.ExecuteScalar > 1 Then
                strQuery = "select Id,Entry_No,Entry_Type,Entry_Date,Party,sum(Amount) as Amount,jobcardNo as Ref_No,Entry_Head,JobCardNo,Discount,Chassis_No,Entry_sub_head,Remarks,Payment_Mode,Bank,PayId,BkId " &
                                    " from cash_register where JobCardNo=(select JobCardNo from cash_register where ID ='" & id & "')"
            Else
                strQuery = "select Id,Entry_No,Entry_Type,Entry_Date,Party,Amount,Prefix_RefNo,Ref_No,Entry_Head,JobCardNo,Entry_sub_head,Remarks,Payment_Mode,Bank,PayId,BkId " & _
                    " from cash_register where id='" & id & "' "
            End If
            If FromDate <> Nothing And ToDate <> Nothing Then
                strQuery += " and Entry_Date between'" & Format(FromDate, "yyyy-MM-dd HH:mm:ss") & "' And '" & Format(ToDate, "yyyy-MM-dd HH:mm:ss") & "'"

            End If

            strQuery += " order by Entry_Date,Entry_No,Id "
            cmd.CommandText = strQuery

            da.SelectCommand = cmd
            da.Fill(Objds, "Cash_Register")
            CommonDA.Remove_Null(Objds, False)

        Catch ex As Exception
            cn.Dispose()
            cn = Nothing

        End Try
        Return Objds

    End Function

    Public Function Get_Distict_Party() As BkDS

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Da As New MySqlDataAdapter
        Dim Objds As New BkDS

        Try

            cmd.Connection = cn
            cn.Open()

            strQuery = " SELECT DISTINCT  party from cash_register where party <> '' "
            cmd.CommandText = strQuery
            Da.SelectCommand = cmd
            Da.Fill(Objds, "cash_Register")

            cn.Close()
            cn = Nothing

        Catch ex As Exception
            cn.Dispose()
            cn = Nothing
        End Try

        Return Objds

    End Function
    'Public Function Get_SalesRegister(ByVal Id As Integer) As BkDS

    '    Dim cn As New MySqlConnection(CommonDA.ConnectionString)
    '    Dim cmd As New MySqlCommand
    '    Dim strQuery As String
    '    Dim Da As New MySqlDataAdapter
    '    Dim Objds As New BkDS

    '    Try

    '        cmd.Connection = cn
    '        cn.Open()

    '        strQuery = " SELECT if(ifnull(InvoiceNo,'') <> '' ,InvoiceNo,BkNo) as Ref_No,Cust_Name as Party,BkId," & _
    '            "concat(Branch_Name,', ',Veh_Model_Name) as Remarks from booking_header a inner join branch b on b.BranchId=a.Branch " & _
    '            "inner join veh_model v on v.Veh_Model=a.Veh_Model_Id  where BkId=" & Id & " "
    '        cmd.CommandText = strQuery
    '        Da.SelectCommand = cmd
    '        Da.Fill(Objds, "Cash_Register")

    '        cn.Close()
    '        cn = Nothing

    '    Catch ex As Exception
    '        cn.Dispose()
    '        cn = Nothing
    '    End Try

    '    Return Objds

    'End Function

    'Public Function Get_Regigister_Summary(ByVal FromDate As Date, ByVal ToDate As Date, ByVal Htp As Hashtable, ByVal type As String) As BkDS

    '    Dim cn As New MySqlConnection(CommonDA.ConnectionString)
    '    Dim cmd As New MySqlCommand
    '    Dim strQuery As String
    '    Dim Id As Integer = 0
    '    Dim Da As New MySqlDataAdapter
    '    Dim Ds As New BkDS
    '    Dim ToDate1 As DateTime

    '    ToDate1 = New DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 59)
    '    Try

    '        cmd.Connection = cn
    '        cn.Open()

    '        strQuery = "SELECT  Entry_Date,a.Entry_Head,Entry_sub_head,Amount,a.Entry_Type,Branch_Id,Total from cash_register a left  join " &
    '                    "(select sum(Amount) as Total,Entry_Head,Entry_Type from cash_register group by Entry_Head,Entry_Type )b" &
    '                     " On a.Entry_head=b.Entry_Head And a.Entry_Type=b.Entry_Type "

    '        strQuery = strQuery & " where Entry_Date between  " &
    '            "'" & Format(FromDate.Date, "yyyy-MM-dd") & "' and '" & Format(ToDate1, "yyyy-MM-dd HH:mm:ss") & "' "
    '        ' Format(ToDate.Date, "yyyy-MM-dd")

    '        If Not Htp Is Nothing Then
    '            For Each Key As Object In Htp.Keys
    '                Select Case Key
    '                    Case EnquiryDS.SearchEnqBy.Branch
    '                        strQuery = strQuery & " And a.Branch = " & Htp(Key).ToString & ""
    '                    Case EnquiryDS.SearchEnqBy.Team_LeaderId
    '                        strQuery = strQuery & " And a.Team_LeaderId = " & Htp(Key).ToString & ""
    '                    Case EnquiryDS.SearchEnqBy.Branch_Zone
    '                        strQuery = strQuery & " And Sales_Zone = '" & Htp(Key).ToString & "'"
    '                    Case BkDS.SearchBookingBy.Hypt_To
    '                        strQuery = strQuery & " And Hypt_To = '" & Htp(Key).ToString & "'"
    '                    Case EnquiryDS.SearchEnqBy.Sales_Branch
    '                        If Htp(Key).ToString = 1 Then
    '                            strQuery = strQuery & "  And Dealer=0  "
    '                        Else
    '                            strQuery = strQuery & "  And Dealer<>0 "
    '                        End If
    '                End Select
    '            Next
    '        Else

    '        End If

    '        strQuery = strQuery & " "
    '        cmd.CommandText = strQuery
    '        Da.SelectCommand = cmd
    '        Da.Fill(Ds, "cash_register")

    '        cn.Close()
    '        cn = Nothing

    '    Catch ex As Exception
    '        cn.Dispose()
    '        cn = Nothing
    '    End Try

    '    Return Ds

    'End Function


    Public Function Entry_Head(ByVal Id As Integer, ByVal dr As BkDS.Entry_HeadRow, ByVal entryHead As String) As Boolean

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim head As String
        Dim objds As String = " "
        Dim Eno As String
        Dim Head_Id As Integer = 0
        Try

            cmd.Connection = cn
            cn.Open()

            strQuery = "Select EntryHead_Id from entry_heads where EntryHead_Id= '" & Id & "' "
            ' " EntryHead ='" & dr.Entry_Head & "' and Entry_Sub_Head='" & dr.Entry_Sub_Head & "'"
            cmd.CommandText = strQuery
            Eno = cmd.ExecuteScalar

            If Eno = Nothing Then

                strQuery = " Insert into entry_heads (Entry_Head,Entry_sub_head,Entry_Type,Head_Id)" & _
                           " Values ( " & _
                           "'" & dr.Entry_Head & "'," & _
                           "'" & dr.Entry_Sub_Head & "'," & _
                           "'" & dr.Entry_Type & "'"

                If dr.Entry_Sub_Head <> "" Then
                    head = "(select entryhead_Id from entry_heads where entry_Head='" & entryHead & "' )"
                    cmd.CommandText = head
                    Head_Id = cmd.ExecuteScalar
                    strQuery = strQuery & ",'" & Head_Id & "')"
                Else
                    strQuery = strQuery & ",'0')"
                End If
                cmd.CommandText = strQuery
                Id = cmd.ExecuteScalar

            Else
                strQuery = " Update entry_heads Set " & _
                       " Entry_Head = '" & dr.Entry_Head & "'," & _
                       " Entry_sub_head = '" & dr.Entry_Sub_Head & "'," & _
                       " Entry_Type = '" & dr.Entry_Type & "'" & _
                       " Where EntryHead_id = '" & Eno & "'"
                cmd.CommandText = strQuery
                cmd.ExecuteNonQuery()

                strQuery = " Update entry_heads Set " & _
                     " Entry_Head = '" & dr.Entry_Head & "' " & _
                     " Where Head_Id = '" & Eno & "'"
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

    Public Function Entry_Validation(ByVal entryHead As String, ByVal EntrysubHead As String) As Boolean

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Id As Integer
        Try

            cmd.Connection = cn
            cn.Open()

            strQuery = "Select entryhead_Id from entry_heads where Entry_Head ='" & entryHead & "' and Entry_Sub_Head='" & EntrysubHead & "'"
            cmd.CommandText = strQuery
            Id = cmd.ExecuteScalar

            If Id = 0 Then
                Return True
            Else
                Return False
                Exit Function
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
    Public Function Get_Entries(ByVal id As Integer, ByVal htp As Hashtable)
        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim da As New MySqlDataAdapter
        Dim cmd As New MySqlCommand
        Dim strQuery As String = ""
        Dim Objds As New BkDS

        Try

            cmd.Connection = cn
            cn.Open()
            strQuery = "select EntryHead_Id,Entry_Head,Entry_sub_head,Head_Id,Entry_Type from entry_heads where 1=1 "

            If id > 0 Then
                strQuery = strQuery & "and EntryHead_Id='" & id & "'"
            End If

            If Not htp Is Nothing Then
                For Each key As Object In htp.Keys

                    Select Case key
                        Case BkDS.searchEntryHeadBy.Entry_Head
                            strQuery = strQuery & " And Entry_Head like '%" & htp(key).ToString & "%'"
                        Case BkDS.searchEntryHeadBy.Entry_sub_head
                            strQuery = strQuery & " And Entry_Sub_Head like '%" & htp(key).ToString & "%'"
                        Case BkDS.searchEntryHeadBy.Entry_Type
                            strQuery = strQuery & " And Entry_Type like '%" & htp(key).ToString & "%'"
                    End Select

                Next

            End If
            strQuery += " order by Entry_Head,Head_Id "
            cmd.CommandText = strQuery

            da.SelectCommand = cmd
            da.Fill(Objds, "Entry_Head")
            CommonDA.Remove_Null(Objds, False)

        Catch ex As Exception
            cn.Dispose()
            cn = Nothing

        End Try
        Return Objds

    End Function

    Public Function Get_BankName(ByVal Bank As String) As BkDS

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Da As New MySqlDataAdapter
        Dim Objds As New BkDS

        Try

            cmd.Connection = cn
            cn.Open()
            strQuery = " SELECT DISTINCT Bank from Cash_Register where Bank <> '' "

            If Bank <> "" Then
                strQuery = strQuery & " and Bank like '%" & Bank & "%' "
            End If

            strQuery = strQuery & " order by Bank "

            cmd.CommandText = strQuery
            Da.SelectCommand = cmd
            Da.Fill(Objds, "Cash_Register")

            cn.Close()
            cn = Nothing

        Catch ex As Exception
            cn.Dispose()
            cn = Nothing
        End Try

        Return Objds

    End Function

    Public Function Get_entryhead() As BkDS

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Da As New MySqlDataAdapter
        Dim Objds As New BkDS

        Try

            cmd.Connection = cn
            cn.Open()

            strQuery = " SELECT DISTINCT Entry_Head,EntryHead_Id,Entry_Type from entry_heads where Head_Id='0' order by Entry_Head "
            cmd.CommandText = strQuery
            Da.SelectCommand = cmd
            Da.Fill(Objds, "Entry_Head")

            cn.Close()
            cn = Nothing

        Catch ex As Exception
            cn.Dispose()
            cn = Nothing
        End Try

        Return Objds

    End Function

    Public Function Delete_entryheads(ByVal id As Integer) As Boolean

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Da As New MySqlDataAdapter
        Dim Objds As New BkDS

        Try

            cmd.Connection = cn
            cn.Open()

            strQuery = " Delete from entry_heads where EntryHead_Id='" & id & "'"
            cmd.CommandText = strQuery
            Da.SelectCommand = cmd
            Da.Fill(Objds, "Entry_Head")

            cn.Close()
            cn = Nothing

        Catch ex As Exception
            cn.Dispose()
            cn = Nothing
            Return False
        End Try

        Return True

    End Function

    Public Function Get_Entrysubhead(ByVal EntryHead As String) As BkDS

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Da As New MySqlDataAdapter
        Dim Objds As New BkDS

        Try

            cmd.Connection = cn
            cn.Open()
            strQuery = " SELECT DISTINCT  Entry_sub_head,EntryHead_Id,Entry_Type from entry_heads where Entry_sub_head <> '' "

            If EntryHead <> "" Then
                strQuery = strQuery & " and Entry_head like '" & EntryHead & "' "
            End If

            strQuery = strQuery & " order by Entry_sub_head "

            cmd.CommandText = strQuery
            Da.SelectCommand = cmd
            Da.Fill(Objds, "Entry_Head")

            cn.Close()
            cn = Nothing

        Catch ex As Exception
            cn.Dispose()
            cn = Nothing
        End Try

        Return Objds

    End Function

    Public Function Check_Prefix(ByVal RefNo As String, ByVal Prifix As String) As Integer

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Da As New MySqlDataAdapter
        Dim id As Integer

        Try

            cmd.Connection = cn
            cn.Open()
            strQuery = " SELECT Id from cash_register where Prefix_RefNo= '" & Prifix & "' and Ref_No ='" & RefNo & "'"

            cmd.CommandText = strQuery
            id = cmd.ExecuteScalar()


            cn.Close()
            cn = Nothing

        Catch ex As Exception
            cn.Dispose()
            cn = Nothing
        End Try

        Return id

    End Function


    Public Function Receipt_Tally_Imported(ByVal EntryNo As String) As Boolean

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Status As Boolean = False

        cmd.Connection = cn
        cn.Open()

        Try
            strQuery = " update cash_register Set PostedToTally = 1 where Entry_No = '" & EntryNo & "'"
            cmd.CommandText = strQuery
            cmd.ExecuteNonQuery()

            Status = True

            cn.Close()
            cn = Nothing

        Catch ex As Exception
            cn.Dispose()
            cn = Nothing
            Status = False
        End Try


        Return Status

    End Function

    Public Function Import_JobCard_BillSummery(ByVal Ds As DataSet, ByVal dr As BkDS.Cash_RegisterRow, ByVal Entry_Type As String, ByVal prefix As String()) As Boolean

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        Dim strQuery As String = ""
        Dim Tr As MySqlTransaction
        Dim Amount As String = ""
        Dim RefNo As String = ""
        Dim Entry_No As String = ""
        Dim Serviceinv As String = "Service Invoice"
        Dim Service As String = "SERVICE"
        Dim JobCardDt As String = ""
        Dim ServAdv As String = ""
        Dim Status As Boolean = False
        Dim JobCard As String = ""
        Dim Chassis_No As String = ""
        Dim i = 0, Id As Integer = 0
        Dim Dates As Date
        Dim PrefixSub As String = ""
        Dim InvDate As String = ""
        cmd.Connection = cn
        cn.Open()

        'Tr = cn.BeginTransaction(IsolationLevel.ReadCommitted)
        'cmd.Transaction = Tr

        Try
            i = 0

            For Each drb In Ds.Tables("JobCard").Rows

                If drb(0).ToString <> "" Then
                    InvDate = drb(0)
                    Continue For
                End If
                PrefixSub = ""
                Entry_No = ""
                JobCard = ""
                Chassis_No = ""

                Do While Entry_No = Nothing Or Entry_No = ""

                    Entry_No = Get_Next_Entry_No(Entry_Type, "R")
                Loop

                If IsDBNull(drb("F2")) And IsDBNull(drb("Job Card Date")) Then
                    Continue For
                ElseIf drb("F2").ToString = "" Then
                    Continue For
                End If
                RefNo = drb(1)

                If drb("F2") <> "" Then
                    RefNo = drb(1).ToString.Trim

                    For Each item In prefix

                        If RefNo.Contains(item) Then

                            RefNo = RefNo.Replace(item, "")
                            PrefixSub = item
                            Exit For
                        End If

                    Next

                    If PrefixSub = "" Then
                        Continue For
                    End If

                End If


                If (drb("Job Card Date")).ToString <> "" Then
                    JobCardDt = drb(2).ToString.Trim
                    Dates = Convert.ToDateTime(JobCardDt)
                End If

                If drb("Service Advisor") <> "" Then
                    ServAdv = drb(3).ToString.Trim
                End If

                If drb("Job Card") <> "" Then
                    JobCard = drb(5).ToString.Trim
                End If

                If drb("Chassis No") <> "" Then
                    Chassis_No = drb(6).ToString.Trim
                End If

                If drb("Invoice Amount").ToString <> "" Then
                    Amount = drb("Invoice Amount").ToString.Replace(",", "").Trim
                End If



                If JobCard = "" Then
                    JobCard = drb(1).ToString.Trim

                End If

                Try
                    strQuery = "select Id from cash_register where Ref_No='" & RefNo.Trim & "' and JobCardNo = '" & JobCard.Trim & "'"
                    cmd.CommandText = strQuery
                    Id = cmd.ExecuteScalar()
                Catch ex As Exception
                    Id = 0
                End Try


                If Id <> 0 Then


                Else

                    strQuery = " Insert into cash_register(Entry_No,Entry_Type,Entry_Date,Amount,Ref_No,Entry_Head,Entry_Sub_Head," & _
                               " Remarks,BkId,PayId,Branch_Id,User_Id,User_Name,Chassis_No,Created_On,JobCardNo,Prefix_RefNo) "
                    strQuery += " Values ( " & _
                                "'" & Entry_No & "'," & _
                               "'" & Entry_Type & "'," & _
                               "'" & Format(Dates, "yyyy-MM-dd") & "'," & _
                               "'" & Val(Amount.Trim) & "'," & _
                               "'" & RefNo.Trim & "'," & _
                               "'" & Service & "'," & _
                               "'" & Serviceinv & "'," & _
                               "'" & ServAdv.Trim & "'," & _
                               "'" & dr.BkId & "'," & _
                               "'" & dr.PayId & "'," & _
                               "'" & dr.Branch_Id & "'," & _
                               "'" & dr.User_Id & "'," & _
                               "'" & dr.User_Name & "'," & _
                               "'" & Chassis_No & "'," & _
                               "'" & Format(Date.Now, "yyyy-MM-dd HH:mm:ss") & "'," & _
                               "'" & JobCard.Trim & "','" & PrefixSub & "');Select LAST_INSERT_ID();"

                    cmd.CommandText = strQuery
                    cmd.ExecuteNonQuery()
                End If
                i += 1


                'LblStatus.Text = "Process Executed : " & i & "/" & Ds.Tables("Cash_register").Rows.Count
                'LblStatus.Refresh()

            Next


            'LblStatus.Text = "Ending process..."
            'LblStatus.Refresh()

            'Tr.Commit()
            'Tr.Dispose()

            Status = True

            cn.Close()
            cn = Nothing

        Catch ex As Exception
            'If Not Tr Is Nothing Then
            '    Tr.Rollback()
            'End If
            cn.Dispose()
            cn = Nothing
            Status = False
        End Try

        Ds = Nothing

        Return Status

    End Function


    Public Function Get_Service_Bills(ByVal fromDate As Date, ByVal todate As Date) As BkDS
        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Da As New MySqlDataAdapter
        Dim Ds As New BkDS
        Dim Objds As New BkDS

        Try

            cmd.Connection = cn
            cn.Open()

            strQuery = "select Id,Entry_No,Entry_Type,Entry_Date,Party,Payment_Mode,Amount,Prefix_RefNo,Ref_No,Entry_Head,Entry_sub_head,Remarks," &
                       " EntryHead_Id,EntrySubHead_Id,Branch_Name,PostedToTally,JobCardNo from cash_register a left join branch b on a.Branch_Id=b.BranchId where 1=1 "

            If fromDate <> Nothing And todate <> Nothing Then
                strQuery += " and date(Entry_Date) between'" & Format(fromDate.Date, "yyyy-MM-dd") & "' And '" & Format(todate.Date, "yyyy-MM-dd") & "'"
            End If

            '   strQuery += "Group by JobCardNo "
            strQuery += " order by Entry_Date,Entry_No,Id "
            cmd.CommandText = strQuery

            Da.SelectCommand = cmd
            Da.Fill(Objds, "Cash_Register")
            Objds = CommonDA.Remove_Null(Objds, False)


            'strQuery = "select Id,Entry_No,Entry_Type,Entry_Date,Party,Payment_Mode,Amount,Prefix_RefNo,Ref_No,Entry_Head,Entry_sub_head,Remarks, " & _
            '           "EntryHead_Id,EntrySubHead_Id,Branch_Name,PostedToTally,JobCardNo from cash_register a left join branch b on a.Branch_Id=b.BranchId where 1=1  " & _
            '           "and date(Entry_Date) between'" & Format(fromDate.Date, "yyyy-MM-dd") & "' And '" & Format(todate.Date, "yyyy-MM-dd") & "' And Branch_Id ='1' And User_Id ='84' order by Entry_Date,Entry_No,Id "

            'cmd.CommandText = strQuery

            'Da.SelectCommand = cmd
            'Da.Fill(Objds, "Cash_Register_Details")
            'Objds = CommonDA.Remove_Null(Objds, False)


        Catch ex As Exception
            cn.Dispose()
            cn = Nothing
            Return Nothing
        End Try

        Return Objds

    End Function


    Public Function Get_services(ByVal Fromdate As Date, ByVal Todate As Date) As DataSet

        Dim cn As New MySqlConnection(CommonDA.ConnectionString) 'connectionstring change
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Da As New MySqlDataAdapter
        Dim Ds As New DataSet

        Try

            cmd.Connection = cn
            cn.Open()

            strQuery = "SELECT a.id,a.job_Card,a.job_Card_Date,a.invoice_Number,b.Invoice_Date,a.Reg_No,a.Chassis_No,a.Engine_No,a.Customer_Name,b.Invoice_Amount," & _
                       "b.updated from service a INNER JOIN service_header b on a.id = b.id where b.updated = '1' and b.Invoice_Date BETWEEN '" & Fromdate & "' AND '" & Todate & "' GROUP BY job_Card"

            cmd.CommandText = strQuery
            Da.SelectCommand = cmd
            Da.Fill(Ds, "service")

            Ds = CommonDA.Remove_Null(Ds)
            cn.Close()
            cn = Nothing

        Catch ex As Exception
            cn.Dispose()
            cn = Nothing
            Return Nothing
        End Try

        Return Ds

    End Function

    Public Function Import_Services(ByVal Ds As DataSet) As Boolean

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Tr As MySqlTransaction

        Dim Job_Card As String
        Dim Invoice As String
        Dim Invoice_Date As Date
        Dim Job_Card_Date As Date
        Dim Status As Boolean = False
        Dim i As Integer = 0

        cmd.Connection = cn
        cn.Open()

        Tr = cn.BeginTransaction(IsolationLevel.ReadCommitted)
        cmd.Transaction = Tr

        Try

            i = 0

            For Each Dr In Ds.Tables("Service").Rows

                'Job_Card = Dr("job_Card")
                Job_Card_Date = Dr("Job_Card_Date")
                Invoice = Dr("invoice_Number")
                Invoice_Date = Dr("Invoice_Date")

                strQuery = "Delete from service_Bills where BILL_NUM = '" & Dr("invoice_Number") & "'"
                cmd.CommandText = strQuery
                cmd.ExecuteNonQuery()


                strQuery = " Insert into Service_bills (RO_NUM,RO_DATE,BILL_NUM,BILL_DATE,VIN,PARTY_CD,SERV_ADVS,CF_CUSTOMER,GRAND_TOTAL ) " &
                           " Values ( " &
                           "'" & Dr("job_Card") & "'," &
                           "'" & Format(Job_Card_Date, "yyyy-MM-dd") & "'," &
                           "'" & Dr("invoice_Number") & "'," &
                           "'" & Format(Invoice_Date, "yyyy-MM-dd") & "'," &
                           "'" & Dr("Reg_No") & "'," &
                           "'" & Dr("chassis_No") & "'," &
                           "'" & Dr("Engine_No") & "'," &
                           "'" & Dr("Customer_Name") & "'," &
                           "'" & Val(Dr("Invoice_Amount")) & "')"

                cmd.CommandText = strQuery
                cmd.ExecuteNonQuery()

                i += 1
                'LblStatus.Text = "Process Executed : " & i & "/" & Ds.Tables("Service_Bil_Header").Rows.Count
                'LblStatus.Refresh()

            Next


            'LblStatus.Text = "Ending process..."
            'LblStatus.Refresh()

            Tr.Commit()
            Tr.Dispose()

            Status = True

            cn.Close()
            cn = Nothing

        Catch ex As Exception
            If Not Tr Is Nothing Then
                Tr.Rollback()
            End If
            cn.Dispose()
            cn = Nothing
            Status = False
        End Try

        Ds = Nothing

        Return Status

    End Function

    Public Function Update_CashRegister_Party(ByVal Ds As DataSet) As Boolean

        Dim cn As New MySqlConnection(CommonDA.ConnectionString)
        Dim cmd As New MySqlCommand
        Dim strQuery As String
        Dim Tr As MySqlTransaction

        Dim ChassisNo As String = ""
        Dim Customer_Name As String = ""
        Dim Status As Boolean = False
        Dim i As Integer = 0

        cmd.Connection = cn
        cn.Open()

        Tr = cn.BeginTransaction(IsolationLevel.ReadCommitted)
        cmd.Transaction = Tr

        Try

            i = 0

            For Each Dr In Ds.Tables("JobCard").Rows

                ChassisNo = ""
                Customer_Name = ""
                ChassisNo = Dr("Chassis No").ToString.Trim
                Customer_Name = Dr("Customer Name").ToString.Trim

                strQuery = "Update cash_register SET Party = '" & Customer_Name.Trim & "' where  Chassis_No ='" & ChassisNo.Trim & "'"
                cmd.CommandText = strQuery
                cmd.ExecuteNonQuery()


                i += 1
                'LblStatus.Text = "Process Executed : " & i & "/" & Ds.Tables("JobCard").Rows.Count
                'LblStatus.Refresh()

            Next

            'LblStatus.Text = "Ending process..."
            'LblStatus.Refresh()

            Tr.Commit()
            Tr.Dispose()

            Status = True

            cn.Close()
            cn = Nothing

        Catch ex As Exception
            If Not Tr Is Nothing Then
                Tr.Rollback()
            End If
            cn.Dispose()
            cn = Nothing
            Status = False
        End Try

        Ds = Nothing

        Return Status
    End Function

End Class
