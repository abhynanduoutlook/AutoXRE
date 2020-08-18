Imports System.Drawing.Drawing2D

Public Class Form1

    Private pColor1 As Color
    Private pColor3 As Color
    Private pColor2 As Color
    Private pValue As Double

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Button1.BackColor = Color.Green
        Button1.Text = "c"
        Timer1.Interval = 350
        Timer1.Start()
        Button1.Width = 0

    End Sub


    'Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
    '    Dim StringB As String = Button1.Text
    '    Button1.Text = ""

    '    Timer1.Interval = 350
    '    Timer1.Start()

    'End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'Timer1.Stop()
        'loading()
        'Timer2.Interval = 350
        'Timer2.Start()
        If Panel1.Width >= Button1.Width Then
            Button1.Width = Button1.Width + 10

        End If


    End Sub

    'Public Sub loading1()

    '    Button1.Text = Button1.Text.Replace("*", "\\")

    'End Sub
    'Public Sub loading()
    '    Button1.Text = Button1.Text + "*"
    '    Button1.Refresh()

    'End Sub
    'Public Sub loading2()

    '    Button1.Text = Button1.Text.Replace("\\", "//")
    '    Button1.Refresh()

    'End Sub
    'Public Sub loading3()

    '    Button1.Text = Button1.Text.Replace("//", ">")
    '    Button1.Refresh()

    'End Sub
    'Public Sub loading4()


    '    Button1.Text = Button1.Text.Replace(">", "=")
    '    Button1.Refresh()
    'End Sub

    'Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
    '    Timer2.Stop()
    '    loading1()
    '    Timer3.Interval = 350
    '    Timer3.Start()
    'End Sub

    'Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
    '    Timer3.Stop()
    '    loading2()
    '    Timer4.Interval = 350
    '    Timer4.Start()
    'End Sub

    'Private Sub Timer4_Tick(sender As Object, e As EventArgs) Handles Timer4.Tick
    '    Timer4.Stop()
    '    loading3()
    '    Timer5.Interval = 350
    '    Timer5.Start()
    'End Sub

    'Private Sub Timer5_Tick(sender As Object, e As EventArgs) Handles Timer5.Tick
    '    Timer5.Stop()
    '    loading4()
    '    Timer1.Start()
    'End Sub
End Class
