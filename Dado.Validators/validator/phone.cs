//---------------------------------------------------------------------------------
// Dado Validators v1.0.0, Copyright 2014 roydukkey, 2014-04-05 (Sat, 05 April 2014).
// Released under the GPL Version 3 license (https://github.com/roydukkey/Dado.Validators/raw/master/LICENSE).
//---------------------------------------------------------------------------------

namespace Dado.Validators
{
	using System;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Drawing;
	using System.Linq;
	using System.Text.RegularExpressions;
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

		private const string NUMBER_EXPRESSION = @"(?:(?:\+?1\s*(?:[.-]\s*)?)?(?:\(\s*([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*\)|([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9]))\s*(?:[.-]\s*)?)?([2-9]1[02-9]|[2-9][02-9]1|[2-9][02-9]{2})\s*(?:[.-]\s*)?([0-9]{4})";
		private const string EXTENSION_EXPRESSION = @"(?:\s*(?:{0})\s*(\d{{1,{1}}}))?";

		private const string VALIDATION_EXPRESSION = "^{0}$";

		private const string DEFAULT_EXPRESSION = "^" + NUMBER_EXPRESSION + "$";
		private const string DEFAULT_ERROR_MESSAGE = "Please enter a valid phone number.";
		private const int DEFAULT_MAXIMUM_EXTENSION_LENGTH = 4;
		private const string DEFAULT_EXTENSION_PREFIXES = "#,x,ext,extension";

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
		///		Deterimes whether phone extension should be included in validation.
		/// </summary>
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(false),
			Description("Deterimes whether phone extension should be included in validation.")
		]
		public bool AllowExtension
		{
			get { return (bool)(ViewState["AllowExtension"] ?? false); }
			set { ViewState["AllowExtension"] = value; }
		}
		/// <summary>
		///		Indicates a comma delimited list of extension prefixes.
		/// </summary>
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(DEFAULT_EXTENSION_PREFIXES),
			Description("Indicates a comma delimited list of extension prefixes.")
		]
		public string ExtensionPrefixes
		{
			get { return (string)(ViewState["ExtensionPrefixes"] ?? DEFAULT_EXTENSION_PREFIXES); }
			set { ViewState["ExtensionPrefixes"] = value; }
		}
		/// <summary>
		///		Deterimes the maximum length of a valid phone extension.
		/// </summary>
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(DEFAULT_MAXIMUM_EXTENSION_LENGTH),
			Description("Deterimes the maximum length of a valid phone extension.")
		]
		public int MaximumExtensionLength
		{
			get { return (int)(ViewState["MaximumExtensionLength"] ?? false); }
			set { ViewState["MaximumExtensionLength"] = value; }
		}
		/// <summary>
		///		Indicates the regular expression assigned to be the validation criteria.
		/// </summary>
		[
			Browsable(false),
			EditorBrowsable(EditorBrowsableState.Never),
			DefaultValue(DEFAULT_EXPRESSION)
		]
		public override string ValidationExpression
		{
			get {
				return String.Format(VALIDATION_EXPRESSION, NUMBER_EXPRESSION + (AllowExtension ? BuildExtentionRegex() : null));
			}
			set {
				throw new NotSupportedException(
					String.Format(Global.NOT_SUPPORTED_EXCEPTION, "ValidationExpression", this.GetType().ToString())
				);
			}
		}
		/// <summary>
		///		Gets the validated value as a proper phone number.
		/// </summary>
		[
			Themeable(false),
			Description("Gets the validated value as a proper phone number.")
		]
		public virtual string ValidatedPhoneNumber
		{
			get {
				if(!IsValid)
					return null;

				// Get the control value, return true if it is not found
				string controlValue = GetControlValidationValue(ControlToValidate);
				if (String.IsNullOrEmpty(controlValue)) {
					Debug.Fail("Should have been caught by PropertiesValid check");
					return null;
				}

				return Regex.Replace(controlValue, BuildExtentionRegex(), "").Trim();
			}
		}
		/// <summary>
		///		Gets the validated value as a proper phone extension.
		/// </summary>
		[
			Themeable(false),
			Description("Gets the validated value as a proper phone extension.")
		]
		public virtual string ValidatedExtension
		{
			get {
				if(!IsValid)
					return null;

				// Get the control value, return true if it is not found
				string controlValue = GetControlValidationValue(ControlToValidate);
				if (String.IsNullOrEmpty(controlValue)) {
					Debug.Fail("Should have been caught by PropertiesValid check");
					return null;
				}

				return Regex.Replace(controlValue, NUMBER_EXPRESSION, "");
			}
		}

		#endregion Control Attributes

		#region Constructor

		/// <summary>
		///		Initializes a new instance of the PhoneValidator class.
		/// </summary>
		public PhoneValidator() : base()
		{
			DefaultErrorMessage = DEFAULT_ERROR_MESSAGE;
		}

		#endregion Constructor

		#region Private Methods

		/// <summary>
		///		Registers the validator on the page.
		/// </summary>
		private string BuildExtentionRegex()
		{
			string parsedExtensions = "";

			foreach (string ext in ExtensionPrefixes.Split(',').Distinct()) {
				if (!String.IsNullOrWhiteSpace(ext))
					parsedExtensions = parsedExtensions.Append("|", Regex.Escape(ext.Trim()));
			}

			return String.Format(EXTENSION_EXPRESSION, parsedExtensions, DEFAULT_MAXIMUM_EXTENSION_LENGTH);
		}

		#endregion Private Methods
	}
}