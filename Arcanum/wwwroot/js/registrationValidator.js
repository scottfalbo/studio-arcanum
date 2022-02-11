'use strict';

let emailIsValidFormat = false;
let passwordIsMatch = false;

// Validation to ensure that the user enters the same password
// in both fields during account creation.
$(function () {
    $("#password-input").keyup(validatePassword);
});
$(function () {
    $("#password-validator").keyup(validatePassword);
});

function validatePassword() {
    var password1 = $("#password-input").val();
    var password2 = $("#password-validator").val();
    if (password1 === password2) {
        $('#password-valid-status').text('O');
        passwordIsMatch = true;
    }
    else {
        $('#password-valid-status').text('X');
        passwordIsMatch = false;
    }
    validateAllInputs();
}

// Validates that the inputted email is a proper email format.
$(function () {
    $('#email-input').keyup(validateEmail);
});

function validateEmail() {
    let emailInput = $('#email-input').val();
    if (!checkFormat(emailInput)) {
        $('#email-validator').text('X');
        emailIsValidFormat = false;
    }
    else {
        $('#email-validator').text('O');
        emailIsValidFormat = true;
    }
    validateAllInputs();
}

function checkFormat(email) {
    let emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/
    return emailPattern.test(email);
}

// If both the email and password pass validation the register button is activated.
function validateAllInputs() {
    if (passwordIsMatch === true && emailIsValidFormat === true) {
        $('#create-account-submit').removeClass('fade-me');
    }
    else {
        $('#create-account-submit').addClass('fade-me');
    }
}