Imports System.ComponentModel

Public Class Alarm
    Implements INotifyPropertyChanged, IComparable(Of Alarm)

    Private _alarmNumber As Integer
    Private _alarmTime As DateTime = New DateTime

    Public Function CompareTo(ByVal other As Alarm) As Integer Implements System.IComparable(Of AlarmClockWP7.Alarm).CompareTo
        If _alarmTime = other._alarmTime Then
            Return 0
        Else
            If _alarmTime < other._alarmTime Then
                Return -1
            Else
                Return 1
            End If
        End If

    End Function

    Public Sub New()

        '_alarmTime = (DateTime.Now.Hour & ":" & DateTime.Now.Minute)
        _alarmTime = DateTime.Now  '.ToShortTimeString
        _alarmNumber = 10
    End Sub



    Public Property AlarmNumber() As Integer
        Get
            Return _alarmNumber
        End Get
        Set(value As Integer)
            _alarmNumber = value
            'announce change for update
            NotifyPropertyChanged("AlarmNumber")
        End Set
    End Property

    Public Property AlarmTime() As DateTime
        Get
            Return _alarmTime
        End Get
        Set(value As DateTime)
            _alarmTime = value
            NotifyPropertyChanged("AlarmTime")
        End Set
    End Property

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Private Sub NotifyPropertyChanged(ByVal propertyName As String)
        Dim handler As PropertyChangedEventHandler = PropertyChangedEvent
        If Nothing IsNot handler Then
            handler(Me, New PropertyChangedEventArgs(propertyName))
        End If
    End Sub



End Class
