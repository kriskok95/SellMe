$(document).ready(function () {
    $(".container-fluid").on('click', '.addToFavorites', function test () {
        event.stopImmediatePropagation();
        var adId = $(this).val();
        console.log(adId);

        $.ajax({
            url: "/Favorites/Add",
            type: "GET",
            data: "adId=" + adId,
            dataType: 'json',
            success: function (response) {
                alert("You have successfully added this ad to favorites!");
                $("#" + adId).replaceWith('<button value="' + adId + '"' + 'class="btn removeFromFavorites" id="' + adId + '"><img src="/img/favorite.png" alt="favorite" /></button>');
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
                    alert("You have successfully removed this ad from favorites!");
                    $("#" + adId).replaceWith('<button value="' + adId + '"' + 'class="btn addToFavorites" id="' + adId + '"><img src="/img/not-favorite.png" alt="not-favorite" /></button>');
                }
            });
        });
});