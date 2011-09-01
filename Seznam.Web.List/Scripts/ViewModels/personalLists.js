$(function () {
    $(Views.PersonalLists).live('pagebeforeshow', function (event) {
        $(Views.PersonalLists + " section[data-role='content'] ul").listview("refresh");
    });

    var personalListsViewModel = {
        lists: ko.observableArray(),
        shared: ko.observable(false),
        users: ko.observable(),

        addList: function () {
            $.mobile.changePage(Views.CreateList, { transition: "slideup" });
        },
        listUpdated: function () {
            this.lists.valueHasMutated();
        },
        viewList: function (e) {
            var index = $(e.srcElement).closest("li").index();
            var list = this.lists()[index];
            Util.publish(Events.ViewListDetails, [list]);
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

    ko.applyBindings(personalListsViewModel, $(Views.PersonalLists)[0]);

    Util.subscribe(Events.UpdateAllData, function (data) {
        if (data.personalLists) {
            personalListsViewModel.lists(data.personalLists);
            personalListsViewModel.listUpdated();
        }
    });

    Util.subscribe(Events.ListCreated, this, function () {
        personalListsViewModel.listUpdated();
        var activeId = "#" + $.mobile.activePage.attr("id");
        if (activeId == Views.CreateList)
            $.mobile.changePage(Views.PersonalLists, { transition: "slidedown" });
        else if (activeId == Views.PersonalLists)
            $(Views.PersonalLists + " section[data-role='content'] ul").listview("refresh");
        else
        { }
    });

    Util.subscribe(Events.ItemCreated, function (data) {
        personalListsViewModel.listUpdated();
        var activeId = "#" + $.mobile.activePage.attr("id");
        if (activeId == Views.PersonalLists)
            $(Views.PersonalLists + " section[data-role='content'] ul").listview("refresh");
    });
    Util.subscribe(Events.ItemDeleted, function (data) {
        personalListsViewModel.listUpdated();
        var activeId = "#" + $.mobile.activePage.attr("id");
        if (activeId == Views.PersonalLists)
            $(Views.PersonalLists + " section[data-role='content'] ul").listview("refresh");
    });
    Util.subscribe(Events.ItemToggled, function (data) {
        personalListsViewModel.listUpdated();
        var activeId = "#" + $.mobile.activePage.attr("id");
        if (activeId == Views.PersonalLists)
            $(Views.PersonalLists + " section[data-role='content'] ul").listview("refresh");
    });
});
