using agDataAccess.Models;
using Microsoft.Extensions.Options;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Componentes.EstadoPedido;
using PortalAG_V2.Shared.Model.EstadoPedidos;
using PortalAG_V2.Shared.Services;
using Syncfusion.Blazor.Grids;

namespace PortalAG_V2.Pages.EstadosPedidos;

public partial class Historicos
{
    SfGrid<EstadoPedidosNoMOD> Grid;
    MainServices service;
    MudDatePicker? _DatePickerInicio;
    MudDatePicker? _DatePickerfin;
    DateTime? dateToday = DateTime.Today;
    DateTime? dateNull = null;
    private string fInicio;
    private string fFin;
    private bool _processing = false;
    private bool _validateData = true;
    string UrlHistoricos = "GetEstadoPedidos/Historicos";
    private List<EstadoPedidosNoMOD> ListaHistoricos = new List<EstadoPedidosNoMOD>();
    private List<string> ButtonsGrid = new List<string>() { "ExcelExport" };
    private bool showCallAlert = true;
    bool Loading = false;


    public async Task CargarDatos(string inicio, string fin)
    {
        try
        {
            Loading = true;            
            service = new MainServices();
            var lista = await service.EstadoPedido.HttpClientInstance.GetAsync($"api/v2/{UrlHistoricos}/{inicio}/{fin}/");
            if (lista.IsSuccessStatusCode)
            {

                ListaHistoricos = JsonConvert.DeserializeObject<List<EstadoPedidosNoMOD>>(await lista.Content.ReadAsStringAsync());
                snakBarCreation("Listo!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                Loading = false;
            }
            else
            {                

                ListaHistoricos = new List<EstadoPedidosNoMOD>();
                Loading = false;
            }
            StateHasChanged();
        }
        catch (Exception e)
        {
            string mensaje = e.Message;
        }
    }


    public async Task ProcessSomething()
    {
        _processing = true;
        ListaHistoricos = new List<EstadoPedidosNoMOD>();
        await CargarDatos(fInicio, fFin);
        _processing = false;
    }

    public async Task ToolbarClick(Syncfusion.Blazor.Navigations.ClickEventArgs args)
    {

        if (args.Item.Id == "Grid_excelexport")
        {
            if (ListaHistoricos.Count() > 0)
            {
                snakBarCreation("Exportando...", Defaults.Classes.Position.BottomStart, Severity.Info, 5000);
                _processing = true;
                await Grid.ExportToExcelAsync();
                _processing = false;
                snakBarCreation("Listo!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
            }
            else
            {
                snakBarCreation("No hay datos para exportar", Defaults.Classes.Position.BottomStart, Severity.Warning, 1000);
            }
        }
    }

    #region Funcion Alerta
    private void CerrarAlerta()
    {
        showCallAlert = !showCallAlert;
    }
    #endregion


    public void DobleClick(RowSelectEventArgs<EstadoPedidosNoMOD> args)
    {



        var parameters = new DialogParameters<MudBlazorDialogCustom>();
        parameters.Add(x => x.Nombre, $"{args.Data.Vendedor}");
        parameters.Add(x => x.Usuario, $"{args.Data.IDVendedor}");
        parameters.Add(x => x.RSocial, $"{args.Data.RazonSocial}");
        parameters.Add(x => x.RutRS, $"{args.Data.IDCliente}");
        parameters.Add(x => x.option, $"otro");
        parameters.Add(x => x.FechaSolicitud, $"{args.Data.FechaTerminoAutorizacion}");
        parameters.Add(x => x.Lista, args.Data);
        //parameters.Add(x => x.ListaBulos, args.Data.Bultos);

        var options = new MudBlazor.DialogOptions() { CloseButton = false, FullWidth = true, MaxWidth = MaxWidth.ExtraExtraLarge };

        DialogService.Show<MudBlazorDialogCustom>($"Pedido Numero: {args.Data.NroDocumento}", parameters, options);

    }
    #region SnackBar
    private void snakBarCreation(string msj, string position, MudBlazor.Severity style, int duration)
    {
        _snackBar.Configuration.VisibleStateDuration = duration;
        _snackBar.Configuration.PositionClass = position;
        _snackBar.Add(msj, style);
    }
    #endregion
}
