/// <reference path="~/Scripts/jquery-1.5.2.js" />
/// <reference path="~/Scripts/dojo.js.uncompressed.js" />
/// <reference path="~/Scripts/json2.js" />
/// <reference path="~/Scripts/seznam.app.js" />
/// <reference path="~/Scripts/seznam.util.js" />
/// <reference path="~/Scripts/seznam.vars.js" />


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
            console.log("View list ", list.name, " (", list.id, ") ");
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
        if (activeId==Views.CreateList)
            $.mobile.changePage(Views.PersonalLists, { transition: "slidedown" });
        else if (activeId == Views.PersonalLists)
            $(Views.PersonalLists + " section[data-role='content'] ul").listview("refresh");
        else
        {}
    });
});
