//---------------------------------------------------------------------------------
// Dado Validators, Copyright 2013 roydukkey, 2013-01-11 (Fri, 11 January 2013).
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

		private const string VALIDATION_EXPRESSION = @"^.*\.({0})$";
		private const string DEFAULT_ERROR_MESSAGE = "Please enter a file of a following type: {0}.";

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
			get { return String.Format(DEFAULT_ERROR_MESSAGE, base.ErrorMessage == DefaultErrorMessage ? ProcessErrorMessage() : base.ErrorMessage); }
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
			get { return ProcessValidTypes(); }
			set {
				throw new NotSupportedException(
					String.Format(Global.NOT_SUPPORTED_EXCEPTION, "ValidationExpression", this.GetType().ToString())
				);
			}
		}
		/// <summary>
		///		Indicates a comma delimited list of case-insensitive, acceptable file types.
		/// </summary> 
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(""),
			Description("Indicates a comma delimited list of case-insensitive, acceptable file types.")
		]
		public string ValidTypes
		{
			get { return (string) (ViewState["ValidTypes"] ?? String.Empty); }
			set { ViewState["ValidTypes"] = value; }
		}

		#endregion Control Attributes

		#region Private Methods

		/// <summary>
		///		Process ValidTypes for insertion into ValidationExpression.
		/// </summary>
		/// <returns></returns>
		private string ProcessValidTypes()
		{
			string validTypes = "";
			foreach (string type in ValidTypes.Replace(".", "").Split(',')) {
				if (!String.IsNullOrWhiteSpace(type)) {
					string berry = "";
					foreach (char letter in type.Trim().ToLower())
						berry += "[" + letter + Char.ToUpper(letter) + "]";
					validTypes = validTypes.Append("|", berry);
				}
			}
			return String.Format(VALIDATION_EXPRESSION, validTypes);
		}
		/// <summary>
		///		Process ValidTypes for insertion into ErrorMessage.
		/// </summary>
		/// <returns></returns>
		private string ProcessErrorMessage()
		{
			string validTypes = "";
			foreach (string type in ValidTypes.Replace(".", "").Split(',')) {
				if (!String.IsNullOrWhiteSpace(type))
					validTypes = validTypes.Append(", ", "." + type.Trim().ToLower());
			}
			return validTypes;
		}

		#endregion Private Methods
	}
}
