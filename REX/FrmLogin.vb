


Imports System.Configuration

Public Class FrmLogin

    'Dim ObjDA As New UserDA
    'Dim UDs As New UserDS
    'Dim UDsSc As New UserDS

    'Declare the variables
    Dim drag As Boolean
    Dim mousex As Integer
    Dim mousey As Integer

    Dim FrmClose As Boolean = False
    Dim Login_Ok As Boolean = False
    Dim Selected_User As String = ""

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub FrmLogin_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If FrmClose = False Then
            e.Cancel = True
        Else
            If Login_Ok Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
            Else
                Me.DialogResult = Windows.Forms.DialogResult.Cancel
            End If
        End If
    End Sub

    Private Sub FrmLogin_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        DoubleBuffered = True
        Me.Cursor = Cursors.AppStarting
        ' LinkLabelUpdate.Text += "   (V." & FrmMDI.App_Ver & ")"
        TxtUname.Text = "User Name "
        TxtPw.Text = "Password "
        TxtUname.Focus()
        '  Load_All_Users()
        PublicShared.Sys_Mode = ConfigurationManager.AppSettings.Item("Mode").ToString
        TxtUname.Text = PublicShared.Sys_Mode
        Create_List(1)

        '   Create_GeneralUser_Login()

        Me.Cursor = Cursors.Default
        DoubleBuffered = True
    End Sub

    Private Sub Form1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        drag = True 'Sets the variable drag to true.
        mousex = Windows.Forms.Cursor.Position.X - Me.Left 'Sets variable mousex
        mousey = Windows.Forms.Cursor.Position.Y - Me.Top 'Sets variable mousey
    End Sub

    Private Sub Form1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        'If drag is set to true then move the form accordingly.
        If drag Then

            Me.Top = Windows.Forms.Cursor.Position.Y - mousey
            Me.Left = Windows.Forms.Cursor.Position.X - mousex
        End If
    End Sub

    Private Sub Form1_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp
        drag = False 'Sets drag to false, so the form does not move according to the code in MouseMove
    End Sub

    Private Sub TxtUname_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TxtUname.KeyDown
        If e.KeyCode = Keys.Enter And TxtUname.Text <> "User Name " Then
            TxtPw.Focus()
        End If
    End Sub

    Private Sub TxtUname_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TxtUname.KeyPress
        If TxtUname.Text = "User Name " Then
            TxtUname.Text = ""
            TxtUname.ForeColor = Color.Black
        End If
    End Sub

    Private Sub TxtPw_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtPw.GotFocus
        If TxtPw.Text = "Password " Then
            ' TxtPw.Select(0, 0)
            TxtPw.Clear()
            TxtPw.Focus()
        End If
    End Sub

    Private Sub Login()







        Dim LoginDs As New CompDS
        Dim dr As CompDS.UsersRow


        LoginDs.Merge(PublicShared.User_Dt.Select("U_Name = '" & TxtUname.Text & "' And U_Pw = '" & TxtPw.Text & "'"))


        If LoginDs.Users.Rows.Count > 0 Then

            dr = LoginDs.Users.Rows(0)
            PublicShared.User_Type = dr.Type
            Login_Ok = True
            Timer1.Start()

        Else
            MsgBox("Invalid Username or Password", MsgBoxStyle.Information, "Validation")
            TxtUname.Focus()
        End If







    End Sub

    Private Sub TxtPw_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TxtPw.KeyDown
        If e.KeyCode = Keys.Enter Then
            Login()
        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If Me.Opacity <= 0.01 Then
            FrmClose = True
            Timer1.Stop()
            Me.Close()
        Else
            Me.Opacity = Me.Opacity - 0.035
        End If
    End Sub

    Private Sub BtnGo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnGo.Click
        If TxtUname.Text = "" Then
            TxtUname.Focus()
        ElseIf TxtPw.PasswordChar <> "*" Then
            TxtPw.Focus()
        Else

            Login()
        End If
    End Sub

    'Private Sub Load_All_Users()

    '    Dim Htp As New Hashtable

    '    ObjDA = New UserDA
    '    Htp.Add(UserDS.SearchUsersBy.Active, 1)

    '    If PublicShared.Sys_Mode <> "" Then
    '        Htp.Add(UserDS.SearchUsersBy.Type, PublicShared.Sys_Mode)
    '    End If
    '    If PublicShared.Branch_Id > 0 Then
    '        Htp.Add(UserDS.SearchUsersBy.Sc_Branch, PublicShared.Branch_Id)
    '    End If

    '    UDsSc = ObjDA.Get_Sc_Users(0, Htp)
    '    UDs.Merge(PublicShared.User_Dt)

    '    If PublicShared.Sys_Mode <> "" Then
    '        If UDs.Users.Select("Type='" & PublicShared.Sys_Mode & "'").Count > 3 Then
    '            Create_GeneralUser_Login()
    '        Else
    '            Create_List(UDs.Users.Rows.Count)
    '        End If
    '    ElseIf PublicShared.Sys_Mode = "" Then
    '        Create_GeneralUser_Login()
    '    End If

    'End Sub

    'Private Sub Create_GeneralUser_Login()

    '    Dim i As Integer = 0

    '    Dim MyButton As New Button()
    '    'Set the button properties...
    '    With MyButton
    '        .Location = New Point(BtnUser.Location.X, BtnUser.Location.Y - i)
    '        .Size = TxtUname.Size
    '        '.TabIndex = 0
    '        .Name = "Btn" & UCase(0)
    '        .Text = "&" & UCase("LOGIN")
    '        .BackColor = BtnUser.BackColor
    '        .FlatStyle = BtnUser.FlatStyle
    '        .FlatAppearance.BorderSize = BtnUser.FlatAppearance.BorderSize
    '        .FlatAppearance.BorderColor = BtnUser.FlatAppearance.BorderColor
    '        .FlatAppearance.MouseDownBackColor = BtnUser.FlatAppearance.MouseDownBackColor
    '        .FlatAppearance.MouseOverBackColor = BtnUser.FlatAppearance.MouseOverBackColor
    '        .ForeColor = BtnUser.ForeColor
    '        .Font = BtnUser.Font
    '        .Image = BtnUser.Image
    '        .ImageAlign = BtnUser.ImageAlign
    '        .TabIndex = BtnUser.TabIndex

    '        .Tag = "BTGeneral"

    '    End With

    '    i += 60
    '    Controls.Add(MyButton)

    '    AddHandler MyButton.Click, AddressOf MyButton_Click

    'End Sub

    Private Sub Create_List(ByVal Count As Integer)

        Dim i As Integer

        Select Case Count
            Case 1
                i = 0
            Case 2
                i = -30
            Case 3
                i = -60
            Case 4
                i = -90
            Case 5
                i = -120
            Case 6
                i = -150
        End Select


        Dim MyButton As New Button()
            'Set the button properties...
            With MyButton
                .Location = New Point(BtnUser.Location.X, BtnUser.Location.Y - i)
                .Size = TxtUname.Size
            '.TabIndex = 0
            .Name = "Btn" & UCase("User")
            .Text = "&" & UCase(PublicShared.Sys_Mode)
            .BackColor = BtnUser.BackColor
                .FlatStyle = BtnUser.FlatStyle
                .FlatAppearance.BorderSize = BtnUser.FlatAppearance.BorderSize
                .FlatAppearance.BorderColor = BtnUser.FlatAppearance.BorderColor
                .FlatAppearance.MouseDownBackColor = BtnUser.FlatAppearance.MouseDownBackColor
                .FlatAppearance.MouseOverBackColor = BtnUser.FlatAppearance.MouseOverBackColor
                .ForeColor = BtnUser.ForeColor
                .Font = BtnUser.Font
                .Image = BtnUser.Image
                .ImageAlign = BtnUser.ImageAlign
                .TabIndex = BtnUser.TabIndex

                If Count > 3 Then
                    .Tag = "BTGeneral"
                Else
                    .Tag = "BTUname"
                End If

            End With

            i += 60
            Controls.Add(MyButton)

            AddHandler MyButton.Click, AddressOf MyButton_Click




    End Sub

    Private Sub MyButton_Click(ByVal obj As Object, ByVal e As EventArgs)
        For Each Control In Me.Controls
            If TypeOf (Control) Is Button And (Control.Tag = "BTUname" Or Control.Tag = "BTGeneral") And obj.Name <> Control.Name Then
                Control.Visible = False
            ElseIf obj.Name = Control.Name Then
                If Control.tag = "BTGeneral" Then
                    Selected_User = Replace(obj.Name, "Btn", "")
                    Control.visible = False
                    TxtUname.Location = BtnUser.Location
                    BtnGo.Visible = True
                    TxtUname.Visible = True
                    TxtUname.Focus()
                    TxtPw.Visible = True
                    BtnSwitch.Visible = True
                Else
                    Selected_User = Replace(obj.Name, "Btn", "")
                    Control.location = BtnUser.Location
                    BtnGo.Visible = True
                    TxtPw.Visible = True
                    TxtPw.Focus()
                    BtnSwitch.Visible = True
                End If
            End If
        Next
    End Sub

    Private Sub TxtPw_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TxtPw.KeyPress
        If TxtPw.Text = "Password " Then
            TxtPw.Text = ""
        End If
        TxtPw.PasswordChar = "*"
    End Sub

    'Private Sub BtnSwitch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSwitch.Click
    '    BtnGo.Visible = False
    '    TxtUname.Visible = False
    '    TxtPw.Visible = False
    '    TxtPw.Text = ""
    '    BtnSwitch.Visible = False
    '    For I As Integer = 0 To 1 'Bug Fix
    '        For Each Control In Me.Controls
    '            If TypeOf (Control) Is Button And (Strings.Left(Control.Tag, 6) = "BTUname") Then
    '                Control.dispose()
    '            End If
    '        Next
    '    Next
    '    If PublicShared.Sys_Mode <> "" Then
    '        If UDs.Users.Select("Type='" & PublicShared.Sys_Mode & "'").Count > 3 Then
    '            Create_GeneralUser_Login()
    '        Else
    '            Create_List(UDs.Users.Rows.Count)
    '        End If
    '    ElseIf PublicShared.Sys_Mode = "" Then
    '        Create_GeneralUser_Login()
    '    End If
    'End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        FrmClose = True

    End Sub

    Private Sub LinkLabelUpdate_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabelUpdate.LinkClicked
        If MsgBox("Are you sure want to update the software?", MsgBoxStyle.YesNo, "Update") = MsgBoxResult.Yes Then
            Load_Update(True)
        End If
    End Sub

    Private Sub Load_Update(ByVal ShowUptoDate As Boolean)
        Dim Path As String

        Try
            Path = Application.StartupPath & "\UpdateX.exe " & ShowUptoDate
            Microsoft.VisualBasic.Interaction.Shell(Path, AppWinStyle.NormalFocus)
        Catch ex As Exception
            MsgBox("Path error ! UpdateX not found. " & Path, MsgBoxStyle.Critical, "CSMS")
        End Try
    End Sub

End Class
