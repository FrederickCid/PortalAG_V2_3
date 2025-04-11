using PortalAG_V2.Shared.Wrapper;
using PortalAG_V2.Shared.Settings;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Managers
{
    public interface IPreferenceManager
    {
        Task SetPreference(IPreference preference);

        Task<IPreference> GetPreference();

        Task<IResult> ChangeLanguageAsync(string languageCode);
    }
}