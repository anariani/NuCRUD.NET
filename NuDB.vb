Imports System
Imports System.Data
Imports System.Data.Common
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.Data.Odbc
Imports MySql.Data.MySqlClient
Imports System.Windows.Forms
Imports System.Data.OracleClient



Public Class NuDB
    Public Shared MYSQL As Integer = 0
    Public Shared SQL As Integer = 1
    Public Shared ODBC As Integer = 2
    Public Shared OLEDB As Integer = 3
    Public Shared ADODB As Integer = 4
    Public Shared ORACLE As Integer = 5

    Private idbConn As IDbConnection = Nothing
    Private idbAdapter As IDbDataAdapter = Nothing
    Private dbAdapter As DbDataAdapter = Nothing
    Private iReader As IDataReader = Nothing
    Private cmd As IDbCommand = Nothing
    Private dbDriver As Integer
    Private dbConfig As String
    Private q As String

    Public Sub New(dbd As Integer, dbc As String)
        dbDriver = dbd
        dbConfig = dbc
    End Sub
    Public Sub close()
        idbConn.Close()
    End Sub
    Public Function connect()
        Select Case dbDriver
            Case OLEDB
                idbConn = New OleDbConnection(dbConfig)
            Case SQL
                idbConn = New SqlConnection(dbConfig)
            Case ODBC
                idbConn = New OdbcConnection(dbConfig)
            Case MYSQL
                idbConn = New MySqlConnection(dbConfig)
            Case ADODB
                idbConn = New ADODB.Connection
                idbConn.ConnectionString = dbConfig
            Case ORACLE
                idbConn = New OracleConnection(dbConfig)
            Case Else
        End Select
        If IsNothing(idbConn) Then
            Return False
        Else

            Try
                idbConn.Open()
            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                cmd = idbConn.CreateCommand()
            End Try
            Return isConnected()
        End If
    End Function
    Private Function isConnected()
        If idbConn.State = ConnectionState.Open Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function getDataAdapter() As IDbDataAdapter
        Select Case dbDriver
            Case OLEDB
                idbAdapter = New OleDbDataAdapter(q, dbConfig)
            Case SQL
                idbAdapter = New SqlDataAdapter(q, dbConfig)
            Case ODBC
                idbAdapter = New OdbcDataAdapter(q, dbConfig)
            Case MYSQL
                idbAdapter = New MySqlDataAdapter(q, dbConfig)
            Case ADODB
                idbAdapter = New OleDbDataAdapter(q, dbConfig)
            Case ORACLE
                idbAdapter = New OracleDataAdapter(q, dbConfig)
            Case Else
        End Select
        Return idbAdapter
    End Function
    Public Sub setQuery(query As String)
        If isConnected() Then
            cmd.CommandText = query
        Else
            MsgBox("Could Not Make Connection")
        End If
    End Sub
    Public Function query()
        Dim stat = cmd.ExecuteNonQuery()
        'cmd.Dispose()
        Return stat
    End Function
    Public Function loadObject()
        Dim table As New DataTable
        dbAdapter = getDataAdapter()
        dbAdapter.SelectCommand = cmd
        dbAdapter.Fill(table)
        If table.Rows.Count >= 1 Then
            Return table.Rows(0)
        Else
            Return Nothing
        End If
    End Function
    Public Function loadObjectList()
        Dim table As New DataTable
        dbAdapter = getDataAdapter()
        dbAdapter.SelectCommand = cmd
        dbAdapter.Fill(table)
        If table.Rows.Count >= 1 Then
            Return table.Rows
        Else
            Return Nothing
        End If
    End Function

    Public Function loadResult()
        Dim res As Object = cmd.ExecuteScalar
        If res Is Nothing Then
            Return ""
        Else
            Return res.ToString()
        End If
    End Function

    Public Function loadDataSet(table As String, Optional ByVal q As String = Nothing) As DataSet
        Dim ds As New DataSet()
        If q IsNot Nothing Then
            setQuery(q)
        Else
            setQuery("select * from " & table)
        End If
        dbAdapter = getDataAdapter()
        dbAdapter.SelectCommand = cmd
        dbAdapter.Fill(ds)
        Return ds
    End Function
    Public Sub setDataGrid(datagrid As DataGridView, table As String, Optional ByVal q As String = Nothing)
        Dim ds As DataSet
        If q IsNot Nothing Then
            ds = loadDataSet(table, q)
        Else
            ds = loadDataSet(table)
        End If
        datagrid.DataSource = ds.Tables(0)
        datagrid.AutoResizeColumns()
        datagrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

    End Sub
End Class
