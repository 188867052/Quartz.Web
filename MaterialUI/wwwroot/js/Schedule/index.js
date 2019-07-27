(function () {
    'use strict';

    window.Index = function () {
    };

    window.Index.prototype = {

        initialize: function () {
        },

        // Public Properties

        // Public Methods

        delete: function () {
            var url = event.currentTarget.getAttribute("action");
            var data = event.currentTarget.getAttribute("data");
            $.ajaxSettings.async = false;
            $.get(url, JSON.parse(data), $.proxy(this._onDelete, this));
        },

        search: function () {
            var url = event.currentTarget.getAttribute("action");
            var form = $('form')[0];

            var data = framework.getData(form);
            $.post(url, data, $.proxy(this._onSearch, this));
        },

        submit: function (e) {
            var form = $(".modal.fade.in form")[0];
            var data = framework.getData(form);
            $.post(event.currentTarget.getAttribute("url"), data, $.proxy(this._onSubmit, this));
        },

        changeTriggerType: function () {
            var url = event.currentTarget.getAttribute("url");
            $.ajaxSettings.async = true;
            $.get(url, { type: event.currentTarget.value}, $.proxy(this._onChangeTriggerType, this));
        },

        _onDelete: function (response) {
            $("#smallAlertModal").modal("hide");
            location.reload()
        },

        _onChangeTriggerType: function (response) {
            $(".replace").each(function (index) {
                if (index == 0) {
                    $(this).replaceWith(response);
                } else {
                    $(this).remove();
                }
            });
        },

        _onSubmit: function (response) {
            location.reload()
        },

        _onSearch: function (response) {
            $(".table-responsive").replaceWith(response);
        },
    };
})();