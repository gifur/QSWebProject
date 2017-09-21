var FormWizard = function () {


    return {
        //main function to initiate the module
        init: function () {
            if (!jQuery().bootstrapWizard) {
                return;
            }

            var form = $('#submit_form');
            var error = $('.alert-danger', form);
            var success = $('.alert-success', form);

            var handleTitle = function(tab, navigation, index) {
                var total = navigation.find('li').length;
                var current = index + 1;
                // set wizard title
                $('.step-title', $('#form-create')).text('步骤 ' + (index + 1) + ' / ' + total);
                // set done steps
                jQuery('li', $('#form-create')).removeClass("done");
                var li_list = navigation.find('li');
                for (var i = 0; i < index; i++) {
                    jQuery(li_list[i]).addClass("done");
                }

                if (current >= total) {
                    $('#form-create').find('.button-delete').show();
                    $('#form-create').find('.button-next').hide();
                    $('#form-create').find('.button-view').show();
                } else {
                    $('#form-create').find('.button-next').show();
                }
                App.scrollTo($('.page-title'));
            }
            // default form wizard
            $('#form-create').bootstrapWizard({
                onTabClick: function() { return false; },
                onNext: function (tab, navigation, index) {
                    success.hide();
                    error.hide();
                    handleTitle(tab, navigation, index);
                },
                onPrevious: function (tab, navigation, index) {
                    success.hide();
                    error.hide();

                    handleTitle(tab, navigation, index);
                },
                onTabShow: function (tab, navigation, index) {
                    var total = navigation.find('li').length;
                    var current = index + 1;
                    var $percent = (current / total) * 100;
                    $('#form-create').find('.progress-bar').css({
                        width: $percent + '%'
                    });
                }
            });

            $('#form-create').find('.button-delete').hide();
            $('#form-create .button-view').hide();
        }

    };

}();