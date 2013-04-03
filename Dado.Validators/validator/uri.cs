//---------------------------------------------------------------------------------
// Dado Validators, Copyright 2013 roydukkey, 2013-04-03 (Wed, 03 April 2013).
// Dual licensed under the MIT (http://www.roydukkey.com/mit) and
// GPL Version 2 (http://www.roydukkey.com/gpl) licenses.
//---------------------------------------------------------------------------------

namespace Dado.Validators
{
	using System;
	using System.Diagnostics;
	using System.Drawing;
	using System.ComponentModel;
	using System.Web.UI;
	using System.Web.UI.WebControls;

	/// <summary>
	///		Checks if the value of the associated input control is a valid uri.
	/// </summary>
	[
		ToolboxData("<{0}:UriValidator runat=\"server\" ControlToValidate=\"ControlId\" ErrorMessage=\"UriValidator\" />"),
		ToolboxBitmap(typeof(ResFinder), "Dado.image.requiredField.bmp")
	]
	public class UriValidator : BaseValidator
	{
		#region Fields

		private const string DEFAULT_ERROR_MESSAGE = "Please enter a valid URI.";

		#endregion Fields

		#region Control Attributes

		/// <summary>
		///		Gets or sets the text for the error message.
		/// </summary>
		[
			DefaultValue(DEFAULT_ERROR_MESSAGE)
		]
		public override string ErrorMessage
		{
			get { return base.ErrorMessage; }
			set { base.ErrorMessage = value; }
		}
		/// <summary>
		///		Gets or sets the UriKind the values are validated against.
		/// </summary>
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(UriKind.RelativeOrAbsolute),
			Description("Gets or sets the UriKind the values are validated against.")
		]
		public UriKind Kind
		{
			get { return (UriKind)(ViewState["Kind"] ?? UriKind.RelativeOrAbsolute); }
			set { ViewState["Kind"] = value; }
		}
		/// <summary>
		///		Deterimes whether URI should try to be fixed during validation.
		/// </summary>
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(false),
			Description("Deterimes whether URI should try to be fixed during validation.")
		]
		public bool TryToFix
		{
			get { return (bool)(ViewState["TryToFix"] ?? false); }
			set { ViewState["TryToFix"] = value; }
		}
		/// <summary>
		///		Gets or sets a value indicating whether client-side validation is enabled.
		/// </summary>
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(true),
			Description("Gets or sets a value indicating whether client-side validation is enabled.")
		]
		new private bool EnableClientScript
		{
			get { return base.EnableClientScript; }
			set { base.EnableClientScript = value; }
		}

		#endregion Control Attributes

		#region Protected Methods

		/// <summary>
		///		Registers the validator on the page.
		/// </summary>
		/// <param name="e">A <see cref='System.EventArgs'/> that contains the event data.</param>
		protected override void OnInit(EventArgs e)
		{
			DefaultErrorMessage = DEFAULT_ERROR_MESSAGE;
			EnableClientScript = false;
			base.OnInit(e);
		}
		/// <summary>
		///		Called during the validation stage when ASP.NET processes a Web Form.
		/// </summary>
		/// <returns>true if the value in the input control is valid; otherwise, false.</returns>
		protected override bool EvaluateIsValid()
		{
			// Get the control value, return true if it is not found 
			string controlValue = GetControlValidationValue(ControlToValidate);
			if ((controlValue == null || controlValue.Trim().Length == 0)) {
				Debug.Fail("Should have been caught by PropertiesValid check");
				return true;
			}

			if (TryToFix) {
				if (!controlValue.StartsWith("http://") && !controlValue.StartsWith("https://")) {
					SetControlValidationValue(ControlToValidate, "http://" + controlValue);
				}
			}

			return Uri.IsWellFormedUriString(controlValue, Kind);
		}

		/// <summary>
		///		Sets the validation value of the control named relative to the validator.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		protected virtual void SetControlValidationValue(string name, string value)
		{
			// get the control using the relative name 
			Control control = NamingContainer.FindControl(name);
			if (control == null) {
				return;
			}

			if (control is IEditableTextControl) {
				((IEditableTextControl)control).Text = value;
			}
			else {
				// get its validation property
				PropertyDescriptor prop = GetValidationProperty(control);
				if (prop == null) {
					return;
				}

				// get its value as a string 
				object currentValue = prop.GetValue(control);
				if (currentValue is ListItem) {
					((ListItem)currentValue).Value = value;
				}
			}
		}

		#endregion Protected Methods
	}
}