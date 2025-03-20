using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EMOC20250320.AppWebMVC.Models;

namespace EMOC20250320.AppWebMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly Test20250319DbContext _context;

        public ProductController(Test20250319DbContext context)
        {
            _context = context;
        }

        // GET: Product
        public async Task<IActionResult> Index(Product? product, int topRegistro = 10)
        {
            var query = _context.Products.AsQueryable();

            if (product != null)
            {
                if (!string.IsNullOrWhiteSpace(product.ProductName))
                    query = query.Where(s => s.ProductName.Contains(product.ProductName));
                if (!string.IsNullOrWhiteSpace(product.Description))
                    query = query.Where(s => s.Description.Contains(product.Description));
                if (product.BrandId > 0)
                    query = query.Where(s => s.BrandId == product.BrandId);
                if (product.WarehouseId > 0)
                    query = query.Where(s => s.WarehouseId == product.WarehouseId);
            }

            if (topRegistro > 0)
                query = query.Take(topRegistro);

            query = query.Include(p => p.Warehouse).Include(p => p.Brand);

            // Cargar listas asegurándonos de que no sean null
            var marcas = _context.Brands?.ToList() ?? new List<Brand>();
            marcas.Add(new Brand { BrandName = "SELECCIONAR", BrandId = 0 });

            var warehouses = _context.Warehouses?.ToList() ?? new List<Warehouse>();
            warehouses.Add(new Warehouse { WarehouseName = "SELECCIONAR", WarehouseId = 0 });

            // Usar los nombres correctos de las propiedades
            ViewData["WarehouseId"] = new SelectList(warehouses, "WarehouseName", "Notes", 0);
            ViewData["MarcaId"] = new SelectList(marcas, "BrandId", "BrandName", 0);

            return View(await query.ToListAsync());
        }


        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Warehouse)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName");
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseName", "Notes");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,Description,Price,WarehouseId,BrandId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName", product.BrandId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseName", "Notes", product.WarehouseId);
            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName", product.BrandId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseName", "Notes", product.WarehouseId);
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,Description,Price,WarehouseId,BrandId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName", product.BrandId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseName", "Notes", product.WarehouseId);
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Warehouse)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}

