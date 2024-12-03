$(document).ready(function () {
    let selectedClassId = 1;
    let selectedSubjectId = 1;
    let currentTab = 'grades';

    $(".btn-check").click(function () {
        selectedClassId = $(this).next("label").data("class-id");
        sessionStorage.setItem("selectedClassId", selectedClassId);
        switch (currentTab) {
            case 'absences': loadAbsencesContent(); break;
            case 'grades': loadContent(); break;
            case 'remarks': loadRemarkContent(); break;
        }
    });

    $(document).on("click", ".list-group-item", function () {
        selectedSubjectId = $(this).attr("id");
        sessionStorage.setItem("selectedContentId", selectedSubjectId);
        if (selectedClassId && selectedSubjectId) {
            loadTabContent();
        }
    });

    $(document).on("click", ".nav-item", function (e) {
        currentTab = $(e.target).attr('data-target');
        if (selectedClassId) {
            loadTabContent();
        }
    });

    $(document).on("click", "#addGradeButton button", function (e) {
        if (selectedClassId && selectedSubjectId) {
            $("#hiddenClassIdForGrade").val(selectedClassId);
            $("#hiddenSubjectIdForGrade").val(selectedSubjectId);
        } else {
            e.preventDefault();
            alert("Моля, изберете клас и предмет преди да добавите оценка.");
        }
    });

    $(document).on("click", "#addAbsenceButton button", function (e) {
        if (selectedClassId) {
            $("#hiddenClassIdForAbsence").val(selectedClassId);
        } else {
            e.preventDefault();
        }
    });

    $(document).on("click", "#addRemarkButton button", function (e) {
        if (selectedClassId) {
            $("#hiddenClassIdForRemark").val(selectedClassId);
        } else {
            e.preventDefault();
        }
    });

    function makeAjaxRequest(url, data, successCallback) {
        return $.ajax({
            url: url,
            type: 'GET',
            data: data,
            success: successCallback,
            error: function () {
                alert("Възникна грешка при зареждането на съдържанието.");
            }
        });
    }

    function updateUIVisibility() {
        const showGradesUI = currentTab === 'grades';
        $('#subjectsSidebar').toggle(showGradesUI);
        $('#mainContentContainer').toggleClass('col-lg-10', showGradesUI).toggleClass('col-lg-12', !showGradesUI);

        $('#addGradeButton').toggle(currentTab === 'grades');
        $('#addRemarkButton').toggle(currentTab === 'remarks');
        $('#addAbsenceButton').toggle(currentTab === 'absences');
    }

    function loadContent() {
        return makeAjaxRequest(
            '/Diary/LoadClassAndContent',
            { classId: selectedClassId },
            function (response) {
                $("#content").html(response);
                updateUIVisibility();
            }
        );
    }

    function loadGradeContent() {
        makeAjaxRequest(
            '/Diary/LoadGradeContent',
            { classId: selectedClassId, subjectId: selectedSubjectId },
            function (response) {
                $("#main-content").html(response);
                updateUIVisibility();
            }
        );
    }

    function loadRemarkContent() {
        makeAjaxRequest(
            '/Diary/LoadRemarksContent',
            { classId: selectedClassId },
            function (response) {
                $("#main-content").html(response);
                updateUIVisibility();
            }
        );
    }

    function loadAbsencesContent() {
        makeAjaxRequest(
            '/Diary/LoadAbsencesContent',
            { classId: selectedClassId, subjectId: selectedSubjectId },
            function (response) {
                $("#main-content").html(response);
                updateUIVisibility();
            }
        );
    }

    function loadTabContent() {
        switch (currentTab) {
            case 'absences': loadAbsencesContent(); break;
            case 'grades': loadGradeContent(); break;
            case 'remarks': loadRemarkContent(); break;
        }
    }

    async function initialize() {
        if (selectedClassId) {
            console.log("Initializing with class:", selectedClassId);
            $(`label[data-class-id="${selectedClassId}"]`).prev('.btn-check').prop('checked', true);
            await loadContent();

            if (selectedSubjectId) {
                console.log("Selecting subject:", selectedSubjectId);
                $(`#${selectedSubjectId}`).addClass('active');
                loadGradeContent();
            }
        }
    }

    initialize();
});