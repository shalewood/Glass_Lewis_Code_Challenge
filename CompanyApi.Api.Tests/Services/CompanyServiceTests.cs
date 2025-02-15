using CompanyApi.Api.Models;
using CompanyApi.Api.Repository.Interface;
using CompanyApi.Api.Services.Implementation;
using CompanyApi.Api.Tests.TestData;
using Moq;

namespace CompanyApi.Api.Tests.Services
{
    public class CompanyServiceTests
    {
        private readonly Mock<ICompanyRepository> _mockRepo;
        private readonly CompanyService _service;
        public CompanyServiceTests()
        {
            _mockRepo = new Mock<ICompanyRepository>();
            _service = new CompanyService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllCompaniesAsync_ShouldReturnCompanies()
        {
            // Arrange
            var expectedCompanies = TestDataHelper.GetCompanies();
            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedCompanies);

            // Act
            var result = await _service.GetAllCompaniesAsync();

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(expectedCompanies.Count, result.Count());
            Assert.All(result, company => Assert.Contains(company, expectedCompanies));
        }

        [Fact]
        public async Task GetCompanyByIdAsync_ShouldReturnCompany()
        {
            // Arrange
            var expectedCompany = TestDataHelper.CompanyData();
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(expectedCompany);

            // Act
            Company? result = await _service.GetCompanyByIdAsync(1);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCompany.Id, result.Id);
            Assert.Equal(expectedCompany.Name, result.Name);
            Assert.Equal(expectedCompany.Ticker, result.Ticker);
            Assert.Equal(expectedCompany.Isin, result.Isin);
        }

        [Fact]
        public async Task GetCompanyByIdAsync_ShouldReturnNullCompany()
        {
            // Arrange
            var expectedNullCompany = TestDataHelper.CompanyDataEmpty();
            _mockRepo.Setup(repo => repo.GetByIdAsync(0)).ReturnsAsync(expectedNullCompany);

            // Act
            Company result = await _service.GetCompanyByIdAsync(0);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetCompanyByIsinAsync_ShouldReturnCompany_WhenIdDoesExist()
        {
            // Arrange
            var expectedCompany = TestDataHelper.CompanyData();
            var issin = expectedCompany.Isin;
            _mockRepo.Setup(repo => repo.GetByIsinAsync(issin)).ReturnsAsync(expectedCompany);

            // Act
            var result = await _service.GetCompanyByIsinAsync(issin);
            //Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCompany.Id, result.Id);
            Assert.Equal(expectedCompany.Name, result.Name);
            Assert.Equal(expectedCompany.Ticker, result.Ticker);
            Assert.Equal(expectedCompany.Isin, result.Isin);
        }

        [Fact]
        public async Task GetCompanyByIsinAsync_ShouldReturnNullCompany_WhenIdDoesNotExist()
        {
            // Arrange
            var expectedNullCompany = TestDataHelper.CompanyDataEmpty();
            var issin = "0";
            _mockRepo.Setup(repo => repo.GetByIsinAsync(issin)).ReturnsAsync(expectedNullCompany);

            // Act
            var result = await _service.GetCompanyByIsinAsync(issin);
            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddCompanyAsync_ShouldAddCompanyCallRepoOnce()
        {
            // Arrange
            var expectedNewCompany = TestDataHelper.CompanyNewData();
            _mockRepo.Setup(repo => repo.AddAsync(expectedNewCompany))
                .Returns(Task.CompletedTask);

            // Act
            await _service.AddCompanyAsync(expectedNewCompany);

            // Assert
            _mockRepo.Verify(repo => repo.AddAsync(expectedNewCompany), Times.Once);
        }

        [Fact]
        public async Task UpdateCompanyAsync_ShouldUpdateCompanyCallRepoOnce()
        {
            // Arrange
            var updateCompany = TestDataHelper.CompanyNewData();
            updateCompany.Name = "Test Name";

            _mockRepo.Setup(repo => repo.UpdateAsync(updateCompany))
                .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateCompanyAsync(updateCompany);

            // Assert
            _mockRepo.Verify(repo => repo.UpdateAsync(updateCompany), Times.Once);
        }

        [Fact]
        public async Task DeleteCompanyAsync_ShouldDeleteCompanyCallRepoOnce()
        {
            // Arrange
            var removedCompany = TestDataHelper.CompanyData();

            _mockRepo.Setup(repo => repo.DeleteAsync(removedCompany))
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteCompanyAsync(removedCompany);

            // Assert
            _mockRepo.Verify(repo => repo.DeleteAsync(removedCompany), Times.Once);
        }
    }
}