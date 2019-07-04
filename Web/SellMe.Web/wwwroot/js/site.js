//Get subcategories by selected categoryId

$(document).ready(function () {
	$('#CreateAdDetailInputModel_CategoryId').on("change",
		function () {
			var categoryId = $(this).find('option:selected').val();
			console.log(categoryId);
			$.ajax({
				url: "GetSubcategories",
				type: "GET",
				data: "categoryId=" + categoryId,
				dataType: 'json',
				success: function (response) {
					$("#CreateAdDetailInputModel_SubCategoryId").empty(); //Reset subcategories
					$("#CreateAdDetailInputModel_SubCategoryId").append("<option>--Select--</option>");
					$.each(response, function (key, value) {
						$("#CreateAdDetailInputModel_SubCategoryId").append('<option value=' + value.id + '>' + value.name + '</option>');
						console.log(value.id);
					});
				}
			});
		});
});


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
