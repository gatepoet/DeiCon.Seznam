/// <reference path="jquery-1.5.js" />
/// <reference path="dojo.js.uncompressed.js" />
/// <reference path="json2.js" />

//Seznam = function (id, personalListId, personalviewId) {
//    this.id = id;
//    this.personalLists = new PersonalListCollection(personalListId, personalviewId);
//    var iii = id;
//    dojo.subscribe(PersonalListCollection.ListCountChangedEvent, function (count) {
//        _setPersonalListCount(count);
//    });

//    function _setPersonalListCount(count){
//        $("#" + iii + " ul li:first .counter").html(count);
//    }
//}

//PersonalListCollection = function (listId, viewId) {
//    this.items = new Array();
//    this.id = listId;
//    this.viewId = viewId;
//    this.current = null;

//    this.count = function () {
//        return this.items.length;
//    }

//    this.add = function (list) {
//        this._addList(list);
//        this._onPersonalListCountChanged();
//    }

//    this.select = function (listName) {
//        var list = this.getByName(listName);
//        this.current = list;
//        this._onCurrentItemChanged(list);
//    }

//    this.getByName = function (name) {
//        for (i = 0; i < this.items.length; i++) {
//            var item = this.items[i];
//            if (item.name == name)
//                return item;
//        }
//    }

//    this.addAll = function (lists) {
//        for (i = 0; i < lists.length; i++) {
//            this._addList(lists[i]);
//        }
//        this._onPersonalListCountChanged();
//    }

//    this.update = function (updatedList) {
//        var list = this.getByName(list.name);
//        $.extend(list, updatedList);

//        if (this._isCurrent(updatedList.name))
//            this._onCurrentItemChanged(list);
//        this._onListItemChanged(list);
//    }

//    this._isCurrent = function (name) {
//        if (!current) return false;
//        return (current.name == name);
//    }

//    this._onPersonalListCountChanged = function () {
//        dojo.publish(PersonalListCollection.ListCountChangedEvent, [this.count()]);
//    }
//    this._onCurrentItemChanged = function (list) {
//        dojo.publish(PersonalListCollection.CurrentItemChangedEvent, [list]);
//    }
//    this._onListItemChanged = function (list) {
//        dojo.publish(PersonalListCollection.ListItemChangedEvent, [list]);
//    }
//    this._addList = function (list) {
//        this.items.push(list);
//        var html = list.getHtml(this.viewId);
//        var $newItemRow = $("#" + this.id + " ul li:last");
//        $newItemRow.before(html);
//    }
//}
//PersonalListCollection.CountChangedEvent = "personalListCollection.countChanged";
//PersonalListCollection.CurrentItemChangedEvent = "personalListCollection.currentItemChanged";

//PersonalList = function (name, count) {
//    this.name = name;
//    this.items = new Array();

//    this.count = function () {
//        return this.items.length;
//    }

//    this.add = function (list) {
//        this._addList(list);
//    }

//    this.updateItem = new function (updatedItem) {
//        var item = this.getByName(updatedItem.name);
//        $.extend(item, updatedList);
//        this._onItemUpdated(item);
//    }

//    this.getByName = function (name) {
//        for (i = 0; i < this.items.length; i++) {
//            var item = this.items[i];
//            if (item.name == name)
//                return item;
//        }
//    }

//    this.getHtml = function (viewId) {
//        var row = '<li class="arrow">';
//        row += '<a href="#' + viewId + '">' + this.name + '</a> ';
//        row += '<small class="counter">' + this.count + '</small>';
//        row += '<input name="name" type="hidden" value="' + this.name + '" />';
//        row += '</li>\n';
//        return row;
//    }

//    this._onItemUpdated(item) {
//        dojo.publish(PersonalList.ItemChangedEvent, [this.name, item])
//    }
//}
//PersonalList.ItemChangedEvent = "personalList.itemChanged";


//PersonalListItem = function (name, count, completed) {
//    this.name = name;
//    this.count = count ? count : 0;
//    this.completed = completed ? completed : false;


//    this.getHtml = function (viewId) {
//        var row = '<li class="arrow">';
//        row += '<a href="#' + viewId + '">' + this.name + '</a> ';
//        row += '<small class="counter">' + this.count + '</small>';
//        row += getCompletedHtml(this.completed);
//        row += '<input name="name" type="hidden" value="' + this.name + '" />';
//        row += '</li>\n';
//        return row;

//        function getCompletedHtml(completed) {
//            if (completed)
//                return '<input type="checkbox" checked="checked"></small>';
//            else
//                return '<input type="checkbox"></small>';
//        }

//    }
//}
//PersonalListItem.ItemChangedEvent = "personalListItemChanged";




Events = function () { }
Events.CreateNewListEvent = "createNewList";
Events.PersonalListAddedEvent = "personalListAdded";
Events.ShowPersonalListDetailsEvent = "showPersonalListDetails";
Events.CreateNewPersonalListItemEvent = "createNewPersonalListItem";
Events.PersonalListChangedEvent = "personalListChanged";

Events.SignUp = "signUp";
Events.SignupFailed = "signUpFailed";
Events.SignedUp = "signedUp";

Events.LogIn = "logIn";
Events.LoginFailed = "logInFailed";
Events.LoggedIn = "loggedIn";

Views = function () { }
Views.Main = "#loggedin_home";
Views.Home = "#home";
Views.LogIn = "#login";
Views.LogOut = "#logout";
Views.SignUp = "#signup";


Net = function () { }
Net.put = function (data, url, success) { Net.ajax(data, url, success, 'POST'); }
Net.get = function (url, success) { Net.ajax('GET', url, success); }
Net.ajax = function (data, url, success, method) {
    $.ajax({
        type: method,
        url: url,
        dataType: 'json',
        data: data,
        contentType: 'application/json; charset=utf-8',
        processData: false,
        success: success,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            throw ("Ooooops!, request failed with status: " + XMLHttpRequest.status + ' ' + XMLHttpRequest.responseText);
        }
    });
}


$.fn.clear = function () {
    this.val("");
}


dojo.subscribe(Events.SignUp, function(username, password) {
    var message = {username: username, password: password}
    Net.put(JSON.stringify(message), Url.SignUp, function(data) {
        var msg = null;
        if (!data)
            msg = "An error occured. Please try again!";
        else if (data.ok)
            dojo.publish(Events.SignedUp, [data.userId]);
        else
            dojo.publish(Events.SignUpFailed, [msg ? msg : data.message]);
    });
});

dojo.subscribe(Events.LogIn, function (username, password) {
    var message = { username: username, password: password }
    Net.put(JSON.stringify(message), Url.LogIn, function (data) {
        var msg = null;
        if (!data)
            msg = "An error occured. Please try again!";
        if (data && data.ok)
            dojo.publish(Events.LoggedIn, [data.userId]);
        else
            dojo.publish(Events.LoginFailed, [msg ? msg : data.message]);
    });
});




