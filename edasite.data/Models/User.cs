using System;
using System.Collections.Generic;

namespace edasite.data.Models;

public partial class User
{
    public int UserNo { get; set; }

    public string? AdSoyad { get; set; }

    public string? Email { get; set; }

    public string? Rol { get; set; }

    public string? Telefon { get; set; }

    public string? Sifre { get; set; }

    public virtual ICollection<Daire> Daires { get; } = new List<Daire>();
}
