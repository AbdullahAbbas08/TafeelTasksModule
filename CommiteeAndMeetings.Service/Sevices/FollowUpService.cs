using AutoMapper;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using IHelperServices.Models;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Models.Enums;
using Models.ProjectionModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class FollowUpService : BusinessService<FollowUp, FollowUpDTO>, IFollowUpService
    {
        IHelperServices.ISessionServices _SessionServices;
        public FollowUpService(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer stringLocalizer, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
            _SessionServices = sessionServices;
        }

        #region Add New FollowUp

        public AddFollowUpDelegationDTO AddFollowUp(List<FollowUpDTO> objectsToAdd, bool from_delegation)
        {
            try
            {
                foreach (var objectToAdd in objectsToAdd)
                {


                    if (objectToAdd == null)
                        return new AddFollowUpDelegationDTO { fail = true };
                    int? followUpOrgId = null;
                    if (objectToAdd.OrganizationId == null)
                    {
                        followUpOrgId = _UnitOfWork.GetRepository<Organization>().GetAll().FirstOrDefault(w => w.OrganizationId == _SessionServices.OrganizationId).FollowUpOrganizationId;
                        if (followUpOrgId == null && from_delegation == true)
                        {
                            return new AddFollowUpDelegationDTO { thereIsNoFollowUpOrgForYourOrg = true };
                        }
                    }
                    FollowUp New_object = new FollowUp();

                    if (objectToAdd.TransactionActionRecipientId == null)
                        objectToAdd.TransactionActionRecipientId = _UnitOfWork.GetRepository<TransactionAction>()
                                                                              .GetById(objectToAdd.TransactionActionId).MaxTransactionActionRecipientId;

                    var transactioAction = _UnitOfWork.GetRepository<TransactionActionRecipient>()
                        .GetAll()
                        .FirstOrDefault(w => w.TransactionActionRecipientId == objectToAdd.TransactionActionRecipientId)
                        .TransactionAction;
                    if (transactioAction == null)
                    {
                        return new AddFollowUpDelegationDTO { fail = true };
                    }
                    New_object.TransactionId = transactioAction.TransactionId;
                    New_object.TransactionActionId = transactioAction.TransactionActionId;
                    New_object.TransactionActionRecipientId = objectToAdd.TransactionActionRecipientId;

                    New_object.CreatedBy = _SessionServices.UserId;
                    New_object.UserId = from_delegation == true ? null : _SessionServices.UserId;
                    New_object.OrganizationId = from_delegation == false ? null : (objectToAdd.OrganizationId ?? followUpOrgId);
                    New_object.FollowUpStatusTypeId = (int)FollowUpStatusTypeEnum.active;
                    New_object.FollowUpStatusCreatedOn = DateTimeOffset.Now;
                    New_object.FinishedDate = objectToAdd.FinishedDate;
                    var inserted = _UnitOfWork.GetRepository<FollowUp>().Insert(New_object);

                    bool success = UpdateFollowUpStatus(inserted.FollowUpId, FollowUpStatusTypeEnum.active);

                }
                return new AddFollowUpDelegationDTO { success = true };
            }
            catch (Exception ex)
            {

                return new AddFollowUpDelegationDTO { fail = true };

            }
        }
        #endregion
        public bool UpdateFollowUpStatus(int followUpId, FollowUpStatusTypeEnum FollowStatusType)
        {
            try
            {
                FollowUpStatus followUpStatus = new FollowUpStatus
                {
                    CreatedById = _SessionServices.UserId.Value,
                    CreatedOn = DateTimeOffset.Now,
                    FollowStatusTypeId = (int)FollowStatusType,
                    FollowUpId = followUpId,

                };
                _UnitOfWork.GetRepository<FollowUpStatus>().Insert(followUpStatus);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }

        }
    }
}