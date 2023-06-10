using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PRUEBA.Models;
using PRUEBA.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PRUEBA.ViewModels;
using iTextSharp.text.pdf;
using iTextSharp.text;
using OpenAI.GPT3;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels;

namespace PRUEBA.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    private readonly UserManager<IdentityUser> _userManager;

    private readonly IConfiguration _configuration;

    

    public HomeController(ApplicationDbContext context, UserManager<IdentityUser> userManager,IConfiguration configuration)
    {
        _context=context;
        _userManager=userManager;
    }

    public async Task<IActionResult> IndexAsync()
    {
      Console.WriteLine("Order Date (Local): " + DateTime.Now.ToString());

      string apiKey = _configuration["API_KEY"];
        
        var gpt3 = new OpenAIService(new OpenAiOptions()
        {
            ApiKey = apiKey
        });

      var completionResult = await gpt3.Completions.CreateCompletion(new CompletionCreateRequest()
      {
          Prompt = "Dame un titulo para mi empresa ecommerce de Raizen Tech sobre ventas de productos tecnologicos",
          Model = OpenAI.GPT3.ObjectModels.Models.TextDavinciV2,
          Temperature = 0.5F,
          MaxTokens = 100
      });

      if (completionResult.Successful)
      {
          foreach (var choice in completionResult.Choices)
          {
              ViewBag.Message = choice.Text;
          }                
      }
      else
      {
          if (completionResult.Error == null)
          {
              throw new Exception("Unknown Error");
          }
          Console.WriteLine($"{completionResult.Error.Code}: {completionResult.Error.Message}");
      }
      

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
  public async Task<IActionResult> ProcesarPago(string shipping)
{
  System.Console.WriteLine(shipping);
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
        OrderDate=DateTime.UtcNow,
        Shipping_Address=shipping,
    };
    _context.ORDERs.Add(newOrder);
    await _context.SaveChangesAsync();

    decimal nuevoTotal=0;
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
        nuevoTotal+=cartItem.Product.Price * cartItem.Quantity;
        
    }

    newOrder.Total_Amount = nuevoTotal;
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


  [HttpGet]
  public IActionResult DescargarFactura(int orderId)
  {
      var order = _context.ORDERs.Include(o => o.Users).FirstOrDefault(o => o.id == orderId);
    if (order == null)
    {
        return NotFound();
    }

    using (var memoryStream = new MemoryStream())
    {
        Document doc = new Document(PageSize.A4);
        PdfWriter writer = PdfWriter.GetInstance(doc, memoryStream);

        doc.Open();

        // Agregamos metadatos al documento
        doc.AddTitle("Factura de Compra");
        doc.AddCreator("MiEmpresa");

        // Agregamos el contenido de la factura
        doc.Add(new Paragraph("Factura de Compra"));
        doc.Add(new Paragraph($"Fecha: {order.OrderDate}"));
        doc.Add(new Paragraph($"Nombre: {order.Users.Name}"));
        doc.Add(new Paragraph($"Dirección: {order.Shipping_Address ?? order.Users.Address}"));
        doc.Add(new Paragraph("Detalles de Compra: "));

        var orderItems = _context.ORDER_ITEMs
                     .Include(oi => oi.Product)  // Esto cargará los productos relacionados
                     .Where(oi => oi.Order.id == order.id)
                     .ToList();

        doc.Add(new Paragraph("/n"));
        // Crear una tabla con 5 columnas (producto, cantidad, precio unitario, subtotal)
        PdfPTable table = new PdfPTable(4);
        table.WidthPercentage = 100; //Establecer el ancho de la tabla al 100% del documento

        // Agregar cabeceras de la tabla
        table.AddCell("Producto");
        table.AddCell("Cantidad");
        table.AddCell("Precio Unitario");
        table.AddCell("Subtotal");
        // Rellenar la tabla con los detalles de los productos
        foreach (var item in orderItems)
        {
            table.AddCell(item.Product.Name);
            table.AddCell(item.Quantity.ToString());
            table.AddCell(item.Product.Price.ToString());
            table.AddCell((item.Quantity * item.Product.Price).ToString());
        }

        // Añadir la tabla al documento
        doc.Add(table);

        doc.Add(new Paragraph($"Total: {order.Total_Amount}"));
        doc.Close();

        var bytes = memoryStream.ToArray();
        return File(bytes, "application/pdf", $"Factura-{order.id}.pdf");
    }
  }
}
