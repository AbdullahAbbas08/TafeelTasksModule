using CommiteeAndMeetings.DAL.ModelInterfaces;
using CommiteeDatabase.Models.Domains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeAndMeetings.DAL.MeetingDomains
{
    [Table("MeetingHeaderAndFooters", Schema = "Meeting")]
    public class MeetingHeaderAndFooter : _BaseEntity, IAuditableInsertNoRole
    {
        public int Id { get; set; }
        public string TitleAR { get; set; }
        public string TitleEn { get; set; }
        public string Html { get; set; }
        public HeaderAndFooterType HeaderAndFooterType { get; set; }
        [NotMapped]
        private string _HeaderAndFooterTypeString;
        [NotMapped]
        public string HeaderAndFooterTypeString
        {
            get { return _HeaderAndFooterTypeString; }
            set { _HeaderAndFooterTypeString = HeaderAndFooterType.ToString(); }
        }
        public virtual List<Meeting_Meeting_HeaderAndFooter> Meetings { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
    }
    public enum HeaderAndFooterType
    {
        Header = 1,
        Footer = 2
    }
}
