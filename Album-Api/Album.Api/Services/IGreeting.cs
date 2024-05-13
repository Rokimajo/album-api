namespace Album.Api.Services;

public interface IGreeting
{
    public Task<string> ReturnHello(string name);
}