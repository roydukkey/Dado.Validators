//---------------------------------------------------------------------------------
// Dado Validators, Copyright 2013 roydukkey, 2013-04-04 (Thur, 04 April 2013).
// Dual licensed under the MIT (http://www.roydukkey.com/mit) and
// GPL Version 2 (http://www.roydukkey.com/gpl) licenses.
//---------------------------------------------------------------------------------

namespace Dado.Validators
{
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Diagnostics;
	using System.Web.UI;
	using WebControls = System.Web.UI.WebControls;

	/// <summary>
	///		Allows custom code to perform validation on the client and/or server.
	/// </summary>
	[
		DefaultEvent("ServerValidate"),
		ToolboxData("<{0}:CustomValidator runat=\"server\" ControlToValidate=\"ControlId\" />"),
		ToolboxBitmap(typeof(ResFinder), "Dado.image.custom.bmp")
	]
	public class CustomValidator : BaseValidator
	{
		#region Private Fields

		private static readonly object EventServerValidate = new object();

		#endregion Private Fields

		#region Control Attributes

		/// <summary>
		///		Gets and sets the custom client Javascript function used for validation.
		/// </summary>
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(""),
			Description("Gets and sets the custom client Javascript function used for validation.")
		]
		public string ClientValidationFunction
		{
			get { return (string) (ViewState["ClientValidationFunction"] ?? String.Empty); }
			set { ViewState["ClientValidationFunction"] = value; }
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
			get { return (bool) (ViewState["ValidateEmptyText"] ?? false); }
			set { ViewState["ValidateEmptyText"] = value; }
		}

		#endregion Control Attributes

		#region Public Events

		/// <summary>
		///		Represents the method that will handle the <see langword='ServerValidate'/> event of a <see cref='Dado.Validators.CustomValidator'/>.
		/// </summary>
		[
			Description("Represents the method that will handle the ServerValidate event of a CustomValidator.")
		]
		public event WebControls.ServerValidateEventHandler ServerValidate
		{
			add { Events.AddHandler(EventServerValidate, value); }
			remove { Events.RemoveHandler(EventServerValidate, value); }
		}

		#endregion Public Events

		#region Protected Methods

		/// <summary>
		///		Adds the properties of the <see cref='Dado.Validators.CustomValidator'/> control to the output stream for rendering on the client.
		/// </summary>
		/// <param name="writer">A <see cref='System.Web.UI.HtmlTextWriter'/> that contains the output stream for rendering on the client.</param>
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			base.AddAttributesToRender(writer);
			if (RenderUplevel) {
				string id = ClientID;
				HtmlTextWriter expandoAttributeWriter = (EnableLegacyRendering) ? writer : null;
				AddExpandoAttribute(expandoAttributeWriter, id, "evaluationfunction", "CustomValidatorEvaluateIsValid", false);
				if (ClientValidationFunction.Length > 0) {
					AddExpandoAttribute(expandoAttributeWriter, id, "clientvalidationfunction", ClientValidationFunction);
					if (ValidateEmptyText)
						AddExpandoAttribute(expandoAttributeWriter, id, "validateemptytext", "true", false);
				}
			}
		}
		/// <summary>
		///		Checks the properties of the control for valid values.
		/// </summary>
		/// <returns>true if the control properties are valid; otherwise, false.</returns>
		protected override bool ControlPropertiesValid()
		{
			// Need to override the BaseValidator implementation, because for CustomValidator, it is fine
			// for the ControlToValidate to be blank.
			string controlToValidate = ControlToValidate;
			if (controlToValidate.Length > 0)
				// Check that the property points to a valid control. Will throw and exception if not found
				CheckControlValidationProperty(controlToValidate, "ControlToValidate");
			return true;
		}
		/// <summary>
		///		Called during the validation stage when ASP.NET processes a Web Form.
		/// </summary>
		/// <returns>true if the value in the input control is valid; otherwise, false.</returns>
		protected override bool EvaluateIsValid()
		{
			// If no control is specified, we always fire the event. If they have specified a control, we 
			// only fire the event if the input is non-blank.
			string controlValue = String.Empty;
			string controlToValidate = ControlToValidate;
			if (controlToValidate.Length > 0) {
				controlValue = GetControlValidationValue(controlToValidate);
				Debug.Assert(controlValue != null, "Should have been caught be property check");
				// If the text is empty, we return true. Whitespace is ignored for coordination with RequiredFieldValidator.
				if ((controlValue == null || controlValue.Trim().Length == 0) && !ValidateEmptyText) return true;
			}

			return OnServerValidate(controlValue);
		}
		/// <summary>
		///		Raises the <see langword='ServerValidate'/> event for the <see cref='System.Web.UI.WebControls.CustomValidator'/>.
		/// </summary>
		/// <param name="value">The value to validate. </param>
		/// <returns>true if the value specified by the value parameter passes validation; otherwise, false.</returns>
		protected virtual bool OnServerValidate(string value)
		{
			WebControls.ServerValidateEventHandler handler = (WebControls.ServerValidateEventHandler)Events[EventServerValidate];
			WebControls.ServerValidateEventArgs args = new WebControls.ServerValidateEventArgs(value, true);
			if (handler != null) {
				handler(this, args);
				return args.IsValid;
			}
			else return true;
		}

		#endregion Protected Methods
	}
}
