$(document).ready(function () {
    $('.btn-approve').click(function () {
        var button = $(this);
        var Id = $(this).attr('data-class-id');
        $.ajax({
            method: "POST",
            data: { ClassId: Id, IsApproved : true},
            url: "/Admin/ApproveClass",
            success: function (response) {
                console.log(response);
                $('#row-status-' + Id).html('<span style="color:red">Duyệt</span>');
                $('.btn-approve').css('display', 'none');
                $('.btn-not-approve').css('display', 'none');
            },
            error: function (response) {
                console.log(response.responseText);
            }

        });
    });
    $('.btn-not-approve').click(function () {
        var button = $(this);
        var Id = $(this).attr('data-class-id');
        $.ajax({
            method: "POST",
            data: { ClassId: Id, IsApproved: false },
            url: "/Admin/ApproveClass",
            success: function (response) {
                console.log(response);
                $('#row-status-' + Id).html('<span style="color:red">Không duyệt</span>');
                $('.btn-approve').css('display', 'none');
                $('.btn-not-approve').css('display', 'none');
            },
            error: function (response) {
                console.log(response.responseText);
            }

        });
    });

});