using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Entidades.Entidades
{
    public class WBaseLegacyEntity : ICloneable
    {
        #region Properties

        [NotMapped]
        public string Id { get; set; } = string.Empty;

        //public string Operador { get; set; }
        //public DateTime? DataAlter { get; set; }
        //public short? HoraAlter { get; set; }
        //public short? TimeStamp { get; set; }

        [NotMapped]
        public bool AuditSetted { get; protected set; } = false;

        #endregion Properties

        #region Methods
        //public void SetAuditInfo (IRequestContext ctx)
        //{
        //    WReflectionUtils.SetPropertyValue (this, "Operador", ctx.User.Identity.Name);
        //    WReflectionUtils.SetPropertyValue (this, "DataAlter", DateTime.Now.Date);
        //    WReflectionUtils.SetPropertyValue (this, "HoraAlter", DateTime.Now.HoraAntiga());
        //    AuditSetted = true;            
        //}
        public object Clone()
        {
            var newObject = (WBaseLegacyEntity)Activator.CreateInstance(GetType());

            foreach (PropertyInfo originalProp in GetType().GetProperties())
            {
                if (originalProp.PropertyType.IsSubclassOf(typeof(WBaseLegacyEntity)))
                    originalProp.SetValue(newObject, null);
                else if (originalProp.PropertyType.IsInterface)
                {
                    if (originalProp.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))  // verifica se é um campo relacional
                        originalProp.SetValue(newObject, null);
                }
                else
                {
                    if (originalProp.CanRead && originalProp.CanWrite)
                        originalProp.SetValue(newObject, originalProp.GetValue(this));
                }
            }

            return newObject;
        }
        #endregion // Methods
    }
}
