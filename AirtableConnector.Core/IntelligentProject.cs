using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirtableConnector.Core
{
    public class IntelligentProject
    {
        public int ProjectId { get; set; }
        public string City { get; set; }
        public int CostPerSqft { get; set; }
        public int SiteArea { get; set; }
        public int GrossFloorArea { get; set; }
        public int TotalCost { get; set; }
        
    }
}
