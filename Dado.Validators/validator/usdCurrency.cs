//---------------------------------------------------------------------------------
// Dado Validators v1.0.0, Copyright 2014 roydukkey, 2014-04-05 (Sat, 05 April 2014).
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