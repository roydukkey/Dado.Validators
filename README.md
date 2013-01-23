Dado.Validators (version 0.3.1.0)
=================================

Dado Validators is an extension of the System.Web.UI.WebControls.BaseValidator class provided by .NET 4.

**Download** [Solution](//github.com/roydukkey/Dado.Validators/zipball/master) | [Assembly](//github.com/roydukkey/Dado.Validators/blob/master/Release/DadoValidatorsAssembly-v0.3.1.0.zip?raw=true)

If you have questions about Dado Validators, email me at `dado@roydukkey.com`.

## Features
* __Styling by Class Name:__ Dado Validators doesn't rely of non-breaking spaces or inline styles, but rather entirely on the class name.
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
			Checks if the value of the associated input control has an acceptable file extension.
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
			Checks if the value of the associated input control is a valid phone number.
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
			Makes the associated input control a required field.
		</td>
	</tr>
	<tr>
		<td valign="top">
			<strong>Time:</strong><br />
			Checks if the value of the associated input control is in a valid time format. ie: HH:mm, h:mm tt, h:mmtt, h:mm, hh:mm tt, hh:mmtt, hh:mm
		</td>
		<td valign="top">
			<strong>Type:</strong><br />
			Checks if the value of the associated input control is of a specified .NET type. ie: Boolean, Byte, Char, DateTime, Decimal, Double, Int16, Int32, Int64, SByte, Single
		</td>
	</tr>
	<tr>
		<td valign="top">
			<strong>Zip Code:</strong><br />
			Checks if the value of the associated input control is a valid zip code.
		</td>
		<td></td>
	</tr>
</table>


## License

Dual licensed under the MIT (http://www.roydukkey.com/mit) and GPL (http://www.roydukkey.com/gpl) licenses.