﻿using Client.Models.DTO;
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
        private string _apiURL = "https://localhost:7001/api/";

        public CompanyAPIController()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_apiURL)
            };
        }

        // GET: Company
        public async Task<ActionResult> Index()
        {
          

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("Company");
                if (response.IsSuccessStatusCode)
                {
                    string jsonData = await response.Content.ReadAsStringAsync();
                    var companies = JsonConvert.DeserializeObject<List<CompanyDTO>>(jsonData);
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
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"Company/{id}");
                if (response.IsSuccessStatusCode)
                {
                    string jsonData = await response.Content.ReadAsStringAsync();
                    var company = JsonConvert.DeserializeObject<CompanyDTO>(jsonData);

                   
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
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to delete company.");
                    return View("Error");
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