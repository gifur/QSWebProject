//初始化名为 remoteVal 的客户端验证规则，并在 validators 集合中注册该规则
Sys.Mvc.ValidatorRegistry.validators.remoteVal = function (rule) {
    var url = rule.ValidationParameters.url;
    var parameterName = rule.ValidationParameters.parameterName;

    return function (value, context) {  // anonymous function

        if (!value || !value.length) {
            return true;
        }

        if (context.eventName != 'blur') {
            return true;
        }

        var newUrl = ((url.indexOf('?') < 0) ? (url + '?') : (url + '&'))
            + encodeURIComponent(parameterName) + '=' + encodeURIComponent(value);
        var completedCallback = function (executor) {
            if (executor.get_statusCode() != 200) {
                return; // there was an error
            }

            var responseData = executor.get_responseData();
            if (responseData != 'OK') {
                // add error to validation message
                var newMessage = (responseData == 'FAIL' ?
                    rule.ErrorMessage : responseData);
                context.fieldContext.addError(newMessage);
            }
        };

        var r = new Sys.Net.WebRequest();
        r.set_url(newUrl);
        r.set_httpVerb('GET');
        r.add_completed(completedCallback);
        r.invoke();
        return true; // optimistically assume success
    };
};