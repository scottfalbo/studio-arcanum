'use strict';

// Show the artist admin window
$(function() {
    $('.show-artist-admin').click(function() {
        $('.artist-admin').removeClass('hide-me');
    });
});


// Closes popup windows
$(function() {
    $('.close-popup').click(function() {
        $('.fullscreen').addClass('hide-me');
    });
});