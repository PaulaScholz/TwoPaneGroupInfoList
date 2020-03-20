# TwoPaneGroupInfoList

## Windows Developer Incubation and Learning - Paula Scholz

## Introduction

![TwoPaneGroupInfoList](ReadmeImages/TwoPaneGroupInfoList.gif)

A Universal Windows Platform sample that illustrates the [TwoPaneView](https://docs.microsoft.com/en-us/windows/uwp/design/controls-and-patterns/two-pane-view) layout control for [Dual-Screen experiences](https://docs.microsoft.com/en-us/dual-screen/introduction), using a randomly-generated Contact list presented in groups with the [SemanticZoom](https://docs.microsoft.com/en-us/windows/uwp/design/controls-and-patterns/semantic-zoom) control.

[TwoPaneView](https://docs.microsoft.com/en-us/windows/uwp/design/controls-and-patterns/two-pane-view) provides two distinct areas of content that can be spanned onto separate screens on dual-screen devices like the [Surface Neo](https://www.microsoft.com/en-us/surface/devices/surface-neo?&OCID=AID2000022_SEM_oCeJqLSf&msclkid=41672d2d892e1554df52734a51ae580b). `TwoPaneView` is the primary layout panel used to support dual-screen development for UWP applications.

While `TwoPaneView` is part of the Windows SDK, Microsoft recommends you use the version inside the [Windows UI](https://microsoft.github.io/microsoft-ui-xaml/) library, which provides updated versions of existing Windows platform controls.

## System Requirements

At the time of writing (March, 2020), development for dual-screen experiences requires
[Windows 10 version 10.0.18362](https://docs.microsoft.com/en-us/windows/uwp/whats-new/windows-10-build-18362) or better, along with its companion [Windows SDK](https://developer.microsoft.com/en-US/windows/downloads/windows-10-sdk/).  You also need [Windows UI](https://microsoft.github.io/microsoft-ui-xaml/) `version 2.4.0-prerelease-200203002` or better, available for installation through NuGet.

## Development Tools

Please refer to the [Windows 10x development tools](https://docs.microsoft.com/en-us/dual-screen/windows/get-dev-tools) guide.

You will need the latest version of [Visual Studio 2019](https://visualstudio.microsoft.com/downloads/). This sample was built with Visual Studio 2019 16.5.

You will also need the [Microsoft Emulator with the Windows 10x Emulator Image](https://blogs.windows.com/windowsdeveloper/2020/03/10/microsoft-emulator-and-windows-10x-emulator-image-preview-build-19578-available-now/) from the Microsoft Store, so that you may run this sample without an actual dual-screen device. 

The sample may also be run on desktop `Windows version 10.0.18362` or better without an emulator. In this case, it will appear as if on a single screen device.

## Using the Windows UI Library

To use the [Windows UI library](https://microsoft.github.io/microsoft-ui-xaml/) inside your dual-screen application, you must first install it from NuGet.  Right-click on your project file in the Visual Studio Solution Explorer to launch the NuGet tool.

Make sure you include the prerelease packages by selecting the checkbox.  You will need version `2.4.0-prerelease.20203002` or better.

Then, to use Windows UI Library controls rather than SDK controls, you will need to place a XamlControlsResources reference in your `App.xaml` Resource Dictionary.

For more details, please refer to the [Getting started with the Windows UI Library](https://docs.microsoft.com/en-us/uwp/toolkits/winui/getting-started) guide.


## Configuring your Emulator

You may need to configure your Win10x Emulator image.  Please refer to the [release notes](https://docs.microsoft.com/en-us/dual-screen/windows/release-notes#emulator-app) for the Windows 10x development tools.


## Debugging
You should follow this [guidance](https://docs.microsoft.com/en-us/dual-screen/windows/use-emulator#visual-studio-2019-preview) for debugging your Windows 10x UWP dual-screen apps.  

## Visual Studio Solution

The Visual Studio Solution is shown below:

![VisualStudioSolution](ReadmeImages/VisualStudioSolution.png)

`MainPage.xaml` contains the single Page and `TwoPaneView` control and serves as the core of the project.  

Inside each pane are UserControls, one for displaying the generated Contacts list on the `MainView` pane (Pane1), called `GroupedListView`, and another for the Contact edit form, hosted on the `DisplayView` pane (Pane2). These UserControls are implemented in `GroupedListView.xaml` and `DisplayView.xaml`, along with their respective code-behind files.

In the `Contact.cs` file we generate both the Contact list and its respective alphabetical groups used to populate a `CollectionViewSource` object bound to an `ObservableCollection<GroupInfoList>` in `GroupedListView`.

In the `GroupList` folder we have two classes used to support the [SemanticZoom](https://docs.microsoft.com/en-us/windows/uwp/design/controls-and-patterns/semantic-zoom) control in `GroupedListView` which provides an index to the alphabetic groups of the Contact list.

## TwoPaneView

The primary display layout panel for our application is [TwoPaneView](https://docs.microsoft.com/en-us/windows/uwp/design/controls-and-patterns/two-pane-view). This control provides separate display surfaces for each screen when the application is `spanned` across screens, and when the application is hosted on a single screen as it does at application launch, its `PanePriority` and `Mode` properties are used to determine which Pane is displayed on the single screen.

![TwoPaneView Panes](ReadmeImages/TwoPaneShell.png)

The `MainView` Pane1 contains the `GroupedInfoList` UserControl and the `DisplayView` Pane2 contains the Contact edit form UserControl.

`MainPage.xaml` is the single Page in our solution and looks like this:

```xaml
<Page
    x:Class="GroupList.MainPage"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:local="using:GroupList.GroupList"
    xmlns:contact="using:GroupList"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:controls="using:Microsoft.UI.Xaml.Controls"
    mc:Ignorable="d"
    Background="{ThemeResource ApplicationPageBackgroundThemeBrush}">

    <Grid Background="{ThemeResource SystemControlPageBackgroundChromeLowBrush}">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
        </Grid.RowDefinitions>
        
        <!-- This is the BackButton -->
        <Button Style="{StaticResource NavigationBackButtonNormalStyle}" x:Name="BackButton" Click="BackButton_Click" 
                IsEnabled="False" Visibility="{x:Bind ApplicationNotSpanned, Mode=OneWay}"/>
        
        <!-- TwoPaneView allows screen spanning and display of panes under software control.  Here, we use UserControls
              to host the actual content of the panes.    -->
        <controls:TwoPaneView x:Name="MainView"
                 Grid.Row="1"
                 Pane1Length="1*"
                 Pane2Length="0*"
                 PanePriority="Pane1"
                 MinTallModeHeight="641"
                 MinWideModeWidth="641"
                 TallModeConfiguration="TopBottom"
                 WideModeConfiguration="LeftRight">
            <controls:TwoPaneView.Pane1>
                <local:GroupedListView />
            </controls:TwoPaneView.Pane1>
            <controls:TwoPaneView.Pane2>
                <contact:DisplayView />
            </controls:TwoPaneView.Pane2>
        </controls:TwoPaneView>
    </Grid>
</Page>
```
This Xaml sets up a two-row `Grid`.  The first row contains a navigation back button, active when Pane 2 (DisplayView) is the dominant pane and the display is not spanned (single-screen).  The second row contains our [TwoPaneView](https://docs.microsoft.com/en-us/uwp/api/microsoft.ui.xaml.controls.twopaneview?view=winui-2.4) control.

Note that each pane of the `TwoPaneView` is populated solely by a `UserControl`.  In Pane1, we have the `GroupedListView` used to create and display `Contact` objects in a `GridView`, and in Pane2 we have a Contact edit form used to display and edit individual Contact records.  Contact changes are not persisted to the Contact list as this is beyond the scope of the sample.  Contacts are generated randomly each time the application runs.

We need to handle several application states inside `MainPage.xaml.cs` to reflect orientation changes and respond to user events.  These take place in the `MainPage_SizedChanged` event handler, which is fired when rotation or application spanning occurs.  Let's examine this method:

```csharp
        /// <summary>
        /// Fired when a rotation or spanning occurs. Dual-Screen experience windows
        /// are either maximized or minimized, there is no intermediate position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {           
           
            switch(ApplicationView.GetForCurrentView().ViewMode)
            {
                case ApplicationViewMode.Spanning:
                    //
                    Debug.WriteLine("MainPage_SizeChanged View Mode is ApplicationViewMode.Spanning");
                    ApplicationIsSpanned = !ApplicationIsSpanned;
                    break;
                case ApplicationViewMode.Default:
                    //
                    Debug.WriteLine("MainPage_SizeChanged View Mode is ApplicationViewMode.Default");
                    ApplicationIsSpanned = false;
                    break;
                case ApplicationViewMode.CompactOverlay:
                    //
                    Debug.WriteLine("MainPage_SizeChanged View Mode is ApplicationViewMode.CompactOverlay");
                    break;
                default:
                    //
                    break;
            }

            if (GroupedListView.Current.SelectedContact != null)
            {
                // set the current contact in the display view
                DisplayView.Current.SetCurrentContact(GroupedListView.Current.SelectedContact);
            }

            // if we're spanned and we have a current contact
            // if the application is spanned, it doesn't matter what the Pane1Length or Pane2Length is
            if (ApplicationIsSpanned && GroupedListView.Current.SelectedContact != null)
            {
                // set the flag so we know we were spanned
                applicationWasSpanned = true;
            }
            else if (!ApplicationIsSpanned && GroupedListView.Current.SelectedContact != null)  // not spanned and have a current contact
            {
                // if we were spanned and are now not
                if(applicationWasSpanned)
                {
                    // We want to see the DisplayView dominant
                    SetDisplayViewDominant();

                    applicationWasSpanned = false;
                }
                else if (CurrentDominantView == DominantView.Main)
                {
                    // if we weren't spanned, and still are not, set GroupInfoList dominant
                    SetMainViewDominant();
                } 
                else
                {
                    // set the DisplayView Contact edit form dominant
                    SetDisplayViewDominant();
                }
            }
        }
        
```
The switch statement determines the spanned status of the application.  We do this by examing the `ApplicationViewMode` value of the current`ApplicationView`. Note that you need to be using the preview `WinUI` library to have the `Spanning` enum value.

There is no way to query the actual application spanned status. Instead we know the application starts in an unspanned default mode and we keep track of changes through our `ApplicationIsSpanned` boolean property each time `MainPage_SizeChanged` is fired.

Then, if we have a currently selected contact in the `GroupedListView`, we call the `DisplayView.Current.SetCurrentContact` method, which will change the `TwoPaneView.WideModeConfiguration` and `TwoPaneView.TallModeConfiguration` to `LeftRight` and `TopBotton`, by calling `MainPage.SetDualPanes()` through the static `Current` instance variable.

```csharp
        /// <summary>
        /// Set us up as single-pane and disable the back button.
        /// </summary>
        public void SetMainViewDominant()
        {
            MainView.Pane1Length = OneStarGridLength;
            MainView.Pane2Length = ZeroStarGridLength;
            MainView.PanePriority = MUXC.TwoPaneViewPriority.Pane1;
            MainView.WideModeConfiguration = MUXC.TwoPaneViewWideModeConfiguration.SinglePane;
            MainView.TallModeConfiguration = MUXC.TwoPaneViewTallModeConfiguration.SinglePane;
            CurrentDominantView = DominantView.Main;

            BackButton.IsEnabled = false;
        }

        /// <summary>
        /// Set up as spanned across screens.
        /// </summary>
        public void SetDualPanes()
        {
            MainView.WideModeConfiguration = MUXC.TwoPaneViewWideModeConfiguration.LeftRight;
            MainView.TallModeConfiguration = MUXC.TwoPaneViewTallModeConfiguration.TopBottom;
        }

        /// <summary>
        /// Set the Contact Edit form dominant on a single screen and enable the back button.
        /// </summary>
        public void SetDisplayViewDominant()
        {
            MainView.Pane1Length = ZeroStarGridLength;
            MainView.Pane2Length = OneStarGridLength;
            MainView.PanePriority = MUXC.TwoPaneViewPriority.Pane2;
            CurrentDominantView = DominantView.Display;
            MainView.WideModeConfiguration = MUXC.TwoPaneViewWideModeConfiguration.LeftRight;
            MainView.TallModeConfiguration = MUXC.TwoPaneViewTallModeConfiguration.TopBottom;

            BackButton.IsEnabled = true;
        }

        /// <summary>
        /// User pressed the BackButton, so change the dominant pane to the contact list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            SetMainViewDominant();
        }
```
If our application is in a Spanned state, we want to display both panes.  If we have a current contact, the Contact edit form will be displayed in Pane 2 on `DisplayView.xmal` and if we do not, then the `GridView` containing the Contact list will be displayed on both panes and both the `WideModeConfiguration` and `TallModeConfiguration` of the `MainView` will be set to a `SinglePane` configuration.

## GroupedInfoList and SemanticZoom

The Contact list created in GroupInfoList is assigned to a [CollectionViewSource](https://docs.microsoft.com/en-us/uwp/api/Windows.UI.Xaml.Data.CollectionViewSource) that provides grouping and current-item support to its assigned `ObservableCollection<GroupInfoList>`.  

Inside the `Documentation` folder of this repository, you will find a `semanticzoom_web.htm` file that provides a comprehensive explanation of the `Contact`, `GroupInfoList`, `EmptyOrFullSelector` and `GroupedListView` classes.  You may also view this [documentation](https://swifter.github.io/GroupInfoList/semanticzoom_web.htm) online at my personal GitHub repository.

Paula Scholz<br>
March 16, 2020<br>
Windows Developer Incubation and Learning









