using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobbApi.Api.Client.DTOs
{
    public class JobSearchListDto
    {
        public List<JobSearchItemDto> Jobs { get; set; }
        public int TotalCount { get; set; }

    }

    public class JobSearchItemDto
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string CompanyName { get; set; }
        public int? CountryId { get; set; }

    }
}
