$(document).ready(function () {
    $('#SearchByIdCheckBox').click(function () {

        if ($(this).prop('checked')) {
            $(this).attr("value", "true");
            console.log($(this).attr("value"));
        } else {
            $(this).attr("value", "false");
            console.log($(this).attr("value"));
        }
    });
}); 