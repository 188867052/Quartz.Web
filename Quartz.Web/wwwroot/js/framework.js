(function () {
    'use strict';

    window.Framework = function () {
    };

    window.Framework.prototype = {

        // Private Fields

        initialize: function () {
            this.btnModalInit();
            this.btnActionInit();
            this.pageInit();
        },

        pageInit: function () {
            $('.pagination li').on("click", $.proxy(this._onPageInitEvent, this));
        },

        btnActionInit: function () {
            $('.btn-action').on("click", $.proxy(this._onBtnActionEvent, this));
        },

        btnModalInit: function () {
            $('button[data-toggle="modal"]').on("click", $.proxy(this._onBtnModalEvent, this));
        },

        _onBtnModalEvent: function (e) {
            this._onBtnEvent(e, $.proxy(this.displayDialog, this));
        },

        _onBtnActionEvent: function (e) {
            this._onBtnEvent(e, $.proxy(this.refresh, this));
        },

        _onBtnEvent: function (e, pointer) {
            var url = e.currentTarget.getAttribute("action");
            var data = e.currentTarget.getAttribute("data");
            $.ajaxSettings.async = false;
            $.get(url, JSON.parse(data), pointer);
        },

        _onPageInitEvent: function (e) {
            var currentNode = e.currentTarget;
            var tableNode = currentNode.parentElement.previousElementSibling;
            if (currentNode.className == "active" || isNaN(currentNode.textContent)) { return; }
            var data = JSON.parse(tableNode.getAttribute('data'));
            data.index = e.currentTarget.textContent;
            data.size = 10;
            var url = tableNode.getAttribute('url');
            $.get(url, data, $.proxy(this._onPageInit, this, e));
        },

        _onPageInit: function (e, response) {
            var html = $.parseHTML(response);
            var pageNode = e.currentTarget.parentElement;
            pageNode.previousElementSibling.replaceWith(html[0])
            pageNode.replaceWith(html[1])
            this.btnModalInit();
            this.btnActionInit();
            this.pageInit();
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