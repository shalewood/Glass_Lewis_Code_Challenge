using CompanyApi.Api.Data;
using CompanyApi.Api.Repository.Implementation;
using CompanyApi.Api.Tests.TestData;
using Microsoft.EntityFrameworkCore;

namespace CompanyApi.Api.Tests.Repository
{
    public  class CompanyRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<AppDBContext> _dbContextOptions;
        private readonly AppDBContext _context; // Real context for setup
        private readonly CompanyRepository _repository;
        
        //Set Up
        public CompanyRepositoryTests()
        {
            // Since DB is seeded, Use exisitng Companies
            _dbContextOptions = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "TestCompanyApiDatabase")
                .Options;

            _context = new AppDBContext(_dbContextOptions);

            _context.Database.EnsureCreated();

            _repository = new CompanyRepository(_context); // Use the REAL context here!
        }


        [Fact]
        public async Task GetByIdAsync_ReturnCompany_WhenIdDoesExist()
        {
            // Arrange
            var expectedCompany = TestDataHelper.CompanyData();

            // Act
            var result = await _repository.GetByIdAsync(expectedCompany.Id); // Use the actual ID

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCompany.Id, result.Id);
            Assert.Equal(expectedCompany.Name, result.Name);
            Assert.Equal(expectedCompany.Ticker, result.Ticker);
            Assert.Equal(expectedCompany.Isin, result.Isin);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenIdDoesNotExist()
        {
            // Arrange (no need to add a company)

            // Act
            var result = await _repository.GetByIdAsync(0); // Use an ID that doesn't exist

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIsinAsync_ReturnsCompany_WhenIsinExists()
        {
            // Arrange
            var expectedCompany = TestDataHelper.CompanyData();
            _context.SaveChanges();

            // Act
            var result = await _repository.GetByIsinAsync(expectedCompany.Isin);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCompany.Id, result.Id);
            Assert.Equal(expectedCompany.Name, result.Name);
            Assert.Equal(expectedCompany.Ticker, result.Ticker);
            Assert.Equal(expectedCompany.Isin, result.Isin);
        }

        [Fact]
        public async Task GetByIsinAsync_ReturnsNull_WhenIsinDoesNotExist()
        {
            // Arrange

            // Act
            var result = await _repository.GetByIsinAsync("NonExistentIsin");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllCompanies()
        {
            // Arrange
            var companies = TestDataHelper.GetCompanies();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(companies.Count, result.Count());
        }
        [Fact]
        public async Task AddAsync_AddsCompany()
        {
            // Arrange
            var expectedCompany = TestDataHelper.CompanyData();

            // Act

            // Assert
            var addedCompany = await _context.Companies.FindAsync(expectedCompany.Id); // Retrieve from the database
            Assert.NotNull(addedCompany);
            Assert.Equal(expectedCompany.Name, addedCompany.Name);
            Assert.Equal(expectedCompany.Isin, addedCompany.Isin);
            Assert.Equal(expectedCompany.Ticker, addedCompany.Ticker);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesCompany()
        {
            // Arrange
            var company = TestDataHelper.CompanyData();

            var exstingCompany = await _context.Companies.FindAsync(company.Id);

            exstingCompany.Name = "Updated Name";
            exstingCompany.Isin = "UPDATEDISIN";
            exstingCompany.Ticker = "UPDATEDTICKER";

            // Act
            await _repository.UpdateAsync(exstingCompany);
            await _context.SaveChangesAsync();

            // Assert
            var updatedCompany = await _context.Companies.FindAsync(company.Id);
            Assert.NotNull(updatedCompany);
            Assert.Equal("Updated Name", updatedCompany.Name);
            Assert.Equal("UPDATEDISIN", updatedCompany.Isin);
            Assert.Equal("UPDATEDTICKER", updatedCompany.Ticker);
        }

        [Fact]
        public async Task DeleteAsync_DeletesCompany()
        {
            // Arrange
            var company = TestDataHelper.CompanyData();

            // Act
            await _repository.DeleteAsync(company);
            await _context.SaveChangesAsync();

            // Assert
            var deletedCompany = await _context.Companies.FindAsync(company.Id);
            Assert.Null(deletedCompany);
        }

        // Tear Down
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
