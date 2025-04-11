using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Componentes;
using PortalAG_V2.Shared.Model.DispositivosModel;
using PortalAG_V2.Shared.Services;

namespace PortalAG_V2.Pages.PermisosAPP;

public partial class Dispositivos
{
    private ClientFactory conexion;

    MostrarDispositivos mostrarDispoDesde = new MostrarDispositivos();
    MostrarDispositivos mostrarDispoDesde1 = new MostrarDispositivos();

    public List<MostrarDispositivos> _listArea = new List<MostrarDispositivos>();
    public List<BuscarPorTipo> _listDispo1 = new List<BuscarPorTipo>();

    bool Loading = false;

    private const string urlArea = "/api/v1/Dispositivos/MostrarDispositivo";
    private const string urlPortipo = "/api/v1/Dispositivos/BuscarPorTipo";

    protected override async Task OnInitializedAsync()
    {
        conexion = new MainServices().PermisosAPPPublic;
        var auxArea = await conexion.HttpClientInstance.GetAsync($"{urlArea}");
        try
        {
            _listArea = JsonConvert.DeserializeObject<List<MostrarDispositivos>>(await auxArea.Content.ReadAsStringAsync());
        } 
        catch ( Exception ex )
        {
            _snackBar.Add("Error al deserealizar dispositivos!", Severity.Error);
        }
    }

    public async Task MostrarDispositivos(MostrarDispositivos data)
    {
        mostrarDispoDesde1 = data;
        conexion = new MainServices().PermisosAPPPublic;
        Loading = true;
        var auxDispo = await conexion.HttpClientInstance.GetAsync($"{urlPortipo}/{mostrarDispoDesde1.IDTipo}");
        try
        {
            _listDispo1 = JsonConvert.DeserializeObject<List<BuscarPorTipo>>(await auxDispo.Content.ReadAsStringAsync());
            Loading = false;
            StateHasChanged();
        }
        catch ( Exception ex )
        {
            Loading = false;
            _snackBar.Add("Error al deserealizar dispositivos", Severity.Error);
        }
    }

    public async Task SeleccionarArea(MostrarDispositivos data)
    {
        mostrarDispoDesde = data;
    }

    private async void clickDesde(DataGridRowClickEventArgs<BuscarPorTipo> args)
    {
        var parameters = new DialogParameters
        {
            { nameof(DispositivosDialog.args), args.Item}
        };

        var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth= MaxWidth.ExtraSmall, FullWidth= false };

        var dialogo = _dialogService.Show<DispositivosDialog>("", parameters, options);
        var result = await dialogo.Result;
        if (!result.Cancelled)
        {
            await MostrarDispositivos(mostrarDispoDesde1);
            StateHasChanged();
        }
    }

    Func<MostrarDispositivos, string> convertDescripcion = p => p.Descripcion.ToString();
}


