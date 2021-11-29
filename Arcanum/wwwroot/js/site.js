'use strict';

// Shows popup windows.
$(function() {
    $('.show-popup').click(function() {
        $('.admin-outer').removeClass('hide-me');
    });
});


// Closes popup windows.
$(function() {
    $('.close-popup').click(function() {
        $('.fullscreen').addClass('hide-me');
    });
});