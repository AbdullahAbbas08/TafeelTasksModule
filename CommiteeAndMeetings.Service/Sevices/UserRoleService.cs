using AutoMapper;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.BLL.Hosting;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using DbContexts.MasarContext.ProjectionModels;
using IHelperServices;
using IHelperServices.Models;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class UserRoleService:BusinessService<UserRole, UserRoleDTO>, IUserRoleService
    {

        private readonly IMailServices _MailServices;
        private readonly IHelperServices.ISessionServices _sessionServices;
        public UserRoleService(
          IUnitOfWork unitOfWork,
          IMailServices MailServices,
          IMapper mapper, ISecurityService securityService,
          IStringLocalizer stringLocalizer,
          IHelperServices.ISessionServices sessionServices,
          IOptions<AppSettings> appSettings)

          : base(unitOfWork, mapper, stringLocalizer,securityService, sessionServices,appSettings)
        {
            _MailServices = MailServices;
            _sessionServices = sessionServices;

        }

        public bool InsertUserRoleFromUser(IEnumerable<UserRoleDTO> entities)
        {
            //var setting = _UnitOfWork.GetRepository<SystemSetting>().GetAll().Where(x => x.SystemSettingCode.ToUpper() == "AddUserRoleRequest").FirstOrDefault().SystemSettingValue;

            foreach (var enttiy in entities)
            {
                // If UserRoleRequest has right to make role for selected user OR The loggedin user is the same user that u want to make role to him
               
                    return InsertUserRole(enttiy);
                


                // If UserRoleRequest has NOT right to make role for selected user only if this user Agree to Role this Job
               
            }

            return true;
        }

        public bool InsertUserRole(UserRoleDTO enttiy)
        {
            var userRole = _UnitOfWork.GetRepository<UserRole>().GetAll().Where(u => u.OrganizationId == enttiy.OrganizationId && u.UserId == enttiy.UserId && u.RoleId == enttiy.RoleId).ToList();
            // check if role is Committe
            var role = _UnitOfWork.GetRepository<Role>().GetAll().Where(c => c.RoleId == enttiy.RoleId).FirstOrDefault();

            if (userRole != null && userRole.Count > 0)
            {
                if (role.IsCommitteRole)
                {
                    userRole = _UnitOfWork.GetRepository<UserRole>().GetAll().Where(u => u.UserId == enttiy.UserId && u.RoleId == enttiy.RoleId).ToList();
                    if (userRole != null && userRole.Count > 0)
                    {
                        return false;
                    }
                }

                enttiy.UserRoleId = userRole.FirstOrDefault().UserRoleId;
                enttiy.EnabledSince = enttiy.EnabledSince != null ? DateTimeOffset.Parse(((DateTimeOffset)enttiy.EnabledSince).ToString("yyyy/MM/dd")) : enttiy.EnabledSince;
                enttiy.EnabledUntil = enttiy.EnabledUntil != null ? DateTimeOffset.Parse(((DateTimeOffset)enttiy.EnabledUntil).ToString("yyyy/MM/dd")) : enttiy.EnabledUntil;
                var _UserRoles = base.Update(new List<UserRoleDTO>() { enttiy });
                base._UnitOfWork.SaveChangesAsync();
                // Send Mail Notification To User
                SendMail(_UserRoles);

            }

            else
            {
                if (role.IsCommitteRole)
                {
                    userRole = _UnitOfWork.GetRepository<UserRole>().GetAll().Where(u => u.UserId == enttiy.UserId && u.RoleId == enttiy.RoleId).ToList();
                    if (userRole != null && userRole.Count > 0)
                    {
                        return false;
                    }
                }
                enttiy.EnabledSince = enttiy.EnabledSince != null ? DateTimeOffset.Parse(((DateTimeOffset)enttiy.EnabledSince).ToString("yyyy/MM/dd")) : enttiy.EnabledSince;
                enttiy.EnabledUntil = enttiy.EnabledUntil != null ? DateTimeOffset.Parse(((DateTimeOffset)enttiy.EnabledUntil).ToString("yyyy/MM/dd")) : enttiy.EnabledUntil;
                var _UserRoles = base.Insert(new List<UserRoleDTO>() { enttiy });
                base._UnitOfWork.SaveChangesAsync();

                // Send Mail Notification To User
                SendMail(_UserRoles);
            }

            return true;
        }

        public void InserUserRoleRequest(UserRoleDTO entity)
        {
            UserRoleRequest UserRoleRequest = new UserRoleRequest();

            UserRoleRequest.UserRoleRequestId = entity.UserRoleId;
            UserRoleRequest.UserId = entity.UserId;
            UserRoleRequest.RoleId = entity.RoleId;
            UserRoleRequest.OrganizationId = entity.OrganizationId;
            UserRoleRequest.RoleOverridesUserPermissions = entity.RoleOverridesUserPermissions;
            UserRoleRequest.EnabledSince = entity.EnabledSince != null ? DateTimeOffset.Parse(((DateTimeOffset)entity.EnabledSince).ToString("yyyy/MM/dd")) : entity.EnabledSince; ;
            UserRoleRequest.EnabledUntil = entity.EnabledUntil != null ? DateTimeOffset.Parse(((DateTimeOffset)entity.EnabledUntil).ToString("yyyy/MM/dd")) : entity.EnabledUntil;
            UserRoleRequest.Notes = entity.Notes;
            UserRoleRequest.UserRoleStatus = "Pending";


            _UnitOfWork.GetRepository<UserRoleRequest>().Insert(UserRoleRequest);
        }

        public void SendMail(UserRoleDTO _UserRole)
        {
            var _User = _UnitOfWork.GetRepository<User>().GetById(_UserRole.UserId);
            var _Organization = _UnitOfWork.GetRepository<Organization>().GetById(_UserRole.OrganizationId);
            var _Role = _UnitOfWork.GetRepository<Role>().GetById(_UserRole.RoleId);

            string Email_style = @"
                                        text-align: center;
                                        flex-direction: column;
                                        justify-content: center;
                                        align-items: center;";

            string Email_image_style = @" 
                                                margin: auto;
                                                display: center;
                                                justify-content: center;
                                                margin: 32px;
                                               ";
            string image_style = @"
                                            text-align: center;
                                            width: 90px;
                                            margin: 0;";
            //margin-left: 16px;

            string table_style = @"
                                            width: 810px;
                                            justify-content: center;
                                            margin-bottom: -5px;
                                            direction: rtl;
                                            border: 1px solid #cccccc;
                                            display: table;
                                            border-collapse: separate;
                                            border-spacing: 2px;
                                            border-color: grey;";

            string tr_style = @" 
                                            white-space: normal;
                                            line-height: normal;
                                            font-weight: normal;
                                            font-size: medium;
                                            font-style: normal;
                                            color: -internal-quirk-inherit;
                                            text-align: start;
                                            direction: rtl;
                                            font-variant: normal;";


            string td_style = @"
                                            border: 1px solid #cccccc;
                                            border-bottom: 1px solid #EEE;
                                            padding: 10px;
                                            width: 3px;
                                            margin-bottom: -3px;
                                            font-weight: 600;
                                            margin: -6px;";

            string _Mail = $@"
                                           
                                           <div style='{Email_style}'>
                                           <img style='{Email_image_style}' src='cid:TopHeader'>
                                           <div style='
                                                width: 810px;
                                                display: flex;
                                                flex-direction: column;
                                                justify-content: center;
                                                align-items: center;
                                                margin: 0;
                                                padding: 0;'>
                                             <table style='{table_style}'>
                                            <tr style='{tr_style}'>
                                                <td colspan='4' style='{td_style}'>
                                                <h3 style='
                                                text-align: center;
                                                background: #13817E;
                                                padding: 9px 0;
                                                font-weight: 900;
                                                margin-bottom: 0px;
                                                color: #fff;'>{_stringLocalizer["EmailHeader"]}</h3></td>
                                            </tr>
                                            <tr style='{tr_style}'>
                                                <td style='{td_style}'>{_stringLocalizer["Corporation"]} </td>
                                                <td style='{td_style}'>{_Organization.OrganizationNameAr}</td>
                                                 <td style='{td_style}'>{_Organization.OrganizationNameEn}</td>
                                                <td style='{td_style}'> Corporation</td>
                                            </tr>
                                            <tr style='{tr_style}'>
                                                <td style='{td_style}'>{_stringLocalizer["Authority"]}</td>
                                                <td style='{td_style}'>{_Role.RoleNameAr}</td>
                                                <td style='{td_style}'>{_Role.RoleNameEn}</td>
                                                <td style='{td_style}'> Authority</td>
                                            </tr>
                                            <tr style='{tr_style}'>
                                                <td style='{td_style}'>{_stringLocalizer["Member"]} </td>
                                                <td style='{td_style}'>{_User.FullNameAr}</td>
                                                <td style='{td_style}'>{_User.FullNameEn}</td>
                                                <td style='{td_style}'> Member</td>
                                            </tr>
                                             <tr style='{tr_style}'>
                                                <td style='{td_style}'>{_stringLocalizer["Status"]}</td>
                                                <td style='{td_style}'>{_stringLocalizer["Pending"]}</td>
                                                <td style='{td_style}'>Sent</td>
                                                <td style='{td_style}'>Status</td>
                                            </tr>
                                      </table>
                                    </div>         
                                    <h2 style='margin-top: 57px;
                                             font-size: 17px;'>{_stringLocalizer["EmailFooter"]}</h2>
                                        <div style='{image_style}'>
                                            <img src='cid:header' >
                                            <img src='cid:footer' >
                                        </div>
                                     </div>";
            _MailServices.SendNotificationEmail(_User.Email, "تــفــويــض", "", true, CreateAlternateView(_Mail), "", Hosting.AngularRootPath, null);
        }

        public void SendMail(IEnumerable<UserRoleDTO> _UserRoles)
        {
            foreach (var _UserRole in _UserRoles)
            {
                var _User = _UnitOfWork.GetRepository<User>().GetById(_UserRole.UserId);
                var _Organization = _UnitOfWork.GetRepository<Organization>().GetById(_UserRole.OrganizationId);
                var _Role = _UnitOfWork.GetRepository<Role>().GetById(_UserRole.RoleId);

                string Email_style = @"
                                        text-align: center;
                                        flex-direction: column;
                                        justify-content: center;
                                        align-items: center;";

                string Email_image_style = @" 
                                                margin: auto;
                                                display: center;
                                                justify-content: center;
                                                margin: 32px;
                                               ";
                string image_style = @"
                                            text-align: center;
                                            width: 90px;
                                            margin: 0;";
                //margin-left: 16px;

                string table_style = @"
                                            width: 810px;
                                            justify-content: center;
                                            margin-bottom: -5px;
                                            direction: rtl;
                                            border: 1px solid #cccccc;
                                            display: table;
                                            border-collapse: separate;
                                            border-spacing: 2px;
                                            border-color: grey;";

                string tr_style = @" 
                                            white-space: normal;
                                            line-height: normal;
                                            font-weight: normal;
                                            font-size: medium;
                                            font-style: normal;
                                            color: -internal-quirk-inherit;
                                            text-align: start;
                                            direction: rtl;
                                            font-variant: normal;";


                string td_style = @"
                                            border: 1px solid #cccccc;
                                            border-bottom: 1px solid #EEE;
                                            padding: 10px;
                                            width: 3px;
                                            margin-bottom: -3px;
                                            font-weight: 600;
                                            margin: -6px;";

                string _Mail = $@"
                                           
                                           <div style='{Email_style}'>
                                           <img style='{Email_image_style}' src='cid:TopHeader'>
                                           <div style='
                                                width: 810px;
                                                display: flex;
                                                flex-direction: column;
                                                justify-content: center;
                                                align-items: center;
                                                margin: 0;
                                                padding: 0;'>
                                             <table style='{table_style}'>
                                            <tr style='{tr_style}'>
                                                <td colspan='4' style='{td_style}'>
                                                <h3 style='
                                                text-align: center;
                                                background: #13817E;
                                                padding: 9px 0;
                                                font-weight: 900;
                                                margin-bottom: 0px;
                                                color: #fff;'>{_stringLocalizer["EmailHeader"]}</h3></td>
                                            </tr>
                                            <tr style='{tr_style}'>
                                                <td style='{td_style}'>{_stringLocalizer["Corporation"]} </td>
                                                <td style='{td_style}'>{_Organization.OrganizationNameAr}</td>
                                                 <td style='{td_style}'>{_Organization.OrganizationNameEn}</td>
                                                <td style='{td_style}'> Corporation</td>
                                            </tr>
                                            <tr style='{tr_style}'>
                                                <td style='{td_style}'>{_stringLocalizer["Authority"]}</td>
                                                <td style='{td_style}'>{_Role.RoleNameAr}</td>
                                                <td style='{td_style}'>{_Role.RoleNameEn}</td>
                                                <td style='{td_style}'> Authority</td>
                                            </tr>
                                            <tr style='{tr_style}'>
                                                <td style='{td_style}'>{_stringLocalizer["Member"]} </td>
                                                <td style='{td_style}'>{_User.FullNameAr}</td>
                                                <td style='{td_style}'>{_User.FullNameEn}</td>
                                                <td style='{td_style}'> Member</td>
                                            </tr>
                                      </table>
                                    </div>         
                                    <h2 style='margin-top: 57px;
                                             font-size: 17px;'>{_stringLocalizer["EmailFooter"]}</h2>
                                        <div style='{image_style}'>
                                            <img src='cid:header' >
                                            <img src='cid:footer' >
                                        </div>
                                     </div>";
                _MailServices.SendNotificationEmail(_User.Email, "تــفــويــض", "", true, CreateAlternateView(_Mail), "", Hosting.AngularRootPath, null);

            }
        }
        private AlternateView CreateAlternateView(string message)
        {
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(message, null, "text/html");

            string path_TopHeader = Path.Combine(Hosting.AngularRootPath, "assets//images//EmailImages//TopHeader.jpg"); //My TopHeader
                                                                                                                         //------------------------------------TopHeader Image
            LinkedResource imagelink_TopHeader = new LinkedResource(path_TopHeader, "image/png")
            {
                ContentId = "TopHeader",

                TransferEncoding = System.Net.Mime.TransferEncoding.Base64
            };
            htmlView.LinkedResources.Add(imagelink_TopHeader);
            //--------------------------------------------------header Image
            string pathheader = Path.Combine(Hosting.AngularRootPath, "assets//images//EmailImages//header.jpg"); //My Header


            LinkedResource imagelink_header = new LinkedResource(pathheader, "image/png")
            {
                ContentId = "header",

                TransferEncoding = System.Net.Mime.TransferEncoding.Base64
            };
            htmlView.LinkedResources.Add(imagelink_header);

            //--------------------------------------------------Footer Image
            string path_footer = Path.Combine(Hosting.AngularRootPath, "assets//images//EmailImages//footer.jpg"); //My footer


            LinkedResource imagelink_Footer = new LinkedResource(path_footer, "image/png")
            {
                ContentId = "footer",

                TransferEncoding = System.Net.Mime.TransferEncoding.Base64
            };
            htmlView.LinkedResources.Add(imagelink_Footer);
            return htmlView;
        }

    }
}
