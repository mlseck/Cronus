"use strict";

function addNewRows(table) {
    var t = table.toString();
    var x = document.getElementById(t);
    var new_row = x.rows[1].cloneNode(true);
    var len = x.rows.length;
    len = $('.tr').id + 1;
    new_row.cells[0].innerHTML.id = len;
    x = x.firstElementChild;
    x.appendChild(new_row);

    return;
}

function removeRow(table, row) {
    var t = table.toString();
    var i = row.parentNode.parentNode.rowIndex;
    if (i !== 1) {
        document.getElementById(t).deleteRow(i);
        console.log(table.text);
    }
    return;
}

$(document).ready(function () {
    $(".hoursInput").change(function () {
        var sum = 0;
        $(".hoursInput").each(function () {
            sum += parseInt($(this).val()) || 0;
        });
        $('#totalHours').text(sum + " Total Hours");
    });
});

function AddRow(_dayOfRow, _entryday) {
    console.log("Executing Add Script");
    $.ajax({
        async: false,
        data: { entryDay: _entryday },
        url: '/Home/AddHourWorked'
    }).success(function (partialView) {
        var divID = "#hoursworkedrow" + _dayOfRow.id.slice(-3);
        console.log(divID);
        $(divID).append(partialView);
    });
}

$(document).ready(function () {
    $('#save').click(function () {
        alert("HERE");
    });
});

//function getPreviousWeek(_day, _month, _year) {
//    var _currentWeek = _year + "-" + _month + "-" + _day + " 00:00:00"
//    console.log(_currentWeek)
//    $.ajax({
//        contentType: "application/json",
//        type: 'POST',
//        data: JSON.stringify({
//            currentWeek: _currentWeek
//        }),
//        url: '/Home/Index/',
//        success: function (response){
//            console.log("Successfully fetched hours")
//        },
//        error: function (response) {
//            console.log("Failed")
//        }
//    });
//}

function getPreviousWeek() {
    $.ajax({
        contentType: "application/json",
        type: 'POST',
        data: $('form').serialize(),
        url: '/Home/PreviousWeek/'
    }).success(function (response) {
        console.log("Successfully fetched hours");
    }).error(function (response) {
        console.log("Failed");
    });
}

function disableDiv() {
    $("#EditHoursWorked :input").attr("disabled", true);
}

