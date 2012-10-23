Imports AlarmClockWP7.AlarmServiceReference

Public Class AlarmCompare
    Implements IComparer(Of AlarmDetail)

    Public Function Compare(ByVal a1 As AlarmServiceReference.AlarmDetail, _
                        ByVal a2 As AlarmServiceReference.AlarmDetail) As Integer Implements System.Collections.Generic.IComparer(Of AlarmServiceReference.AlarmDetail).Compare

        Return a1.AlarmDateTime.CompareTo(a2.AlarmDateTime)
    End Function

End Class

