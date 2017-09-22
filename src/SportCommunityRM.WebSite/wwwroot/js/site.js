var defaultDatepickerOptions = {
    maxViewMode: 2,
    autoclose: true,
    todayHighlight: true
};

var nullOrEmptyString = function (s) {
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
