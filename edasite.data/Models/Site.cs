using System;
using System.Collections.Generic;

namespace edasite.data.Models;

public partial class Site
{
    public int SiteNo { get; set; }

    public string? SiteAdi { get; set; }

    public string? Adres { get; set; }

    public string? Bilgi { get; set; }

    public virtual ICollection<Aidat> Aidats { get; } = new List<Aidat>();

}
