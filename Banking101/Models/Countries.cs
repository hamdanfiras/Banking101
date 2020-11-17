using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking101.Models
{
    public static class Countries
    {
        public static Dictionary<string, string> GetCountries()
        {
            return new Dictionary<string, string>
            {
                { "LB", "Lebanon"},
                { "FR", "France"},
                { "RU", "Russia"},
            };
        }

    }
}
