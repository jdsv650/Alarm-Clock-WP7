Imports AlarmClockWP7.AlarmServiceReference

Partial Public Class ConfirmNewView
    Inherits PhoneApplicationPage

    Dim proxy As AlarmClient = New AlarmClient

    Public Sub New()
        InitializeComponent()
        AddHandler proxy.AddAlarmCompleted, AddressOf AddAlarmCompleted
    End Sub

    Private Sub AddAlarmCompleted(ByVal sender As Object, ByVal e As AddAlarmCompletedEventArgs)
        'check ret val ? -1 fails
    End Sub

    Private Sub YesButton_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        If CType(Application.Current, App).AddRemove = 1 Then  'Add alarm

            proxy.AddAlarmAsync(CType(Application.Current, App).salarm, CType(Application.Current, App).name)
            ' CType(Application.Current, App).alarmL._aList.Add(CType(Application.Current, App).alarm1)
        End If

        NavigationService.Navigate(New Uri("/MainPage.xaml", UriKind.Relative))
    End Sub

    Private Sub NoButton_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles NoButton.Click
        'ignore changes
        NavigationService.GoBack()
    End Sub
End Class
