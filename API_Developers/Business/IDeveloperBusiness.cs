using API_Developers.Model;

namespace API_Developers.Business
{
    public interface IDeveloperBusiness
    {
        Task<Developer> Create(Developer developer);
        Developer FindById(long id);
        List<Developer> FindAll();
        Developer Update(Developer developer);
        void Delete(long Id);
    }
}
