using System;
using System.Collections.Generic;

namespace signiel.Models;

public partial class LandingCardTag
{
    public ulong Id { get; set; }

    public ulong Card { get; set; }

    public string Text { get; set; } = null!;

    public virtual LandingCard CardNavigation { get; set; } = null!;
}
