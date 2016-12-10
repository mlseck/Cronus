
//$(document).ready(function () {
//    $(".hoursInput").change(function () {
//        var sum = 0;
//        $(".hoursInput").each(function () {
//            sum += parseInt($(this).val()) || 0;
//        });
//        $('#totalHours').text(sum + " Total Hours");
//    });
//});

function AddRow(_dayOfRow, _entryday) {
    $.ajax({
        async: false,
        data: { entryDay: _entryday },
        url: '/Home/AddHourWorked'
    }).success(function (partialView) {
        var divID = "#hoursworkedrow" + _dayOfRow.id.slice(-3);
        console.log(divID);
        $(divID).append(partialView);
    })
}

//$(document).ready(function () {
//    $('#save').click(function () {
//        alert("HERE");
//    });
//});

function disableDiv() {
    $("#EditHoursWorked :input").attr("readonly", "readonly");
    //$("#EditHoursWorked :input").attr("disabled", true);
}

function CopyHours(){
    var _currentWeek = new Date($('#currentWeekEndDate').val());
    //var _currentWeek = date.getFullYear() + "-" + date.getMonth() + "-" + date.getDate() + " 00:00:00";
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
            var _entryDay = response[x].CurrentDay;
            console.log(_entryDay);
            $.ajax({
                async: false,
                data: { entryDay: _entryDay},
                url: '/Home/AddHourWorked'
            }).success(function (partialView) {
                console.log("Success");
                var divID = "#hoursworkedrowMon";
                $(divID).append(partialView);
            })
        }
    }).error(function (response) {
        console.log("Failed");
    })
}