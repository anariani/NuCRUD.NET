Public Class NuField
    Public name As String
    Public key As Boolean = False
    Public title As String
    Public update As Boolean = True
    Public list As Boolean = True
    Public create As Boolean = True
    Public read As Boolean = True
    Public type As String = "text"
    Public width As String = Nothing
    Public options As New List(Of DictionaryEntry)
    Public dateFormat As String = "yyyy-mm-dd"
    Public defaultValue As String = Nothing
    Public values As List(Of DictionaryEntry)
    Public searchable As Boolean = False
    Public dispay

    Sub New(namefield As String, Optional titlefield As String = Nothing, Optional showinlist As Boolean = True, Optional keyfield As Boolean = False, Optional search As Boolean = False)
        name = namefield
        title = If(titlefield Is Nothing, namefield, titlefield)
        list = showinlist
        key = keyfield
        searchable = search
    End Sub

    Public Overrides Function ToString() As String
        Return name
    End Function


End Class
