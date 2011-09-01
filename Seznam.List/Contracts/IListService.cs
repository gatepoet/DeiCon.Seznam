using System;

namespace Seznam.List.Contracts
{
    public interface IListService : IDisposable
    {
        SeznamList CreateList(SeznamList list);
        ItemChangedData CreateListItem(string listId, string name, int count);
        ItemChangedData TogglePersonalItem(string listId, string name, bool completed);
        ItemChangedData ToggleSharedItem(string listId, string name, bool completed);
        ItemChangedData DeleteItem(string listId, string name);
        SeznamSummmary GetSummary(string userId);
    }
}
