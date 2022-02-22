'use strict';

$(function() {
    $('#sortable').sortable();
    $('#sortable').disableSelection();
    $('#save-view-preferences').click(saveOrder);
});

function saveOrder() {
    console.log('hello');
    let itemsOrder = new Array();
    $('#sortable li').each(function(i) {
        let item = new Object();
        item.Id = $(this).find($('.item-id')).val();
        item.IsEnabled = $(this).find($('.enabled-flag'))
            .attr('checked') == 'checked' ? true : false;
        item.Description = $(this).find($('.item-description')).val();
        item.Order = i;
        itemsOrder.push(item);
    });
    let data = JSON.stringify(itemsOrder);
    $.ajax({
        type: 'POST',
        url: '/MovingImagesTest?handler=MoveItems',
        data: data,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
        success: function() {
            console.log('success');
        }
    });
}