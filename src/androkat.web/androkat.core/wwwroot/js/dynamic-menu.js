(function ($, window, document) {
    'use strict';

    var pluginName = 'dynamicMenu',
        defaults = {
            activeClass: 'active',
            homePage: '/'
        };

    function DynamicMenu(element, options) {
        this.element = element;
        this.settings = $.extend({}, defaults, options);
        this.init();
    }

    $.extend(DynamicMenu.prototype, {
        init: function () {
            var currentPage = this.getCurrentPage();
            var hrefContext = this.isHomePage(currentPage)
                ? this.settings.homePage
                : currentPage;

            this.setActiveItem(hrefContext);
        },

        getCurrentPage: function () {
            var path = window.location.pathname;

            return "/" + path.substr(path.indexOf('/') + 1);
        },

        isHomePage: function (currentPage) {
            var homePage = ['', this.settings.homePage];

            return $.inArray(currentPage, homePage) !== -1;
        },

        setActiveItem: function (hrefContext) {

            $(this.element)
                .find('a[href="' + hrefContext + '"]')
                .addClass(this.settings.activeClass);

            $(this.element).find('a[href="' + hrefContext + '"]').attr("aria-current", "page");
        }
    });

    $.fn[pluginName] = function (options) {
        return this.each(function () {
            if (!$.data(this, 'plugin_' + pluginName)) {
                $.data(this, 'plugin_' + pluginName, new DynamicMenu(this, options));
            }
        });
    };
})(jQuery, window, document);