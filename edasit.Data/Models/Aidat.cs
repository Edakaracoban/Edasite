using System;
using System.Collections.Generic;

namespace edasit.Data.Models;

public partial class Aidat
{
    public int AidatNo { get; set; }

    public int? Donem { get; set; }

    public decimal? Tutar { get; set; }

    public string? OdemeDurumu { get; set; }

    public int? SiteNo { get; set; }

    public virtual Site? SiteNoNavigation { get; set; }
}
