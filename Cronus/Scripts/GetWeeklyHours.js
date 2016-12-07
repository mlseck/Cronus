var empID = ''
var count = 0;





$(document).ready(function () {
    empID = $("#empId").html()
    getWeeklyHours();
})


function getWeeklyHours() {
    $.ajax({
        contentType: "application/json",
        data: { empId: empID, monthOffSet:count },
        url: "/Home/GetWeeklyHours/",
        dataType: "json",
        success: function (data) {
            var hrsWrkd = 0
            var count = 0
            var weekCount = 1
            $.each(data, function (index, element) {
                hrsWrkd = hrsWrkd + parseInt(element.HrsWorked)
                count = count + 1;
                var str = element.entryDate;
                var num = parseInt(str.replace(/[^0-9]/g, ""));
                var currentDate = new Date(num);
                var weekday = new Array(7);
                weekday[0] = "Sunday";
                weekday[1] = "Monday";
                weekday[2] = "Tuesday";
                weekday[3] = "Wednesday";
                weekday[4] = "Thursday";
                weekday[5] = "Friday";
                weekday[6] = "Saturday";

                var getDay = weekday[currentDate.getDay()];
                if (getDay == "Saturday") {
                    $('#week' + weekCount).html(hrsWrkd + " hours.")
                    weekCount++
                    hrsWrkd = 0
                    count = 0
                }
                else if (element.isLastDay) {
                    $('#week' + weekCount).html(hrsWrkd + " hours.")
                    weekCount++
                    hrsWrkd = 0
                    count = 0
                }
            });
        },
        error: function () {

        }
    });
}
