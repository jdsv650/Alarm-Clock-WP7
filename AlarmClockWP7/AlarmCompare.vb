Imports AlarmClockWP7.AlarmServiceReference

Public Class AlarmCompare
    Implements IComparer(Of AlarmDetail)

    Public Function Compare(ByVal a1 As AlarmServiceReference.AlarmDetail, _
                        ByVal a2 As AlarmServiceReference.AlarmDetail) As Integer Implements System.Collections.Generic.IComparer(Of AlarmServiceReference.AlarmDetail).Compare

        If a1.AlarmDateTime.Day < a2.AlarmDateTime.Day Then
            Return -1
        End If

        If a1.AlarmDateTime.Day = a2.AlarmDateTime.Day Then
            Return a1.AlarmDateTime.TimeOfDay.CompareTo(a2.AlarmDateTime.TimeOfDay)
        End If

        'esle second alarm is greater
        Return 1
    End Function

End Class

