using CompanyApi.Api.Models;
using CompanyApi.Api.Repository.Interface;
using CompanyApi.Api.Services.Interface;

namespace CompanyApi.Api.Services.Implementation
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<Company> GetCompanyByIdAsync(int id)
        {
            return await _companyRepository.GetByIdAsync(id);
        }

        public async Task<Company> GetCompanyByIsinAsync(string isin)
        {
            return await _companyRepository.GetByIsinAsync(isin);
        }

        public async Task<IEnumerable<Company>> GetAllCompaniesAsync()
        {
            return await _companyRepository.GetAllAsync();
        }

        public async Task AddCompanyAsync(Company company)
        {
            await _companyRepository.AddAsync(company);
        }

        public async Task UpdateCompanyAsync(Company company)
        {
            await _companyRepository.UpdateAsync(company);
        }

        public async Task DeleteCompanyAsync(Company company)
        {
            await _companyRepository.DeleteAsync(company);
        }
    }
}
