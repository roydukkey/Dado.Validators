﻿//---------------------------------------------------------------------------------
// Dado Validators v1.0.1, Copyright 2015 roydukkey, 2015-01-07 (Wed, 07 Jan 2015).
// Released under the GPL Version 3 license (https://github.com/roydukkey/Dado.Validators/raw/master/LICENSE).
//---------------------------------------------------------------------------------

namespace Dado.Validators
{
	using System;
	using System.Drawing;
	using System.ComponentModel;
	using System.Web.UI;

	/// <summary>
	///		Checks if the value of the associated input control is a valid zip code.
	/// </summary>
	[
		ToolboxData("<{0}:ZipCodeValidator runat=\"server\" ControlToValidate=\"ControlId\" />"),
		ToolboxBitmap(typeof(ResFinder), "Dado.image.zipCode.bmp")
	]
	public class ZipCodeValidator : RegularExpressionValidator
	{
		#region Fields

		private const string VALIDATION_EXPRESSION = @"^(\d{5}-\d{4}|\d{5}|\d{9})$";
		private const string DEFAULT_ERROR_MESSAGE = "Please enter a valid zip code.";

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
		///		Initializes a new instance of the ZipCodeValidator class.
		/// </summary>
		public ZipCodeValidator() : base()
		{
			DefaultErrorMessage = DEFAULT_ERROR_MESSAGE;
		}

		#endregion Constructor
	}
}
