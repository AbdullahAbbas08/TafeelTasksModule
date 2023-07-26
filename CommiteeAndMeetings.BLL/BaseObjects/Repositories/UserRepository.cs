using CommiteeAndMeetings.BLL.BaseObjects;
using CommiteeAndMeetings.BLL.Contexts;
using CommiteeAndMeetings.DAL.Domains;
using IHelperServices;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using static UserRepository;

/// <summary>
/// Summary description for Class1
/// </summary>

    public class UserRepository : BaseRepository<User>, IUserRepository
{

    private MasarContext Db { get; set; }
    ISessionServices _sessionServices;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private ISession _session => _httpContextAccessor.HttpContext.Session;
    //  public ITransactionActionRecipientRepository TransactionActionRecipientRepository { get; set; }
    public UserRepository(MasarContext mainDbContext, ISessionServices sessionServices, IHttpContextAccessor httpContextAccessor) : base(mainDbContext, sessionServices, httpContextAccessor)
    {
        Db = mainDbContext;
        _httpContextAccessor = httpContextAccessor;
        //   TransactionActionRecipientRepository = new TransactionActionRecipientRepository(mainDbContext, sessionServices);
    }
    public User GetUserWithRolesAndPermissions(string Username)
    {
        return base._dbContext.Users
            //.Include(x => x.UserRoles)
            //.ThenInclude(x => x.Role)
            //.ThenInclude(x => x.RolePermissions)
            //.ThenInclude(x => x.Permission)
            .FirstOrDefault(x => x.Username.ToUpper() == Username.ToUpper());
    }
}
