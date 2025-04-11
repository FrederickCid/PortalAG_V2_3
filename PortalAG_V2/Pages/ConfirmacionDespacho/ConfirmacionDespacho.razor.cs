using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Auth;
using PortalAG_V2.Componentes;
using PortalAG_V2.Shared.Model.AvisoDePago;
using PortalAG_V2.Shared.Model.SolicitudMovimiento;
using PortalAG_V2.Shared.Services;
using System.Linq;
using static MudBlazor.CategoryTypes;

namespace PortalAG_V2.Pages.ConfirmacionDespacho;

partial class ConfirmacionDespacho
{
    #region Variables

    private ClientFactory conexion;

    private Timer? timer;
    Loading? loading;
    private bool _processing = false;
    bool _color = false;

    string _IDUser = "";
    private int Count = 240;

    private HashSet<ConfirmacionDespachoModel> selectRow = new HashSet<ConfirmacionDespachoModel>();
    private List<ConfirmacionDespachoModel> _listConfirmacion = new List<ConfirmacionDespachoModel>();

    #endregion

    #region Endpoints
    private const string urlListado = "api/v2/ConfirmacionDespacho/Listado";
    #endregion

    protected override async Task OnInitializedAsync()
    {
        StartCountdown();
        await ConsultarLista();
    }

    public void StartCountdown()
    {
        try
        {
            timer = new Timer(new TimerCallback(async (e) =>
            {
                if (Count <= 0)
                {
                    await Actualizar();
                    Count = 240;
                }
                else
                {
                    Count--;
                }
                await InvokeAsync(StateHasChanged);
            }), null, 1000, 1000);
        }
        catch (Exception ex)
        {
            string mensaje = ex.Message;
        }
    }

    public async Task ConsultarLista()
    {
        var user = await _authenticationManager.CurrentUser();
        _IDUser = user.GetUserId();

        conexion = new MainServices().Formularios;
        var auxListado = await conexion.HttpClientInstance.GetAsync($"{urlListado}/{_IDUser}");
        if (auxListado.IsSuccessStatusCode)
        {
            try
            {
                _listConfirmacion = JsonConvert.DeserializeObject<List<ConfirmacionDespachoModel>>(await auxListado.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                _snackBar.Add("Error al deserealizar listado: " + ex.Message, Severity.Error);
            }
        }
        else
        {
            _snackBar.Add("Error al consultar listado", Severity.Error);
        }
    }

    public string ColorSelect(ConfirmacionDespachoModel element, int rowNumber)
    {
        return selectRow.Contains(element) ? "selected" : string.Empty;
    }

    private async void ClickRow(DataGridRowClickEventArgs<ConfirmacionDespachoModel> args)
    {

        var parameters = new DialogParameters
        {
            { nameof(DialogConfirmacion.args), args.Item}
        };

        var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Small, FullWidth = false, DisableBackdropClick = true };

        var dConfirmacion = _dialogService.Show<DialogConfirmacion>($"Por favor confirmar datos para despacho", parameters, options);
        var result = await dConfirmacion.Result;

        if (!result.Cancelled)
        {
            loading?.Abrir();
            await Task.Delay(3000);
            await ConsultarLista();
            _snackBar.Add("Confirmacion Correcta", Severity.Info);
            StateHasChanged();
            loading?.Cerrar();
        }
        else
        {
            await ConsultarLista();
            StateHasChanged();
        }
    }

    public async Task Actualizar()
    {
        try
        {
            await ConsultarLista();
            StateHasChanged();
        }
        catch (Exception ex)
        {
            string mensaje = ex.Message;
            loading?.Cerrar();
        }
    }
}
