﻿//---------------------------------------------------------------------------------
// Dado Validators v1.0.1, Copyright 2015 roydukkey, 2015-01-07 (Wed, 07 Jan 2015).
// Released under the GPL Version 3 license (https://github.com/roydukkey/Dado.Validators/raw/master/LICENSE).
//---------------------------------------------------------------------------------

namespace Dado
{
	using System;
	using System.Text.RegularExpressions;

	internal static class StringExtensions
	{
		#region Public Methods

		/// <summary>
		///		Prepend new a string with a delimiter when the current string isn't Null or Empty.
		/// </summary>
		/// <param name="s"></param>
		/// <param name="delimiter"></param>
		/// <param name="text"></param>
		public static string Prepend(this String s, string delimiter, params string[] text)
		{
			return PrependProcessor(delimiter, s, true, text);
		}
		/// <summary>
		///		Prepend new a string with a delimiter when the current string isn't Null or Empty.
		/// </summary>
		/// <param name="s"></param>
		/// <param name="delimiter"></param>
		/// <param name="text"></param>
		/// <param name="ignoreNullOrEmpty"></param>
		public static string Prepend(this String s, string delimiter, bool ignoreNullOrEmpty, params string[] text)
		{
			return PrependProcessor(delimiter, s, ignoreNullOrEmpty, text);
		}
		/// <summary>
		///		Append new a string with a delimiter when the current string isn't Null or Empty.
		/// </summary>
		/// <param name="s"></param>
		/// <param name="delimiter"></param>
		/// <param name="text"></param>
		public static string Append(this String s, string delimiter, params string[] text)
		{
			return AppendProcessor(delimiter, s, true, text);
		}
		/// <summary>
		///		Append new a string with a delimiter when the current string isn't Null or Empty.
		/// </summary>
		/// <param name="s"></param>
		/// <param name="delimiter"></param>
		/// <param name="text"></param>
		/// <param name="ignoreNullOrEmpty"></param>
		public static string Append(this String s, string delimiter, bool ignoreNullOrEmpty, params string[] text)
		{
			return AppendProcessor(delimiter, s, ignoreNullOrEmpty, text);
		}

		#endregion Public Methods

		#region Internal Methods

		/// <summary>
		///		Prepend new a string with a delimiter when the current string isn't Null or Empty.
		/// </summary>
		/// <param name="delimiter"></param>
		/// <param name="currentString"></param>
		/// <param name="ignoreNullOrEmpty"></param>
		/// <param name="newStrings"></param>
		/// <returns></returns>
		internal static string PrependProcessor(string delimiter, string currentString, bool ignoreNullOrEmpty, params string[] newStrings)
		{
			if (newStrings != null)
				foreach (string newString in newStrings)
					currentString = (
						ignoreNullOrEmpty && String.IsNullOrEmpty(newString) ? null :
						newString + (!String.IsNullOrEmpty(currentString) ? delimiter : null)
					) + currentString;
			return currentString;
		}
		/// <summary>
		///		Append new a string with a delimiter when the current string isn't Null or Empty.
		/// </summary>
		/// <param name="delimiter"></param>
		/// <param name="currentString"></param>
		/// <param name="ignoreNullOrEmpty"></param>
		/// <param name="newStrings"></param>
		/// <returns></returns>
		internal static string AppendProcessor(string delimiter, string currentString, bool ignoreNullOrEmpty, params string[] newStrings)
		{
			if (newStrings != null)
				foreach (string newString in newStrings)
					currentString = currentString + (
						ignoreNullOrEmpty && String.IsNullOrEmpty(newString) ? null :
						(!String.IsNullOrEmpty(currentString) ? delimiter : null) + newString
					);
			return currentString;
		}

		#endregion Internal Methods
	}
}