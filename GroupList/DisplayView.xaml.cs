using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using GroupList.Model;
// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace GroupList
{
    public sealed partial class DisplayView : UserControl
    {
#pragma warning disable CA2211 // Non-constant fields should not be visible
        public static DisplayView Current;
#pragma warning restore CA2211 // Non-constant fields should not be visible

        public Contact CurrentContact { get; set; }

        public DisplayView()
        {
            this.InitializeComponent();

            // give others access to us
            Current = this;
        }

        public void SetCurrentContact(Contact aContact)
        {
            CurrentContact = aContact;

            EditCurrentContact();
        }

        public void EditCurrentContact()
        {
            ResetContactForm();

            FirstName.Text = CurrentContact.FirstName;
            LastName.Text = CurrentContact.LastName;
            PositionTB.Text = CurrentContact.Position;
            PhoneNumberTB.Text = CurrentContact.PhoneNumber;
            BiographyTB.Text = CurrentContact.Biography;
        }

        private void ResetContactForm()
        {
            // not string.Empty
            FirstName.Text = "";
            LastName.Text = "";
            PositionTB.Text = "";
            PhoneNumberTB.Text = "";
            BiographyTB.Text = "";
        }

        private void ContactCancelButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ResetContactForm();

            MainPage.Current.SetMainViewDominant();
        }

        private void ContactSaveButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ResetContactForm();

            //Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            MainPage.Current.SetMainViewDominant();
        }
    }
}
