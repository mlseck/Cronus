var weeklyHrs = 0;
var weekCount = 0;
var count = 0;
$('.fc-button-prev span').click(function () {
    dayRender();
});

$('.fc-button-next span').click(function () {
    dayRender();
});

$(document).ready(function () {

    $(function () {
        $.ajax({
            type: "POST",
            contentType: "application/json",
            data: { empId: $("#empID").val() },
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
                                data: { date: date.format() , empId: $("#empID").val() },
                                url: "/Home/GetHoursWorkedPerDay/",
                                dataType: "json",
                                success: function (data) {
                                    //will fill with content if no hours logged
                                    $('#modalTitle').html("Hours Worked on " + date.format("dddd") + ", " + date.format("MMMM") + " " + date.format('D'))
                                    $('#modalBody').html("You worked no hours on this date.")
                                    $('#fullCalModal').modal()

                                    //will overwrite if hours logged
                                    $('#modalTitle').html("Hours Worked on " + date.format("dddd") + ", " + date.format("MMMM") + " " + date.format("D"))


                                    var newLine = "\n"
                                    var HrsWrkd = ""
                                    //add if adminstrative user to edit hours

                                    //var link = document.createElement('a');
                                    //link.innerHTML = "Edit Activity";


                                    $.each(data, function (index, element) {
                                        //link.setAttribute('href', '/');
                                        HrsWrkd += "You worked " + element.HrsWorked + " Hour(s) on " + element.ActivityName + " for project: " + element.ProjectName + "<br />"
                                        $('#modalBody').html(HrsWrkd)
                                    });


                                    $('#fullCalModal').modal()

                                },
                                error: function () {

                                }
                            });
                        }
                    },

                    //Add function for project hover??
                    //eventRender: function (event, element) {
                    //    //add if statemenet for only administrative users
                    //    $(element).tooltip({ title: "Click to edit project." });
                    //},


                    //adds hours on each day cell for each hours worked on each day. 
                    dayRender: function (date, cell) {
                        var hrsWrkd = ""
                        //weeklyHours(0, date)
                        //dont know if this will work, waiting on DB to be fixed
                        $.ajax({
                            contentType: "application/json",
                            data: { date: date.format() , empId: $("#empID").val() },
                            url: "/Home/GetHoursWorkedPerDay/",
                            dataType: "json",
                            success: function (data) {

                                if (data.length == 0) {
                                    weeklyHours(parseInt(0), date)
                                }

                                else {

                                    $.each(data, function (index, element) {
                                        //link.setAttribute('href', '/');
                                        hrsWrkd += element.HrsWorked
                                        weeklyHours(parseInt(element.HrsWorked), date)
                                        cell.append("<br />" + "<br />" + "<br />" + hrsWrkd + "hour(s)")
                                    });

                                }

                               
                            },
                            error: function () {

                            }
                        });
                    },


                    //Click event
                    eventClick: function (event, jsEvent, view) {
                        $('#modalTitle').html(event.title)
                        $('#modalBody').html(
                            event.title + " starts " + event.start.format("dddd") + ", " + event.start.format("MMMM") + " " + event.start.format('D')
                            + " and ends "
                            + event.end.format("dddd") + ", " + event.end.format("MMMM") + " " + event.end.format('D')
                            );
                        $('#fullCalModal').modal()


                        //console.log(event.id)
                        //window.location = "/Project/Edit/" + event.id;
                    },


                    //Getting events

                    //adding buttong to go to weekly
                    customButtons: {
                        WeekButton: {
                            text: 'Go to Weekly view',
                            click: function () {
                                window.location.href = '/Home/';
                                return false;
                            }
                        }
                    },


                    header: {
                        left: 'prev today',
                        center: 'title',
                        right: 'WeekButton, next'
                        //right: 'month,agendaWeek,agendaDay'
                    },

                    defaultView: 'month',
                    editable: true,
                    height: 650,
                    events: $.map(data, function (item, i) {
                        var event = new Object();
                        event.id = item.projectID;
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




function weeklyHours(hrs, date) {
    var dateDay = date.format("dddd");
    weeklyHrs = weeklyHrs + hrs
    count++

    if (count == 7) {
        $('#week' + weekCount).html(weeklyHrs + " hours logged this week. ")
        count = 0;
        weeklyHrs = 0
        weekCount ++
    }
}
