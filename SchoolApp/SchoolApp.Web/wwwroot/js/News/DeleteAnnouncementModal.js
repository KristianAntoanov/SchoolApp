

document.getElementById('deleteModal').addEventListener('show.bs.modal', function (event) {
    var button = event.relatedTarget;
    var announcementId = button.getAttribute('data-announcement-id');
    var announcementTitle = button.getAttribute('data-announcement-title');

    document.getElementById('announcementTitle').textContent = announcementTitle;
    document.getElementById('deleteForm').action = '/News/DeleteAnnouncement/' + announcementId;
});
