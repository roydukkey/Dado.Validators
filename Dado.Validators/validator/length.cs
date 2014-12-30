//---------------------------------------------------------------------------------
// Dado Validators v1.0.0, Copyright 2014 roydukkey, 2014-04-05 (Sat, 05 April 2014).
// Released under the GPL Version 3 license (https://github.com/roydukkey/Dado.Validators/raw/master/LICENSE).
//---------------------------------------------------------------------------------

namespace Dado.Validators
{
	using System;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Drawing;
	using System.Web.UI;

	/// <summary>
	///		Checks if the value of the associated input control meets minimum, and maximum text length constraints.
	/// </summary>
	[
		ToolboxData("<{0}:LengthValidator runat=\"server\" ControlToValidate=\"ControlId\" MaximumLength=\"LengthValidator\" />"),
		ToolboxBitmap(typeof(ResFinder), "Dado.image.length.bmp")
	]
	public class LengthValidator : BaseValidator
	{
		#region Fields

		private const string MinimumLengthErrorMessage_Default = "Value must not be less than {0} character{1} in length.";
		private const string MaximumLengthErrorMessage_Default = "Value must not be more than {0} character{1} in length.";

		#endregion Fields

		#region Control Attributes

		/// <summary>
		///		Make ErrorMessage private because there aren't generic ErrorMessages
		/// </summary>
		new private string ErrorMessage
		{
			get { return base.ErrorMessage; }
			set { base.ErrorMessage = value; }
		}
		/// <summary>
		///		Indicates the maximum text length for the control value.
		/// </summary>
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(0),
			Description("Indicates the minimum text length for the control value.")
		]
		public int MinimumLength
		{
			get { return (int)(ViewState["MinimumLength"] ?? default(int)); }
			set { ViewState["MinimumLength"] = value; }
		}
		/// <summary>
		///		Message to display when MinimumLength is surpassed.
		/// </summary>
		[
			Category("Appearance"),
			Themeable(false),
			DefaultValue(MinimumLengthErrorMessage_Default),
			Description("Message to display when MinimumLength is surpassed.")
		]
		public string MinimumLengthErrorMessage
		{
			get {
				return String.Format(
					(string)(ViewState["MinimumLengthErrorMessage"] ?? MinimumLengthErrorMessage_Default),
					MinimumLength,
					MinimumLength > 1 ? "s" : null
				);
			}
			set { ViewState["MinimumLengthErrorMessage"] = value; }
		}
		/// <summary>
		///		Indicates the maximum text length for the control value.
		/// </summary>
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(0),
			Description("Indicates the maximum text length for the control value.")
		]
		public int MaximumLength
		{
			get { return (int)(ViewState["MaximumLength"] ?? default(int)); }
			set { ViewState["MaximumLength"] = value; }
		}
		/// <summary>
		///		Message to display when MaximumLength is surpassed.
		/// </summary>
		[
			Category("Appearance"),
			Themeable(false),
			DefaultValue(MaximumLengthErrorMessage_Default),
			Description("Message to display when MaximumLength is surpassed.")
		]
		public string MaximumLengthErrorMessage
		{
			get {
				return String.Format(
					(string)(ViewState["MaximumLengthErrorMessage"] ?? MaximumLengthErrorMessage_Default),
					MaximumLength,
					MaximumLength > 1 ? "s" : null
				);
			}
			set { ViewState["MaximumLengthErrorMessage"] = value; }
		}

		#endregion Control Attributes

		#region Protected Methods

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
				AddExpandoAttribute(expandoAttributeWriter, id, "evaluationfunction", "LengthValidatorEvaluateIsValid", false);
				AddExpandoAttribute(expandoAttributeWriter, id, "minimumlength", MinimumLength.ToString());
				AddExpandoAttribute(expandoAttributeWriter, id, "minimumerrormessage", MinimumLengthErrorMessage);
				AddExpandoAttribute(expandoAttributeWriter, id, "maximumlength", MaximumLength.ToString());
				AddExpandoAttribute(expandoAttributeWriter, id, "maximumerrormessage", MaximumLengthErrorMessage);
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
			// Check if Lengths nullify each other
			if (MaximumLength > 0 && MinimumLength > MaximumLength) {
				Debug.Fail("MinimumLength cannot be great than MaximumLength.");
				return false;
			}
			// Do not validate empty control. Use a required field validator.
			if (controlValue != "") {
				// See if value is greater than MinimumLength
				if (MinimumLength > 0 && controlValue.Length < MinimumLength) {
					ErrorMessage = MinimumLengthErrorMessage;
					return false;
				}
				// See if value is greater than MaximumLength
				if (MaximumLength > 0 && controlValue.Length > MaximumLength) {
					ErrorMessage = MaximumLengthErrorMessage;
					return false;
				}
			}
			return true;
		}

		#endregion Protected Methods
	}
}