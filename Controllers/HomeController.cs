using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PRUEBA.Models;
using PRUEBA.Data;

namespace PRUEBA.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context=context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Catalogo(){
      var productos = _context.PRODUCTs.ToList();
      return View(productos);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(PRODUCT product)
    {
        _context.PRODUCTs.Add(product);
        _context.SaveChanges();
        return RedirectToAction("Catalogo");
    }

public IActionResult Delete(int id)
    {
        var persona = _context.PRODUCTs.Find(id);
        return View(persona);
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        System.Console.WriteLine(id);
        var persona = _context.PRODUCTs.Find(id);
        _context.PRODUCTs.Remove(persona);
        _context.SaveChanges();
        return RedirectToAction("Catalogo");
    }
}
