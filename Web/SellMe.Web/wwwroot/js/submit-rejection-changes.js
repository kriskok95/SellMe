$(document).ready(function () {
    let elementForRemove;
    $('.elm').on('click',
        function () {
            elementForRemove = $(this);
        });

    $('.create-button').on('click',
        function () {
            let rejectionId = $(this).val();
            console.log(rejectionId);

            $.ajax({
                url: '/Ads/SubmitRejectedAd',
                type: 'POST',
                data: "rejectionId=" + rejectionId,
                success: function () {
                    elementForRemove.hide('slow', function () { elementForRemove.remove() });
                    $(function () {
                        $('#modal').modal('show');
                        $('.modal-message').html('Your changes were submitted successfully. Your ad will be reviewed as soon as possible!');
                        $(function () {
                            var myModal = $('#modal');
                            clearTimeout(myModal.data('hideInterval'));
                            myModal.data('hideInterval', setTimeout(function () {
                                myModal.modal('hide');
                            }, 6000));
                        });
                    });
                }
            });
        });
});