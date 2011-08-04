/// <reference path="jquery-1.5.2.js" />
/// <reference path="dojo.js.uncompressed.js" />
/// <reference path="json2.js" />

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
                msg = "An error occured. Please try again!";
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
                msg = "An error occured. Please try again!";
            if (data && data.ok) {
                Util.publish(Events.Authorized, [data.userId]);
                Util.publish(Events.LoggedIn, [data.userId]);

            }
            else
                Util.publish(Events.LoginFailed, [msg ? msg : data.message]);
        });
    });


    //Authorized
    Util.subscribe(Events.Authorized, function (userId) {
        //set user id
        this.id = userId;
        this.loggedIn = true;
        $.mobile.changePage(Views.Main);
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
};

Seznam = function (options) {
    this.options = $.extend(this.defaults, options);
    this.personalLists = new Array();
    this.sharedLists = new Array();

    Util.subscribe(Events.UpdateAllData, function (data) {
        $.extend(this, data);
    });
};


Events = new Object();
Events.CreateNewList = "createNewList";
Events.PersonalListAdded = "personalListAdded";
Events.ShowPersonalListDetails = "showPersonalListDetails";
Events.CreateNewPersonalListItem = "createNewPersonalListItem";
Events.PersonalListChanged = "personalListChanged";

Events.SignUp = "signUp";
Events.SignupFailed = "signUpFailed";
Events.SignedUp = "signedUp";

Events.LogIn = "logIn";
Events.LoginFailed = "logInFailed";
Events.LoggedIn = "loggedIn";
Events.Authorized = "authorized";

Events.LogOut = "logOut";
Events.LoggedOut = "loggedOut";

Events.UpdateAllData = "updataAllData";


Views = new Object();
Views.Main = "#main";
Views.Home = "#home";
Views.LogIn = "#login";
Views.LogOut = "#logout";
Views.SignUp = "#signup";
Views.PersonalLists = "#personal_lists";
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
        msg += " [" + data.toString() + "]";
    console.log(msg);
    dojo.publish(name, data);
};

Util.subscribe = function(name, action) {
    dojo.subscribe(name, function (data) {
        action(data);
        console.log("Event " + name + " handled.");
    });
};

