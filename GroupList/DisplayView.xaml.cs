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
using Windows.UI.Xaml.Controls;
using GroupList.Model;
using GroupList.GroupList;

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

            if(MainPage.Current.ApplicationIsSpanned)
            {
                MainPage.Current.SetDualPanes();
            }

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

            GroupedListView.Current.ResetCurrentSelection();
        }

        private void ContactSaveButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ResetContactForm();

            MainPage.Current.SetMainViewDominant();

            GroupedListView.Current.ResetCurrentSelection();
        }
    }
}
