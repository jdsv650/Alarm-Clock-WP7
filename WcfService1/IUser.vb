Imports System.ServiceModel
Imports System.Collections.Generic

<ServiceContract()>
Public Interface IUser

    <OperationContract()>
    Function CreateUser(ByVal uname As String, ByVal email As String, ByVal phone As String) As Integer

    <OperationContract()>
    Function ReturnUsers() As DataSet

    <OperationContract()>
    Function ReturnUserList() As List(Of User)
End Interface
