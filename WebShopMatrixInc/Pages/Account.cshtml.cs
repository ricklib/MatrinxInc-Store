using Microsoft.AspNetCore.Mvc.RazorPages;
using DataAccessLayer.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;

namespace WebShopMatrixInc.Pages;

public class AccountModel : PageModel
{
    private readonly ILogger<ProductViewModel> _logger;
    private readonly CustomerRepository _customerRepository;
    public Customer? Customer { get; set; } = new Customer();

    public AccountModel(ILogger<ProductViewModel> logger, CustomerRepository customerRepository)
    {
        _logger = logger;
        _customerRepository = customerRepository;
    }
    
    public void OnGet()
    {
        try
        {
            Customer = _customerRepository.GetCustomerById(5);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
    }
}