using System;
using System.Collections.Generic;

namespace edasit.Data.Models;

public partial class Daire
{
    public int DaireNo { get; set; }

    public int? MetreKare { get; set; }

    public int? UserNo { get; set; }

    public virtual User? UserNoNavigation { get; set; }
}
