using AdminPandel.Models;
using System.Threading.Tasks;

namespace AdminPandel.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}