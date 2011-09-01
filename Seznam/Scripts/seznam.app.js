/// <reference path="jquery-1.6.2.js" />
/// <reference path="dojo.js.uncompressed.js" />
/// <reference path="json2.js" />
/// <reference path="seznam.util.js" />
/// <reference path="seznam.socket.js" />

$('body').live('pagecreate', function (event) {
    Util.lazyLoadAll();
});
$.mobile.page.prototype.options.domCache = true;
var account;
var seznam;
var socket;

$(function () {
    account = new Account();
    seznam = new Seznam();
    socket = new Socket();
});


