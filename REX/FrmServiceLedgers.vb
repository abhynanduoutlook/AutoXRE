﻿
Public Class FrmServiceLedgers

    Dim Ds As New TallyDs
    Dim Dr As TallyDs.Service_LedgersRow
    Dim ObjDA As New CommonDA
    Dim Htp As New Hashtable
    Dim lvItem As New ListViewItem
    Dim m_SortingColumn As ColumnHeader
    Dim Id As Integer = 0
    Dim TempDS As New CompDS
    Dim Is_Ledger As Boolean = False
    'Declare the variables
    Dim drag As Boolean
    Dim mousex As Integer
    Dim mousey As Integer

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

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

    Private Sub Initlz()

        ObjDA = New CommonDA
        Dim ReturnDs As New CompDS

        Create_Header()
        'Load_Type()

        TxtCode.Focus()

    End Sub

    Private Sub frmEmpRegister_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Cursor = Cursors.AppStarting
        Initlz()
        Find()
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub Find()

        Me.Cursor = Cursors.WaitCursor

        ObjDA = New CommonDA
        Ds = New TallyDs



        If TxtValueSearch.Text.Trim <> "" And CmbSearch.Text = "PartNo" Then
            Htp.Add(TallyDs.SearchServLabourSettingsBy.PartNo, TxtValueSearch.Text.Trim)
        ElseIf TxtValueSearch.Text.Trim <> "" And CmbSearch.Text = "PartDescription" Then
            Htp.Add(TallyDs.SearchServLabourSettingsBy.PartDescription, TxtValueSearch.Text.Trim)
        ElseIf TxtValueSearch.Text.Trim <> "" And CmbSearch.Text = "LedgerName" Then
            Htp.Add(TallyDs.SearchServLabourSettingsBy.LedgerName, TxtValueSearch.Text.Trim)
        End If

        Ds = ObjDA.Get_Serv_Labour_Settings(Htp)

        ListView1.Items.Clear()


        For Each Me.Dr In Ds.Service_Ledgers.Rows

            lvItem = ListView1.Items.Add(Dr.Id)
            lvItem.SubItems.Add(Dr.PartNo)
            lvItem.SubItems.Add(Dr.PartDescription)
            lvItem.SubItems.Add(Dr.LedgerName)
        Next

        Me.Cursor = Cursors.Default

    End Sub

    Private Sub Create_Header()
        ListView1.Columns.Add("Id", 0)
        ListView1.Columns.Add("Part/Labour No", 150)
        ListView1.Columns.Add("Part/Labour Description", 230)
        ListView1.Columns.Add("Ledger Name", 250)
        'ListView1.Columns.Add("Status", 60, HorizontalAlignment.Center)
    End Sub

    Private Sub ListView1_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles ListView1.ColumnClick



        ListView1.Columns(e.Column).ListView.Sort()
        Dim new_sorting_column As ColumnHeader =
        ListView1.Columns(e.Column)

        ' Figure out the new sorting order.
        Dim sort_order As System.Windows.Forms.SortOrder

        If m_SortingColumn Is Nothing Then
            ' New column. Sort ascending.
            sort_order = SortOrder.Ascending
        Else
            ' See if this is the same column.
            If new_sorting_column.Equals(m_SortingColumn) Then
                ' Same column. Switch the sort order.
                If m_SortingColumn.Text.StartsWith("> ") Then
                    sort_order = SortOrder.Descending
                Else
                    sort_order = SortOrder.Ascending
                End If
            Else
                ' New column. Sort ascending.
                sort_order = SortOrder.Ascending
            End If

            ' Remove the old sort indicator.
            m_SortingColumn.Text =
                m_SortingColumn.Text.Substring(2)
        End If

        ' Display the new sort order.
        m_SortingColumn = new_sorting_column
        If sort_order = SortOrder.Ascending Then
            m_SortingColumn.Text = "> " & m_SortingColumn.Text
        Else
            m_SortingColumn.Text = "< " & m_SortingColumn.Text
        End If

        ' Create a comparer.
        ListView1.ListViewItemSorter = New _
        ListViewComparer(e.Column, sort_order)
        ListView1.Sort()

    End Sub

    Private Sub BtnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub

    Private Sub BtnMaximize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnMaximize.Click

        If Me.WindowState = FormWindowState.Maximized Then
            Me.WindowState = FormWindowState.Normal
        Else
            Me.WindowState = FormWindowState.Maximized
        End If

    End Sub

    Private Sub BtnMinimize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnMinimize.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub ListView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView1.DoubleClick
        Edit()
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub Edit()
        Id = 0
        TempDS.Clear()
        If ListView1.SelectedItems.Count = 0 Then
            MsgBox("Please select an item..", MsgBoxStyle.Information, "Information")
            Exit Sub
        End If
        Id = ListView1.SelectedItems(0).Text
        TempDS.Merge(Ds.Settings.Select("Id = '" & Id & "'", ""))
        If Id > 0 Then
            Dr = TempDS.Settings.Rows(0)
            TxtCode.Text = Dr.PartNo
            txtdesc.Text = Dr.PartDescription
            txtLedname.Text = Dr.LedgerName
            TxtCode.Focus()
        End If
    End Sub

    Private Sub BtnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnEdit.Click
        Edit()
    End Sub

    Private Sub BtnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Save()
    End Sub

    Private Function Validation() As Boolean

        If TxtCode.Text.Trim = "" Then
            TxtCode.BackColor = Color.Yellow
            TxtCode.Focus()
            Return False
        Else
            TxtCode.BackColor = Color.WhiteSmoke
        End If

        If txtdesc.Text.Trim = "" Then
            txtdesc.BackColor = Color.Yellow
            txtdesc.Focus()
            Return False
        End If

        TempDS = New CompDS
        TempDS.Merge(Ds.Settings.Select("Id <> '" & Id & "' And S_Key = '" & TxtCode.Text.Trim & "'"))
        If TempDS.Settings.Rows.Count > 0 Then
            TxtCode.BackColor = Color.Yellow
            TxtCode.Focus()
            Return False
        Else
            TxtCode.BackColor = Color.WhiteSmoke
        End If

        TempDS = New CompDS
        TempDS.Merge(Ds.Settings.Select("Id <> '" & Id & "' And S_Value = '" & txtdesc.Text & "'"))
        If TempDS.Settings.Rows.Count > 0 Then
            txtdesc.BackColor = Color.Yellow
            txtdesc.Focus()
            Return False
        Else
            txtdesc.BackColor = Color.WhiteSmoke
        End If

        If Id = 0 Then
            If txtdesc.Text.Trim = "" Then
                txtdesc.BackColor = Color.Yellow
                txtdesc.Focus()
                Return False
            Else
                txtdesc.BackColor = Color.WhiteSmoke
            End If
        End If


        Return True

    End Function

    Private Sub Save()

        If Validation() = False Then
            Exit Sub
        End If

        Me.Cursor = Cursors.WaitCursor

        Dim DsSave As New TallyDs
        Dim Status As Boolean


        ObjDA = New CommonDA

        BtnSave.BackColor = Color.Red
        BtnSave.Refresh()

        Dr = DsSave.Service_Ledgers.Rows.Add

        Dr.Id = Id
        Dr.PartNo = TxtCode.Text.Trim
        Dr.PartDescription = txtdesc.Text.Trim
        Dr.LedgerName = txtLedname.Text.Trim
        DsSave.AcceptChanges()

        Status = ObjDA.Save_Serv_Labour_Settings(Dr)

        If Status Then
            MsgBox("Saved successfully.", MsgBoxStyle.Information, "Information")

            Find()
            Clear_Data()


            'Settings
            Dim DsComp As New TallyDs
            Dim ObjDa As New CommonDA
            DsComp = ObjDa.Get_Serv_Labour_Settings(Htp)


        Else
            MsgBox("Error found.", MsgBoxStyle.Critical, "Error")
        End If

        DsSave = Nothing

        Me.Cursor = Cursors.Default

    End Sub

    Private Sub BtnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Clear_Data()
    End Sub

    Private Sub Clear_Data()
        Id = 0
        TxtCode.Text = ""
        txtdesc.Text = ""
        txtLedname.Text = ""
        'TxtNewPw.Text = ""
        'ChkActive.Checked = True
        TxtCode.Focus()
    End Sub

    Private Sub TxtSearch_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            Find()
        End If
    End Sub

    Private Sub BtnFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnFind.Click
        Find()
    End Sub


    Private Sub TxtNameSearch_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            Find()
        End If
    End Sub

    Private Sub BtnSave_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.LostFocus
        BtnSave.BackColor = Color.WhiteSmoke
    End Sub

    Private Sub BtnPrivileges_Click(sender As System.Object, e As System.EventArgs)

    End Sub

    Private Sub BtnAdd_Click_1(sender As System.Object, e As System.EventArgs) Handles BtnAdd.Click
        Clear_Data()
    End Sub

    Private Sub ListView1_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles ListView1.KeyDown
        If e.KeyCode = Keys.Enter Then
            Edit()
        End If
    End Sub



    'Private Sub Load_Type()

    '    Dim Dt As New DataTable
    '    Dim ObjDA As New CommonDA

    '    Dt.Merge(PublicShared.Forms_Dt.DefaultView.ToTable(True, "UserType"))

    '    CmbType.DataSource = Dt.DefaultView
    '    CmbType.ValueMember = "UserType"
    '    CmbType.DisplayMember = "UserType"

    '    CmbType.AutoCompleteMode = AutoCompleteMode.SuggestAppend
    '    CmbType.AutoCompleteSource = AutoCompleteSource.ListItems

    '    CmbType.SelectedIndex = -1

    'End Sub

    Private Sub BtnPw_Click(sender As System.Object, e As System.EventArgs)
        'Update_Pw()
    End Sub

    'Private Sub Update_Pw()
    '    TempDS = New UserDS
    '    TempDS.Merge(Ds.Users.Select("UserId = '" & UserId & "'"))
    '    If TempDS.Users.Rows.Count > 0 Then
    '        Dr = TempDS.Users.Rows(0)
    '        If TxtOldPw.Text <> Dr.U_Pw Then
    '            TxtOldPw.BackColor = Color.Yellow
    '            TxtOldPw.Focus()
    '            Exit Sub
    '        End If
    '    End If
    '    ObjDA = New UserDA
    '    If ObjDA.Update_Pw(UserId, TxtNewPw.Text.Trim) Then
    '        MsgBox("Updated successfully", MsgBoxStyle.Information, "CSMS")
    '        Clear_Data()
    '        'Find()
    '    Else
    '        MsgBox("Error found.", MsgBoxStyle.Critical, "Error")
    '    End If
    'End Sub

    'Private Sub LblType_Click(sender As System.Object, e As System.EventArgs) Handles LblType.Click

    'End Sub




    Private Sub Delete_Click(sender As System.Object, e As System.EventArgs) Handles Delete.Click
        If ListView1.SelectedItems.Count = 0 Then
            Exit Sub
        End If
        If MsgBox("Are you sure want delete this entry?", MsgBoxStyle.YesNo, "Delete") = MsgBoxResult.No Then
            Exit Sub
        End If
        Dim Status As Boolean
        ' ObjDA = New VehDA

        Status = ObjDA.Delete_Serv_Labour_Settings(Val(ListView1.SelectedItems(0).Text))

        If Status Then
            MsgBox("Delete successfully..", MsgBoxStyle.Information, "Deleted")
            Find()
            Clear_Data()

        Else
            MsgBox("Unable to delete the selected item..", MsgBoxStyle.Information, "Information")
        End If
    End Sub




End Class