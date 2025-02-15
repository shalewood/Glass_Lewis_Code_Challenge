using AutoMapper;
using CompanyApi.Api.Controllers;
using CompanyApi.Api.Models;
using CompanyApi.Api.Models.DTO;
using CompanyApi.Api.Services.Interface;
using CompanyApi.Api.Tests.TestData;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CompanyApi.Tests.Controller
{
    public class CompanyControllerTests
    {
        private readonly Mock<ICompanyService> _mockCompanyService;
        private readonly Mock<IValidator<CompanyDTO>> _mockValidator;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CompanyController _controller;

        public CompanyControllerTests()
        {
            _mockCompanyService = new Mock<ICompanyService>();
            _mockValidator = new Mock<IValidator<CompanyDTO>>();
            _mockMapper = new Mock<IMapper>();
            _controller = new CompanyController(_mockCompanyService.Object, _mockValidator.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllCompanies_ReturnsOkResult_WithCompanies()
        {
            // Arrange
            var companies = TestDataHelper.GetCompanies();
            var companiesDTO = TestDataHelper.GetCompaniesDTO();
            _mockCompanyService.Setup(s => s.GetAllCompaniesAsync()).ReturnsAsync(companies);
            _mockMapper.Setup(m => m.Map<IEnumerable<CompanyDTO>>(companies)).Returns(companiesDTO);

            // Act
            var result = await _controller.GetAllCompanies();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCompanies = Assert.IsType<List<CompanyDTO>>(okResult.Value);
            Assert.Equal(companiesDTO.Count, returnedCompanies.Count);
        }

        [Fact]
        public async Task GetCompanyById_ReturnsOkResult_WhenCompanyExists()
        {
            // Arrange
            var company = TestDataHelper.CompanyData();
            var companyDTO = TestDataHelper.CompanyDTOData();
            _mockCompanyService.Setup(s => s.GetCompanyByIdAsync(1)).ReturnsAsync(company);
            _mockMapper.Setup(m => m.Map<CompanyDTO>(company)).Returns(companyDTO);

            // Act
            var result = await _controller.GetCompanyById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCompany = Assert.IsType<CompanyDTO>(okResult.Value);
            Assert.Equal(companyDTO.Name, returnedCompany.Name);
            Assert.Equal(companyDTO.Exchange, returnedCompany.Exchange);
            Assert.Equal(companyDTO.Ticker, returnedCompany.Ticker);
            Assert.Equal(companyDTO.Website, returnedCompany.Website);
        }

        [Fact]
        public async Task GetCompanyById_ReturnsNotFound_WhenCompanyDoesNotExist()
        {
            // Arrange
            var expectedNullCompany = TestDataHelper.CompanyDataEmpty();
            _mockCompanyService.Setup(s => s.GetCompanyByIdAsync(1)).ReturnsAsync(expectedNullCompany);

            // Act
            var result = await _controller.GetCompanyById(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetCompanyByIsin_ReturnsOkResult_WhenCompanyExists()
        {
            // Arrange
            var company = new Company { Isin = "0" };
            var companyDTO = new CompanyDTO { Isin = "0" };
            _mockCompanyService.Setup(s => s.GetCompanyByIsinAsync("0")).ReturnsAsync(company);
            _mockMapper.Setup(m => m.Map<CompanyDTO>(company)).Returns(companyDTO);

            // Act
            var result = await _controller.GetCompanyByIsin("0");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCompany = Assert.IsType<CompanyDTO>(okResult.Value);
            Assert.Equal(companyDTO.Isin, returnedCompany.Isin);
        }

        [Fact]
        public async Task GetCompanyByIsin_ReturnsNotFound_WhenCompanyDoesNotExist()
        {
            // Arrange
            var expectedNullCompany = TestDataHelper.CompanyDataEmpty();
            _mockCompanyService.Setup(s => s.GetCompanyByIsinAsync("0")).ReturnsAsync(expectedNullCompany);

            // Act
            var result = await _controller.GetCompanyByIsin("0");

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task AddCompany_ReturnsCreated_WhenCompanyIsValid()
        {
            // Arrange
            var companyDTO = TestDataHelper.CompanyDTOData();
            var company = TestDataHelper.CompanyData();
            var expectedNullCompany = TestDataHelper.CompanyDataEmpty();
            var validationResult = new ValidationResult();

            _mockValidator.Setup(v => v.ValidateAsync(companyDTO, default)).ReturnsAsync(validationResult);
            _mockMapper.Setup(m => m.Map<Company>(companyDTO)).Returns(company);
            _mockCompanyService.Setup(s => s.AddCompanyAsync(company)).Returns(Task.CompletedTask);
            _mockCompanyService.Setup(s => s.GetCompanyByIsinAsync(companyDTO.Isin)).ReturnsAsync(expectedNullCompany);
            _mockMapper.Setup(m => m.Map<CompanyDTO>(company)).Returns(companyDTO);

            // Act
            var result = await _controller.AddCompany(companyDTO);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetCompanyById), createdAtActionResult.ActionName);
            Assert.Equal(companyDTO, createdAtActionResult.Value);
        }

        [Fact]
        public async Task AddCompany_ReturnsBadRequest_WhenCompanyIsInvalid()
        {
            // Arrange
            var companyDTO = new CompanyDTO();
            var validationResult = new ValidationResult(new[] { new ValidationFailure("Name", "Name is required") });
            _mockValidator.Setup(v => v.ValidateAsync(companyDTO, default)).ReturnsAsync(validationResult);

            // Act
            var result = await _controller.AddCompany(companyDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task AddCompany_ReturnsBadRequest_WhenISINAlreadyExists()
        {
            // Arrange
            var companyDTO = TestDataHelper.CompanyDTOData();
            var company = TestDataHelper.CompanyData();
            var existingCompany = TestDataHelper.CompanyData();
            var validationResult = new ValidationResult();
            _mockValidator.Setup(v => v.ValidateAsync(companyDTO, default)).ReturnsAsync(validationResult);
            _mockCompanyService.Setup(s => s.GetCompanyByIsinAsync(companyDTO.Isin)).ReturnsAsync(existingCompany);

            // Act
            var result = await _controller.AddCompany(companyDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("ISIN already exist", badRequestResult.Value.ToString());
        }



        [Fact]
        public async Task UpdateCompany_ReturnsNoContent_WhenCompanyIsValid()
        {
            // Arrange
            var companyDTO = TestDataHelper.CompanyDTOData();
            var company = TestDataHelper.CompanyData();
            _mockMapper.Setup(m => m.Map<Company>(companyDTO)).Returns(company);
            _mockCompanyService.Setup(s => s.GetCompanyByIdAsync(company.Id)).ReturnsAsync(company);
            _mockCompanyService.Setup(s => s.UpdateCompanyAsync(company)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateCompany(1, companyDTO);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        [Fact]
        public async Task UpdateCompany_ReturnsBadRequest_WhenIdsDoNotMatch()
        {
            // Arrange
            int id = 0; // ID from the route
            var companyDTO = TestDataHelper.CompanyDTOData();
            var company = TestDataHelper.CompanyNewData();

            _mockCompanyService.Setup(s => s.GetCompanyByIdAsync(id)).ReturnsAsync(company);
            _mockMapper.Setup(m => m.Map<Company>(companyDTO)).Returns(TestDataHelper.CompanyNewData());

            // Act
            var result = await _controller.UpdateCompany(id, companyDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Invalid Company", badRequestResult.Value.ToString());
        }


        [Fact]
        public async Task DeleteCompany_ReturnsNotFound_WhenCompanyDoesNotExist()
        {
            // Arrange
            _mockCompanyService.Setup(s => s.GetCompanyByIdAsync(1)).ReturnsAsync((Company)null);

            // Act
            var result = await _controller.DeleteCompany(1);

            // Assert
            Assert.IsType<NotFoundResult>(result); // Or NotFoundObjectResult if you want to return a message
        }
    }
}
