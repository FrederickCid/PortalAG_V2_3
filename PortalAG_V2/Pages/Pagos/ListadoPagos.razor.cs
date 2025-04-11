using ClosedXML.Excel;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Auth;
using PortalAG_V2.Services;
using PortalAG_V2.Shared.Helpers;
using PortalAG_V2.Shared.Model.Pagos;
using PortalAG_V2.Shared.Services;
using Syncfusion.Blazor.Gantt.Internal;
using System;

namespace PortalAG_V2.Pages.Pagos
{
    partial class ListadoPagos
    {
        [Inject] ExportService exportService { get; set; }
        [Inject] IJSRuntime js { get; set; }

        DateTime? date1 = DateTime.Today.AddDays(-1);
        DateTime? date2 = DateTime.Today;
        public string TextValue { get; set; }
        public string usuarioSelect { get; set; } = "Todos";

        public List<PagosDTO> _listaPagos = new List<PagosDTO>();
        public PagosDTO _searchString = new PagosDTO();

        public List<string> _listUsuarios = new List<string>();

        private ClientFactory conexion;
        private const string urlListaPagos = "api/v2/Pagos";

        private bool _processing = false;
        private bool _processingDos = false;
        string _IDUser = "";
        protected override async Task OnInitializedAsync()
        {

            var user = await _authenticationManager.CurrentUser();
            var IDuse = user.GetFirstName();
            _IDUser = user.GetUserId();

            _listaPagos = new List<PagosDTO>();
            _searchString = new PagosDTO();
            date1 = DateTime.Now;//DateTime.Now.Date.AddDays(-1);
            date2 = DateTime.Now;
            
            if(_IDUser == "crar")
            {
                _listUsuarios = new List<string>() { "Todos", "Soledad", "Jenifer", "Alejandra" };
            }
            else
            {
                _listUsuarios = new List<string>() { IDuse };
            }
           



            conexion = new MainServices().Pagos;
            CargarDatosPago();

        }

        private async void  CargarDatosPago()
        {
            string auxDate1 = date1.Value.ToString("dd-MM-yyyy");
            string auxDate2 = date2.Value.ToString("dd-MM-yyyy");
            var auxListaPagos = await conexion.HttpClientInstance.GetAsync($"{urlListaPagos}/{auxDate1}/{auxDate2}/{usuarioSelect}");
            if (auxListaPagos.IsSuccessStatusCode)
            {
                try
                {
                    _listaPagos = JsonConvert.DeserializeObject<List<PagosDTO>>(await auxListaPagos.Content.ReadAsStringAsync());
                }
                catch (Exception ex)
                {
                    _snackBar.Add("Error deserealizar lista de pagos", Severity.Error);
                }
            }
            else
            {
                _snackBar.Add("Error al consultar lista de paagos", Severity.Error);
            }
            StateHasChanged();
        }

        private async Task SeleccionUsuario(String seleccion)
        {
            _processingDos = true;
            usuarioSelect = seleccion;
            string auxDate1 = date1.Value.ToString("dd-MM-yyyy");
            string auxDate2 = date2.Value.ToString("dd-MM-yyyy");
            var auxListaPagos = await conexion.HttpClientInstance.GetAsync($"{urlListaPagos}/{auxDate1}/{auxDate2}/{usuarioSelect}");
            if (auxListaPagos.IsSuccessStatusCode)
            {
                try
                {
                    _listaPagos = JsonConvert.DeserializeObject<List<PagosDTO>>(await auxListaPagos.Content.ReadAsStringAsync());
                }
                catch (Exception ex)
                {
                    _snackBar.Add("Error deserealizar lista de pagos", Severity.Error);
                }
            }
            else
            {
                _snackBar.Add("Error al consultar lista de paagos", Severity.Error);
            }
            _processingDos = false;
            StateHasChanged();
        }

        private async Task CambioFechaDesde()
        {
            Recargar();
        }

        private async Task CambioFechaHasta()
        {
            Recargar();
        }
        private void NuevoPago()
        {
            _navigationManager.NavigateTo("/nuevopago");
        }

        private async Task Recargar()
        {
            _processingDos = true;
            string auxDate1 = date1.Value.ToString("dd-MM-yyyy");
            string auxDate2 = date2.Value.ToString("dd-MM-yyyy");
            var auxListaPagos = await conexion.HttpClientInstance.GetAsync($"{urlListaPagos}/{auxDate1}/{auxDate2}/{usuarioSelect}");
            if (auxListaPagos.IsSuccessStatusCode)
            {
                try
                {
                    _listaPagos = JsonConvert.DeserializeObject<List<PagosDTO>>(await auxListaPagos.Content.ReadAsStringAsync());
                }
                catch (Exception ex)
                {
                    _snackBar.Add("Error deserealizar lista de pagos", Severity.Error);
                }
            }
            else
            {
                _snackBar.Add("Error al consultar lista de paagos", Severity.Error);
            }
            _processingDos = false;
            StateHasChanged();

        }

