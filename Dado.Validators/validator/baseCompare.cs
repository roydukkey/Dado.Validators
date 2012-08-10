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
	using System.Text.RegularExpressions;
	using System.Web.UI;
	using WebControls = System.Web.UI.WebControls;

	/// <summary>
	///		Serves as the abstract base class for validation controls that perform typed comparisons.
	/// </summary>
	public abstract class BaseCompareValidator : BaseValidator
	{
		#region Control Attributes

		/// <summary>
		///		Gets or sets the data type that the values being compared are converted to before the comparison is made.
		/// </summary>
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(WebControls.ValidationDataType.String),
			Description("Gets or sets the data type that the values being compared are converted to before the comparison is made.")
		]
		public WebControls.ValidationDataType Type
		{
			get { return (WebControls.ValidationDataType) (ViewState["Type"] ?? WebControls.ValidationDataType.String); }
			set {
				if (value < WebControls.ValidationDataType.String || value > WebControls.ValidationDataType.Currency)
					throw new ArgumentOutOfRangeException("value");
				ViewState["Type"] = value;
			}
		}
 		/// <summary>
		///		Gets or sets a value indicating whether values are converted to a culture-neutral format before being compared.
 		/// </summary>
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(false),
			Description("Gets or sets a value indicating whether values are converted to a culture-neutral format before being compared.")
		]
		public bool CultureInvariantValues
		{
			get { return (bool) (ViewState["CultureInvariantValues"] ?? false); }
			set { ViewState["CultureInvariantValues"] = value; }
		}

		#endregion Control Attributes

		#region Protected Properties

		/// <summary>
		///		Gets the maximum year that can be represented by a two-digit year.
		/// </summary>
		protected static int CutoffYear
		{
			get { return DateTimeFormatInfo.CurrentInfo.Calendar.TwoDigitYearMax; }
		}

		#endregion Protected Properties

		#region Public Methods
		/// <summary>
		///		Determines whether the specified string can be converted to the specified data type. This version of the overloaded method tests currency, double, and date values using the format used by the current culture.
		/// </summary>
		/// <param name="text">The string to test.</param>
		/// <param name="type">One of the <see cref='System.Web.UI.WebControls.ValidationDataType'/> values.</param>
		/// <returns>true if the specified data string can be converted to the specified data type; otherwise, false.</returns>
		public static bool CanConvert(string text, WebControls.ValidationDataType type)
		{
			return CanConvert(text, type, false);
		}
		/// <summary>
		///		Determines whether the specified string can be converted to the specified data type. This version of the overloaded method allows you to specify whether values are tested using a culture-neutral format.
		/// </summary>
		/// <param name="text">The string to test.</param>
		/// <param name="type">One of the <see cref='System.Web.UI.WebControls.ValidationDataType'/> enumeration values.</param>
		/// <param name="cultureInvariant">true to test values using a culture-neutral format; otherwise, false.</param>
		/// <returns>true if the specified data string can be converted to the specified data type; otherwise, false.</returns>
		public static bool CanConvert(string text, WebControls.ValidationDataType type, bool cultureInvariant)
		{
			object value = null;
			return Convert(text, type, cultureInvariant, out value);
		}

		#endregion Public Methods

		#region Protected Methods

		/// <summary>
		///		Adds the HTML attributes and styles that need to be rendered for the control to the specified <see cref='System.Web.UI.HtmlTextWriter'/> object.
		/// </summary>
		/// <param name="writer">A <see cref='System.Web.UI.HtmlTextWriter'/> that contains the output stream for rendering on the client.</param>
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			base.AddAttributesToRender(writer);
			if (RenderUplevel) {
				WebControls.ValidationDataType type = Type;
				if (type != WebControls.ValidationDataType.String) {
					string id = ClientID;
					HtmlTextWriter expandoAttributeWriter = (EnableLegacyRendering) ? writer : null;

					AddExpandoAttribute(expandoAttributeWriter, id, "type", PropertyConverter.EnumToString(typeof(WebControls.ValidationDataType), type), false);

					NumberFormatInfo info = NumberFormatInfo.CurrentInfo;
					if (type == WebControls.ValidationDataType.Double) {
						string decimalChar = info.NumberDecimalSeparator;
						AddExpandoAttribute(expandoAttributeWriter, id, "decimalchar", decimalChar);
					}
					else if (type == WebControls.ValidationDataType.Currency) {
						string decimalChar = info.CurrencyDecimalSeparator;
						AddExpandoAttribute(expandoAttributeWriter, id, "decimalchar", decimalChar);

						string groupChar = info.CurrencyGroupSeparator;
						// Map non-break space onto regular space for parsing 
						if (groupChar[0] == 160)
							groupChar = " ";
						AddExpandoAttribute(expandoAttributeWriter, id, "groupchar", groupChar);

						int digits = info.CurrencyDecimalDigits;
						AddExpandoAttribute(expandoAttributeWriter, id, "digits", digits.ToString(NumberFormatInfo.InvariantInfo), false);

						// VSWhidbey 83165
						int groupSize = GetCurrencyGroupSize(info);
						if (groupSize > 0) {
							AddExpandoAttribute(expandoAttributeWriter, id, "groupsize", groupSize.ToString(NumberFormatInfo.InvariantInfo), false);
						}
					}
					else if (type == WebControls.ValidationDataType.Date) {
						AddExpandoAttribute(expandoAttributeWriter, id, "dateorder", GetDateElementOrder(), false);
						AddExpandoAttribute(expandoAttributeWriter, id, "cutoffyear", CutoffYear.ToString(NumberFormatInfo.InvariantInfo), false);

						// VSWhidbey 504553: The changes of this bug make client-side script not 
						// using the century attribute anymore, but still generating it for
						// backward compatibility with Everett pages. 
						int currentYear = DateTime.Today.Year;
						int century = currentYear - (currentYear % 100);
						AddExpandoAttribute(expandoAttributeWriter, id, "century", century.ToString(NumberFormatInfo.InvariantInfo), false);
					}
				}
			}
		}
		/// <summary>
		///		Determines the order in which the month, day, and year appear in a date value for the current culture.
		/// </summary>
		/// <returns>A string that represents the order in which the month, day, and year appear in a date value for the current culture.</returns>
		protected static string GetDateElementOrder()
		{
			DateTimeFormatInfo info = DateTimeFormatInfo.CurrentInfo;
			string shortPattern = info.ShortDatePattern;
			if (shortPattern.IndexOf('y') < shortPattern.IndexOf('M'))
				return "ymd";
			else if (shortPattern.IndexOf('M') < shortPattern.IndexOf('d'))
				return "mdy";
			else
				return "dmy";
		}
		/// <summary>
		///		Generates the four-digit year representation of the specified two-digit year.
		/// </summary>
		/// <param name="shortYear">A two-digit year.</param>
		/// <returns>The four-digit year representation of the specified two-digit year.</returns>
		protected static int GetFullYear(int shortYear)
		{
			Debug.Assert(shortYear >= 0 && shortYear < 100);
			return DateTimeFormatInfo.CurrentInfo.Calendar.ToFourDigitYear(shortYear);
		}
		/// <summary>
		///		Try to convert the test into the validation data type
		/// </summary>
		/// <param name="text"></param>
		/// <param name="type"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		protected static bool Convert(string text, WebControls.ValidationDataType type, out object value)
		{
			return Convert(text, type, false, out value);
		}
		/// <summary>
		///		Try to convert the test into the validation data type
		/// </summary>
		/// <param name="text"></param>
		/// <param name="type"></param>
		/// <param name="cultureInvariant"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		protected static bool Convert(string text, WebControls.ValidationDataType type, bool cultureInvariant, out object value)
		{
			value = null;
			try {
				switch (type) {
					case WebControls.ValidationDataType.String:
						value = text;
						break;
					case WebControls.ValidationDataType.Integer:
						value = Int32.Parse(text, CultureInfo.InvariantCulture);
						break;
					case WebControls.ValidationDataType.Double: {
							string cleanInput;
							if (cultureInvariant)
								cleanInput = ConvertDouble(text, CultureInfo.InvariantCulture.NumberFormat);
							else
								cleanInput = ConvertDouble(text, NumberFormatInfo.CurrentInfo);

							if (cleanInput != null)
								value = Double.Parse(cleanInput, CultureInfo.InvariantCulture);
							break;
						}
					case WebControls.ValidationDataType.Date: {
							if (cultureInvariant)
								value = ConvertDate(text, "ymd");
							else {
								// if the calendar is not gregorian, we should not enable client-side, so just parse it directly: 
								if (!(DateTimeFormatInfo.CurrentInfo.Calendar.GetType() == typeof(GregorianCalendar))) {
									value = DateTime.Parse(text, CultureInfo.CurrentCulture);
									break;
								}

								string dateElementOrder = GetDateElementOrder();
								value = ConvertDate(text, dateElementOrder);
							}
							break;
						}
					case WebControls.ValidationDataType.Currency: {
							string cleanInput;
							if (cultureInvariant)
								cleanInput = ConvertCurrency(text, CultureInfo.InvariantCulture.NumberFormat);
							else
								cleanInput = ConvertCurrency(text, NumberFormatInfo.CurrentInfo);

							if (cleanInput != null)
								value = Decimal.Parse(cleanInput, CultureInfo.InvariantCulture);
							break;
						}
				}
			}
			catch {
				value = null;
			}
			return (value != null);
		}
		/// <summary>
		///		Compare two strings using the type and operator
		/// </summary>
		/// <param name="leftText"></param>
		/// <param name="rightText"></param>
		/// <param name="op"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		protected static bool Compare(string leftText, string rightText, WebControls.ValidationCompareOperator op, WebControls.ValidationDataType type)
		{
			return Compare(leftText, false, rightText, false, op, type);
		}
		/// <summary>
		///		Compare two strings using the type and operator
		/// </summary>
		/// <param name="leftText"></param>
		/// <param name="cultureInvariantLeftText"></param>
		/// <param name="rightText"></param>
		/// <param name="cultureInvariantRightText"></param>
		/// <param name="op"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		protected static bool Compare(string leftText, bool cultureInvariantLeftText, string rightText, bool cultureInvariantRightText, WebControls.ValidationCompareOperator op, WebControls.ValidationDataType type)
		{
			object leftObject;
			if (!Convert(leftText, type, cultureInvariantLeftText, out leftObject)) return false;

			if (op == WebControls.ValidationCompareOperator.DataTypeCheck) return true;

			object rightObject;
			if (!Convert(rightText, type, cultureInvariantRightText, out rightObject)) return true;

			int compareResult;
			switch (type) {
				case WebControls.ValidationDataType.String:
					compareResult = String.Compare((string)leftObject, (string)rightObject, false, CultureInfo.CurrentCulture);
					break;
				case WebControls.ValidationDataType.Integer:
					compareResult = ((int)leftObject).CompareTo(rightObject);
					break;
				case WebControls.ValidationDataType.Double:
					compareResult = ((double)leftObject).CompareTo(rightObject);
					break;
				case WebControls.ValidationDataType.Date:
					compareResult = ((DateTime)leftObject).CompareTo(rightObject);
					break;
				case WebControls.ValidationDataType.Currency:
					compareResult = ((Decimal)leftObject).CompareTo(rightObject);
					break;
				default:
					Debug.Fail("Unknown Type");
					return true;
			}
			switch (op) {
				case WebControls.ValidationCompareOperator.Equal:
					return compareResult == 0;
				case WebControls.ValidationCompareOperator.NotEqual:
					return compareResult != 0;
				case WebControls.ValidationCompareOperator.GreaterThan:
					return compareResult > 0;
				case WebControls.ValidationCompareOperator.GreaterThanEqual:
					return compareResult >= 0;
				case WebControls.ValidationCompareOperator.LessThan:
					return compareResult < 0;
				case WebControls.ValidationCompareOperator.LessThanEqual:
					return compareResult <= 0;
				default:
					Debug.Fail("Unknown Operator");
					return true;
			}
		}
 		/// <summary>
		///		Determines whether the validation control can perform client-side validation.
 		/// </summary>
		/// <returns>true if the validation control can perform client-side validation; otherwise, false.</returns>
		protected override bool DetermineRenderUplevel()
		{
			// We don't do client-side validation for dates with non gregorian calendars
			return
				Type == WebControls.ValidationDataType.Date && DateTimeFormatInfo.CurrentInfo.Calendar.GetType() != typeof(GregorianCalendar) ?
				false :
				base.DetermineRenderUplevel();
		}

		#endregion Protected Methods

		#region Internal Methods

		/// <summary>
		///		Convert a short date to a string
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		internal string ConvertToShortDateString(string text)
		{
			// VSWhidbey 83099, 85305, we should ignore error if it happens and
			// leave text as intact when parsing the date.  We assume the caller
			// (validator) is able to handle invalid text itself.
			DateTime date;
			if (DateTime.TryParse(text, CultureInfo.CurrentCulture, DateTimeStyles.None, out date))
				text = date.ToShortDateString();
			return text;
		}
		/// <summary>
		///		Check if a string is in a standard data format.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		internal bool IsInStandardDateFormat(string date)
		{
			// VSWhidbey 115454: We identify that date string with only numbers
			// and specific punctuation separators is in standard date format. 
			const string standardDateExpression = "^\\s*(\\d+)([-/]|\\. ?)(\\d+)\\2(\\d+)\\s*$";
			return Regex.Match(date, standardDateExpression).Success;
		}
		/// <summary>
		///		Converts a culture invariant to a current culture format.
		/// </summary>
		/// <param name="valueInString"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		internal string ConvertCultureInvariantToCurrentCultureFormat(string valueInString, WebControls.ValidationDataType type)
		{
			object value;
			Convert(valueInString, type, true, out value);
			return
				value is DateTime ?
				// For Date type we explicitly want the date portion only
				((DateTime)value).ToShortDateString() :
				System.Convert.ToString(value, CultureInfo.CurrentCulture);
		}

		#endregion Internal Methods

		#region Private Methods

		/// <summary>
		///		VSWhidbey 83165
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		private static int GetCurrencyGroupSize(NumberFormatInfo info)
		{
			int[] groupSizes = info.CurrencyGroupSizes;
			return groupSizes != null && groupSizes.Length == 1 ? groupSizes[0] : -1;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="info"></param>
		/// <returns></returns>
		private static string ConvertCurrency(string text, NumberFormatInfo info)
		{
			string decimalChar = info.CurrencyDecimalSeparator;
			string groupChar = info.CurrencyGroupSeparator;

			// VSWhidbey 83165 
			string beginGroupSize, subsequentGroupSize;
			int groupSize = GetCurrencyGroupSize(info);
			if (groupSize > 0) {
				string groupSizeText = groupSize.ToString(NumberFormatInfo.InvariantInfo);
				beginGroupSize = "{1," + groupSizeText + "}";
				subsequentGroupSize = "{" + groupSizeText + "}";
			}
			else beginGroupSize = subsequentGroupSize = "+";

			// Map non-break space onto regular space for parsing 
			if (groupChar[0] == 160) groupChar = " ";
			int digits = info.CurrencyDecimalDigits;
			bool hasDigits = (digits > 0);
			string currencyExpression =
					"^\\s*([-\\+])?((\\d" + beginGroupSize + "(\\" + groupChar + "\\d" + subsequentGroupSize + ")+)|\\d*)"
					+ (hasDigits ? "\\" + decimalChar + "?(\\d{0," + digits.ToString(NumberFormatInfo.InvariantInfo) + "})" : string.Empty)
					+ "\\s*$";

			Match m = Regex.Match(text, currencyExpression);
			if (!m.Success) return null;

			// Make sure there are some valid digits 
			if (m.Groups[2].Length == 0 && hasDigits && m.Groups[5].Length == 0) return null;

			return m.Groups[1].Value
				+ m.Groups[2].Value.Replace(groupChar, string.Empty)
				+ ((hasDigits && m.Groups[5].Length > 0) ? "." + m.Groups[5].Value : string.Empty);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="info"></param>
		/// <returns></returns>
		private static string ConvertDouble(string text, NumberFormatInfo info)
		{
			// VSWhidbey 83156: If text is empty, it would be default to 0 for
			// backward compatibility reason. 
			if (text.Length == 0) return "0";

			string decimalChar = info.NumberDecimalSeparator;
			string doubleExpression = "^\\s*([-\\+])?(\\d*)\\" + decimalChar + "?(\\d*)\\s*$";

			Match m = Regex.Match(text, doubleExpression);
			if (!m.Success) return null;

			// Make sure there are some valid digits 
			if (m.Groups[2].Length == 0 && m.Groups[3].Length == 0) return null;

			return m.Groups[1].Value
				+ (m.Groups[2].Length > 0 ? m.Groups[2].Value : "0")
				+ ((m.Groups[3].Length > 0) ? "." + m.Groups[3].Value : string.Empty);
		}
		/// <summary>
		///		
		/// </summary>
		/// <param name="text"></param>
		/// <param name="dateElementOrder"></param>
		/// <returns></returns>
		/// <remarks>When updating the regular expressions in this method, you must also update the regular expressions in WebUIValidation.js::ValidatorConvert().  The server and client regular expressions must match.</remarks>
		private static object ConvertDate(string text, string dateElementOrder)
		{
			// always allow the YMD format, if they specify 4 digits
			string dateYearFirstExpression = "^\\s*((\\d{4})|(\\d{2}))([-/]|\\. ?)(\\d{1,2})\\4(\\d{1,2})\\.?\\s*$";
			Match m = Regex.Match(text, dateYearFirstExpression);
			int day, month, year;
			if (m.Success && (m.Groups[2].Success || dateElementOrder == "ymd")) {
				day = Int32.Parse(m.Groups[6].Value, CultureInfo.InvariantCulture);
				month = Int32.Parse(m.Groups[5].Value, CultureInfo.InvariantCulture);
				if (m.Groups[2].Success)
					year = Int32.Parse(m.Groups[2].Value, CultureInfo.InvariantCulture);
				else
					year = GetFullYear(Int32.Parse(m.Groups[3].Value, CultureInfo.InvariantCulture));
			}
			else {
				if (dateElementOrder == "ymd") return null;

				// also check for the year last format 
				string dateYearLastExpression = "^\\s*(\\d{1,2})([-/]|\\. ?)(\\d{1,2})(?:\\s|\\2)((\\d{4})|(\\d{2}))(?:\\s\u0433\\.|\\.)?\\s*$";
				m = Regex.Match(text, dateYearLastExpression);
				if (!m.Success) return null;
				if (dateElementOrder == "mdy") {
					day = Int32.Parse(m.Groups[3].Value, CultureInfo.InvariantCulture);
					month = Int32.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture);
				}
				else {
					day = Int32.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture);
					month = Int32.Parse(m.Groups[3].Value, CultureInfo.InvariantCulture);
				}
				if (m.Groups[5].Success)
					year = Int32.Parse(m.Groups[5].Value, CultureInfo.InvariantCulture);
				else
					year = GetFullYear(Int32.Parse(m.Groups[6].Value, CultureInfo.InvariantCulture));
			}
			return new DateTime(year, month, day);
		} 

		#endregion Private Methods
	}
}
