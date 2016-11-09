
$(document).ready(function () {
    $(function () {
        $.ajax({
            type: "POST",
            contentType: "application/json",
            data: "{}",
            url: "/Home/GetEvents",
            dataType: "json",


            //On Date Clicked
            success: function (data) {
                //alert(data);
                $('#calendar').fullCalendar({
                    dayClick: function (date, allDay, jsEvent, view) {

                        if (allDay) {
                            $.ajax({
                                contentType: "application/json",
                                data: "{}",
                                url: "/Home/GetHoursWorkedPerDay/",
                                dataType: "json",
                                success: function (data) {
                                    console.log(data)
                                    console.log("you clicked on the date " + date.format())                              

                                    //console.log(data.ActivityName)


                                    $('#modalTitle').html(data.ActivityName);
                                    //$('#modalBody').html(data.HrsWorked);
                                    $('#fullCalModal').modal();
                                },
                                error: function () {

                                }
                            });
                        }
                    },

                //Getting events
                    header: {
                        left: 'prev today',
                        center: 'title',
                        right: 'next'
                        //right: 'month,agendaWeek,agendaDay'
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


//$(document).ready(function () {
//    $('#bootstrapModalFullCalendar').fullCalendar({
//        events: '/hackyjson/cal/',
//        header: {
//            left: '',
//            center: 'prev title next',
//            right: ''
//        },
//        eventClick: function (event, jsEvent, view) {
//            $('#modalTitle').html(event.title);
//            $('#modalBody').html(event.description);
//            $('#eventUrl').attr('href', event.url);
//            $('#fullCalModal').modal();
//        }
//    });
//});

