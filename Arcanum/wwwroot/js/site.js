'use strict';

// Shows popup windows.
$(function() {
    $('.show-popup').click(function() {
        $('.admin-outer').removeClass('hide-me');
    });
});

// Close outer admin popup and fullscreen fade.
$(function() {
    $('.close-outer-admin').click(function() {
        $('.admin-outer').addClass('hide-me');
        $('.fullscreen-fade').addClass('hide-me');
    });
});

// Delete confirmation pop-up open.
$(function() {
    $('.delete-confirmation-button').click(function () {
        $(this).siblings('.delete-confirmation-container')
            .removeClass('hide-me');
    });
});

// Delete confirmation popup close.
$(function() {
    $('.delete-confirmation-cancel').click(function() {
        $('.delete-confirmation-container').addClass('hide-me');
    });
});
