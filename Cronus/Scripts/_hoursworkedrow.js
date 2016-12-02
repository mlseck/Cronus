function GetActivities(_projectID) {
    var selectedProj = _projectID.value;
    var ddlid = _projectID.id;
    var ddlstring = ddlid.replace("Project_project", "Activity_activity");
    ddlstring = "#" + ddlstring;
    console.log(ddlid);
    console.log(ddlstring);
    var procemessage = "<option value='0'> Please wait...</option>";
    console.log(procemessage);
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
function DeleteRow(_row) {
    console.log("Executing Delete Script")
    var entryID = $(_row).parentNode;
    console.log("Parent Node: " + entryID)
    $(_row).parents("#hoursWorkedRow:first").remove();
    return false;
}