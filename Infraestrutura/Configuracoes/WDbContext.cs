using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Data;
using System.Reflection;

namespace Infraestrutura.Configuracoes
{
    public delegate void DbContextSaveChangesDelegate(object sender, ChangeTracker changeTracker);

    public class WDbContext : DbContext
    {
        //public IRequestContext Context { get; protected set; }
        public static readonly List<Type> Entities = new();
        public static readonly List<Assembly> Assemblies = new();

        protected static List<DbContextSaveChangesDelegate> onChange = new();
        public WDbContext(DbContextOptions<WDbContext> options)
            : base(options)
        {
            //Context = null;
            Database.GetDbConnection().StateChange += Connection_StateChange;
            Database.SetCommandTimeout(180);
        }

        public void AddOnSaveChanges(DbContextSaveChangesDelegate onSaveChanges)
        {
            var obj = onChange.Find(osc => osc == onSaveChanges);
            if (obj == null)
                onChange.Add(onSaveChanges);
        }

        public void RemoveOnSaveChanges(DbContextSaveChangesDelegate onSaveChanges)
        {
            onChange.Remove(onSaveChanges);
        }

        private void Connection_StateChange(object sender, StateChangeEventArgs e)
        {
            //if (e.CurrentState == ConnectionState.Open && Context?.UserInfo?.AccountId != null)
            {
                // O banco de dados está configurado para aplicar um predicado com o accountId
                // sempre que uma operação for executada; SELECT, UPDATE e DELETE sempre terão
                // um WHERE como accountId para garantir a integridade do banco
                // Ver os constraints para todas as tabelas:
                //    ALTER TABLE WorkCell
                //    ADD CONSTRAINT df_TenantId_WorkCell
                //    DEFAULT CAST(SESSION_CONTEXT(N'AccountId') AS int) FOR AccountId;

                //string accountId = Context.UserInfo.AccountId.ToString();
                //Database.ExecuteSqlRaw($"exec sp_set_session_context @key = N'AccountId', @value = '{accountId}'");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (Type tipo in Entities)
                modelBuilder.Entity(tipo);

            foreach (Assembly assembly in Assemblies)
                modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        }

        public IDataReader OpenSQL(string queryString)
        {
            using var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = queryString;
            cmd.CommandType = CommandType.Text;

            if (Database.GetDbConnection().State != ConnectionState.Open)
                Database.GetDbConnection().Open();

            return cmd.ExecuteReader();
        }

        /// <summary>
        /// Executa uma ação numa unidade de trabalho separada da unidade principal.
        /// Use esse recurso quando precisar de uma transação no banco de dados.indenpendente das alterações ocorridas na unidade de trabalho 
        /// principal em uma requisição.. Lance uma exceção se precisar fazer rollback.
        /// Exemplo: obter um token no banco de dados e registrar imediatamente seu uso.
        /// </summary>
        /// <param name="action">a ação a ser executada como uma unidade de trabalho.</param>
        public void RunAsUnitOfWork(Action<WDbContext> action)
        {
            var tr = Database.CreateExecutionStrategy();
            tr.Execute(() =>
            {
                using var transaction = Database.BeginTransaction();
                try
                {
                    action(this);
                    SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            });
        }

        public override int SaveChanges()
        {
            /*
            IEnumerable<WBaseEntity> changedEntities = ChangeTracker
                .Entries()
                .Where(entry => entry.Entity.GetType().IsSubclassOf(typeof(WBaseEntity)) && (entry.State.Equals(EntityState.Added) || entry.State.Equals(EntityState.Modified)))
                .Select(entry => entry.Entity)
                .Cast<WBaseEntity>();

            foreach (WBaseEntity entity in changedEntities)
            {
                entity.AccountId = Context.UserInfo.AccountId ?? entity.AccountId;
                
                entity.ChangeDate = DateTime.UtcNow;
                entity.ChangeUserId = Context.UserInfo.ChangeUserId;
            }

            IEnumerable<WGeneralBaseEntity> changedEntitiesNoAccount = ChangeTracker
                .Entries()
                .Where(entry => (entry.Entity.GetType().BaseType == (typeof(WGeneralBaseEntity))) && (entry.State.Equals(EntityState.Added) || entry.State.Equals(EntityState.Modified)))
                .Select(entry => entry.Entity)
                .Cast<WGeneralBaseEntity>();

            foreach (WGeneralBaseEntity entity in changedEntitiesNoAccount)
            {
                entity.ChangeDate = DateTime.UtcNow;
                entity.ChangeUserId = Context.UserInfo.ChangeUserId;
            }

            foreach (var onSaveChange in onChange)
                try
                {
                    onSaveChange(this, ChangeTracker);
                }
                catch (Exception) { }
            */
            return base.SaveChanges();
        }

    }
}
