using System.Net;
using Messenger.Core;

namespace Tribe.Core.ClientExceptions;

public class ForbiddenException : ClientException
{
    public ForbiddenException() : base(ErrorCodes.ForbiddenError, (int)HttpStatusCode.Forbidden)
    {
    }

    public ForbiddenException(string message) : base(message, (int)HttpStatusCode.Forbidden)
    {
    }
}