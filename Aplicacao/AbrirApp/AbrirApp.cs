using Dominio.Interface;
using Entidades.Entidades;
using Infraestrutura.Configuracoes;
using Infraestrutura.Utils;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Aplicacao.AbrirApp
{
    public class AbrirApp
    {
        private readonly WConfig _conf;        

        public AbrirApp() 
        {
            _conf = new WConfig()
            {
                OmegaXmlFolder = "C:\\Users\\alexandre.lima\\Documents\\",
                DbAlias = "DBTP"
            };
            
            _ = _conf.GetSettings();
            var dbSetting = _conf.GetCurDbSetting();

            try
            {
                var connStr = dbSetting.GetConnectionString();
                var dbOptions = new DbContextOptionsBuilder<WDbContext>();

                dbOptions = dbOptions.UseSqlServer(connStr, providerOptions => providerOptions.EnableRetryOnFailure()).EnableSensitiveDataLogging();

                _conf.ConfigDbContext();

                _conf.CreateDbCtxt(dbOptions.Options);

                using var reader = _conf.GetDbCtxt().OpenSQL("SELECT EMPRESA FROM USUA WHERE 0 <> 0");

                Console.WriteLine("Sucesso na conexão");                
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }
        }

        public string GetPahtXml()
        {
            return _conf.GetOmegaXmlPath();
        }

        public void FazSelect()
        {
            using var reader = _conf.GetDbCtxt().OpenSQL("SELECT * FROM AIHR WHERE PESSOA = 'ALEXANDREL' AND AREA = 8 ORDER BY DT_LANCTO");            

            while (reader.Read())
            {
                Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", reader.GetInt16(0), reader.GetInt16(1), reader.GetString(2),
                    reader.GetInt16(3), reader.GetDateTime(4));
            }

            reader.Close();
        }

        public List<Cliente> FazSelectCliente()
        {
            var cliente = _conf.GetDbCtxt().Set<Cliente>();
            var lista = cliente.Where(c => c.EmpFil == 460004)// && c.Codigo == 40001)
                .OrderBy(c => c.Codigo)
                .AsNoTracking()
                .Take(5)
                .ToList();

            //return lista;
            return null;
            //foreach (var c in lista) 
            //{
            //    Console.WriteLine($"Empresa {c.EmpFil} - {c.Codigo} - {c.NomeFantasia}");

            //    //converter retorno para json
            //    string jsonString = JsonSerializer.Serialize(c);

            //    Console.WriteLine(jsonString);
            //}            
        }

        public async Task GetHorasTrabAsync()
        {


        }

        public string FazSelectClienteJson()
        {
            var cliente = _conf.GetDbCtxt().Set<Cliente>();
            var lista = cliente.Where(c => c.EmpFil == 460004)// && c.Codigo == 40001)
                .OrderBy(c => c.Codigo)
                .AsNoTracking()
                .Take(1)
                .ToList();

            //var options = new JsonSerializerOptions { WriteIndented = true };
            //string jsonString = JsonSerializer.Serialize(lista, options);

            //return jsonString;

            //usando os metodos prontos
            string json = WStringUtils.ToJson(lista);
            return json;

        }

        public T ConverteJSonParaObject<T>(string jsonString)
        {
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                T obj = (T)serializer.ReadObject(ms);
                return obj;
            }
            catch
            {
                throw;
            }
        }
    }

    public class WeatherForecast
    {
        public DateTimeOffset Date { get; set; }
        public int TemperatureCelsius { get; set; }
        public string? Summary { get; set; }
        public string? SummaryField;
        public IList<DateTimeOffset>? DatesAvailable { get; set; }
        public Dictionary<string, HighLowTemps>? TemperatureRanges { get; set; }
        public string[]? SummaryWords { get; set; }
    }

    public class HighLowTemps
    {
        public int High { get; set; }
        public int Low { get; set; }
    }
}
