using AutoMapper;
using CompanyApi.Api.Models;
using CompanyApi.Api.Models.DTO;
using CompanyApi.Api.Services.Interface;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace CompanyApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IValidator<CompanyDTO> _validator;
        private readonly IMapper _mapper;

        public CompanyController(ICompanyService companyService, IValidator<CompanyDTO> validator, IMapper mapper)
        {
            _companyService = companyService;
            _validator = validator;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCompanies()
        {
            var companies = await _companyService.GetAllCompaniesAsync();
            var companiesDTO = _mapper.Map<IEnumerable<CompanyDTO>>(companies);
            return Ok(companiesDTO);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCompanyById(int id)
        {
            var company = await _companyService.GetCompanyByIdAsync(id);
            if (company == null)
            {
                return NotFound("Company not found.");
            }
            var companyDTO = _mapper.Map<CompanyDTO>(company);
            return Ok(companyDTO);
        }

        [HttpGet("isin/{isin}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCompanyByIsin(string isin)
        {
            var company = await _companyService.GetCompanyByIsinAsync(isin);
            if (company == null)
            {
                return NotFound("Company not found.");
            }
            var companyDTO = _mapper.Map<CompanyDTO>(company);
            return Ok(companyDTO);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCompany([FromBody] CompanyDTO companyDto)
        {
            ValidationResult result = await _validator.ValidateAsync(companyDto);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            var companyCheck = await _companyService.GetCompanyByIsinAsync(companyDto.Isin);
            if (companyCheck != null && companyCheck.Isin == companyDto.Isin)
            {
                return BadRequest($"ISIN already exist: {companyDto.Isin}");
            }

            var company = _mapper.Map<Company>(companyDto);
            await _companyService.AddCompanyAsync(company);
            return CreatedAtAction(nameof(GetCompanyById), new { id = company.Id }, companyDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCompany(int id, [FromBody] CompanyDTO companyDto)
        {
            var company = await _companyService.GetCompanyByIdAsync(id);

            if (id != company.Id)
            {
                return BadRequest($"Invalid Company");
            }
            await _companyService.UpdateCompanyAsync(_mapper.Map<Company>(companyDto));
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var company = await _companyService.GetCompanyByIdAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            await _companyService.DeleteCompanyAsync(company);
            return NoContent();
        }
    }
}
