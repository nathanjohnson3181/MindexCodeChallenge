using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Models
{
    public class Compensation
    {
        public int CompensationId { get; set; }
        [JsonIgnore]
        public virtual Employee Employee { get; set; }
        public int Salary { get; set; }
        public DateTime EffectiveDate { get; set; }
    }

    public class CompensationViewModel
    {
        public int CompensationId { get; set; }
        public string EmployeeId { get; set; }
        public int Salary { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}


