/// <reference path="jquery-1.5.2.js" />
/// <reference path="dojo.js.uncompressed.js" />
/// <reference path="json2.js" />
/// <reference path="../Views/Home/Index.cshtml" />

var account;
var seznam;
$(function() {
    account = new Account();
    seznam = new Seznam();
});

Account = function (options) {
    this.options = $.extend(this.defaults, options);
    this.id = null;
    this.loggedIn = false;


    //SignUp
    Util.subscribe(Events.SignUp, function (message) {
        Net.put(JSON.stringify(message), Url.SignUp, function (data) {
            var msg = null;
            if (!data)
                msg = GENERAL_ERROR_MESSAGE;
            if (data && data.ok) {
                Util.publish(Events.Authorized, [data.userId]);
                Util.publish(Events.SignedUp, [data.userId]);
            }
            else
                Util.publish(Events.SignupFailed, [msg ? msg : data.message]);
        });
    });

    //LogIn
    Util.subscribe(Events.LogIn, function (message) {
        Net.post(JSON.stringify(message), Url.LogIn, function (data) {
            var msg = null;
            if (!data)
                msg = GENERAL_ERROR_MESSAGE;
            if (data && data.ok) {
                Util.publish(Events.Authorized, [data.userId]);
                Util.publish(Events.LoggedIn, [data.userId]);
            }
            else
                Util.publish(Events.LoginFailed, [msg ? msg : data.message]);
        });
    });


    //Authorized
    Util.subscribe(Events.Authorized, this, function (userId, context) {
        //set user id
        context.id = userId;
        context.loggedIn = true;
        $.mobile.changePage(Views.Main, { transition: "slideup" });
        $.mobile.showPageLoadingMsg();
        $.getJSON(Url.GetAllData, function (data) {
            Util.publish(Events.UpdateAllData, [data]);
            $.mobile.hidePageLoadingMsg();
        });

    });

    //LogOut
    Util.subscribe(Events.LogOut, function () {
        Net.post(null, Url.LogOut, function (data) {
            if (data && data.ok)
                Util.publish(Events.LoggedOut);
        });
    });

    //LoggedOut
    Util.subscribe(Events.LoggedOut, function () {
        $.mobile.changePage(Views.Home, { transition: "slidedown" });
    });

};

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
        Net.put(JSON.stringify(item), Url.CreatePersonalListItem, function (data) {
            if (data) {
                Util.publish(Events.PersonalListItemCreated, [data]);
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
    Util.subscribe(Events.TogglePersonalListItem, this, function (message, context) {
        Net.post(JSON.stringify(message), Url.TogglePersonalListItem, function (message) {
            if (message) {
                Util.publish(Events.PersonalListItemToggled, [message]);
            }
        });
    });
    Util.subscribe(Events.PersonalListItemToggled, this, function (message, context) {
        var list = context.getPersonalList(context.personalLists, message.listId);
        var item = context.getListItem(list.items, message.itemName);
        item.completed = message.itemCompleted;
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


Events = new Object();

Events.SignUp = "signUp";
Events.SignupFailed = "signUpFailed";
Events.SignedUp = "signedUp";

Events.LogIn = "logIn";
Events.LoginFailed = "logInFailed";
Events.LoggedIn = "loggedIn";
Events.Authorized = "authorized";

Events.LogOut = "logOut";
Events.LoggedOut = "loggedOut";

Events.UpdateAllData = "updateAllData";

Events.CreateList = "createList";
Events.ListCreated = "listCreated";
Events.ListUpdated = "listUpdated";
Events.ViewListDetails = "viewListDetails";
Events.CreatePersonalListItem = "createPersonalListItem";
Events.PersonalListItemCreated = "personalListItemCreated";
Events.TogglePersonalListItem = "togglePersonalListItem";
Events.PersonalListItemToggled = "personalListItemtoggled";



Views = new Object();
Views.Main = "#main";
Views.Home = "#home";
Views.LogIn = "#login";
Views.LogOut = "#logout";
Views.SignUp = "#signup";
Views.PersonalLists = "#personal_lists";
Views.PersonalListDetail = "#personal_list_detail";
Views.CreateList = "#create_list";
Views.SharedLists = "#shared_lists";


Net = new Object();
Net.put = function(data, url, success) { Net.ajax(data, url, success, 'PUT'); };
Net.post = function(data, url, success) { Net.ajax(data, url, success, 'POST'); };
Net.get = function (url, success) {
    $.ajax({
        type: "GET",
        url: url,
        contentType: 'text/html',
        processData: false,
        success: success,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            throw ("Ooooops!, request failed with status: " + XMLHttpRequest.status + ' ' + XMLHttpRequest.responseText);
        }
    });
};

Net.ajax = function(data, url, success, method) {
    $.ajax({
            type: method,
            url: url,
            dataType: 'json',
            data: data,
            cache: false,
            contentType: 'application/json; charset=utf-8',
            processData: false,
            success: success,
            error: function(XMLHttpRequest, textStatus, errorThrown) {
                throw ("Ooooops!, request failed with status: " + XMLHttpRequest.status + ' ' + XMLHttpRequest.responseText);
            }
        });
};

Util = new Object();
Util.lazyLoadAll = function () {
    $('link[rel="section"][type="text/html"]').each(function () {
        var $section = $(this);
        var url = $section.attr("href");
        console.log("Lazy loading '" + url + "'.");
        var o = $.mobile.loadPage(url);
        $section.remove();
        //        Net.get(url, function (html) {
        //            $(html).appendTo(".ui-page").trigger("create");
        //        });
    });
};
Util.lazyLoadCategory = function(category) {
    $('link[rel="section"][type="text/html"][category="' + category + '"]').each(function () {
        var $section = $(this);
        var url = $section.attr("href");
        var html = Net.get(url);
        console.log("Lazy loading '" + url + "'.");
        $section.replaceWith(html);
    });
};

Util.publish = function (name, data) {
    var msg = "Event " + name + " fired.";
    if (data)
        msg += " [" + JSON.stringify(data) + "]";
    console.log(msg);
    dojo.publish(name, data);
};

Util.subscribe = function (name, context, action) {
    var handle = function (data) {
        action(data, this);
        console.log("Event " + name + " handled.");
    };
    if (!action) {
        action = context;
        dojo.subscribe(name, handle);
    }
    else {
        dojo.subscribe(name, context, handle);
    }
};

var GENERAL_ERROR_MESSAGE = "An error occured. Please try again!";