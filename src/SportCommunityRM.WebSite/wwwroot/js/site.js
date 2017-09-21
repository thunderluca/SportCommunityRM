var defaultDatepickerOptions = {
    maxViewMode: 2,
    autoclose: true,
    todayHighlight: true
};

var nullOrEmptyString = function (s) {
    return _.isNull(s) || _.isUndefined(s) || s.trim() === '';
};
