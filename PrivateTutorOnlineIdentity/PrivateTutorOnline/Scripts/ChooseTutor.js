$(document).ready(function () {
    $('.btn-choose-tutor').click(function () {
         
        var Id = $(this).attr('data-tutor-id');
        $.ajax({
            method: "POST", 
            data: { TutorId : Id},
            url: "/Tutors/ChooseTutor" , 
            success: function (response) {  
                 
                $.toaster({
                    message: 'Chọn gia sư thành công', 
                    priority: 'success'
                });
            } ,
            error: function (response) {
                $.toaster({
                    message: 'Có lỗi xảy ra ! Chọn gia sư thất bại',
                    priority: 'warning'
                });
                console.log(response.responseText);
            }
           
        });
    })

});