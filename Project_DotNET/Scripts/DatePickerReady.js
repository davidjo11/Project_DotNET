if (!Modernizr.inputtypes.date) {
    $(function () {
        $.datepicker.setDefaults($.datepicker.regional['fr']);
        $("input[type='date']")
                    .datepicker()
                    .get(0)
                    .setAttribute("type", "text");
        $(".datepicker").each(function () {
            var dpvalue = $(this).attr("value").split("-");
            var myDate = new Date(dpvalue[0], dpvalue[1]-1,dpvalue[2]);
            $(this).datepicker('setDate', myDate);
        });
    })
}