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

// Delete user confirmation pop-up.
$(function() {
    $('.delete-user-button').click(function() {
       $('.delete-user-container').removeClass('hide-me'); 
    });
});
