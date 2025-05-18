namespace TalanLunch.Application.Interfaces
{
    public interface IMailService
    {
        public Task SendEmailAsync(string EmailToId,
                                   string EmailToName,
                                   string EmailSubject,
                                   string EmailBody,
                                   CancellationToken cancellationToken = default);
    }
}
