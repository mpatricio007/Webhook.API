using System.Runtime.CompilerServices;

namespace Webhook.API.Model;

public sealed record Fine(Guid Id, string ClientId, string DriverName, decimal Amount, DateTime CreatAt);

public sealed record CreateFineRequest(string ClientId, string DriverName, decimal Amount);