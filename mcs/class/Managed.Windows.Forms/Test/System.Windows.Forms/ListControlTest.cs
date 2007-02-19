//
// ListControlTest.cs: Tests for ListControl abstract class.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// Copyright (c) 2006 Novell, Inc. (http://www.novell.com)
//
// Authors:
//	Carlos Alberto Cortez <calberto.cortez@gmail.com>
//

using System;
using System.Collections;
using System.IO;
using System.Data;
using System.Windows.Forms;
using NUnit.Framework;

namespace MonoTests.System.Windows.Forms
{
	[TestFixture]
	public class ListControlTest
	{
		[Test]
		// Bug 80794
		public void DataBindingsTest ()
		{
			string table =
@"<?xml version=""1.0"" standalone=""yes""?>
<DOK>
<DOK>
<klient>287</klient>
</DOK>
</DOK>
";
			string lookup =
@"<?xml version=""1.0"" standalone=""yes""?>
<klient>
<klient>
<nimi>FAILED</nimi>
<kood>316</kood>
</klient>
<klient>
<nimi>SUCCESS</nimi>
<kood>287</kood>
</klient>
</klient>";

			using (Form frm = new Form ()) {
				frm.ShowInTaskbar = false;
				DataSet dsTable = new DataSet ();
				dsTable.ReadXml (new StringReader (table));
				DataSet dsLookup = new DataSet ();
				dsLookup.ReadXml (new StringReader (lookup));
				ComboBox cb = new ComboBox ();
				cb.DataSource = dsLookup.Tables [0];
				cb.DisplayMember = "nimi";
				cb.ValueMember = "kood";
				cb.DataBindings.Add ("SelectedValue", dsTable.Tables [0], "klient");
				frm.Controls.Add (cb);
				Assert.AreEqual ("", cb.Text, "#01");
				frm.Show ();
				Assert.AreEqual ("SUCCESS", cb.Text, "#02");
			}
		}
		
		[Test]
		public void DisplayMemberNullTest ()
		{
			ListControlChild lc = new ListControlChild ();
			lc.DisplayMember = null;
			Assert.AreEqual (String.Empty, lc.DisplayMember, "#1");
		}

		[Test]
		[ExpectedException (typeof (Exception))]
		public void DataSourceWrongArgumentType ()
		{
			ListControlChild lc = new ListControlChild ();
			lc.DataSource = new object ();
		}

#if NET_2_0
		[Test]
		public void AllowSelection ()
		{
			ListControlChild lc = new ListControlChild ();
			Assert.IsTrue (lc.allow_selection);
		}
#endif
	}

	public class ListControlChild : ListControl
	{
		public override int SelectedIndex {
			get {
				return -1;
			}
			set {
			}
		}

#if NET_2_0
		public bool allow_selection {
			get { return base.AllowSelection; }
		}
#endif

		protected override void RefreshItem (int index)
		{
		}

		protected override void SetItemsCore (IList items)
		{
		}
	}
}

