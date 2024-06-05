using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Backend.Model;
using System.Text;

namespace Frontned.Controllers
{
    public class EmployeeController : Controller
    {
        string BaseUrl = "http://ec2-13-60-21-217.eu-north-1.compute.amazonaws.com:8080/";
        // GET: Employee
        public async Task<ActionResult> Index()
        {
            List<Employee> ProdInfo = new List<Employee>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage Res = await client.GetAsync("api/Employee");

                if (Res.IsSuccessStatusCode)
                {
                    var PrResponse = Res.Content.ReadAsStringAsync().Result;

                    ProdInfo = JsonConvert.DeserializeObject<List<Employee>>(PrResponse);
                }
            }
            return View(ProdInfo);
        }

        // GET: Employee/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var employee = new Employee();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync($"api/Employee/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    employee = JsonConvert.DeserializeObject<Employee>(responseContent);
                }
            }
            return View(employee);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        public async Task<ActionResult> Create(Employee viewModel)
        {
            var employee = new Employee
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Job = viewModel.Job,
                Salary = viewModel.Salary
            };


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var customerJson = JsonConvert.SerializeObject(employee);
                var content = new StringContent(customerJson, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("api/Employee", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index"); // Redirect to the employee list
                }
                else
                {
                    return View("Error");
                }
            }
                

            return View(viewModel);
        }

        // GET: Employee/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var employee = new Employee();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync($"api/Employee/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    employee = JsonConvert.DeserializeObject<Employee>(responseContent);
                }
            }
                
            return View(employee);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, Employee viewModel)
        {
            var employee = new Employee
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Job = viewModel.Job,
                Salary = viewModel.Salary
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Serialize the modified customer object to JSON and send it in the request body
                var customerJson = JsonConvert.SerializeObject(employee);
                var content = new StringContent(customerJson, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"api/Employee/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index"); // Redirect to the employee list or another appropriate action
                }
            }
                
            // Handle the case where the update failed or ModelState is not valid
            return View(employee);
        }

        // GET: Employee/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var employee = new Employee();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync($"api/Employee/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    employee = JsonConvert.DeserializeObject<Employee>(responseContent);
                }

            }

            return View(employee);
        }

        //  POST: Employee/Delete/5 
        [HttpPost]
        public async Task<ActionResult> Delete(int id, FormCollection collection)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.DeleteAsync($"api/Employee/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index"); // Redirect to the employee list or another appropriate action
                }
                else
                {
                    return View("Error");
                }
            }
            
        }
    }
}
