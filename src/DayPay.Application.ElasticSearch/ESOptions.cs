using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayPay;

public class ESOptions
{
    public string Url { get; set; } = string.Empty;

    public string DefaultIndex { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public Dictionary<string, string> Indexes { get; set; }
}
