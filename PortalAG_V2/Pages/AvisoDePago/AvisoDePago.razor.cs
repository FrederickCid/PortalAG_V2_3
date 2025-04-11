using Newtonsoft.Json;
using PortalAG_V2.Auth;
using PortalAG_V2.Shared.Services;
using PortalAG_V2.Shared.Model.AvisoDePago;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.Popups;
using System.Net.Http.Json;

namespace PortalAG_V2.Pages.AvisoDePago;

partial class AvisoDePago
{

    #region ULR
    //private ClientFactory conexion;
    MainServices service;
    private const string UrlCliente = "api/v2/AvisodePagos/BuscarCliente";
    private const string UrlAyudaCliente = "api/v2/AvisodePagos/ayudaCliente";
    private const string UrlAvisoPago = "api/v2/AvisodePagos/ListadoPagos";
    private const string UrlListadoBancos = "api/v2/AvisodePagos/ConsultaBancos";
    private const string UrlListadoBancosAndes = "api/v2/AvisodePagos/ConsultaBancosAndes";
    private const string UrlEnvioAviso = "api/v2/AvisodePagos/IngresoAviso";
    #endregion

    public List<BancoModel> Bancos = new List<BancoModel>();
    public List<BancoAndesModel> BancosAndes = new List<BancoAndesModel>();
    public List<AvisoPagoModel> listadoPagos = new List<AvisoPagoModel>();

    public List<AvisoPagoModel> listadoPagosDetalle = new List<AvisoPagoModel>();

    public string idCliente;
    public string NombreCliente;
    public bool dlgFormulario = false;
    public bool dlgAyuda = false;
    public bool dlgPorCliente = false;
    bool isFileSelected = false;



    public SfDialog formularioAviso;
    public SfGrid<ClienteModel> AyudaGrid;
    public int currentOpenTab { get; set; }

    public AvisoPagoModel AvisoPagoModel = new AvisoPagoModel();

    #region dialog formulario ingreso
    DateTime staticaFecha = DateTime.Now;
    AvisoPagoModel avisoPago = new AvisoPagoModel();
    #endregion


    #region Dialog Ayuda
    public string txNombreCliente = "";
    public string AyudaCheckC = "0";
    public List<ClienteModel> ClienteAyuda = new List<ClienteModel>();
    #endregion

    string _IDUser = "";
    

    protected override async Task OnInitializedAsync()
    {
        var user = await _authenticationManager.CurrentUser();
        var IDuse = user.GetFirstName();
        _IDUser = user.GetUserId();
        service = new MainServices();
        service.ConectionService.HttpClientInstance.DefaultRequestHeaders.Add("ID", _IDUser);
        await Cargar();
        await CargaBancos();
        await CargaBancosAndes();
        avisoPago.Fecha = DateTime.Now;
    }

    private async Task CargaBancos()
    {
        HttpClient Coneccion = service.ConectionService.HttpClientInstance;
        var response = await Coneccion.GetAsync(UrlListadoBancos);
        if (response.IsSuccessStatusCode)
        {
            Bancos = await response.Content.ReadFromJsonAsync<List<BancoModel>>();
        }
    }
    private async Task CargaBancosAndes()
    {
        HttpClient Coneccion = service.ConectionService.HttpClientInstance;
        var response = await Coneccion.GetAsync(UrlListadoBancosAndes);
        if (response.IsSuccessStatusCode)
        {
            BancosAndes = await response.Content.ReadFromJsonAsync<List<BancoAndesModel>>();
        }
    }

    private async Task Cargar()
    {
        HttpClient Coneccion = service.ConectionService.HttpClientInstance;
        var response = await Coneccion.GetAsync(UrlAvisoPago);
        if (response.IsSuccessStatusCode)
        {
            listadoPagos = await response.Content.ReadFromJsonAsync<List<AvisoPagoModel>>();
        }
    }

    public async Task BuscarCliente()
    {
        if (string.IsNullOrWhiteSpace(idCliente))
        {
            DialogService.AlertAsync("Debe ingresar rut", "Error Rut Cliente");
        }
        else
        {
            var response = await service.ConectionService.HttpClientInstance.GetAsync(UrlCliente + $"/{idCliente}");
            if (response.IsSuccessStatusCode)
            {
                var aux = await response.Content.ReadFromJsonAsync<ClienteModel>();
                NombreCliente = aux.RazonSocial;
            }
            else
            {
                DialogService.AlertAsync("Rut cliente no encontrado", "Error Rut Cliente");
                NombreCliente = "";
            }
        }
    }

    public async Task AyudaCliente()
    {
        dlgAyuda = true;
        txNombreCliente = "";
        ClienteAyuda = new List<ClienteModel>();
    }

    public async Task BuscarPorCliente()
    {

        if (!string.IsNullOrWhiteSpace(idCliente) && !string.IsNullOrEmpty(NombreCliente))
        {
            HttpClient Coneccion = service.ConectionService.HttpClientInstance;
            var response = await Coneccion.GetAsync(UrlAvisoPago + $"/{idCliente}");
            if (response.IsSuccessStatusCode)
            {
                //listadoPagosDetalle = JsonConvert.DeserializeObject<List<AvisoPagoModel>>(await response.Content.ReadAsStringAsync());
                listadoPagosDetalle = await response.Content.ReadFromJsonAsync<List<AvisoPagoModel>>();
            }
            else
            {
                listadoPagosDetalle = new List<AvisoPagoModel>();
            }
            dlgPorCliente = true;
        }
        else
        {
            DialogService.AlertAsync("Debe ingresar cliente", "Error Rut Cliente");
        }
    }

