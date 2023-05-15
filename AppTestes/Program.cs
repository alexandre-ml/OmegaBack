// See https://aka.ms/new-console-template for more information
using Aplicacao;
using Aplicacao.Interfaces;
using Dominio.Interface;
using Infraestrutura.Configuracoes;
using Infraestrutura.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

WConfig _conf = new WConfig()
{
    OmegaXmlFolder = "C:\\Users\\alexandre.lima\\Documents\\",
    DbAlias = "DBTP"
};
_ = _conf.GetSettings();
var dbSetting = _conf.GetCurDbSetting();

try
{
    var connStr = dbSetting?.GetConnectionString();
    //var dbOptions = new DbContextOptionsBuilder<WDbContext>();

    //dbOptions = dbOptions.UseSqlServer(connStr, providerOptions => providerOptions.EnableRetryOnFailure()).EnableSensitiveDataLogging();

    _conf.ConfigDbContext();

    var builder = Host.CreateDefaultBuilder(args);

    builder.ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;

        //dbContext
        services.AddDbContext<WDbContext>(options => options.UseSqlServer(connStr, 
            providerOptions => providerOptions.EnableRetryOnFailure()).EnableSensitiveDataLogging());

        // interface e repositorio
        //services.AddScoped(typeof(IWGeneric<>), typeof(RepositorioGenericos<>));
        services.AddScoped<IWHorasAi, WHorasAi>();

        // interface aplicacao
        services.AddScoped<IWHorasAiApp, HorasAiApp>();

        // serviço dominio
        //services.AddScoped<IServicoProduto, ServicoProduto>();

    });

    using var app = builder.Build();

    Console.WriteLine(app.Services.GetServices<IWHorasAi>().ToString());

    app.Run();

}
catch (Exception exc)
{
    Console.WriteLine(exc);
}

//var builder = Host.CreateDefaultBuilder(args);

//builder.ConfigureServices(services => services.AddSingleton<IWHorasAiApp, HorasAiApp>());

//builder.ConfigureServices((hostContext, services) =>
// {
//     IConfiguration configuration = hostContext.Configuration;

   
//services.AddDbContext<WDbContext>(op => );
// });

//using var host = builder.Build();

//Console.WriteLine("Hello, World! - Before Run");

//AbrirApp OpenApp = new();


//host.Run();

Console.WriteLine("Hello, World! - After Run");

/*
#region TesteBruto
string jsonString =
    @"{
      ""Date"": ""2019-08-01T00:00:00-07:00"",
      ""TemperatureCelsius"": 25,
      ""Summary"": ""Hot"",
      ""DatesAvailable"": [
        ""2019-08-01T00:00:00-07:00"",
        ""2019-08-02T00:00:00-07:00""
      ],
      ""TemperatureRanges"": {
                    ""Cold"": {
                        ""High"": 20,
          ""Low"": -10
                    },
        ""Hot"": {
                        ""High"": 60,
          ""Low"": 20
        }
                },
      ""SummaryWords"": [
        ""Cool"",
        ""Windy"",
        ""Humid""
      ]
    }
    ";

WeatherForecast? weatherForecast =
    JsonSerializer.Deserialize<WeatherForecast>(jsonString);

Console.WriteLine($"Date: {weatherForecast?.Date}");
Console.WriteLine($"TemperatureCelsius: {weatherForecast?.TemperatureCelsius}");
Console.WriteLine($"Summary: {weatherForecast?.Summary}");
#endregion
*/
//IWHorasAiApp horasAiApp = new();

//horasAiApp.GetList();



//Console.WriteLine(OpenApp.GetPahtXml());

////fazer um select utilizando o open
//OpenApp.FazSelect();

//fazer um select usando um 'b-data'
//var clientes = OpenApp.FazSelectCliente();
//foreach (var c in clientes)
//{
//    converter retorno para json
//    string jsonString1 = JsonSerializer.Serialize(c);

//    Console.WriteLine(jsonString1);
//}
//Console.WriteLine("nova impressão");
//var clientes1 = OpenApp.FazSelectClienteJson();
//Console.WriteLine(clientes1);

//Console.WriteLine("deserilização do json para obj c#");
//Cliente? objCli = WStringUtils.Deserialize<Cliente>(clientes1);
//string aux = clientes1;//.Substring(1, clientes1.Length - 2);
//Console.WriteLine("após substring");
//Console.WriteLine(aux);

//var objCli = WStringUtils.Deserialize<Cliente>(aux);
////foreach (var c in objCli)
//{
//    Console.WriteLine($"Empresa {objCli.EmpFil} - {objCli.Codigo} - {objCli.NomeFantasia}");
//}
