$(document).ready(function () {
    var elementForRemove;
    $(".elm").click(function () {
        elementForRemove = $(this);
    });

    $('.approve-button').on('click',
        function () {
            let adId = $(this).val();

            $.ajax({
                url: "/Administration/Ads/Approve",
                type: "GET",
                data: "adId=" + adId,
                dataType: 'json',
                success: function () {
                    elementForRemove.hide('slow', function () { elementForRemove.remove(); });
                    $(function () {
                        $('#modal').modal('show');
                        $('.modal-message').html('You have successfully approved this ad!');
                        $(function () {
                            var myModal = $('#modal');
                            clearTimeout(myModal.data('hideInterval'));
                            myModal.data('hideInterval', setTimeout(function () {
                                myModal.modal('hide');
                            }, 3000));
                        });
                    });
                }
            });
        });
});