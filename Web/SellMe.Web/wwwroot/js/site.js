
//Archie Ad 
$(document).ready(function () {
	var elementForRemove;
    $(".trElm").click(function() {
        elementForRemove = $(this);
    });
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



