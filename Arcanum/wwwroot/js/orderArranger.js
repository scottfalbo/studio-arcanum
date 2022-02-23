'use strict';

$(function() {
    $('#sortable-portfolio-images').sortable();
    $('#sortable-portfolio-images').disableSelection();
    $('#update-portfolio-image-order').click(savePortfolioImageOrder);
});

function savePortfolioImageOrder() {
    let imageOrder = new Array();
    $('#sortable-portfolio-images li').each(function(i) {
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