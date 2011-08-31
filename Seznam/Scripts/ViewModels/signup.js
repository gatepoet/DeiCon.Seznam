/// <reference path="~/Scripts/jquery-1.5.2.js" />
/// <reference path="~/Scripts/dojo.js.uncompressed.js" />
/// <reference path="~/Scripts/json2.js" />
/// <reference path="~/Scripts/seznam.app.js" />
/// <reference path="~/Scripts/seznam.util.js" />
/// <reference path="~/Scripts/seznam.vars.js" />

$(function () {
    var signupViewModel = {
        username: ko.observable(),
        password: ko.observable(),

        error: ko.observable(),

        signUp: function () {
            this.error(null);
            Util.publish(Events.SignUp, [{ username: this.username(), password: this.password()}]);
        }
    };

    ko.applyBindings(signupViewModel, $(Views.SignUp)[0]);


    Util.subscribe(Events.SignedUp, function (userId) {
        //set user id
        $.mobile.changePage(Views.Main);
    });
    Util.subscribe(Events.SignupFailed, function (reason) {
        signupViewModel.error(reason);
    });
});
