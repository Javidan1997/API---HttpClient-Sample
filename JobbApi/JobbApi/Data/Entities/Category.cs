using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobbApi.Data.Entities
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }
        public string Icon { get; set; }

        public List<Job> Jobs { get; set; }

    }
}
