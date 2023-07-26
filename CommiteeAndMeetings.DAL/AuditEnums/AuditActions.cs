using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.AuditEnums
{
    public enum AuditActions
    {
        #region Users
        UserCreated,
        RoleAppliedToUser,
        UserDeleted,
        UserEditView,
        UserUpdated,
        UsersListViewed,
        UserUpdateProfile,
        SignaturePasswordChanged,
        UserSignatureUpdated,
        UserTarkinUpdated,
        UserSignatureRemoved,
        UserTarkinRemoved,
        UserRoleDeactivated,
        CorrespondentOrganizationDeleted,
        CorrespondentOrganizationCreated,
        UserPermissionCreated,
        UserPermissionDisabled,
        UserPasswordChanged,
        ProfilePictureChanged,
        UserPermissionDeleted,
        UserPermissionUpdated,
        SearchUsers,

        #endregion
        #region Transaction
        TransactionListViewed,
        TransactionSave,
        TransactionUpdate,
        #endregion
        #region Document
        DocumentView,
        DocumentSave,
        #endregion
        #region Delegation
        DelegationListViewed,
        DelegationSave,
        DelegationTransactionActionAttachmentSave,
        DelegationTransactionActionRecipientsSave,
        #endregion
        #region Organizations
        OrganizationCreated,
        OrganizationUpdate,
        OrganizationDeleted,
        SearchOrganizations,
        OrganizationListViewed,
        OrganizationEdit,
        #endregion

        #region Roles
            RoleListViewed,
        #endregion

        #region Transactions
        TransactionRegistered,
        TransactionMapViewd,
        DraftActivated,
        TransactionUpdated,
        RelatedTransactionAdded
        #endregion
    }
}
