//---------------------------------------------------------------------------------
// Dado Validators, Copyright 2013 roydukkey.
// Dual licensed under the MIT (http://www.roydukkey.com/mit) and
// GPL Version 2 (http://www.roydukkey.com/gpl) licenses.
//---------------------------------------------------------------------------------

namespace Dado.Validators
{
	using System;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Drawing;
	using System.Web.UI;

	/// <summary>
	///		Makes the associated input control a required field.
	/// </summary>
	[
		ToolboxData("<{0}:RequiredFieldValidator runat=\"server\" ControlToValidate=\"ControlId\" />"),
		ToolboxBitmap(typeof(ResFinder), "Dado.image.requiredField.bmp")
	]
	public class RequiredFieldValidator : BaseValidator
	{
		#region Fields

		private const string DEFAULT_ERROR_MESSAGE = "Please enter a value.";

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
		///		Gets or sets the initial value of the associated input control.
		/// </summary>
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(""),
			Description("Gets or sets the initial value of the associated input control.")
		]
		public string InitialValue
		{
			get { return (string)(ViewState["InitialValue"] ?? String.Empty); }
			set { ViewState["InitialValue"] = value; }
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
			base.OnInit(e);
		}
		/// <summary>
		///		Adds the HTML attributes and styles that need to be rendered for the control to the specified <see cref='System.Web.UI.HtmlTextWriter'/> object.
		/// </summary>
		/// <param name="writer">An <see cref='System.Web.UI.HtmlTextWriter'/> that represents the output stream to render HTML content on the client.</param>
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			base.AddAttributesToRender(writer);
			if (RenderUplevel) {
				string id = ClientID;
				HtmlTextWriter expandoAttributeWriter = (EnableLegacyRendering) ? writer : null;
				AddExpandoAttribute(expandoAttributeWriter, id, "evaluationfunction", "RequiredFieldValidatorEvaluateIsValid", false);
				AddExpandoAttribute(expandoAttributeWriter, id, "initialvalue", InitialValue);
			}
		}
		/// <summary>
		///		Called during the validation stage when ASP.NET processes a Web Form.
		/// </summary>
		/// <returns>true if the value in the input control is valid; otherwise, false.</returns>
		protected override bool EvaluateIsValid()
		{
			// Get the control value, return true if it is not found 
			string controlValue = GetControlValidationValue(ControlToValidate);
			if (controlValue == null) {
				Debug.Fail("Should have been caught by PropertiesValid check");
				return true;
			}

			// See if the control has changed
			return !controlValue.Trim().Equals(InitialValue.Trim());
		}

		#endregion Protected Methods
	}
}