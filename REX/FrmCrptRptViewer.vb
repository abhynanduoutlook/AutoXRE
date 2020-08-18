

Public Class FrmCrptRptViewer
    Dim objReport As Object
    Public rds As DataSet
    Dim rptparams As Hashtable
    Dim rptselectionformula As String = ""
    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
    End Sub
    Public Sub New(ByVal objRpt As Object, ByVal ds As DataSet, ByVal Params As Hashtable, Optional ByVal Viewer_Titile As String = "Report")

        InitializeComponent()
        Crv.ShowGroupTreeButton = False

        objReport = objRpt
        rds = ds
        rptparams = Params
        '   rptselectionformula = selectionformula

        Me.Text = Viewer_Titile

    End Sub

    Private Sub FrmCrptRptViewer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim cmd As New SqlClient.SqlCommand()
        Dim myDA As New SqlClient.SqlDataAdapter()
        Dim con As New  _
        CrystalDecisions.Shared.TableLogOnInfo()
        Dim currValue As New CrystalDecisions.Shared.ParameterValues
        Dim paraValue As New CrystalDecisions.Shared.ParameterDiscreteValue

        Try

            If Not rds Is Nothing Then objReport.SetDataSource(rds)
            If Not rptselectionformula.Trim = "" Then objReport.recordselectionformula = rptselectionformula

            Dim key As Object
            If Not rptparams Is Nothing Then
                For Each key In rptparams.Keys
                    paraValue.Value = rptparams(key).ToString
                    currValue.Add(paraValue)
                    objReport.DataDefinition.ParameterFields(key.ToString).ApplyCurrentValues(currValue)
                Next
            End If

            Crv.ReportSource = objReport

        Catch Excep As Exception
            '  MessageBox.Show(Excep.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

End Class