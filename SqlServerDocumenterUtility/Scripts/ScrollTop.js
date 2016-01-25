/*
 * ScollTop
 * @namespace global
 * @description Handler for scrolling for the app that will 
 *  hide and show a button enabling the user to quickly 
 *  be scrolled back to the top of the page.
 */
var ScrollTop = (function (me) {
    var offset = 60, // minimum scroll height for the button to show
        offsetOpacity = 250, // scroll height at which to dim the opacity of the button
        scrollDuration = 700;

    $(window).scroll(onScroll);
    $('.scroll-top').on('click', onScrollTopClicked);

    function onScroll(ev) {
        var scrollPos = $(window).scrollTop();
        var elem = $('.scroll-top');

        if (scrollPos < offset) {
            elem.removeClass('scroll-top-visible, scroll-top-faded').hide();
        }
        else if (scrollPos > offsetOpacity) {
            elem.removeClass('scroll-top-visible').addClass('scroll-top-faded').show();
        }
        else {
            elem.removeClass('scroll-top-faded').addClass('scroll-top-visbile').show();
        }
    }

    //click handler for the scroll top button
    function onScrollTopClicked(ev) {
        var anchor = $(this);
        animateScrollTop(anchor);
        ev.preventDefault();
    }

    //function handling the animated scrolling
    function animateScrollTop(anchor) {
        $('html, body').stop().animate({
            scrollTop: $(anchor.attr('href')).offset().top - 30
        }, scrollDuration);
    }

})(ScrollTop);