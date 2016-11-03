$(document).ready(function () {
    $(function () {
        $.ajax({
            type: "POST",
            contentType: "application/json",
            data: "{}",
            url: "/Home/GetEvents",
            dataType: "json",
            success: function (data) {
                alert(data);
                $('#calendar').fullCalendar({
                    header: {
                        left: 'prev,next today',
                        center: 'title',
                        right: 'month,agendaWeek,agendaDay'
                    },

                    defaultView: 'month',
                    editable: true,
                    height: 650,
                    events: $.map(data, function (item, i) {
                        var event = new Object();
                        event.title = item.projectName;
                        event.start = moment(item.projectStartDate).utc();
                        event.end = moment(item.projectEndDate).utc();
                        return event;
                    })
                });

            },
            error: function () {

            }
        });


    })
});


$(document).ready(function () {
    $.ajax({
        contentType: "application/json",
        data: "{}",
        url: "/Home/GetHoursWorkedPerDay/",
        dataType: "json",
        success: function (data) {
            console.log(data);
        },
        error: function () {

        }
    });
});