//---------------------------------------------------------------------------------
// Dado Validators, Copyright 2013 roydukkey, 2013-01-23 (Wed, 23 January 2013).
// Dual licensed under the MIT (http://www.roydukkey.com/mit) and
// GPL Version 2 (http://www.roydukkey.com/gpl) licenses.
//---------------------------------------------------------------------------------

function ValidatorGetValue(id) {
  var control = document.getElementById(id);
  if (typeof(control.value) == "string") {
		if ("type" in control && control.type == "file") return control.value.replace(/^.*[\\\/]/, '')
		return control.value;
	}
  return ValidatorGetValueRecursive(control);
}

function ValidatorUpdateDisplay(val) {
	if (val.display == "None") return;

	val.style.visibility = val.style.display = "";
	val.className = val.className.replace(val.invalidClassName, "").replace(/^\s+|\s+$/g, "");

	if (
		!val.isvalid || ( navigator.userAgent.indexOf("Mac") > -1 && navigator.userAgent.indexOf("MSIE") > -1 )
	)
		val.className += (val.className ? " " : "") + val.invalidClassName;
}

function LengthValidatorEvaluateIsValid(val) { var min = ~~val.minimumlength, max = ~~val.maximumlength;
	// Do not validate empty control. Use a required field validator.
	if (ValidatorGetValue(val.controltovalidate) != "" && (max <= 0 && min > 0 || min <= max)) {
		// See if value is greater than MinimumLength
		if (min > 0 && ValidatorGetValue(val.controltovalidate).length < min) {
			val.innerHTML = "<span>" + val.minimumerrormessage + "</span>";
			return false;
		}
		// See if value is greater than MaximumLength
		if (max > 0 && ValidatorGetValue(val.controltovalidate).length > max) {
			val.innerHTML = "<span>" + val.maximumerrormessage + "</span>";
			return false;
		}
	}
	return true;
}

function TypeValidatorEvaluateIsValid(val) { var v = ValidatorGetValue(val.controltovalidate);
	if(v == null) return true;
	switch (val.type) {
    case "Boolean":
			return (v.toLowerCase() == "true" || v.toLowerCase() == "false");

		case "Byte":
			return +v >= 0 && +v <= 255;

		case "Char":
			return v.length == 1;

		case "Decimal":
			v = v.replace(",", "");
			return +v >= -79228162514264337593543950335 && +v <= 79228162514264337593543950335;

		case "Double":
			v = v.replace(",", "");
			return +v >= -1.7976931348623157E+308 && +v <= 1.7976931348623157E+308;

		case "Int16":
			return +v >= -32768 && +v <= 32767;

		case "Int64":
			return +v >= -9223372036854775808 && +v <= 9223372036854775807;

		case "SByte":
			return +v >= -128 && +v <= 127;

		case "Single":
			return +v >= -3.402823e38 && +v <= 3.402823e38;

		//case "Int32":
		default:
			return +v >= -2147483648 && +v <= 2147483647;
	}
}

function ValidateOnLoad() {
	Page_InvalidControlToBeFocused = null;
	if (typeof(OnLoad_Validators) == "undefined") return true;
	for (var i = 0; i < OnLoad_Validators.length; i++) ValidatorValidate(OnLoad_Validators[i]);
}