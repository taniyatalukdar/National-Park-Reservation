using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    class Campsite
    {
       public int CampSiteID { get; set; }
       public int CampgroundId { get; set; }
       public int Site_Number { get; set; }
       public int Max_Occupancy { get; set; }
       public bool Accessible { get; set; }
       public int Max_Rv_Length { get; set; }
       public bool Utilities { get; set; }
    }
}
