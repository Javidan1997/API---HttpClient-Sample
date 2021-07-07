using JobbApi.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobbApi.Api.Client.DTOs
{
    public class JobFilterListDto
    {
        public List<JobFilterItemDto> Jobs { get; set; }
        public int TotalCount { get; set; }

    }

    public class JobFilterItemDto
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public decimal? Experience { get; set; }
        public Qualification? Qualification { get; set; }
        public Gender? Gender { get; set; }
        public JobType? JobType { get; set; }
        public int? CategoryId { get; set; }
        public int? CountryId { get; set; }

    }
}
