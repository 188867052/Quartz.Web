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
                var data = e.currentTarget.getAttribute("data");
                $.ajaxSettings.async = false;
                var point = this.displayDialog;
                $.get(url, JSON.parse(data), $.proxy(function (response) {
                    $(".modal").remove();
                    $("body").append(response);
                }, this));
            });

            $('.btn-action').on("click", function (e) {
                var url = e.currentTarget.getAttribute("action");
                var data = e.currentTarget.getAttribute("data");
                $.ajaxSettings.async = false;
                var point = this.refresh;
                $.get(url, JSON.parse(data), $.proxy(location.reload(), this));
            });

            this.pageInit();
        },

        pageInit: function () {
            $('.pagination li').on("click", function (e) {
                if (e.currentTarget.className == "active" || isNaN(e.currentTarget.textContent)) { return;}
                var data = JSON.parse(this.parentElement.previousElementSibling.getAttribute('data'));
                data.index = e.currentTarget.textContent;
                data.size = 10;
                var url = this.parentElement.previousElementSibling.getAttribute('url');
                $.get(url, data, $.proxy(function (response) { this.parentElement.previousElementSibling.innerHTML = response; this.parentElement.remove(); framework.pageInit(); }, this));
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