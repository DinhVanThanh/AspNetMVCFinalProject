$(document).ready(function () {
    $('.approve-recieve-class-request').click(function () {
        var button = $(this);
        var Id = $(this).attr('data-class-id');
        $.ajax({
            method: "POST",
            data: { ClassId: Id, IsApproved: true },
            url: "/ManageRegistrationClasses/ApproveOrRejectEnrollClass",
            success: function (response) {
                console.log(response);
                $('#row-status-' + Id).html('<span style="color:red">Đã duyệt nhận lớp</span>');
                button.css('display', 'none');
                $('#reject-recieve-class-request-' + Id).css('display', 'none');

                $.toaster({
                    message: 'Duyệt thành công',
                    priority: 'success'
                });

            },
            error: function (response) {
                $.toaster({
                    message: 'Duyệt thất bại',
                    priority: 'warning'
                });
                console.log(response.responseText);
            }

        });
    });
    $('.reject-recieve-class-request').click(function () {
        var button = $(this);
        var Id = $(this).attr('data-class-id');
        $.ajax({
            method: "POST",
            data: { ClassId: Id, IsApproved: false },
            url: "/ManageRegistrationClasses/ApproveOrRejectEnrollClass",
            success: function (response) {
                console.log(response);
                $('#row-status-' + Id).html('<span style="color:red">Chờ nhận lớp</span>');
                $('#approve-recieve-class-request-' + Id).css('display', 'none'); 
                button.css('display', 'none');

                $.toaster({
                    message: 'Từ chối thành công',
                    title: 'Duyệt',
                    priority: 'success'
                });
            },
            error: function (response) {
                $.toaster({
                    message: 'Có lỗi xảy ra ! Từ chối thất bại',
                    priority: 'warning'
                });
                console.log(response.responseText);
            }

        });
    });

});