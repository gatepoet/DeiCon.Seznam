/// <reference path="jquery-1.6.2.js" />
/// <reference path="dojo.js.uncompressed.js" />
/// <reference path="json2.js" />

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
    $('link[rel="content"][type="text/html"]').each(function () {
        var $section = $(this);
        var url = $section.attr("href");
        console.log("Lazy loading '" + url + "'.");
        Net.get(url, function (html) {
            $section.replaceWith(html);
        });
    });
    $('link[rel="page"][type="text/html"]').each(function () {
        var $section = $(this);
        var url = $section.attr("href");
        console.log("Lazy loading '" + url + "'.");
        $.mobile.loadPage(url);
        $section.remove();
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

