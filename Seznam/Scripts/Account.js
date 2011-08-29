/// <reference path="jquery-1.5.2.js" />
/// <reference path="dojo.js.uncompressed.js" />
/// <reference path="json2.js" />
/// <reference path="Util.js" />




var account;
$(function() {
    account = new Account();
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
