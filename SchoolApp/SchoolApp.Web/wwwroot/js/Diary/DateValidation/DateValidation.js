

document.addEventListener('DOMContentLoaded', function () {
    const dateInput = document.getElementById('addedOn');
    const form = document.querySelector('form');

    function validateDate() {
        if (!dateInput) return true;

        const selectedDate = new Date(dateInput.value);
        const today = new Date();

        const oneYearAgo = new Date(today);
        oneYearAgo.setFullYear(today.getFullYear() - 1);

        const validationSpan = document.querySelector('[data-valmsg-for="AddedOn"]') ||
            createValidationSpan();

        if (selectedDate < oneYearAgo) {
            validationSpan.textContent = 'Данните не могат да бъдат въведени за предходни години';
            validationSpan.classList.add('field-validation-error');
            return false;
        } else {
            validationSpan.textContent = '';
            validationSpan.classList.remove('field-validation-error');
            return true;
        }
    }

    function createValidationSpan() {
        const span = document.createElement('span');
        span.setAttribute('data-valmsg-for', 'AddedOn');
        span.classList.add('field-validation-error');
        dateInput.parentNode.appendChild(span);
        return span;
    }

    if (dateInput) {
        dateInput.addEventListener('blur', validateDate);
    }

    if (form) {
        form.addEventListener('submit', function (e) {
            if (!validateDate()) {
                e.preventDefault();
            }
        });
    }
});