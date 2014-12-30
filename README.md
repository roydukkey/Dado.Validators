Dado.Validators (version 1.0.0)
=================================

Dado Validators is an extension of the System.Web.UI.WebControls.BaseValidator class provided by .NET 4.
Additional, Mono is supported since v1.0.0 of Dado.Validators.

**Download** [Solution](//github.com/roydukkey/Dado.Validators/zipball/master) | [Assembly](//github.com/roydukkey/Dado.Validators/blob/master/Release/DadoValidatorsAssembly-v1.0.0.zip?raw=true)


## Features
* __Styling by Class Name:__ Dado Validators doesn't rely on non-breaking spaces or inline styles, but rather entirely on the class name.
* __Invalid Class Name:__ Invalid class names are assigned to the validator only when the validation event returns false.
* __Default Error Messages:__ All validators have default error messages.


## Included Validators

![Two Face Singing Example](http://l33t.roydukkey.com/dadoValidatorsToolbox.png)

<table>
	<tr>
		<td valign="top">
			<strong>Compare:</strong><br />
			Compares the value entered by the user in an input control with the value entered in another input control, or with a constant value.
		</td>
		<td valign="top">
			<strong>Custom:</strong><br />
			Allows custom code to perform validation on the client and/or server.
		</td>
	</tr>
	<tr>
		<td valign="top">
			<strong>Email:</strong><br />
			Checks if the value of the associated input control is a valid email address.
		</td>
		<td valign="top">
			<strong>File Type:</strong><br />
			Checks if the value of the associated input control has an acceptable file extension by resctriction or allowance.
		</td>
	</tr>
	<tr>
		<td valign="top">
			<strong>Image:</strong><br />
			Checks if the value of the associated input control meets exact, minimum, and maximum height and width constraints.
		</td>
		<td valign="top">
			<strong>Length:</strong><br />
			Checks if the value of the associated input control meets minimum, and maximum text length constraints.
		</td>
	</tr>
	<tr>
		<td valign="top">
			<strong>Phone:</strong><br />
			Checks if the value of the associated input control is a valid phone number. Allows speficication of extensions or whether to allow them at all.
		</td>
		<td valign="top">
			<strong>Range:</strong><br />
			Checks if the value of the associated input control is within some minimum and maximum values, which can be constant values or values of other controls.
		</td>
	</tr>
	<tr>
		<td valign="top">
			<strong>Regular Expression:</strong><br />
			Checks if the value of the associated input control matches the pattern of a regular expression.
		</td>
		<td valign="top">
			<strong>Require Field:</strong><br />
			Makes the associated input control a required field. Added support for CheckBox and CheckBoxLists.
		</td>
	</tr>
	<tr>
		<td valign="top">
			<strong>Time:</strong><br />
			Checks if the value of the associated input control is in a valid time format. ie: HH:mm, h:mm tt, h:mmtt, h:mm, hh:mm tt, hh:mmtt, hh:mm
		</td>
		<td valign="top">
			<strong>Type:</strong><br />
			Checks if the value of the associated input control is of a specified .NET type. ie: Boolean, Byte, Char, Decimal, Double, Int16, Int32, Int64, SByte, Single
		</td>
	</tr>
	<tr>
		<td valign="top">
			<strong>Zip Code:</strong><br />
			Checks if the value of the associated input control is a valid zip code.
		</td>
		<td valign="top">
			<strong>URI:</strong><br />
			Checks if the value of the associated input control is a valid a well formatted URI, either relative, absolute, or both.
		</td>
	</tr>
</table>