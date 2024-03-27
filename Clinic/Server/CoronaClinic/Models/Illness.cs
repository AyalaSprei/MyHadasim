using System;
using System.Collections.Generic;

namespace CoronaClinic.Models;

public partial class Illness
{
    public int IllnessId { get; set; }

    public int MemberId { get; set; }

    public DateTime PositiveDate { get; set; }

    public DateTime NegativeDate { get; set; }

    public virtual Member Member { get; set; } = null!;
}
