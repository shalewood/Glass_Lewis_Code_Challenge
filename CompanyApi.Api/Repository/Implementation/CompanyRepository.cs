using CompanyApi.Api.Data;
using CompanyApi.Api.Models;
using CompanyApi.Api.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace CompanyApi.Api.Repository.Implementation
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly AppDBContext _context;

        public CompanyRepository(AppDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task<Company> GetByIdAsync(int id)
        {
            return await _context.Companies.FindAsync(id);
        }

        public async Task<Company> GetByIsinAsync(string isin)
        {
            return await _context.Companies.FirstOrDefaultAsync(c => c.Isin == isin);
        }

        public async Task UpdateAsync(Company company)
        {
            var exisitngCompany = await _context.Companies.FindAsync(company.Id);

            // Update accordingly -  stops error as item is alreday being tracked
            exisitngCompany.Isin = company.Isin;
            exisitngCompany.Name = company.Name;
            exisitngCompany.Ticker = company.Ticker;
            exisitngCompany.Exchange = company.Exchange;
            exisitngCompany.Website = company.Website;

            _context.Companies.Update(exisitngCompany);
            await SaveChangesAsync();
        }

        public async Task AddAsync(Company company)
        {
            await _context.Companies.AddAsync(company);
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(Company company)
        {
            _context.Companies.Remove(company);
            await SaveChangesAsync();
        }

        private async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
