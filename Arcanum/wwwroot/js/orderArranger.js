'use strict';

$(function() {
    $('#sortable').sortable();
    $('#sortable').disableSelection();
    $('#save-view-preferences').click(saveOrder);
});

function saveOrder() {
    let imageOrder = new Array();
    $('#sortable li').each(function(i) {
        let image = new Object();
        image.Id = $(this).find($('.image-id')).val();
        image.Order = $(this).find($('.image-order')).val();
        image.Order = i;
        imageOrder.push(image);
    });
    let data = JSON.stringify(imageOrder);
    $.ajax({
        type: 'POST',
        url: '/Artist?handler=UpdateImageOrder',
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