using System;
using System.Collections.Generic;

namespace signiel.Models;

public partial class TripDetailImage
{
    public ulong Id { get; set; }

    public ulong Detail { get; set; }

    public string Image { get; set; } = null!;

    public virtual TripDetail DetailNavigation { get; set; } = null!;
}
