Imports System.Windows.Forms
Imports System.Drawing


Public Class NuForm
    Public action As String = "VIEW"
    Public crud As NuCRUD
    Public mPanel As New TableLayoutPanel()
    Public dataPanel As New TableLayoutPanel()
    Private Sub NuForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.AutoSize = True
        Select Case action
            Case "VIEW"
                Me.Text = crud.table.title
            Case "ADD"
                Me.Text = "Tambah " & crud.table.title
            Case "EDIT"
                Me.Text = "Edit " & crud.table.title
        End Select
        addTable()
    End Sub
    Public Overloads Function ShowDialog(Optional act As String = "VIEW", Optional ByRef nuCrud As NuCRUD = Nothing) As DialogResult
        action = act
        crud = nuCrud
        Return MyBase.ShowDialog()
    End Function
    Private Sub addTable()
        mPanel.AutoSize = True
        mPanel.Dock = DockStyle.Fill
        mPanel.Controls.Clear()
        mPanel.ColumnStyles.Clear()
        mPanel.RowStyles.Clear()
        mPanel.ColumnCount = 1
        mPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100))
        mPanel.RowCount = 2
        mPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(SizeType.Percent, 100))
        mPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(SizeType.Absolute, 60))
        mPanel.BackColor = Color.Transparent
        ' mainPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(SizeType.Absolute, 30))

        'mainPanel.BackColor = Color.Aqua
        mPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble

        Me.Controls.Add(mPanel)
        addData()
    End Sub

    Private Sub addData()
        dataPanel.AutoSize = True
        dataPanel.Dock = DockStyle.Fill
        dataPanel.Controls.Clear()
        dataPanel.ColumnStyles.Clear()
        dataPanel.RowStyles.Clear()
        dataPanel.ColumnCount = 3
        dataPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30))
        dataPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70))
        Dim fields = getFields()
        For Each nf As NuField In fields
            dataPanel.RowStyles.Add(New RowStyle(SizeType.AutoSize))
            dataPanel.RowCount += 1
            Dim lbl As New Label()
            lbl.Text = If(nf.title Is Nothing, nf.name, nf.title)
            lbl.Dock = DockStyle.Fill
            dataPanel.Controls.Add(lbl, 0, dataPanel.RowCount - 1)
            addControl(nf)
        Next
        mPanel.Controls.Add(dataPanel)
    End Sub
    Private Sub addControl(nf As NuField)
        Dim txt
        Select Case nf.type
            Case "date"
                txt = addDate(nf)
            Case "options"
                txt = addOptions(nf)
            Case "textarea"
                txt = addTextArea(nf)
            Case Else
                txt = addText(nf)
        End Select
        dataPanel.Controls.Add(txt, 1, dataPanel.RowCount - 1)
    End Sub
    Private Function addDate(nf As NuField)
        Dim txt As New DateTimePicker()
        txt.Name = nf.name
        txt.Dock = DockStyle.Fill
        Return txt
    End Function
    Private Function addOptions(nf As NuField)
        Dim txt As New ComboBox()
        txt.DataSource = nf.options
        txt.DisplayMember = "Value"
        txt.ValueMember = "Key"
        txt.Dock = DockStyle.Fill
        Return txt
    End Function
    Private Function addText(nf)
        Dim txt As New TextBox()
        txt.Name = nf.name
        txt.Text = ""
        txt.Dock = DockStyle.Fill
        Return txt
    End Function
    Private Function addTextArea(nf)
        Dim txt As New TextBox()
        txt.Name = nf.name
        txt.Text = ""
        txt.Multiline = True
        txt.Height = txt.Height * 2
        txt.Dock = DockStyle.Fill
        Return txt
    End Function
    Private Function getFields() As List(Of NuField)
        Dim dbfields As New List(Of NuField)
        Select Case action
            Case "VIEW"
                For Each nf As NuField In crud.table.fields
                    If nf.read Then
                        dbfields.Add(nf)
                    End If
                Next
            Case "ADD"
                For Each nf As NuField In crud.table.fields
                    If nf.create Then
                        dbfields.Add(nf)
                    End If
                Next
            Case "EDIT"
                For Each nf As NuField In crud.table.fields
                    If nf.update Then
                        dbfields.Add(nf)
                    End If
                Next
        End Select

        Return dbfields
    End Function
End Class