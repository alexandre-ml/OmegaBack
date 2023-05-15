using Entidades.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infraestrutura.Configuracoes
{
    public class WConfig : IDisposable
    {
        public string? OmegaXmlFolder { get; set; }
        public string? DbAlias { get; set; }
        public List<WConfigItem>? Colunas { get; set; }
        public WConfigForm? frmMain { get; set; }
        public WConfigForm? frmQryModel { get; set; }

        protected WOmegaSettings? Settings = null;
        protected WDbContext? DbCtxt = null;
        private bool DisposedValue;

        public string GetOmegaXmlPath()
        {
            return OmegaXmlFolder + "omegaApi.xml";
        }

        public WOmegaSettings? GetSettings(bool reload = false)
        {
            if (Settings == null || reload)
            {
                Settings = new WOmegaSettings(GetOmegaXmlPath());
                Settings.LoadOmegaSettings();
            }
            else if (reload)
                Settings.ReLoadOmegaSettings();

            return Settings;
        }

        public WDbSettings? GetCurDbSetting()
        {
            if (DbAlias == null)
                return null;

            return Settings?.GetDbSettingsByName(DbAlias);
        }

        public void CreateDbCtxt(DbContextOptions<WDbContext> options)
        {
            DbCtxt?.Dispose();
            DbCtxt = new WDbContext(options);
        }

        public WDbContext? GetDbCtxt()
        {
            return DbCtxt;
        }

        public void CopyTo(WConfig config)
        {
            config.DbAlias = DbAlias;
            config.OmegaXmlFolder = OmegaXmlFolder;
            config.Settings = Settings;
            config.DbCtxt = DbCtxt;
        }

        public void ConfigDbContext()
        {
            // Configurações para o WDbContext
            //var assembly = Assembly.GetExecutingAssembly();

            //utlizado metodo para buscar as tabelas que estão no projetos de entidades
            var assembly = WBaseEntity.GetExecAssembly();

            WDbContext.Assemblies.Add(assembly);

            var tipos = assembly.GetTypes();

            var baseLegacy = typeof(WBaseLegacyEntity);
            var baseGenEntity = typeof(WGeneralBaseEntity);
            var baseEntity = typeof(WBaseEntity);

            foreach (Type tipo in tipos)
            {
                if (tipo.IsPublic && tipo.IsClass && !tipo.IsAbstract && tipo != baseEntity &&
                   (tipo.IsSubclassOf(baseGenEntity) || tipo.IsSubclassOf(baseLegacy)))
                {
                    WDbContext.Entities.Add(tipo);
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!DisposedValue)
            {
                if (disposing)
                {
                    DbCtxt?.Dispose();
                    DbCtxt = null;
                }

                DisposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
