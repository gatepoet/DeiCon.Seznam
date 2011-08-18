using System.Text;

namespace Seznam.Data.Services.List.Contracts
{
    public interface IListService
    {
        SeznamSummmary GetSummary(string userId);
        SeznamList CreateList(SeznamList list);
        SeznamListItem CreateListItem(string listId, string name, int count);
        SeznamListItem TogglePersonalListItem(string listId, string name, bool completed);
    }
}
