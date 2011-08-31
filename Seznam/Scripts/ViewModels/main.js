/// <reference path="~/Scripts/jquery-1.5.2.js" />
/// <reference path="~/Scripts/dojo.js.uncompressed.js" />
/// <reference path="~/Scripts/json2.js" />
/// <reference path="~/Scripts/seznam.app.js" />
/// <reference path="~/Scripts/seznam.util.js" />
/// <reference path="~/Scripts/seznam.vars.js" />

$(function () {
    var mainViewModel = {

        personalLists: ko.observableArray(),
        sharedLists: ko.observableArray(),
        viewPersonal: function () {
            console.log("personal");
            $.mobile.changePage(Views.PersonalLists);
        },
        viewShared: function () {
            $.mobile.changePage(Views.SharedLists);
        },
        logOut: function () {
            dojo.publish(Events.LogOut);
        },
        personalListsUpdated: function () {
            this.personalLists.valueHasMutated();
        },
        sharedListsUpdated: function () {
            this.sharedLists.valueHasMutated();
        }
    };
    mainViewModel.personalListCount = ko.dependentObservable(function () {
        return this.personalLists().length;
    }, mainViewModel);
    mainViewModel.sharedListCount = ko.dependentObservable(function () {
        return this.sharedLists().length;
    }, mainViewModel);

    ko.applyBindings(mainViewModel, $(Views.Main)[0]);

    Util.subscribe(Events.UpdateAllData, function (data) {
        if (data.personalLists)
            mainViewModel.personalLists(data.personalLists);
        if (data.sharedLists)
            mainViewModel.sharedLists(data.sharedLists);
        $(Views.Main + " section[data-role='content'] ul").listview("refresh");
    });
    Util.subscribe(Events.ListCreated, this, function (list) {
        mainViewModel.personalListsUpdated();
    });
    Util.subscribe(Events.SharedListCreated, this, function () {
        mainViewModel.sharedListsUpdated();
    });

    ko.bindingHandlers.jClick = {
        init: function (element, valueAccessor) {
            $(element).click(function () {
                valueAccessor();
            });
        }
    };
});