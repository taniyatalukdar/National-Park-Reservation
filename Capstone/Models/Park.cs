﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
     public class Park
    {
       
            public int Id { get; set; }
            public string Name { get; set; }
            public string Location { get; set; }
            public DateTime Establish_Date { get; set; }
            public int Area { get; set; }
            public int Visitors { get; set; }
            public string Description { get; set; }

        
    }

}

