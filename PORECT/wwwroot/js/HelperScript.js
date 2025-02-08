function DataTransaction(param) {
     //debugger;
    var json = JSON.stringify(param.entity);
    $.ajax({
        type: 'POST',
        url: param.param1,
        data: { data: json },
        dataType: 'json',
        beforeSend: function () {
            $("#ajaxLoading").show();
        },
        complete: function () {
            $("#ajaxLoading").hide();
        },
        success: function (data) {
            // debugger;
            if (data.IsSuccess == false) {
                ErrorMessage("Failed", data.Message);
            }
            else {
                if (data.Message == null || data.Message == "") {
                    if (param.param2.toLowerCase() == "delete") {
                        data.Message = "Data deleted successfully.";
                    }
                    else {
                        data.Message = "Data submitted successfully.";
                    }
                }
                SuccessMessage("Success", data.Message);
                setTimeout(function () {
                    if (param.param3 != null && param.param3 != "" && param.param3 != undefined) {
                        window.location.href = param.param3;
                    }
                    else {
                        window.location.reload();
                    }
                }, 2000);
            }
        },
        error: function (data, xhr, ajaxOptions, thrownError) {
            //ErrorMessage("Error", "Failed to save data.", JSON.stringify(data));
            ErrorMessage("Error", "Transaction data failed. Error code: " + data.status + ". Please contact administrator.");
        }
    });
}
function FormDataTransaction(param) {
    //debugger;
    $.ajax({
        type: 'POST',
        url: param.param1,
        data: param.entity,
        processData: false, // Required for FormData
        contentType: false, // Required for FormData
        beforeSend: function () {
            $("#ajaxLoading").show();
        },
        complete: function () {
            $("#ajaxLoading").hide();
        },
        success: function (data) {
            // debugger;
            if (data.IsSuccess == false) {
                ErrorMessage("Failed", data.Message);
            }
            else {
                if (data.Message == null || data.Message == "") {
                    if (param.param2.toLowerCase() == "delete") {
                        data.Message = "Data deleted successfully.";
                    }
                    else {
                        data.Message = "Data submitted successfully.";
                    }
                }
                SuccessMessage("Success", data.Message);
                setTimeout(function () {
                    if (param.param3 != null && param.param3 != "" && param.param3 != undefined) {
                        window.location.href = param.param3;
                    }
                    else {
                        window.location.reload();
                    }
                }, 2000);
            }
        },
        error: function (data, xhr, ajaxOptions, thrownError) {
            //ErrorMessage("Error", "Failed to save data.", JSON.stringify(data));
            ErrorMessage("Error", "Transaction data failed. Error code: " + data.status + ". Please contact administrator.");
        }
    });
}

function Download(param) {
    $.ajax({
        type: 'GET',
        url: param.param1,
        data: param.entity,
        xhrFields: {
            responseType: 'blob'
        },
        beforeSend: function () {
            $("#ajaxLoading").show();
        },
        complete: function () {
            $("#ajaxLoading").hide();
        },
        success: function (blob, status, xhr) {
            /*debugger;*/
            const contentDisposition = xhr.getResponseHeader('Content-Disposition');
            var fileName = null; // Default file name
            if (contentDisposition) {
                const match = contentDisposition.match(/filename="?(.+?)"?$/);
                if (match) {
                    fileName = match[1].split(';')[0].replace("\"", "");
                }
            }
            const url = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.id = 'BlobDownload';
            link.style.display = 'none';
            link.href = url;
            if (fileName != null) {
                link.download = fileName; // Set dynamic file name
            }
            document.body.appendChild(link);
            link.click();
            window.URL.revokeObjectURL(url);
            $("#BlobDownload").remove();
        },
        error: function (data, xhr, ajaxOptions, thrownError) {
            //ErrorMessage("Error", "Failed to save data.", JSON.stringify(data));
            ErrorMessage("Error", "Download failed. Error code: " + data.status + ". Please contact administrator.");
        }
    });
}

