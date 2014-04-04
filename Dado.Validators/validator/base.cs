//---------------------------------------------------------------------------------
// Dado Validators, Copyright 2014 roydukkey.
// Dual licensed under the MIT (http://www.roydukkey.com/mit) and
// GPL Version 2 (http://www.roydukkey.com/gpl) licenses.
//---------------------------------------------------------------------------------

using System.Web.UI;

[assembly: TagPrefix("Dado.Validators", "Dado")]
namespace Dado.Validators
{
	using System;
	using System.ComponentModel;
	using System.Configuration;
	using System.Diagnostics;
	using System.Drawing;
	using System.Reflection;
	using System.Web.Compilation;
	using System.Web.Configuration;
	using WebControls = System.Web.UI.WebControls;

	/// <summary>
	///		The Base for all Dado Validators
	/// </summary>
	[
		DefaultProperty("ErrorMessage"),
		Designer("System.Web.UI.Design.WebControls.BaseValidatorDesigner, System.Design")
	]
	public abstract class BaseValidator : WebControls.BaseValidator
	{
		#region Fields

		private const string VALIDATOR_FILE_NAME = "Dado.js.validation.min.js";
    private const string VALIDATOR_INITIALIZE_SCRIPT = "DadoValidation_Initialize({0});";
    private const string VALIDATOR_SCRIPT_KEY = "Dado-Validators.Initialize";
		private const string DEFAULT_ERROR_MESSAGE = "Please enter a correct value.";

		private const string CSS_CLASS = "validator";
		private const string CSS_CLASS_INVALID = "invalid";
		private string _defaultErrorMessage = DEFAULT_ERROR_MESSAGE;
		private bool _isErrorMessageSet;

		private bool preRenderCalled;
		private static bool? _isPartialRenderingEnabled;
		private bool? _enableLegacyRendering;
		private string _webFormScriptReference;

		#endregion Fields

		#region Control Attributes

		/// <summary>
		///		Gets or sets the CSS class rendered by the Web control.
		/// </summary>
		[
			Category("Appearance"),
			DefaultValue(CSS_CLASS),
			Description("Gets or sets the CSS class rendered by the Web control."),
			CssClassProperty()
		]
		public override string CssClass
		{
			get { return base.CssClass; }
			set { base.CssClass = value; }
		}
		/// <summary>
		///		Gets or sets the CSS class rendered by the Web control when it is not valid.
		/// </summary>
		[
			Category("Appearance"),
			Themeable(false),
			DefaultValue(CSS_CLASS_INVALID),
			Description("Gets or sets the CSS class rendered by the Web control when it is not valid."),
			CssClassProperty()
		]
		public string CssClassInvalid
		{
			get { return (string)(ViewState["CssClassInvalid"] ?? CSS_CLASS_INVALID); }
			set { ViewState["CssClassInvalid"] = value; }
		}
		/// <summary>
		///		Gets or sets the text for the error message.
		/// </summary>
		[
			Localizable(true),
			Category("Appearance"),
			DefaultValue(DEFAULT_ERROR_MESSAGE),
			Description("Gets or sets the text for the error message.")
		]
		new public virtual string ErrorMessage
		{
			get { return _isErrorMessageSet ? base.ErrorMessage : DefaultErrorMessage; }
			set { _isErrorMessageSet = true; base.ErrorMessage = value; }
		}
		/// <summary>
		///		Gets or sets a value indicating whether client-side validation is enabled.
		/// </summary>
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(true),
			Description("Gets or sets a value indicating whether client-side validation is enabled.")
		]
		new public virtual bool EnableClientScript
		{
			get { return base.EnableClientScript; }
			set { base.EnableClientScript = value; }
		}
		
		/// <summary>
		///		Gets or sets the text color of validation messages.
		/// </summary>
		[
			Category("Appearance"),
			Themeable(false),
      DefaultValue(typeof(Color), "Empty"),
			Description("Gets or sets the text color of validation messages.")
		]
		public override Color ForeColor
		{
			get { return base.ForeColor; }
			set { base.ForeColor = value; }
		}
		
		#endregion Control Attributes

		#region Protected Properties

