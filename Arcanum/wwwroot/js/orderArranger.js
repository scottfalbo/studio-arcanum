'use strict';

// Re-order images
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

// Re order portfolios
$(function() {
    $('#sortable-artist-portfolios').sortable();
    $('#sortable-artist-portfolios').disableSelection();
    $('#update-portfolio-order').click(savePortfolioImageOrder);
});
function savePortfolioImageOrder() {
    let portfolioOrder = new Array();
    $('#sortable-artist-portfolios li').each(function(i) {
        let portfolio = new Object();
        portfolio.Id = $(this).find($('.portfolio-id')).val();
        portfolio.Order = $(this).find($('.portfolio-order')).val();
        portfolio.Order = i;
        portfolioOrder.push(portfolio);
    });
    let data = JSON.stringify(portfolioOrder);
    $.ajax({
        type: 'POST',
        url: '/Artist?handler=UpdatePortfolioOrder',
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