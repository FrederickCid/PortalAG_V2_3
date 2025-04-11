using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Shared.Model.PermisosAPPDTO;
using PortalAG_V2.Shared.Services;

namespace PortalAG_V2.Pages.PermisosAPP;

public partial class PermisosAPP
{
    #region Variables

    private ClientFactory conexion;

    public bool mostrarnombres = true;
    private bool _buscando = false;

    public string _usuario { get; set; }

    ConsultarIDAPPModel mostrarAppPrincipal = new ConsultarIDAPPModel();
    ConsultarIDAPPModel mostrarSubAppPrincipal = new ConsultarIDAPPModel();
    ConsultarIDAPPModel dataAppprincipal = new ConsultarIDAPPModel();
    ConsultarIDAPPModel dataAppSub = new ConsultarIDAPPModel();
    MostrarNombresModel MostrarNombresDesde = new MostrarNombresModel();

    public List<ConsultarIDAPPModel> _listApp = new List<ConsultarIDAPPModel>();
    public List<ConsultarIDAPPModel> _listSubApp = new List<ConsultarIDAPPModel>();
    public List<MostrarNombresModel> _listNombres = new List<MostrarNombresModel>();
    public List<MostrarAccesosDTO> _listAccesoUsuario = new List<MostrarAccesosDTO>();

    private List<ConsultarPermisosUsuario> listPermisos = new List<ConsultarPermisosUsuario>();


    #endregion

    #region EndPoints

    private const string urlConsultaPermisosAPP = "api/v2/PermisosAPP/ConsultaPermisos";
    private const string urlConsultaPermisoUsuario = "api/v2/PermisosAPP/ConsultarPermisosUsuarios";
    private const string urlCRUDPermisos = "api/v2/PermisosAPP/CRUDPermisos";
    private const string urlMostrarNombres = "api/v1/ControlUsuarios/MostrarNombres";
    private const string urlMostrarAccesosUsuarios = "/api/v1/ControlUsuarios/MostrarAccesoUsuario";

    #endregion

    protected override async Task OnInitializedAsync()
    {
        conexion = new MainServices().SolMovimiento;
        var auxConsulta = await conexion.HttpClientInstance.GetAsync($"{urlConsultaPermisosAPP}/1/0");
        try
        {
            _listApp = JsonConvert.DeserializeObject<List<ConsultarIDAPPModel>>(await auxConsulta.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            _snackBar.Add("Error al deserealizar", Severity.Error);
        }
    }


    public async Task ConsultarUsuario()
    {
        try
        {
            if (!string.IsNullOrEmpty(_usuario))
            {
                conexion = new MainServices().PermisosAPP;
                var auxNombres = await conexion.HttpClientInstance.GetAsync($"{urlMostrarNombres}/{_usuario}");
                if (auxNombres.IsSuccessStatusCode)
                {
                    _listNombres = JsonConvert.DeserializeObject<List<MostrarNombresModel>>(await auxNombres.Content.ReadAsStringAsync());
                    if (_listNombres.Count > 0)
                    {
                        mostrarnombres = false;
                        MostrarNombresDesde.Nombres = "";
                        MostrarNombresDesde.ApellidoPaterno = "";
                        _snackBar.Add("Usuario Encontrado", Severity.Success);
                    }
                    else
                    {
                        _snackBar.Add("Error al consultar Nombre", Severity.Error);
                    }
                }
            }
            else
            {
                _snackBar.Add("Error al consultar Nombre", Severity.Error);
            }
        }
        catch (Exception ex)
        {
            _snackBar.Add($"Error al consultar Nombre - {ex.Message}", Severity.Error);
        }
    }

    private async Task mostrarSubApp(ConsultarIDAPPModel data)
    {
        dataAppprincipal = data;

        var auxDataPrincipal = await conexion.HttpClientInstance.GetAsync($"{urlConsultaPermisosAPP}/2/{dataAppprincipal.ID}");
        if (auxDataPrincipal.IsSuccessStatusCode)
        {
            try
            {
                _listSubApp = JsonConvert.DeserializeObject<List<ConsultarIDAPPModel>>(await auxDataPrincipal.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                _snackBar.Add("Error al deserealizar Sub App", Severity.Error);
            }
        }
    }

    public async Task MostrarAccesos(MostrarNombresModel data)
    {
        MostrarNombresDesde = data;
        conexion = new MainServices().SolMovimiento;
        _buscando = true;

        if (!System.String.IsNullOrEmpty(MostrarNombresDesde.IDUsuario))
        {
            var auxBuscarPermisos = await conexion.HttpClientInstance.GetAsync($"{urlConsultaPermisoUsuario}/{MostrarNombresDesde.IDUsuario}");
            try
            {
                if (auxBuscarPermisos.IsSuccessStatusCode)
                {
                    listPermisos = JsonConvert.DeserializeObject<List<ConsultarPermisosUsuario>>(await auxBuscarPermisos.Content.ReadAsStringAsync());
                    StateHasChanged();
                    _buscando = false;
                }
                else
                {
                    _snackBar.Add("Error al buscar usuario!", Severity.Error);
                }
            }
            catch (Exception ex)
            {
                _snackBar.Add("Error: " + ex.Message, Severity.Error);
            }
        }
        else
        {
            _snackBar.Add("Por favor ingrese un usuario!", Severity.Warning);
        }
    }

    private async Task SeleccionarSubApp(ConsultarIDAPPModel data)
    {
        dataAppSub = data;
    }

    private async Task ActivarPermiso()
    {
        var auxInfoApp = await conexion.HttpClientInstance.GetAsync($"{urlCRUDPermisos}/2/{dataAppprincipal.ID}/{dataAppSub.ID}/{MostrarNombresDesde.IDUsuario}");
        await MostrarAccesos(MostrarNombresDesde);
        _snackBar.Add("Permiso concedido", Severity.Success);
    }
    private async Task QuitarPermiso()
    {
        var auxInfoApp = await conexion.HttpClientInstance.GetAsync($"{urlCRUDPermisos}/3/{dataAppprincipal.ID}/{dataAppSub.ID}/{MostrarNombresDesde.IDUsuario}");
        await MostrarAccesos(MostrarNombresDesde);
        _snackBar.Add("Permiso elimando", Severity.Success);
    }

    Func<ConsultarIDAPPModel, string> convertApp = p => p.Nombre;
    Func<ConsultarIDAPPModel, string> convertSubApp = p => p.Nombre;
    Func<MostrarNombresModel, string> convertNombres = p => p.Nombres;
}
