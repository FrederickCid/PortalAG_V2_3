

using agDataAccess.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Shared.Model.EstadoPedidos;
using PortalAG_V2.Shared.Services;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PortalAG_V2.Componentes.EstadoPedido
{
    public partial class MudBlazorDialogCustom
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public string Nombre { get; set; }
        [Parameter] public string ApellidoPaterno { get; set; }
        [Parameter] public string Usuario { get; set; }
        [Parameter] public string RSocial { get; set; }
        [Parameter] public string RutRS { get; set; }
        [Parameter] public string option { get; set; }
        [Parameter] public string FechaSolicitud { get; set; }
        [Parameter] public string Descripcion { get; set; }
        [Parameter] public EstadoPedidosNoMOD Lista { get; set; }
        [Parameter] public List<BultosModel> ListaBultos { get; set; }
        [Parameter] public string ButtonText { get; set; }

        public List<EstadoPedidoDespachoImgModels> Imagenes = new();


        private string UrlGetImagenesEStadoPedido = "api/v2/GetEstadoPedidos/ImagenesDespacho/";
        public MainServices? service;
        bool Loading = false;
        void ClearList()
        {
            Lista = null;
        }

        void Submit() => MudDialog.Close(DialogResult.Ok(true));
        void Cancel() => MudDialog.Cancel();

        void onClickBultos()
        {
            var parameters = new DialogParameters<MudDialogBultos>();
            parameters.Add(x => x.Bultos, Lista.Bultos);

            var options = new MudBlazor.DialogOptions() { CloseButton = false, FullWidth = true, MaxWidth = MaxWidth.Large };

            DialogService.Show<MudDialogBultos>($"Pedido Numero: {Lista.NroDocumento}", parameters, options);
        }

        #region calcular sla V1
        double FechaResta(string TiempoTermino, string TiempoInicio)
        {

            if ((TiempoTermino != null) && (TiempoInicio != null))
            {
                System.Globalization.CultureInfo norwCulture = System.Globalization.CultureInfo.CreateSpecificCulture("es-CL");
                Calendar cal = norwCulture.Calendar;

                if ((TiempoTermino != "") && (TiempoInicio != ""))
                {
                    var inicio = DateTime.Parse(TiempoInicio);
                    var fin = DateTime.Parse(TiempoTermino);

                    int weekNoFin = cal.GetWeekOfYear(fin,
                     norwCulture.DateTimeFormat.CalendarWeekRule,
                     norwCulture.DateTimeFormat.FirstDayOfWeek);

                    int weekNoinicio = cal.GetWeekOfYear(inicio,
                     norwCulture.DateTimeFormat.CalendarWeekRule,
                     norwCulture.DateTimeFormat.FirstDayOfWeek);
                    //double diferencia;                    
                    var aux = inicio - fin;
                    //return Convert.ToInt32((inicio - fin).TotalHours);
                    return Convert.ToInt32((inicio.Hour) - (((fin - inicio).TotalHours - (fin - inicio).Days * 15) - (weekNoFin - weekNoinicio) * 13));
                    //return diferencia;

                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        double CalculoSLA(string Finicio, string Ffin, double hora)
        {
            System.Globalization.CultureInfo norwCulture = System.Globalization.CultureInfo.CreateSpecificCulture("es-CL");
            Calendar cal = norwCulture.Calendar;
            if ((Finicio != null) && (Ffin != null))
            {

                if (Finicio != "" && Ffin != "")
                {
                    var inicio = DateTime.Parse(Finicio);
                    var fin = DateTime.Parse(Ffin);

                    int weekNoFin = cal.GetWeekOfYear(fin,
                     norwCulture.DateTimeFormat.CalendarWeekRule,
                     norwCulture.DateTimeFormat.FirstDayOfWeek);

                    int weekNoinicio = cal.GetWeekOfYear(inicio,
                     norwCulture.DateTimeFormat.CalendarWeekRule,
                     norwCulture.DateTimeFormat.FirstDayOfWeek);
                    //var resta = (fin - inicio).TotalHours;
                    // Console.WriteLine((((inicio - fin) - (inicio - fin) * 15) / hora).TotalHours);
                    return (((((fin - inicio).TotalHours - (fin - inicio).Days * 15) - (weekNoFin - weekNoinicio) * 13) * 100) / hora);
                }
                else
                {
                    return 0;
                }

            }
            else
            {
                return 0;
            }

        }
        #endregion

        #region Calculo de Horas Trabajadas Y SLA V2
        //test
        double CalcularHorasTrabajadas(string inicio, string fin)
        {
            //validar que no sea los datos en null
            if ((fin != null) && (inicio != null))
            {
                if ((fin != "") && (inicio != ""))
                {
                    var finicio = DateTime.Parse(inicio);
                    var ffin = DateTime.Parse(fin);
                    double horasTrabajadas = 0;

                    // Considerar días laborales y horas de trabajo
                    for (DateTime current = finicio; current < ffin; current = current.AddMinutes(1))
                    {
                        //llamo las funciones para ver si entra en las horas laborales y los dias de la semana dentro del array
                        if (EsDiaLaboral(current) && EsHoraDeTrabajo(current))
                        {
                            horasTrabajadas += 1;
                        }
                    }

                    return Math.Round(horasTrabajadas / 60, 1, MidpointRounding.ToEven);  // Convertir minutos a horas

                }
                else
                {
                    return 0;
                }


            }
            else
            {
                return 0;
            }



        }

        bool EsDiaLaboral(DateTime fecha)
        {
            // Definir días laborales
            DayOfWeek[] diasLaborales = { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };

            return Array.IndexOf(diasLaborales, fecha.DayOfWeek) != -1;
        }

        bool EsHoraDeTrabajo(DateTime fecha)
        {
            // Definir horas de trabajo
            int horaInicio = 8;  // 8:00 AM
            int horaFin = 17;    // 5:00 PM

            return fecha.Hour >= horaInicio && fecha.Hour <= horaFin;
        }

        double CalculoSLA2(string Finicio, string Ffin, double hora)
        {
            if ((Finicio != null) && (Ffin != null))
            {

                if (Finicio != "" && Ffin != "")
                {

                    var horasTrabajadas = CalcularHorasTrabajadas(Finicio, Ffin);
                    return (horasTrabajadas * 100 / hora);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }

        }
        #endregion

        private void VerFacturas()
        {
            var parameters = new DialogParameters<DialogFacturas>
            {
                { x => x.Detalle, Lista.Facturas },
                { x => x.razonSocial, Lista.RazonSocial },
                { x => x.idCliente, Lista.IDCliente },
                { x => x.nroDocumento, Lista.NroDocumento },
            };
            var options = new MudBlazor.DialogOptions() { CloseButton = false, FullWidth = true, MaxWidth = MaxWidth.Medium };

            DialogService.Show<DialogFacturas>($"Facturas: {Lista.Facturas.Count()}", parameters, options);
        }

        private async Task GetImagenes(int IDOperacion)
        {
            Loading = true;
            Imagenes = new();
            try
            {
                service = new MainServices();
                var lista = await service.ConectionServiceNotaCredito.HttpClientInstance.GetAsync($"{UrlGetImagenesEStadoPedido}{IDOperacion}");

                if (lista.IsSuccessStatusCode)
                {
                    Imagenes = JsonConvert.DeserializeObject<List<EstadoPedidoDespachoImgModels>>(await lista.Content.ReadAsStringAsync());
                    Loading = false;
                    StateHasChanged();
                }
                Loading = false;
                StateHasChanged();

            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                Loading = false;
                StateHasChanged();
            }
        }

        private async Task DialogImagenes()
        {
            await GetImagenes(Lista.IDOperacion);

            if (Imagenes.FirstOrDefault().Msg != "ER")
            {

                var parameters = new DialogParameters<ImagenesDespachoDialog>
            {
                { x => x.Imagenes, Imagenes },
                { x => x.IDOperacion, Lista.IDOperacion}

            };
                var options = new MudBlazor.DialogOptions() { CloseButton = false, MaxWidth = MaxWidth.Medium };

                DialogService.Show<ImagenesDespachoDialog>($"Pedido: {Lista.IDOperacion}", parameters, options);
            }
            else
            {
                snakBarCreation($"{Imagenes.FirstOrDefault().Msg} - {Imagenes.FirstOrDefault().MsgResult}", Defaults.Classes.Position.BottomStart, Severity.Warning, 3000);

            }
        }
        private void snakBarCreation(string msj, string position, MudBlazor.Severity style, int duration)
        {
            _snackBar.Configuration.VisibleStateDuration = duration;
            _snackBar.Configuration.PositionClass = position;
            _snackBar.Add(msj, style);
        }

    }
}
