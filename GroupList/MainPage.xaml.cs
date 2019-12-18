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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GroupList
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {


#pragma warning disable CA2211 // Non-constant fields should not be visible
        public static MainPage Current = null;
#pragma warning restore CA2211 // Non-constant fields should not be visible

        public GridLength Pane1DominantWidth { get; set; }
        public GridLength Pane1SharedWidth { get; set; }
        public GridLength Pane2DominantWidth { get; set; }
        public GridLength Pane2SharedWidth { get; set; }


        public MainPage()
        {
            this.InitializeComponent();

            Pane1DominantWidth = new GridLength(1, GridUnitType.Star);
            Pane1SharedWidth = new GridLength(0.5, GridUnitType.Star);
            Pane2DominantWidth = new GridLength(0, GridUnitType.Star);
            Pane2SharedWidth = new GridLength(0.5, GridUnitType.Star);

            Current = this;
        }

        public void SetMainViewDominant()
        {
            MainView.Pane1Length = Pane1DominantWidth;
            MainView.Pane2Length = Pane2DominantWidth;
        }

        public void SetMainViewShared()
        {
            MainView.Pane1Length = Pane1SharedWidth;
            MainView.Pane2Length = Pane2SharedWidth;
        }

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
    }
}
