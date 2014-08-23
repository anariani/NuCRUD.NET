
Imports System.Data.Common
Imports System.Reflection
Imports System.Windows.Forms
Imports System.Drawing

Public Class NuCRUD
    Public table As NuTable
    Private form As Form
    Private q As String
    Private cLeft As Integer = 1
    Private db As NuDB
    Private datagrid As New DataGridView()
    Private title As New Label()
    Private search As New TextBox()
    Private cari As New Button()
    Private cariby As New ComboBox()
    Private reset As New Button()
    Private mainPanel As New TableLayoutPanel()
    Private headerPanel As New Panel()
    Private footerPanel As New Panel()
    Private first As New PictureBox()
    Private prev As New PictureBox()
    Private nex As New PictureBox()
    Private last As New PictureBox()
    Private txtPage As New TextBox()
    Private add As New Button()
    Private dialogForm As New NuForm()

    Dim page As Integer = 1
    Dim limit As Integer = 10
    Dim totalpage As Integer

    Dim CW As Integer
    Dim CH As Integer
    Dim IW As Integer
    Dim IH As Integer
    Dim lblPage As New Label

    Sub New(ByVal DBDriver As Integer, ByVal DBConfig As String, ByRef tablepanel As TableLayoutPanel, ByRef nutable As NuTable)
        db = New NuDB(DBDriver, DBConfig)
        db.connect()
        table = nutable
        limit = table.pageSize
        mainPanel = tablepanel
        main()
    End Sub

    Private Sub main()
        addTable()
        addHeaderPanel()
        'addButton()
        addDatagrid()
        'addButton()
        addFooterPanel()
        'formResize()
    End Sub
    Private Sub addTable()
        'mainPanel.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
        Dim cheight As Integer = mainPanel.Height
        mainPanel.Controls.Clear()
        mainPanel.ColumnStyles.Clear()
        mainPanel.RowStyles.Clear()
        mainPanel.ColumnCount = 1
        mainPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100))
        mainPanel.RowCount = 3
        mainPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(SizeType.Absolute, 70))
        mainPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(SizeType.Percent, 100))
        mainPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(SizeType.Absolute, 60))
        mainPanel.BackColor = Color.Transparent
        ' mainPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(SizeType.Absolute, 30))

        'mainPanel.BackColor = Color.Aqua
        'mainPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble

    End Sub
    Private Sub addDatagrid()
        datagrid.Dock = DockStyle.Fill

        datagrid.AllowUserToAddRows = False
        datagrid.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
        AddHandler datagrid.DataBindingComplete, AddressOf datagridDataBindingComplete
        AddHandler datagrid.RowPostPaint, AddressOf datagridRowPostPaint
        AddHandler datagrid.CellClick, AddressOf datagridClick
        prepareDatagrid()

        mainPanel.Controls.Add(datagrid)
    End Sub
    Private Function addWhere()
        Dim dbfields As New List(Of NuField)
        Dim where As String = ""
        Dim list As New ArrayList
        Console.WriteLine(cariby.SelectedValue)
        If search.Text.Length >= 2 Then
            If cariby.SelectedValue = "All" Then

                For Each nf As NuField In table.fields
                    If nf.searchable Then
                        list.Add(nf.name & " like '%" & search.Text & "%'")
                    End If
                Next
                If list.Count >= 1 Then
                    where = "where " & String.Join(" or ", list.ToArray)
                End If
            Else
                where = "where " & cariby.SelectedValue & " like '%" & search.Text & "%'"
            End If

        End If
        Return where
    End Function
    Private Sub datagridClick(sender As Object, e As DataGridViewCellEventArgs)
        'Throw New NotImplementedException
        Dim count As Integer = table.fields.Count

        If e.RowIndex >= 0 Then
            If e.ColumnIndex = count Then
                MsgBox(e.ColumnIndex)
            End If
        End If
    End Sub

    Private Sub addButton()
        If datagrid.Columns("edit") Is Nothing Then
            Dim imgEdit As New DataGridViewImageColumn()
            imgEdit.Image = My.Resources.edit
            datagrid.Columns.Add(imgEdit)
            imgEdit.HeaderText = ""
            imgEdit.Name = "edit"
        End If
        If datagrid.Columns("delete") Is Nothing Then
            Dim imgDelete As New DataGridViewImageColumn()
            imgDelete.Image = My.Resources.delete
            datagrid.Columns.Add(imgDelete)
            imgDelete.HeaderText = ""
            imgDelete.Name = "delete"
        End If
    End Sub
    Public Function getNuField(ByRef dbfield As DataGridViewColumn) As NuField
        Dim returnField As NuField = Nothing
        For Each field As NuField In table.fields
            Console.WriteLine(field)
        Next
        For Each field As NuField In table.fields
            Console.WriteLine(field.name)
            If field.name = dbfield.Name Then
                Return field
            End If
        Next
        Return returnField
    End Function

    Private Sub addTitle()
        title.Text = table.title
        title.AutoSize = True
        'title.Left = 25
        'label.TextAlign = ContentAlignment.MiddleCenter
        'label.Width = mainPanel.Width
        headerPanel.Controls.Add(title)
        cLeft = cLeft + 1
    End Sub
    Public Sub formResize()
        IW = mainPanel.FindForm.Width
        IH = mainPanel.FindForm.Height
        CW = IW
        CH = IH
        AddHandler mainPanel.FindForm.Resize, AddressOf onForm_Resize
    End Sub
    Public Sub onForm_Resize(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim RW As Double = (mainPanel.FindForm.Width - CW) / CW ' Ratio change of width
        Dim RH As Double = (mainPanel.FindForm.Height - CH) / CH ' Ratio change of height

        For Each Ctrl As Control In mainPanel.FindForm.Controls
            Ctrl.Width += CInt(Ctrl.Width * RW)
            Ctrl.Height += CInt(Ctrl.Height * RH)
            Ctrl.Left += CInt(Ctrl.Left * RW)
            Ctrl.Top += CInt(Ctrl.Top * RH)
        Next

        CW = mainPanel.FindForm.Width
        CH = mainPanel.FindForm.Height
    End Sub

    Private Sub datagridRowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs)
        Dim recordNumber = If(page < 2, e.RowIndex + 1, e.RowIndex + ((page - 1) * limit) + 1)
        Using b As SolidBrush = New SolidBrush(datagrid.RowHeadersDefaultCellStyle.ForeColor)

            e.Graphics.DrawString(recordNumber.ToString(System.Globalization.CultureInfo.CurrentUICulture), _
                                   datagrid.DefaultCellStyle.Font, _
                                   b, _
                                   e.RowBounds.Location.X + 20, _
                                   e.RowBounds.Location.Y + 4)
        End Using
    End Sub

    Private Sub datagridDataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs)
        fixFields()
        addButton()

    End Sub
    Private Sub fixFields()
        For Each f As NuField In table.fields
            If f.title IsNot Nothing Then
                datagrid.Columns(f.name).HeaderText = f.title
            End If
            If Not f.list Then
                datagrid.Columns(f.name).Visible = False
            End If
            If f.type = "options" Then
                If datagrid.Columns(f.name & "text") Is Nothing Then
                    Dim colX As New DataGridViewTextBoxColumn

                    colX.HeaderText = f.name
                    colX.Name = f.name & "text"
                    datagrid.Columns.Add(colX)

                    colX.DisplayIndex = datagrid.Columns(f.name).DisplayIndex

                    datagrid.Columns(f.name).Visible = False
                End If
                For Each row As DataGridViewRow In datagrid.Rows
                    Dim val = getOptions(f, row.Cells(f.name).Value)
                    row.Cells("programText").Value = val
                Next

            End If
        Next
    End Sub

    Private Function getOptions(f As NuField, val As Object)
        For Each row As DictionaryEntry In f.options
            If row.Key = val Then
                Return row.Value
            End If
        Next
        Return Nothing
    End Function

    Private Sub addSearchBox()
        search.Top = title.Top + title.Height + 10
        search.Width = 200
        search.Height = 25
        headerPanel.Controls.Add(search)
        cariby.Top = search.Top
        cariby.Height = search.Height
        cariby.Left = search.Left + search.Width + 5
        cariby.DisplayMember = "Value"
        cariby.ValueMember = "Key"
        Dim ComboValues As New List(Of DictionaryEntry)
        ComboValues.Add(New DictionaryEntry("All", "Semua"))
        For Each nf As NuField In table.fields
            If nf.searchable Then
                ComboValues.Add(New DictionaryEntry(nf.name, nf.title))
            End If
        Next
        cariby.DataSource = ComboValues
        cariby.SelectedValue = "All"
        headerPanel.Controls.Add(cariby)
        cLeft = cLeft + 1
    End Sub

    Private Sub addHeaderPanel()
        headerPanel.Dock = DockStyle.Fill
        mainPanel.Controls.Add(headerPanel)
        addTitle()
        addSearchBox()
        addCari()
        addReset()
        add.Text = "Tambah Baru"
        add.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        'add.Dock = DockStyle.Right
        add.Top = search.Top
        add.Width = 100
        add.Left = headerPanel.Right - add.Width - 5
        add.TextAlign = ContentAlignment.MiddleCenter
        AddHandler add.Click, AddressOf addClick
        headerPanel.Controls.Add(add)
    End Sub

    Private Sub addCari()
        cari.Cursor = Cursors.Hand
        cari.Text = "Cari"
        cari.Top = cariby.Top
        cari.Left = cariby.Left + cariby.Width + 5
        cari.Height = search.Height
        cari.Width = 75
        AddHandler cari.Click, AddressOf cariClik
        headerPanel.Controls.Add(cari)
    End Sub

    Private Sub addReset()
        reset.Cursor = Cursors.Hand
        reset.Text = "Reset"
        reset.Top = search.Top
        reset.Left = cari.Left + cari.Width + 5
        reset.Height = search.Height
        reset.Width = 75
        AddHandler reset.Click, AddressOf resetClick
        headerPanel.Controls.Add(reset)
    End Sub

    Private Sub addFooterPanel()
        footerPanel.Dock = DockStyle.Fill
        mainPanel.Controls.Add(footerPanel)
        first.Cursor = Cursors.Hand
        first.Image = My.Resources.first_icon
        first.SizeMode = PictureBoxSizeMode.Zoom
        first.Width = 30
        first.Height = 30
        AddHandler first.Click, AddressOf firstClick
        footerPanel.Controls.Add(first)
        prev.Cursor = Cursors.Hand
        prev.Image = My.Resources.prev_icon
        prev.SizeMode = PictureBoxSizeMode.Zoom
        prev.Top = first.Top
        prev.Left = first.Left + first.Width + 5
        prev.Width = 30
        prev.Height = 30
        AddHandler prev.Click, AddressOf prevClick
        footerPanel.Controls.Add(prev)
        txtPage.Text = 1
        txtPage.Top = first.Top
        txtPage.Left = prev.Left + prev.Width + 5
        txtPage.Width = 60
        txtPage.Height = 30
        txtPage.TextAlign = HorizontalAlignment.Center
        AddHandler txtPage.KeyPress, AddressOf txtPageKeyPress
        AddHandler txtPage.Validated, AddressOf txtPageValidated
        footerPanel.Controls.Add(txtPage)
        nex.Cursor = Cursors.Hand
        nex.Image = My.Resources.next_icon
        nex.SizeMode = PictureBoxSizeMode.Zoom
        nex.Top = first.Top
        nex.Left = txtPage.Left + txtPage.Width + 5
        nex.Width = 30
        nex.Height = 30
        AddHandler nex.Click, AddressOf nextClick
        footerPanel.Controls.Add(nex)
        last.Cursor = Cursors.Hand
        last.Image = My.Resources.last_icon
        last.SizeMode = PictureBoxSizeMode.Zoom
        last.Top = first.Top
        last.Left = nex.Left + nex.Width + 5
        last.Width = 30
        last.Height = 30
        AddHandler last.Click, AddressOf lastClick
        footerPanel.Controls.Add(last)
        lblPage.AutoSize = True
        lblPage.Top = first.Top
        lblPage.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        lblPage.TextAlign = ContentAlignment.MiddleRight
        'adjust position so the right hand point is in the same position as before
        'Label1.Left = rightAlign - Label1.Width
        lblPage.Dock = DockStyle.Right
        footerPanel.Controls.Add(lblPage)
    End Sub

    Private Sub nextClick(sender As Object, e As EventArgs)
        If page < totalpage Then
            page = page + 1
            prepareDatagrid()
        End If
    End Sub

    Private Sub prepareDatagrid()
        Dim ds As New DataSet
        txtPage.Text = page
        Dim dbfields As New List(Of NuField)
        For Each nf As NuField In table.fields
            If nf.dispay Is Nothing Then
                dbfields.Add(nf)
            End If
        Next
        Dim start = If(page < 2, 0, (page - 1) * limit)
        q = "select " & String.Join(",", dbfields) & " from " & table.table & " " & addWhere() & " " & addOrder()
        Dim qdetail As String = q & " limit " & start & "," & limit
        Console.WriteLine(qdetail)
        ds = db.loadDataSet(table.table, qdetail)
        datagrid.DataSource = ds.Tables(0)

        Dim qtotal = "select count(*) from (" & q & ") as v"
        db.setQuery(qtotal)
        Dim total As String = db.loadResult()
        If total = "" Then
            total = "0"
            totalpage = 0
            lblPage.Text = " Total Records : " & total
        Else
            Dim ntotal As Integer = CInt(total)
            totalpage = Int(ntotal / limit) + 1
            lblPage.Text = "Halaman " & page.ToString("#,###,###") & " dari " & totalpage.ToString("#,###,###") & ", Total Records : " & ntotal.ToString("#,###,###")
        End If
    End Sub
    Private Function addOrder()
        If table.defaultSorting IsNot Nothing Then
            Return "order by " & table.defaultSorting
        Else
            Return ""
        End If
    End Function

    Private Sub prevClick(sender As Object, e As EventArgs)
        If page > 1 Then
            page = page - 1
            prepareDatagrid()
        End If
    End Sub

    Private Sub firstClick(sender As Object, e As EventArgs)
        If page > 1 Then
            page = 1
            prepareDatagrid()
        End If
    End Sub

    Private Sub lastClick(sender As Object, e As EventArgs)
        page = totalpage
        prepareDatagrid()
    End Sub

    Private Sub txtPageValidated(sender As Object, e As EventArgs)
        If IsNumeric(txtPage.Text) Then
            page = txtPage.Text
            prepareDatagrid()
        End If
    End Sub

    Private Sub txtPageKeyPress(sender As Object, e As KeyPressEventArgs)
        If Not IsNumeric(e.KeyChar) And Not e.KeyChar = ChrW(Keys.Back) Then
            e.Handled = True
        End If
    End Sub

    Private Sub cariClik(sender As Object, e As EventArgs)
        page = 1
        totalpage = 0
        prepareDatagrid()
    End Sub

    Private Sub resetClick(sender As Object, e As EventArgs)
        page = 1
        search.Text = ""
        prepareDatagrid()
    End Sub

    Private Sub addClick(sender As Object, e As EventArgs)
        dialogForm.ShowDialog(, Me)
    End Sub





End Class
