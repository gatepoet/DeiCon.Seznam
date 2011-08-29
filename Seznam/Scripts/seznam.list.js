/// <reference path="jquery-1.5.2.js" />
/// <reference path="dojo.js.uncompressed.js" />
/// <reference path="json2.js" />
/// <reference path="Util.js" />



var seznam;
$(function() {
    seznam = new Seznam();
});
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
        //context.personalLists.push(list);

        Net.put(JSON.stringify(list), Url.CreateList, function (data) {
            if (data) {
                Util.publish(Events.ListCreated, [data]);
            }
        });
    });
    Util.subscribe(Events.ListCreated, this, function (list, context) {
        context.personalLists.push(list);
    });

    //Create list item
    Util.subscribe(Events.CreatePersonalListItem, this, function (item, context) {
        Net.put(JSON.stringify(item), Url.CreatePersonalListItem, function (message) {
            if (message) {
                if (message.ok)
                    Util.publish(Events.PersonalListItemCreated, [message.data]);
                else {
                    Util.publish(Events.CreatePersonalListItemFailed, [message.message]);

                }
            }
        });
    });
    Util.subscribe(Events.PersonalListItemCreated, this, function (message, context) {
        for (var i = 0; i < context.personalLists.length; i++) {
            var list = context.personalLists[i];
            if (list.id == message.listId) {
                list.items.push(message);
                break;
            }
        }
    });

    // Delete list item
    Util.subscribe(Events.DeletePersonalListItem, this, function (item, context) {
        Net.destroy(JSON.stringify(item), Url.DeletePersonalListItem, function (message) {
            if (message.ok) {
                Util.publish(Events.PersonalListItemDeleted, [$.parseJSON(this.data)]);
            }
            else {
                console.log(data.message);
            }
        });
    });
    Util.subscribe(Events.PersonalListItemDeleted, this, function (message, context) {
        var list = context.getList(context.personalLists, message.listId);
        context.removeItem(list.items, message.name);
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

    Util.subscribe(Events.TogglePersonalListItem, this, function (message, context) {
        Net.post(JSON.stringify(message), Url.TogglePersonalListItem, function (message) {
            if (message.ok) {
                Util.publish(Events.PersonalListItemToggled, [message.data]);
            }
        });
    });
    Util.subscribe(Events.PersonalListItemToggled, this, function (message, context) {
        var list = context.getPersonalList(context.personalLists, message.listId);
        var item = context.getListItem(list.items, message.name);
        item.completed = message.completed;
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
    this.getPersonalList = function (lists, id) {
        for (var i = 0; i < lists.length; i++) {
            var list = lists[i];
            if (list.id == id) {
                return list;
            }
        }
        return null;
    };
};
