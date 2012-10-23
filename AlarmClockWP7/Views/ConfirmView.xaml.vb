Imports System.Windows.Controls.Primitives
Imports AlarmClockWP7.AlarmServiceReference

Partial Public Class Page3
    Inherits PhoneApplicationPage

    Dim proxy As AlarmClient = New AlarmClient

    Public Sub New()
        InitializeComponent()
        AddHandler proxy.UpdateAlarmCompleted, AddressOf UpdateAlarmCompleted

        ' get global to add or Remove
        If CType(Application.Current, App).AddRemove = 0 Then  'Remove
            PageTitle.Text = "Remove"
        Else  'Add or Edit depends on what page we navigated from
            PageTitle.Text = "Save"
        End If
    End Sub

    Private Sub UpdateAlarmCompleted(ByVal sender As Object, ByVal e As UpdateAlarmCompletedEventArgs)
        'check ret val ? -1 fails
    End Sub

    Private Sub YesButton_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles YesButton.Click
        'Add or remove alarm from list check global (0 remove)  (1 add) (2 edit)

        If CType(Application.Current, App).AddRemove = 2 Then  'Edit alarm
            'update database
            proxy.UpdateAlarmAsync(CType(Application.Current, App).salarm)
            ' CType(Application.Current, App).alarmL._aList.RemoveAt(CType(Application.Current, App).alarmSelected)
            ' CType(Application.Current, App).alarmL._aList.Insert(CType(Application.Current, App).alarmSelected, (CType(Application.Current, App).alarm1))
        End If

        NavigationService.Navigate(New Uri("/MainPage.xaml", UriKind.Relative))
    End Sub

    Private Sub NoButton_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles NoButton.Click
        'ignore changes
        NavigationService.GoBack()
    End Sub

End Class
