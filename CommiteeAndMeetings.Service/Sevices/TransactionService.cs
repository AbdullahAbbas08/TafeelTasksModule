using AutoMapper;
using AutoMapper.QueryableExtensions;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.BLL.Contexts;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.Views;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using CommiteeDatabase.Models.Domains;
using DbContexts.MasarContext.ProjectionModels;
using EFCore.BulkExtensions;
using IHelperServices.Models;
using LinqHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Models.Enums;
using Models.ProjectionModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class TransactionService : BusinessService<Transaction, TransactionDTO>, ITransactionService
    {
        public string culture = string.Empty;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDataProtectService _dataProtectService;
        IHelperServices.ISessionServices _sessionServices;
        //   private readonly ITransactionActionRepository transactionActionRepository;
        public readonly MasarContext _context;
        ISystemSettingsService _SystemSettingsService;
        protected readonly IHelperServices.ISessionServices _SessionServices;
        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer stringLocalizer, IHelperServices.ISessionServices sessionSevices, ISystemSettingsService systemSettingsService, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        { 
            _context = new MasarContext();
            //    transactionActionRepository = _UnitOfWork.GetRepository<TransactionAction>(true) as ITransactionActionRepository;
            _unitOfWork = unitOfWork;
            _sessionServices = sessionServices;
            _SystemSettingsService = systemSettingsService;
            culture = _sessionServices.Culture;
            _SessionServices = sessionSevices;
        }

        

        //public List<Vw_ReturnGroupReferralDTO> GroupReferral(GroupReferralDTO groupReferralDTO)
        //{
        //    #region audit record action
        //    if (IsAuditEnabled)
        //    {
        //        AuditService auditService = new AuditService(null, _SessionServices, null);
        //        try
        //        {
        //            auditService.SaveAuditTrail(null, groupReferralDTO, "Delegation", "Added", null, null);
        //        }
        //        catch (Exception ex)
        //        {


        //        }


        //    }

        //    #endregion
        //    try
        //    {
        //        string New_tranActionRecIds = "";
        //        string New_tranActionIds = "";
        //        if (!string.IsNullOrEmpty(groupReferralDTO.tranActionRecIds))
        //        {
        //            List<string> recipient_arrBefore = groupReferralDTO.tranActionRecIds.Split(',').ToList();
        //            string tranActionRecIdsAfterDecrypt = "";
        //            recipient_arrBefore.ForEach(s =>
        //            {
        //                string transactionId = _dataProtectService.Decrypt(s.Split('_')[0]);
        //                string transactionActionRecipientId = _dataProtectService.Decrypt(s.Split('_')[1]);
        //                tranActionRecIdsAfterDecrypt = tranActionRecIdsAfterDecrypt + transactionId + "_" + transactionActionRecipientId + ",";
        //            });
        //            groupReferralDTO.tranActionRecIds = tranActionRecIdsAfterDecrypt.Remove(tranActionRecIdsAfterDecrypt.Length - 1, 1);
        //        }
        //        if (!string.IsNullOrEmpty(groupReferralDTO.tranActionIds))
        //        {
        //            List<string> actions_arrBefore = groupReferralDTO.tranActionIds.Split(',').ToList();
        //            string tranActionsIdsAfterDecrypt = "";
        //            actions_arrBefore.ForEach(s =>
        //            {
        //                string transactionId = _dataProtectService.Decrypt(s.Split('_')[0]);
        //                string transactionActionId = _dataProtectService.Decrypt(s.Split('_')[1]);
        //                tranActionsIdsAfterDecrypt = tranActionsIdsAfterDecrypt + transactionId + "_" + transactionActionId + ",";
        //            });
        //            groupReferralDTO.tranActionIds = tranActionsIdsAfterDecrypt.Remove(tranActionsIdsAfterDecrypt.Length - 1, 1);
        //        }

        //        List<Vw_ReturnGroupReferralDTO> result = new List<Vw_ReturnGroupReferralDTO>();
        //        /*Check If It Is Preparation*/
        //        if (groupReferralDTO.actionId == (int)ActionEnum.Preparation && (!string.IsNullOrEmpty(groupReferralDTO.tranActionRecIds) && groupReferralDTO.tranActionRecIds != null))
        //        {
        //            List<string> recipient_arr = groupReferralDTO.tranActionRecIds.Split(',').ToList();
        //            recipient_arr.ForEach(s =>
        //            {
        //                int transactionId = int.Parse(s.Split('_')[0]);
        //                int transactionActionRecipientId = int.Parse(s.Split('_')[1]);
        //                TransactionActionRecipient Curerent_recipient = _unitOfWork.GetRepository<TransactionActionRecipient>().GetAll()
        //                                                        .Where(w =>
        //                                                            w.TransactionAction.TransactionId == transactionId
        //                                                            && w.TransactionActionRecipientId == transactionActionRecipientId)
        //                                                        .FirstOrDefault();
        //                if (Curerent_recipient != null && Curerent_recipient.RecipientStatusId != (int)RecipientStatusesEnum.Sent)
        //                {
        //                    if (Curerent_recipient.IsCc == false)
        //                    {
        //                        bool Sucess = ApplyDefaultDelegation(groupReferralDTO, transactionId, Curerent_recipient);
        //                        if (Sucess)
        //                        {
        //                            result.Add(new Vw_ReturnGroupReferralDTO
        //                            {
        //                                id = transactionId,
        //                                reasonCode = "SuccessedCode",
        //                                status = "ok",
        //                                TransactionActionFormated = "ok",
        //                            });
        //                        }
        //                        else
        //                        {
        //                            New_tranActionRecIds += $"{s},";
        //                        }
        //                    }
        //                    else if (Curerent_recipient.IsCc == true)
        //                    {
        //                        bool Sucess = ApplyWorkFlowImplementationPreparationCC(groupReferralDTO, transactionId, Curerent_recipient);
        //                        if (Sucess)
        //                        {
        //                            result.Add(new Vw_ReturnGroupReferralDTO
        //                            {
        //                                id = transactionId,
        //                                reasonCode = "SuccessedCode",
        //                                status = "ok",
        //                                TransactionActionFormated = "ok",
        //                            });
        //                        }
        //                        else
        //                        {
        //                            New_tranActionRecIds += $"{s},";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        New_tranActionRecIds += $"{s},";
        //                    }
        //                }
        //            });
        //            if (!string.IsNullOrEmpty(New_tranActionRecIds))
        //            {
        //                groupReferralDTO.tranActionRecIds = New_tranActionRecIds;
        //                return transactionRepository.GroupReferral(groupReferralDTO).ToList();
        //            }
        //            return result;

        //        }

        //        /*Not Preparation*/
        //        else
        //        {
        //            string NewOrgs = "";
        //            /* Check If It Is Delegate*/
        //            if ((!string.IsNullOrEmpty(groupReferralDTO.directToOrgId) || !string.IsNullOrEmpty(groupReferralDTO.directToEmpId)) && (!string.IsNullOrEmpty(groupReferralDTO.tranActionRecIds) && groupReferralDTO.tranActionRecIds != null))
        //            {
        //                List<string> recipient_arr = groupReferralDTO.tranActionRecIds.Split(',').ToList();
        //                recipient_arr.ForEach(s =>
        //                {
        //                    int transactionId = int.Parse(s.Split('_')[0]);
        //                    int transactionActionRecipientId = int.Parse(s.Split('_')[1]);
        //                    TransactionActionRecipient Curerent_recipient = transactionActionRecipientsRepository.GetAll()
        //                                                            .Where(w =>
        //                                                                w.TransactionAction.TransactionId == transactionId
        //                                                                && w.TransactionActionRecipientId == transactionActionRecipientId)
        //                                                            .FirstOrDefault();
        //                    if (Curerent_recipient != null && Curerent_recipient.RecipientStatusId != (int)RecipientStatusesEnum.Sent)
        //                    {
        //                        bool Sucess = ApplyDefaultDelegation(groupReferralDTO, transactionId, Curerent_recipient);
        //                        if (Sucess)
        //                        {
        //                            result.Add(new Vw_ReturnGroupReferralDTO
        //                            {
        //                                id = transactionId,
        //                                reasonCode = "SuccessedCode",
        //                                status = "ok",
        //                                TransactionActionFormated = "ok",
        //                            });
        //                        }
        //                        else
        //                        {
        //                            New_tranActionRecIds += $"{s},";
        //                        }
        //                    }
        //                });
        //                if (!string.IsNullOrEmpty(New_tranActionRecIds))
        //                {
        //                    groupReferralDTO.tranActionRecIds = New_tranActionRecIds;
        //                    return transactionRepository.GroupReferral(groupReferralDTO).ToList();
        //                }
        //                return result;
        //            }
        //            /*Check If groupReferralDTO.orgCCIds Is Not Null*/
        //            else if ((!string.IsNullOrEmpty(groupReferralDTO.orgCCIds) || !string.IsNullOrEmpty(groupReferralDTO.empCCIds)) && string.IsNullOrEmpty(groupReferralDTO.tranActionIds))
        //            {
        //                List<int> new_CC_Recipient_Organizations = new List<int>();
        //                if (!string.IsNullOrEmpty(groupReferralDTO.orgCCIds))
        //                {
        //                    new_CC_Recipient_Organizations = groupReferralDTO.orgCCIds.Split(',').Select(x => int.Parse(x)).ToList();
        //                }
        //                new_CC_Recipient_Organizations.ForEach(s =>
        //                {
        //                    // Check IF this Recipient has WorkFlow 
        //                    bool has_workFlow = CheckWorkFlow(s);
        //                    if (has_workFlow)
        //                    {
        //                        List<string> recipient_arr = groupReferralDTO.tranActionRecIds.Split(',').ToList();
        //                        recipient_arr.ForEach(x =>
        //                        {
        //                            int transactionId = int.Parse(x.Split('_')[0]);
        //                            int transactionActionRecipientId = int.Parse(x.Split('_')[1]);
        //                            Transaction currentTransacTion = transactionRepository.GetById(transactionId);
        //                            TransactionActionRecipient recipient = transactionActionRecipientsRepository.GetById(transactionActionRecipientId);
        //                            //update cc to recieved if not
        //                            if (recipient.IsCC && recipient.RecipientStatusId == null)
        //                            {
        //                                transactionActionRecipientsRepository.UpdateTransctionActionRecStatusByTARecipientId(new int[] { recipient.TransactionActionRecipientId }, (int)RecipientStatusesEnum.Seen, "", null, null);
        //                            }
        //                            WorkFlowCustom_ProcessDTO Valid_Process = _delegationService.Find_Valid_Process(currentTransacTion, new TransactionActionRecipient
        //                            {
        //                                DirectedToOrganizationId = s,
        //                                TransactionAction = recipient.TransactionAction
        //                                ,
        //                                IsCC = true
        //                                ,
        //                                TransactionActionId = recipient.TransactionActionId,
        //                            });
        //                            if (Valid_Process != null)
        //                            {
        //                                // Create Delegation DTO
        //                                TransactionAction New_transactionAction = new TransactionAction()
        //                                {
        //                                    TransactionId = transactionId,
        //                                    ActionId = (int)ActionEnum.InternalDelegation,
        //                                    ReferrerTransactionActionId = recipient.TransactionActionId,
        //                                    ReferrerTransactionActionRecipientId = recipient.TransactionActionRecipientId,
        //                                    DirectedFromUserId = null,
        //                                    DirectedFromOrganizationId = _SessionServices.OrganizationId,
        //                                    CreatedByUserRoleId = _SessionServices.UserRoleId,
        //                                    CreatedOn = DateTimeOffset.Now,
        //                                };

        //                                List<TransactionActionRecipientsDTO> New_recipients = new List<TransactionActionRecipientsDTO>()
        //                                    {
        //                                       new TransactionActionRecipientsDTO
        //                                        {
        //                                            DirectedToOrganizationId = s,
        //                                            IsCC = true,
        //                                            IsUrgent = false,
        //                                            Notes = groupReferralDTO.note,
        //                                            RequiredActionId = groupReferralDTO.requiredActionId,
        //                                            transactionActionRecipientAttachmentDTO =transactionActionRecipientAttachmentRepository.GetTransactionActionRecipientsAttachments(0, 0, recipient.TransactionActionRecipientId, false).Select(t => new TransactionActionRecipientAttachmentDTO
        //                                {
        //                                    TransactionAttachmentId = t.TransactionAttachmentId,
        //                                    CreatedBy = _SessionServices.UserId,
        //                                }).ToList()
        //                            }
        //                                    };

        //                                // #three select TransactionActionAttachment

        //                                List<TransactionActionAttachmentDTO> New_TransactionactionAttachment_lst = transactionActionRecipientAttachmentRepository.GetTransactionActionRecipientsAttachments(0, recipient.TransactionActionId, 0, false).Select(t => new TransactionActionAttachmentDTO
        //                                {
        //                                    TransactionAttachmentId = t.TransactionAttachmentId,
        //                                    CreatedBy = _SessionServices.UserId,
        //                                    CreatedOn = DateTimeOffset.Now,
        //                                }).ToList();



        //                                //#five Create New DelegationDTO
        //                                DelegationDTO NewRecord_DelegationDTO = new DelegationDTO()
        //                                {
        //                                    isEmployee = false,
        //                                    TransactionId = _dataProtectService.Encrypt(New_transactionAction.TransactionId.ToString()),
        //                                    ActionId = New_transactionAction.ActionId,
        //                                    IsTransaction = false,
        //                                    //TransactionNumber
        //                                    ActionNumber = New_transactionAction.ActionNumber,
        //                                    CreatedByUserRoleId = New_transactionAction.CreatedByUserRoleId,
        //                                    CreatedByUserRoleName = _SessionServices.UserName,
        //                                    DirectedFromUserId = New_transactionAction.DirectedFromUserId,
        //                                    DirectedFromOrganizationId = New_transactionAction.DirectedFromOrganizationId,
        //                                    OutgoingTransactionTypeId = New_transactionAction.OutgoingTransactionTypeId,
        //                                    OutgoingImportanceLevelId = New_transactionAction.OutgoingImportanceLevelId,
        //                                    OutgoingIsConfidential = New_transactionAction.OutgoingIsConfidential,
        //                                    ReferrerTransactionActionId = _dataProtectService.Encrypt(New_transactionAction.ReferrerTransactionActionId.ToString()),
        //                                    ReferrerTransactionActionRecipientId = _dataProtectService.Encrypt(New_transactionAction.ReferrerTransactionActionRecipientId.ToString()),
        //                                    transactionActionAttachmentDTO = New_TransactionactionAttachment_lst,
        //                                    transactionActionRecipientsDTO = New_recipients,
        //                                    ApplyAcceptConfirmation = true,
        //                                    IssCCWorkFlow = currentTransacTion.TransactionTypeId == 2 || currentTransacTion.TransactionTypeId == 3 ? false : true,
        //                                };
        //                                bool delgated = _delegationService.Delegate(NewRecord_DelegationDTO);
        //                                result.Add(new Vw_ReturnGroupReferralDTO
        //                                {
        //                                    id = transactionId,
        //                                    reasonCode = "SuccessedCode",
        //                                    status = "ok",
        //                                    TransactionActionFormated = "ok",
        //                                });
        //                            }

        //                        });
        //                    }
        //                    else
        //                    {
        //                        NewOrgs += (string.IsNullOrEmpty(NewOrgs) ? Convert.ToString(s) : ("," + Convert.ToString(s)));
        //                    }

        //                });
        //                if (!string.IsNullOrEmpty(NewOrgs) || !string.IsNullOrEmpty(groupReferralDTO.empCCIds))
        //                {
        //                    groupReferralDTO.orgCCIds = NewOrgs;
        //                    var returnGroupReferral = transactionRepository.GroupReferral(groupReferralDTO).ToList();
        //                    if (!string.IsNullOrEmpty(returnGroupReferral.First()?.ReceipientsIds))
        //                    {
        //                        var ReceipientsIds = returnGroupReferral.First()?.ReceipientsIds.Substring(1).Split(",");
        //                        //SendEmail_SMS_Notification(ReceipientsIds, groupReferralDTO.actionId);
        //                        _delegationService.sendNotificationForReceipientsId(ReceipientsIds, null);
        //                    }
        //                    return returnGroupReferral;
        //                }
        //                else
        //                {
        //                    return result;

        //                }

        //            }
        //            else
        //            {
        //                // this for out box to add transaction action
        //                List<string> transactionAction_arr = groupReferralDTO.tranActionIds.Split(',').ToList();
        //                transactionAction_arr.ForEach(s =>
        //                {
        //                    int transactionId = int.Parse(s.Split('_')[0]);
        //                    var currentTransaction = transactionRepository.GetById(transactionId);
        //                    //if (currentTransaction.TransactionTypeId == (int)TransactionTypeEnum.InternalDecision || currentTransaction.TransactionTypeId == (int)TransactionTypeEnum.ExternalDecision)
        //                    {
        //                        int transactionActionId = int.Parse(s.Split('_')[1]);
        //                        TransactionActionRecipient Curerent_recipient = transactionActionRepository.GetAll()
        //                                                                .FirstOrDefault(w =>
        //                                                                    w.TransactionId == transactionId
        //                                                                    && w.TransactionActionId == transactionActionId)
        //                                                                .TransactionActionRecipients.Where(a => !a.IsCC).FirstOrDefault();
        //                        if (Curerent_recipient == null)
        //                        {
        //                            Curerent_recipient = transactionActionRepository.GetAll()
        //                                                              .FirstOrDefault(w =>
        //                                                                  w.TransactionId == transactionId
        //                                                                  && w.TransactionActionId == transactionActionId)
        //                                                              .ReferrerTransactionActionRecipient;
        //                        }
        //                        if (Curerent_recipient != null && Curerent_recipient.RecipientStatusId != (int)RecipientStatusesEnum.Sent)
        //                        {
        //                            groupReferralDTO.ignoreValidateDelegation = true;
        //                            bool Sucess = ApplyDefaultDelegation(groupReferralDTO, transactionId, Curerent_recipient);
        //                            if (Sucess)
        //                            {
        //                                result.Add(new Vw_ReturnGroupReferralDTO
        //                                {
        //                                    id = transactionId,
        //                                    reasonCode = "SuccessedCode",
        //                                    status = "ok",
        //                                    TransactionActionFormated = "ok",
        //                                });
        //                            }
        //                            else
        //                            {
        //                                New_tranActionIds += $"{s},";
        //                            }
        //                        }
        //                    }
        //                });
        //                if (result.Count() > 0)
        //                    return result;

        //                var returnGroupReferral = transactionRepository.GroupReferral(groupReferralDTO).ToList();
        //                if (!string.IsNullOrEmpty(returnGroupReferral.First()?.ReceipientsIds))
        //                {
        //                    var ReceipientsIds = returnGroupReferral.First()?.ReceipientsIds.Substring(1).Split(",");
        //                    //SendEmail_SMS_Notification(ReceipientsIds, groupReferralDTO.actionId);
        //                }
        //                return returnGroupReferral;
        //            }

        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        return new List<Vw_ReturnGroupReferralDTO>();
        //    }

        //}

        //private bool ApplyDefaultDelegation(GroupReferralDTO groupReferralDTO, int transactionId, TransactionActionRecipient Curerent_recipient)
        //{
        //    try
        //    {


        //        /* Create New DelegationDTO */

        //        /* Get Current TransactionAction */
        //        TransactionAction New_transactionAction = new TransactionAction()
        //        {
        //            TransactionId = Curerent_recipient.TransactionAction.TransactionId,
        //            ActionId = (int)ActionEnum.InternalDelegation,
        //            ReferrerTransactionActionId = Curerent_recipient.TransactionActionId,
        //            ReferrerTransactionActionRecipientId = Curerent_recipient.TransactionActionRecipientId,
        //            DirectedFromUserId = (groupReferralDTO.isEmp ? _SessionServices.UserId : null),
        //            DirectedFromOrganizationId = (groupReferralDTO.isEmp ? null : _SessionServices.OrganizationId),
        //            CreatedByUserRoleId = _SessionServices.UserRoleId,
        //            CreatedOn = DateTimeOffset.Now,
        //        };
        //        /*Get Recipient Attachment*/
        //        List<TransactionActionRecipientAttachmentDTO> transactionActionRecipientAttachmentDTO = Curerent_recipient.TransactionActionRecipientAttachments
        //                                                                    .Select(s => new TransactionActionRecipientAttachmentDTO
        //                                                                    {
        //                                                                        TransactionAttachmentId = s.TransactionAttachmentId,
        //                                                                        CreatedBy = s.CreatedBy,
        //                                                                    }).ToList();
        //        List<TransactionActionRecipientsDTO> New_recipients = new List<TransactionActionRecipientsDTO>();
        //        if (!string.IsNullOrEmpty(groupReferralDTO.directToOrgId))
        //        {
        //            List<int> new_Recipient_Organization = groupReferralDTO.directToOrgId.Split(',').Select(s => int.Parse(s)).ToList();
        //            new_Recipient_Organization.ForEach(s =>
        //            {
        //                New_recipients.Add(new TransactionActionRecipientsDTO
        //                {
        //                    DirectedToOrganizationId = s,
        //                    IsCC = false,
        //                    IsUrgent = false,
        //                    Notes = groupReferralDTO.note,
        //                    RequiredActionId = groupReferralDTO.requiredActionId,
        //                    transactionActionRecipientAttachmentDTO = transactionActionRecipientAttachmentDTO,
        //                    CorrespondentUserId = groupReferralDTO.correspondentUserId,
        //                    SendNotification = true
        //                });
        //            });
        //        }
        //        if (!string.IsNullOrEmpty(groupReferralDTO.directToEmpId))
        //        {
        //            List<int> new_Recipient_Users = groupReferralDTO.directToEmpId.Split(',').Select(s => int.Parse(s)).ToList();
        //            new_Recipient_Users.ForEach(s =>
        //            {
        //                New_recipients.Add(new TransactionActionRecipientsDTO
        //                {
        //                    DirectedToUserId = s,
        //                    IsCC = false,
        //                    IsUrgent = false,
        //                    Notes = groupReferralDTO.note,
        //                    RequiredActionId = groupReferralDTO.requiredActionId,
        //                    transactionActionRecipientAttachmentDTO = transactionActionRecipientAttachmentDTO,
        //                    CorrespondentUserId = groupReferralDTO.correspondentUserId,
        //                    SendNotification = true
        //                });
        //            });
        //        }
        //        if (!string.IsNullOrEmpty(groupReferralDTO.empCCIds))
        //        {
        //            List<int> new_CC_Recipient_Users = groupReferralDTO.empCCIds.Split(',').Select(s => int.Parse(s)).ToList();
        //            new_CC_Recipient_Users.ForEach(s =>
        //            {
        //                New_recipients.Add(new TransactionActionRecipientsDTO
        //                {
        //                    DirectedToUserId = s,
        //                    IsCC = true,
        //                    IsUrgent = false,
        //                    Notes = groupReferralDTO.note,
        //                    RequiredActionId = groupReferralDTO.requiredActionId,
        //                    transactionActionRecipientAttachmentDTO = transactionActionRecipientAttachmentDTO,
        //                    CorrespondentUserId = groupReferralDTO.correspondentUserId,
        //                    SendNotification = true
        //                });
        //            });
        //        }
        //        if (!string.IsNullOrEmpty(groupReferralDTO.orgCCIds))
        //        {
        //            List<int> new_CC_Recipient_Organizations = groupReferralDTO.orgCCIds.Split(',').Select(s => int.Parse(s)).ToList();
        //            new_CC_Recipient_Organizations.ForEach(s =>
        //            {
        //                New_recipients.Add(new TransactionActionRecipientsDTO
        //                {
        //                    DirectedToOrganizationId = s,
        //                    IsCC = true,
        //                    IsUrgent = false,
        //                    Notes = groupReferralDTO.note,
        //                    RequiredActionId = groupReferralDTO.requiredActionId,
        //                    transactionActionRecipientAttachmentDTO = transactionActionRecipientAttachmentDTO,
        //                    CorrespondentUserId = groupReferralDTO.correspondentUserId,
        //                    SendNotification = true
        //                });
        //            });
        //        }

        //        // #three select TransactionActionAttachment
        //        List<TransactionActionAttachmentDTO> New_TransactionactionAttachment_lst = transactionActionRecipientAttachmentRepository.GetTransactionActionRecipientsAttachments(0, Curerent_recipient.TransactionActionId, 0, false).Select(t => new TransactionActionAttachmentDTO
        //        {
        //            TransactionAttachmentId = t.TransactionAttachmentId,
        //            CreatedBy = _SessionServices.UserId,
        //            CreatedOn = DateTimeOffset.Now,
        //        }).ToList();

        //        // #five Create New DelegationDTO
        //        DelegationDTO NewRecord_DelegationDTO = new DelegationDTO()
        //        {
        //            isEmployee = groupReferralDTO.isEmp,
        //            TransactionId = _dataProtectService.Encrypt(New_transactionAction.TransactionId.ToString()),
        //            ActionId = New_transactionAction.ActionId,
        //            IsTransaction = false,
        //            //TransactionNumber
        //            ActionNumber = New_transactionAction.ActionNumber,
        //            CreatedByUserRoleId = New_transactionAction.CreatedByUserRoleId,
        //            CreatedByUserRoleName = _SessionServices.UserName,
        //            DirectedFromUserId = New_transactionAction.DirectedFromUserId,
        //            DirectedFromOrganizationId = New_transactionAction.DirectedFromOrganizationId,
        //            OutgoingTransactionTypeId = New_transactionAction.OutgoingTransactionTypeId,
        //            OutgoingImportanceLevelId = New_transactionAction.OutgoingImportanceLevelId,
        //            OutgoingIsConfidential = New_transactionAction.OutgoingIsConfidential,
        //            ReferrerTransactionActionId = _dataProtectService.Encrypt(New_transactionAction.ReferrerTransactionActionId.ToString()),
        //            ReferrerTransactionActionRecipientId = _dataProtectService.Encrypt(New_transactionAction.ReferrerTransactionActionRecipientId.ToString()),
        //            transactionActionAttachmentDTO = New_TransactionactionAttachment_lst,
        //            transactionActionRecipientsDTO = New_recipients,
        //            ignoreValidateDelegation = groupReferralDTO.ignoreValidateDelegation,
        //        };
        //        bool result = _delegationService.Delegate(NewRecord_DelegationDTO);

        //        return result;
        //    }
        //    catch (Exception Ex)
        //    {

        //        throw;
        //    }
        //}


    }


}
