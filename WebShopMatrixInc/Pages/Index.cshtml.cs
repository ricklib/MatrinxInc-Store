using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

public class IndexModel : PageModel
{
    private readonly IProductRepository _productRepository;

    public IEnumerable<Product> Products { get; set; } = new List<Product>();

    public IndexModel(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public void OnGet()
    {
        Products = _productRepository.GetAllProducts();
    }
    public string GetImageBase64(byte[] imageData)
    {
        if (imageData == null || imageData.Length == 0)
            return string.Empty;
    
        return $"data:image/jpeg;base64,{Convert.ToBase64String(imageData)}";
    }
}