    public async Task IngresarAvisoPago()
    {
        if (string.IsNullOrWhiteSpace(idCliente))
        {
            DialogService.AlertAsync("Debe ingresar cliente", "Error Cliente");
        }
        else if (string.IsNullOrEmpty(NombreCliente))
        {
            DialogService.AlertAsync("Cliente no encontrado", "Error Cliente");
        }
        else
        {
            dlgFormulario = true;
            avisoPago = new AvisoPagoModel();
            avisoPago.IDCliente = idCliente;
            avisoPago.RazonSocial = NombreCliente;
            avisoPago.IDTipoPago = 13;
            avisoPago.Valor = 0;
            avisoPago.Comentarios = "";
            avisoPago.Fecha = DateTime.Now;
        }
    }

    public async Task ClkAyudaCliente()
    {
        if (string.IsNullOrWhiteSpace(txNombreCliente))
        {
            DialogService.AlertAsync("texto a buscar vacio", "Error");
        }
        else if (string.IsNullOrWhiteSpace(AyudaCheckC))
        {
            DialogService.AlertAsync("selecione tipo de busquedad", "Error");
        }
        else
        {
            var response = await service.ConectionService.HttpClientInstance.GetAsync(UrlAyudaCliente + $"/{txNombreCliente}/{AyudaCheckC}");
            if (response.IsSuccessStatusCode)
            {
                ClienteAyuda = await response.Content.ReadFromJsonAsync<List<ClienteModel>>();
                await AyudaGrid.Refresh();

            }
            else
            {
                DialogService.AlertAsync("Rut cliente no encontrado", "Error Rut Cliente");
                NombreCliente = "";
            }
        }
    }

    public async Task limpiarDatos()
    {
        NombreCliente = "";
    }
    public void RowSelectCliente(RowSelectEventArgs<ClienteModel> args)
    {
        idCliente = args.Data.IDCliente;
        NombreCliente = args.Data.RazonSocial;

        dlgAyuda = false;
        StateHasChanged();
    }
    public void onChangeComboBoxBanco(Syncfusion.Blazor.DropDowns.ChangeEventArgs<string, BancoModel> args)
    {
        DialogService.AlertAsync(args.Value.ToString());
    }

    #region Funcionalidad
    public async Task EnvioAviso()
    {
        bool prompt = await DialogService.ConfirmAsync("Enviar Aviso Pago");
        if (prompt)
        {
            var response = await service.ConectionService.HttpClientInstance.PostAsJsonAsync<AvisoPagoModel>(UrlEnvioAviso, avisoPago);
            //var response = await service.ConectionService.HttpClientInstance.PostAsJsonAsync(UrlEnvioAviso, avisoPago);
            if (response.IsSuccessStatusCode)
            {
                var aux = await response.Content.ReadFromJsonAsync<ClienteModel>();
                DialogService.AlertAsync("Ingresado con exito");
                avisoPago = new AvisoPagoModel();
                CerrarFAviso();
                NombreCliente = "";
                idCliente = "";
                await Cargar();
                isFileSelected = false;
            }
            else
            {
                DialogService.AlertAsync("Error al ingresar", "Aviso no ingresado");
                NombreCliente = "";
                isFileSelected = false;
            }
        }
        else
        {

        }
        StateHasChanged();
    }

    private async Task OnChangeAsync(UploadChangeEventArgs args)
    {
        avisoPago.Imagenes = new List<Archivo> { };

        foreach (var file in args.Files)
        {
            avisoPago.Imagenes.Add(new Archivo()
            {
                Stream = Convert.ToBase64String(file.Stream.ToArray()),
                FileInfo = file.FileInfo.Name
            });
        }
        isFileSelected = avisoPago.Imagenes.Any();

    }

    private async void CerrarFAviso()
    {
        dlgFormulario = false;
        avisoPago = new AvisoPagoModel();
        isFileSelected = false;
        avisoPago.Fecha = DateTime.Now;
    }
    #endregion

    private void SelectedItemChanged(int arg)
    {
        switch (arg)
        {
            case 0:
                avisoPago.IDTipoPago = 13;
                break;
            case 1:
                avisoPago.IDTipoPago = 7;
                break;
        }
        avisoPago.IDBanco = 0;
        avisoPago.NroCuenta = "";
        avisoPago.NroComprobante = "";
        avisoPago.IDBancoOrigen = 0;
        avisoPago.NroCuentaOrigen = "";

        StateHasChanged();
    }

    internal void SelecionBanco(Syncfusion.Blazor.DropDowns.SelectEventArgs<BancoAndesModel> args)
    {
        avisoPago.IDBanco = args.ItemData.IDBanco;
        avisoPago.NroCuenta = args.ItemData.CuentaCorriete;
    }
}
