Seznam = function (options) {
    this.options = $.extend(this.defaults, options);
    this.personalLists = new Array();
    this.sharedLists = new Array();

    //Initial update
    Util.subscribe(Events.UpdateAllData, this, function (data, context) {
        $.extend(context, data);
    });

    //Create list
    Util.subscribe(Events.CreateList, function (list) {
        var url = Url.GetUserIds + "/" + list.users;
        Net.getJSON(url, function (data) {
            if (data && data.ok) {
                list.users = data.message;
                Net.put(JSON.stringify(list), Url.CreateList);
            }
        });
    });
    Util.subscribe(Events.CreateItem, function (item)           { Net.put(JSON.stringify(item), Url.CreateItem); });
    Util.subscribe(Events.CreateSharedItem, function (item)     { Net.put(JSON.stringify(item), Url.CreateSharedItem); });
    Util.subscribe(Events.DeleteItem, function (item)           { Net.destroy(JSON.stringify(item), Url.DeleteItem); });
    Util.subscribe(Events.ToggleItem, function (message)        { Net.post(JSON.stringify(message), Url.ToggleItem); });
    Util.subscribe(Events.ToggleSharedItem, function (message)  { Net.post(JSON.stringify(message), Url.ToggleSharedItem); });
        
    Util.subscribe(Events.ListCreated, this, function (message, context)        { context.personalLists.push(message.list); });
    Util.subscribe(Events.ItemCreated, this, function (message, context)        { context.createItem(context.personalLists, message.item); });
    Util.subscribe(Events.ItemDeleted, this, function (message, context)        { context.deleteItem(context.personalLists, message.item); });
    Util.subscribe(Events.ItemToggled, this, function (message, context)        { context.toggle(context.personalLists, message.item); });

    Util.subscribe(Events.SharedListCreated, this, function (message, context)  { context.sharedLists.push(message.list); });
    Util.subscribe(Events.SharedItemCreated, this, function (message, context)  { context.createItem(context.sharedLists, message.item); });
    Util.subscribe(Events.SharedItemDeleted, this, function (message, context)  { context.deleteItem(context.sharedLists, message.item); });
    Util.subscribe(Events.SharedItemToggled, this, function (message, context)  { context.toggle(context.sharedLists, message.item); });

    this.toggle = function (coll, data) {
        var list = this.getList(coll, data.listId);
        var item = this.getItem(list.items, data.name);
        item.completed = data.completed;
    };
    
    this.getList = function (coll, listId) {
        for (var i = 0; i < coll.length; i++) {
            var list = coll[i];
            if (list.id == listId) {
                return list;
            }
        }
        return null;
    };
    
    this.getItem = function (list, name) {
        for (var i = 0; i < list.length; i++) {
            var item = list[i];
            if (item.name == name) {
                return item;
            }
        }
        return null;

    };
    
    this.getItemIndex = function (list, name) {
        for (var i = 0; i < list.length; i++) {
            var item = list[i];
            if (item.name == name) {
                return i;
            }
        }
        return -1;
    };
    
    this.createItem = function (coll, item) {
        var list = this.getList(coll, item.listId);
        list.items.push(item);
    };
    
    this.deleteItem = function (coll, item) {
        var list = this.getList(coll, item.listId);
        var i = this.getItemIndex(list.items, item.name);
        list.items.remove(i);
    };
};