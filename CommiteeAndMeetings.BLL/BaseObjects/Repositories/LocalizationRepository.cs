using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.BLL.Contexts;
using CommiteeAndMeetings.DAL.Domains;
using IHelperServices;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.BLL.BaseObjects.Repositories
{
    public class LocalizationRepository : BaseRepository<Localization>, ILocalizationRepository
    {
        private MasarContext _context { get; set; }
        ISessionServices _sessionServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public LocalizationRepository(MasarContext mainDbContext, ISessionServices sessionServices, IHttpContextAccessor httpContextAccessor) : base(mainDbContext, sessionServices, httpContextAccessor)
        {
            _context = mainDbContext;
            _httpContextAccessor = httpContextAccessor;
            //   TransactionActionRecipientRepository = new TransactionActionRecipientRepository(mainDbContext, sessionServices);
        }

        public string GetByKey(string KeyCode, string culture)
        {
            string Name = "";
            if (culture.ToLower() == "ar")
            {
                Name = _context.Localizations.Where(w => w.Key == KeyCode).FirstOrDefault()?.ValueAr;

            }
            else if (culture.ToLower() == "en")
            {
                Name = _context.Localizations.Where(w => w.Key == KeyCode).FirstOrDefault()?.ValueEn;


            }
            else
            {
                Name = _context.Localizations.Where(w => w.Key == KeyCode).FirstOrDefault()?.ValueFn;


            }
            return Name;
        }
    }
}

