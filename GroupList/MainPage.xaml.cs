//***********************************************************************
//
// Copyright (c) 2020 Microsoft Corporation. All rights reserved.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//**********************************************************************​
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using System.Diagnostics;
using Windows.UI.ViewManagement;
using GroupList.GroupList;
using MUXC = Microsoft.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GroupList
{
    /// <summary>
    /// The DominantView reflects which Pane of the TwoPaneView has 
    /// priority of display.  Main is Pane1, Display is Pane2 (Contact edit),
    /// and Shared reflects spanned status acrosss both screens.
    /// </summary>
    public enum DominantView { Main, Display, Shared}

    /// <summary>
    /// No view models on the simple MainPage.  Add data is displayed
    /// in UserControls.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {

        private bool _applicationIsSpanned = false;

        /// <summary>
        /// True if the application is spanned across two screens.  We may bind 
        /// this in the UI in future, so implement property change notification.
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

        private GridLength OneStarGridLength = new GridLength(1, GridUnitType.Star);
        private GridLength ZeroStarGridLength = new GridLength(0, GridUnitType.Star);

        public DominantView CurrentDominantView { get; set; }

        private bool applicationWasSpanned = false;

        public MainPage()
        {
            this.InitializeComponent();

            SizeChanged += MainPage_SizeChanged;

            MainView.ModeChanged += MainView_ModeChanged;

            Current = this;
        }

        private void MainView_ModeChanged(MUXC.TwoPaneView sender, object args)
        {
            switch (sender.Mode)
            {
                case MUXC.TwoPaneViewMode.SinglePane:
                    //
                    Debug.WriteLine("MainView_ModeChanged TwoPaneView Mode is SinglePane");

                    break;
                case MUXC.TwoPaneViewMode.Tall:
                    //
                    Debug.WriteLine("MainView_ModeChanged TwoPaneView Mode is Tall");
                    break;
                case MUXC.TwoPaneViewMode.Wide:
                    //
                    Debug.WriteLine("MainView_ModeChanged TwoPaneView Mode is Wide");
                    break;
                default:
                    //
                    break;
            }
        }

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
                    SetDisplayViewDominant();
                }
            }
        }

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

        public void SetDualPanes()
        {
            MainView.WideModeConfiguration = MUXC.TwoPaneViewWideModeConfiguration.LeftRight;
            MainView.TallModeConfiguration = MUXC.TwoPaneViewTallModeConfiguration.TopBottom;
        }

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
