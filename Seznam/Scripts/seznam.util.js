/// <reference path="jquery-1.5.2.js" />
/// <reference path="dojo.js.uncompressed.js" />
/// <reference path="json2.js" />



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
Events.ViewSharedListDetails = "viewSharedListDetails";
Events.CreateItem = "createPersonalListItem";
Events.CreateSharedItem = "createSharedListItem";
Events.CreateItemFailed = "createPersonalListItemFailed";
Events.CreateSharedItemFailed = "createSharedListItemFailed";
Events.ItemCreated = "personalListItemCreated";
Events.SharedItemCreated = "sharedListItemCreated";
Events.ToggleSharedItem = "toggleSharedItem";
Events.SharedItemToggled = "sharedItemToggled";
Events.DeleteItem = "deletePersonalListItem";
Events.ItemDeleted = "personalListItemDeleted";
Events.ToggleItem = "togglePersonalListItem";
Events.ItemToggled = "personalListItemtoggled";



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
Views.SharedListDetail = "#shared_list_detail";


Net = new Object();
Net.put = function(data, url, success) { Net.ajax(data, url, success, 'PUT'); };
Net.post = function(data, url, success) { Net.ajax(data, url, success, 'POST'); };
Net.destroy = function(data, url, success) { Net.ajax(data, url, success, 'DELETE'); };
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
Net.getJSON = function (url, success) {
    $.ajax({
        type: "GET",
        url: url,
        dataType: 'json',
        cache: false,
        contentType: 'application/json; charset=utf-8',
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




// Array Remove - By John Resig (MIT Licensed)
Array.prototype.remove = function (from, to) {
    var rest = this.slice((to || from) + 1 || this.length);
    this.length = from < 0 ? this.length + from : from;
    return this.push.apply(this, rest);
};

