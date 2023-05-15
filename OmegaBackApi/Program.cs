using Aplicacao;
using Aplicacao.Interfaces;
using Dominio.Interface;
using Infraestrutura.Configuracoes;
using Infraestrutura.Repositorios;
using Microsoft.EntityFrameworkCore;

WConfig _conf = new ()
{
    OmegaXmlFolder = "C:\\Users\\alexandre.lima\\Documents\\",
    DbAlias = "DBTP"
};

try
{    
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddControllers().AddJsonOptions(
    x => x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles); ;

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "OmegaBack - Estudos API",
            Version = "v1",
            Description = "Api criada para estudos utilizando um banco de dados não relacional já criado e carregado",

            Contact = new Microsoft.OpenApi.Models.OpenApiContact
            {
                Name = "Alexandre Lima",
                Email = "alexandre87ml@gmail.com"
            }
        });
    });    

    // interface e repositorio
    //services.AddScoped(typeof(IWGeneric<>), typeof(RepositorioGenericos<>));
    builder.Services.AddScoped<IWHorasAi, WHorasAi>();

    // interface aplicacao
    builder.Services.AddScoped<IWHorasAiApp, HorasAiApp>();

    _ = _conf.GetSettings();

    var dbSetting = _conf.GetCurDbSetting();

    var connStr = dbSetting?.GetConnectionString();

    _conf.ConfigDbContext();

    builder.Services.AddDbContext<WDbContext>(options => options.UseSqlServer(connStr,
            providerOptions => providerOptions.EnableRetryOnFailure()).EnableSensitiveDataLogging());

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OmegaBack v1"));
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception exc)
{
    Console.WriteLine(exc);
}