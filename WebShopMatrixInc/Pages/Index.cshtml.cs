using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

public class IndexModel : PageModel
{
    private readonly IProductRepository? _productRepository;
    private readonly IPartRepository? _partRepository;

    public IEnumerable<Product> Products { get; set; } = new List<Product>();
    public IEnumerable<Part> Parts { get; set; } = new List<Part>();


    public IndexModel(IProductRepository productRepository, IPartRepository partRepository)
    {
        _productRepository = productRepository;
        _partRepository = partRepository;
    }

    public void OnGet()
    {
        Products = _productRepository.GetAllProducts();
        Parts = _partRepository.GetAllParts();
    }
}