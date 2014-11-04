(function ($) {
    var defaultOptions = {
        errorClass: 'has-error',
        validClass: 'has-success',
        highlight: function (element) {
            $(element).closest(".form-group")
                .addClass(defaultOptions.errorClass)
                .removeClass(defaultOptions.validClass);
        },
        unhighlight: function (element) {
            $(element).closest(".form-group")
            .removeClass(defaultOptions.errorClass)
            .addClass(defaultOptions.validClass);
        }
    };

    $.validator.setDefaults(defaultOptions);

    $.validator.unobtrusive.options = {
        errorClass: defaultOptions.errorClass,
        validClass: defaultOptions.validClass,
    };
})(jQuery);