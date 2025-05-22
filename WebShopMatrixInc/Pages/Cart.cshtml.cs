using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using WebShopMatrixInc;

public class CartViewModel : PageModel
{
    private readonly ILogger<CartViewModel>? _logger;
    private readonly ICustomerRepository? _customerRepository;
    private readonly IOrderRepository? _orderRepository;
    
    public List<Product> Cart { get; private set; }
    public decimal TotalPrice { get; private set; }
    public int CustomerID { get; set; }
    public bool PlaceOrderFailed { get; set; }
    public bool OrderPlaced { get; set; }

    public CartViewModel(ILogger<CartViewModel> logger,
                        ICustomerRepository customerRepository, 
                        IOrderRepository orderRepository)
    {
        _logger = logger;
        _customerRepository = customerRepository;
        _orderRepository = orderRepository;
        Cart = new List<Product>();
        CustomerID = -1;
        PlaceOrderFailed = false;
        OrderPlaced = false;
    }

    public void OnGet()
    {
        Cart = HttpContext.Session.GetObjectFromJson<List<Product>>("Cart") ?? new List<Product>();
        TotalPrice = Cart.Sum(x => x.Price);
    }

    public IActionResult OnPost()
    {
        HttpContext.Session.Remove("Cart");
        Cart = new List<Product>();
        TotalPrice = 0;
        return RedirectToPage();
    }

    public IActionResult OnPostCreateOrder(int id)
    {
        CustomerID = HttpContext.Session.GetInt32("CustomerId") ?? -1;

        if (CustomerID == -1)
        {
            return RedirectToPage("/login");
        }
        
        var customer = _customerRepository.GetCustomerById(CustomerID);

        if (customer == null)
        {
            PlaceOrderFailed = true;
            return RedirectToPage("/login");
        }

        Cart = HttpContext.Session.GetObjectFromJson<List<Product>>("Cart") ?? new List<Product>();
        decimal orderTotal = Cart.Sum(x => x.Price);

        PlaceOrderFailed = false;
        
        _orderRepository.AddOrder(new Order
        {
            Customer = customer, 
            OrderDate = DateTime.Now, 
            Total = orderTotal
        });
        
        OrderPlaced = true;
        
        HttpContext.Session.Remove("Cart");
        Cart = new List<Product>();
        TotalPrice = 0;
        
        return RedirectToPage("/orderhistory");
    }
}