namespace androkat.application.Interfaces;

public interface IAuthService
{
    bool IsAuthenticated(string email);
}