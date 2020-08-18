Imports AutoXDS
Public Class PublicShared
    Private Shared FinId As Integer
    Private Shared FinName As String
    Private Shared FinStart As String
    Private Shared FinEnd As String
    Private Shared UserId As Integer
    Private Shared Admin As Integer
    Private Shared UserPw As String
    Private Shared UserName As String
    Private Shared UserType As String
    Private Shared UserBranchId As String
    Private Shared UserENo As String
    Private Shared SysMode As String
    Private Shared Sc As Boolean
    Private Shared Cashier As Boolean
    Private Shared Company_Info As CompDS.CompanyRow
    Private Shared SettingsDt As CompDS.SettingsDataTable
    Private Shared LedgersDt As CompDS.LedgersDataTable
    Private Shared BranchCode As String = "TCR"
    Private Shared BranchId As Integer = 1
    Private Shared BranchDt As CompDS.BranchDataTable
    Private Shared Ds As TallyDs
    Private Shared ServiceFile_Name As String
    Private Shared UserDt As CompDS.UsersDataTable
    Private Shared FormsDt As CompDS.FormsDataTable
    Private Shared Mfd_YearMonthDt As CompDS.Mfd_Year_MonthDataTable
    Private Shared ServerDate As DateTime



    Public Shared Property Server_Date As DateTime
        Get
            Return ServerDate
        End Get
        Set(ByVal value As DateTime)
            ServerDate = value
        End Set
    End Property
  
    Public Shared Property User_Id() As Integer
        Get
            Return UserId
        End Get
        Set(ByVal value As Integer)
            UserId = value
        End Set
    End Property

    Public Shared Property IsAdmin() As Integer
        Get
            Return Admin
        End Get
        Set(ByVal value As Integer)
            Admin = value
        End Set
    End Property

    Public Shared Property User_Type() As String
        Get
            Return UserType
        End Get
        Set(ByVal value As String)
            UserType = value
        End Set
    End Property



    Public Shared Property Fin_Id() As Integer
        Get
            Return FinId
        End Get
        Set(ByVal value As Integer)
            FinId = value
        End Set
    End Property

    Public Shared Property Fin_Name() As String

        Get
            Return FinName
        End Get
        Set(ByVal value As String)
            FinName = value
        End Set
    End Property

    Public Shared Property Fin_Start() As Date

        Get
            Return FinStart
        End Get
        Set(ByVal value As Date)
            FinStart = value
        End Set
    End Property

    Public Shared Property Fin_End() As Date

        Get
            Return FinEnd
        End Get
        Set(ByVal value As Date)
            FinEnd = value
        End Set
    End Property

    Public Shared Property Company_Info_Dr As CompDS.CompanyRow
        Get
            Return Company_Info
        End Get
        Set(ByVal value As CompDS.CompanyRow)
            Company_Info = value
        End Set
    End Property

    Public Shared Property User_Name() As String

        Get
            Return UserName
        End Get
        Set(ByVal value As String)
            UserName = value
        End Set
    End Property

    Public Shared Property User_ENo() As String

        Get
            Return UserENo
        End Get
        Set(ByVal value As String)
            UserENo = value
        End Set

    End Property

    Public Shared Property User_BrachId() As Integer
        Get
            Return UserBranchId
        End Get
        Set(ByVal value As Integer)
            UserBranchId = value
        End Set
    End Property

    Public Shared Property User_PW() As String
        Get
            Return UserPw
        End Get
        Set(ByVal value As String)
            UserPw = value
        End Set
    End Property

    Public Shared Property Sys_Mode() As String

        Get
            Return SysMode
        End Get
        Set(ByVal value As String)
            SysMode = value
        End Set
    End Property

    Public Shared Property Settings_Dt() As CompDS.SettingsDataTable

        Get
            Return SettingsDt
        End Get
        Set(ByVal value As CompDS.SettingsDataTable)
            SettingsDt = value
        End Set
    End Property


    Public Shared Property Ledgers_Dt() As CompDS.LedgersDataTable

        Get
            Return LedgersDt
        End Get
        Set(ByVal value As CompDS.LedgersDataTable)
            LedgersDt = value
        End Set
    End Property

    Public Shared Property Branch_Code() As String

        Get
            Return BranchCode
        End Get
        Set(ByVal value As String)
            BranchCode = value
        End Set
    End Property

    Public Shared Property Branch_Id() As Integer

        Get
            Return BranchId
        End Get
        Set(ByVal value As Integer)
            BranchId = value
        End Set
    End Property

    Public Shared Property Branch_Dt() As CompDS.BranchDataTable
        Get
            Return BranchDt
        End Get
        Set(ByVal value As CompDS.BranchDataTable)
            BranchDt = value
        End Set
    End Property



    Public Shared Property ServiceFileName As String
        Get
            Return ServiceFile_Name
        End Get
        Set(ByVal value As String)
            ServiceFile_Name = value
        End Set
    End Property

    Public Shared Property User_Dt() As CompDS.UsersDataTable
        Get
            Return UserDt
        End Get
        Set(ByVal value As CompDS.UsersDataTable)
            UserDt = value
        End Set
    End Property

    Public Shared Property DSt() As TallyDs
        Get
            Return Ds
        End Get
        Set(ByVal value As TallyDs)
            Ds = value
        End Set
    End Property

    Public Shared Property Forms_Dt() As CompDS.FormsDataTable
        Get
            Return FormsDt
        End Get
        Set(ByVal value As CompDS.FormsDataTable)
            FormsDt = value
        End Set
    End Property


    Public Shared Property Mfd_Year_Month_Dt() As CompDS.Mfd_Year_MonthDataTable
        Get
            Return Mfd_YearMonthDt
        End Get
        Set(ByVal value As CompDS.Mfd_Year_MonthDataTable)
            Mfd_YearMonthDt = value
        End Set
    End Property

End Class
