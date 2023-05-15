using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entidades.Entidades
{
    public class WGeneralBaseEntity : ICloneable
    {
        #region Properties

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.Empty;
        public Guid? ChangeUserId { get; set; }
        public DateTime? ChangeDate { get; set; }
        public byte[] TimeStamp { get; set; }

        [NotMapped]
        internal bool AuditSetted { get; set; } = false;

        #endregion Properties

        #region Methods

        public object Clone()
        {
            WGeneralBaseEntity newObject = (WGeneralBaseEntity)Activator.CreateInstance(this.GetType());

            foreach (System.Reflection.PropertyInfo originalProp in this.GetType().GetProperties())
            {
                if (originalProp.PropertyType.IsSubclassOf(typeof(WGeneralBaseEntity)))
                {
                    originalProp.SetValue(newObject, null);
                }
                else if (originalProp.PropertyType.IsInterface)
                {
                    if (originalProp.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))  // verifica se é um campo relacional
                    {
                        originalProp.SetValue(newObject, null);
                    }
                }
                else
                {
                    if (originalProp.CanRead && originalProp.CanWrite)
                    {
                        originalProp.SetValue(newObject, originalProp.GetValue(this));
                    }
                }
            }

            return newObject;
        }

        #endregion Methods
    }
}
