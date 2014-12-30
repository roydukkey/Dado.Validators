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
					litTest.Text += "<p>";
					litTest.Text += "vldPhoneNumber.ValidatedPhoneNumber: " + vldPhoneNumber.ValidatedPhoneNumber;
					litTest.Text += "<br />";
					litTest.Text += "vldPhoneNumber.ValidatedExtension: " + vldPhoneNumber.ValidatedExtension;
					litTest.Text += "<br />";
					litTest.Text += "cblCheckBoxList.SelectedValue: " + cblCheckBoxList.SelectedValue;
					litTest.Text += "<br />";
					litTest.Text += "rblRadioButtonList.SelectedValue: " + rblRadioButtonList.SelectedValue;
					litTest.Text += "</p>";
				}
			};

		}
	}
}