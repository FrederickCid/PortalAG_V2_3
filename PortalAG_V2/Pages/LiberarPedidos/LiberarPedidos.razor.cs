using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Auth;
using PortalAG_V2.Componentes;
using PortalAG_V2.Shared.Model.LiberarPedidos;
using PortalAG_V2.Shared.Model.SolicitudMovimiento;
using PortalAG_V2.Shared.Services;
using System.Net.Http.Json;

namespace PortalAG_V2.Pages.LiberarPedidos;

partial class LiberarPedidos
{
    private const string urlConsultaPedidos = "api/v2/Despacho/ConsultaPedidoLiberar";
    private ClientFactory Baliza;

    private List<ResponseConsultaPedidos> ConsultaList = new List<ResponseConsultaPedidos>();
    MudDatePicker? _DatePickerInicio;
    MudDatePicker? _DatePickerfin;
    DateTime? dateToday = DateTime.Today;
    DateTime? dateNull = null;
    bool Loading = false;

    private string fInicio;
    private string fFin;
    private string _idCliente { get; set; }
    private int _nroPedido { get; set; } = 0;


    protected override async Task OnInitializedAsync()
    {
        await BusquedaInicial();
    }

    private async Task BusquedaInicial()
    {
        Loading = true;
        Baliza = new MainServices().Baliza;

        RequestConsultaPedidos data = new RequestConsultaPedidos()
        {
            nroPedido = 0,
            fechaInicio = "",
            fechaFin = "",
            idCliente = "",
            idUsuario = ""
        };

        var auxPedidos = await Baliza.HttpClientInstance.PostAsJsonAsync<RequestConsultaPedidos>(urlConsultaPedidos, data);
        if (auxPedidos.IsSuccessStatusCode)
        {
            ConsultaList = JsonConvert.DeserializeObject<List<ResponseConsultaPedidos>>(await auxPedidos.Content.ReadAsStringAsync());
            Loading = false;
            StateHasChanged();
        }
        else
        {
            _snackBar.Add("Error en consultar API pedidos", Severity.Error);
            Loading = false;
        }
    }

    private async Task BuscarDocumento()
    {
        Loading = true;
        var user = await _authenticationManager.CurrentUser();
        Baliza = new MainServices().Baliza;

        RequestConsultaPedidos data = new RequestConsultaPedidos()
        {
            nroPedido = 0,
            fechaInicio = "",
            fechaFin = "",
            idCliente = "",
            idUsuario = user.GetUserId()
        };

        if (!String.IsNullOrEmpty(_idCliente))
        {
            data.idCliente = _idCliente;
        }

        if (_nroPedido != 0)
        {
            data.nroPedido = _nroPedido;
        }

        if (dateToday.ToString() != "")
        {
            data.fechaFin = dateToday.Value.ToString("dd/MM/yy");
        }
        if (dateNull.ToString() != "")
        {
            data.fechaInicio = dateNull.Value.ToString("dd/MM/yy");
        }

        var auxBusqueda = await Baliza.HttpClientInstance.PostAsJsonAsync<RequestConsultaPedidos>(urlConsultaPedidos, data);
        if (auxBusqueda.IsSuccessStatusCode)
        {
            ConsultaList = JsonConvert.DeserializeObject<List<ResponseConsultaPedidos>>(await auxBusqueda.Content.ReadAsStringAsync());
            StateHasChanged();
            Loading = false;
        }
        else
        {
            _snackBar.Add("Error en consultar API pedidos", Severity.Error);
            Loading = false;
        }
    }

    private async Task verDetalle(ResponseConsultaPedidos args)
    {
        var parameters = new DialogParameters
            {
                { nameof(DetallePedidoLP.Detalle), args},
                { nameof( DetallePedidoLP.tipoDocumentoNumero), args.TipoDocumento }
            };

        var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.ExtraLarge, FullWidth = false };

        var dialogo = _dialogService.Show<DetallePedidoLP>("Detalle de facturas", parameters, options);
        var result = await dialogo.Result;

        if (!result.Cancelled)
        {
            BusquedaInicial();
            _snackBar.Add("Pedido Liberado", Severity.Success);
        }
    }

}