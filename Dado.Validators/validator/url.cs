//---------------------------------------------------------------------------------
// Dado Validators, Copyright 2013 roydukkey, 2013-01-11 (Fri, 11 January 2013).
// Dual licensed under the MIT (http://www.roydukkey.com/mit) and
// GPL Version 2 (http://www.roydukkey.com/gpl) licenses.
//---------------------------------------------------------------------------------

/* Beta
 * namespace Dado.Validators
{
	using System;
	using System.Drawing;
	using System.ComponentModel;
	using System.Web.UI;

	/// <summary>
	///		Checks if the value of the associated input control is a valid url.
	/// </summary>
	[
		ToolboxData("<{0}:UrlValidator runat=\"server\" ControlToValidate=\"ControlId\" ErrorMessage=\"UrlValidator\" />"),
		ToolboxBitmap(typeof(ResFinder), "Dado.image.requiredField.bmp")
	]
	public class UrlValidator : RegularExpressionValidator
	{

		#region Control Attributes

		/// <summary>
		///		Indicates the regular expression assigned to be the validation criteria.
		/// </summary> 
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(Global.URL_VALIDATION_EXPRESSION),
			Description("Indicates the regular expression assigned to be the validation criteria.")
		]
		new private string ValidationExpression
		{
			get
			{
				return Global.URL_VALIDATION_EXPRESSION;
			}
			set
			{
				base.ValidationExpression = Global.URL_VALIDATION_EXPRESSION;
			}
		}

		/// <summary>
		///		Gets or sets the text for the error message displayed in a ValidationSummary control when validation fails.
		/// </summary>
		[
			Category("Appearance"),
			Themeable(false),
			DefaultValue("Please enter a valid URL."),
			Description("Gets or sets the text for the error message displayed in a ValidationSummary control when validation fails.")
		]
		new public string ErrorMessage
		{
			get
			{
				return String.IsNullOrEmpty(base.ErrorMessage) ? "Please enter a valid URL." : base.ErrorMessage;
			}
			set
			{
				base.ErrorMessage = value;
			}
		}

		#endregion Control Attributes

		#region Protected Methods

		/// <summary>
		///		Checks the client brower and configures the validator for compatibility prior to rendering.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreRender(EventArgs e)
		{
			// init value
			ErrorMessage = ErrorMessage;
			ValidationExpression = Global.URL_VALIDATION_EXPRESSION;
			base.OnPreRender(e);
		}

		#endregion Protected Methods

	}
}*/