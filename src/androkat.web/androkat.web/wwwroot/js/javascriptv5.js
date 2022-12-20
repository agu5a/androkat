(function ($) {

    $.fn.scrollPagination = function (options) {

        var settings = {
            nop: 10, // The number of posts per scroll to be loaded
            offset: 0, // Initial offset, begins at 0 in this case
            error: 'Nincs több!', // When the user reaches the end this is
            // the message that is
            // displayed. You can change this if you want.
            delay: 500, // When you scroll down the posts will load after a
            // delayed amount of time.
            // This is mainly for usability concerns. You can alter this as you
            // see fit
            scroll: true,
            // The main bit, if set to false posts will not load as the user
            // scrolls.
            // but will still load if the user clicks.
            url: ''
        };

        // Extend the options so they work with the plugin
        if (options) {
            $.extend(settings, options);
        }

        // For each so that we keep chainability.
        return this.each(function () {

            // Some variables
            $this = $(this);
            $settings = settings;
            var offset = $settings.offset;
            var busy = false; // Checks if the scroll action is happening
            // so we don't run it multiple times

            // Custom messages based on settings
            if ($settings.scroll == true)
                $initmessage = 'Görgessen a többihez';
            else
                $initmessage = 'Kattintson a többihez';

            // init messages
            $('.loading-bar').html($initmessage);

            function getParameterByName(name, url = window.location.href) {
                name = name.replace(/[\[\]]/g, '\\$&');
                var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'), results = regex.exec(url);
                if (!results) return null;
                if (!results[2]) return '';
                return decodeURIComponent(results[2].replace(/\+/g, ' '));
            }

            function getData() {
                var f = getParameterByName('f');

                $.post($settings.url, {   
                    offset: offset,
                    f: f == null ? '' : f
                }, function (data) {
                    // Change loading bar (it may have been altered)
                    $('.loading-bar').html($initmessage);

                    // If there is no data returned, there are no more posts to be shown. Show error
                    if (data == "") {
                        $('.loading-bar').html($settings.error);
                    } else {
                        // Offset increases
                        offset = offset + $settings.nop;
                        // Append the data to the div
                        $this.append(data); 
                        // No longer busy!
                        busy = false;
                    }
                });
            }

            getData(); // Run function initially

            // If scrolling is enabled
            if ($settings.scroll == true) {
                // .. and the user is scrolling
                $(window)
                    .scroll(
                        function () {

                            // Check the user is at the bottom of the
                            // element
                            if ($(window).scrollTop()
                                + $(window).height() > $this
                                    .height()
                                && !busy) {

                                // Now we are working, so busy is true
                                busy = true;

                                // Tell the user we're loading posts
                                $('.loading-bar').html('Keresés...');

                                // Run the function to fetch the data
                                // inside a delay
                                // This is useful if you have content in
                                // a footer you
                                // want the user to see.
                                setTimeout(function () {

                                    getData();

                                }, $settings.delay);

                            }
                        });
            }

            // Also bar can be loaded by clicking the loading bar/
            $('.loading-bar').click(function () {

                if (busy == false) {
                    busy = true;
                    getData();
                }

            });

        });
    };

})(jQuery);