		/// <summary>
		///		Is "XHTML 1.0 Transitional" rendering allowed?
		/// </summary>
		protected bool EnableLegacyRendering
		{
			get {
				if (!_enableLegacyRendering.HasValue) {
					XhtmlConformanceSection xhtml = (XhtmlConformanceSection)WebConfigurationManager.GetSection("system.web/xhtmlConformance");

					if (xhtml != null && xhtml.ElementInformation.IsPresent) {
						_enableLegacyRendering = xhtml.Mode == XhtmlConformanceMode.Legacy;
					}

					xhtml = (XhtmlConformanceSection)ConfigurationManager.OpenMachineConfiguration().GetSection("system.web/xhtmlConformance");

					if (xhtml != null && xhtml.ElementInformation.IsPresent) {
						_enableLegacyRendering = xhtml.Mode == XhtmlConformanceMode.Legacy;
					}

					_enableLegacyRendering = false;
				}

				return _enableLegacyRendering.Value;
			}
		}
		/// <summary>
		///		Gets or sets the text for the default error message.
		/// </summary>
		protected virtual string DefaultErrorMessage
		{
			get { return _defaultErrorMessage; }
			set { _defaultErrorMessage = value; }
		}

		#endregion Protected Properties

		#region Private Properties

		/// <summary>
		///		Gets or sets the text for the reference to the form's script namespace.
		/// </summary>
		private string WebFormScriptReference
		{
			get {
				if (_webFormScriptReference == null) {
					Type type = typeof(Page);

					if (type == null) {
						_webFormScriptReference = "window";
					}
					else {
						PropertyInfo property = type.GetProperty("WebFormScriptReference", BindingFlags.Instance | BindingFlags.NonPublic);
						if (property == null)
							_webFormScriptReference = "window";
						else
							_webFormScriptReference = (string)property.GetValue(Page, null);
					}
				}

				return _webFormScriptReference;
			}
		}

		#endregion Private Properties

		#region Constructor

		/// <summary>
		///		Initializes a new instance of the BaseValidator class.
		/// </summary>
		protected BaseValidator() : base()
		{
			ForeColor = Color.Empty;
			DefaultErrorMessage = DEFAULT_ERROR_MESSAGE;
			CssClass = CSS_CLASS;
		}

		#endregion Constructor

		#region Protected Methods

