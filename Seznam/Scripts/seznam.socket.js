/// <reference path="jquery-1.6.2.js" />
/// <reference path="dojo.js.uncompressed.js" />
/// <reference path="json2.js" />
/// <reference path="seznam.util.js" />


Socket = function (options) {
    this.options = $.extend(this.defaults, options);
    this.connected = false;

    this.client = fm.websync.client;
    this.client.initialize();

    Util.subscribe(Events.Authorized, this, function (message, context) {
        context.client.connect();
        context.client.subscribe({
            channel: "/user/" + message.userId,
            onReceive: function (args) {
                var msg = args.data;
                Util.publish(msg.eventType, [msg]);
            }
        });
    });

    Util.subscribe(Events.LoggedOut, this, function (message, context) {
        context.client.disconnect();
    });

};
