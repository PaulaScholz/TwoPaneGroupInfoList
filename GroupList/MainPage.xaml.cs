using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using System.Diagnostics;
using Windows.UI.ViewManagement;
using GroupList.GroupList;
using Windows.System;
using System.Text.RegularExpressions;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GroupList
{


    public enum DominantView { Main, Display, Shared}

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        /// <summary>
        /// The beginning width of TwoPaneView element, MainView
        /// </summary>
        private double BeginningWidth { get; set; }

        /// <summary>
        /// The beginning height of TwoPaneView element, MainView
        /// </summary>
        private double BeginningHeight { get; set; }

        /// <summary>
        /// The current ApplicationViewOrientation of the MainView (Portrait or Landscape)
        /// </summary>
        public ApplicationViewOrientation CurrentOrientation { get; set; }

        private bool _applicationIsSpanned = false;

        /// <summary>
        /// True if the application is spanned across two screens.  We're going
        /// to bind this in the UI, so implement property change notification.
        /// </summary>
        public bool ApplicationIsSpanned 
        { 
            get { return _applicationIsSpanned; }
            set { 
                    Set(ref _applicationIsSpanned, value);
                    OnPropertyChanged(nameof(ApplicationNotSpanned));
                }
        }

        public bool ApplicationNotSpanned
        {
            get { return !_applicationIsSpanned; }
        }

#pragma warning disable CA2211 // Non-constant fields should not be visible
        public static MainPage Current = null;
#pragma warning restore CA2211 // Non-constant fields should not be visible

        public GridLength Pane1DominantWidth { get; set; }
        public GridLength Pane1SharedWidth { get; set; }
        public GridLength Pane2DominantWidth { get; set; }
        public GridLength Pane2SharedWidth { get; set; }

        public DominantView CurrentDominantView { get; set; }

        public bool OrientationChanged { get; set; }

        private bool applicationWasSpanned = false;

        public MainPage()
        {
            this.InitializeComponent();

            Pane1DominantWidth = new GridLength(1, GridUnitType.Star);
            Pane1SharedWidth = new GridLength(0.5, GridUnitType.Star);
            Pane2DominantWidth = new GridLength(0, GridUnitType.Star);
            Pane2SharedWidth = new GridLength(0.5, GridUnitType.Star);

            Loaded += MainPage_Loaded;

            SizeChanged += MainPage_SizeChanged;

            Current = this;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Get the initial size of the TwoPaneView element
            BeginningWidth = MainView.ActualWidth;
            BeginningHeight = MainView.ActualHeight;

        }

        private void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {           
            ApplicationViewOrientation oldOrientation = CurrentOrientation;            

            // this computes the current orientation and spanned status
            QueryOrientation();

            if(GroupedListView.Current.SelectedContact != null)
            {
                // set the current contact in the display view
                DisplayView.Current.SetCurrentContact(GroupedListView.Current.SelectedContact);
            }

            // if we're spanned and we have a current contact
            if (ApplicationIsSpanned && GroupedListView.Current.SelectedContact != null)
            {
                // set the flag so we know we were spanned
                applicationWasSpanned = true;

                // set the main view to have Pane1 and Pane2 lengths equal so they span
                SetMainViewShared();
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
                    SetDisplayViewDominant();
                }
            }
        }

        /// <summary>
        /// We compute the orientation and spanned status of the TwoPaneView here.
        /// Results update the properties "ApplicationIsSpanned" and "CurrentOrientation"
        /// </summary>
        private void QueryOrientation()
        {
            // based on initial conditions and current conditions, compute orientation
            double currentWidth = MainView.ActualWidth;
            double currentHeight = MainView.ActualHeight;

            // this runs before MainPage_Loaded, when BeginningWidth & BeginningHeight are zero
            if (BeginningWidth != 0)
            {
                if(BackButton.Visibility == Visibility.Collapsed)
                {
                    double buttonHeight = BackButton.Height;

                    // if the BackButton is collapsed, take its height into account
                    if (currentWidth == BeginningWidth && currentHeight == BeginningHeight + buttonHeight)
                    {
                        // if width and height are equal to the beginning width & height, we're not spanned
                        ApplicationIsSpanned = false;
                    }
                    else if (currentWidth >= BeginningWidth && currentHeight >= BeginningHeight + buttonHeight)
                    {
                        // it is possible that currentWidth AND currentHeight might be equal to 
                        // their beginning values but that is caught by the clause above.  If only
                        // one of them is equal, then this clause is executed instead.
                        ApplicationIsSpanned = true;
                    }
                    else
                    {
                        ApplicationIsSpanned = false;
                    }
                }
                else
                {
                    if (currentWidth == BeginningWidth && currentHeight == BeginningHeight)
                    {
                        // if width and height are equal to the beginning width & height, we're not spanned
                        ApplicationIsSpanned = false;
                    }
                    else if (currentWidth >= BeginningWidth && currentHeight >= BeginningHeight)
                    {
                        // it is possible that currentWidth AND currentHeight might be equal to 
                        // their beginning values but that is caught by the clause above.  If only
                        // one of them is equal, then this clause is executed instead.
                        ApplicationIsSpanned = true;
                    }
                    else
                    {
                        ApplicationIsSpanned = false;
                    }
                }


            }
            else
            {
                // Application always starts as not spanned
                ApplicationIsSpanned = false;
            }

            // compute our orientation because we can't rely on DisplayInformation or ApplicationView values
            if (currentWidth > currentHeight && !ApplicationIsSpanned)
            {
                CurrentOrientation = ApplicationViewOrientation.Landscape;
            }
            else if (currentWidth < currentHeight && !ApplicationIsSpanned)
            {
                CurrentOrientation = ApplicationViewOrientation.Portrait;
            }
            else if (ApplicationIsSpanned && currentWidth > currentHeight)
            {
                CurrentOrientation = ApplicationViewOrientation.Portrait;
            }
            else
            {
                CurrentOrientation = ApplicationViewOrientation.Landscape;
            }
        }

        public void SetMainViewDominant()
        {
            MainView.Pane1Length = Pane1DominantWidth;
            MainView.Pane2Length = Pane2DominantWidth;
            CurrentDominantView = DominantView.Main;

            BackButton.IsEnabled = false;
        }

        public void SetMainViewShared()
        {
            MainView.Pane1Length = Pane1SharedWidth;
            MainView.Pane2Length = Pane2SharedWidth;
            CurrentDominantView = DominantView.Shared;
        }

        public void SetDisplayViewDominant()
        {
            MainView.Pane1Length = Pane2DominantWidth;
            MainView.Pane2Length = Pane1DominantWidth;
            CurrentDominantView = DominantView.Display;

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

        #region INotifiyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion
    }
}
