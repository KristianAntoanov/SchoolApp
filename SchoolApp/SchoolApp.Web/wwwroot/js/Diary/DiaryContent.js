

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
                    $('#addAbsenceButton').show();
                    $('#addGradeButton').hide();
                    $('#addRemarkButton').hide();
                    loadAbsencesContent();
                }
                break;
            case 'grades':
                if (selectedClassId != null) {
                    $('#addGradeButton').show();
                    $('#addRemarkButton').hide();
                    $('#addAbsenceButton').hide();
                    loadGradeContent();
                }
                break;
            case 'remarks':
                if (selectedClassId != null) {
                    $('#addRemarkButton').show();
                    $('#addGradeButton').hide();
                    $('#addAbsenceButton').hide();
                    loadRemarkContent();
                }
                break;
        }
    });

    $(document).on("click", "#addGradeButton button", function (e) {
        if (selectedClassId != null && selectedSubjectId != null) {
            $("#hiddenClassIdForGrade").val(selectedClassId);
            $("#hiddenSubjectIdForGrade").val(selectedSubjectId);
        } else {
            e.preventDefault();
            alert("Моля, изберете клас и предмет преди да добавите оценка.");
        }
    });

    $(document).on("click", "#addAbsenceButton button", function (e) {
        if (selectedClassId != null) {
            $("#hiddenClassIdForAbsence").val(selectedClassId);
        } else {
            e.preventDefault();
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
                    $('#addGradeButton').show();
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