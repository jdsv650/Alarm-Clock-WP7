Imports System.ServiceModel
Imports System.Collections.Generic

<ServiceContract()>
Public Interface IAlarm

    <OperationContract()>
    Function ReturnAlarmList(ByVal uname As String) As List(Of AlarmDetail)

    <OperationContract()>
    Function UpdateAlarm(ByVal alarm As AlarmDetail) As Integer

    <OperationContract()>
    Function RemoveAlarm(ByVal num As Integer) As Integer

    <OperationContract()>
    Function AddAlarm(ByVal alarm As AlarmDetail, ByVal usern As String) As Integer

End Interface