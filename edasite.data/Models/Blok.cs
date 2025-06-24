using System;
using System.Collections.Generic;

namespace edasite.data.Models;

public partial class Blok
{
    public int BlokNo { get; set; }

    public string? BlokAdi { get; set; }

    public int? KatSayisi { get; set; }

    public int? SiteNo { get; set; }

    public virtual Site? SiteNoNavigation { get; set; }

    public virtual ICollection<Site> Sites { get; } = new List<Site>();
}
