using Biblio.Web.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Biblio.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoriaDao _categoriaDao;

        public CategoryController(ICategoriaDao categoriaDao)
        {
            _categoriaDao = categoriaDao;
        }
        // GET: CategoryController
        public async Task<IActionResult> Index()
        {
            var result = await _categoriaDao.GetAllAsync();

            if (result.IsSuccess)
            {
                return View(result.Data);
            }
            else
            {
                // Handle the error case, e.g., log the error and show an error message
                ModelState.AddModelError(string.Empty, result.Message);
                return View(new List<Categoria>()); // Return an empty list or handle as needed
            }
            return View();
        }

        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Categoria categoria)
        {
            try
            {
                var result = await _categoriaDao.AddAsync(categoria);

                if (result.IsSuccess)
                {
                    // Optionally, you can redirect to the Index or Details page after successful creation
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Handle the error case, e.g., log the error and show an error message
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View(categoria); // Return the view with the model to show validation errors
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


    }
}
