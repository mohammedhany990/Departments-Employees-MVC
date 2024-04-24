using Demo.DAL.Models;

namespace Demo.PL.Helpers
{
    public interface IEmailSettings
    {
        public void SendEmail(Email emai);
    }
}
