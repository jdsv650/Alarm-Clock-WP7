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
    Dim dTime As DateTime = New DateTime

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

        dTime = DateTime.Now

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
        CType(Application.Current, App).slist.Clear()

        Dim index As Integer = 0

        'list of alarms
        For Each a In e.Result
            'for any alarm check recurring
            If a.Monday = True And DateTime.Now.DayOfWeek = DayOfWeek.Monday And a.AlarmDateTime.TimeOfDay > DateTime.Now.TimeOfDay Then 'alarm on Monday and day of week is monday keep it

                CType(Application.Current, App).alist.Add(a) 'add it
            ElseIf a.Tuesday = True And DateTime.Now.DayOfWeek = DayOfWeek.Tuesday And a.AlarmDateTime.TimeOfDay > DateTime.Now.TimeOfDay Then 'alarm on tues and day of week is tues 
                CType(Application.Current, App).alist.Add(a) 'add it
            ElseIf a.Wednesday = True And DateTime.Now.DayOfWeek = DayOfWeek.Wednesday And a.AlarmDateTime.TimeOfDay > DateTime.Now.TimeOfDay Then 'alarm on wed and day of week is wed 
                CType(Application.Current, App).alist.Add(a) 'add it
            ElseIf a.Thursday = True And DateTime.Now.DayOfWeek = DayOfWeek.Thursday And a.AlarmDateTime.TimeOfDay > DateTime.Now.TimeOfDay Then 'alarm on thurs and day of week is thurs
                CType(Application.Current, App).alist.Add(a) 'add it
            ElseIf a.Friday = True And DateTime.Now.DayOfWeek = DayOfWeek.Friday And a.AlarmDateTime.TimeOfDay > DateTime.Now.TimeOfDay Then 'alarm on fri and day of week is fri
                CType(Application.Current, App).alist.Add(a) 'add it
            ElseIf a.Saturday = True And DateTime.Now.DayOfWeek = DayOfWeek.Saturday And a.AlarmDateTime.TimeOfDay > DateTime.Now.TimeOfDay Then 'alarm on sat and day of week is sat
                CType(Application.Current, App).alist.Add(a) 'add it
            ElseIf a.Sunday = True And DateTime.Now.DayOfWeek = DayOfWeek.Sunday And a.AlarmDateTime.TimeOfDay > DateTime.Now.TimeOfDay Then 'alarm on sun and day of week is sun
                CType(Application.Current, App).alist.Add(a) 'add it

            ElseIf a.AlarmDateTime.Day > DateTime.Now.Day And a.Monday = False And a.Tuesday = False And a.Wednesday = False And a.Thursday = False And a.Friday = False And a.Saturday = False And a.Sunday = False Or
                    (a.AlarmDateTime.Day = DateTime.Now.Day And a.AlarmDateTime.TimeOfDay > DateTime.Now.TimeOfDay And a.Monday = False And a.Tuesday = False And a.Wednesday = False And a.Thursday = False And a.Friday = False And a.Saturday = False And a.Sunday = False) Then
                CType(Application.Current, App).alist.Add(a) 'add it
            End If
                ' MessageBox.Show(a.AlarmNumber.ToString)
        Next

        'sort first
        CType(Application.Current, App).alist.Sort(New AlarmCompare)

        Dim str As String
        'alarm list built so build recurring list for display in alarm view
        For Each a In CType(Application.Current, App).alist
            str = ""
            If a.Sunday = True Then
                str = str + "S"
            End If
            If a.Monday = True Then
                str = str + "M"
            End If
            If a.Tuesday = True Then
                str = str + "T"
            End If
            If a.Wednesday = True Then
                str = str + "W"
            End If
            If a.Thursday = True Then
                str = str + "R"
            End If
            If a.Friday = True Then
                str = str + "F"
            End If
            If a.Saturday = True Then
                str = str + "S"
            End If
            If str = "" Then
                str = "     "
            End If

            CType(Application.Current, App).slist.Add(str)
        Next

        If getNextAlarmIndex() = -1 Then
            NextAlarmButton.Content = "No Alarms"
            NextAlarmButton.IsEnabled = False
        Else
            NextAlarmButton.Content = CType(Application.Current, App).alist(CType(Application.Current, App).alarmActive).AlarmDateTime.ToShortTimeString
            NextAlarmButton.IsEnabled = True

        End If
    End Sub


    Public Function getNextAlarmIndex() As Integer
        Dim lowIndex As Integer = -1

        If CType(Application.Current, App).alist.Count > 0 Then
            lowIndex = 0
        End If

        CType(Application.Current, App).alarmActive = lowIndex
        Return lowIndex
    End Function

    '  System.Windows.Threading.DispatcherTimer.Tick handler
    '  Updates the current seconds display 
    Private Sub dispatcherTimer_Tick(ByVal sender As Object, ByVal e As EventArgs)
        PageTitle.Text = Date.Now.ToShortTimeString
        DateBlock.Text = Date.Now.ToShortDateString

        'start of new day reload list of alarms 
        If (DateTime.Now.Year > dTime.Year Or DateTime.Now.Month > dTime.Month Or DateTime.Now.Day > dTime.Day) Then
            proxyA.ReturnAlarmListAsync(CType(Application.Current, App).name)
            dTime = DateTime.Now
        End If

        '**** Activate alarm *****
        If CType(Application.Current, App).alist.Count > 0 Then
            If CType(Application.Current, App).alist(0).AlarmDateTime.TimeOfDay <= DateTime.Now.TimeOfDay Then
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

    Private Sub Button1_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button1.Click
        If CType(Application.Current, App).nameIndex = -1 Then 'never selected a user
            MessageBox.Show("Select a User First")
        Else
            proxyA.ReturnAlarmListAsync(CType(Application.Current, App).name)
            MessageBox.Show("Alarms synchronized")
        End If
    End Sub
End Class
