Imports System.Windows.Threading
Imports System.Linq

Partial Public Class Page1
    Inherits PhoneApplicationPage

    Public Sub New()
        InitializeComponent()
        PageTitle.Text = Date.Now.ToShortTimeString
        DateBlock.Text = Date.Now.ToShortDateString

        Dim timer As DispatcherTimer = New DispatcherTimer

        'DispatcherTimer setup
        AddHandler timer.Tick, AddressOf dispatcherTimer_Tick
        timer.Interval = New TimeSpan(0, 0, 1)
        timer.Start()

        AlarmsListBox.ItemsSource = CType(Application.Current, App).alist
        ' AlarmsListBox.ItemsSource = CType(Application.Current, App).alist
        '  MessageBox.Show("Alist count" + CType(Application.Current, App).alist.Count.ToString)
    End Sub

    '  System.Windows.Threading.DispatcherTimer.Tick handler
    '  Updates the current seconds display 
    Private Sub dispatcherTimer_Tick(ByVal sender As Object, ByVal e As EventArgs)
        PageTitle.Text = Date.Now.ToShortTimeString
        DateBlock.Text = Date.Now.ToShortDateString
    End Sub

    Private Sub PhoneApplicationPage_OrientationChanged(sender As System.Object, e As Microsoft.Phone.Controls.OrientationChangedEventArgs) Handles MyBase.OrientationChanged
        If Orientation = PageOrientation.Landscape Or Orientation = PageOrientation.LandscapeLeft Or Orientation = PageOrientation.LandscapeRight Then
            ' Set Alarm list box so scroll works on landscape
            AlarmsListBox.Height = 200
        Else 'Set Alarm list box so scroll works on Portrait
            AlarmsListBox.Height = 400
        End If
    End Sub

    Private Sub AlarmsListBox_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles AlarmsListBox.SelectionChanged
        'Handle view alarm details 
        ' selectedIndex equals alarm index number 0 - N
        MessageBox.Show(AlarmsListBox.SelectedIndex)
        CType(Application.Current, App).alarmSelected = AlarmsListBox.SelectedIndex

        If (AlarmsListBox.SelectedIndex <> -1) Then  'Not page back()
            NavigationService.Navigate(New Uri("/Views/DetailsPivotPage.xaml", UriKind.Relative))
        End If
    End Sub

    Private Sub PhoneApplicationPage_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        ' Bind listbox item here
        '***************** CType(Application.Current, App).AddRemove = 2 'edit

        AlarmsListBox.ItemsSource = CType(Application.Current, App).alist
        AlarmAtBlock.Text = (CType(Application.Current, App).name + " - Alarm at:")

        ' CType(Application.Current, App).alarmL._aList.Sort()
        ' AlarmsListBox.ItemsSource = CType(Application.Current, App).alarmL._aList
        ' For Each item In alarmL._aList
        '   MessageBox.Show(item.AlarmTime)
        ' Next

    End Sub
End Class
