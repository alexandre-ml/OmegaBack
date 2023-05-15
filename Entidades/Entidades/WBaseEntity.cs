using System.Reflection;

namespace Entidades.Entidades
{
    public class WBaseEntity : WGeneralBaseEntity
    {
        #region Properties

        public Guid AccountId { get; set; }

        #endregion Properties

        public static Assembly GetExecAssembly()
        {
            // Configurações para o WDbContext
            var assembly = Assembly.GetExecutingAssembly();

            return assembly;
        }
    }
}
