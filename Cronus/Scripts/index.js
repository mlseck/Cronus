$(document).load(function () {
    $('#dateTitle').text("View for the Week of" + new Date(new Date().getTime()).toLocaleDateString());
    alert('HERE');
});


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
    $.ajax({
        url: '/Home/GetBetweenDates',
        data: JSON.stringify({
            invoker: 'Amill',
            affected: null
        }),
        //Need to pass through a variable
        success: function (data) {
            console.log(data);
        },
        statusCode: {
            404: function (content) { alert('cannot find resource'); },
            500: function (content) { alert('internal server error'); }
        },
        error: function (req, status, errorObj) {
            // handle status === "timeout"
            // handle other errors
        }
    });
});

$(document).ready(function () {
    $('#save').click(function () {
        alert("HERE");
    });
});
