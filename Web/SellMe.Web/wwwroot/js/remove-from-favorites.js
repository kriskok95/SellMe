$(document).ready(function () {
    $(".container-fluid").on('click', '.removeFromFavorites', function
        () {
        event.stopImmediatePropagation();
        var adId = $(this).val();

        $.ajax({
            url: "/Favorites/Remove",
            type: "GET",
            data: "adId=" + adId,
            dataType: 'json',
            success: function (response) {
                $(function () {
                    $('#remove-favorites-modal').modal('show');
                    $(function () {
                        var myModal = $('#remove-favorites-modal');
                        clearTimeout(myModal.data('hideInterval'));
                        myModal.data('hideInterval', setTimeout(function () {
                            myModal.modal('hide');
                        }, 2500));
                    });
                });
                $("#" + adId).fadeOut('slow', function () { $(this).remove(); });
            }
        });
    });
});