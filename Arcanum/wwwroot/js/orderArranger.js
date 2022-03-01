'use strict';

// Define sortable lists for re-order.
$(function() {
    $('#sortable-portfolio-image').sortable();
    $('#sortable-portfolio-image').disableSelection();
    $('#sortable-portfolio').sortable();
    $('#sortable-portfolio').disableSelection();
    $('#sortable-artist').sortable();
    $('#sortable-artist').disableSelection();
    $('#sortable-studio-image').sortable();
    $('#sortable-studio-image').disableSelection();
});

// Re-order images update button.
// Gets target set and calls function to save updates.
$(function() {
    $('.update-order').click(function() {
        var target = $(this).data('button');
        console.log(target);
        saveOrder(target);
    });
});

// Calls a code back method to save updated order to the database.
function saveOrder(target) {
    let itemOrder = new Array();
    $(`#sortable-${target} > li`).each(function(i) {
        let item = new Object();
        if (target === 'artist')
            item.ArtistId = $(this).find($(`.${target}-id`)).val();
        else
            item.Id = $(this).find($(`.${target}-id`)).val();
        item.Order = $(this).find($(`.${target}-order`)).val();
        item.Order = i;
        itemOrder.push(item);
    });

    let data = JSON.stringify(itemOrder);
    let handler = getHandler(target);
    let route = getRoute(target);

    $.ajax({
        type: 'POST',
        url: `/${route}?handler=${handler}`,
        data: data,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
        success: function() {
            console.log('success');
            $('.loading-bar-container').addClass('hide-me');
        }
    });
}

// Helper function to get the appropriate handler.
function getHandler(target) {
    switch(target) {
        case 'portfolio-image':
            return 'UpdatePortfolioImageOrder';
        case 'studio-image':
            return 'UpdateStudioImageOrder';
        case 'portfolio':
            return 'UpdatePortfolioOrder'
        case 'artist':
            return 'UpdateArtistOrder';
        default:
            break;
      }
}

// Helper function to get the appropriate route.
function getRoute(target) {
    switch(target) {
        case 'portfolio-image':
            return 'Artist';
        case 'studio-image':
            return 'TheStudio';
        case 'portfolio':
            return 'Artist'
        case 'artist':
            return 'Admin/SecretLair';
        default:
            break;
      }
}
