function GetActivities(_projectID) {
    var selectedProj = _projectID.value;
    var ddlid = _projectID.id;
    var ddlstring = "#actddl" + ddlid;
    var procemessage = "<option value='0'> Please wait...</option>";
    $(ddlstring).html(procemessage).show();

    var url = "/Home/FillActivities";

    $.ajax({
        url: url,
        data: { projectID: selectedProj },
        cache: false,
        type: "POST",
        success: function (data) {
            var markup = "<option value='0'>Select Activity</option>";
            for (var x = 0; x < data.length; x++) {
                markup += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
            }

            $(ddlstring).html(markup).show();

        }
        ,
        error: function (reponse) {
            alert("error : " + reponse);
        }
    });

}