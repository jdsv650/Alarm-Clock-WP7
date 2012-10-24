Imports System.Windows.Threading
Imports AlarmClockWP7.AlarmServiceReference
Imports Microsoft.Devices
Imports Microsoft.Phone

Partial Public Class AlarmActive
    Inherits PhoneApplicationPage

    Dim proxy As AlarmClient = New AlarmClient
    Dim al As New AlarmDetail
    Dim flag As Integer = 1
    Dim cam As PhotoCamera

    Public Sub New()
        InitializeComponent()

        PageTitle.Text = Date.Now.ToShortTimeString
        '   DateBlock.Text = Date.Now.ToShortDateString

        Dim timer As DispatcherTimer = New DispatcherTimer
        Dim t2 As DispatcherTimer = New DispatcherTimer

        'DispatcherTimer setup
        AddHandler timer.Tick, AddressOf dispatcherTimer_Tick
        timer.Interval = New TimeSpan(0, 0, 10)
        timer.Start()

        'DispatcherTimer2 setup
        AddHandler t2.Tick, AddressOf dispatcherTimer2_Tick
        t2.Interval = New TimeSpan(0, 0, 1)
        t2.Start()

        ' cam = New PhotoCamera(CameraType.Primary)
        ' Event is fired when the PhotoCamera object has been initialized.
        ' AddHandler cam.Initialized, AddressOf cam_Initialized
        ' Set the VideoBrush source to the camera.
        'kills alarm sound
        ' viewfinderBrush.SetSource(cam)
        
    End Sub

    ' Private Sub cam_Initialized(ByVal sender As Object, ByVal e As Microsoft.Devices.CameraOperationCompletedEventArgs)

    '    If e.Succeeded Then
    '        Me.Dispatcher.BeginInvoke(Sub()
    ' Write message.
    '                                      MessageBox.Show("Camera initialized.")
    '                                  End Sub)
    '    End If

    '   If al.Flashlight = True Then
    '       cam.FlashMode = FlashMode.On
    '   End If

    'End Sub

    '  System.Windows.Threading.DispatcherTimer.Tick handler
    '  Updates the current seconds display 
    Private Sub dispatcherTimer_Tick(ByVal sender As Object, ByVal e As EventArgs)
        PageTitle.Text = Date.Now.ToShortTimeString
        ' DateBlock.Text = Date.Now.ToShortDateString
        'loop alarm
        CType(Application.Current, App).GlobalMediaElement.Position = TimeSpan.FromSeconds(0.0)

    End Sub

    '  System.Windows.Threading.DispatcherTimer.Tick handler
    '  Updates the current seconds display 
    Private Sub dispatcherTimer2_Tick(ByVal sender As Object, ByVal e As EventArgs)

        If al.Blinking = True Then
            If flag = 1 Then
                Image1.Visibility = Windows.Visibility.Visible
                Image2.Visibility = Windows.Visibility.Visible
                flag = 0
            Else
                Image1.Visibility = Windows.Visibility.Collapsed
                Image2.Visibility = Windows.Visibility.Collapsed
                flag = 1
            End If
        End If

    End Sub

    Private Sub SnoozeButton_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles SnoozeButton.Click

        'snooze true - add snooze interval to time and create new alarm
        If (al.Snooze = True) Then
            'Create new alarm 5,10,15.. minutes from now (snooze interval)
            CType(Application.Current, App).alist.Clear()
            CType(Application.Current, App).GlobalMediaElement.Stop()
            CType(Application.Current, App).GlobalMediaElement.AutoPlay = False

            al.AlarmDateTime = DateTime.Now.AddMinutes(al.SnoozeInterval)
            'add to db
            proxy.AddAlarmAsync(al, CType(Application.Current, App).name)

            NavigationService.Navigate(New Uri("/MainPage.xaml", UriKind.Relative))
        End If

        'don't do anything if snooze disabled

    End Sub

    Private Sub OffButton_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles OffButton.Click
        'clear alarm list will reset on page load
        CType(Application.Current, App).alist.Clear()
        CType(Application.Current, App).GlobalMediaElement.Stop()
        CType(Application.Current, App).GlobalMediaElement.AutoPlay = False

        'If cam.FlashMode = FlashMode.On Then
        'cam.FlashMode = FlashMode.Off
        ' End If

        NavigationService.Navigate(New Uri("/MainPage.xaml", UriKind.Relative))
    End Sub

    Private Sub PhoneApplicationPage_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded


        al = CType(Application.Current, App).alist(0)

        '***  If Switched from Resource (.dll) to Content type .xap then  remove /AlarmClockWP7;component prefix ********
        'volume range listed as 0-1 actual is 0.5-1 on develop sys
        If (al.AudibleAlarm = True) Then
            CType(Application.Current, App).GlobalMediaElement.Volume = System.Convert.ToDouble(al.VolumeLevel * 0.005 + 0.5)
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
            CType(Application.Current, App).GlobalMediaElement.Play()
        End If

        If al.VisualAlarm = True Then
            TextBlock1.Text = "Message: "
            MessageTextBlock.Text = al.AlarmText
        Else
            TextBlock1.Text = ""
        End If

        If al.Vibrate = True Then  'vibrate for 2 seconds (0-5) valid
            'no way to test this on the emulator
            VibrateController.Default.Start(TimeSpan.FromSeconds(2))
        End If



    End Sub




    Private Sub InitializePropertyValues()
        '  CType(Application.Current, App).GlobalMediaElement.Volume = System.Convert.ToDouble(.Value)

    End Sub


    Private Sub PhoneApplicationPage_BackKeyPress(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles MyBase.BackKeyPress
        ' cancel backkey or shut of alarm on backkey?
        'e.Cancel = True
        'clear alarm list will reset on page load
        CType(Application.Current, App).alist.Clear()
        CType(Application.Current, App).GlobalMediaElement.Stop()
        CType(Application.Current, App).GlobalMediaElement.AutoPlay = False

        '  If cam.FlashMode = FlashMode.On Then
        'cam.FlashMode = FlashMode.Off
        ' End If

        NavigationService.Navigate(New Uri("/MainPage.xaml", UriKind.Relative))

    End Sub
End Class
