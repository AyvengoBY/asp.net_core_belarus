function previewFile() {
    $('.text-danger').hide();
    var fileName = "/images/" + $('#uploadFileInput').val();
    $('#newImage').attr('src', fileName);
};

