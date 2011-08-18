using System;

namespace Seznam.Data
{
    public interface IRepository
    {
        T Store<T>(T data);
        T GetById<T>(string id);
        T GetByCriteria<T>(Func<T,bool> predicate);
    }
}