$(function() {
    var personalListDetailViewModel = {
        error: ko.observable(),
        list: ko.observable(),
        items: ko.observableArray(),
        newItemName: ko.observable(""),
        currentItem: ko.observable(),
        count: ko.observable(0),
        addItem: function () {
            var message = { listId: this.list().id, name: this.newItemName(), count: this.count() };
            Util.publish(Events.CreateItem, [message]);
        },
        toggleListItem: function (e) {
            var index = $(e.srcElement).closest("li").index();
            var item = this.items()[index];
            var message = { listId: this.list().id, itemName: item.name, itemCompleted: !item.completed };
            Util.publish(Events.ToggleItem, [message]);
        },
        deleteItem: function (e) {
            var index = $(e.srcElement).closest("li").index();
            var item = this.items()[index];
            Util.publish(Events.DeleteItem, [item]);
        },
        listUpdated: function () {
            this.items.valueHasMutated();
        },
        refreshList: function () {
            $(Views.PersonalListDetail + " section[data-role='content'] ul").listview("refresh");
            this.error(null);
        },
        getTheme: function (complete) {
            return complete ? 'e' : 'b';
        }

    };
    personalListDetailViewModel.name = ko.dependentObservable(function () {
        if (this.list())
            return this.list().name;
        return "";
    }, personalListDetailViewModel);

    ko.applyBindings(personalListDetailViewModel, $(Views.PersonalListDetail)[0]);
    
    Util.subscribe(Events.ViewListDetails, function (list) {
        console.log(JSON.stringify(list));
        personalListDetailViewModel.list(list);
        var items = list.items;
        items.getTheme = personalListDetailViewModel.getTheme;
        personalListDetailViewModel.items(list.items);
        $.mobile.changePage(Views.PersonalListDetail);
        personalListDetailViewModel.refreshList();

    });

    Util.subscribe(Events.CreateItemFailed, function (message) {
        personalListDetailViewModel.error(message);
    });

    Util.subscribe(Events.ItemCreated, function () {
        personalListDetailViewModel.listUpdated();
        personalListDetailViewModel.refreshList();

    });
    Util.subscribe(Events.ItemToggled, function () {
        personalListDetailViewModel.listUpdated();
        personalListDetailViewModel.refreshList();
    });
    Util.subscribe(Events.ItemDeleted, function () {
        personalListDetailViewModel.listUpdated();
        personalListDetailViewModel.refreshList();
    });
});
