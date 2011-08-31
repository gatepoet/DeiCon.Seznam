/// <reference path="~/Scripts/jquery-1.5.2.js" />
/// <reference path="~/Scripts/dojo.js.uncompressed.js" />
/// <reference path="~/Scripts/json2.js" />
/// <reference path="~/Scripts/seznam.app.js" />
/// <reference path="~/Scripts/seznam.util.js" />
/// <reference path="~/Scripts/seznam.vars.js" />


$(function () {
    $(Views.SharedLists).live('pagebeforeshow', function (event) {
        $(Views.SharedLists + " section[data-role='content'] ul").listview("refresh");
    });
    var sharedListsViewModel = {
        lists: ko.observableArray(),
        shared: ko.observable(false),
        users: ko.observable(),

        listUpdated: function () {
            this.lists.valueHasMutated();
        },
        viewList: function (e) {
            var index = $(e.srcElement).closest("li").index();
            var list = this.lists()[index];
            Util.publish(Events.ViewSharedListDetails, [list]);
        }
    };

    ko.applyBindings(sharedListsViewModel, $(Views.SharedLists)[0]);

    Util.subscribe(Events.UpdateAllData, function (data) {
        if (data.sharedLists) {
            sharedListsViewModel.lists(data.sharedLists);
            sharedListsViewModel.listUpdated();
        }
    });
    Util.subscribe(Events.SharedListCreated, this, function () {
        sharedListsViewModel.listUpdated();
        var currentPageId = "#" + $.mobile.activePage.attr('id');
        if (currentPageId == Views.SharedLists)
            $(Views.SharedLists + " section[data-role=content] ul[data-role=listview]").listview("refresh");
    });
});
