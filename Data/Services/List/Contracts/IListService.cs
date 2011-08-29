using System.Text;

namespace Seznam.Data.Services.List.Contracts
{
    public interface IListService
    {
        SeznamList CreateList(SeznamList list);
        SeznamListItem CreateListItem(string listId, string name, int count);
        SeznamListItem TogglePersonalListItem(string listId, string name, bool completed);
        void DeleteItem(string listId, string name);
        SeznamSummmary GetSummary(string userId, string username);
    }
}
