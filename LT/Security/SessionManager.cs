using LT.Data;
using LT.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace LT.Security
{
    public class SessionManager
    {
        private readonly Dictionary<string, string> activeSessions = new Dictionary<string, string>();
        private readonly dbContext dbContext;

        public SessionManager(dbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool IsUserLoggedIn(string userid)
        {
            return dbContext.UserSession.AsNoTracking().Any(s => s.userid == userid);
        }

        public bool IsUserLoggedIn(string userid, string sessionid)
        {
            return dbContext.UserSession.AsNoTracking().Any(s => s.userid == userid && s.sessionid == sessionid);
        }

        public void AddSession(string sessionid, string userid)
        {
            dbContext.UserSession.Add(new UserSession { sessionid = sessionid, userid = userid });
            dbContext.SaveChanges();
        }

        public void RemoveSession(string userid)
        {
            var sessions = dbContext.UserSession.AsNoTracking().Where(s => s.userid == userid).ToList();
            if (sessions != null)
            {
                foreach (var session in sessions)
                {
                    dbContext.UserSession.Remove(session);
                }
                dbContext.SaveChanges();
            }
        }
    }
}
