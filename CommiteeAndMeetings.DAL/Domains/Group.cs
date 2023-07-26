using CommiteeAndMeetings.DAL.ModelInterfaces;
using CommiteeDatabase.Models.Domains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.DAL.Domains
{
    public class Group:_BaseEntity, IAuditableInsertNoRole, IAuditableUpdate, IAuditableDelete
    {
        public Group()
        {
            GroupUsers =new HashSet<GroupUsers>();
            TaskGroups =new HashSet<TaskGroups>();
        }
        [Key]
        public int GroupId { get; set; }
        [Required]
        [StringLength(maximumLength:250)]
        public string GroupNameAr { get; set; }
        [Required]
        [StringLength(maximumLength: 250)]
        public string GroupNameEn { get; set; }

        // one Group Created By One User
        [ForeignKey("CreatedByUser")]
        public int? CreatedBy { get; set; }
        public virtual User CreatedByUser { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        //one group has many users
        public virtual ICollection<GroupUsers> GroupUsers { get; set; }

        public virtual ICollection<TaskGroups> TaskGroups { get; set; }
      
    }
}
