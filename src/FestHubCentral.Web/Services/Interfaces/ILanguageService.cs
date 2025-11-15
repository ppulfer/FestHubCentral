using System.Globalization;

namespace FestHubCentral.Web.Services.Interfaces;

public interface ILanguageService
{
    CultureInfo CurrentCulture { get; }
    event EventHandler? CultureChanged;
    Task SetCultureAsync(string culture);
    Task<string> GetCultureAsync();
}
