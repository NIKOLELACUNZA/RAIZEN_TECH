using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PRUEBA.Models;
using PRUEBA.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

    public IActionResult AboutUs()
    {
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

    public IActionResult Detalle(int id){
      PRODUCT producto = _context.PRODUCTs.Find(id);
      return View("DetalleProducto",producto)
      ;
    }

    [HttpPost]
public async Task<IActionResult> AddToCart(int productId, int quantity)
{
    // Obtén el usuario actual
    string idUserDef = _userManager.GetUserId(User);
    USERS user = await _context.USERs.FirstOrDefaultAsync(u => u.idUserDef == idUserDef);

    // Obtén el carrito del usuario
    CART cart = await _context.CARTs.FirstOrDefaultAsync(c => c.Users == user);

    // Si el carrito no existe, crea uno nuevo y guárdalo en la base de datos
    if (cart == null)
    {
        cart = new CART { Users = user };
        _context.CARTs.Add(cart);
        await _context.SaveChangesAsync();
    }

    // Obtén el producto por su ID
    PRODUCT product = await _context.PRODUCTs.FindAsync(productId);

    // Crea un nuevo objeto CART_ITEM con el producto y la cantidad proporcionada
    CART_ITEM cartItem = new CART_ITEM
    {
        Cart = cart,
        Product = product,
        Quantity = quantity
    };
    // Guarda el nuevo objeto CART_ITEM en la base de datos
    _context.CART_ITEMs.Add(cartItem);
    await _context.SaveChangesAsync();

    // Redirige al usuario a la vista del carrito
    return RedirectToAction("Carrito");
}


    public async Task<IActionResult> Carrito()
{
    // Obtén el usuario por idUserDef
    string idUserDef = _userManager.GetUserId(User);
    USERS user = await _context.USERs.FirstOrDefaultAsync(u => u.idUserDef == idUserDef);

    // Obtén el carrito por el idUserDef
    CART cart = await _context.CARTs.FirstOrDefaultAsync(c => c.Users == user);

    // Obtén la lista de elementos del carrito que tienen el carrito específico
    List<CART_ITEM> listaCarrito = await _context.CART_ITEMs.Include(ci => ci.Product).Where(ci => ci.Cart == cart).ToListAsync();

    System.Console.WriteLine(listaCarrito[1].Product);

    if (listaCarrito[1].Product == null)
    {
        System.Console.WriteLine("Error");
    }

    return View("Carrito", listaCarrito);
}




}
