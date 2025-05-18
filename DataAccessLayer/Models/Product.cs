using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models;

public class Product
{        
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }
    
    public byte[] Image { get; set; }

    public ICollection<Order> Orders { get; } = new List<Order>();

    public ICollection<Part> Parts { get; } = new List<Part>();
    
    public string GetImageBase64()
    {
        if (Image == null || Image.Length == 0)
            return string.Empty;
    
        return $"data:image/jpeg;base64,{Convert.ToBase64String(Image)}";
    }
}

