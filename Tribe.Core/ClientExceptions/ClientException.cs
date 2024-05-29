namespace Tribe.Core.ClientExceptions;

public class ClientException(string message) : Exception(message)
{
    public ClientException(string message, int statusCode) : this(message)
    {
        StatusCode = statusCode;
    }

    public int StatusCode { get; set; } = 400;

    /// <summary>
    ///     Данные для локализации ошибок на клиенте
    /// </summary>
    public Dictionary<string, string> PlaceholderData { get; } = new();
}