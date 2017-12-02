$(document).ready(function () {
    $('.btn-approve').click(function () {
        var button = $(this);
        var Id = $(this).attr('data-class-id');
        $.ajax({
            method: "POST",
            data: { ClassId: Id, IsApproved : true},
            url: "/Admin/ApproveOrRejectClass",
            success: function (response) {
                console.log(response);
                $('#row-status-' + Id).html('<span style="color:red">Duyệt</span>');
                button.css('display', 'none');
                $('.btn-not-approve').each(function () {
                    if ($(this).attr('data-class-id') === Id) {
                        $(this).css('display', 'none');
                    }
                })
                
                $.toaster({ 
                    message : 'Duyệt thành công', 
                    title : 'Duyệt', 
                    priority: 'success' 
                });

            },
            error: function (response) {
                $.toaster({
                    message: 'Duyệt thất bại',
                    title: 'Duyệt',
                    priority: 'warning'
                });
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
            url: "/Admin/ApproveOrRejectClass",
            success: function (response) {
                console.log(response);
                $('#row-status-' + Id).html('<span style="color:red">Không duyệt</span>');
                $('.btn-approve').each(function () {
                    if ($(this).attr('data-class-id') === Id) {
                        $(this).css('display', 'none');
                    }
                })
                 
                 
                button.css('display', 'none');

                $.toaster({
                    message: 'Không duyệt thành công',
                    title: 'Duyệt',
                    priority: 'success'
                });
            },
            error: function (response) {
                $.toaster({
                    message: 'Không duyệt thất bại',
                    title: 'Không Duyệt',
                    priority: 'warning'
                });
                console.log(response.responseText);
            }

        });
    });

});