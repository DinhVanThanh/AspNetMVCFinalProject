$(document).ready(function () {
    $('.receiveClass').click(function () {
        var button = $(this); 
        var Id = $(this).attr('data-classId');
        $.ajax({
            method: "POST", 
            data: { ClassId : Id},
            url: "/ManageRegistrationClasses/ReceiveClass" , 
            success: function (response) {
                console.log(response);
                $('#row-status-' + Id).html('<span style="color:red">Đã nhận</span>');
                button.css('display', 'none');
            } ,
            error: function (response) {
                console.log(response.responseText);
            }
           
        });
    })

});