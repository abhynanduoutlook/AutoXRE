Public Class FrmLoading


    Private Sub FrmLoading_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        DoubleBuffered = True
    End Sub

    Public Sub Change_Status(ByVal Msg As String)
        LblMsg.Text = Msg
        LblMsg.Refresh()
    End Sub
End Class