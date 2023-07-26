using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.DAL.Domains;
using System;

public interface IUserRepository: IRepository<User>
{
    User GetUserWithRolesAndPermissions(string Username);
}
