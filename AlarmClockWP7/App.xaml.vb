﻿Imports AlarmClockWP7.AlarmServiceReference


Partial Public Class App
    Inherits Application

    Private addRemovePage As Integer = 1  ' 0 remove 1 add 2 edit
    Public alarm1 As Alarm = New Alarm   ', alarm2, alarm3, alarm4 As Alarm
    Public alarmSelected As Integer = 0
    Public name As String = ""
    Public nameIndex As Integer = -1

    Private _alarmL As AlarmList = New AlarmList

    'alist is list of alarms in database
    Public alist As List(Of AlarmDetail) = New List(Of AlarmDetail)
    Public alistOrdered As List(Of AlarmDetail) = New List(Of AlarmDetail)

    ' single alarm
    Public salarm As AlarmDetail = New AlarmDetail


    ''' <summary>
    ''' Provides easy access to the root frame of the Phone Application.
    ''' </summary>
    ''' <returns>The root frame of the Phone Application.</returns>
    Public Property RootFrame As PhoneApplicationFrame

    Public Property AddRemove() As Integer
        Get
            Return Me.addRemovePage
        End Get
        Set(value As Integer)
            Me.addRemovePage = value
        End Set
    End Property


    Public Property alarmL() As AlarmList
        Get
            Return Me._alarmL
        End Get
        Set(value As AlarmList)
            Me._alarmL = value
        End Set
    End Property

    ''' <summary>
    ''' Constructor for the Application object.
    ''' </summary>
    ''' 

    Public Sub New()
      
        CType(Application.Current, App).alarmL._aList.Add(New Alarm())
        CType(Application.Current, App).alarmL._aList.Last.AlarmTime = "07:30 AM"

        CType(Application.Current, App).alarmL._aList(0).AlarmTime = "06:30 AM"
        CType(Application.Current, App).alarmL._aList(1).AlarmTime = "08:23 PM"
        CType(Application.Current, App).alarmL._aList(2).AlarmTime = "05:12 AM"
        CType(Application.Current, App).alarmL._aList(3).AlarmTime = "04:35 AM"

        CType(Application.Current, App).alarmL._aList.Sort()

        ' Standard Silverlight initialization
        InitializeComponent()

        ' Phone-specific initialization
        InitializePhoneApplication()

        ' Show graphics profiling information while debugging.
        If System.Diagnostics.Debugger.IsAttached Then
            ' Display the current frame rate counters.
            Application.Current.Host.Settings.EnableFrameRateCounter = True

            ' Show the areas of the app that are being redrawn in each frame.
            'Application.Current.Host.Settings.EnableRedrawRegions = True

            ' Enable non-production analysis visualization mode, 
            ' which shows areas of a page that are handed off to GPU with a colored overlay.
            'Application.Current.Host.Settings.EnableCacheVisualization = True


            ' Disable the application idle detection by setting the UserIdleDetectionMode property of the
            ' application's PhoneApplicationService object to Disabled.
            ' Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
            ' and consume battery power when the user is not using the phone.
            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled
        End If

    End Sub

    ' Code to execute when the application is launching (eg, from Start)
    ' This code will not execute when the application is reactivated
    Private Sub Application_Launching(ByVal sender As Object, ByVal e As LaunchingEventArgs)
    End Sub

    ' Code to execute when the application is activated (brought to foreground)
    ' This code will not execute when the application is first launched
    Private Sub Application_Activated(ByVal sender As Object, ByVal e As ActivatedEventArgs)
    End Sub

    ' Code to execute when the application is deactivated (sent to background)
    ' This code will not execute when the application is closing
    Private Sub Application_Deactivated(ByVal sender As Object, ByVal e As DeactivatedEventArgs)
    End Sub

    ' Code to execute when the application is closing (eg, user hit Back)
    ' This code will not execute when the application is deactivated
    Private Sub Application_Closing(ByVal sender As Object, ByVal e As ClosingEventArgs)
    End Sub

    ' Code to execute if a navigation fails
    Private Sub RootFrame_NavigationFailed(ByVal sender As Object, ByVal e As NavigationFailedEventArgs)
        If Diagnostics.Debugger.IsAttached Then
            ' A navigation has failed; break into the debugger
            Diagnostics.Debugger.Break()
        End If
    End Sub

    Public Sub Application_UnhandledException(ByVal sender As Object, ByVal e As ApplicationUnhandledExceptionEventArgs) Handles Me.UnhandledException

        ' Show graphics profiling information while debugging.
        If Diagnostics.Debugger.IsAttached Then
            Diagnostics.Debugger.Break()
        Else
            e.Handled = True
            MessageBox.Show(e.ExceptionObject.Message & Environment.NewLine & e.ExceptionObject.StackTrace,
                            "Error", MessageBoxButton.OK)
        End If
    End Sub

    Public Property GlobalMediaElement As MediaElement
        Get
            Return CType(App.Current.Resources("AppMediaElement"), MediaElement)
        End Get
        Set(value As MediaElement)

        End Set
    End Property

    Private Sub repeatMedia(sender As System.Object, e As System.Windows.RoutedEventArgs)
        CType(App.Current.Resources("AppMediaElement"), MediaElement).Play()
    End Sub

#Region "Phone application initialization"
    ' Avoid double-initialization
    Private phoneApplicationInitialized As Boolean = False

    ' Do not add any additional code to this method
    Private Sub InitializePhoneApplication()
        If phoneApplicationInitialized Then
            Return
        End If

        ' Create the frame but don't set it as RootVisual yet; this allows the splash
        ' screen to remain active until the application is ready to render.
        RootFrame = New PhoneApplicationFrame()
        AddHandler RootFrame.Navigated, AddressOf CompleteInitializePhoneApplication

        ' Handle navigation failures
        AddHandler RootFrame.NavigationFailed, AddressOf RootFrame_NavigationFailed

        ' Ensure we don't initialize again
        phoneApplicationInitialized = True
    End Sub

    ' Do not add any additional code to this method
    Private Sub CompleteInitializePhoneApplication(ByVal sender As Object, ByVal e As NavigationEventArgs)
        ' Set the root visual to allow the application to render
        If RootVisual IsNot RootFrame Then
            RootVisual = RootFrame
        End If

        ' Remove this handler since it is no longer needed
        RemoveHandler RootFrame.Navigated, AddressOf CompleteInitializePhoneApplication
    End Sub
#End Region

End Class