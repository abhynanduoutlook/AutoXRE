Public NotInheritable Class FrmClipBoard

    Public Sub New(ByVal Title As String, ByVal Content As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        LblTitle.Text = Title
        TxtClipBoard.Text = Content
        BtnCopy.Select()

    End Sub
    Public Sub New(ByVal Title As String, ByVal Content As String, ByVal isHide As Boolean)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        LblTitle.Text = Title
        TxtClipBoard.Text = Content
        BtnCopy.Select()
        Me.MinimizeBox = True
        If isHide Then
            BtnHide.Visible = True
        End If

    End Sub

    Private Sub BtnCopy_Click(sender As System.Object, e As System.EventArgs) Handles BtnCopy.Click
        Try
            My.Computer.Clipboard.SetText(TxtClipBoard.Text)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub BtnOK_Click(sender As System.Object, e As System.EventArgs) Handles BtnOK.Click
        Me.Close()
    End Sub

    Private Sub BtnHide_Click(sender As Object, e As EventArgs) Handles BtnHide.Click

        Me.WindowState = FormWindowState.Minimized

    End Sub


    Public Shared Function Read_Settings(ByVal S_Key As String) As String
        Dim Value As String = ""
        Try
            Value = PublicShared.Settings_Dt.Select("S_Key='" & S_Key & "'").First.Item("S_Value").ToString
        Catch ex As Exception
            End Try
            Return Value
            End Function
End Class
