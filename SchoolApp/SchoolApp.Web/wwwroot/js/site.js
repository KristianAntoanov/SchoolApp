// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    let selectedClassId = null;
    let selectedContentId = null;
    console.log("in classBtn")

    $(".btn-check").click(function () {
        console.log("Бутонът е кликнат!");
        selectedClassId = $(this).next("label").data("class-id");
        localStorage.setItem("selectedClassId", selectedClassId);
        console.log("selectedClassId:", selectedClassId);
        loadContent();
    });

    $(".nav-link").click(function () {
        selectedContentId = $(this).attr("id");
        localStorage.setItem("selectedContentId", selectedContentId);
        loadContent();
    });

    function loadContent() {
        if (selectedClassId || selectedContentId) {
            $.ajax({
                url: '/Diary/LoadClassAndContent',
                type: 'GET',
                data: { classId: selectedClassId, subjectId: selectedContentId },
                success: function (response) {
                    $("#main-content").html(response);
                },
                error: function () {
                    alert("Възникна грешка при зареждането на съдържанието.");
                }
            });
        }
    }
});



