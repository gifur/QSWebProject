var Login = function () {

	var handleLogin = function() {
		$('.login-form').validate({
	            errorElement: 'span', //default input error message container
	            errorClass: 'help-block', // default input error message class
	            focusInvalid: false, // do not focus the last invalid input
	            rules: {
	                NameOrNumber: {
	                    required: true
	                },
	                Password: {
	                    required: true
	                },
	                ValidateCode: {
	                    required: true
	                }
	            },

	            messages: {
	                NameOrNumber: {
	                    required: "用户名是必须的"
	                },
	                Password: {
	                    required: "请输入密码"
	                },
	                ValidateCode: {
	                    required: "请输入验证码"
	                }
	            },

	            invalidHandler: function (event, validator) { //display error alert on form submit   
	                $('.alert-danger', $('.login-form')).show();
	            },

	            highlight: function (element) { // hightlight error inputs
	                $(element)
	                    .closest('.form-group').addClass('has-error'); // set error class to the control group
	            },

	            success: function (label) {
	                label.closest('.form-group').removeClass('has-error');
	                label.remove();
	            },

	            errorPlacement: function (error, element) {
	                error.insertAfter(element.closest('.input-icon'));
	            },

	            submitHandler: function (form) {
	                form.submit();
	            }
	        });

	        $('.login-form input').keypress(function (e) {
	            if (e.which == 13) {
	                if ($('.login-form').validate().form()) {
	                    $('.login-form').submit();
	                }
	                return false;
	            }
	        });
	}

    
    return {
        //main function to initiate the module
        init: function () {
        	
            handleLogin();
        }

    };

}();