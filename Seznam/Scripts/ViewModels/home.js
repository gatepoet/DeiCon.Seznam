/// <reference path="~/Scripts/jquery-1.5.2.js" />
/// <reference path="~/Scripts/dojo.js.uncompressed.js" />
/// <reference path="~/Scripts/json2.js" />
/// <reference path="~/Scripts/seznam.app.js" />
/// <reference path="~/Scripts/seznam.util.js" />
/// <reference path="~/Scripts/seznam.vars.js" />

$(function () {
    var homeViewModel = {
        logIn: function () { $.mobile.changePage(Views.LogIn); },
        signUp: function () { $.mobile.changePage(Views.SignUp); }
    };

    ko.applyBindings(homeViewModel, $(Views.Home)[0]);
});
