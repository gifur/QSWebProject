var ComingSoon = function () {

    return {
        //main function to initiate the module
        init: function () {

            //$.backstretch([
    		//        "assets/img/bg/1.jpg",
    		//        "assets/img/bg/2.jpg",
    		//        "assets/img/bg/3.jpg",
    		//        "assets/img/bg/4.jpg"
    		//        ], {
    		//          fade: 1000,
    		//          duration: 10000
    		//    });
            //var startValue = "2014-8-5"; //表示离开始过去了多少天
            var endValue = "2015-01-21";
            //var sinceDay = new Date(startValue);
            var austDay = new Date(endValue);
            //austDay = new Date(austDay.getFullYear() + 1, 1 - 1, 26);
            $('#defaultCountdown').countdown({/*since:sinceDay,*/ until: austDay});
            $('#year').text(austDay.getFullYear());
        }

    };

}();