//---------------------------------------------------------------------------------
// Dado Validators, Copyright 2012 roydukkey, 2012-05-24 (Tue, 24 July 2012).
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
			val.innerHTML = val.minimumerrormessage;
			return false;
		}
		// See if value is greater than MaximumLength
		if (max > 0 && ValidatorGetValue(val.controltovalidate).length > max) {
			val.innerHTML = val.maximumerrormessage;
			return false;
		}
	}
	return true;
}

function ValidateOnLoad() {
	Page_InvalidControlToBeFocused = null;
	if (typeof(OnLoad_Validators) == "undefined") return true;
	for (var i = 0; i < OnLoad_Validators.length; i++) ValidatorValidate(OnLoad_Validators[i]);
}