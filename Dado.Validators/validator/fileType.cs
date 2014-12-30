//---------------------------------------------------------------------------------
// Dado Validators v1.0.0, Copyright 2014 roydukkey, 2014-04-05 (Sat, 05 April 2014).
// Released under the GPL Version 3 license (https://github.com/roydukkey/Dado.Validators/raw/master/LICENSE).
//---------------------------------------------------------------------------------

namespace Dado.Validators
{
	using System;
	using System.Linq;
	using System.ComponentModel;
	using System.Drawing;
	using System.Web;
	using System.Web.UI;

	/// <summary>
	///		The validation modes available to the <see cref='Dado.Validators.FileTypeValidator'/>.
	/// </summary>
	public enum ValidationFileTypeOperator
	{
		/// <summary>
		///		Validation verifies file's extension is one of the provided <see cref='Dado.Validators.FileTypeValidator.FileExtensions'/>.
		/// </summary>
		Positive,
		/// <summary>
		///		Validation verifies file's extension is not one of the provided <see cref='Dado.Validators.FileTypeValidator.FileExtensions'/>.
		/// </summary>
		Negative
	}

	/// <summary>
	///		Checks if the value of the associated input control has an acceptable file extension.
	/// </summary>
	[
		DefaultProperty("ValidTypes"),
		ToolboxData("<{0}:FileTypeValidator runat=\"server\" ControlToValidate=\"ControlId\" ValidTypes=\"FileTypeValidator\" />"),
		ToolboxBitmap(typeof(ResFinder), "Dado.image.fileType.bmp")
	]
	public class FileTypeValidator : RegularExpressionValidator
	{
		#region Fields

		private const string POSITIVE_VALIDATION_EXPRESSION = @"^.*\.({0})$";
		private const string NEGATIVE_VALIDATION_EXPRESSION = @"^.*\.(?!(?:{0})$)(?![^\.]*\.)(.*)|^[^\.]+$";
		private const string POSITIVE_DEFAULT_ERROR_MESSAGE = "Please enter a file of a following type: {0}.";
		private const string NEGATIVE_DEFAULT_ERROR_MESSAGE = "Please enter a file not of a following type: {0}.";

		#endregion Fields

		#region Control Attributes

		/// <summary>
		///		Indicates a comma delimited list of case-insensitive file extensions.
		/// </summary>
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(""),
			Description("Indicates a comma delimited list of case-insensitive file extensions.")
		]
		public string FileExtensions
		{
			get { return (string)(ViewState["FileExtensions"] ?? String.Empty); }
			set { ViewState["FileExtensions"] = value; }
		}
		/// <summary>
		///		Gets or sets the comparison operation to perform.
		/// </summary>
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(ValidationFileTypeOperator.Positive),
			Description("Gets or sets the comparison operation to perform.")
		]
		public ValidationFileTypeOperator Operator
		{
			get { return (ValidationFileTypeOperator)(ViewState["Operator"] ?? ValidationFileTypeOperator.Positive); }
			set { ViewState["Operator"] = value; }
		}
		/// <summary>
		///		Gets or sets the text for the error message.
		/// </summary>
		[
			DefaultValue(POSITIVE_DEFAULT_ERROR_MESSAGE)
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
			EditorBrowsable(EditorBrowsableState.Never)
		]
		public override string ValidationExpression
		{
			get { return ProcessFileExtensions(); }
			set {
				throw new NotSupportedException(
					String.Format(Global.NOT_SUPPORTED_EXCEPTION, "ValidationExpression", this.GetType().ToString())
				);
			}
		}
		/// <summary>
		///		Gets or sets the data type that the values are validated against.
		/// </summary>
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(false),
			Description("Gets or sets the data type that the values are validated against.")
		]
		public override bool IgnoreCase
		{
			get { return true; }
			set {
				throw new NotSupportedException(
					String.Format(Global.NOT_SUPPORTED_EXCEPTION, "IgnoreCase", this.GetType().ToString())
				);
			}
		}

		#endregion Control Attributes

		#region Protected Properties

		/// <summary>
		///		Gets or sets the text for the default error message.
		/// </summary>
		protected override string DefaultErrorMessage
		{
			get { return String.Format(Operator == ValidationFileTypeOperator.Positive ? POSITIVE_DEFAULT_ERROR_MESSAGE : NEGATIVE_DEFAULT_ERROR_MESSAGE, ProcessErrorMessage()); }
			set { base.DefaultErrorMessage = value; }
		}

		#endregion Protected Properties

		#region Protected Methods

		/// <summary>
		///		Checks the client brower and configures the validator for compatibility prior to rendering.
		/// </summary>
		/// <param name="e">A <see cref='System.EventArgs'/> that contains the event data.</param>
		protected override void OnPreRender(EventArgs e)
		{
			// Ensure FileExtensions are defined
			if (FileExtensions.Trim().Length == 0)
				throw new HttpException("The FileExtensions property of '"+ID+"' cannot be blank.");

			base.OnPreRender(e);
		}

		#endregion Protected Methods

		#region Private Methods

		/// <summary>
		///		Process ValidTypes for insertion into ValidationExpression.
		/// </summary>
		/// <returns></returns>
		private string ProcessFileExtensions()
		{
			string validTypes = "";
			foreach (string type in FileExtensions.ToLower().Replace(".", "").Split(',').Distinct()) {
				if (!String.IsNullOrWhiteSpace(type))
					validTypes = validTypes.Append("|", type.Trim().ToLower());
			}

			return String.Format(Operator == ValidationFileTypeOperator.Positive ? POSITIVE_VALIDATION_EXPRESSION : NEGATIVE_VALIDATION_EXPRESSION, validTypes);
		}
		/// <summary>
		///		Process ValidTypes for insertion into ErrorMessage.
		/// </summary>
		/// <returns></returns>
		private string ProcessErrorMessage()
		{
			string validTypes = "";
			foreach (string type in FileExtensions.Replace(".", "").Split(',')) {
				if (!String.IsNullOrWhiteSpace(type))
					validTypes = validTypes.Append(", ", "." + type.Trim().ToLower());
			}
			return validTypes;
		}

		#endregion Private Methods
	}
}
