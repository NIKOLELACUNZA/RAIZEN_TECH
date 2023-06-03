using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PRUEBA.Models;
using PRUEBA.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PRUEBA.ViewModels;

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
    public IActionResult Ordenes()
    {
        var ordenes = _context.ORDERs.ToList();
        return View(ordenes);
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

    System.Console.WriteLine(quantity);

    // Redirige al usuario a la vista del carrito
    return RedirectToAction("Carrito");
}

[Authorize(Roles = "Administrador,Cliente")]
    public async Task<IActionResult> Carrito()
{
    // Obtén el usuario por idUserDef
    string idUserDef = _userManager.GetUserId(User);
    USERS user = await _context.USERs.FirstOrDefaultAsync(u => u.idUserDef == idUserDef);

    // Obtén el carrito por el idUserDef
    CART cart = await _context.CARTs.FirstOrDefaultAsync(c => c.Users == user);

    // Obtén la lista de elementos del carrito que tienen el carrito específico
    List<CART_ITEM> listaCarrito = await _context.CART_ITEMs.Include(ci => ci.Product).Where(ci => ci.Cart == cart).ToListAsync();

    return View("Carrito", listaCarrito);
}


  public IActionResult PasarelaPagos(){
    return View();
  }

[HttpPost]
  public async Task<IActionResult> ProcesarPago()
{
    string idUserDef = _userManager.GetUserId(User);
    USERS user = await _context.USERs.FirstOrDefaultAsync(u => u.idUserDef == idUserDef);

    // Obtén el carrito por el idUserDef
    CART cart = await _context.CARTs.FirstOrDefaultAsync(c => c.Users == user);

    // Obtén la lista de elementos del carrito que tienen el carrito específico
    List<CART_ITEM> listaCarrito = await _context.CART_ITEMs.Include(ci => ci.Product).Where(ci => ci.Cart == cart).ToListAsync();

    listaCarrito.ForEach(e => System.Console.WriteLine(e.Product.Name));

    // Crear nuevo ORDER
    ORDER newOrder = new ORDER
    {
        // Inicializa los campos necesarios de tu objeto ORDER aquí
        Users = user,
        OrderDate = DateTime.UtcNow,
        // Otros campos...
    };
    _context.ORDERs.Add(newOrder);
    await _context.SaveChangesAsync();

    // Trasladar elementos del carrito a ítems del pedido
    foreach (var cartItem in listaCarrito)
    {
        ORDER_ITEM orderItem = new ORDER_ITEM
        {
            // Inicializa los campos necesarios de tu objeto ORDER_ITEM aquí
            Order = newOrder,
            Product = cartItem.Product,
            Quantity = cartItem.Quantity,
            // Otros campos...
        };
        _context.ORDER_ITEMs.Add(orderItem);
        
        // Eliminar el item del carrito
        _context.CART_ITEMs.Remove(cartItem);
    }

    // Finalmente, guardar los cambios en el contexto y redireccionar al usuario
    await _context.SaveChangesAsync();

    return RedirectToAction("Carrito");
}

  public async Task<IActionResult> Clientes()
  {
      var users = await _context.USERs.ToListAsync();

      return View(users);
  }

  public async Task<IActionResult> DetalleUsuario(int id)
{
    var user = await _context.USERs.FirstOrDefaultAsync(u => u.id == id);
    var orders = await _context.ORDERs.Where(o => o.Users.id == id).ToListAsync();

    if (user == null)
    {
        return NotFound();
    }

    var viewModel = new UserOrdersViewModel
    {
        User = user,
        Orders = orders
    };

    return View(viewModel);
}

}
