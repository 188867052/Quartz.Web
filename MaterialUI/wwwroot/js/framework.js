(function () {
    'use strict';

    window.Framework = function () {
        this._currentPage = 1;
    };

    window.Framework.prototype = {

        // Private Fields
        _currentPage: null,
        _pageSize: null,
        _successPointer: null,
        _leftText: null,
        _rightText: null,

        initialize: function () {
            // TODO
            $('button[data-toggle="modal"]').on("click", function (e) {
                var url = e.currentTarget.getAttribute("action");
                var id = e.currentTarget.getAttribute("id");
                $.ajaxSettings.async = false;
                var point = this.displayDialog;
                $.get(url, { id }, $.proxy(function (response) {
                    $(".modal").remove();
                    $("body").append(response);
                }, this));
            });

            $('.btn-action').on("click", function (e) {
                var url = e.currentTarget.getAttribute("action");
                var id = e.currentTarget.getAttribute("id");
                $.ajaxSettings.async = false;
                var point = this.refresh;
                $.get(url, { id }, $.proxy(location.reload(), this));
            });
        },

        refresh: function () {
            location.reload();
        },

        displayDialog: function (response) {
            $(".modal").remove();
            $("body").append(response);
        },

        getData: function (form) {

            this.checkNotNull(form);

            var data = new Object;
            for (var i = 0; i < form.elements.length; i++) {
                var element = form.elements[i];
                var name = element.getAttribute("name");
                data[name] = element.value;
                if (element.type == 'checkbox') {
                    data[name] = element.type == 'checkbox' ? element.checked : element.value;
                }
            }
            if (event != undefined) {
                data[event.currentTarget.getAttribute('name')] = event.currentTarget.getAttribute('value');
            }
            return data;
        },

        checkNotNull: function (object) {
            if (object == null || object == undefined) {
                alert("value is null or undefined!");
            }
        },


    };
})();