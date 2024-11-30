

document.getElementById('deleteModal').addEventListener('show.bs.modal', function (event) {
    var button = event.relatedTarget;
    var newsId = button.getAttribute('data-news-id');
    var newsTitle = button.getAttribute('data-news-title');

    document.getElementById('newsTitle').textContent = newsTitle;
    document.getElementById('deleteForm').action = '/News/Delete/' + newsId;
});
