using API_Developers.Model.Base;

namespace API_Developers.Repository.Generic
{
    public interface IRepository<T> where T : BaseEntity
    {
        T Create(T item);
        T FindById(long id);
        List<T> FindAll();
        T Update(T item);
        void Delete(long Id);
        void DeleteAll();
        bool Exists(long id);
    }
}
