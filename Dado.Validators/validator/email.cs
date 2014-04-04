//---------------------------------------------------------------------------------
// Dado Validators, Copyright 2014 roydukkey.
// Dual licensed under the MIT (http://www.roydukkey.com/mit) and
// GPL Version 2 (http://www.roydukkey.com/gpl) licenses.
//---------------------------------------------------------------------------------

namespace Dado.Validators
{
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Web.UI;

	/// <summary>
	///		Checks if the value of the associated input control is a valid email address.
	/// </summary>
	[
		ToolboxData("<{0}:EmailValidator runat=\"server\" ControlToValidate=\"ControlId\" />"),
		ToolboxBitmap(typeof(ResFinder), "Dado.image.email.bmp")
	]
	public class EmailValidator : RegularExpressionValidator
	{
		#region Fields

		private const string VALIDATION_EXPRESSION = @"^[-+.\w]{1,64}@[-.\w]{1,64}\.[-.\w]{2,6}$";
		private const string DEFAULT_ERROR_MESSAGE = "Please enter a valid email address.";

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

		#region Constructors

		/// <summary>
		///		Initializes a new instance of the EmailValidator class.
		/// </summary>
		public EmailValidator() : base()
		{
			DefaultErrorMessage = DEFAULT_ERROR_MESSAGE;
		}

		#endregion Constructors
	}
}
