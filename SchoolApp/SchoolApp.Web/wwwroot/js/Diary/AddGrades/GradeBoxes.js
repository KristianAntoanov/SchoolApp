

document.addEventListener('DOMContentLoaded', function () {
    const rows = document.querySelectorAll('tbody tr');
    rows.forEach(row => {
        const gradeBoxes = row.querySelectorAll('.grade-box');
        const gradeDisplay = row.querySelector('.grade-display');
        const realGradeInput = row.querySelector('.real-grade-value');

        if (row.querySelector('.grade-box.selected').getAttribute('data-grade') === '0') {
            gradeDisplay.value = '';
            realGradeInput.value = '0';
        }

        gradeBoxes.forEach(box => {
            box.addEventListener('click', function () {
                gradeBoxes.forEach(b => b.classList.remove('selected'));
                this.classList.add('selected');
                const grade = this.getAttribute('data-grade');
                gradeDisplay.value = grade === '0' ? '' : grade;
                realGradeInput.value = grade;
            });
        });
    });
});