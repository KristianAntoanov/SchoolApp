

function initializeConfirmationModal() {
    // Намираме всички форми, които се нуждаят от потвърждение
    const formsRequiringConfirmation = ['gradeForm', 'remarkForm', 'absenceForm'];

    formsRequiringConfirmation.forEach(formId => {
        const form = document.getElementById(formId);
        if (form) {
            form.addEventListener('submit', function (e) {
                e.preventDefault(); // Спираме нормалния submit

                // Показваме модалния прозорец
                const modal = new bootstrap.Modal(document.getElementById('confirmationModal'));
                modal.show();

                // Добавяме listener за бутона за потвърждение
                document.getElementById('confirmActionBtn').onclick = function () {
                    form.submit(); // Изпращаме формата
                };
            });
        }
    });
}

// Инициализираме когато DOM-ът е зареден
document.addEventListener('DOMContentLoaded', initializeConfirmationModal);