//---------------------------------------------------------------------------------
// Dado Validators, Copyright 2012 roydukkey, 2012-05-24 (Tue, 24 July 2012).
// Dual licensed under the MIT (http://www.roydukkey.com/mit) and
// GPL Version 2 (http://www.roydukkey.com/gpl) licenses.
//---------------------------------------------------------------------------------

namespace Dado.Validators
{
	using System;
	using System.Drawing;
	using System.ComponentModel;
	using System.Web.UI;

	/// <summary>
	///		Checks if the value of the associated input control is a valid phone number.
	/// </summary>
	[
		ToolboxData("<{0}:PhoneValidator runat=\"server\" ControlToValidate=\"ControlId\" />"),
		ToolboxBitmap(typeof(ResFinder), "Dado.image.phone.bmp")
	]
	public class PhoneValidator : RegularExpressionValidator
	{
		#region Fields

		private const string VALIDATION_EXPRESSION = @"^(?:(?:\+?1\s*(?:[.-]\s*)?)?(?:\(\s*([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*\)|([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9]))\s*(?:[.-]\s*)?)?([2-9]1[02-9]|[2-9][02-9]1|[2-9][02-9]{2})\s*(?:[.-]\s*)?([0-9]{4})(?:\s*(?:#|x\.?|ext\.?|extension)\s*(\d+))?$";
		private const string DEFAULT_ERROR_MESSAGE = "Please enter a valid phone number.";

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
		///		Indicates the regular expression assigned to be the validation criteria.
		/// </summary> 
		[
			Browsable(false),
			EditorBrowsable(EditorBrowsableState.Never),
			DefaultValue(VALIDATION_EXPRESSION)
		]
		public override string ValidationExpression
		{
			get { return VALIDATION_EXPRESSION; }
			set {
				throw new NotSupportedException(
					String.Format(Global.NOT_SUPPORTED_EXCEPTION, "ValidationExpression", this.GetType().ToString())
				);
			}
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

		#endregion Protected Methods
	}
}
