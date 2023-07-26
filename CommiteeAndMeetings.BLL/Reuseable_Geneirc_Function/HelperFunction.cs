using CommiteeAndMeetings.BLL.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.BLL.Reuseable_Geneirc_Function
{
    public class HelperFunction :IHelperFunction
    {
        protected readonly MasarContext _Context;

        public HelperFunction()
        {
            if (_Context == null)
            {
                _Context = new MasarContext();

            }
        }
        public bool get_Permission(int UserRoleId, string PermissionCode)
        {
            //using (var _context = new MainDbContext())
            //{

            var result = _Context.COUNTS.FromSqlInterpolated($"sp_CheckPermission {UserRoleId},{PermissionCode}").AsNoTracking().AsEnumerable().FirstOrDefault();
            //var result = this._Context.Database.ExecuteSqlRaw("exec [dbo].[sp_CheckPermission] @UserRoleId,@PermissionCode", parameters);

            if (result.CNT > 0)
            {
                return true;

            }
            else
            {
                return false;
            }
            //}
        }
    }
}
