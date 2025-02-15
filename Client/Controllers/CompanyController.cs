using Client.Models.DTO;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Client.Controllers
{
    public class CompanyAPIController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;
        private readonly string _companyCacheKey = "companies";
        private string _apiURL = "https://localhost:7001/api/";

        public CompanyAPIController()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_apiURL) // Change this to your actual API URL
            };
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        // GET: Company
        public async Task<ActionResult> Index()
        {
            if (_memoryCache.TryGetValue(_companyCacheKey, out List<CompanyDTO> cachedCompanies))
            {
                return View(cachedCompanies);
            }

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("Company");
                if (response.IsSuccessStatusCode)
                {
                    string jsonData = await response.Content.ReadAsStringAsync();
                    var companies = JsonConvert.DeserializeObject<List<CompanyDTO>>(jsonData);

                    // Store the list in cache for 5 minutes
                    _memoryCache.Set(_companyCacheKey, companies, TimeSpan.FromMinutes(5));
                    return View(companies);
                }
                else
                {
                    ViewBag.ErrorMessage = "Error retrieving company data.";
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Exception: " + ex.Message;
                return View("Error");
            }
        }
        // Get Company
        public async Task<ActionResult> Create()
        {
            return View(new CompanyDTO());
        }
        // POST: Company/Create
        [HttpPost]
        public async Task<ActionResult> Create(CompanyDTO companyDto)
        {
            try
            {
                string jsonData = JsonConvert.SerializeObject(companyDto);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("Company", content);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                ModelState.AddModelError("", "Failed to create company.");
                return View(companyDto);
            }
            catch
            {
                return View("Error");
            }
        }

        // Get: Company/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            // Try to retrieve the specific company from the cache using the company id as key
            var companyCacheKey = $"{_companyCacheKey}_{id}";
            if (_memoryCache.TryGetValue(companyCacheKey, out CompanyDTO cachedCompany))
            {
                return View(cachedCompany);
            }

            // If the company is not found in the cache, fetch it from the source
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"Company/{id}");
                if (response.IsSuccessStatusCode)
                {
                    string jsonData = await response.Content.ReadAsStringAsync();
                    var company = JsonConvert.DeserializeObject<CompanyDTO>(jsonData);

                    // Cache the company object with its ID as part of the key for 5 minutes
                    _memoryCache.Set(companyCacheKey, company, TimeSpan.FromMinutes(5));
                    return View(company);
                }
                else
                {
                    ViewBag.ErrorMessage = "Error retrieving company data.";
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Exception: " + ex.Message;
                return View("Error");
            }
        }

        // POST: Company/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(CompanyDTO companyDto)
        {
            try
            {
                string jsonData = JsonConvert.SerializeObject(companyDto);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PutAsync($"Company/{companyDto.Id}", content);
                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    ModelState.AddModelError("", "Failed to update company.");
                    return View(companyDto);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: Company/Delete/5  (Your Delete Confirmation Page)
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"Company/{id}");

                if (response.IsSuccessStatusCode)
                {
                    string jsonData = await response.Content.ReadAsStringAsync();
                    var company = JsonConvert.DeserializeObject<CompanyDTO>(jsonData);

                    if (company == null)
                    {
                        return HttpNotFound();
                    }

                    return View(company);
                }
                else
                {
                    ViewBag.ErrorMessage = "Error retrieving company data.";
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Exception: " + ex.Message;
                return View("Error");
            }
        }

        // POST: Company/Delete/5 (Handles the actual delete operation)
        [HttpPost, ActionName("Delete")] // ActionName("Delete") is important
        [ValidateAntiForgeryToken] // For security (prevents cross-site request forgery)
        public async Task<ActionResult> DeleteConfirmed(int id) // Different action name as Delete is reserved
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"Company/{id}");

                if (response.IsSuccessStatusCode)
                {
                    _memoryCache.Remove(_companyCacheKey); // Clear cache
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to delete company.");
                    return View("Error"); // Or redirect back to the Delete confirmation page
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Exception: " + ex.Message;
                return View("Error");
            }
        }
    }
}