using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

namespace WebShopMatrixInc.Pages;

public class ProductViewModel : PageModel
{
    private readonly ILogger<ProductViewModel> _logger;
    private readonly IProductRepository? _productRepository;
    
    [BindProperty]
    public int Amount { get; set; } = 1;

    public Product Product { get; private set; }

    public ProductViewModel(ILogger<ProductViewModel> logger, IProductRepository productService)
    {
        _logger = logger;
        _productRepository = productService;
    }

    public void OnGet(int id)
    {
        Product = _productRepository.GetProductById(id);
    }

    public IActionResult OnPost(int id)
    {
        var product = _productRepository.GetProductById(id);
        var cart = HttpContext.Session.GetObjectFromJson<List<Product>>("Cart") ?? new List<Product>();

        for (int i = 0; i < Amount; i++)
        {
            cart.Add(product);
        }
        
        HttpContext.Session.SetObjectAsJson("Cart", cart);
        
        _logger.LogInformation($"Added {Amount} of {product.Name} to cart");

        return RedirectToPage(new { id = id });
    }
}