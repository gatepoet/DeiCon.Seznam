/// <reference path="jquery-1.6.2.js" />
/// <reference path="dojo.js.uncompressed.js" />
/// <reference path="json2.js" />
/// <reference path="seznam.util.js" />
/// <reference path="seznam.vars.js" />


Socket = function (options) {
    this.options = $.extend(this.defaults, options);
    this.connected = false;

    this.client = fm.websync.client;
    this.client.initialize();

    Util.subscribe(Events.Authorized, this, function (message, context) {
        Util.subscribe(Events.Connected, context, function (m, c) {
            c.client.subscribe({
                channel: "/user/" + message.userId,
                onReceive: function (args) {
                    var msg = args.data;
                    Util.publish(msg.eventType, [msg]);
                }
            });
        }); 
        
        context.client.connect({
            onSuccess: function (args) {
                Util.publish(Events.Connected);
                console.log("Connected with ID " + args.clientId + ".");
            },
            onStreamFailure: function (args) {
                console.log("Network problems have been detected. " +
            (args.willReconnect ? "Will" : "Will not") + " reconnect.");
            },
            onFailure: function (args) {
                console.log("Could not connect. " + args.error);
            }
        });

    });


    Util.subscribe(Events.LoggedOut, this, function (message, context) {
        context.client.disconnect();
    });

};
