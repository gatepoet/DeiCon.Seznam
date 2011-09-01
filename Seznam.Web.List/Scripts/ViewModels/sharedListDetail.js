$(function () {
    var sharedListDetailViewModel = {
        error: ko.observable(),
        list: ko.observable(),
        items: ko.observableArray(),
        newItemName: ko.observable(""),
        currentItem: ko.observable(),
        count: ko.observable(0),
        addItem: function () {
            var message = { listId: this.list().id, name: this.newItemName(), count: this.count() };
            Util.publish(Events.CreateSharedItem, [message]);
        },
        toggleListItem: function (e) {
            var index = $(e.srcElement).closest("li").index();
            var item = this.items()[index];
            var message = { listId: this.list().id, itemName: item.name, itemCompleted: !item.completed };
            Util.publish(Events.ToggleSharedItem, [message]);
        },
        listUpdated: function () {
            this.items.valueHasMutated();
        },
        refreshList: function () {
            $(Views.SharedListDetail + " section[data-role='content'] ul").listview("refresh");
            this.error(null);
        },
        getTheme: function (complete) {
            return complete ? 'e' : 'b';
        }

    };
    sharedListDetailViewModel.name = ko.dependentObservable(function () {
        if (this.list())
            return this.list().name;
        return "";
    }, sharedListDetailViewModel);


    ko.applyBindings(sharedListDetailViewModel, $(Views.SharedListDetail)[0]);

    Util.subscribe(Events.ViewSharedListDetails, function (list) {
        sharedListDetailViewModel.list(list);
        var items = list.items;
        items.getTheme = sharedListDetailViewModel.getTheme;
        sharedListDetailViewModel.items(list.items);
        $.mobile.changePage(Views.SharedListDetail);
        sharedListDetailViewModel.refreshList();

    });

    Util.subscribe(Events.CreateSharedItemFailed, function (message) {
        sharedListDetailViewModel.error(message);
    });

    Util.subscribe(Events.SharedItemCreated, function () {
        sharedListDetailViewModel.listUpdated();
        sharedListDetailViewModel.refreshList();

    });
    Util.subscribe(Events.SharedItemToggled, function () {
        sharedListDetailViewModel.listUpdated();
        sharedListDetailViewModel.refreshList();
    });
    Util.subscribe(Events.SharedItemDeleted, function () {
        sharedListDetailViewModel.listUpdated();
        sharedListDetailViewModel.refreshList();
    });
});