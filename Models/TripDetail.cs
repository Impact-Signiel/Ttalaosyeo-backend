using System;
using System.Collections.Generic;

namespace signiel.Models;

public partial class TripDetail
{
    public ulong Id { get; set; }

    public ulong Trip { get; set; }

    public ulong Schedule { get; set; }

    public string Location { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual TripSchedule ScheduleNavigation { get; set; } = null!;

    public virtual ICollection<TripDetailImage> TripDetailImages { get; set; } = new List<TripDetailImage>();

    public virtual Trip TripNavigation { get; set; } = null!;
}
