//---------------------------------------------------------------------------------
// Dado Validators v1.0.1, Copyright 2015 roydukkey, 2015-01-07 (Wed, 07 Jan 2015).
// Released under the GPL Version 3 license (https://github.com/roydukkey/Dado.Validators/raw/master/LICENSE).
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

		#region Constructor

		/// <summary>
		///		Initializes a new instance of the TimeValidator class.
		/// </summary>
		public TimeValidator() : base()
		{
			DefaultErrorMessage = DEFAULT_ERROR_MESSAGE;
		}

		#endregion Constructor

		#region Public Methods

		/// <summary>
		///		Returns the validated value as a TimeSpan
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

		#endregion Public Methods
	}
}
