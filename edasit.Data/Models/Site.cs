using System;
using System.Collections.Generic;

namespace edasit.Data.Models;

public partial class Site
{
    public int SiteNo { get; set; }

    public string? SiteAdi { get; set; }

    public string? Adres { get; set; }

    public string? Bilgi { get; set; }

    public virtual ICollection<Aidat> Aidats { get; } = new List<Aidat>();

    public virtual ICollection<Blok> Bloks { get; } = new List<Blok>();
}
