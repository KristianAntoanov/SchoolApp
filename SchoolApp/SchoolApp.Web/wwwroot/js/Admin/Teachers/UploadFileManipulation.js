

document.addEventListener('DOMContentLoaded', function () {
    const imageInput = document.getElementById('imageInput');
    const fileNameDisplay = document.getElementById('selectedFileName');
    const clearFileButton = document.getElementById('clearFileButton');
    const browseButton = document.getElementById('browseButton');

    if (fileNameDisplay.textContent.trim() !== 'Няма избран файл') {
        clearFileButton.style.display = 'block';
    }

    browseButton.addEventListener('click', function () {
        imageInput.click();
    });

    imageInput.addEventListener('change', function () {
        if (this.files.length > 0) {
            fileNameDisplay.textContent = this.files[0].name;
            clearFileButton.style.display = 'block';
        } else {
            fileNameDisplay.textContent = 'Няма избран файл';
            clearFileButton.style.display = 'none';
        }
    });

    clearFileButton.addEventListener('click', function () {
        imageInput.value = '';
        fileNameDisplay.textContent = 'Няма избран файл';
        clearFileButton.style.display = 'none';
    });
});