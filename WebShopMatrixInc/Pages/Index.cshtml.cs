using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IProductRepository? _productRepository;
    private readonly IPartRepository? _partRepository;
    [BindProperty]
    public string SelectedCategory { get; set; } = "All";

    public IEnumerable<Product> Products { get; set; } = new List<Product>();
    public IEnumerable<Part> Parts { get; set; } = new List<Part>();


    public IndexModel(ILogger<IndexModel> logger, IProductRepository productRepository, IPartRepository partRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
        _partRepository = partRepository;
    }

    public void OnGet()
    {
        Products = _productRepository.GetAllProducts();
        Parts = _partRepository.GetAllParts();
    }
    
    public IActionResult OnPost()
    {
        _logger.LogInformation("Category selected: " + SelectedCategory);
        
        if (SelectedCategory == "All")
        {
            Products = _productRepository.GetAllProducts();
            return Page();      
        }
        
        Products = _productRepository.GetAllProducts().Where(p => p.Category == SelectedCategory);
        return Page();
    }
}