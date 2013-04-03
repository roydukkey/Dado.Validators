//---------------------------------------------------------------------------------
// Dado Validators, Copyright 2013 roydukkey, 2013-04-03 (Wed, 03 April 2013).
// Dual licensed under the MIT (http://www.roydukkey.com/mit) and
// GPL Version 2 (http://www.roydukkey.com/gpl) licenses.
//---------------------------------------------------------------------------------

namespace Dado.Validators
{
	using System;
	using System.Drawing;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Text.RegularExpressions;
	using System.Web;
	using System.Web.UI;

	/// <summary>
	///		Checks if the value of the associated input control matches the pattern of a regular expression.
	/// </summary>
	[
		ToolboxData("<{0}:RegularExpressionValidator runat=\"server\" ControlToValidate=\"ControlId\" />"),
		ToolboxBitmap(typeof(ResFinder), "Dado.image.regularExpression.bmp")
	]
	public class RegularExpressionValidator : BaseValidator
	{
		#region Control Attributes

		/// <summary>
		///		Indicates the regular expression assigned to be the validation criteria.
		/// </summary>
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(""),
			Description("Indicates the regular expression assigned to be the validation criteria.")
		]
		public virtual string ValidationExpression
		{
			get { return (string)(ViewState["ValidationExpression"] ?? String.Empty); }
			set {
				try {
					Regex.IsMatch(String.Empty, value);
				}
				catch (Exception e) {
					throw new HttpException("Bad Regex: " + value, e);
				}
				ViewState["ValidationExpression"] = value;
			}
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
				AddExpandoAttribute(expandoAttributeWriter, id, "evaluationfunction", "RegularExpressionValidatorEvaluateIsValid", false);
				if (ValidationExpression.Length > 0)
					AddExpandoAttribute(expandoAttributeWriter, id, "validationexpression", ValidationExpression);
			}
		}
		/// <summary>
		///		Determines whether the content in the input control is valid.
		/// </summary>
		/// <returns>true if the control is valid; otherwise, false.</returns>
		protected override bool EvaluateIsValid()
		{
			// Always succeeds if input is empty or value was not found
			string controlValue = GetControlValidationValue(ControlToValidate);
			Debug.Assert(controlValue != null, "Should have already been checked");
			if (controlValue == null || controlValue.Trim().Length == 0) return true;

			try {
				// we are looking for an exact match, not just a search hit 
				Match m = Regex.Match(controlValue, ValidationExpression);
				return (m.Success && m.Index == 0 && m.Length == controlValue.Length);
			}
			catch {
				Debug.Fail("Regex error should have been caught in property setter.");
				return true;
			}
		}

		#endregion Protected Methods
	}
}
