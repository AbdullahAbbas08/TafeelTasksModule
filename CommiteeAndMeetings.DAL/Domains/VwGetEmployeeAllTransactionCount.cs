using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Keyless]
    public partial class VwGetEmployeeAllTransactionCount
    {
        [Required]
        [Column("username")]
        [StringLength(450)]
        public string Username { get; set; }
        public int? مسددة { get; set; }
        [Column("صورة ونسخة")]
        public int? صورةونسخة { get; set; }
        [Column("غير مستلمة")]
        public int? غيرمستلمة { get; set; }
        [Column("غير مستلمة - سرية")]
        public int? غيرمستلمةسرية { get; set; }
        [Column("قرارات وتعماميم")]
        public int? قراراتوتعماميم { get; set; }
        [Column(" مستلمة")]
        public int? مستلمة { get; set; }
        [Column(" متأخرة")]
        public int? متأخرة { get; set; }
    }
}
