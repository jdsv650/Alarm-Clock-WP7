Imports System.Collections.Generic
Imports System.Collections
Imports System.ComponentModel

Public Class AlarmList
    Implements INotifyPropertyChanged

    ' Private _aList As New List(Of Alarm)
    Public _aList As New List(Of Alarm)

    Public Sub New()
        _aList.Add(New Alarm())
        _aList.Add(New Alarm())
        _aList.Add(New Alarm())
        _aList.Add(New Alarm())
        _aList.Add(New Alarm())
    End Sub


    ' Public Property AlarmList() As List(Of Alarm)
    '   Get
    '       Return _aList
    '   End Get
    '   Set(value As List(Of Alarm))
    '       _aList = value
    '       NotifyPropertyChanged("AlarmList")
    '   End Set
    'End Property

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Private Sub NotifyPropertyChanged(ByVal propertyName As String)
        Dim handler As PropertyChangedEventHandler = PropertyChangedEvent
        If Nothing IsNot handler Then
            handler(Me, New PropertyChangedEventArgs(propertyName))
        End If
    End Sub


End Class
