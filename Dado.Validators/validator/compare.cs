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
	using System.Drawing;
	using System.Web;
	using System.Web.UI;
	using WebControls = System.Web.UI.WebControls;

	/// <summary>
	///		Compares the value entered by the user in an input control with the value entered in another input control, or with a constant value.
	/// </summary>
	[
		ToolboxData("<{0}:CompareValidator runat=\"server\" ControlToValidate=\"ControlId\" />"),
		ToolboxBitmap(typeof(ResFinder), "Dado.image.compare.bmp")
	]
	public class CompareValidator : BaseCompareValidator
	{
		#region Control Attributes

		/// <summary>
		///		Gets or sets the ID of the input control to compare with.
		/// </summary>
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(""),
			Description("Gets or sets the ID of the input control to compare with."),
			TypeConverter(typeof(WebControls.ValidatedControlConverter))
		]
		public string ControlToCompare
		{
			get { return (string) (ViewState["ControlToCompare"] ?? String.Empty); }
			set { ViewState["ControlToCompare"] = value; }
		}
		/// <summary>
		///		Gets or sets the comparison operation to perform.
		/// </summary>
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(WebControls.ValidationCompareOperator.Equal),
			Description("Gets or sets the comparison operation to perform.")
		]
		public WebControls.ValidationCompareOperator Operator
		{
			get { return (WebControls.ValidationCompareOperator) (ViewState["Operator"] ?? WebControls.ValidationCompareOperator.Equal); }
			set {
				if (value < WebControls.ValidationCompareOperator.Equal || value > WebControls.ValidationCompareOperator.DataTypeCheck)
					throw new ArgumentOutOfRangeException("value");
				ViewState["Operator"] = value;
			}
		}
		/// <summary>
		///		Gets or sets the specific value to compare with.
		/// </summary>
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(""),
			Description("Gets or sets the specific value to compare with.")
		]
		public string ValueToCompare
		{
			get { return (string) (ViewState["ValueToCompare"] ?? String.Empty); }
			set { ViewState["ValueToCompare"] = value; }
		}

		#endregion Control Attributes

		#region Protected Methods

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
				AddExpandoAttribute(expandoAttributeWriter, id, "evaluationfunction", "CompareValidatorEvaluateIsValid", false);
				if (ControlToCompare.Length > 0) {
					string controlToCompareID = GetControlRenderID(ControlToCompare);
					AddExpandoAttribute(expandoAttributeWriter, id, "controltocompare", controlToCompareID);
					AddExpandoAttribute(expandoAttributeWriter, id, "controlhookup", controlToCompareID);
				}
				if (ValueToCompare.Length > 0) {
					string valueToCompareString = ValueToCompare;
					if (CultureInvariantValues)
						valueToCompareString = ConvertCultureInvariantToCurrentCultureFormat(valueToCompareString, Type);
					AddExpandoAttribute(expandoAttributeWriter, id, "valuetocompare", valueToCompareString);
				}
				if (Operator != WebControls.ValidationCompareOperator.Equal)
					AddExpandoAttribute(expandoAttributeWriter, id, "operator", PropertyConverter.EnumToString(typeof(WebControls.ValidationCompareOperator), Operator), false);
			}
		}
		/// <summary>
		///		Checks the properties of a the control for valid values.
		/// </summary>
		/// <returns>true if the control properties are valid; otherwise, false.</returns>
		protected override bool ControlPropertiesValid()
		{
			// Check the control id references
			if (ControlToCompare.Length > 0) {
				CheckControlValidationProperty(ControlToCompare, "ControlToCompare");

				if (EqualsIgnoreCase(ControlToValidate, ControlToCompare))
					throw new HttpException("SR.GetString(SR.Validator_bad_compare_control, ID, ControlToCompare)");
			}
			else {
				// Check Values
				if (Operator != WebControls.ValidationCompareOperator.DataTypeCheck && !CanConvert(ValueToCompare, Type, CultureInvariantValues))
					throw new HttpException(@" SR.GetString( SR.Validator_value_bad_type, new string[] { ValueToCompare, ""ValueToCompare"", ID, PropertyConverter.EnumToString(typeof(WebControls.ValidationDataType), Type) } ) ");
			}
			return base.ControlPropertiesValid();
		}
		/// <summary>
		///		When overridden in a derived class, this method contains the code to determine whether the value in the input control is valid.
		/// </summary>
		/// <returns>true if the value in the input control is valid; otherwise, false.</returns>
		protected override bool EvaluateIsValid()
		{
			Debug.Assert(PropertiesValid, "Properties should have already been checked");

			// Get the peices of text from the control.
			string leftText = GetControlValidationValue(ControlToValidate);
			Debug.Assert(leftText != null, "Should have already caught this!");

			// Special case: if the string is blank, we don't try to validate it. The input should be 
			// trimmed for coordination with the RequiredFieldValidator.
			if (leftText.Trim().Length == 0) return true;

			// VSWhidbey 83168 
			bool convertDate = (Type == WebControls.ValidationDataType.Date && !DetermineRenderUplevel());
			if (convertDate && !IsInStandardDateFormat(leftText)) leftText = ConvertToShortDateString(leftText);

			// The control has precedence over the fixed value
			bool isCultureInvariantValue = false;
			string rightText = string.Empty;
			if (ControlToCompare.Length > 0) {
				rightText = GetControlValidationValue(ControlToCompare);
				Debug.Assert(rightText != null, "Should have already caught this!");

				// VSWhidbey 83089
				if (convertDate && !IsInStandardDateFormat(rightText)) rightText = ConvertToShortDateString(rightText);
			}
			else {
				rightText = ValueToCompare;
				isCultureInvariantValue = CultureInvariantValues;
			}

			return Compare(leftText, false, rightText, isCultureInvariantValue, Operator, Type);
		}

		#endregion Protected Methods

		#region Internal Methods

		/// <summary>
		///		
		/// </summary>
		/// <param name="s1"></param>
		/// <param name="s2"></param>
		/// <returns></returns>
		internal static bool EqualsIgnoreCase(string s1, string s2)
		{
			if (String.IsNullOrEmpty(s1) && String.IsNullOrEmpty(s2))
				return true;
			if (String.IsNullOrEmpty(s1) || String.IsNullOrEmpty(s2))
				return false;
			if (s2.Length != s1.Length)
				return false;
			return 0 == string.Compare(s1, 0, s2, 0, s2.Length, StringComparison.OrdinalIgnoreCase);
		}

		#endregion Internal Methods
	}
}