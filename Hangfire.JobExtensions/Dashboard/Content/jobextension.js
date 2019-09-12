

function onPost() {
    debugger;

    //element.disabled = true;

    $.ajax({
        async: true,
        cache: false,
        //timeout: 10000,
        url: "JobConfiguration/post",
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        //data: data,
        type: "get",
        success: function (r) {
            debugger;
            //var jobLink = jobLinkBaseUrl + r;
            //var alert = createAlert("alert-success", "Mission launched with id: <a href=\"" + jobLink + "\"><strong>" + r + "</strong></a>");
            //applyAlert(element, alertsElementId, alert);
        },
        error: function (r) {
            debugger;
            //var alert = createAlert("alert-danger", "An error occured during launching: <br/><strong>" + r.responseText + "</strong>");
            //applyAlert(element, alertsElementId, alert);
        }
    });
}