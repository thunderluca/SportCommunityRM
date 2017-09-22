var defaultDatepickerOptions = {
    maxViewMode: 2,
    autoclose: true,
    todayHighlight: true
};

function nullOrEmptyString(s) {
    return _.isNull(s) || _.isUndefined(s) || s.trim() === '';
};

function executeHttpPostRequest(url, data, successCallback, errorCallback) {
    if (!_.isString(data))
        data = JSON.stringify(data);

    $.ajax({
        url: url,
        type: 'POST',
        contentType: 'application/json',
        data: data,
        processData: false,
        success: successCallback,
        error: errorCallback
    });
};

function executeHttpGetRequest(url, data, successCallback, errorCallback) {
    $.ajax({
        url: url,
        type: 'GET',
        data: data,
        success: successCallback,
        error: errorCallback
    });
};

function showConfirm(message, confirmLabel, cancelLabel, callback) {
    bootbox.confirm({
        message: message,
        buttons: {
            confirm: {
                label: confirmLabel,
                className: 'btn-success'
            },
            cancel: {
                label: cancelLabel,
                className: 'btn-danger'
            }
        },
        callback: callback
    });
};