//---------------------------------------------------------------------------------
// Dado Validators, Copyright 2013 roydukkey, 2013-01-11 (Fri, 11 January 2013).
// Dual licensed under the MIT (http://www.roydukkey.com/mit) and
// GPL Version 2 (http://www.roydukkey.com/gpl) licenses.
//---------------------------------------------------------------------------------

namespace Dado.Validators
{
	using System;
	using System.Drawing;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Web;
	using System.Web.UI;
	using WebControls = System.Web.UI.WebControls;

	/// <summary>
	///		Checks if the value of the associated input control is within some minimum and maximum values, which can be constant values or values of other controls.
	/// </summary>
	[
		ToolboxData("<{0}:RangeValidator runat=\"server\" ControlToValidate=\"ControlId\" />"),
		ToolboxBitmap(typeof(ResFinder), "Dado.image.range.bmp")
	]
	public class RangeValidator : BaseCompareValidator
	{
		#region Control Attributes

		/// <summary>
		///		Gets or sets the maximum value of the validation range.
		/// </summary>
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(""),
			Description("Gets or sets the maximum value of the validation range.")
		]
		public string MaximumValue
		{
			get { return (string)(ViewState["MaximumValue"] ?? String.Empty); }
			set { ViewState["MaximumValue"] = value; }
		}
		/// <summary>
		///		Gets or sets the minimum value of the validation range.
		/// </summary>
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(""),
			Description("Gets or sets the minimum value of the validation range.")
		]
		public string MinimumValue
		{
			get { return (string)(ViewState["MinimumValue"] ?? String.Empty); }
			set { ViewState["MinimumValue"] = value; }
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
				AddExpandoAttribute(expandoAttributeWriter, id, "evaluationfunction", "RangeValidatorEvaluateIsValid", false);
				string maxValueString = MaximumValue;
				string minValueString = MinimumValue;
				if (CultureInvariantValues) {
					maxValueString = ConvertCultureInvariantToCurrentCultureFormat(maxValueString, Type);
					minValueString = ConvertCultureInvariantToCurrentCultureFormat(minValueString, Type);
				}
				AddExpandoAttribute(expandoAttributeWriter, id, "maximumvalue", maxValueString);
				AddExpandoAttribute(expandoAttributeWriter, id, "minimumvalue", minValueString);
			}
		}
		/// <summary>
		///		This is a check of properties to determine any errors made by the developer
		/// </summary>
		/// <returns></returns>
		protected override bool ControlPropertiesValid()
		{
			ValidateValues();
			return base.ControlPropertiesValid();
		}
		/// <summary>
		///		Determines whether the content in the input control is valid.
		/// </summary>
		/// <returns>true if the control is valid; otherwise, false.</returns>
		protected override bool EvaluateIsValid()
		{
			Debug.Assert(PropertiesValid, "Should have already been checked");

			// Get the peices of text from the control(s).
			string text = GetControlValidationValue(ControlToValidate);
			Debug.Assert(text != null, "Should have already caught this!");

			// Special case: if the string is blank, we don't try to validate it. The input should be
			// trimmed for coordination with the RequiredFieldValidator. 
			if (text.Trim().Length == 0) return true;

			// VSWhidbey 83168 
			if (Type == WebControls.ValidationDataType.Date && !DetermineRenderUplevel() && !IsInStandardDateFormat(text))
				text = ConvertToShortDateString(text);
			return (
				Compare(text, false, MinimumValue, CultureInvariantValues, WebControls.ValidationCompareOperator.GreaterThanEqual, Type) &&
				Compare(text, false, MaximumValue, CultureInvariantValues, WebControls.ValidationCompareOperator.LessThanEqual, Type)
			);
		}

		#endregion Protected Methods

		#region Private Methods

		/// <summary>
		///		
		/// </summary>
		private void ValidateValues()
		{
			// Check the control values can be converted to data type 
			string maximumValue = MaximumValue;
			if (!CanConvert(maximumValue, Type, CultureInvariantValues))
				throw new HttpException(@"SR.GetString( SR.Validator_value_bad_type, new string[] { maximumValue, ""MaximumValue"", ID, PropertyConverter.EnumToString(typeof(ValidationDataType), Type) } )");

			string minumumValue = MinimumValue;
			if (!CanConvert(minumumValue, Type, CultureInvariantValues))
				throw new HttpException(@"SR.GetString( SR.Validator_value_bad_type, new string[] { minumumValue, ""MinimumValue"", ID, PropertyConverter.EnumToString(typeof(ValidationDataType), Type) } )");

			// Check for overlap. 
			if (Compare(minumumValue, CultureInvariantValues, maximumValue, CultureInvariantValues, WebControls.ValidationCompareOperator.GreaterThan, Type))
				throw new HttpException(@"SR.GetString( SR.Validator_range_overalap, new string[] { maximumValue, minumumValue, ID, } )");
		}

		#endregion Private Methods
	}
}
