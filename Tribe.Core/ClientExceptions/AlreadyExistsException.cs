using Messenger.Core;

namespace Tribe.Core.ClientExceptions;

public class AlreadyExistsException : ClientException
{
    public AlreadyExistsException(string entityName) : base(ErrorCodes.AlreadyExistsError)
    {
        PlaceholderData.Add("EntityName", entityName);
    }
}