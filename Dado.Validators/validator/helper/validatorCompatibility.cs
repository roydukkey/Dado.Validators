//---------------------------------------------------------------------------------
// Dado Validators, Copyright 2013 roydukkey.
// Dual licensed under the MIT (http://www.roydukkey.com/mit) and
// GPL Version 2 (http://www.roydukkey.com/gpl) licenses.
//---------------------------------------------------------------------------------

namespace Dado.Validators
{
	using System;
	using System.Diagnostics;
	using System.Web.UI;
	using System.Reflection;

	internal static class ValidatorCompatibilityHelper
	{
		#region Public Methods

		/// <summary>
		///		Registers a JavaScript array declaration with the <see cref='System.Web.UI.Page'/> object using an array name and array value.
		/// </summary>
		/// <param name="control">The <see cref='System.Web.UI.Control'/> with the containing invocable member.</param>
		/// <param name="arrayName">The array name to register.</param>
		/// <param name="arrayValue">The array value or values to register.</param>
		/// <remarks>Needed to support Validators in AJAX 1.0 (Windows OS Bugs 2015831)</remarks>
		public static void RegisterArrayDeclaration(Control control, string arrayName, string arrayValue)
		{
			Type scriptManagerType = Type.GetType("System.Web.UI.ScriptManager, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35", false);
			Debug.Assert(scriptManagerType != null);

			scriptManagerType.InvokeMember(
				"RegisterArrayDeclaration",
				BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod,
				null, /*binder*/
				null, /*target*/
				new object[] { control, arrayName, arrayValue }
			);
		}
		/// <summary>
		///		Registers the client script resource with the <see cref='System.Web.UI.Page'/> object using a type and a resource name.
		/// </summary>
		/// <param name="control">The <see cref='System.Web.UI.Control'/> with the containing invocable member.</param>
		/// <param name="type">The type of the client script resource to register.</param>
		/// <param name="resourceName">The name of the client script resource to register.</param>
		public static void RegisterClientScriptResource(Control control, Type type, string resourceName)
		{
			Type scriptManagerType = Type.GetType("System.Web.UI.ScriptManager, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35", false);
			Debug.Assert(scriptManagerType != null);

			scriptManagerType.InvokeMember(
				"RegisterClientScriptResource",
				BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod,
				null, /*binder*/
				null, /*target*/
				new object[] { control, type, resourceName }
			);
		}
		/// <summary>
		///		Registers a name/value pair as a custom (expando) attribute of the specified control given a control ID, an attribute name, an attribute value, and a Boolean value indicating whether to encode the attribute value.
		/// </summary>
		/// <param name="control">The <see cref='System.Web.UI.Control'/> with the containing invocable member.</param>
		/// <param name="controlId">The <see cref='System.Web.UI.Control'/> on the page that contains the custom attribute.</param>
		/// <param name="attributeName">The name of the custom attribute to register.</param>
		/// <param name="attributeValue">The value of the custom attribute.</param>
		/// <param name="encode">A Boolean value indicating whether to encode the custom attribute to register.</param>
		public static void RegisterExpandoAttribute(Control control, string controlId, string attributeName, string attributeValue, bool encode)
		{
			Type scriptManagerType = Type.GetType("System.Web.UI.ScriptManager, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35", false);
			Debug.Assert(scriptManagerType != null);

			scriptManagerType.InvokeMember(
				"RegisterExpandoAttribute",
				BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod,
				null, /*binder*/
				null, /*target*/
				new object[] { control, controlId, attributeName, attributeValue, encode }
			);
		}
		/// <summary>
		///		Registers an OnSubmit statement with the <see cref='System.Web.UI.Page'/> object using a type, a key, and a script literal. The statement executes when the <see cref='System.Web.UI.HtmlControls.HtmlForm'/> is submitted.
		/// </summary>
		/// <param name="control">The <see cref='System.Web.UI.Control'/> with the containing invocable member.</param>
		/// <param name="type">The type of the OnSubmit statement to register.</param>
		/// <param name="key">The key of the OnSubmit statement to register.</param>
		/// <param name="script">The script literal of the OnSubmit statement to register.</param>
		public static void RegisterOnSubmitStatement(Control control, Type type, string key, string script)
		{
			Type scriptManagerType = Type.GetType("System.Web.UI.ScriptManager, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35", false);
			Debug.Assert(scriptManagerType != null);

			scriptManagerType.InvokeMember(
				"RegisterOnSubmitStatement",
				BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod,
				null, /*binder*/
				null, /*target*/
				new object[] { control, type, key, script }
			);
		}
		/// <summary>
		///		Registers the startup script with the <see cref='System.Web.UI.Page'/> object using a type, a key, a script literal, and a Boolean value indicating whether to add script tags.
		/// </summary>
		/// <param name="control">The <see cref='System.Web.UI.Control'/> with the containing invocable member.</param>
		/// <param name="type">The type of the startup script to register.</param>
		/// <param name="key">The key of the startup script to register.</param>
		/// <param name="script">The startup script literal to register.</param>
		/// <param name="addScriptTags">A Boolean value indicating whether to add script tags.</param>
		public static void RegisterStartupScript(Control control, Type type, string key, string script, bool addScriptTags)
		{
			Type scriptManagerType = Type.GetType("System.Web.UI.ScriptManager, System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35", false);
			Debug.Assert(scriptManagerType != null);

			scriptManagerType.InvokeMember(
				"RegisterStartupScript",
				BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod,
				null, /*binder*/
				null, /*target*/
				new object[] { control, type, key, script, addScriptTags }
			);
		}

		#endregion Public Methods
	}
}
