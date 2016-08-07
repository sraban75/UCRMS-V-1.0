
function CloseModal(el, dataAction, dataUrl) {
    if (dataAction == "formsubmit") {
        $("#" + dataUrl).submit();
    }
    else if (dataAction == "refreshparent") {
        window.parent.location = window.parent.location;
    }
    else if (dataAction == "refreshself") {
        window.location = window.location;
    }
    else if (dataAction == "redirect") {
        window.location = dataUrl;
    }
    else if (dataAction == "function") {
        eval(dataUrl);
    }
    var win = $(el).closest(".modal");
    win.modal("hide");
    setTimeout(function () {
        win.next(".modal-backdrop").remove();
        win.remove();
    }, 500);
}
function UYResult(msg, status, dataAction, dataUrl)
{
    var html = '' +
            '<div id="userUpdate" class="modal fade">' +
                '<div class="modal-dialog modal-sm">' +
                    '<div class="modal-content">' +
                        '<div class="modal-header modal-title-' + status + '">' +
                            '<h4 class="modal-title">ABS</h4>' +
                        '</div>' +
                        '<div class="modal-body">' +
                            '<p>' + msg + '</p>' +
                        '</div>' +
                        '<div class="modal-footer">' +
                            '<button type="button" class="btn btn-primary" data-dismiss="modal" onclick="CloseModal(this, \'' + dataAction + '\',\'' + dataUrl + '\')">OK</button>' +
                        '</div>' +
                    '</div>' +
                '</div>' +
            '</div>';

    var dialogWindow = $(html).appendTo('body');
    dialogWindow.modal({ backdrop: 'static' });
}