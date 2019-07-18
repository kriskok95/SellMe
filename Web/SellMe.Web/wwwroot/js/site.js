
//Archie Ad 
$(document).ready(function () {
	var elementForRemove;
	$(".trElm").click(function () {
		elementForRemove = $(this);
	})
	$(".archive").click(function () {
		var adId = $(this).val();
		console.log(adId);

		$.ajax({
            url: "ArchiveAd",
			type: "GET",
			data: "adId=" + adId,
			dataType: 'json',
			success: function (response) {
                elementForRemove.hide('slow', function () { elementForRemove.remove(); });
			}
		});
	});
});

//Activate ad
$(document).ready(function () {
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
			}
		});
	});
});

//Input file 
$(".custom-file-input").on("change", function () {
	var fileName = $(this).val().split("\\").pop();
	$(this).siblings(".custom-file-label").addClass("selected").html(fileName);
});


//Add to favorites
$(document).ready(function () {
	$(".container-fluid").on('click', '.addToFavorites', function () {
		event.stopImmediatePropagation();
		var adId = $(this).val();

		$.ajax({
			url: "/Favorites/Add",
			type: "GET",
			data: "adId=" + adId,
			dataType: 'json',
			success: function (response) {
				alert("You have successfully added this ad to favorites!");
				$("#" + adId).replaceWith('<button type="button" value="' + adId + '"' + 'class="btn btn-danger btn-block mt-4 removeFromFavorites" id="' + adId + '">Remove From Favorites</button>');
			}
		});
    });

    //Remove from favorites
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
					$("#" + adId).replaceWith('<button type="button" value="' + adId + '"' + 'class="btn btn-primary btn-block mt-4 addToFavorites" id="' + adId + '">Add To Favorites</button>');
				}
			});
		});
});

