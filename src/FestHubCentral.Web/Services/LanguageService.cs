using System.Globalization;
using FestHubCentral.Web.Services.Interfaces;

namespace FestHubCentral.Web.Services;

public class LanguageService : ILanguageService
{
    private CultureInfo _currentCulture = CultureInfo.CurrentCulture;

    public CultureInfo CurrentCulture => _currentCulture;

    public event EventHandler? CultureChanged;

    public Task SetCultureAsync(string culture)
    {
        var cultureInfo = new CultureInfo(culture);
        _currentCulture = cultureInfo;
        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        CultureChanged?.Invoke(this, EventArgs.Empty);

        return Task.CompletedTask;
    }

    public Task<string> GetCultureAsync()
    {
        return Task.FromResult(_currentCulture.Name);
    }
}
