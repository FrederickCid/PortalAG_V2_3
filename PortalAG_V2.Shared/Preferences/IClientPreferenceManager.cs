using PortalAG_V2.Shared.Managers;
using MudBlazor;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Preferences
{
    public interface IClientPreferenceManager : IPreferenceManager
    {
        Task<MudTheme> GetCurrentThemeAsync();

        Task<bool> ToggleDarkModeAsync();
    }
}