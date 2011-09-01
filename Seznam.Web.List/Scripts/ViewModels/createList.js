$(function () {
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
