using CompanyApi.Api.Models;

namespace CompanyApi.Api.Repository.Interface
{
    public interface ICompanyRepository
    {
        Task<Company> GetByIdAsync(int id);
        Task<Company> GetByIsinAsync(string isin);
        Task<IEnumerable<Company>> GetAllAsync();
        Task AddAsync(Company company);
        Task UpdateAsync(Company company);
        Task DeleteAsync(Company company);
    }
}
