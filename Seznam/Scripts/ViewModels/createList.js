/// <reference path="~/Scripts/jquery-1.5.2.js" />
/// <reference path="~/Scripts/dojo.js.uncompressed.js" />
/// <reference path="~/Scripts/json2.js" />
/// <reference path="~/Scripts/seznam.app.js" />
/// <reference path="~/Scripts/seznam.util.js" />
/// <reference path="~/Scripts/seznam.vars.js" />

$(Views.CreateList).live('pagecreate', function (event) {
    var createListViewModel = {
        name: ko.observable(),
        shared: ko.observable(false),
        users: ko.observable(),

        create: function () {
            var msg = {
                name: this.name(),
                shared: this.shared()
            };
            if (this.shared())
                msg.users = this.users();
            Util.publish(Events.CreateList, [msg]);
        },
        update: function () {
            $(Views.CreateList).trigger("refresh");
        }
    };
    createListViewModel.allowCreate = ko.dependentObservable(function () {
        return this.name();
    }, createListViewModel);

    ko.applyBindings(createListViewModel, $(Views.CreateList)[0]);
});
