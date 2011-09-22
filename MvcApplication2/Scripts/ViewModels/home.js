$(Views.Home).live('pagecreate', function (event) {
    var homeViewModel = {
        logIn: function () {
             $.mobile.changePage(Views.LogIn);
        },
        signUp: function () { $.mobile.changePage(Views.SignUp); }
    };

    ko.applyBindings(homeViewModel, $(Views.Home)[0]);
});
