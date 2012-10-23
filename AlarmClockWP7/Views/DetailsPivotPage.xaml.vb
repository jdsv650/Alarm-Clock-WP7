Imports AlarmClockWP7.AlarmServiceReference

Partial Public Class PivotPage1
    Inherits PhoneApplicationPage

    Dim proxy As AlarmClient = New AlarmClient

    Public Sub New()
        Dim indexSel As Integer = CType(Application.Current, App).alarmSelected

        InitializeComponent()

        ' get global to add or Remove
        If CType(Application.Current, App).AddRemove = 0 Or CType(Application.Current, App).AddRemove = 2 Then  'Edit or Remove
            RemoveAlarmGrid.Visibility = Windows.Visibility.Visible
            'edit
            If (CType(Application.Current, App).alist.Count > 0) Then
                'set picker to get currently selected time
                'TimePicker.Value = CType(Application.Current, App).alarmL._aList(indexSel).AlarmTime.ToShortTimeString
                TimePicker.Value = CType(Application.Current, App).alist(CType(Application.Current, App).alarmSelected).AlarmDateTime.ToShortTimeString
                'audio
                AudibleOnOffToggleButton.IsChecked = CType(Application.Current, App).alist(CType(Application.Current, App).alarmSelected).AudibleAlarm
                VolumeSlider.Value = CType(Application.Current, App).alist(CType(Application.Current, App).alarmSelected).VolumeLevel * 0.01 '0-1 stored as 0-100
                SoundSelectionListBox.SelectedIndex = CType(Application.Current, App).alist(CType(Application.Current, App).alarmSelected).SoundNumber - 1 'sounds 1-5
                'snooze
                SnoozeToggleButton.IsChecked = CType(Application.Current, App).alist(CType(Application.Current, App).alarmSelected).Snooze
                SnoozeIntervalListBox.SelectedIndex = getSnoozeIndex(CType(Application.Current, App).alist(CType(Application.Current, App).alarmSelected).SnoozeInterval)
                'repeats
                MondayCheckBox.IsChecked = CType(Application.Current, App).alist(CType(Application.Current, App).alarmSelected).Monday
                TuesdayCheckBox.IsChecked = CType(Application.Current, App).alist(CType(Application.Current, App).alarmSelected).Tuesday
                WednesdayCheckBox.IsChecked = CType(Application.Current, App).alist(CType(Application.Current, App).alarmSelected).Wednesday
                ThursdayCheckBox.IsChecked = CType(Application.Current, App).alist(CType(Application.Current, App).alarmSelected).Thursday
                FridayCheckBox.IsChecked = CType(Application.Current, App).alist(CType(Application.Current, App).alarmSelected).Friday
                SaturdayCheckBox.IsChecked = CType(Application.Current, App).alist(CType(Application.Current, App).alarmSelected).Saturday
                SundayCheckBox.IsChecked = CType(Application.Current, App).alist(CType(Application.Current, App).alarmSelected).Sunday
                'visual
                VisualToggleButton.IsChecked = CType(Application.Current, App).alist(CType(Application.Current, App).alarmSelected).VisualAlarm
                BlinkToggleButton.IsChecked = CType(Application.Current, App).alist(CType(Application.Current, App).alarmSelected).Blinking
                'handle null values in db (web inter adds them on update)
                If (CType(Application.Current, App).alist(CType(Application.Current, App).alarmSelected).AlarmText) Is Nothing Then
                    AlarmMessageTextBox.Text = "  "
                Else
                    AlarmMessageTextBox.Text = CType(Application.Current, App).alist(CType(Application.Current, App).alarmSelected).AlarmText
                End If
                'vibrate-flash
                VibrateToggleButton.IsChecked = CType(Application.Current, App).alist(CType(Application.Current, App).alarmSelected).Vibrate
                FlashToggleButton.IsChecked = CType(Application.Current, App).alist(CType(Application.Current, App).alarmSelected).Flashlight
                ' AddRemoveAlarmButton.Content = "Remove"
                ' AddRemoveAlarmImage.Source = New Imaging.BitmapImage(New Uri("/AlarmClockWP7;component/Icons/control_eject.png", UriKind.Relative))
            Else
                MessageBox.Show("List of Alarms is empty")

            End If

            Else  'Add
                ' AddRemoveAlarmButton.Content = "Save"
                'AddRemoveAlarmImage.Source = New Imaging.BitmapImage(New Uri("/AlarmClockWP7;component/Icons/database.png", UriKind.Relative))
                ' 
                RemoveAlarmGrid.Visibility = Windows.Visibility.Collapsed

                TimePicker.Value = DateTime.Now.ToShortTimeString
            End If
    End Sub

    Private Function getSnoozeIndex(ByVal num As Integer)
        If num = 5 Then
            Return 0
        ElseIf num = 10 Then
            Return 1
        ElseIf num = 15 Then
            Return 2
        ElseIf num = 20 Then
            Return 3
        ElseIf num = 25 Then
            Return 4
        ElseIf num = 30 Then
            Return 5
        End If

        Return 0
    End Function

    Private Sub PhoneApplicationPage_OrientationChanged(sender As System.Object, e As Microsoft.Phone.Controls.OrientationChangedEventArgs) Handles MyBase.OrientationChanged
        If Orientation = PageOrientation.Landscape Or Orientation = PageOrientation.LandscapeLeft Or Orientation = PageOrientation.LandscapeRight Then
            ' Set sound selection list box so scroll works on landscape
            SoundSelectionListBox.Height = 120
        Else 'Set sound selection list box so scroll works on Portrait
            SoundSelectionListBox.Height = 300
        End If
    End Sub

    Private Sub AudibleOnOffToggleButton_Checked(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles AudibleOnOffToggleButton.Checked, AudibleOnOffToggleButton.Unchecked
        'Audible alarm on/off
        If AudibleOnOffToggleButton.IsChecked Then
            AudibleOnOffToggleButton.Content = "ON"
            AudibleImage.Source = New Imaging.BitmapImage(New Uri("/AlarmClockWP7;component/Icons/bell.png", UriKind.Relative))
            VolumeSlider.Value = 0.5

            CType(Application.Current, App).GlobalMediaElement.Play()

        Else
            AudibleOnOffToggleButton.Content = "OFF"
            AudibleImage.Source = New Imaging.BitmapImage(New Uri("/AlarmClockWP7;component/Icons/bell_off.png", UriKind.Relative))
            VolumeSlider.Value = 0

            CType(Application.Current, App).GlobalMediaElement.Stop()
        End If
    End Sub

    Private Sub VolumeSlider_ValueChanged(sender As System.Object, e As System.Windows.RoutedPropertyChangedEventArgs(Of System.Double)) Handles VolumeSlider.ValueChanged
        If VolumeSlider.Value = 0 Then
            AudibleOnOffToggleButton.IsChecked = False
            AudibleOnOffToggleButton.Content = "OFF"
        Else
            AudibleOnOffToggleButton.IsChecked = True
            AudibleOnOffToggleButton.Content = "ON"
        End If

    End Sub


    Private Sub SnoozeToggleButton_Checked(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles SnoozeToggleButton.Checked, SnoozeToggleButton.Unchecked
        'Snooze on/off
        If SnoozeToggleButton.IsChecked Then
            SnoozeToggleButton.Content = "ON"
            SnoozeImage.Source = New Imaging.BitmapImage(New Uri("/AlarmClockWP7;component/Icons/control_pause.png", UriKind.Relative))
            'Turn snooze on
        Else
            SnoozeToggleButton.Content = "OFF"
            SnoozeImage.Source = New Imaging.BitmapImage(New Uri("/AlarmClockWP7;component/Icons/control _off.png", UriKind.Relative))
            'Turn snooze off
        End If
    End Sub

    Private Sub VibrateToggleButton_Checked(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles VibrateToggleButton.Checked, VibrateToggleButton.Unchecked
        'Vibrate on/off
        If VibrateToggleButton.IsChecked Then
            VibrateToggleButton.Content = "ON"
            VibrateImage.Source = New Imaging.BitmapImage(New Uri("/AlarmClockWP7;component/Icons/ipod_cast.png", UriKind.Relative))
            'Turn vibrate on
        Else
            VibrateToggleButton.Content = "OFF"
            VibrateImage.Source = New Imaging.BitmapImage(New Uri("/AlarmClockWP7;component/Icons/ipod_cast_off.png", UriKind.Relative))
            'Turn vibrate off
        End If
    End Sub

    Private Sub FlashToggleButton_Checked(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles FlashToggleButton.Checked, FlashToggleButton.Unchecked
        'Flashlight on/off
        If FlashToggleButton.IsChecked Then
            FlashToggleButton.Content = "ON"
            FlashImage.Source = New Imaging.BitmapImage(New Uri("/AlarmClockWP7;component/Icons/lightbulb.png", UriKind.Relative))
            'Flashlight on
        Else
            FlashToggleButton.Content = "OFF"
            FlashImage.Source = New Imaging.BitmapImage(New Uri("/AlarmClockWP7;component/Icons/lightbulb_off.png", UriKind.Relative))
            'Turn Flashlight off
        End If
    End Sub

    Private Sub VisualToggleButton_Checked(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles VisualToggleButton.Checked, VisualToggleButton.Unchecked
        'Visual on/off
        If VisualToggleButton.IsChecked Then
            VisualToggleButton.Content = "ON"
            VisualImage.Source = New Imaging.BitmapImage(New Uri("/AlarmClockWP7;component/Icons/clipboard_text.png", UriKind.Relative))
            'Visual on
        Else
            VisualToggleButton.Content = "OFF"
            VisualImage.Source = New Imaging.BitmapImage(New Uri("/AlarmClockWP7;component/Icons/clipboard_text_off.png", UriKind.Relative))
            'Visual off
        End If
    End Sub

    Private Sub BlinkToggleButton_Checked(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles BlinkToggleButton.Checked, BlinkToggleButton.Unchecked
        'Blink on/off
        If BlinkToggleButton.IsChecked Then
            BlinkToggleButton.Content = "ON"
            BlinkImage.Source = New Imaging.BitmapImage(New Uri("/AlarmClockWP7;component/Icons/link_break.png", UriKind.Relative))
            'Blink on
        Else
            BlinkToggleButton.Content = "OFF"
            BlinkImage.Source = New Imaging.BitmapImage(New Uri("/AlarmClockWP7;component/Icons/link_break_off.png", UriKind.Relative))
            'Blink off
        End If
    End Sub


    Private Sub SoundSelectionListBox_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles SoundSelectionListBox.SelectionChanged
        '***  If Switched from Resource (.dll) to Content type .xap then  remove /AlarmClockWP7;component prefix ********
        ' If SoundSelectionListBox.SelectedIndex = 0 Then
        'CType(Application.Current, App).GlobalMediaElement.Source = New Uri("/AlarmClockWP7;component/Sounds/Alarm_Clock.mp3", UriKind.Relative)
        ' ElseIf SoundSelectionListBox.SelectedIndex = 1 Then
        ' CType(Application.Current, App).GlobalMediaElement.Source = New Uri("/AlarmClockWP7;component/Sounds/Car_Alarm.mp3", UriKind.Relative)
        ' ElseIf SoundSelectionListBox.SelectedIndex = 2 Then
        ' CType(Application.Current, App).GlobalMediaElement.Source = New Uri("/AlarmClockWP7;component/Sounds/Evacuate.mp3", UriKind.Relative)
        ' ElseIf SoundSelectionListBox.SelectedIndex = 3 Then
        ' CType(Application.Current, App).GlobalMediaElement.Source = New Uri("/AlarmClockWP7;component/Sounds/Rooster.mp3", UriKind.Relative)
        ' ElseIf SoundSelectionListBox.SelectedIndex = 4 Then
        ' CType(Application.Current, App).GlobalMediaElement.Source = New Uri("/AlarmClockWP7;component/Sounds/Tornado_Siren.mp3", UriKind.Relative)
        ' End If
    End Sub

    Private Sub RemoveAlarmButton_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles RemoveAlarmButton.Click
        ' get global to add or Remove
        CType(Application.Current, App).AddRemove = 0

        NavigationService.Navigate(New Uri("/Views/ConfirmRemoveView.xaml", UriKind.Relative))
    End Sub

    Private Sub SaveChangesButton_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles SaveChangesButton.Click
        Dim editTString As String
        Dim addTString As String
        'If Remove is chosen here we arrived via back button
        'set back to edit
        If CType(Application.Current, App).AddRemove = 0 Then
            CType(Application.Current, App).AddRemove = 2
        End If

        If CType(Application.Current, App).AddRemove = 2 Then ' Check add or edit
            ' Save edit  
            'Set alarm number*** (CType(Application.Current, App).alist(CType(Application.Current, App).alarmSelected).AlarmNumber)
            CType(Application.Current, App).salarm.AlarmNumber = (CType(Application.Current, App).alist(CType(Application.Current, App).alarmSelected).AlarmNumber)

            'CType(Application.Current, App).salarm.AlarmDateTime = DateTime.Now
            ' CType(Application.Current, App).salarm.AlarmDateTime = Date.FromOADate(TimePicker.Value.Value.ToOADate)
            'trashes date portion 
            'CType(Application.Current, App).salarm.AlarmDateTime = DateTime.FromOADate(TimePicker.Value.Value.ToOADate)

            'DateTime.Parse("2009/02/26 18:37:58")
            editTString = DateTime.Parse((CType(Application.Current, App).alist(CType(Application.Current, App).alarmSelected).AlarmDateTime.Year.ToString) + "/" + _
                                         (CType(Application.Current, App).alist(CType(Application.Current, App).alarmSelected).AlarmDateTime.Month.ToString) + "/" + _
                                         (CType(Application.Current, App).alist(CType(Application.Current, App).alarmSelected).AlarmDateTime.Day.ToString) + " " + _
                TimePicker.Value.Value.Hour.ToString + ":" + TimePicker.Value.Value.Minute.ToString + ":" + TimePicker.Value.Value.Second.ToString)

            CType(Application.Current, App).salarm.AlarmDateTime = editTString

            MessageBox.Show("Current date =  " + DateTime.Now.ToString)
            MessageBox.Show("Current alarm date =  " + CType(Application.Current, App).salarm.AlarmDateTime.ToString)

            ' add +1 to day if alarm time < current time and days are equal (for 24hr timer)
            If DateTime.Now.Date = CType(Application.Current, App).salarm.AlarmDateTime.Date And
               DateTime.Now.TimeOfDay > CType(Application.Current, App).salarm.AlarmDateTime.TimeOfDay Then

                CType(Application.Current, App).salarm.AlarmDateTime = CType(Application.Current, App).salarm.AlarmDateTime.AddDays(1)

                ' else if next day and alarm time > current time (greater than 24 hr) subtract a day Add(-1)
                ' to bring back to previous day
            ElseIf (DateTime.Now.Month < CType(Application.Current, App).salarm.AlarmDateTime.Date.Month Or
                    DateTime.Now.Day < CType(Application.Current, App).salarm.AlarmDateTime.Date.Day) And
                     DateTime.Now.TimeOfDay < CType(Application.Current, App).salarm.AlarmDateTime.TimeOfDay Then

                CType(Application.Current, App).salarm.AlarmDateTime = CType(Application.Current, App).salarm.AlarmDateTime.AddDays(-1)
            End If

            MessageBox.Show("Edit alarm datetime =  " + CType(Application.Current, App).salarm.AlarmDateTime.ToString)

            'audio
            CType(Application.Current, App).salarm.AudibleAlarm = AudibleOnOffToggleButton.IsChecked
            CType(Application.Current, App).salarm.VolumeLevel = VolumeSlider.Value * 100
            CType(Application.Current, App).salarm.SoundNumber = SoundSelectionListBox.SelectedIndex + 1
            'snooze
            CType(Application.Current, App).salarm.Snooze = SnoozeToggleButton.IsChecked
            If SnoozeIntervalListBox.SelectedIndex = 0 Then
                CType(Application.Current, App).salarm.SnoozeInterval = 5
            ElseIf SnoozeIntervalListBox.SelectedIndex = 1 Then
                CType(Application.Current, App).salarm.SnoozeInterval = 10
            ElseIf SnoozeIntervalListBox.SelectedIndex = 2 Then
                CType(Application.Current, App).salarm.SnoozeInterval = 15
            ElseIf SnoozeIntervalListBox.SelectedIndex = 3 Then
                CType(Application.Current, App).salarm.SnoozeInterval = 20
            ElseIf SnoozeIntervalListBox.SelectedIndex = 4 Then
                CType(Application.Current, App).salarm.SnoozeInterval = 25
            ElseIf SnoozeIntervalListBox.SelectedIndex = 5 Then
                CType(Application.Current, App).salarm.SnoozeInterval = 30
                'repeats
            End If
            CType(Application.Current, App).salarm.Monday = MondayCheckBox.IsChecked
            CType(Application.Current, App).salarm.Tuesday = TuesdayCheckBox.IsChecked
            CType(Application.Current, App).salarm.Wednesday = WednesdayCheckBox.IsChecked
            CType(Application.Current, App).salarm.Thursday = ThursdayCheckBox.IsChecked
            CType(Application.Current, App).salarm.Friday = FridayCheckBox.IsChecked
            CType(Application.Current, App).salarm.Saturday = SaturdayCheckBox.IsChecked
            CType(Application.Current, App).salarm.Sunday = SundayCheckBox.IsChecked
            'visual
            CType(Application.Current, App).salarm.VisualAlarm = VisualToggleButton.IsChecked
            CType(Application.Current, App).salarm.Blinking = BlinkToggleButton.IsChecked
            CType(Application.Current, App).salarm.AlarmText = AlarmMessageTextBox.Text
            'vibrate-flash
            CType(Application.Current, App).salarm.Vibrate = VibrateToggleButton.IsChecked
            CType(Application.Current, App).salarm.Flashlight = FlashToggleButton.IsChecked

            CType(Application.Current, App).salarm.Color = "Red"
            CType(Application.Current, App).salarm.email1 = " "
            CType(Application.Current, App).salarm.email2 = " "
            CType(Application.Current, App).salarm.email3 = " "

            NavigationService.Navigate(New Uri("/Views/ConfirmView.xaml", UriKind.Relative))

        ElseIf CType(Application.Current, App).AddRemove = 1 Then 'add new

            'Set alarm number*** 
            CType(Application.Current, App).salarm.AlarmNumber = 0 ' is (re)set by server - is identity
            ' no date part 
            ' CType(Application.Current, App).salarm.AlarmDateTime = DateTime.FromOADate(TimePicker.Value.Value.ToOADate)

            addTString = DateTime.Parse((DateTime.Now.Year.ToString) + "/" + _
                                        (DateTime.Now.Month.ToString) + "/" + _
                          (DateTime.Now.Day.ToString) + " " + TimePicker.Value.Value.Hour.ToString + ":" + TimePicker.Value.Value.Minute.ToString + ":" + TimePicker.Value.Value.Second.ToString)

            CType(Application.Current, App).salarm.AlarmDateTime = addTString

            ' add +1 to day if alarm time < current time (for 24hr timer)
            If DateTime.Now.TimeOfDay > CType(Application.Current, App).salarm.AlarmDateTime.TimeOfDay Then
                CType(Application.Current, App).salarm.AlarmDateTime = CType(Application.Current, App).salarm.AlarmDateTime.AddDays(1)
            End If

            MessageBox.Show("Adding alarm datetime =  " + CType(Application.Current, App).salarm.AlarmDateTime.ToString)

            'audio
            CType(Application.Current, App).salarm.AudibleAlarm = AudibleOnOffToggleButton.IsChecked
            CType(Application.Current, App).salarm.VolumeLevel = VolumeSlider.Value * 100
            CType(Application.Current, App).salarm.SoundNumber = SoundSelectionListBox.SelectedIndex + 1
            'snooze
            CType(Application.Current, App).salarm.Snooze = SnoozeToggleButton.IsChecked
            If SnoozeIntervalListBox.SelectedIndex = 0 Then
                CType(Application.Current, App).salarm.SnoozeInterval = 5
            ElseIf SnoozeIntervalListBox.SelectedIndex = 1 Then
                CType(Application.Current, App).salarm.SnoozeInterval = 10
            ElseIf SnoozeIntervalListBox.SelectedIndex = 2 Then
                CType(Application.Current, App).salarm.SnoozeInterval = 15
            ElseIf SnoozeIntervalListBox.SelectedIndex = 3 Then
                CType(Application.Current, App).salarm.SnoozeInterval = 20
            ElseIf SnoozeIntervalListBox.SelectedIndex = 4 Then
                CType(Application.Current, App).salarm.SnoozeInterval = 25
            ElseIf SnoozeIntervalListBox.SelectedIndex = 5 Then
                CType(Application.Current, App).salarm.SnoozeInterval = 30
                'repeats
            End If
            CType(Application.Current, App).salarm.Monday = MondayCheckBox.IsChecked
            CType(Application.Current, App).salarm.Tuesday = TuesdayCheckBox.IsChecked
            CType(Application.Current, App).salarm.Wednesday = WednesdayCheckBox.IsChecked
            CType(Application.Current, App).salarm.Thursday = ThursdayCheckBox.IsChecked
            CType(Application.Current, App).salarm.Friday = FridayCheckBox.IsChecked
            CType(Application.Current, App).salarm.Saturday = SaturdayCheckBox.IsChecked
            CType(Application.Current, App).salarm.Sunday = SundayCheckBox.IsChecked
            'visual
            CType(Application.Current, App).salarm.VisualAlarm = VisualToggleButton.IsChecked
            CType(Application.Current, App).salarm.Blinking = BlinkToggleButton.IsChecked
            CType(Application.Current, App).salarm.AlarmText = AlarmMessageTextBox.Text
            'vibrate-flash
            CType(Application.Current, App).salarm.Vibrate = VibrateToggleButton.IsChecked
            CType(Application.Current, App).salarm.Flashlight = FlashToggleButton.IsChecked

            CType(Application.Current, App).salarm.Color = "Red"
            CType(Application.Current, App).salarm.email1 = " "
            CType(Application.Current, App).salarm.email2 = " "
            CType(Application.Current, App).salarm.email3 = " "

            NavigationService.Navigate(New Uri("/Views/ConfirmNewView.xaml", UriKind.Relative))
        End If

    End Sub
End Class
