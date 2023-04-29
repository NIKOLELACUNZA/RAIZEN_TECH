using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PRUEBA.Models;
using PRUEBA.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace PRUEBA.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    private readonly UserManager<IdentityUser> _userManager;

    public HomeController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context=context;
        _userManager=userManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Privacy()
    {
      // Debug
      var user = await _userManager.GetUserAsync(User);
      var roles = await _userManager.GetRolesAsync(user);
      var role = roles.FirstOrDefault();
      System.Console.WriteLine($"Rol del usuario: {role}");
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }



    [Authorize(Roles = "Administrador,Cliente")]
    public IActionResult Catalogo(){
      var productos = _context.PRODUCTs.ToList();
      return View(productos);
    }

    [Authorize(Roles = "Administrador")]
      public IActionResult Create()
      {
          return View();
      }

    [Authorize(Roles = "Administrador")]
      [HttpPost]
      public IActionResult Create(PRODUCT product)
      {

          if(ModelState.IsValid){
            _context.PRODUCTs.Add(product);
            _context.SaveChanges();
            return RedirectToAction("Catalogo");
          }
          return View("Create");
          
      }

[Authorize(Roles = "Administrador")]
    public IActionResult Delete(int id)
    {
        var persona = _context.PRODUCTs.Find(id);
        return View(persona);
    }

[Authorize(Roles = "Administrador")]
    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        System.Console.WriteLine(id);
        var persona = _context.PRODUCTs.Find(id);
        _context.PRODUCTs.Remove(persona);
        _context.SaveChanges();
        return RedirectToAction("Catalogo");
    }

[Authorize(Roles = "Administrador")]
public IActionResult Edit(int id)
    {
        var producto = _context.PRODUCTs.Find(id);
        return View(producto);
    }
    
[Authorize(Roles = "Administrador")]
    [HttpPost]
    public IActionResult Edit(PRODUCT producto)
    {
        _context.PRODUCTs.Update(producto);
        _context.SaveChanges();
        return RedirectToAction("Catalogo");
    }


}
