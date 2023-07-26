using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("WorkFlowFilter")]
    [Index(nameof(WorkFlowProcessId), Name = "IX_WorkFlowFilter_WorkFlowProcessId")]
    public partial class WorkFlowFilter
    {
        [Key]
        public int WorkFlowFilterId { get; set; }
        public int WorkFlowProcessId { get; set; }
        public int ObjectId { get; set; }
        public string ObjectName { get; set; }

        [ForeignKey(nameof(WorkFlowProcessId))]
        [InverseProperty("WorkFlowFilters")]
        public virtual WorkFlowProcess WorkFlowProcess { get; set; }
    }
}
