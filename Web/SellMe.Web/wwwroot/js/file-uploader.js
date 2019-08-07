// These are template HTMLs for new dynamically added input fields.
let newElementHtml =
    '<div class="image">' +
        '<div class="button-delete"><i class="fas fa-times-circle"></i></div>' +
    '     <input type="file" id="CreateAdDetailInputModel_Images" name="CreateAdDetailInputModel.Images" class="image-upload" />' +
    '</div>';

// The row - container of the input fields and their holders.
let newRowHtml = '<div class="image-row d-flex justify-content-start mt-5"></div>';

// This function loads an image as the background of the parent element (div),
// when an image is selected through an input[type="file"] field.
function loadImageIntoBackground(element, image) {
    let file = image;
    let reader = new FileReader();
    let parent = $(element).parent();

    reader.onloadend = function () {
        $(parent).css('background-image', 'url("' + reader.result + '")');
    };

    if (file) {
        reader.readAsDataURL(file);
    }
}

// This function attaches event to a newly created dynamic input field
function attachEvents() {
    $('.image-upload').change(function () {
        loadImageIntoBackground(this, this.files[0]);

        $(this).hide();

        let totalImageElements = $($($(this).parent()).parent()).children().length;

        let rowsCount = $($($($(this).parent()).parent()).parent()).children().length;

        if (rowsCount === 2 && totalImageElements === 4) {
            // Create the new dynamic input field
            let newInputField = $(newElementHtml);

            // If it is not the first element on the row, add a miniature margin to it
            if (totalImageElements >= 1) {
                newInputField.addClass('ml-5');
            }
        }
        else if(totalImageElements === 4) {
            // If we have more than 4 images already added to this row.
            // Dynamically create a new row so that the new input fields and
            // newly selected images go on the new row.
            // That way the miniature design we are trying to implement will not break horribly.
            let newRow = $(newRowHtml);
            newRow.append(newElementHtml);
            $($($($(this).parent()).parent()).parent()).append(newRow);
        } else {
            // Create the new dynamic input field
            let newInputField = $(newElementHtml);

            // If it is not the first element on the row, add a miniature margin to it
            if(totalImageElements >= 1) {
                newInputField.addClass('ml-5');
            }

            $($($(this).parent()).parent()).append(newInputField);
        }

        // Attach the event handlers again to the newly created input field.
        attachEvents();
    });
}

attachEvents();
