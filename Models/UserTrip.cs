using System;
using System.Collections.Generic;

namespace signiel.Models;

public partial class UserTrip
{
    public ulong Id { get; set; }

    public ulong User { get; set; }

    public ulong Trip { get; set; }

    public DateOnly Date { get; set; }

    public virtual Trip TripNavigation { get; set; } = null!;

    public virtual User UserNavigation { get; set; } = null!;
}
