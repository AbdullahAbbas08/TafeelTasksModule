using CommiteeAndMeetings.BLL;
using CommiteeAndMeetings.BLL.BaseObjects;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.BLL.Contexts;
using CommiteeAndMeetings.BLL.Hosting;
using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeDatabase.Models.Domains;
using IHelperServices;
using IHelperServices.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class JobSchedulingService : IJobSchedulingService
    {
        protected readonly MasarContext _Context;
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IUsersService _usersService;
        private readonly ICommiteeLocalizationService commiteeLocalizationService;
        private readonly IHttpContextAccessor _contextAccessor;
        IMailServices mailService;
        private readonly ICommitteeMeetingSystemSettingService _systemSettingsService;


        public JobSchedulingService(
           IUnitOfWork UnitOfWork, IMailServices _mailService,
            IUsersService usersService,
           MasarContext context, IHttpContextAccessor contextAccessor, ICommiteeLocalizationService _commiteeLocalizationService,
           ICommitteeMeetingSystemSettingService systemSettingsService
           )
        {
            _Context = context;
            _UnitOfWork=UnitOfWork;
            _usersService=usersService;
            _contextAccessor=contextAccessor;
            commiteeLocalizationService = _commiteeLocalizationService;
            mailService = _mailService;
            _systemSettingsService = systemSettingsService;

        }

        public Task SendReminderMail(string code)
        {
            var remindDays = _UnitOfWork.GetRepository<CommitteeMeetingSystemSetting>().GetAll().Where(x => x.SystemSettingCode == code).FirstOrDefault();
            if(remindDays != null && !string.IsNullOrEmpty(remindDays.SystemSettingValue))
            {
                int numberOfDays=Convert.ToInt32(remindDays.SystemSettingValue);
                var allAppliedTasks = _UnitOfWork.GetRepository<CommiteeTask>().GetAll()
                    .Where(x => EF.Functions.DateDiffDay(x.EndDate, DateTimeOffset.Now) == -numberOfDays).ToList().GroupBy(x => x.MainAssinedUserId).ToList();
                foreach(var task in allAppliedTasks)
                {
                    var mainAssignedUserId = task.Key;
                    var data = task.ToList();
                    string Message = "";
                    string mailSubject = "";
                    var title=commiteeLocalizationService.GetLocaliztionByCode("ReminderTitle",true);
                    title = title.Replace("#N", numberOfDays.ToString());
                    getMailMessage(data, ref Message, ref mailSubject, title);
                    AlternateView htmlViewForIncoming = CreateAlternateView(Message, null, "text/html");
                    mailService.SendNotificationEmail(data.FirstOrDefault().MainAssinedUser.Email, mailSubject,
                           null, true, htmlViewForIncoming, null, Hosting.AngularRootPath, null
                           );
                }
               
            }
            return Task.CompletedTask;
        }

        public Task TaskEscalation()
        {
            var taskEscalationSettings = _UnitOfWork.GetRepository<CommiteeTaskEscalation>().GetAll().ToList();
            var settingWithRecurringCAtegory = taskEscalationSettings.GroupBy(x => new { x.MainAssinedUserId, x.DelayPeriod });
            foreach (var setting in taskEscalationSettings)
            {
                if(setting.ComiteeTaskCategoryId == null || setting.ComiteeTaskCategoryId == 0)
                {
                    var settingGroup= settingWithRecurringCAtegory.Where(x=>x.Key.MainAssinedUserId == setting.MainAssinedUserId && x.Key.DelayPeriod == setting.DelayPeriod).FirstOrDefault();
                    var execludedCategories = settingGroup.Where(z => z.ComiteeTaskCategoryId != 0 && z.ComiteeTaskCategoryId !=null).Select(x=>x.ComiteeTaskCategoryId).ToList();
                    var appliedTasks = new List<CommiteeTask>();
                    var note = commiteeLocalizationService.GetLocaliztionByCode("EscalationNote", true);
                    var logList = new List<UpdateTaskLogMainAssignedUser>();

                    if (execludedCategories.Count() > 0)
                    {
                     appliedTasks = _UnitOfWork.GetRepository<CommiteeTask>().GetAll().Where(x => x.MainAssinedUserId == setting.MainAssinedUserId && setting.DelayPeriod == EF.Functions.DateDiffDay(x.EndDate,DateTimeOffset.Now) 
                     ).ToList().Where(z=>(!execludedCategories.Contains(Convert.ToInt32(z.ComiteeTaskCategoryId)))).ToList();
                        //var anothe
                        //foreach (var cat in execludedCategories)
                        //{
                        //    var specTasks = _UnitOfWork.GetRepository<CommiteeTask>().GetAll().Where(x => x.MainAssinedUserId == setting.MainAssinedUserId && x.ComiteeTaskCategoryId == cat.Value && setting.DelayPeriod == EF.Functions.DateDiffDay(x.EndDate, DateTimeOffset.Now)).ToList();
                        //    foreach (var task in specTasks)
                        //    {
                        //        var log = new UpdateTaskLogMainAssignedUser();
                        //        log.FullNameAr = task.MainAssinedUser.FullNameAr;
                        //        log.FullNameEn = task.MainAssinedUser.FullNameEn;
                        //        log.MainAssignedUserId = task.MainAssinedUserId;
                        //        log.Notes = note;
                        //        log.CancelDate = DateTimeOffset.Now;
                        //        log.CommiteeTaskId = task.CommiteeTaskId;
                        //        task.MainAssinedUserId = setting.NewMainAssinedUserId;
                        //        logList.Add(log);
                        //        _Context.CommiteeTasks.Update(task);
                        //    }
                        //    _Context.UpdateTaskLogMainAssignedUser.AddRange(logList);
                        //    logList.Clear();
                        //}

                    }
                    else
                    {
                        appliedTasks = _UnitOfWork.GetRepository<CommiteeTask>().GetAll().Where(x => x.MainAssinedUserId == setting.MainAssinedUserId && setting.DelayPeriod == EF.Functions.DateDiffDay(x.EndDate, DateTimeOffset.Now)).ToList();
                    }
                    
                    foreach (var task in appliedTasks)
                    {

                       
                        var log = new UpdateTaskLogMainAssignedUser();
                        log.FullNameAr = task.MainAssinedUser.FullNameAr;
                        log.FullNameEn = task.MainAssinedUser.FullNameEn;
                        log.MainAssignedUserId = task.MainAssinedUserId;
                        log.Notes = note;
                        log.CancelDate = DateTimeOffset.Now;
                        log.ByHangfire = true;
                        log.CommiteeTaskId = task.CommiteeTaskId;
                        task.MainAssinedUserId = setting.NewMainAssinedUserId;
                        log.NewMainAssignedUserId = task.MainAssinedUserId;
                        var ee = _Context.Users.Where(x => x.UserId == log.NewMainAssignedUserId).FirstOrDefault();
                        log.NewFullNameAr= ee.FullNameAr;
                        log.NewFullNameEn=ee.FullNameEn;
                        
                            string Message = "";
                            string mailSubject = "";
                            var title = commiteeLocalizationService.GetLocaliztionByCode("taskTransferTitle", true);
                            //title = title.Replace("#N", numberOfDays.ToString());
                            getMailMessage(task, log, ref Message, ref mailSubject, title);
                            AlternateView htmlViewForIncoming = CreateAlternateView(Message, null, "text/html");
                            mailService.SendNotificationEmail(task.MainAssinedUser.Email, mailSubject,
                                   null, true, htmlViewForIncoming, null, Hosting.AngularRootPath, null
                                   );
                            _Context.CommiteeTasks.Update(task);

                        logList.Add(log);
                       
                        
                    }
                    _Context.UpdateTaskLogMainAssignedUser.AddRange(logList);
                    _Context.SaveChanges();

                }
                else
                {
                    //(DateTimeOffset.Now - x.EndDate).Days
                    var note = commiteeLocalizationService.GetLocaliztionByCode("EscalationNote", true);

                    var appliedTasks = _UnitOfWork.GetRepository<CommiteeTask>().GetAll()
                        .Where(x => x.MainAssinedUserId == setting.MainAssinedUserId && setting.DelayPeriod == EF.Functions.DateDiffDay(x.EndDate, DateTimeOffset.Now) && x.ComiteeTaskCategoryId == setting.ComiteeTaskCategoryId).ToList();
                    var logList = new List<UpdateTaskLogMainAssignedUser>();

                    foreach (var task in appliedTasks)
                    {
                       
                        var log = new UpdateTaskLogMainAssignedUser();
                        log.FullNameAr = task.MainAssinedUser.FullNameAr;
                        log.FullNameEn = task.MainAssinedUser.FullNameEn;
                        log.MainAssignedUserId = task.MainAssinedUserId;
                        log.Notes = note;
                        log.ByHangfire = true;
                        log.CancelDate = DateTimeOffset.Now;
                        log.CommiteeTaskId = task.CommiteeTaskId;
                        task.MainAssinedUserId = setting.NewMainAssinedUserId;
                        log.NewMainAssignedUserId = task.MainAssinedUserId;
                        var ee = _Context.Users.Where(x => x.UserId == log.NewMainAssignedUserId).FirstOrDefault();
                        log.NewFullNameAr = ee.FullNameAr;
                        log.NewFullNameEn = ee.FullNameEn;
                            string Message = "";
                            string mailSubject = "";
                            var title = commiteeLocalizationService.GetLocaliztionByCode("taskTransferTitle", true);
                            //title = title.Replace("#N", numberOfDays.ToString());
                            getMailMessage(task, log, ref Message, ref mailSubject, title);
                            AlternateView htmlViewForIncoming = CreateAlternateView(Message, null, "text/html");
                            mailService.SendNotificationEmail(task.MainAssinedUser.Email, mailSubject,
                                   null, true, htmlViewForIncoming, null, Hosting.AngularRootPath, null
                                   );
                            _Context.CommiteeTasks.Update(task);

                        logList.Add(log);
                        
                    }
                    _Context.UpdateTaskLogMainAssignedUser.AddRange(logList);
                    _Context.SaveChanges();
                }
                _UnitOfWork.SaveChanges();
            }
            return Task.CompletedTask;
        }

        public void getMailMessage(List<CommiteeTask> tasks, ref string mailMessage, ref string mailSubject, string mailTitle)
        {
            try
            {

                var subject = commiteeLocalizationService.GetLocaliztionByCode("ReminderMailSubject", true);
                var JobTitle = commiteeLocalizationService.GetLocaliztionByCode("JobTitle", true);
                var JobDate = commiteeLocalizationService.GetLocaliztionByCode("JobDate", true);
                var commiteeName = commiteeLocalizationService.GetLocaliztionByCode("commiteeName", true);

                var taskDetailsLink = commiteeLocalizationService.GetLocaliztionByCode("taskDetailsLink", true);

                string TaskDetails = _systemSettingsService.GetByCode("taskDetailsLink").SystemSettingValue;



                //if (task.CommiteeTaskId == 0)
                //{
                //    var lastInsertedTaskId = .GetRepository<CommiteeTask>().GetAll().OrderByDescending(x => x.CommiteeTaskId).FirstOrDefault().CommiteeTaskId;
                //    task.CommiteeTaskId = lastInsertedTaskId;
                //}
                //string commiteeTaskEncyption = Encription.EncriptStringAES(tasks.ToString());
                //var taskDetailsLink = $"{systemsettinglink}/tasks/{task.CommiteeTaskId}";


                mailSubject = subject;

                string CreateDate = DateTime.Now.ToString("yyyy-MM-dd", new CultureInfo("ar-AE"));
                string CreateDateEn = DateTime.Now.ToString("dd-MM-yyyy");


                //string lblTransactionType = emailParams.lblTransactionType;
                //string lblTransactionDate = emailParams.lblTransactionDate;
                //string lblDelegationFromOrg = emailParams.lblDelegationFromOrg;
                //string lblUrl = emailParams.lblTransURL;
                //string lblRequiredAction = emailParams.lblRequiredAction;
                //string lblTransactionNumberEn = emailParams.lblTransactionNumberEn;
                //string lblTransactionSubjectEn = emailParams.lblTransactionSubjectEn;
                //string lblTransactionTypeEn = emailParams.lblTransactionTypeEn;
                //string lblTransactionDateEn = emailParams.lblTransactionDateEn;
                //string lblDelegationFromOrgEn = emailParams.lblDelegationFromOrgEn;
                //string lblUrlEn = emailParams.lblTransURLEn;
                //string lblRequiredActionEn = emailParams.lblRequiredActionEn;
                //string lblMailHeader = emailParams.lblMailHeader;
                //string lblThanks = emailParams.ThanksLocalization;
                //string lblThanks2 = emailParams.ThanksLocalization2;
                //string incomingOrgNameAr = string.Empty;
                //string incomingOrgNameEn = string.Empty;
                //string IncomingOrganizationTR = string.Empty;
                //string ExternalDelegationOrgsTR = string.Empty;
                //string EmailValueColor = emailParams.EmailValueColor;
                //string EmailLblColor = emailParams.EmailLblColor;
                string Email_style = @"
	                                        text-align: center;
	                                        flex-direction: column;
	                                        justify-content: center;
	                                        align-items: center;";
                string Email_image_style = @" 
                                            text-align: center !important;
	                                        margin: auto !important;
	                                        justify-content: center;
	                                        margin: 32px;";
                string image_style = @"margin: 0;";
                string table_style = @"
	                                        width: 100%;
	                                        margin-bottom: -5px;
	                                        direction: rtl;
	                                        border: 1px solid #cccccc;
	                                        display: table;
	                                        border-collapse: collapse;
	                                        border-spacing: 2px;
	                                        border-color: grey;";
                string tr_style = @" 
	                                        //white-space: normal;
	                                        //line-height: normal;
	                                        font-weight: normal;
	                                        font-size: medium;
	                                        font-style: normal;
	                                        color: -internal-quirk-inherit;
	                                        text-align: start;
	                                        font-variant: normal;
                                    ";
                string td_style = @"
                                    padding: 10px;
	                                width: 3px;
	                                margin-bottom: -3px;
	                                font-weight: 600;
	                                margin: -6px;
                                    
                                    ";
                string td_style_En = @"
                                        padding: 10px;
	                                    width: 3px;
	                                    margin-bottom: -3px;
	                                    font-weight: 600;
	                                    margin: -6px;
	                                    direction: ltr;";
                string w_20 = @"width: 20%;";
                string w_30 = @"width: 30%;";
                string rtl = @"direction: rtl;";
                string text_center = @" text-align: center;";
                string tr_display = @" display: {displayOption}; ";

                // Add url
                var HtmlString_new = new StringBuilder();
                HtmlString_new.Append($@"                                           
                                          <div style='{Email_style}'>
                                           <img style='{Email_image_style}' src='cid:TopHeader'>
                                           <div style='
		                                        width: 100%;
		                                        display: flex;
		                                        flex-direction: column;
		                                        justify-content: center;
		                                        margin: 0;
		                                        padding: 0;
                                                
	                                        '>		
		                                 <table style='width: 100%' border='1'>
			                                <tr style='{tr_style}'>
				                                <td colspan='5' style='{td_style}'>
				                                    <h2 style=' text-align: center; background: #13817E; padding: 9px 0; font-weight: 900; margin-bottom: 0px; color: #fff;'> {mailTitle} </h2>
                                                </td>
			                                </tr>
                                           <tr style='{tr_style}'>
                                                <td style='{td_style_En + ';' + w_20};text-align: center;'> {JobTitle} </td>   
                                                <td colspan='3' style='{td_style + ';' + w_20 + rtl};text-align: center;'> {taskDetailsLink} </td>
				                                <td style='{td_style + ';' + w_20 + rtl};text-align: center;'> {JobDate} </td>
                                            </tr >");
                foreach (var item in tasks)
                {

                    string taskAfterReplace = null;
                    var tasklink = "";
                    string commiteeTaskEncyption = Encription.EncriptStringAES(item.CommiteeTaskId.ToString());
                    string unicodedCommiteeTaskId = HttpUtility.UrlEncode(commiteeTaskEncyption);

                    //if (commiteeTaskEncyption.Contains('='))
                    //{
                    //    taskAfterReplace = commiteeTaskEncyption.Replace("=", "%3D");
                    //}
                    //if (commiteeTaskEncyption.Contains('/'))
                    //{
                    //    taskAfterReplace = taskAfterReplace.Replace("/", "%2F");
                    //}
                    //if (commiteeTaskEncyption.Contains(' '))
                    //{
                    //    taskAfterReplace = commiteeTaskEncyption.Replace(" ", "+");
                    //}
                    //if (string.IsNullOrEmpty(taskAfterReplace))
                    //{

                    //    tasklink = $"{TaskDetails}/tasks/{commiteeTaskEncyption}";
                    //}
                    tasklink = $"{TaskDetails}/tasks/{unicodedCommiteeTaskId}";



                    HtmlString_new.Append($@"<tr style='{tr_style}'>
                                                <td style='{td_style_En + ';' + w_20};text-align: center;'> {item.Title} </td>   
                                                <td colspan='3' style='{td_style + ';' + w_20 + rtl};text-align: center;'>{tasklink} </td>
				                                <td style='{td_style + ';' + w_20 + rtl};text-align: center;'>{item.EndDate.DateTime} </td>
                                            </tr >");
                }


                HtmlString_new.Append($@" </table>
                                                </div>                                                     
                                                    </div>");

                //< td style = '{td_style_En + '; ' + w_20};background:{EmailLblColor}' >{ lblRequiredActionEn} </ td > 
                //< td style = '{td_style + '; ' + w_20 + rtl};background:{EmailLblColor}' >{ lblRequiredAction} </ td >



                //str HtmlString = new StringBuilder();
                //HtmlString.Append("<div style='width:680px;Margin:0 auto;'><img src='cid:TopHeader'  style='width:100% !important;min-width:680px !important;text-align:right; float:right!important'/> ");
                //HtmlString.Append("<img src='cid:header'  style='width:100% !important;min-width:680px !important;text-align:right;float:right!important'/>");
                //HtmlString.Append("<table style='width:100%;text-align:right;line-height:20px;font-size:15px;direction:rtl; margin-bottom: 20px; padding - right: 6%'>");
                //HtmlString.Append("<tbody>");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'> <span > " + lblTransactionNumber + "</span></th>  <td style='border-bottom: 1px solid #EEE;'>" + TransactionNumber + "</td>  </tr>");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblTransactionSubject + "</span></th><td style='border-bottom: 1px solid #EEE;'> " + trans_Subject + "</td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'> <span > " + lblTransactionType + "</span></th><td style='border-bottom: 1px solid #EEE;'>" + TransactionType + "</td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'> <span > " + lblTransactionDate + "</span></th><td style='border-bottom: 1px solid #EEE;'>" + CreateDate + "   </td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblDelegationFromOrg + "</span></th><td style='border-bottom: 1px solid #EEE;'>  " + delegatedFrom + " </td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblImportance_level + "</span></th><td style='border-bottom: 1px solid #EEE;'>  " + ImportanceName + " </td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblConfidentiality_level + "</span></th><td style='border-bottom: 1px solid #EEE;'>  " + ConfidentialityName + " </td></tr> ");
                //HtmlString.Append("</tbody></table >");
                //HtmlString.Append("<img src='cid:footer'  style='width:100%;text-align:right;line-height:20px;font-size:15px;direction:rtl;'/></div>");


                mailMessage = HtmlString_new.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void getMailMessage(CommiteeTask task,UpdateTaskLogMainAssignedUser logMainAssignedUser, ref string mailMessage, ref string mailSubject, string mailTitle)
        {
            try
            {

                var subject = commiteeLocalizationService.GetLocaliztionByCode("TaskTransferSubject", true);
                var oldAssigned = commiteeLocalizationService.GetLocaliztionByCode("OldMainAssignedUser", true);
                var newAssigned = commiteeLocalizationService.GetLocaliztionByCode("NewMainAssignedUser", true);
                var TaskTitle = commiteeLocalizationService.GetLocaliztionByCode("Title", true);

                mailSubject = subject;

                string CreateDate = DateTime.Now.ToString("yyyy-MM-dd", new CultureInfo("ar-AE"));
                string CreateDateEn = DateTime.Now.ToString("dd-MM-yyyy");


                //string lblTransactionType = emailParams.lblTransactionType;
                //string lblTransactionDate = emailParams.lblTransactionDate;
                //string lblDelegationFromOrg = emailParams.lblDelegationFromOrg;
                //string lblUrl = emailParams.lblTransURL;
                //string lblRequiredAction = emailParams.lblRequiredAction;
                //string lblTransactionNumberEn = emailParams.lblTransactionNumberEn;
                //string lblTransactionSubjectEn = emailParams.lblTransactionSubjectEn;
                //string lblTransactionTypeEn = emailParams.lblTransactionTypeEn;
                //string lblTransactionDateEn = emailParams.lblTransactionDateEn;
                //string lblDelegationFromOrgEn = emailParams.lblDelegationFromOrgEn;
                //string lblUrlEn = emailParams.lblTransURLEn;
                //string lblRequiredActionEn = emailParams.lblRequiredActionEn;
                //string lblMailHeader = emailParams.lblMailHeader;
                //string lblThanks = emailParams.ThanksLocalization;
                //string lblThanks2 = emailParams.ThanksLocalization2;
                //string incomingOrgNameAr = string.Empty;
                //string incomingOrgNameEn = string.Empty;
                //string IncomingOrganizationTR = string.Empty;
                //string ExternalDelegationOrgsTR = string.Empty;
                //string EmailValueColor = emailParams.EmailValueColor;
                //string EmailLblColor = emailParams.EmailLblColor;
                string Email_style = @"
	                                        text-align: center;
	                                        flex-direction: column;
	                                        justify-content: center;
	                                        align-items: center;";
                string Email_image_style = @" 
                                            text-align: center !important;
	                                        margin: auto !important;
	                                        justify-content: center;
	                                        margin: 32px;";
                string image_style = @"margin: 0;";
                string table_style = @"
	                                        width: 100%;
	                                        margin-bottom: -5px;
	                                        direction: rtl;
	                                        border: 1px solid #cccccc;
	                                        display: table;
	                                        border-collapse: collapse;
	                                        border-spacing: 2px;
	                                        border-color: grey;";
                string tr_style = @" 
	                                        //white-space: normal;
	                                        //line-height: normal;
	                                        font-weight: normal;
	                                        font-size: medium;
	                                        font-style: normal;
	                                        color: -internal-quirk-inherit;
	                                        text-align: start;
	                                        font-variant: normal;
                                    ";
                string td_style = @"
                                    padding: 10px;
	                                width: 3px;
	                                margin-bottom: -3px;
	                                font-weight: 600;
	                                margin: -6px;
                                    
                                    ";
                string td_style_En = @"
                                        padding: 10px;
	                                    width: 3px;
	                                    margin-bottom: -3px;
	                                    font-weight: 600;
	                                    margin: -6px;
	                                    direction: ltr;";
                string w_20 = @"width: 20%;";
                string w_30 = @"width: 30%;";
                string rtl = @"direction: rtl;";
                string text_center = @" text-align: center;";
                string tr_display = @" display: {displayOption}; ";

                // Add url
                var HtmlString_new = new StringBuilder();
                HtmlString_new.Append($@"                                           
                                          <div style='{Email_style}'>
                                           <img style='{Email_image_style}' src='cid:TopHeader'>
                                           <div style='
		                                        width: 100%;
		                                        display: flex;
		                                        flex-direction: column;
		                                        justify-content: center;
		                                        margin: 0;
		                                        padding: 0;
                                                
	                                        '>		
		                                 <table style='width: 100%' border='1'>
			                                <tr style='{tr_style}'>
				                                <td colspan='2' style='{td_style}'>
				                                    <h2 style=' text-align: center; background: #13817E; padding: 9px 0; font-weight: 900; margin-bottom: 0px; color: #fff;'> {mailTitle} </h2>
                                                </td>
			                                </tr>
                                           <tr style='{tr_style}'>
				                                <td style='{td_style + ';' + w_20 + rtl }text-align: center;'> {task.Title} </td>
                                                <td style='{td_style + ';' + w_20 + rtl}text-align: center;'> {TaskTitle} </td>   
                                            </tr >
                                            <tr style='{tr_style}'>
				                                <td style='{td_style + ';' + w_20 + rtl }text-align: center;'> {logMainAssignedUser.FullNameAr} </td>
                                                <td style='{td_style + ';' + w_20 + rtl}text-align: center;'> {oldAssigned} </td>   
                                            </tr >
                                            <tr style='{tr_style}'>
				                                <td style='{td_style + ';' + w_20 + rtl }text-align: center;'> {logMainAssignedUser.NewFullNameAr} </td>
                                                <td style='{td_style + ';' + w_20 + rtl}text-align: center;'> {newAssigned} </td>   
                                            </tr >
                                            ");

                HtmlString_new.Append($@" </table>
                                                </div>                                                     
                                                    </div>");

                //< td style = '{td_style_En + '; ' + w_20};background:{EmailLblColor}' >{ lblRequiredActionEn} </ td > 
                //< td style = '{td_style + '; ' + w_20 + rtl};background:{EmailLblColor}' >{ lblRequiredAction} </ td >



                //str HtmlString = new StringBuilder();
                //HtmlString.Append("<div style='width:680px;Margin:0 auto;'><img src='cid:TopHeader'  style='width:100% !important;min-width:680px !important;text-align:right; float:right!important'/> ");
                //HtmlString.Append("<img src='cid:header'  style='width:100% !important;min-width:680px !important;text-align:right;float:right!important'/>");
                //HtmlString.Append("<table style='width:100%;text-align:right;line-height:20px;font-size:15px;direction:rtl; margin-bottom: 20px; padding - right: 6%'>");
                //HtmlString.Append("<tbody>");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'> <span > " + lblTransactionNumber + "</span></th>  <td style='border-bottom: 1px solid #EEE;'>" + TransactionNumber + "</td>  </tr>");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblTransactionSubject + "</span></th><td style='border-bottom: 1px solid #EEE;'> " + trans_Subject + "</td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'> <span > " + lblTransactionType + "</span></th><td style='border-bottom: 1px solid #EEE;'>" + TransactionType + "</td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'> <span > " + lblTransactionDate + "</span></th><td style='border-bottom: 1px solid #EEE;'>" + CreateDate + "   </td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblDelegationFromOrg + "</span></th><td style='border-bottom: 1px solid #EEE;'>  " + delegatedFrom + " </td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblImportance_level + "</span></th><td style='border-bottom: 1px solid #EEE;'>  " + ImportanceName + " </td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblConfidentiality_level + "</span></th><td style='border-bottom: 1px solid #EEE;'>  " + ConfidentialityName + " </td></tr> ");
                //HtmlString.Append("</tbody></table >");
                //HtmlString.Append("<img src='cid:footer'  style='width:100%;text-align:right;line-height:20px;font-size:15px;direction:rtl;'/></div>");


                mailMessage = HtmlString_new.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AlternateView CreateAlternateView(string message, object p, string v)
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
            //string pathheader = Path.Combine(Hosting.AngularRootPath, "assets//images//EmailImages//header.jpg"); //My Header


            //LinkedResource imagelink_header = new LinkedResource(pathheader, "image/png")
            //{
            //    ContentId = "header",

            //    TransferEncoding = System.Net.Mime.TransferEncoding.Base64
            //};
            //htmlView.LinkedResources.Add(imagelink_header);

            //--------------------------------------------------Footer Image
            //string path_footer = Path.Combine(Hosting.AngularRootPath, "assets//images//EmailImages//footer.jpg"); //My footer


            //LinkedResource imagelink_Footer = new LinkedResource(path_footer, "image/png")
            //{
            //    ContentId = "footer",

            //    TransferEncoding = System.Net.Mime.TransferEncoding.Base64
            //};
            //htmlView.LinkedResources.Add(imagelink_Footer);
            return htmlView;
        }


    }
}
