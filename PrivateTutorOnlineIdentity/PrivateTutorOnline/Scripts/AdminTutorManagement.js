$(document).ready(function () {
    $('.activate-tutor').click(function () {
        var button = $(this);
        var Id = $(this).attr('data-tutor-id');
         
        $.ajax({
            method: "POST",
            data: { TutorId: parseInt(Id) },
            url: "/Admin/ActivateTutor",
            success: function (response) {
                console.log(response);
                $('.row-tutor-is-activated-' + Id).html('<span style="color:blue;">Đã kích hoạt</span>');
                button.css('display', 'none');
                $.toaster({
                    message: 'Kích hoạt tài khoản thành công',
                    priority: 'success'
                });
            },
            error: function (response) {
                $.toaster({
                    message: 'Có lỗi xảy ra ! Kích hoạt tài khoản thất bại',
                    priority: 'warning'
                });
                console.log(response.responseText);
            }

        });
    });
    $('.enable-tutor').click(function () {
        var checkbox = $(this);
        var Id = $(this).attr('data-tutor-id');

        $.ajax({
            method: "POST",
            data: { TutorId: Id },
            url: "/Admin/DisableTutor",
            success: function (response) {
                console.log(response);

                if (checkbox.prop('checked')) {
                    $.toaster({
                        message: 'Mở tài khoản thành công',
                        priority: 'success'
                    });
                    $('.row-tutor-is-enable-' + Id).html('<span style="color:blue;">Mở</span>');
                }
                else {
                    $.toaster({
                        message: 'Khóa tài khoản thành công',
                        priority: 'success'
                    });
                    $('.row-tutor-is-enable-' + Id).html('<span style="color:blue;">Khóa</span>');
                }
            },
            error: function (response) {
                $.toaster({
                    message: 'Có lỗi xảy ra ! Mở khóa tài khoản thất bại',
                    priority: 'warning'
                });
                console.log(response.responseText);
            }

        });
    });

});