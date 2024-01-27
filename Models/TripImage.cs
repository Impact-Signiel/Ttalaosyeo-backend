using System;
using System.Collections.Generic;

namespace signiel.Models;

public partial class TripImage
{
    public ulong Id { get; set; }

    public ulong Trip { get; set; }

    public string Image { get; set; } = null!;

    public virtual Trip TripNavigation { get; set; } = null!;
}
