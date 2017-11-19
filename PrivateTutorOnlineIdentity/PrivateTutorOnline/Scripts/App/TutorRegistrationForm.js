$(document).ready(function () {
    $('#btnUpload').click(function () {

        // Checking whether FormData is available in browser  
        if (window.FormData !== undefined) {
            var myData = {
                "FullName": $('input[name="FullName"]').val(),
                "Gender": $('input[name="Gender"]').val(),
                "IdentityNumber": $('input[name="IdentityNumber"]').val(),
                "HomeTown": $('input[name="HomeTown"]').val(),
                "Address": $('input[name="Address"]').val()
                 
            }
            $.ajax({
                type: "POST",
                url: '/',
                data: myEmail,
                dataType: "json",
                success: function (result) {
                    alert("Mail envoyé.");
                },
                error: function (result) {
                    alert("Echec lors de l'envoi du mail.");
                }
            });
        }
    });
});