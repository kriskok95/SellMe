﻿$(document).ready(function () {
    var elementForRemove;
    $(".removeElement").click(function () {
        elementForRemove = $(this);
    });
    $(".activate").click(function () {
        var adId = $(this).val();
        console.log(adId);

        $.ajax({
            url: "ActivateAd",
            type: "GET",
            data: "adId=" + adId,
            dataType: 'json',
            success: function (response) {
                elementForRemove.hide('slow', function () { elementForRemove.remove(); });
                $(function () {
                    $('#modal').modal('show');
                    $('.modal-message').html('You have successfully activated this ad!');
                    $(function () {
                        var myModal = $('#modal');
                        clearTimeout(myModal.data('hideInterval'));
                        myModal.data('hideInterval', setTimeout(function () {
                            myModal.modal('hide');
                        }, 2500));
                    });
                });
            }
        });
    });
});