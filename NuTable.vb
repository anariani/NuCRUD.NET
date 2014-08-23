Public Class NuTable
    Public title As String = "Title"
    Public table As String = "Table Name"
    Public paging As Boolean = True
    Public pageSize As Integer = 10
    Public sorting As Boolean = True
    Public defaultSorting As String = "id desc"
    Public fields As New List(Of NuField)
    Public action As New NuAction
    Public query As String
    Public where As String
    Public orderby As String
    Sub New()
    End Sub
End Class
