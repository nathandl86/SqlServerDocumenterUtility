
/*
 * SectionLoader
 * @namespace global
 * @description Handler for dynamically showing additional content
 *      on the application page that isn't initally rendered when 
 *      the page laods.
 */
var SectionLoader = (function (me) {
    $('.page-scroll a').on('click', onMenuItemClick);

    var contentContainers = [];

    //hanlder for the click of a nav menu link
    function onMenuItemClick(ev) {
        var anchor = $(this);
        loadPartial(anchor);
        animateScroll(anchor);
        ev.preventDefault();
    }

    //animates the downward scroll to the section
    function animateScroll(anchor) {
        $('html, body').stop().animate({
            scrollTop: $(anchor.attr('href')).offset().top - 30
        }, 700);
    }

    //loads a partial view by sending a get request to the MVC
    // controller based on the url data attribute of the clicked
    // menu link
    function loadPartial(anchor) {
        var section = $(anchor.attr('href'));

        //makes the call to get additional conent and appends it to the section
        // matching the href attribute of the menu link
        if (section.hasClass("lazy-load") && section.find('.section').length <= 0) {
            var url = anchor.data('url');
            $.get(url, function (data) {
                section.append(data);
                
                //evalutates the content containers if there is additional 
                // dynamic content that should be rendered in the new content.
                if (contentContainers.length > 0) {
                    for (var i = 0, len = contentContainers.length; i < len; i++ ){
                        var item = contentContainers[i];
                        if (section.attr('id').toLowerCase() === item.name.toLowerCase()) {
                            $(item.selector).append(item.data);
                        }
                    }
                }
            });
        }        
    }

    //gets the markdown's compiled html file and creates a content container item
    // so that when the about link is clicked, the markdown html can be placed into it.
    var markdownContainer = $('#markdownLocation');
    $.get(markdownContainer.data('url'), function (data) {
        contentContainers.push({ 
            name: "about", 
            selector: "#markdownDestination",
            data: data
        });
    });
})(SectionLoader);