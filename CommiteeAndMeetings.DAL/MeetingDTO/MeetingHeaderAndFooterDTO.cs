using CommiteeAndMeetings.DAL.MeetingDomains;
using System;
using System.Collections.Generic;

namespace CommiteeAndMeetings.DAL.MeetingDTO
{
    public class MeetingHeaderAndFooterDTO
    {
        public int Id { get; set; }
        public string TitleAR { get; set; }
        public string TitleEn { get; set; }
        public string Html { get; set; }
        public HeaderAndFooterType HeaderAndFooterType { get; set; }
        private string _HeaderAndFooterTypeString;
        public string HeaderAndFooterTypeString
        {
            get { return _HeaderAndFooterTypeString; }
            set { _HeaderAndFooterTypeString = HeaderAndFooterType.ToString(); }
        }
        public virtual List<Meeting_Meeting_HeaderAndFooterDTO> Meetings { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
    }
}