//function Download(param) {
//    fetch(param.param1, {
//        method: 'GET'
//    })
//    .then(response => {
//        if (!response.ok) {
//            throw new Error('Network response was not ok');
//        }
//        //debugger;
//        const contentDisposition = response.headers.get('Content-Disposition');
//        var fileName = null;
//        if (contentDisposition) {
//            const match = contentDisposition.match(/filename="?(.+?)"?$/);
//            if (match) {
//                fileName = match[1].split(';')[0].replace("\"", "");
//            }
//        }
//        return response.blob().then(blob => ({ blob, fileName }));
//        //return response.blob();
//    })
//    .then(({ blob, fileName }) => {
//        //debugger;
//        const url = window.URL.createObjectURL(blob);
//        const link = document.createElement('a');
//        link.id = 'BlobDownload';
//        link.style.display = 'none';
//        link.href = url;
//        if (fileName != null) {
//            link.download = fileName; // Set dynamic file name
//        }
//        document.body.appendChild(link);
//        link.click();
//        window.URL.revokeObjectURL(url);
//        $("#BlobDownload").remove();
//    })
//    .catch(error => {
//        //debugger;
//        ErrorMessage("Error", "Failed to download file");
//    });
//}

function OpenPopup(param) {
    //debugger;
    $.ajax({
        url: param.param1,
        data: param.param2,
        //dataType: 'json',
        beforeSend: function () {
            $("#ajaxLoading").show();
        },
        complete: function () {
            $("#ajaxLoading").hide();
        },
        success: function (data) {
             //debugger;
            $("#" + param.param4 + " .modal-body .container-fluid.card").html(data);
            if (param.param3 != null && param.param3 != "" && param.param3 != undefined) {
                $("#" + param.param4 + " .modal-header .modal-title").text(param.param3);
            }
            $("#" + param.param4).modal("show");
        },
        error: function (data, xhr, ajaxOptions, thrownError) {
            //ErrorMessage("Error", "Failed to save data.", JSON.stringify(data));
            ErrorMessage("Error", "Failed to open " + param.param3 + ". Error code: " + data.status + ". Please contact administrator.");
        }
    });
}

function RedirectToAction(param) {
    //debugger;
    window.location.href = param.param1;
}

function ModalDialogDraggable(dialog) {
    var dialogWrapper = $("#" + dialog);
    var title = dialogWrapper.find(".modal-header");

    var dragStartX, dragStartY;

    title.on("mousedown", function (e) {
        dragStartX = e.clientX;
        dragStartY = e.clientY;

        $(document).on("mousemove", dragDialog);
        $(document).on("mouseup", stopDragDialog);
    });

    function dragDialog(e) {
        var offsetX = e.clientX - dragStartX;
        var offsetY = e.clientY - dragStartY;

        var left = parseInt(dialogWrapper.css("left")) || 0;
        var top = parseInt(dialogWrapper.css("top")) || 0;

        dialogWrapper.css("left", (left + offsetX) + "px");
        dialogWrapper.css("top", (top + offsetY) + "px");

        dragStartX = e.clientX;
        dragStartY = e.clientY;
    }
    function stopDragDialog() {
        $(document).off("mousemove", dragDialog);
        $(document).off("mouseup", stopDragDialog);
    }
}

function ConvertMonth(month) {
    var result = "";
    if (parseInt(month) == 1) {
        result = "January";
    }
    else if (parseInt(month) == 2) {
        result = "February";
    }
    else if (parseInt(month) == 3) {
        result = "March";
    }
    else if (parseInt(month) == 4) {
        result = "April";
    }
    else if (parseInt(month) == 5) {
        result = "May";
    }
    else if (parseInt(month) == 6) {
        result = "June";
    }
    else if (parseInt(month) == 7) {
        result = "July";
    }
    else if (parseInt(month) == 8) {
        result = "August";
    }
    else if (parseInt(month) == 9) {
        result = "September";
    }
    else if (parseInt(month) == 10) {
        result = "October";
    }
    else if (parseInt(month) == 11) {
        result = "November";
    }
    else if (parseInt(month) == 12) {
        result = "December";
    }
    return result;
}
