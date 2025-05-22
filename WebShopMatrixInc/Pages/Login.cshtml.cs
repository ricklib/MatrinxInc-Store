using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebShopMatrixInc.Pages;

public class LoginModel : PageModel
{
    private readonly ILogger<ProductViewModel> _logger;
    private readonly ICustomerRepository _customerRepository;
    
    [BindProperty]
    public int? CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public bool InvalidId { get; set; }
    
    public LoginModel(ILogger<ProductViewModel> logger, ICustomerRepository customerRepository)
    {
        _logger = logger;
        _customerRepository = customerRepository;
        InvalidId = false;
    }

    public void OnGet()
    {
        
    }

    public IActionResult OnPost()
    {
        Customer = _customerRepository.GetCustomerById(CustomerId.Value);

        if (Customer == null)
        {
            InvalidId = true;
            return Page();
        }
        
        InvalidId = false;
        
        HttpContext.Session.SetInt32("CustomerId", Customer.Id);
        
        return RedirectToPage("/account");
    }
}