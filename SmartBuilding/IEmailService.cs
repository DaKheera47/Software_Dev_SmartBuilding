namespace SmartBuilding;

public interface IEmailService
{
    void SendMail(string to, string subject, string body);
}
