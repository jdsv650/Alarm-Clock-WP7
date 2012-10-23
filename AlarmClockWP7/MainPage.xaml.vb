Imports System
Imports System.Windows.Threading
Imports System.Data
Imports System.Configuration
Imports System.Collections
Imports AlarmClockWP7.UserServiceReference
Imports AlarmClockWP7.AlarmServiceReference

Partial Public Class MainPage
    Inherits PhoneApplicationPage

    Dim proxyU As UserClient = New UserClient
    Dim proxyA As AlarmClient = New AlarmClient

    Public ulist As List(Of User) = New List(Of User)

    ' Constructor
    Public Sub New()
        InitializeComponent()

        PageTitle.Text = Date.Now.ToShortTimeString
        DateBlock.Text = Date.Now.ToShortDateString

        Dim timer As DispatcherTimer = New DispatcherTimer

        'DispatcherTimer setup
        AddHandler timer.Tick, AddressOf dispatcherTimer_Tick
        timer.Interval = New TimeSpan(0, 0, 1)
        timer.Start()

        proxyU = New UserClient
        AddHandler proxyU.ReturnUserListCompleted, AddressOf UsersCompleted
        proxyU.ReturnUserListAsync()

        proxyA = New AlarmClient
        AddHandler proxyA.ReturnAlarmListCompleted, AddressOf AlarmsCompleted

    End Sub

    Private Sub UsersCompleted(ByVal sender As Object, ByVal e As ReturnUserListCompletedEventArgs)
        'keep a list of users
        For Each user In e.Result
            ulist.Add(user)
        Next

        UserListBox.ItemsSource = e.Result

        'if not first load use sel index
        If CType(Application.Current, App).nameIndex <> -1 Then
            UserListBox.SelectedIndex = CType(Application.Current, App).nameIndex
        End If

    End Sub

    Private Sub AlarmsCompleted(ByVal sender As Object, ByVal e As ReturnAlarmListCompletedEventArgs)
        CType(Application.Current, App).alist.Clear()
        Dim index As Integer = 0

        'list of alarms
        For Each a In e.Result
            ' **** don't list alarms over 24 hours old
            ' If a.AlarmDateTime < DateTime.Now.AddHours(-24) Then
            ' don't list expired alarms
            If a.AlarmDateTime < DateTime.Now Then    
                'do nothing
            Else
                CType(Application.Current, App).alist.Add(a) 'add it
            End If
            ' MessageBox.Show(a.AlarmNumber.ToString)
        Next

        CType(Application.Current, App).alist.Sort(New AlarmCompare)

        '  getNextAlarm()
        If getNextAlarmIndex() = -1 Then
            NextAlarmButton.Content = "No Alarms"
            NextAlarmButton.IsEnabled = False
        Else
            NextAlarmButton.Content = CType(Application.Current, App).alist(getNextAlarmIndex).AlarmDateTime.ToShortTimeString
            NextAlarmButton.IsEnabled = True

        End If
    End Sub

    Private Function getNextAlarmIndex() As Integer
        Dim lowIndex As Integer = -1

        If CType(Application.Current, App).alist.Count > 0 Then
            lowIndex = 0
            CType(Application.Current, App).alarmSelected = 0
        End If

        Return lowIndex
    End Function

    '  System.Windows.Threading.DispatcherTimer.Tick handler
    '  Updates the current seconds display 
    Private Sub dispatcherTimer_Tick(ByVal sender As Object, ByVal e As EventArgs)
        PageTitle.Text = Date.Now.ToShortTimeString
        DateBlock.Text = Date.Now.ToShortDateString

        If CType(Application.Current, App).alist.Count > 0 Then
            If CType(Application.Current, App).alist(CType(Application.Current, App).alarmSelected).AlarmDateTime <= DateTime.Now Then
                NavigationService.Navigate(New Uri("/Views/AlarmActive.xaml", UriKind.Relative))
            End If
        End If

    End Sub


    Private Sub ViewButton_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles ViewButton.Click
        ' Set global to edit
        CType(Application.Current, App).AddRemove = 2
        NavigationService.Navigate(New Uri("/Views/AlarmsView.xaml", UriKind.Relative))
    End Sub

    Private Sub NextAlarmButton_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles NextAlarmButton.Click

        ' Set global to Edit
        CType(Application.Current, App).AddRemove = 2
        NavigationService.Navigate(New Uri("/Views/DetailsPivotPage.xaml", UriKind.Relative))
    End Sub


    Private Sub AddAlarmButton_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles AddAlarmButton.Click
        ' Set global to Add
        CType(Application.Current, App).AddRemove = 1

        NavigationService.Navigate(New Uri("/Views/DetailsPivotPage.xaml", UriKind.Relative))
    End Sub


    Private Sub PhoneApplicationPage_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If CType(Application.Current, App).nameIndex = -1 Then 'never selected a user
            ViewButton.Visibility = Windows.Visibility.Collapsed
            NextAlarmButton.Visibility = Windows.Visibility.Collapsed
            AddAlarmButton.Visibility = Windows.Visibility.Collapsed
        Else
            ViewButton.Visibility = Windows.Visibility.Visible
            NextAlarmButton.Visibility = Windows.Visibility.Visible
            AddAlarmButton.Visibility = Windows.Visibility.Visible

            proxyA.ReturnAlarmListAsync(CType(Application.Current, App).name)
        End If

        Try
            Do While NavigationService.BackStack.Any 'Not start of program (empty throughs exception) then erase history
                NavigationService.RemoveBackEntry()
            Loop
        Catch

        End Try
    End Sub

    Private Sub UserListBox_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles UserListBox.SelectionChanged

        ViewButton.Visibility = Windows.Visibility.Visible
        NextAlarmButton.Visibility = Windows.Visibility.Visible
        AddAlarmButton.Visibility = Windows.Visibility.Visible

        CType(Application.Current, App).nameIndex = UserListBox.SelectedIndex 'reuse same user name
        CType(Application.Current, App).name = ulist(UserListBox.SelectedIndex).UserName

        proxyA.ReturnAlarmListAsync(CType(Application.Current, App).name)

    End Sub
End Class