        public async void VerDetalle(PagosDTO data)
        {
            
            var parameters = new DialogParameters
            {
                {nameof(DialogDetallePago.pagoSelect), data}
            };

            var options = new DialogOptions
            {
                ClassBackground = "my-custom-class",
                FullWidth = true,
                MaxWidth = MaxWidth.Large,
                CloseButton = true,
                DisableBackdropClick = true,
            };
            var dialogo = _dialogService.Show<DialogDetallePago>($"Pago Nro {data.numeroCobranza} ", parameters, options);
            var result = await dialogo.Result;
            if (!result.Cancelled)
            {

            }

        }
        private async Task ExportExcel()
        {
            _processing = true;
            await Task.Delay(TimeSpan.FromSeconds(2));
            var wb = new XLWorkbook();
            wb.Properties.Author = "TI";
            wb.Properties.Title = "Lista Pagos";
            wb.Properties.Subject = "Pagos realizados";

            var ws = wb.Worksheets.Add("Listados pagos");
            ws.Columns().AdjustToContents();

            ws.Cell(1,1).Style.Fill.BackgroundColor = XLColor.BabyBlue;
            ws.Cell(1,2).Style.Fill.BackgroundColor = XLColor.BabyBlue;
            ws.Cell(1,3).Style.Fill.BackgroundColor = XLColor.BabyBlue;
            ws.Cell(1,4).Style.Fill.BackgroundColor = XLColor.BabyBlue;
            ws.Cell(1,5).Style.Fill.BackgroundColor = XLColor.BabyBlue;
            ws.Cell(1,6).Style.Fill.BackgroundColor = XLColor.BabyBlue;
            ws.Cell(1,7).Style.Fill.BackgroundColor = XLColor.BabyBlue;
            ws.Cell(1,8).Style.Fill.BackgroundColor = XLColor.BabyBlue;
            ws.Cell(1,9).Style.Fill.BackgroundColor = XLColor.BabyBlue;
            ws.Cell(1,10).Style.Fill.BackgroundColor = XLColor.BabyBlue;
            ws.Cell(1,11).Style.Fill.BackgroundColor = XLColor.BabyBlue;
            ws.Cell(1,12).Style.Fill.BackgroundColor = XLColor.BabyBlue;
            ws.Cell(1,13).Style.Fill.BackgroundColor = XLColor.BabyBlue;
            ws.Cell(1,14).Style.Fill.BackgroundColor = XLColor.BabyBlue;

            ws.Cell(1, 1).Style.Font.Bold = true;
            ws.Cell(1, 2).Style.Font.Bold = true;
            ws.Cell(1, 3).Style.Font.Bold = true;
            ws.Cell(1, 4).Style.Font.Bold = true;
            ws.Cell(1, 5).Style.Font.Bold = true;
            ws.Cell(1, 6).Style.Font.Bold = true;
            ws.Cell(1, 7).Style.Font.Bold = true;
            ws.Cell(1, 8).Style.Font.Bold = true;
            ws.Cell(1, 9).Style.Font.Bold = true;
            ws.Cell(1, 10).Style.Font.Bold = true;
            ws.Cell(1, 11).Style.Font.Bold = true;
            ws.Cell(1, 12).Style.Font.Bold = true;
            ws.Cell(1, 13).Style.Font.Bold = true;
            ws.Cell(1, 14).Style.Font.Bold = true;
            ws.Columns(1, 14).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            ws.Cell(1, 1).Value = "NumeroCobranza";
            ws.Cell(1, 2).Value = "IDCliente";
            ws.Cell(1, 3).Value = "RazonSocial";
            ws.Cell(1, 4).Value = "FechaPago";
            ws.Cell(1, 5).Value = "IdUsuario";
            ws.Cell(1, 6).Value = "ValorCobranza";
            ws.Cell(1, 7).Value = "CantidadPedidos";
            ws.Cell(1, 8).Value = "Efectivo";
            ws.Cell(1, 9).Value = "TarjetaCredito";
            ws.Cell(1, 10).Value = "TarjetaDebito";
            ws.Cell(1, 11).Value = "Transferencia";
            ws.Cell(1, 12).Value = "Deposito";
            ws.Cell(1, 13).Value = "Cheque";
            ws.Cell(1, 14).Value = "Letra";

            //ws.Rows(1, 2).AdjustToContents();
            
            for (int row = 0; row < _listaPagos.Count; row++)
            {
                ws.Cell(row + 2, 1).Value = _listaPagos[row].numeroCobranza;
                ws.Cell(row + 2, 2).Value = _listaPagos[row].iDCliente;
                ws.Cell(row + 2, 3).Value = _listaPagos[row].razonSocial;
                ws.Cell(row + 2, 4).Value = _listaPagos[row].fechaPago;
                ws.Cell(row + 2, 5).Value = _listaPagos[row].idUsuario;
                ws.Cell(row + 2, 6).Value = _listaPagos[row].valorCobranza;
                ws.Cell(row + 2, 7).Value = _listaPagos[row].cantidadPedidos;
                ws.Cell(row + 2, 8).Value = _listaPagos[row].efectivo;
                ws.Cell(row + 2, 9).Value = _listaPagos[row].tarjetaCredito;
                ws.Cell(row + 2, 10).Value = _listaPagos[row].tarjetaDebito;
                ws.Cell(row + 2, 11).Value = _listaPagos[row].transferencia;
                ws.Cell(row + 2, 12).Value = _listaPagos[row].deposito;
                ws.Cell(row + 2, 13).Value = _listaPagos[row].cheque;
                ws.Cell(row + 2, 14).Value = _listaPagos[row].letra;
            }

            ws.Columns().AdjustToContents();
            MemoryStream XLSStream = new();
            wb.SaveAs(XLSStream);

            string auxDate1 = date1.Value.ToString("dd-MM-yyyy");
            string auxDate2 = date2.Value.ToString("dd-MM-yyyy");

            await javaScript.SaveAs($"ListadosPagos_{auxDate1}_{auxDate2}.xlsx", XLSStream.ToArray());
            _processing = false;
        }
    }
}
