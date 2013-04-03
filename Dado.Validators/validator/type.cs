//---------------------------------------------------------------------------------
// Dado Validators, Copyright 2013 roydukkey, 2013-04-03 (Wed, 03 April 2013).
// Dual licensed under the MIT (http://www.roydukkey.com/mit) and
// GPL Version 2 (http://www.roydukkey.com/gpl) licenses.
//---------------------------------------------------------------------------------

namespace Dado.Validators
{
	using System;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Drawing;
	using System.Web.UI;

	/// <summary>
	///		The types available for validation.
	/// </summary>
	public enum ValidationType
	{
		/// <summary>
		///		Provides server-side and client-side validation.
		/// </summary>
		Boolean,
		/// <summary>
		///		Provides server-side and client-side validation.
		/// </summary>
		Byte,
		/// <summary>
		///		Provides server-side and client-side validation.
		/// </summary>
		Char,
		/*
		Use: <Dado:CompareValidator runat="server" ControlToValidate="txtDateActive" ValidationGroup="vlgSubmit" Type="Date" Operator="DataTypeCheck" ErrorMessage="Please enter a valid date." />
		DateTime,
		*/
		/// <summary>
		///		Provides server-side and client-side validation.
		/// </summary>
		Decimal,
		/// <summary>
		///		Provides server-side and client-side validation.
		/// </summary>
		Double,
		/// <summary>
		///		Provides server-side and client-side validation.
		/// </summary>
		Int16,
		/// <summary>
		///		Provides server-side and client-side validation.
		/// </summary>
		Int32,
		/// <summary>
		///		Provides server-side and client-side validation.
		/// </summary>
		Int64,
		/// <summary>
		///		Provides server-side and client-side validation.
		/// </summary>
		SByte,
		/// <summary>
		///		Provides server-side and client-side validation.
		/// </summary>
		Single
	}

	/// <summary>
	///		Checks if the value of the associated input control has an acceptable type.
	/// </summary>
	[
		DefaultProperty("Type"),
		ToolboxData("<{0}:TypeValidator runat=\"server\" ControlToValidate=\"ControlId\" Type=\"Int32\" />"),
		ToolboxBitmap(typeof(ResFinder), "Dado.image.custom.bmp")
	]
	public class TypeValidator : BaseValidator
	{
		#region Fields

		private const string DEFAULT_ERROR_MESSAGE = "Please enter a value of type {0}.";

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
			get { return String.Format(base.ErrorMessage ?? DEFAULT_ERROR_MESSAGE, Type); }
			set { base.ErrorMessage = value; }
		}
		/// <summary>
		///		Gets or sets the data type that the values are validated against.
		/// </summary>
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(ValidationType.Int32),
			Description("Gets or sets the data type that the values are validated against.")
		]
		public ValidationType Type
		{
			get { return (ValidationType)(ViewState["Type"] ?? ValidationType.Int32); }
			set { ViewState["Type"] = value; }
		}
		/// <summary>
		///		Gets or sets a Boolean value indicating whether empty text should be validated.
		/// </summary>
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(false),
			Description("Gets or sets a Boolean value indicating whether empty text should be validated."),
		]
		public bool ValidateEmptyText
		{
			get { return (bool)(ViewState["ValidateEmptyText"] ?? false); }
			set { ViewState["ValidateEmptyText"] = value; }
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
		/// <summary>
		///		Adds the HTML attributes and styles that need to be rendered for the control to the specified <see cref='System.Web.UI.HtmlTextWriter'/> object.
		/// </summary>
		/// <param name="writer">An <see cref='System.Web.UI.HtmlTextWriter'/> that represents the output stream to render HTML content on the client.</param>
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			base.AddAttributesToRender(writer);

			if (RenderUplevel) {
				string id = ClientID;
				HtmlTextWriter expandoAttributeWriter = (EnableLegacyRendering) ? writer : null;
				AddExpandoAttribute(expandoAttributeWriter, id, "evaluationfunction", "TypeValidatorEvaluateIsValid", false);
				AddExpandoAttribute(expandoAttributeWriter, id, "type", Type.ToString());
				AddExpandoAttribute(expandoAttributeWriter, id, "validateemptytext", ValidateEmptyText ? "true" : "false", false);
			}
		}
		/// <summary>
		///		Called during the validation stage when ASP.NET processes a Web Form.
		/// </summary>
		/// <returns>true if the value in the input control is valid; otherwise, false.</returns>
		protected override bool EvaluateIsValid()
		{
			// Get the control value, return true if it is not found 
			string controlValue = GetControlValidationValue(ControlToValidate);
			if ((controlValue == null || controlValue.Trim().Length == 0) && !ValidateEmptyText) {
				Debug.Fail("Should have been caught by PropertiesValid check");
				return true;
			}

			switch (Type) {
				case ValidationType.Boolean:
					bool t1;
					return Boolean.TryParse(controlValue, out t1);

				case ValidationType.Byte:
					byte t2;
					return Byte.TryParse(controlValue, out t2);

				case ValidationType.Char:
					char t3;
					return Char.TryParse(controlValue, out t3);

				case ValidationType.Decimal:
					decimal t4;
					return Decimal.TryParse(controlValue, out t4);

				case ValidationType.Double:
					double t5;
					return Double.TryParse(controlValue, out t5);

				case ValidationType.Int16:
					short t6;
					return Int16.TryParse(controlValue, out t6);

				case ValidationType.Int64:
					long t7;
					return Int64.TryParse(controlValue, out t7);

				case ValidationType.SByte:
					sbyte t8;
					return SByte.TryParse(controlValue, out t8);

				case ValidationType.Single:
					float t9;
					return Single.TryParse(controlValue, out t9);

				//case ValidationType.Int32:
				default:
					int t10;
					return Int32.TryParse(controlValue, out t10);
			}
		}

		#endregion Protected Methods
	}
}