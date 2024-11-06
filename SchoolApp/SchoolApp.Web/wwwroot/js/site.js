// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    let selectedClassId = null;
    let selectedSubjectId = null;
    let currentTab = 'grades';

    $(".btn-check").click(function () {

        selectedClassId = $(this).next("label").data("class-id");
        localStorage.setItem("selectedClassId", selectedClassId);

        switch (currentTab) {
            case 'absences':
                loadAbsencesContent();
                break;
            case 'grades':
                loadContent();
                break;
            case 'remarks':
                loadRemarkContent();
                break;
        }
    });

    $(document).on("click", ".list-group-item", function () {

        selectedSubjectId = $(this).attr("id");
        localStorage.setItem("selectedContentId", selectedSubjectId);

        if (selectedClassId != null && selectedSubjectId != null) {
            // Вместо директно да зареждаме оценките, проверяваме кой е текущият раздел
            switch (currentTab) {
                case 'grades':
                    loadGradeContent();
                    break;
                case 'remarks':
                    loadRemarkContent();
                    break;
                case 'absences':
                    loadAbsencesContent();
                    break;
            }
        }
    });

    $(document).on("click", ".nav-item", function (e) {
        let target = $(e.target).attr('data-target'); // Взима data-target атрибута

        currentTab = target;

        switch (target) {
            case 'absences':
                if (selectedClassId != null) {
                    $('#addGradeButton').hide();
                    loadAbsencesContent();
                }
                break;
            case 'grades':
                if (selectedClassId != null) {
                    $('#addGradeButton').show();
                    loadGradeContent();
                }
                break;
            case 'remarks':
                if (selectedClassId != null) {
                    $('#addGradeButton').hide();
                    loadRemarkContent();
                }
                break;
        }
    });

    $(document).on("click", "#addGradeButton button", function (e) {
        if (selectedClassId != null && selectedSubjectId != null) {
            $("#hiddenClassId").val(selectedClassId);
            $("#hiddenSubjectId").val(selectedSubjectId);
        } else {
            e.preventDefault();
            alert("Моля, изберете клас и предмет преди да добавите оценка.");
        }
    });

    function loadContent() {
        $.ajax({
            url: '/Diary/LoadClassAndContent',
            type: 'GET',
            data: { classId: selectedClassId },
            success: function (response) {
                $("#content").html(response);
                if (currentTab !== 'grades') {
                    $('#subjectsSidebar').hide();
                    $('#mainContentContainer').removeClass('col-lg-10').addClass('col-lg-12');
                } else {
                    $('#subjectsSidebar').show();
                    $('#mainContentContainer').removeClass('col-lg-12').addClass('col-lg-10');
                }
            },
            error: function () {
                alert("Възникна грешка при зареждането на съдържанието.");
            }
        });
    }

    function loadGradeContent() {
        $.ajax({
            url: '/Diary/LoadGradeContent',
            type: 'GET',
            data: { classId: selectedClassId, subjectId: selectedSubjectId },
            success: function (response) {
                $("#main-content").html(response);
                if (currentTab === 'grades') {
                    $('#subjectsSidebar').show();
                    $('#mainContentContainer').removeClass('col-lg-12').addClass('col-lg-10');
                }
            },
            error: function () {
                alert("Възникна грешка при зареждането на съдържанието.");
            }
        });
    }

    function loadRemarkContent() {
        $.ajax({
            url: '/Diary/LoadRemarksContent',
            type: 'GET',
            data: { classId: selectedClassId },
            success: function (response) {
                $("#main-content").html(response);
                if (currentTab === 'remarks') {
                    $('#subjectsSidebar').hide();
                    $('#mainContentContainer').removeClass('col-lg-10').addClass('col-lg-12');
                }
            },
            error: function () {
                alert("Възникна грешка при зареждането на съдържанието.");
            }
        });
    }

    function loadAbsencesContent() {
        $.ajax({
            url: '/Diary/LoadAbsencesContent',
            type: 'GET',
            data: { classId: selectedClassId, subjectId: selectedSubjectId },
            success: function (response) {
                $("#main-content").html(response);
                if (currentTab === 'absences') {
                    $('#subjectsSidebar').hide();
                    $('#mainContentContainer').removeClass('col-lg-10').addClass('col-lg-12');
                }
            },
            error: function () {
                alert("Възникна грешка при зареждането на съдържанието.");
            }
        });
    }
});

document.addEventListener('DOMContentLoaded', function () {
    const rows = document.querySelectorAll('tbody tr');

    rows.forEach(row => {
        const gradeBoxes = row.querySelectorAll('.grade-box');
        const gradeDisplay = row.querySelector('input[type="number"][name*="Grade"]');

        gradeBoxes.forEach(box => {
            box.addEventListener('click', function () {
                // Remove selected class from all boxes in this row
                gradeBoxes.forEach(b => b.classList.remove('selected'));

                // Add selected class to clicked box
                this.classList.add('selected');

                // Update grade display
                const grade = this.getAttribute('data-grade');
                gradeDisplay.value = grade;
            });
        });
    });
});

$(document).ready(function () {
    // Функция за инициализиране на popovers
    function initializePopovers() {
        $('[data-bs-toggle="popover"]').popover({
            trigger: 'click',
            html: true,
            container: 'body'
        });
    }

    // Първоначално инициализиране
    initializePopovers();

    // При клик върху popover елемент
    $(document).on('click', '[data-bs-toggle="popover"]', function (e) {

        const $this = $(this);
        const isVisible = $this.data('bs.popover') &&
            $this.data('bs.popover').tip &&
            $this.data('bs.popover').tip.classList.contains('show');

        // Скрий всички други popovers
        $('[data-bs-toggle="popover"]').not(this).popover('hide');

        // Ако този popover е вече видим, скрий го
        if (isVisible) {
            $this.popover('hide');
        } else {
            $this.popover('show');
        }
    });

    // При клик извън popover
    $(document).on('click', function (e) {
        if ($(e.target).data('bs-toggle') !== 'popover' &&
            $(e.target).parents('[data-bs-toggle="popover"]').length === 0 &&
            $(e.target).parents('.popover').length === 0) {

            $('[data-bs-toggle="popover"]').popover('hide');
        }
    });

    // Унищожаване на popovers при скриване
    $(document).on('hidden.bs.popover', function (e) {
        $(e.target).data('bs-popover', null);
        initializePopovers();
    });
});