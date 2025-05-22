using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebShopMatrixInc.Pages;

public class OrderHistoryModel : PageModel
{
    private ILogger<OrderHistoryModel> _logger;
    private IOrderRepository _orderRepository;
    private ICustomerRepository _customerRepository;

    public int CustomerId { get; set; }
    public IEnumerable<Order> CustomerOrders { get; set; }

    public OrderHistoryModel(ILogger<OrderHistoryModel> logger, IOrderRepository orderRepository, ICustomerRepository customerRepository)
    {
        _logger = logger;
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
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
            CustomerOrders = _orderRepository.GetAllOrders().Where(o => o.CustomerId == CustomerId).ToList();
            return Page();
        }
    }
}