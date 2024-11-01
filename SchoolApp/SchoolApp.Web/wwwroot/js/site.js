// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {
    let selectedClassId = null;
    let selectedSubjectId = null;
    console.log("in classBtn")

    $(".btn-check").click(function () {
        console.log("Бутонът е кликнат!");
        selectedClassId = $(this).next("label").data("class-id");
        localStorage.setItem("selectedClassId", selectedClassId);
        console.log("selectedClassId:", selectedClassId);
        loadContent();
    });

    $(document).on("click", ".list-group-item", function () {
        console.log("Бутонът за предмет е кликнат!");
        selectedSubjectId = $(this).attr("id");
        localStorage.setItem("selectedContentId", selectedSubjectId);
        console.log("selectedsubjectId:", selectedSubjectId);
        if (selectedClassId != null && selectedSubjectId != null) {
            console.log("Извиква loadGradeContent")
            loadGradeContent();
        }
    });

    //$(".nav-link").click(function (e) {

    //    // Зарежда съдържанието чрез Ajax
    //    $.ajax({
    //        url: url,
    //        type: 'GET',
    //        success: function (data) {
    //            $('#main-content2').html(data); // Вмъква получените данни в основното съдържание
    //        },
    //        error: function () {
    //            alert("Възникна грешка при зареждането на съдържанието.");
    //        }
    //    });
    //});

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
            },
            error: function () {
                alert("Възникна грешка при зареждането на съдържанието.");
            }
        });
    }
});

