//---------------------------------------------------------------------------------
// Dado Validators, Copyright 2013 roydukkey, 2013-06-11 (Tues, 11 June 2013).
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