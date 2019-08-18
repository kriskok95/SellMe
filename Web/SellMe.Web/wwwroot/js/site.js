
//Archie Ad 
$(document).ready(function () {
    var elementForRemove;
    $(".trElm").click(function () {
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
                $(function () {
                    $('#modal').modal('show');
                    $('.modal-message').html('You have successfully archived this ad!');
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

//Input file 
$(".custom-file-input").on("change", function () {
    var fileName = $(this).val().split("\\").pop();
    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
});





