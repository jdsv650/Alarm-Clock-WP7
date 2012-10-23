Partial Public Class PopupCtrl
    Inherits UserControl

    Public Sub New()
        InitializeComponent()
    End Sub


    Private Sub NoButton_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles NoButton.Click
        Me.Visibility = Windows.Visibility.Collapsed
    End Sub

    Private Sub YesButton_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles YesButton.Click
        Me.Visibility = Windows.Visibility.Collapsed
    End Sub

End Class
