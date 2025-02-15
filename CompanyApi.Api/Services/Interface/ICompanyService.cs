using CompanyApi.Api.Models;

namespace CompanyApi.Api.Services.Interface
{
    public interface ICompanyService
    {
        Task<Company> GetCompanyByIdAsync(int id);
        Task<Company> GetCompanyByIsinAsync(string isin);
        Task<IEnumerable<Company>> GetAllCompaniesAsync();
        Task AddCompanyAsync(Company company);
        Task UpdateCompanyAsync(Company company);
        Task DeleteCompanyAsync(Company company);
    }
}
