/// <reference path="~/Scripts/jquery-1.5.2.js" />
/// <reference path="~/Scripts/dojo.js.uncompressed.js" />
/// <reference path="~/Scripts/json2.js" />
/// <reference path="~/Scripts/seznam.app.js" />
/// <reference path="~/Scripts/seznam.util.js" />
/// <reference path="~/Scripts/seznam.vars.js" />

$(function () {
    var loginViewModel = {
        username: ko.observable(),
        password: ko.observable(),

        error: ko.observable(),

        logIn: function () {
            this.error(null);
            Util.publish(Events.LogIn, [{ username: this.username(), password: this.password()}]);
        }
    };
    ko.applyBindings(loginViewModel, $(Views.LogIn)[0]);


    dojo.subscribe(Events.LoggedIn, function () {
        loginViewModel.username(null);
        loginViewModel.password(null);
    });

    dojo.subscribe(Events.LoginFailed, function (reason) {
        loginViewModel.error(reason);
    });
});
