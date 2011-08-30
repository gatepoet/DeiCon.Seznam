/// <reference path="jquery-1.5.2.js" />
/// <reference path="dojo.js.uncompressed.js" />
/// <reference path="json2.js" />
/// <reference path="seznam.account.js" />
/// <reference path="seznam.list.js" />
/// <reference path="seznam.util.js" />
/// <reference path="seznam.socket.js" />

$('body').live('pagecreate', function (event) {
    Util.lazyLoadAll();
});

var account;
var seznam;
var socket;

$(function () {
    account = new Account();
    seznam = new Seznam();
    socket = new Socket();
});


