"use strict";

$(document).ready(function () {
    var startHours = $("#totalHoursStart").html();
    $('#totalHours').text(startHours + " Total Hours");

    $(".hoursInput").change(function () {
        var sum = 0;
        $(".hoursInput").each(function () {
            sum += parseInt($(this).val()) || 0;
        });
        $('#totalHours').text(sum + " Total Hours");
    });
});

//$(document).ready(function () {
//    function changeTotalHours() {
//        $('#totalHours').text(15);
//    };

//    $('#_hoursworkedrow').change(function () {
//        changeTotalHours();
//        console.log("hit")
//    });
//});

function AddRow(_dayOfRow, _entryday) {
    $.ajax({
        async: false,
        data: { entryDay: _entryday, projectID: 0, activityID: 0 },
        url: '/Home/AddHourWorked'
    }).success(function (partialView) {
        var divID = "#hoursworkedrow" + _dayOfRow.id.slice(-3);
        console.log(divID);
        $(divID).append(partialView);
    });
}

//$(document).ready(function () {
//    $('#save').click(function () {
//        alert("HERE");
//    });
//});

function disableDiv() {
    $("#EditHoursWorked :input").attr("readonly", "readonly");
    $("#addhourworkedMon").attr("disabled", true);$("#addhourworkedTue").attr("disabled", true);
    $("#addhourworkedWed").attr("disabled", true);$("#addhourworkedThu").attr("disabled", true);
    $("#addhourworkedFri").attr("disabled", true);$("#addhourworkedSat").attr("disabled", true);
    $("#addhourworkedSun").attr("disabled", true);$("#lastWeekHours").attr("disabled", true);
    $("#submitButton").attr("disabled", true);
}

function CopyHours() {
    var _currentWeek = new Date($('#currentWeekEndDate').val());
    console.log("Current Week ISO: " + _currentWeek);
    $.ajax({
        contentType: "application/json",
        type: 'POST',
        data: JSON.stringify({
            currentWeek: _currentWeek
        }),
        url: '/Home/GetLastWeek',
        dataType: 'json'
    }).success(function (response) {
        console.log("Success");
        console.log(response);
        for (var x = 0; x < response.length; x++) {
            var weekday = new Array(7);
            weekday[0] = "Sun";weekday[1] = "Mon";weekday[2] = "Tue";weekday[3] = "Wed";
            weekday[4] = "Thu";weekday[5] = "Fri";weekday[6] = "Sat";
            $.ajax({
                async: false,
                data: { entryDay: response[x].currentDay, projectID: response[x].Project_projectID, activityID: response[x].Activity_activityID },
                url: '/Home/AddLastWeekPartials'
            }).success(function (partialView) {
                var divID = "#hoursworkedrow" + weekday[response[x].currentDay];
                $(divID).prepend(partialView);
            });
        }
    }).error(function (response) {
        console.log("Failed");
    });
}

