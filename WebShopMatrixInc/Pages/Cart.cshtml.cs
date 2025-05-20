using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

namespace WebShopMatrixInc.Pages;

public class CartViewModel : PageModel
{
    private readonly ILogger<CartViewModel>? _logger;
    private readonly IProductRepository? _productRepository;
    public decimal TotalPrice { get; private set; }
    public List<Product> Cart { get; private set; }

    public Product Product { get; private set; }

    public CartViewModel(ILogger<CartViewModel> logger, IProductRepository productService)
    {
        _logger = logger;
        _productRepository = productService;
    }

    public void OnGet(int id)
    {
        Cart = HttpContext.Session.GetObjectFromJson<List<Product>>("Cart") ?? new List<Product>();
        TotalPrice = Cart.Sum(x => x.Price);
    }
}