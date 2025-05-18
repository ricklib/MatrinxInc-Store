using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models;

public class Part
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
    
    public byte[] Image { get; set; }

    public ICollection<Product> Products { get; } = new List<Product>();
    
    public string GetImageBase64()
    {
        if (Image == null || Image.Length == 0)
            return string.Empty;
    
        return $"data:image/jpeg;base64,{Convert.ToBase64String(Image)}";
    }
}

