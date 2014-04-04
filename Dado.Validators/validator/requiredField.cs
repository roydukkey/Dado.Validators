//---------------------------------------------------------------------------------
// Dado Validators, Copyright 2014 roydukkey.
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
	using WebControls = System.Web.UI.WebControls;

	/// <summary>
	///		Makes the associated input control a required field.
	/// </summary>
	[
		ToolboxData("<{0}:RequiredFieldValidator runat=\"server\" ControlToValidate=\"ControlId\" />"),
		ToolboxBitmap(typeof(ResFinder), "Dado.image.requiredField.bmp")
	]
	public class RequiredFieldValidator : BaseValidator
	{
		#region Fields

		private const string DEFAULT_ERROR_MESSAGE = "Please enter a value.";
		private const string DEFAULT_CHECKBOX_ERROR_MESSAGE = "You must select this option.";
		private const string DEFAULT_LIST_ERROR_MESSAGE = "Please select an option.";
		private const string DEFAULT_FILE_ERROR_MESSAGE = "Please select a file.";
		private bool _isCheckBox = false;
		private bool _isCheckBoxList = false;
		private bool _isRadioButtonList = false;
		private bool _isFileUpload = false;

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
		///		Gets or sets the initial value of the associated input control.
		/// </summary>
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(""),
			Description("Gets or sets the initial value of the associated input control.")
		]
		public string InitialValue
		{
			get { return (string)(ViewState["InitialValue"] ?? String.Empty); }
			set { ViewState["InitialValue"] = value; }
		}

		#endregion Control Attributes
		
		#region Protected Methods

		/// <summary>
		///		Registers the validator on the page.
		/// </summary>
		/// <param name="e">A <see cref='System.EventArgs'/> that contains the event data.</param>
		protected override void OnInit(EventArgs e)
		{
			DefaultErrorMessage = PropertiesValid && _isCheckBox
				? DEFAULT_CHECKBOX_ERROR_MESSAGE
				: (_isCheckBoxList || _isRadioButtonList)
					? DEFAULT_LIST_ERROR_MESSAGE
					: _isFileUpload
						? DEFAULT_FILE_ERROR_MESSAGE
						: DEFAULT_ERROR_MESSAGE;
			base.OnInit(e);
		}
		/// <summary>
		///		Determines whether the control specified by the ControlToValidate property is a valid control.
		/// </summary>
		/// <returns>true if the control specified by ControlToValidate is a valid control; otherwise, false.</returns>
		protected override bool ControlPropertiesValid()
		{
			// Check for blank control to validate 
			string controlToValidate = ControlToValidate;
			if (controlToValidate.Length == 0)
				throw new Exception(
					String.Format("The {0} property of '{1}' cannot be blank.", "ControlToValidate", ID)
				);

			// Check that the property points to a valid control. Will throw and exception if not found 
			CheckControlValidationProperty(controlToValidate, "ControlToValidate");

			return true;
		}
		/// <summary>
		///		Verifies whether the specified control is on the page and contains validation properties.
		/// </summary>
		/// <param name="name">The control to verify.</param>
		/// <param name="propertyName">Additional text to describe the source of the exception, if an exception is thrown from using this method.</param>
		new protected void CheckControlValidationProperty(string name, string propertyName)
		{
			// get the control using the relative name
			Control c = NamingContainer.FindControl(name);
			if (c == null)
				throw new Exception(
					String.Format("Unable to find control id '{0}' referenced by the '{1}' property of '{2}'.", name, propertyName, ID)
				);

			// Add Validation for CheckBox
			if (_isCheckBox = (c is WebControls.CheckBox && !(c is WebControls.RadioButton))) return;

			// Add Validation for CheckBoxList
			else if (_isCheckBoxList = c is WebControls.CheckBoxList) return;

			// Allows Proper Default Error Message
			else if (_isRadioButtonList = c is WebControls.RadioButtonList) { }
			else { _isFileUpload = c is WebControls.FileUpload; }

			// get its validation property 
			PropertyDescriptor prop = GetValidationProperty(c);
			if (prop == null)
				throw new Exception(
					String.Format("Control '{0}' referenced by the {1} property of '{2}' cannot be validated.", name, propertyName, ID)
				);
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

				AddExpandoAttribute(expandoAttributeWriter, id, "evaluationfunction", "RequiredFieldValidatorEvaluateIsValid", false);
				AddExpandoAttribute(expandoAttributeWriter, id, "initialvalue", InitialValue);
				AddExpandoAttribute(expandoAttributeWriter, id, "validatefor",
					_isCheckBox
						? "checkbox"
						: _isCheckBoxList
							? "checkboxlist"
							: "default"
				);
			}
		}
		/// <summary>
		///		Called during the validation stage when ASP.NET processes a Web Form.
		/// </summary>
		/// <returns>true if the value in the input control is valid; otherwise, false.</returns>
		protected override bool EvaluateIsValid()
		{
			string controlValue = GetControlValidationValue(ControlToValidate);
			if (controlValue == null) {

				// Provide Validation for CheckBox.
				if (_isCheckBox) {
					return ((WebControls.CheckBox)NamingContainer.FindControl(ControlToValidate)).Checked;
				}
				// Provide Validation for CheckBoxList.
				else if (_isCheckBoxList) {
					WebControls.CheckBoxList c = (WebControls.CheckBoxList)NamingContainer.FindControl(ControlToValidate);

					foreach (WebControls.ListItem item in c.Items)
						if (item.Selected)
							return true;

					return false;
				}

				// Get the control value, return true if it is not found
				Debug.Fail("Should have been caught by PropertiesValid check");
				return true;
			}

			// See if the control has changed
			return !controlValue.Trim().Equals(InitialValue.Trim());
		}

		#endregion Protected Methods
	}
}