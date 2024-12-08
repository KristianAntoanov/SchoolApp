

function initializeConfirmationModal() {
    const formsRequiringConfirmation = ['gradeForm', 'remarkForm', 'absenceForm'];

    formsRequiringConfirmation.forEach(formId => {
        const form = document.getElementById(formId);
        if (form) {
            form.addEventListener('submit', function (e) {
                e.preventDefault();

                const modal = new bootstrap.Modal(document.getElementById('confirmationModal'));
                modal.show();

                document.getElementById('confirmActionBtn').onclick = function () {
                    form.submit();
                };
            });
        }
    });
}

// Инициализираме когато DOM-ът е зареден
document.addEventListener('DOMContentLoaded', initializeConfirmationModal);