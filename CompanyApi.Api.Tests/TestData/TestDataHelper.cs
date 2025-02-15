using CompanyApi.Api.Models.DTO;
using CompanyApi.Api.Models;

namespace CompanyApi.Api.Tests.TestData
{
    public static class TestDataHelper
    {
        public static Company CompanyDataEmpty()
        {
            return null;
        }

        public static CompanyDTO CompanyDTODataEmpty()
        {
            return null;
        }

        public static Company CompanyData()
        {

            return new Company { Id = 1, Name = "Apple Inc.", Exchange = "NASDAQ AAPL", Ticker = "AAPL", Isin = "US0378331005", Website = "http://www.apple.com" };
        }

        public static Company CompanyNewData()
        {

            return new Company { Id = 6, Name = "Intel Corporation", Exchange = "INTEL", Ticker = "TAH000000945", Isin = "US4581401001", Website = "http://www.intc.com/" };
        }

        public static CompanyDTO CompanyDTOData()
        {
            return new CompanyDTO { Id = 1, Name = "Apple Inc.", Exchange = "NASDAQ AAPL", Ticker = "AAPL", Isin = "US0378331005", Website = "http://www.apple.com" };
        }

        public static List<Company> GetCompanies()
        {
            return new List<Company> {
                new Company { Id = 1, Name = "Apple Inc.", Exchange = "NASDAQ AAPL", Ticker = "AAPL", Isin = "US0378331005", Website = "http://www.apple.com" },
                new Company { Id = 2, Name = "British Airways Plc", Exchange = "Pink Sheets", Ticker = "BAIRY", Isin = "US1104193065" },
                new Company { Id = 3,Name = "Heineken NV", Exchange = "Euronext Amsterdam", Ticker = "HEIA", Isin = "NL0000009165" },
                new Company { Id = 4, Name = "Panasonic Corp", Exchange = "Tokyo Stock Exchange", Ticker = "6752", Isin = "JP3866800000", Website = "http://www.panasonic.co.jp" },
                new Company { Id = 5, Name = "Porsche Automobil", Exchange = "Deutsche Börse", Ticker = "PAH3", Isin = "DE000PAH0038", Website = "https://www.porsche.com/" }
            };
        }

        public static List<CompanyDTO> GetCompaniesDTO()
        {
            return new List<CompanyDTO> {
                new CompanyDTO { Id = 1, Name = "Apple Inc.", Exchange = "NASDAQ AAPL", Ticker = "AAPL", Isin = "US0378331005", Website = "http://www.apple.com" },
                new CompanyDTO { Id = 2, Name = "British Airways Plc", Exchange = "Pink Sheets", Ticker = "BAIRY", Isin = "US1104193065"}
            };
        }
    }
}
