using System;
using System.Collections.Generic;

namespace CoronaClinic.Models;

public partial class Immune
{
    public int ImmuneId { get; set; }

    public int MemberId { get; set; }

    public DateTime Date { get; set; }

    public int CreatorId { get; set; }

    public virtual Creator Creator { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;
}
