//---------------------------------------------------------------------------------
// Dado Validators, Copyright 2014 roydukkey.
// Dual licensed under the MIT (http://www.roydukkey.com/mit) and
// GPL Version 2 (http://www.roydukkey.com/gpl) licenses.
//---------------------------------------------------------------------------------

function DadoValidation_Initialize(webForm) {
	var noop = function () { };

	//-- @Mono
	// TODO: Add events OnChange, OnBlur, OnClick
	//--

	//-- @Mono
	// Removes Mono's force style attributes
	//--
	for (var i = 0, validator; i < webForm.Page_Validators.length; i++) {
		validator = webForm.Page_Validators[i];

		validator.style.display =
		validator.style.visibility = null;
	}

	//-- @Microsoft
	// Adds Mono's methods for updating a Validators' states.
	//--
	if (!webForm.ValidatorFailed) {
		webForm.ValidatorFailed = noop;
	}
	if (!webForm.ValidatorSucceeded) {
		webForm.ValidatorSucceeded = noop;
	}

	//-- @Mono
	// Adds missing function to force validation
	//--
	if (!webForm.ValidatorValidate) {
		webForm.ValidatorValidate = function (validator, validationGroup, event) {
			validator.isvalid = true;

			if (
				(
					typeof (validator.enabled) == "undefined" ||
					validator.enabled != false
				) &&
				webForm.IsValidationGroupMatch(validator, validationGroup) &&
				typeof (validator.evaluationfunction) == "function"
			) {
				validator.isvalid = validator.evaluationfunction(validator);

				if (
					!validator.isvalid &&
					//webForm.Page_InvalidControlToBeFocused == null && // I cannot find where this might exist in Mono. Am I wrong?
					typeof (validator.focusOnError) == "string" &&
					validator.focusOnError == "t"
				)
					webForm.ValidatorSetFocus(validator);
			}
			else
				webForm.ValidatorSucceeded(validator);
		}
	}

	//-- @Mono, @Microsoft
	// Overides function to rely on classnames
	//--
	webForm.ValidatorUpdateDisplay = function (validator, valid) {
		var display = validator.display;

		// for validators that aren't displayed, do nothing
		if (display == "None")
			return;

		validator.className = validator.className.replace(validator.invalidClassName, "").replace(/^\s+|\s+$/g, "");

		if (!(typeof (valid) != "undefined" ? valid : validator.isvalid))
			validator.className += (validator.className ? " " : "") + validator.invalidClassName;
	}

	//-- @Mono, @Microsoft
	// Overides function to add expression flags
	//--
	webForm.RegularExpressionValidatorEvaluateIsValid = function (validator) {
		var value = webForm.ValidatorGetValue(validator.controltovalidate)
			, rx
			, matches;

		if (webForm.ValidatorTrim(value).length == 0) {
			webForm.ValidatorSucceeded(validator);
			return true;
		}

		rx = new RegExp(validator.validationexpression, validator.expressionoptions);
		matches = rx.exec(value);

		if (matches != null && value == matches[0]) {
			webForm.ValidatorSucceeded(validator);
			return true;
		}
		else {
			webForm.ValidatorFailed(validator);
			return false;
		}
	}

	//-- @Mono, @Microsoft
	// Overides function to add support for checkbox and checkboxlist
	//--
	webForm.RequiredFieldValidatorEvaluateIsValid = function (validator) {
		var isValid = false;

		// Validate for Checkbox
		if (validator.validatefor === "checkbox") {
			isValid = document.getElementById(validator.controltovalidate).checked
		}
		// Validate for CheckboxList
		else if (validator.validatefor === "checkboxlist") {
			var list = document.getElementById(validator.controltovalidate)
				, inputs = list.getElementsByTagName("input")
				, i = 0;

			for (; i < inputs.length;)
				if (inputs[i++].checked)
					isValid = true;
		}
		// Default provided validation
		else
			isValid = webForm.ValidatorTrim(webForm.ValidatorGetValue(validator.controltovalidate)) != webForm.ValidatorTrim(validator.initialvalue);

		if(isValid)
			webForm.ValidatorSucceeded(validator);
		else
			webForm.ValidatorFailed(validator);
			
		return isValid;
	}

	//-- @Dado
	// Adds new Type Validator
	//--
	webForm.TypeValidatorEvaluateIsValid = function (validator) {
		var value = webForm.ValidatorGetValue(validator.controltovalidate)
			, isValid = false;

		if (value == null) {
			isValid = true;
		}
		else {
			switch (validator.type) {
				case "Boolean":
					isValid = value.toLowerCase() == "true" || value.toLowerCase() == "false";
					break;

				case "Byte":
					isValid = +value >= 0 && +value <= 255;
					break;

				case "Char":
					isValid = value.length == 1;
					break;

				case "Decimal":
					value = value.replace(",", "");
					isValid = +value >= -79228162514264337593543950335 && +value <= 79228162514264337593543950335;
					break;

				case "Double":
					value = value.replace(",", "");
					isValid = +value >= -1.7976931348623157E+308 && +value <= 1.7976931348623157E+308;
					break;

				case "Int16":
					isValid = +value >= -32768 && +value <= 32767;
					break;

				case "Int64":
					isValid = +value >= -9223372036854775808 && +value <= 9223372036854775807;
					break;

				case "SByte":
					isValid = +value >= -128 && +value <= 127;
					break;

				case "Single":
					isValid = +value >= -3.402823e38 && +value <= 3.402823e38;
					break;

				//case "Int32":
				default:
					isValid = +value >= -2147483648 && +value <= 2147483647;
			}
		}

		if (isValid)
			webForm.ValidatorSucceeded(validator);
		else
			webForm.ValidatorFailed(validator);

		return isValid;
	}

	//-- @Dado
	// Adds new Length Validator
	//--
	webForm.LengthValidatorEvaluateIsValid = function (validator) {
		var min = ~~validator.minimumlength
			, max = ~~validator.maximumlength
			, value = webForm.ValidatorGetValue(validator.controltovalidate);

		// Do not validate empty control. Use a required field validator.
		if (value != "" && (max <= 0 && min > 0 || min <= max)) {
			// See if value is greater than MinimumLength
			if (min > 0 && value.length < min) {
				validator.innerHTML = "<span>" + validator.minimumerrormessage + "</span>";
				webForm.ValidatorFailed(validator);
				return false;
			}
			// See if value is greater than MaximumLength
			if (max > 0 && value.length > max) {
				validator.innerHTML = "<span>" + validator.maximumerrormessage + "</span>";
				webForm.ValidatorFailed(validator);
				return false;
			}
		}
		webForm.ValidatorSucceeded(validator);
		return true;
	}

}