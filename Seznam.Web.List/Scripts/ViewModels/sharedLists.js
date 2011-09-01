$(function () {
    $(Views.SharedLists).live('pagebeforeshow', function () {
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
        },
        getTheme: function (list) {
            var containsIncomplete = list.items.length == 0;
            for (var i = 0; i < list.items.length; i++) {
                if (!list.items[i].completed)
                    containsIncomplete = true;
            }
            return containsIncomplete ? 'a' : 'e';
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
    Util.subscribe(Events.SharedItemCreated, function (data) {
        sharedListsViewModel.listUpdated();
        var activeId = "#" + $.mobile.activePage.attr("id");
        if (activeId == Views.SharedLists)
            $(Views.SharedLists + " section[data-role='content'] ul").listview("refresh");
    });
    Util.subscribe(Events.SharedItemDeleted, function (data) {
        sharedListsViewModel.listUpdated();
        var activeId = "#" + $.mobile.activePage.attr("id");
        if (activeId == Views.SharedLists)
            $(Views.SharedLists + " section[data-role='content'] ul").listview("refresh");
    });
    Util.subscribe(Events.SharedItemToggled, function (data) {
        sharedListsViewModel.listUpdated();
        var activeId = "#" + $.mobile.activePage.attr("id");
        console.log(activeId + " " + Views.SharedLists);
        if (activeId == Views.SharedLists)
            $(Views.SharedLists + " section[data-role='content'] ul").listview("refresh");
    });
});
