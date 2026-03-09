using CGG.Application.Settings;
using Microsoft.Extensions.Options;

namespace CGG.Infrastructure.Services;

/// <summary>
/// Attaches the Monobank merchant token to every outgoing request via X-Token header.
/// </summary>
public class MonobankAuthHandler : DelegatingHandler
{
    private readonly MonobankSettings _settings;

    public MonobankAuthHandler(IOptions<MonobankSettings> settings)
    {
        _settings = settings.Value;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        request.Headers.Add("X-Token", _settings.Token);
        return base.SendAsync(request, cancellationToken);
    }
}
