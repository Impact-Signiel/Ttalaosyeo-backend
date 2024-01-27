using System;
using System.Collections.Generic;

namespace signiel.Models;

public partial class TripTag
{
    public ulong Id { get; set; }

    public ulong Trip { get; set; }

    public string Key { get; set; } = null!;

    public string Value { get; set; } = null!;

    public virtual Trip TripNavigation { get; set; } = null!;
}
