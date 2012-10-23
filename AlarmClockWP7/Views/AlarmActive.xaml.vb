Imports System.Windows.Threading
Imports AlarmClockWP7.AlarmServiceReference

Partial Public Class AlarmActive
    Inherits PhoneApplicationPage

    Dim al As New AlarmDetail
    ' Dim MediaE As MediaElement = New MediaElement

    'Public Event MediaEnded As RoutedEventHandler

    Public Sub New()
        InitializeComponent()

        PageTitle.Text = Date.Now.ToShortTimeString
        '   DateBlock.Text = Date.Now.ToShortDateString

        Dim timer As DispatcherTimer = New DispatcherTimer

        'DispatcherTimer setup
        AddHandler timer.Tick, AddressOf dispatcherTimer_Tick
        timer.Interval = New TimeSpan(0, 0, 10)
        timer.Start()
    End Sub

    '  System.Windows.Threading.DispatcherTimer.Tick handler
    '  Updates the current seconds display 
    Private Sub dispatcherTimer_Tick(ByVal sender As Object, ByVal e As EventArgs)
        PageTitle.Text = Date.Now.ToShortTimeString
        ' DateBlock.Text = Date.Now.ToShortDateString
        CType(Application.Current, App).GlobalMediaElement.Position = TimeSpan.FromSeconds(0.0)

        If al.SoundNumber = 1 Then
            CType(Application.Current, App).GlobalMediaElement.Source = New Uri("/AlarmClockWP7;component/Sounds/Alarm_Clock.mp3", UriKind.Relative)
        ElseIf al.SoundNumber = 2 Then
            CType(Application.Current, App).GlobalMediaElement.Source = New Uri("/AlarmClockWP7;component/Sounds/Car_Alarm.mp3", UriKind.Relative)
        ElseIf al.SoundNumber = 3 Then
            CType(Application.Current, App).GlobalMediaElement.Source = New Uri("/AlarmClockWP7;component/Sounds/Evacuate.mp3", UriKind.Relative)
        ElseIf al.SoundNumber = 4 Then
            CType(Application.Current, App).GlobalMediaElement.Source = New Uri("/AlarmClockWP7;component/Sounds/Rooster.mp3", UriKind.Relative)
        ElseIf al.SoundNumber = 5 Then
            CType(Application.Current, App).GlobalMediaElement.Source = New Uri("/AlarmClockWP7;component/Sounds/Tornado_Siren.mp3", UriKind.Relative)
        End If

    End Sub


    Private Sub SnoozeButton_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles SnoozeButton.Click
        'Create new alarm 5,10,15.. minutes from now (snooze interval)
    End Sub

    Private Sub OffButton_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles OffButton.Click
        'clear alarm list will reset on page load
        CType(Application.Current, App).alist.Clear()
        CType(Application.Current, App).GlobalMediaElement.Stop()
        CType(Application.Current, App).GlobalMediaElement.AutoPlay = False

        NavigationService.Navigate(New Uri("/MainPage.xaml", UriKind.Relative))
    End Sub

    Private Sub PhoneApplicationPage_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        al = CType(Application.Current, App).alist(CType(Application.Current, App).alarmSelected)

        '***  If Switched from Resource (.dll) to Content type .xap then  remove /AlarmClockWP7;component prefix ********
        CType(Application.Current, App).GlobalMediaElement.AutoPlay = True

        If al.SoundNumber = 1 Then
            CType(Application.Current, App).GlobalMediaElement.Source = New Uri("/AlarmClockWP7;component/Sounds/Alarm_Clock.mp3", UriKind.Relative)
        ElseIf al.SoundNumber = 2 Then
            CType(Application.Current, App).GlobalMediaElement.Source = New Uri("/AlarmClockWP7;component/Sounds/Car_Alarm.mp3", UriKind.Relative)
        ElseIf al.SoundNumber = 3 Then
            CType(Application.Current, App).GlobalMediaElement.Source = New Uri("/AlarmClockWP7;component/Sounds/Evacuate.mp3", UriKind.Relative)
        ElseIf al.SoundNumber = 4 Then
            CType(Application.Current, App).GlobalMediaElement.Source = New Uri("/AlarmClockWP7;component/Sounds/Rooster.mp3", UriKind.Relative)
        ElseIf al.SoundNumber = 5 Then
            CType(Application.Current, App).GlobalMediaElement.Source = New Uri("/AlarmClockWP7;component/Sounds/Tornado_Siren.mp3", UriKind.Relative)
        End If

        '  CType(Application.Current, App).GlobalMediaElement = MediaE

        '  AddHandler CType(Application.Current, App).GlobalMediaElement.MediaEnded, AddressOf MediaEnded

        ' MediaEnded(sender, e)

        CType(Application.Current, App).GlobalMediaElement.Play()

    End Sub

    ' Public Sub MediaEnded(sender As System.Object, e As System.Windows.RoutedEventArgs)
    '   CType(Application.Current, App).GlobalMediaElement.Position = TimeSpan.FromSeconds(0.0)
    '   If al.SoundNumber = 1 Then
    ''       CType(Application.Current, App).GlobalMediaElement.Source = New Uri("/AlarmClockWP7;component/Sounds/Alarm_Clock.mp3", UriKind.Relative)
    '   ElseIf al.SoundNumber = 2 Then
    '      CType(Application.Current, App).GlobalMediaElement.Source = New Uri("/AlarmClockWP7;component/Sounds/Car_Alarm.mp3", UriKind.Relative)
    '  ElseIf al.SoundNumber = 3 Then
    '      CType(Application.Current, App).GlobalMediaElement.Source = New Uri("/AlarmClockWP7;component/Sounds/Evacuate.mp3", UriKind.Relative)
    '  ElseIf al.SoundNumber = 4 Then
    '       CType(Application.Current, App).GlobalMediaElement.Source = New Uri("/AlarmClockWP7;component/Sounds/Rooster.mp3", UriKind.Relative)
    '   ElseIf al.SoundNumber = 5 Then
    '       CType(Application.Current, App).GlobalMediaElement.Source = New Uri("/AlarmClockWP7;component/Sounds/Tornado_Siren.mp3", UriKind.Relative)
    '   End If
    ' End Sub

    ' Set the media's starting Volume 
    Private Sub InitializePropertyValues()
        '  CType(Application.Current, App).GlobalMediaElement.Volume = System.Convert.ToDouble(.Value)

    End Sub


End Class
