using System.Net;

namespace Tribe.Core.ClientExceptions;

public class UnauthorizedException(string message) : ClientException(message, (int)HttpStatusCode.Unauthorized);