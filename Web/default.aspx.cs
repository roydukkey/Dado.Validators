//---------------------------------------------------------------------------------
// Dado Validators, Copyright 2012 roydukkey, 2012-05-24 (Tue, 24 July 2012).
// Dual licensed under the MIT (http://www.roydukkey.com/mit) and
// GPL Version 2 (http://www.roydukkey.com/gpl) licenses.
//---------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web
{
	public partial class Default : System.Web.UI.Page
	{
		protected void Page_Init(object sender, EventArgs e)
		{

			btnSubmit.Click += (a, b) =>
			{
				Page.Validate("vlgSubmit");
				if (Page.IsValid) {

				}
			};

		}
	}
}