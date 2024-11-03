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

        loadContent();
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
                if (selectedClassId != null && selectedSubjectId != null) {
                    loadAbsencesContent();
                }
                break;
            case 'grades':
                if (selectedClassId != null) {
                    loadGradeContent();
                }
                break;
            case 'remarks':
                if (selectedClassId != null) {
                    loadRemarkContent();
                }
                break;
        }
    });

    function loadContent() {
        $.ajax({
            url: '/Diary/LoadClassAndContent',
            type: 'GET',
            data: { classId: selectedClassId },
            success: function (response) {
                $("#content").html(response);
            },
            error: function () {
                alert("Възникна грешка при зареждането на съдържанието.");
            }
        });
    }

    function loadGradeContent() {
        console.log("subjectId in loadGradeContent", selectedSubjectId);
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
        console.log("subjectId in loadGradeContent", selectedSubjectId);
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
        console.log("subjectId in loadGradeContent", selectedSubjectId);
        $.ajax({
            url: '/Diary/',
            type: 'GET',
            data: { classId: selectedClassId, subjectId: selectedSubjectId },
            success: function (response) {
                $("#main-content").html(response);
            },
            error: function () {
                alert("Възникна грешка при зареждането на съдържанието.");
            }
        });
    }
});

