$(document).ready(function () {
    var elementForRemove;
    $(".elm").click(function () {
        elementForRemove = $(this);
    });

    $('.unblock-user').on('click',
        function () {
            let userId = $(this).val();

            $.ajax({
                method: "POST",
                url: "/Administration/Users/Unblock",
                data: "userId=" + userId,
                dataType: "json",
                success: function () {
                    elementForRemove.hide('slow', function () { elementForRemove.remove(); });
                    $(function () {
                        $('#modal').modal('show');
                        $('.modal-message').html('You have successfully unblocked this user!');
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