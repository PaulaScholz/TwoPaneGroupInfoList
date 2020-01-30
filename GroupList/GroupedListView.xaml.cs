﻿using System;
using System.Collections.Generic;
using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GroupList.Model;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace GroupList.GroupList
{
    public sealed partial class GroupedListView : UserControl
    {
        public Contact SelectedContact { get; set; }

#pragma warning disable CA2211 // Non-constant fields should not be visible
        public static GroupedListView Current = null;
#pragma warning restore CA2211 // Non-constant fields should not be visible

        public GroupedListView()
        {
            this.InitializeComponent();

            // give others access to us
            Current = this;

            // Sets the CollectionViewSource to an ObservableCollection of GroupInfoList objects
            // containing Contacts generated randomly.
            ContactsCVS.Source = Contact.GetContactsGroupedAllAlpha(200);

            ContactsCVS.View.CurrentChanged += DirectoryView_CurrentChanged;
        }

        private void DirectoryView_CurrentChanged(object sender, object e)
        {
            SelectedContact = ContactsCVS.View.CurrentItem as Contact;

            DisplayView.Current?.SetCurrentContact(SelectedContact);

            MainPage.Current.SetDisplayViewDominant();
        }

        /// <summary>
        /// Click handler, toggles the SemanticZoom control's ZoomedInView or ZoomedOutView.  This toggling can also be
        /// done by clicking on the ZoomedInView's header for each row in its GridView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZoomInOutBtn_Click(object sender, RoutedEventArgs e)
        {
            ZoomControl.IsZoomedInViewActive = !ZoomControl.IsZoomedInViewActive;
        }
    }
}
