using System;
using System.Collections.Generic;

namespace signiel.Models;

public partial class LandingCard
{
    public ulong Id { get; set; }

    public string Title { get; set; } = null!;

    public string Query { get; set; } = null!;

    public string Image { get; set; } = null!;

    public virtual ICollection<LandingCardTag> LandingCardTags { get; set; } = new List<LandingCardTag>();
}
