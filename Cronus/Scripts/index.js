
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

$(document).ready(function () {
    var emp_id='Amill';
    $.ajax({
        type: 'GET',
        url: '/Home/GetBetweenDates',
        data: { employeeID: emp_id },
        success: function (data) {
            console.log(data);
        },
        statusCode: {
            404: function (content) { alert('cannot find resource'); },
            500: function (content) { alert('internal server error'); }
        },
        error: function (req, status, errorObj) {
        }
    });
});

function AddRow(_day) {
    console.log("Executing Add Script")
    $.ajax({
        async: false,
        url: '/Home/AddHourWorked'
    }).success(function (partialView) {
        var divID = "#hoursworkedrow" + _day.id.slice(-3);
        console.log(divID);
        $(divID).append(partialView);
    });
}

$(document).ready(function () {
    $('#save').click(function () {
        alert("HERE");
    });
});