		/// <summary>
		///		Checks the client brower and configures the validator for compatibility prior to rendering.
		/// </summary>
		/// <param name="e">A <see cref='System.EventArgs'/> that contains the event data.</param>
		protected override void OnPreRender(EventArgs e)
		{
			// Register override scripts
			if (DetermineRenderUplevel()) {
				ScriptManager manager = ScriptManager.GetCurrent(Page);

				if (manager != null) {
					ScriptManager.RegisterClientScriptBlock(this, typeof(BaseValidator), VALIDATOR_SCRIPT_KEY, String.Format(VALIDATOR_INITIALIZE_SCRIPT, WebFormScriptReference), true);

					base.OnPreRender(e);
					preRenderCalled = true;

					ScriptManager.RegisterClientScriptResource(this, typeof(BaseValidator), VALIDATOR_FILE_NAME);
				}
				else if(!Page.ClientScript.IsClientScriptBlockRegistered(typeof(BaseValidator), VALIDATOR_SCRIPT_KEY)) {
					//throw new Exception("We Are Here! B");

					Page.ClientScript.RegisterStartupScript(typeof(BaseValidator), VALIDATOR_SCRIPT_KEY, String.Format(VALIDATOR_INITIALIZE_SCRIPT, WebFormScriptReference), true);
					
					base.OnPreRender(e);
					preRenderCalled = true;

					Page.ClientScript.RegisterClientScriptResource(typeof(BaseValidator), VALIDATOR_FILE_NAME);
				}
			}
		}
		/// <summary>
		///		Displays the control on the client.
		/// </summary>
		/// <param name="writer">A <see cref='System.Web.UI.HtmlTextWriter'/> that contains the output stream for rendering on the client.</param>
		protected override void Render(HtmlTextWriter writer)
		{
			bool shouldBeVisible;

			// VSWhidbey 347677, 398978: Backward Compat.: Skip property checking if the validator doesn't have PreRender called and it is not in page control tree.
			if (DesignMode || (!preRenderCalled && Page == null)) {
				// This is for design time. In this case we don't want any expandos created, don't want property checks and always want to be visible.
				shouldBeVisible = true;
			}
			else shouldBeVisible = Enabled && !IsValid;

			// If server-side validation fails, add invalid class
			if (!Enabled) CssClass += " " + "aspNetDisabled";
			else if (!IsValid) CssClass += " " + CssClassInvalid;

			// No point rendering if we have errors 
			if (!PropertiesValid) return;

			// Make sure we are in a form tag with runat=server.
			if (Page != null) Page.VerifyRenderingInServerForm(this);

			// work out what we are displaying 
			WebControls.ValidatorDisplay display = Display;
			bool displayContents;
			bool displayTags;

			if (RenderUplevel) {
				displayTags = true;
				displayContents = display != WebControls.ValidatorDisplay.None;
			}
			else {
				displayContents = display != WebControls.ValidatorDisplay.None && shouldBeVisible;
				displayTags = displayContents;
			}

			// Put ourselves in the array
			if (displayTags && RenderUplevel)
				RegisterValidatorDeclaration();

			// Display it 
			if (displayTags) RenderBeginTag(writer);

			if (displayContents) {
				writer.Write("<span>");
				if (Text.Trim().Length > 0) {
					RenderContents(writer);
				}
				else if (base.HasControls() /* || HasRenderDelegate() Note: There may be an issue here, but as far as I know validator's don't depend on this */)
					base.RenderContents(writer);
				else
					writer.Write(ErrorMessage);
				writer.Write("</span>");
			}

			if (displayTags) RenderEndTag(writer);
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

				AddExpandoAttribute(expandoAttributeWriter, id, "invalidClassName", CssClassInvalid, false);
			}
		}

		#endregion Protected Methods

		#region Internal Methods

		/// <summary>
		///		Adds an attribute to be rendered for clientside validation.
		/// </summary>
		/// <param name="writer">An <see cref='System.Web.UI.HtmlTextWriter'/> that represents the output stream to render HTML content on the client.</param>
		/// <param name="controlId">The <see cref='System.Web.UI.Control'/> on the page that contains the custom attribute.</param>
		/// <param name="attributeName">The name of the custom attribute to register.</param>
		/// <param name="attributeValue">The value of the custom attribute.</param>
		internal void AddExpandoAttribute(HtmlTextWriter writer, string controlId, string attributeName, string attributeValue)
		{
			AddExpandoAttribute(writer, controlId, attributeName, attributeValue, true);
		}
		/// <summary>
		///		Adds an attribute to be rendered for clientside validation.
		/// </summary>
		/// <param name="writer">An <see cref='System.Web.UI.HtmlTextWriter'/> that represents the output stream to render HTML content on the client.</param>
		/// <param name="controlId">The <see cref='System.Web.UI.Control'/> on the page that contains the custom attribute.</param>
		/// <param name="attributeName">The name of the custom attribute to register.</param>
		/// <param name="attributeValue">The value of the custom attribute.</param>
		/// <param name="encode">A Boolean value indicating whether to encode the custom attribute to register.</param>
		internal void AddExpandoAttribute(HtmlTextWriter writer, string controlId, string attributeName, string attributeValue, bool encode)
		{
			AddExpandoAttribute(this, writer, controlId, attributeName, attributeValue, encode);
		}
		/// <summary>
		///		Adds an attribute to be rendered for clientside validation.
		/// </summary>
		/// <param name="control">The <see cref='System.Web.UI.Control'/> with the containing invocable member.</param>
		/// <param name="writer">An <see cref='System.Web.UI.HtmlTextWriter'/> that represents the output stream to render HTML content on the client.</param>
		/// <param name="controlId">The <see cref='System.Web.UI.Control'/> on the page that contains the custom attribute.</param>
		/// <param name="attributeName">The name of the custom attribute to register.</param>
		/// <param name="attributeValue">The value of the custom attribute.</param>
		/// <param name="encode">A Boolean value indicating whether to encode the custom attribute to register.</param>
		internal static void AddExpandoAttribute(Control control, HtmlTextWriter writer, string controlId, string attributeName, string attributeValue, bool encode)
		{
			// if writer is not null, assuming the expando attribute is written out explicitly 
			if (writer != null) {
				writer.AddAttribute(attributeName, attributeValue, encode);
			}
			else {
				Debug.Assert(control != null);
				Page page = control.Page;
				Debug.Assert(page != null);
				ScriptManager manager = ScriptManager.GetCurrent(page);

				if (manager != null) {
					ScriptManager.RegisterExpandoAttribute(control, controlId, attributeName, attributeValue, encode);
				}
				else {
					page.ClientScript.RegisterExpandoAttribute(controlId, attributeName, attributeValue, encode);
				}
			}
		}
		/// <summary>
		///		Determine whether partial rendering is supported.
		/// </summary>
		/// <param name="page"></param>
		/// <returns></returns>
		internal static bool IsPartialRenderingSupported(Page page)
		{
			if (!_isPartialRenderingEnabled.HasValue) {
				ScriptManager manager = ScriptManager.GetCurrent(page);
				_isPartialRenderingEnabled = manager != null && manager.SupportsPartialRendering;
			}

			return _isPartialRenderingEnabled.Value;
		}

		#endregion Internal Methods
	}
}
