//---------------------------------------------------------------------------------
// Dado Validators v1.0.1, Copyright 2015 roydukkey, 2015-01-07 (Wed, 07 Jan 2015).
// Released under the GPL Version 3 license (https://github.com/roydukkey/Dado.Validators/raw/master/LICENSE).
//---------------------------------------------------------------------------------

/* Beta
 * namespace Dado.Validators
{
	using System;
	using System.Drawing;
	using System.ComponentModel;
	using System.Web.UI;

	/// <summary>
	///		Checks if the value of the associated input control is in a valid USD currency format.
	/// </summary>
	[
		ToolboxData("<{0}:USDCurrencyValidator runat=\"server\" ControlToValidate=\"ControlId\" ErrorMessage=\"USDCurrencyValidator\" />"),
		ToolboxBitmap(typeof(ResFinder), "Dado.image.requiredField.bmp")
	]
	public class USDCurrencyValidator : RegularExpressionValidator
	{

		#region Control Attributes

		/// <summary>
		///		Indicates the regular expression assigned to be the validation criteria.
		/// </summary>
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(Global.USD_CURRENCY_VALIDATION_EXPRESSION),
			Description("Indicates the regular expression assigned to be the validation criteria.")
		]
		new private string ValidationExpression
		{
			get
			{
				return Global.USD_CURRENCY_VALIDATION_EXPRESSION;
			}
			set
			{
				base.ValidationExpression = Global.USD_CURRENCY_VALIDATION_EXPRESSION;
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
			ValidationExpression = Global.USD_CURRENCY_VALIDATION_EXPRESSION;
			base.OnPreRender(e);
		}

		#endregion Protected Methods

	}
}
*/