using CraftyShop.Models;
using CraftyShop.Repositories.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CraftyShop.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyRepository _companyRepo;
        public CompanyController(ICompanyRepository companyRepo)
        {
            _companyRepo = companyRepo;
        }
        public IActionResult Index()
        {
            IEnumerable<Company> companies = _companyRepo.GetAll().ToList();
            return View(companies);
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                return View(new Company());
            }
            Company? company = await _companyRepo.Get(x => x.Id == id.Value);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(Company company)
        {
            if (!ModelState.IsValid)
            {
                return View(company);
            }
            if (company.Id == 0)
            {
                await _companyRepo.Add(company);
                TempData["success"] = "Product was created successfully";
            }
            else
            {
                await _companyRepo.Update(company);
                TempData["success"] = "Product was updated successfully";
            }
            return RedirectToAction("Index");
        }

        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new {data = _companyRepo.GetAll().ToList()});
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            Company? company = await _companyRepo.Get(u => u.Id == id.Value);
            if (company == null)
            {
                return NotFound();
            }
            await _companyRepo.Remove(company);
            return NoContent();
        }
        #endregion
    }
}
