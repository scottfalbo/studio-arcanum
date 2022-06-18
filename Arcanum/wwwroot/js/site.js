'use strict';

// Shows popup windows.
$(function() {
    $('.show-popup').click(function() {
        $('.admin-outer').removeClass('hide-me');
        $('.fullscreen-fade').removeClass('hide-me');
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

// Update image info popup open.
$(function() {
    $('.update-image-info-button').click(function() {
        $(this).siblings('.update-image-info-container')
            .removeClass('hide-me');
    });
});

// Update image info popup close.
$(function() {
    $('.update-image-info-cancel').click(function() {
        $('.update-image-info-container').addClass('hide-me');
    });
});

// Loading bar display
$(function() {
    $('.loader').click(function() {
        $('.loading-bar-container').removeClass('hide-me');
    });
});

// Main page optional pop up window
$(function() {
    $('.close-main-page-popup').click(function() {
        $('.main-page-popup').addClass('hide-me');
        $('.fullscreen-fade').addClass('hide-me');
    });   
})

// Gallery Close Button
$(function() {
    $('.close-gallery').click(function() {
        $('.fullscreen-fade').addClass('hide-me');
    });
});
