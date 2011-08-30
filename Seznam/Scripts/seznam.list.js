/// <reference path="jquery-1.5.2.js" />
/// <reference path="dojo.js.uncompressed.js" />
/// <reference path="json2.js" />
/// <reference path="seznam.util.js" />
/// <reference path="seznam.vars.js" />


Seznam = function (options) {
    this.options = $.extend(this.defaults, options);
    this.personalLists = new Array();
    this.sharedLists = new Array();

    //Initial update
    Util.subscribe(Events.UpdateAllData, this, function (data, context) {
        $.extend(context, data);
    });

    //Create list
    Util.subscribe(Events.CreateList, this, function (list, context) {
        if (list.shared)
            list.count = 0;

        Net.getJSON(Url.GetUserIds + "/" + list.users, function (data) {
            if (data && data.ok) {
                list.users = data.message;

                Net.put(JSON.stringify(list), Url.CreateList);
            }
        });


    });
    Util.subscribe(Events.ListCreated, this, function (message, context) {
        context.personalLists.push(message.list);
    });

    Util.subscribe(Events.SharedListCreated, this, function (message, context) {
        context.sharedLists.push(message.list);
    });

    //Create list item
    Util.subscribe(Events.CreateItem, this, function (item, context) {
        Net.put(JSON.stringify(item), Url.CreateItem);
    });
    Util.subscribe(Events.CreateSharedItem, this, function (item, context) {
        Net.put(JSON.stringify(item), Url.CreateSharedItem);
    });
    Util.subscribe(Events.SharedItemCreated, this, function (message, context) {
        var item = message.item;
        for (var i = 0; i < context.sharedLists.length; i++) {
            var list = context.sharedLists[i];
            if (list.id == item.listId) {
                list.items.push(item);
                break;
            }
        }
    });
    Util.subscribe(Events.ItemCreated, this, function (message, context) {
        var item = message.item;
        for (var i = 0; i < context.personalLists.length; i++) {
            var list = context.personalLists[i];
            if (list.id == item.listId) {
                list.items.push(item);
                break;
            }
        }
    });

    // Delete list item
    Util.subscribe(Events.DeleteItem, this, function (item, context) {
        Net.destroy(JSON.stringify(item), Url.DeleteItem, function (message) {
            if (message.ok) {
                Util.publish(Events.ItemDeleted, [$.parseJSON(this.data)]);
            }
            else {
                console.log(data.message);
            }
        });
    });
    Util.subscribe(Events.ItemDeleted, this, function (message, context) {
        var item = message.item;
        var list = context.getList(context.personalLists, item.listId);
        context.removeItem(list.items, item.name);
    });
    Util.subscribe(Events.SharedItemDeleted, this, function (message, context) {
        var item = message.item;
        var list = context.getList(context.sharedLists, item.listId);
        context.removeItem(list.items, item.name);
    });

    this.getList = function (coll, listId) {
        for (var i = 0; i < coll.length; i++) {
            var list = coll[i];
            if (list.id == listId) {
                return list;
            }
        }
        return null;
    };
    this.removeItem = function (list, name) {
        for (var i = 0; i < list.length; i++) {
            var item = list[i];
            if (item.name == name) {
                list.remove(i);
                break;
            }
        }
        return null;
    };

    Util.subscribe(Events.ToggleItem, this, function (message, context) {
        Net.post(JSON.stringify(message), Url.ToggleItem);
    });
    Util.subscribe(Events.ItemToggled, this, function (message, context) {
        var data = message.item;
        var list = context.getList(context.personalLists, data.listId);
        var item = context.getListItem(list.items, data.name);
        item.completed = data.completed;
    });
    this.getListItem = function (list, name) {
        for (var i = 0; i < list.length; i++) {
            var item = list[i];
            if (item.name == name) {
                return item;
            }
        }
        return null;

    };
    this.getList = function (lists, id) {
        for (var i = 0; i < lists.length; i++) {
            var list = lists[i];
            if (list.id == id) {
                return list;
            }
        }
        return null;
    };
    Util.subscribe(Events.ToggleSharedItem, this, function (message, context) {
        Net.post(JSON.stringify(message), Url.ToggleSharedItem);
    });

    Util.subscribe(Events.SharedItemToggled, this, function (message, context) {
        var mi = message.item;
        var list = context.getList(context.sharedLists, mi.listId);
        var item = context.getListItem(list.items, mi.name);
        item.completed = mi.completed;
    });
};