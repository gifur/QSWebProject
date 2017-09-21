var FileUpload = function () {


    return {
        //main function to initiate the module
        init: function () {

             // Initialize the jQuery File Upload widget:
            $('#fileupload').fileupload({
                disableImageResize: false,
                autoUpload: false,
                // Uncomment the following to send cross-domain cookies:
                //xhrFields: {withCredentials: true},                
                url: '/Admin/Photo/Receive'
            });

            // Enable iframe cross-domain access via redirect option:
            $('#fileupload').fileupload(
                'option',
                'redirect',
                window.location.href.replace(
                    /\/[^\/]*$/,
                    '/cors/result.html?%s'
                )
            );

            // Demo settings:
            $('#fileupload').fileupload('option', {
                url: $('#fileupload').fileupload('option', 'url'),
                // Enable image resizing, except for Android and Opera,
                // which actually support image resizing, but fail to
                // send Blob objects via XHR requests:
                disableImageResize: /Android(?!.*Chrome)|Opera/.test(window.navigator.userAgent),
                // 限制文件大小，5M就是这样来的
                maxFileSize: 5000000,
                acceptFileTypes: /(\.|\/)(gif|jpe?g|png)$/i
            });

                // Upload server status check for browsers with CORS support:
            if ($.support.cors) {
                $.ajax({
                    url: '/Admin/Photo/Receive',
                    type: 'HEAD'
                }).fail(function () {
                    $('<div class="alert alert-danger"/>')
                        .text('目前上传所需的服务端尚未可使用 - ' +
                                new Date())
                        .appendTo('#fileupload');
                });
            }

            // Load & display existing files:
            $('#fileupload').addClass('fileupload-processing');
            $.ajax({
                // Uncomment the following to send cross-domain cookies:
                //xhrFields: {withCredentials: true},
                url: $('#fileupload').fileupload('option', 'url'),
                dataType: 'json',
                context: $('#fileupload')[0]
            }).always(function () {
                $(this).removeClass('fileupload-processing');
            }).done(function (result) {
                $(this).fileupload('option', 'done')
                .call(this, $.Event('done'), {result: result});
            });
        }

    };

}();