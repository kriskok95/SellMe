$(document).ready(function () {
    $(".container-fluid").on('click', '.addToFavorites', function test() {
        event.stopImmediatePropagation();
        var adId = $(this).val();
        console.log(adId);

        $.ajax({
            url: "/Favorites/Add",
            type: "GET",
            data: "adId=" + adId,
            dataType: 'json',
            success: function (response) {
                $(function () {
                    $('#add-favorites-modal').modal('show');
                    $(function () {
                        var myModal = $('#add-favorites-modal');
                        clearTimeout(myModal.data('hideInterval'));
                        myModal.data('hideInterval', setTimeout(function () {
                            myModal.modal('hide');
                        }, 2500));
                    });
                });
                $("#" + adId).replaceWith('<button value="' + adId + '"' + 'class="btn removeFromFavorites" id="' + adId + '"><img src="/img/favorite-index.png" alt="favorite" /></button>');
            }
        });
    });

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
                $("#" + adId).replaceWith('<button value="' + adId + '"' + 'class="btn addToFavorites" id="' + adId + '"><img src="/img/not-favorite-index.png" alt="not-favorite" /></button>');
            }
        });
    });
});