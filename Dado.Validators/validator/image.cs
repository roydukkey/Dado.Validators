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
	using System.Web.UI;
	using WebControls = System.Web.UI.WebControls;

	/// <summary>
	///		Checks if the value of the associated input control meets exact, minimum, and maximum height and width constraints.
	/// </summary>
	[
		ToolboxData("<{0}:ImageValidator runat=\"server\" ControlToValidate=\"ControlId\" MaximumWidth=\"ImageValidator\" />"),
		ToolboxBitmap(typeof(ResFinder), "Dado.image.custom.bmp")
	]
	public class ImageValidator : BaseValidator
	{
		#region Fields

		private const string ExactWidthErrorMessage_Default = "The image width must be {0}px.";
		private const string MinimumWidthErrorMessage_Default = "The image width must not be less than {0}px.";
		private const string MaximumWidthErrorMessage_Default = "The image width must not be more than {0}px.";
		private const string ExactHeightErrorMessage_Default = "The image height must be {0}px.";
		private const string MinimumHeightErrorMessage_Default = "The image height must not be less than {0}px.";
		private const string MaximumHeightErrorMessage_Default = "The image height must not be more than {0}px.";

		#endregion Fields

		#region Control Attributes

		/// <summary>
		///		Make ErrorMessage private because there aren't generic ErrorMessages
		/// </summary>
		new private string ErrorMessage
		{
			get { return base.ErrorMessage; }
			set { base.ErrorMessage = value; }
		}
		/// <summary>
		///		Make EnableClientScript private because currently client-side validation isn't supported.
		///		Gets or sets a value indicating whether client-side validation is enabled.
		/// </summary>
		new private bool EnableClientScript
		{
			get { return base.EnableClientScript; }
			set { base.EnableClientScript = value; }
		}
		/// <summary>
		///		Indicates the exact width of the image in the control.
		/// </summary>
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(0),
			Description("Indicates the exact width of the image in the control.")
		]
		public int ExactWidth
		{
			get { return (int)(ViewState["ExactWidth"] ?? default(int)); }
			set { ViewState["ExactWidth"] = value; }
		}
		/// <summary>
		///		Message to display when ExactWidth is surpassed.
		/// </summary>
		[
			Category("Appearance"),
			Themeable(false),
			DefaultValue(ExactWidthErrorMessage_Default),
			Description("Message to display when ExactWidth is surpassed.")
		]
		public string ExactWidthErrorMessage
		{
			get { return String.Format((string)(ViewState["ExactWidthErrorMessage"] ?? ExactWidthErrorMessage_Default), ExactWidth); }
			set { ViewState["ExactWidthErrorMessage"] = value; }
		}
		/// <summary>
		///		Indicates the minimum width of the image of the control.
		/// </summary>
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(0),
			Description("Indicates the minimum width of the image in the control.")
		]
		public int MinimumWidth
		{
			get { return (int)(ViewState["MinimumWidth"] ?? default(int)); }
			set { ViewState["MinimumWidth"] = value; }
		}
		/// <summary>
		///		Message to display when MinimumWidth is surpassed.
		/// </summary>
		[
			Category("Appearance"),
			Themeable(false),
			DefaultValue(MinimumWidthErrorMessage_Default),
			Description("Message to display when MinimumWidth is surpassed.")
		]
		public string MinimumWidthErrorMessage
		{
			get { return String.Format((string)(ViewState["MinimumWidthErrorMessage"] ?? MinimumWidthErrorMessage_Default), MinimumWidth); }
			set { ViewState["MinimumWidthErrorMessage"] = value; }
		}
		/// <summary>
		///		Indicates the maximum width of the image in the control.
		/// </summary>
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(0),
			Description("Indicates the maximum width of the image in the control.")
		]
		public int MaximumWidth
		{
			get { return (int)(ViewState["MaximumWidth"] ?? default(int)); }
			set { ViewState["MaximumWidth"] = value; }
		}
		/// <summary>
		///		Message to display when MaximumWidth is surpassed.
		/// </summary>
		[
			Category("Appearance"),
			Themeable(false),
			DefaultValue(MaximumWidthErrorMessage_Default),
			Description("Message to display when MaximumWidth is surpassed.")
		]
		public string MaximumWidthErrorMessage
		{
			get { return String.Format((string)(ViewState["MaximumWidthErrorMessage"] ?? MaximumWidthErrorMessage_Default), MaximumWidth); }
			set { ViewState["MaximumWidthErrorMessage"] = value; }
		}
		/// <summary>
		///		Indicates the exact height of the image in the control.
		/// </summary>
		[
		Category("Behavior"),
		Themeable(false),
		DefaultValue(0),
		Description("Indicates the exact height of the image in the control.")
		]
		public int ExactHeight
		{
			get { return (int)(ViewState["ExactHeight"] ?? default(int)); }
			set { ViewState["ExactHeight"] = value; }
		}
		/// <summary>
		///		Message to display when ExactHeight is surpassed.
		/// </summary>
		[
			Category("Appearance"),
			Themeable(false),
			DefaultValue(ExactHeightErrorMessage_Default),
			Description("Message to display when ExactHeight is surpassed.")
		]
		public string ExactHeightErrorMessage
		{
			get { return String.Format((string)(ViewState["ExactHeightErrorMessage"] ?? ExactHeightErrorMessage_Default), ExactHeight); }
			set { ViewState["ExactHeightErrorMessage"] = value; }
		}
		/// <summary>
		///		Indicates the minimum height of the image of the control.
		/// </summary>
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(0),
			Description("Indicates the minimum height of the image in the control.")
		]
		public int MinimumHeight
		{
			get { return (int)(ViewState["MinimumHeight"] ?? default(int)); }
			set { ViewState["MinimumHeight"] = value; }
		}
		/// <summary>
		///		Message to display when MinimumHeight is surpassed.
		/// </summary>
		[
			Category("Appearance"),
			Themeable(false),
			DefaultValue(MinimumHeightErrorMessage_Default),
			Description("Message to display when MinimumHeight is surpassed.")
		]
		public string MinimumHeightErrorMessage
		{
			get { return String.Format((string)(ViewState["MinimumHeightErrorMessage"] ?? MinimumHeightErrorMessage_Default), MinimumHeight); }
			set { ViewState["MinimumHeightErrorMessage"] = value; }
		}
		/// <summary>
		///		Indicates the maximum height of the image in the control.
		/// </summary>
		[
			Category("Behavior"),
			Themeable(false),
			DefaultValue(0),
			Description("Indicates the maximum height of the image in the control.")
		]
		public int MaximumHeight
		{
			get { return (int)(ViewState["MaximumHeight"] ?? default(int)); }
			set { ViewState["MaximumHeight"] = value; }
		}
		/// <summary>
		///		Message to display when MaximumHeight is surpassed.
		/// </summary>
		[
			Category("Appearance"),
			Themeable(false),
			DefaultValue(MaximumHeightErrorMessage_Default),
			Description("Message to display when MaximumHeight is surpassed.")
		]
		public string MaximumHeightErrorMessage
		{
			get { return String.Format((string)(ViewState["MaximumHeightErrorMessage"] ?? MaximumHeightErrorMessage_Default), MaximumHeight); }
			set { ViewState["MaximumHeightErrorMessage"] = value; }
		}

		#endregion Control Attributes

		#region Constructor

		/// <summary>
		///		Initializes a new instance of the BaseValidator class.
		/// </summary>
		public ImageValidator() : base()
		{
			EnableClientScript = false;
		}

		#endregion Constructor

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
				//AddExpandoAttribute(expandoAttributeWriter, id, "evaluationfunction", "ImageDimensionsValidatorEvaluateIsValid", false);
				AddExpandoAttribute(expandoAttributeWriter, id, "exactwidth", ExactWidth.ToString());
				AddExpandoAttribute(expandoAttributeWriter, id, "exactwidtherrormessage", ExactWidthErrorMessage);
				AddExpandoAttribute(expandoAttributeWriter, id, "exactheight", ExactHeight.ToString());
				AddExpandoAttribute(expandoAttributeWriter, id, "exactheighterrormessage", ExactHeightErrorMessage);
				AddExpandoAttribute(expandoAttributeWriter, id, "minimumwidth", MinimumWidth.ToString());
				AddExpandoAttribute(expandoAttributeWriter, id, "minimumwidtherrormessage", MinimumWidthErrorMessage);
				AddExpandoAttribute(expandoAttributeWriter, id, "minimumheight", MinimumHeight.ToString());
				AddExpandoAttribute(expandoAttributeWriter, id, "minimumheighterrormessage", MinimumHeightErrorMessage);
				AddExpandoAttribute(expandoAttributeWriter, id, "maximumwidth", MaximumWidth.ToString());
				AddExpandoAttribute(expandoAttributeWriter, id, "maximumwidtherrormessage", MaximumWidthErrorMessage);
				AddExpandoAttribute(expandoAttributeWriter, id, "maximumheight", MaximumHeight.ToString());
				AddExpandoAttribute(expandoAttributeWriter, id, "maximumheighterrormessage", MaximumHeightErrorMessage);
			}
		}
		/// <summary>
		///		Called during the validation stage when ASP.NET processes a Web Form.
		/// </summary>
		/// <returns>true if the value in the input control is valid; otherwise, false.</returns>
		protected override bool EvaluateIsValid()
		{
			Control control = FindControl(this.ControlToValidate);
			if (control is WebControls.FileUpload) {
				WebControls.FileUpload fileUpload = (WebControls.FileUpload)control;

				// Check if Lengths nullify each other
				if (MaximumWidth > 0 && MinimumWidth > MaximumWidth || MaximumHeight > 0 && MinimumHeight > MaximumHeight) {
					Debug.Fail("MinimumWidth cannot be great than MaximumWidth and MinimumHeight cannot be great than MaximumHeight.");
					return false;
				}
				// Do not validate empty control. Use a required field validator.
				if (fileUpload.PostedFile.ContentLength > 0) {
					using (Image image = Image.FromStream(fileUpload.PostedFile.InputStream)) {
						// See if value not is equal to ExactWidth
						if (ExactWidth > 0 && image.Width != ExactWidth) {
							ErrorMessage = ExactWidthErrorMessage;
							return false;
						}

						// See if value not is equal to ExactHeight
						if (ExactHeight > 0 && image.Height != ExactHeight) {
							ErrorMessage = ExactHeightErrorMessage;
							return false;
						}

						// See if value is less than MinimumWidth
						if (MinimumWidth > 0 && image.Width < MinimumWidth) {
							ErrorMessage = MinimumWidthErrorMessage;
							return false;
						}

						// See if value is less than MinimumHeight
						if (MinimumHeight > 0 && image.Height < MinimumHeight) {
							ErrorMessage = MinimumHeightErrorMessage;
							return false;
						}

						// See if value is greater than MaximumWidth
						if (MaximumWidth > 0 && image.Width > MaximumWidth) {
							ErrorMessage = MaximumWidthErrorMessage;
							return false;
						}

						// See if value is greater than MaximumHeight
						if (MaximumHeight > 0 && image.Height > MaximumHeight) {
							ErrorMessage = MaximumHeightErrorMessage;
							return false;
						}
					}
				}
			}
			return true;
		}

		#endregion Protected Methods
	}
}