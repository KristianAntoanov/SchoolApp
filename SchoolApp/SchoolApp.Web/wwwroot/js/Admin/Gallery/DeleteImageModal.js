

document.getElementById('deleteImageModal').addEventListener('show.bs.modal', function (event) {
    var button = event.relatedTarget;
    var imageId = button.getAttribute('data-image-id');
    document.getElementById('imageIdToDelete').value = imageId;
});