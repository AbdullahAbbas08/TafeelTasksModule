using System;

namespace CommiteeAndMeetings.DAL.Domains
{
    public class BusinessException : Exception
    {
        public BusinessException(string message)
            : base(message)
        {
        }
    }
}
