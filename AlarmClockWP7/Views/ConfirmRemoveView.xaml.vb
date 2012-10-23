Imports AlarmClockWP7.AlarmServiceReference

Partial Public Class ConfirmRemoveView
    Inherits PhoneApplicationPage

    Dim proxy As AlarmClient = New AlarmClient

    Public Sub New()
        InitializeComponent()

        AddHandler proxy.RemoveAlarmCompleted, AddressOf RemoveAlarmCompleted
    End Sub

    Private Sub RemoveAlarmCompleted(ByVal sender As Object, ByVal e As RemoveAlarmCompletedEventArgs)
        'check ret val ? -1 fails
    End Sub

    Private Sub YesButton_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        If CType(Application.Current, App).AddRemove = 0 Then   'Remove selected alarm
            proxy.RemoveAlarmAsync((CType(Application.Current, App).alist(CType(Application.Current, App).alarmSelected).AlarmNumber))
            ' CType(Application.Current, App).alarmL._aList.RemoveAt(CType(Application.Current, App).alarmSelected)
        End If

        NavigationService.Navigate(New Uri("/MainPage.xaml", UriKind.Relative))
    End Sub

    Private Sub NoButton_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles NoButton.Click
        'ignore changes
        NavigationService.GoBack()
    End Sub
End Class
