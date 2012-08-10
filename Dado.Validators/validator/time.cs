//---------------------------------------------------------------------------------
// Dado Validators, Copyright 2012 roydukkey, 2012-08-09 (Thu, 09 August 2012).
// Dual licensed under the MIT (http://www.roydukkey.com/mit) and
// GPL Version 2 (http://www.roydukkey.com/gpl) licenses.
//---------------------------------------------------------------------------------

namespace Dado.Validators
{
	using System;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Globalization;
	using System.Drawing;
	using System.Web.UI;

	/// <summary>
	///		Checks if the value of the associated input control is in a valid time format. ie: HH:mm, h:mm tt, h:mmtt, h:mm, hh:mm tt, hh:mmtt, hh:mm
	/// </summary>
	[
		ToolboxData("<{0}:TimeValidator runat=\"server\" ControlToValidate=\"ControlId\" />"),
		ToolboxBitmap(typeof(ResFinder), "Dado.image.custom.bmp")
	]
	public class TimeValidator : RegularExpressionValidator
	{
		#region Fields

		private const string VALIDATION_EXPRESSION = @"^(?:(?:0?[0-9]|1[0-2]):[0-5][0-9]\s?(?:(?:[Aa]|[Pp])[Mm])?|(?:1[3-9]|2[0-3]):[0-5][0-9])$";
		private const string DEFAULT_ERROR_MESSAGE = "Please enter a valid time.";
		private readonly string[] TIME_FORMATS = { "HH:mm", "h:mm tt", "h:mmtt", "h:mm", "hh:mm tt", "hh:mmtt", "hh:mm" };

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
			DefaultValue(VALIDATION_EXPRESSION),
			Browsable(false),
			EditorBrowsable(EditorBrowsableState.Never)
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

		#region Static Methods

		/// <summary>
		///		Returns the validate value as a TimeSpan
		/// </summary>
		/// <returns></returns>
		public TimeSpan? ValidatedValue()
		{
			// Get the control value, return true if it is not found
			string controlValue = GetControlValidationValue(ControlToValidate);
			if (String.IsNullOrEmpty(controlValue)) {
				Debug.Fail("Should have been caught by PropertiesValid check");
				return null;
			}
			return (TimeSpan?)DateTime.ParseExact(controlValue, TIME_FORMATS, null, DateTimeStyles.None).TimeOfDay;
		}

		#endregion Static Methods
	}
}
