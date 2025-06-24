using System;
using System.Collections.Generic;

namespace edasit.Data.Models;

public partial class Blok
{
    public int BlokNo { get; set; }

    public string? BlokAdi { get; set; }

    public int? KatSayisi { get; set; }

    public int? SiteNo { get; set; }

    public virtual Site? SiteNoNavigation { get; set; }
}
