using System;
using System.Collections.Generic;

namespace signiel.Models;

public partial class TripSchedule
{
    public ulong Id { get; set; }

    public ulong Trip { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<TripDetail> TripDetails { get; set; } = new List<TripDetail>();

    public virtual Trip TripNavigation { get; set; } = null!;
}
