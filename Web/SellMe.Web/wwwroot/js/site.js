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
