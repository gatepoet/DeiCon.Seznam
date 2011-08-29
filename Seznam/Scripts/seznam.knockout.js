/// <reference path="jquery-1.5.2.js" />
/// <reference path="dojo.js.uncompressed.js" />
/// <reference path="json2.js" />

$(function() {
    ko.bindingHandlers.longClick = {
        init: function(element, valueAccessor) {
            ko.utils.registerEventHandler(element, "taphold", function() {
                console.log("hold");
                valueAccessor()();
            });
        }
    };
    
    ko.bindingHandlers.jQueryButtonEnable = {
        update: function (element, valueAccessor) {
            ko.bindingHandlers.enable.update(element, valueAccessor);
            var value = ko.utils.unwrapObservable(valueAccessor());
            $(element).button(value ? "enable" : "disable");
        }
    };
    
    ko.bindingHandlers.jQueryTextBoxEnable = {
        init: function (element, valueAccessor) {
            var deviceAgent = navigator.userAgent.toLowerCase();
            var agentID = true; //deviceAgent.match("android");
            if (agentID)
                $(element).children().each(function () {
                    if (valueAccessor()())
                        $(this).removeAttr('disabled');
                    else
                        $(this).attr('disabled', 'disabled');
                    //$(this).get().disable = !valueAccessor()();
                });


            else {
                var value = ko.utils.unwrapObservable(valueAccessor());
                $(element).toggle(value);
            }
        },
        update: function (element, valueAccessor) {
            //ko.bindingHandlers.visible.update(element, valueAccessor);
            var agentID = true; //deviceAgent.match("android");
            if (agentID)
                $(element).children().each(function () {
                    if (valueAccessor()())
                        $(this).removeAttr('disabled');
                    else
                        $(this).attr('disabled', 'disabled');

                    //                            $(this).get().disable = !valueAccessor()();
                });


            else {
                var value = ko.utils.unwrapObservable(valueAccessor());
                $(element).toggle(value);
            }
            //                    var value = ko.utils.unwrapObservable(valueAccessor());
            //                    if (value)
            //                        $(element).slideDown();
            //                    else
            //                        $(element).slideUp();
        }
    };
    
    ko.bindingHandlers.show = {
        init: function (element, valueAccessor) {
            var value = ko.utils.unwrapObservable(valueAccessor());
            if (value)
                $(element).show();
            else
                $(element).hide();
        },
        update: function (element, valueAccessor) {
            var va = valueAccessor();
            var value = ko.utils.unwrapObservable(va);
            if (value)
                $(element).show();
            else
                $(element).hide();
        }
    };
    
    // my checkboxes handler
    ko.bindingHandlers.jqmCheck = {
        init: function (element, valueAccessor) {
            // Get the observable we are bound to
            var modelValue = valueAccessor();

            // register handler for changes
            $(element).click(function () {
                // update model data
                if (ko.utils.unwrapObservable(modelValue)) { modelValue(false); }
                else { modelValue(true); }
            });
        },
        update: function (element, valueAccessor) {
            // First get the latest data that we're bound to
            var value = valueAccessor();

            // Next, whether or not the supplied model property is observable, get its current value
            var valueUnwrapped = ko.utils.unwrapObservable(value);

            // Now manipulate the DOM element to toggle check mark

            $(element).prop("checked", !valueUnwrapped);
            //value.valueHasMutated();
            //$(element).find("input[type='checkbox']").prop("checked", !valueUnwrapped);
            if (valueUnwrapped) {

                //                        $(element).find('.ui-icon').addClass('ui-icon-checkbox-on').removeClass('ui-icon-checkbox-off');
            }
            else {
                //                        $(element).find('.ui-icon').addClass('ui-icon-checkbox-off').removeClass('ui-icon-checkbox-on');
            }
        }
    };

});