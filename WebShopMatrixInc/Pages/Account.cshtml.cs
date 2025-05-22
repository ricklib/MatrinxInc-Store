using Microsoft.AspNetCore.Mvc.RazorPages;
using DataAccessLayer.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace WebShopMatrixInc.Pages;

public class AccountModel : PageModel
{
    private readonly ILogger<ProductViewModel> _logger;
    private readonly ICustomerRepository _customerRepository;
    public int? CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public AccountModel(ILogger<ProductViewModel> logger, ICustomerRepository customerRepository)
    {
        _logger = logger;
        _customerRepository = customerRepository;
        CustomerId = -1;
    }
    
    public IActionResult OnGet()
    {
        CustomerId = HttpContext.Session.GetInt32("CustomerId") ?? -1;
        if (CustomerId == -1)
        {
            return Redirect("/Login");
        }
        else
        {
            Customer = _customerRepository.GetCustomerById(CustomerId.Value);
            return Page();
        }
    }
}