using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.BLL.Reuseable_Geneirc_Function
{
    public interface IHelperFunction
    {
        bool get_Permission(int UserRoleId, string PermissionCode);
    }
}
