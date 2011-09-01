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
