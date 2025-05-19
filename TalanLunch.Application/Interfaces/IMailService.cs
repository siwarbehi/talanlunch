namespace TalanLunch.Application.Interfaces
{
    public interface IMailService
    {
        public Task SendEmailAsync(string to,
                                   string EmailToName,
                                   string subject,
                                   string body,
                                   CancellationToken cancellationToken = default);
    }
}
