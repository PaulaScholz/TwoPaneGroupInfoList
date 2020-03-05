﻿//***********************************************************************
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
using System.Collections.Generic;

namespace GroupList.Model
{
	/// <summary>
	/// This generic GroupInfoList object represents a List<T> of Contact objects associated with a particular Key, in this case, a
	/// letter of the alphabet (string).  This class is used in Contacts.cs when generating the list of Contact objects to be
	/// displayed in the demo, in either Contact.GetContactsGrouped(int) or Contact.GetContactsGroupedAllAlpha(int).  The GroupInfoList
	/// is used by the EmptyOrFullSelector of the SemanticZoom control's ZoomedOutView to enable clicking on a letter, and is
	/// used by the ZoomedInView to provide the Contact objects for a letter, and to provide the letter to use in the ZoomedInView's
	/// header row for each group.
	/// </summary>
    public class GroupInfoList : List<object>
    {
		/// <summary>
		/// The Key represents a letter of the alphabet.  So, what we really have in this GroupInfoList is a list of Contact objects
		/// organized by the first letter of the LastName property of a Contact.
		/// </summary>
        public object Key { get; set; }
    }
}
