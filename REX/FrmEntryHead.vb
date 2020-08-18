Imports AutoXDS
Imports AutoXDA

Public Class FrmEntryHead

    Dim ds As New BkDS
    Dim Dr As BkDS.Entry_HeadRow
    Dim ObjDA As New BkDA
    Dim Htp As New Hashtable
    Dim lvItem As New ListViewItem
    Dim m_SortingColumn As ColumnHeader
    Dim Id As Integer = 0

    'Declare the variables
    Dim drag As Boolean
    Dim mousex As Integer
    Dim mousey As Integer

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
        Create_Header()
        load_Entry_Head()
        load_Entry_sub_Head()
        Load_SearchBy()
        Load_Entry_Type()
    End Sub

    Private Sub frmEmpRegister_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Cursor = Cursors.AppStarting
        Initlz()
        Find()
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub Find()

        Me.Cursor = Cursors.WaitCursor

        ObjDA = New BkDA
        ds = New BkDS
        Htp = New Hashtable

        If TxtSearch.Text.Trim <> "" Then
            If CmbSearchBy.Text = "Entry Head" Then
                Htp.Add(BkDS.searchEntryHeadBy.Entry_Head, TxtSearch.Text.Trim)
            ElseIf CmbSearchBy.Text = "Entry Sub Head" Then
                Htp.Add(BkDS.searchEntryHeadBy.Entry_sub_head, TxtSearch.Text.Trim)
            End If
        End If

        ds = ObjDA.Get_Entries(Nothing, Htp)

        ListView1.Items.Clear()

        For Each Me.Dr In ds.Entry_Head.Rows

            lvItem = ListView1.Items.Add(Dr.EntryHead_Id)
            lvItem.SubItems.Add(Dr.Entry_Head)
            lvItem.SubItems.Add(Dr.Entry_Sub_Head)
            lvItem.SubItems.Add(IIf(Dr.Entry_Type = "P", "Payment", "Receipt"))

        Next

        Me.Cursor = Cursors.Default
       

    End Sub

    Private Sub Create_Header()
        ListView1.Columns.Add("Id", 0)
        ListView1.Columns.Add("Entry Head", 190)
        ListView1.Columns.Add("Entry Sub-Head", 180)
        ListView1.Columns.Add("Entry Type", 70)
        'ListView1.Columns.Add("Status", 60, HorizontalAlignment.Center)
    End Sub
    Private Sub Load_SearchBy()

        CmbSearchBy.Items.Add("Entry Head")
        CmbSearchBy.Items.Add("Entry Sub Head")
        CmbSearchBy.SelectedIndex = 0

    End Sub
    Private Sub Load_Entry_Type()
        CmbEntry_Type.Items.Add("Receipt")
        CmbEntry_Type.Items.Add("Payment")
        CmbEntry_Type.SelectedIndex = -1
    End Sub


    Private Sub ListView1_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles ListView1.ColumnClick

        ListView1.Columns(e.Column).ListView.Sort()
        Dim new_sorting_column As ColumnHeader = _
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
            m_SortingColumn.Text = _
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
        ListView1.ListViewItemSorter = New  _
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
        Load_Edit()
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub Edit()
        If Id > 0 Then

            ObjDA = New BkDA
            ds = New BkDS
            ds = ObjDA.Get_Entries(Id, Nothing)


            'load_Entry_Head()

            'load_party()

            If ds.Entry_Head.Rows.Count > 0 Then

                Dr = ds.Entry_Head.Rows(0)
                CmbEntHead.Text = Dr.Entry_Head
                'load_Entry_sub_Head()
                CmbEntSubHead.Text = Dr.Entry_Sub_Head
                CmbEntry_Type.Text = IIf(Dr.Entry_Type = "P", "Payment", "Receipt")

            End If

        Else
            ObjDA = New BkDA
        End If
    End Sub

    Private Sub BtnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnEdit.Click
        Load_Edit()
    End Sub
    Private Sub Load_Edit()

        If ListView1.SelectedItems.Count = 0 Then
            MsgBox("Please select an item..", MsgBoxStyle.Information, "Information")
            Exit Sub
        End If
        Id = (ListView1.SelectedItems(0).Text)
        Edit()
    End Sub

    Private Sub BtnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Save()
    End Sub

    Private Function Validation() As Boolean

        If CmbEntHead.Text.Trim = "" Then
            CmbEntHead.BackColor = Color.Yellow
            CmbEntHead.Focus()
            Return False
        Else
            CmbEntHead.BackColor = Color.WhiteSmoke
        End If


        If CmbEntSubHead.Text.Trim <> "" And CmbEntHead.SelectedValue = 0 Then
            CmbEntHead.BackColor = Color.Yellow
            CmbEntHead.Focus()
            Return False
        Else
            CmbEntHead.BackColor = Color.WhiteSmoke
        End If


        If CmbEntry_Type.Text.Trim = "" Then
            CmbEntry_Type.BackColor = Color.Yellow
            CmbEntry_Type.Focus()
            Return False
        Else
            CmbEntry_Type.BackColor = Color.WhiteSmoke
        End If


        'Entry and SubHead both duplication
        If ds.Entry_Head.Select("EntryHead_Id <> '" & Id & "' And Entry_Head ='" & CmbEntHead.Text & "' and Entry_Sub_Head='" & CmbEntSubHead.Text & "'").Count > 0 Then
            'If ds.Entry_Head.Rows.Count > 0 Then
            ' If ObjDA.Entry_Validation(CmbEntHead.Text, CmbEntSubHead.Text) <> True Then
            CmbEntHead.BackColor = Color.Yellow
            CmbEntSubHead.BackColor = Color.Yellow
            CmbEntHead.Focus()
            Return False
        Else
            CmbEntHead.BackColor = Color.WhiteSmoke
            CmbEntSubHead.BackColor = Color.WhiteSmoke
        End If

        'SubHead in Head
        If ds.Entry_Head.Select("Entry_Head='" & CmbEntSubHead.Text.Trim & "'").Count > 0 And CmbEntSubHead.Text.Trim <> "" Then
            CmbEntSubHead.BackColor = Color.Yellow
            CmbEntSubHead.Focus()
            Return False
        Else
            CmbEntSubHead.BackColor = Color.WhiteSmoke
        End If

        'Head in SubHead
        If ds.Entry_Head.Select("Entry_Sub_Head='" & CmbEntHead.Text.Trim & "'").Count > 0 Then 'And CmbEntSubHead.Text.Trim <> "" 
            CmbEntHead.BackColor = Color.Yellow
            CmbEntHead.Focus()
            Return False
        Else
            CmbEntHead.BackColor = Color.WhiteSmoke
        End If


        If CmbEntHead.Text.Trim = CmbEntSubHead.Text.Trim Then
            CmbEntHead.BackColor = Color.Yellow
            CmbEntHead.Focus()
            Return False
        Else
            CmbEntHead.BackColor = Color.WhiteSmoke
        End If


        If ds.Entry_Head.Select("EntryHead_Id <> '" & Id & "' And Entry_Sub_Head='" & CmbEntSubHead.Text.Trim & "'").Count > 0 And CmbEntSubHead.Text.Trim <> "" Then
            CmbEntSubHead.BackColor = Color.Yellow
            CmbEntSubHead.Focus()
            Return False
        Else
            CmbEntSubHead.BackColor = Color.WhiteSmoke
        End If

        If ds.Entry_Head.Select("Entry_Head='" & CmbEntSubHead.Text.Trim & "'").Count > 0 And CmbEntSubHead.Text.Trim <> "" Then
            CmbEntSubHead.BackColor = Color.Yellow
            CmbEntSubHead.Focus()
            Return False
        Else
            CmbEntSubHead.BackColor = Color.WhiteSmoke
        End If

        Return True

    End Function

    Private Sub Save()

        If Validation() = False Then
            Exit Sub
        End If

        ds.Entry_Head.Rows.Clear()
        Dr = ds.Entry_Head.Rows.Add

        Dr.Entry_Head = CmbEntHead.Text.Trim
        Dr.Entry_Sub_Head = CmbEntSubHead.Text.Trim
        If CmbEntry_Type.Text = "Payment" Then
            Dr.Entry_Type = ("P")
        Else : CmbEntry_Type.Text = "Receipt"
            Dr.Entry_Type = ("R")
        End If


        ObjDA = New BkDA

        If ObjDA.Entry_Head(Id, Dr, Dr.Entry_Head) Then
            MsgBox("Saved successfully..", MsgBoxStyle.Information, "Save")
            Clear_Data()
            Find()
            CmbEntHead.Focus()

            load_Entry_Head()
            load_Entry_sub_Head()
            'Load_Entry_Type()

        Else
            MsgBox("Error found!", MsgBoxStyle.Critical, "Error")
        End If

    End Sub

    Private Sub load_Entry_Head()

        Dim bkds As New BkDS
        Dim objDA As New BkDA

        bkds = objDA.Get_entryhead
        CmbEntHead.DataSource = bkds.Entry_Head.DefaultView
        CmbEntHead.ValueMember = "EntryHead_Id"
        CmbEntHead.DisplayMember = "Entry_Head"

        CmbEntHead.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        CmbEntHead.AutoCompleteSource = AutoCompleteSource.ListItems

        CmbEntHead.SelectedIndex = -1
    End Sub
    Private Sub load_Entry_sub_Head()

        Dim bkds As New BkDS
        Dim objDA As New BkDA
        Dim EntryHead As String = ""

        EntryHead = CmbEntHead.Text

        bkds = objDA.Get_Entrysubhead(EntryHead)

        CmbEntSubHead.DataSource = bkds.Entry_Head.DefaultView
        CmbEntSubHead.ValueMember = "Entry_sub_head"
        CmbEntSubHead.DisplayMember = "Entry_sub_head"

        CmbEntSubHead.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        CmbEntSubHead.AutoCompleteSource = AutoCompleteSource.ListItems

        CmbEntSubHead.SelectedIndex = -1

    End Sub


    Private Sub BtnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Clear_Data()
    End Sub

    Private Sub Clear_Data()
        Id = 0
        CmbEntHead.Text = ""
        CmbEntSubHead.Text = ""
        CmbEntry_Type.Text = ""
        CmbEntSubHead.SelectedIndex = -1
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

    Private Sub BtnNew_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnNew.Click
        Clear_Data()
        CmbEntHead.Focus()
    End Sub

    Private Sub ListView1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ListView1.KeyDown
        If e.KeyCode = Keys.Enter Then
            Load_Edit()
        End If
    End Sub

    Private Sub Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Delete.Click
        If ListView1.SelectedItems.Count = 0 Then
            MsgBox("Please select an item..", MsgBoxStyle.Information, "Information")
            Exit Sub
        End If
        If MsgBox("Are you sure want delete this entry?", MsgBoxStyle.YesNo, "Delete") = MsgBoxResult.No Then
            Exit Sub
        End If
        Dim Status As Boolean
        ' ObjDA = New VehDA

        Status = ObjDA.Delete_entryheads(Val(ListView1.SelectedItems(0).Text))

        If Status Then
            MsgBox("Delete successfully..", MsgBoxStyle.Information, "Deleted")
            Find()
            Clear_Data()

        Else
            MsgBox("Unable to delete the selected item..", MsgBoxStyle.Information, "Information")
        End If
    End Sub

    Private Sub CmbEntHead_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CmbEntHead.KeyDown
        If e.KeyCode = Keys.Enter Then
            CmbEntSubHead.Focus()
        End If
    End Sub

    Private Sub CmbEntHead_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbEntHead.SelectedIndexChanged
        If CmbEntHead.CanFocus Then
            load_Entry_sub_Head()
        End If
    End Sub

    Private Sub CmbEntSubHead_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CmbEntSubHead.KeyDown
        If e.KeyCode = Keys.Enter Then
            BtnSave.Focus()
        End If
    End Sub

    Private Sub CmbEntHead_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbEntHead.TextChanged
        CmbEntHead.BackColor = Color.WhiteSmoke
    End Sub

    Private Sub CmbEntSubHead_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbEntSubHead.TextChanged
        CmbEntSubHead.BackColor = Color.WhiteSmoke
    End Sub

    Private Sub TxtSearch_KeyDown_1(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TxtSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            Find()
        End If
    End Sub

    Private Sub CmbEntSubHead_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbEntSubHead.SelectedIndexChanged
        'If ds.Entry_Head.Select("Entry_Sub_Head='" & CmbEntSubHead.Text.Trim & "'").Count > 0 Then
        '    CmbEntHead.BackColor = Color.Yellow
        '    CmbEntHead.Focus()
        'Else
        '    CmbEntHead.BackColor = Color.WhiteSmoke

        'End If

    End Sub
End Class