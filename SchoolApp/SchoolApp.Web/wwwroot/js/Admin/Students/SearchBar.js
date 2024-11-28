

function searchStudents() {
    var searchValue = document.getElementById('searchInput').value;
    window.location.href = indexUrl + '?search=' + encodeURIComponent(searchValue);
}

function clearSearch() {
    window.location.href = indexUrl;
}

document.getElementById('searchInput').addEventListener('keypress', function (e) {
    if (e.key === 'Enter') {
        searchStudents();
    }
});