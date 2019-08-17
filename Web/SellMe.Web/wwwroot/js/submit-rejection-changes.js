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
                }

            });
        });
});