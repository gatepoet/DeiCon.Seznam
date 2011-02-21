/// <reference path="jquery-1.5.js" />
/// <reference path="dojo.js.uncompressed.js" />

Seznam = function (id, personalListId, personalviewId) {
    this.id = id;
    this.personalLists = new PersonalListCollection(personalListId, personalviewId);
    var iii = id;
    dojo.subscribe(PersonalListCollection.ListCountChangedEvent, function (count) {
        $("#" + iii + " ul li:first .counter").html(count);
    });
}

PersonalListCollection = function (listId, viewId) {
    this.items = new Array();
    this.id = listId;
    this.viewId = viewId;

    this.count = function () {
        return this.items.length;
    }

    this.add = function (list) {
        this._addList(list);
        this._onPersonalListCountChanged();
    }

    this.select = function (listName) {
        var list = this.getByName(listName);
        this.current = list;
        this._onCurrentItemChanged(list);
    }

    this.getByName = function (name) {
        for (i = 0; i < this.items.length; i++) {
            var item = this.items[i];
            if (item.name == name)
                return item;
        }
    }

    this.addAll = function (lists) {
        for (i = 0; i < lists.length; i++) {
            this._addList(lists[i]);
        }
        this._onPersonalListCountChanged();
    }

    this._onPersonalListCountChanged = function () {
        dojo.publish(PersonalListCollection.ListCountChangedEvent, [this.count()]);
    }
    this._onCurrentItemChanged = function (list) {
        dojo.publish(PersonalListCollection.CurrentItemChangedEvent, [list]);
    }
    this._addList = function (list) {
        this.items.push(list);
        var html = list.getHtml(this.viewId);
        var $newItemRow = $("#" + this.id + " ul li:last");
        $newItemRow.before(html);
    }
}
PersonalListCollection.ListCountChangedEvent = "personalListCountChanged";
PersonalListCollection.CurrentItemChangedEvent = "personalListCurrentItemChanged";

PersonalList = function (name, count) {
    this.name = name;
    this.count = count ? count : 0;
    this.items = new Array();

    this.getHtml = function (viewId) {
        var row = '<li class="arrow">';
        row += '<a href="#' + viewId + '">' + this.name + '</a> ';
        row += '<small class="counter">' + this.count + '</small>';
        row += '<input name="name" type="hidden" value="' + this.name + '" />';
        row += '</li>\n';
        return row;
    }
}

Events = function () { }
Events.CreateNewListEvent = "createNewList";
Events.PersonalListAddedEvent = "personalListAdded";
Events.ShowPersonalListDetails = "showPersonalListDetails";

Net = function () { }
Net.put = function ($form, success) { Net.ajax('PUT', $form.attr('action'), success, $form.serialize()); }
Net.get = function (url, success) { Net.ajax('GET', url, success); }
Net.ajax = function (method, url, success, data) {
    $.ajax({
        type: method,
        url: url,
        dataType: 'json',
        data: data,
        processData: false,
        success: success,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            throw ("Ooooops!, request failed with status: " + XMLHttpRequest.status + ' ' + XMLHttpRequest.responseText);
        }
    });
}


$.fn.clear = function () {
    this.val("");
}
