

document.getElementById('deleteModal').addEventListener('show.bs.modal', function (event) {
    var button = event.relatedTarget;
    var albumId = button.getAttribute('data-album-id');
    var albumTitle = button.getAttribute('data-album-title');

    document.getElementById('albumTitleToDelete').textContent = albumTitle;
    document.getElementById('deleteForm').action = 'Gallery/Delete/' + albumId;
});