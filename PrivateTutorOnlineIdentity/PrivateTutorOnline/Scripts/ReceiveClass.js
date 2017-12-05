$(document).ready(function () {
    $('.receiveClass').click(function () {
        var button = $(this); 
        var Id = $(this).attr('data-classId');
        $.ajax({
            method: "POST", 
            data: { ClassId : Id},
            url: "/ManageRegistrationClasses/ReceiveClass" , 
            success: function (response) {
                var currentdate = new Date();
                var datetime = "Last Sync: " + currentdate.getDate() + "/"
                    + (currentdate.getMonth() + 1) + "/"
                    + currentdate.getFullYear() + " "
                    + currentdate.getHours() + ":"
                    + currentdate.getMinutes() + ":"
                    + currentdate.getSeconds();
                console.log(response);
                $('.recieved-date-' + Id).text(datetime);
                $('#row-status-' + Id).html('<span style="color:red">Chờ duyệt nhận lớp từ phụ huynh</span>');
                button.css('display', 'none');
                $.toaster({
                    message: 'Đăng kí nhập lớp thành công', 
                    priority: 'success'
                });
            } ,
            error: function (response) {
                $.toaster({
                    message: 'Có lỗi xảy ra ! Đăng kí nhận lớp thất bại',
                    priority: 'warning'
                });
                console.log(response.responseText);
            }
           
        });
    })

});