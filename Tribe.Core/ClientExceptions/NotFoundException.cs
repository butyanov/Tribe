using System.Net;
using Messenger.Core;

namespace Tribe.Core.ClientExceptions;

public class NotFoundException<T> : ClientException
{
    public NotFoundException() : base(
        ErrorCodes.NotFoundError, (int)HttpStatusCode.NotFound)
    {
        PlaceholderData.Add("EntityName", typeof(T).Name);
    }
}