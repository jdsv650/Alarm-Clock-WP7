Imports System.Data.SqlClient
Imports System.Data
Imports System.Configuration
Imports System.Collections.Generic
Imports System.Linq
Imports System.Runtime.Serialization
Imports System.ServiceModel
Imports System.ServiceModel.Web
Imports System.Text


' NOTE: You can use the "Rename" command on the context menu to change the class name "User" in code, svc and config file together.
Public Class User
    Implements IUser

    Public Function CreateUser(ByVal uname As String, ByVal email As String, ByVal phone As String) As Integer Implements IUser.CreateUser
        Dim conn As SqlConnection = New SqlConnection(GetConnectionString())
        Dim sql As String = "INSERT INTO ""User"" VALUES(@Email,@UserName,@PhoneNumber)"

        Try
            conn.Open()
            Dim cmd As SqlCommand = New SqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@UserName", uname)
            cmd.Parameters.AddWithValue("@PhoneNumber", phone)
            cmd.Parameters.AddWithValue("@Email", email)
            cmd.CommandType = CommandType.Text
            cmd.ExecuteNonQuery()
        Catch ex As System.Data.SqlClient.SqlException
            Dim msg As String = "Insert Error:"
            msg += ex.Message
            Throw New Exception(msg)
            'MsgBox(ex.ToString)   ' Show error message.
        Finally
            conn.Close()
        End Try

        Return 0
    End Function

    Public Function ReturnUsers() As DataSet Implements IUser.ReturnUsers

        Dim conn As SqlConnection = New SqlConnection(GetConnectionString())
        Dim selectSql As String = "SELECT [UserName] FROM [User]"
        'store the retrieved items here
        Dim userDS As DataSet = New DataSet

        Try
            conn.Open()
            Dim cmd As SqlCommand = New SqlCommand(selectSql, conn)
            Dim SqlAdapt As SqlDataAdapter = New SqlDataAdapter(cmd)

            'Add items to DS
            SqlAdapt.Fill(userDS)

            'CLIENT APP set DDL.DataSource = userDS
            cmd.ExecuteNonQuery()

        Catch ex As System.Data.SqlClient.SqlException
            Dim msg As String = "Select Error:"
            msg += ex.Message
            Throw New Exception(msg)
            'MsgBox(ex.ToString)   ' Show error message.
        Finally
            conn.Close()
        End Try

        Return userDS
    End Function

    Public Function ReturnUserList() As List(Of User) Implements IUser.ReturnUserList
        Dim context As DataClasses1DataContext = New DataClasses1DataContext()

        Dim res = From r In context.Users Select r
        Return res.ToList()

    End Function

    Private Function GetConnectionString() As String

        Return CType(System.Configuration.ConfigurationManager.ConnectionStrings("AlarmClockConnectionString").ConnectionString, String)
        ' "AlarmClockConnectionString" is the name of the Connection String set up in Web.Config 
        ' TeamBConnectionString for CSD
    End Function

End Class
