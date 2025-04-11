using Newtonsoft.Json;
using PortalAG_V2.Shared.Model.Indicadores;
using PortalAG_V2.Shared.Services;
using System.Globalization;
using System.Runtime.Intrinsics.X86;
using static PortalAG_V2.Shared.Model.Indicadores.IndicadoresRankingDTO;

namespace PortalAG_V2.Pages.Indicadores
{
    partial class IndicadoresRanking
    {

        MainServices service;
        string URL = "api/v2/Lineas/Pedidos";

        Timer _timer;

        bool dense = true;
        bool hover = true;
        bool striped = true;
        bool bordered = true;
        string mesM1 = DateTime.Now.AddMonths(-1).ToString("MMMM", new CultureInfo("es-CL")).ToUpper() ;
        string mesM2 = DateTime.Now.AddMonths(-2).ToString("MMMM", new CultureInfo("es-CL")).ToUpper();
        string mesM3 = DateTime.Now.AddMonths(-3).ToString("MMMM", new CultureInfo("es-CL")).ToUpper();

        string[] headings = { " ", "BVN", "BIT", "BPM", "Revisión", "Devolución", "Reposición" };

        List<lineasSacadorDTO> actual = new List<lineasSacadorDTO> ();
        List<lineasSacadorDTO> m1 = new List<lineasSacadorDTO>();
        List<lineasSacadorDTO> m2 = new List<lineasSacadorDTO>();
        List<lineasSacadorDTO> m3 = new List<lineasSacadorDTO>();
        bool loading = true;

        protected override void OnInitialized()
        {
            service = new MainServices();
            _timer = new Timer(async _ => await ConsultarIndicadores(), null, TimeSpan.Zero, TimeSpan.FromMinutes(10));
            base.OnInitialized();
        }

        private async Task ConsultarIndicadores()
        {
            loading = true;
            List<IndicadoresRankingDTO> indicadores = new List<IndicadoresRankingDTO>();
            var auxIndicadores = await service.ConectionService.HttpClientInstance.GetAsync(URL);
            if (auxIndicadores.IsSuccessStatusCode)
            {
                indicadores = JsonConvert.DeserializeObject<List<IndicadoresRankingDTO>>(await auxIndicadores.Content.ReadAsStringAsync());

                var aux = indicadores.FirstOrDefault();

                if (aux != null && aux.actual != null)
                {
                    aux.actual = aux.actual
                        .Where(x => !string.IsNullOrWhiteSpace(x.sacador))
                        .GroupBy(x => x.sacador) // Agrupa por el nombre
                        .Select(g => new lineasSacadorDTO
                        {
                            sacador = g.Key,
                            bV_BVN = g.Sum(x => x.bV_BVN),
                            bV_BIT = g.Sum(x => x.bV_BIT),
                            bV_BPM = g.Sum(x => x.bV_BPM),
                            packing = g.Sum(x => x.packing),
                            devolucion = g.Sum(x => x.devolucion),
                            reposicion = g.Sum(x => x.reposicion)
                            // Agrega aquí los campos que necesites sumar
                        })
                        .ToList();
                }

             
                if (aux != null && aux.m1 != null)
                {
                    aux.m1 = aux.m1
                        .Where(x => !string.IsNullOrWhiteSpace(x.sacador))
                        .GroupBy(x => x.sacador) // Agrupa por el nombre
                        .Select(g => new lineasSacadorDTO
                        {
                            sacador = g.Key,
                            bV_BVN = g.Sum(x => x.bV_BVN),
                            bV_BIT = g.Sum(x => x.bV_BIT),
                            bV_BPM = g.Sum(x => x.bV_BPM),
                            packing = g.Sum(x => x.packing),
                            devolucion = g.Sum(x => x.devolucion),
                            reposicion = g.Sum(x => x.reposicion)
                            // Agrega aquí los campos que necesites sumar
                        })
                        .ToList();
                }

                if (aux != null && aux.m2 != null)
                {
                    aux.m2 = aux.m2
                        .Where(x => !string.IsNullOrWhiteSpace(x.sacador))
                        .GroupBy(x => x.sacador) // Agrupa por el nombre
                        .Select(g => new lineasSacadorDTO
                        {
                            sacador = g.Key,
                            bV_BVN = g.Sum(x => x.bV_BVN),
                            bV_BIT = g.Sum(x => x.bV_BIT),
                            bV_BPM = g.Sum(x => x.bV_BPM),
                            packing = g.Sum(x => x.packing),
                            devolucion = g.Sum(x => x.devolucion),
                            reposicion = g.Sum(x => x.reposicion)
                            // Agrega aquí los campos que necesites sumar
                        })
                        .ToList();
                }

                if (aux != null && aux.m3 != null)
                {
                    aux.m3 = aux.m3
                        .Where(x => !string.IsNullOrWhiteSpace(x.sacador))
                        .GroupBy(x => x.sacador) // Agrupa por el nombre
                        .Select(g => new lineasSacadorDTO
                        {
                            sacador = g.Key,
                            bV_BVN = g.Sum(x => x.bV_BVN),
                            bV_BIT = g.Sum(x => x.bV_BIT),
                            bV_BPM = g.Sum(x => x.bV_BPM),
                            packing = g.Sum(x => x.packing),
                            devolucion = g.Sum(x => x.devolucion),
                            reposicion = g.Sum(x => x.reposicion)
                            // Agrega aquí los campos que necesites sumar
                        })
                        .ToList();
                }
                actual = aux.actual;
                m1 = aux.m1;
                m2 = aux.m2;
                m3 = aux.m3;
                
               

                loading = false;
                StateHasChanged();


                //actual = ProcesarCategoria(indicadores, "actual");
                //m1 = ProcesarCategoria(indicadores, "m1");
                //m2 = ProcesarCategoria(indicadores, "m2");
                //m3 = ProcesarCategoria(indicadores, "m3");

                //Console.WriteLine("Actual:");
                //foreach (var row in actual) Console.WriteLine(row);

                //Console.WriteLine("\nM1:");
                //foreach (var row in m1) Console.WriteLine(row);

                //Console.WriteLine("\nM2:");
                //foreach (var row in m2) Console.WriteLine(row);

                //Console.WriteLine("\nM3:");
                //foreach (var row in m3) Console.WriteLine(row);

            }

        }

        static string[] ProcesarCategoria(List<IndicadoresRankingDTO> indicadores, string categoria)
        {
            var lista = indicadores.FirstOrDefault()?.GetType().GetProperty(categoria)?.GetValue(indicadores.FirstOrDefault()) as List<lineasSacadorDTO>;

            if (lista == null) return new string[0];

            return lista.Select((sacador, index) => $"{sacador.sacador} {sacador.bV_BVN} {sacador.bV_BIT} {sacador.bV_BPM} {sacador.packing}").ToArray();
        }
    }
}
