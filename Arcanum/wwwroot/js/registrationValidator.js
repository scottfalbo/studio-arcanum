'use strict';

let emailIsValidFormat = false;
let passwordIsMatch = false;
let userNameIsValid = false;

// Validation to ensure that the user enters the same password
// in both fields during account creation.
$(function () {
    $('.password-input').keyup(validatePassword);
});
$(function () {
    $('.password-validator').keyup(validatePassword);
});

function validatePassword() {
    let password1 = $('.password-input').val();
    let password2 = $('.password-validator').val();
    if (password1 === password2 && password1.length > 3) {
        $('.password-valid-status').addClass('valid-form-input');
        passwordIsMatch = true;
    }
    else {
        $('.password-valid-status').removeClass('valid-form-input');
        passwordIsMatch = false;
    }
    validateAllInputs();
}

$(function () {
    $('.username-input').keyup(validateUserName);
});

function validateUserName() {
    let userNameInput = $('.username-input').val();
    if (userNameInput.length < 3) {
        $('.username-input').removeClass('valid-form-input');
        userNameIsValid = false;
    }
    else {
        $('.username-input').addClass('valid-form-input');
        userNameIsValid = true;
    }
}

// Validates that the inputted email is a proper email format.
$(function () {
    $('.email-input').keyup(validateEmail);
});

function validateEmail() {
    let emailInput = $('.email-input').val();
    if (!checkFormat(emailInput)) {
        $('.email-input').removeClass('valid-form-input');
        emailIsValidFormat = false;
    }
    else {
        $('.email-input').addClass('valid-form-input');
        emailIsValidFormat = true;
    }
    validateAllInputs();
}
// Regex to check format.
function checkFormat(email) {
    let emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/
    return emailPattern.test(email);
}

// If both the email and password pass validation the register button is activated.
function validateAllInputs() {
    skipEmailValidation();
    if (passwordIsMatch === true && emailIsValidFormat === true && userNameIsValid) {
        $('.conditional-submit').removeClass('fade-me');
    }
    else {
        $('.conditional-submit').addClass('fade-me');
    }
}

// Bypasses the email check if no email is required.
function skipEmailValidation() {
    let checker = $('.bypass-email-validation').val();
    if (checker === 'true') {
        emailIsValidFormat = true;
    }
}

// Event listeners to toggle password input visibility. 
// New password entry.
const peekPassword = document.querySelector('.peek-password-input');
const password = document.querySelector('.password-input');
if (peekPassword != null) {
    peekPassword.addEventListener('click', function () {
        const type = password.getAttribute('type') === 'password' ? 'text' : 'password';
        password.setAttribute('type', type);
    });
}
// Repeat new password entry.
const peekPasswordValidation = document.querySelector('.peek-password-validator');
const passwordValidation = document.querySelector('.password-validator');
if (peekPasswordValidation != null) {
    peekPasswordValidation.addEventListener('click', function () {
        const type = passwordValidation.getAttribute('type') === 'password' ? 'text' : 'password';
        passwordValidation.setAttribute('type', type);
    });
}
// Current password entry.
const peekPasswordCurrent = document.querySelector('.peek-password-current');
const passwordCurrent = document.querySelector('.password-current');
if (peekPasswordCurrent != null) {
    peekPasswordCurrent.addEventListener('click', function () {
        const type = passwordCurrent.getAttribute('type') === 'password' ? 'text' : 'password';
        passwordCurrent.setAttribute('type', type);
    });
}


