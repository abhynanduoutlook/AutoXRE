Imports Microsoft.Office.Interop

Public NotInheritable Class FrmWait
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub New(ByVal TableName As String, ByVal FileName As String)

        ' This call is required by the designer.
        InitializeComponent()
        '  Dim Ds As New DataSet
        PublicShared.DSt = Get_Excel_Data(TableName, FileName)
        If PublicShared.DSt.Tables.Count > 0 Then
            DialogResult = DialogResult.OK
            Me.Close()

        End If



    End Sub



    Public Shared Function Get_Excel_Data(ByVal TableName As String, ByVal File As String) As DataSet

        If Not IO.File.Exists(File) Then
            ' MdiChildren'MyMessageBox("File not exists..")
            Return Nothing
            Exit Function
        End If

        Dim status As Boolean = False
        Dim Ds As New TallyDs
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
            PublicShared.DSt = Ds
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

End Class
