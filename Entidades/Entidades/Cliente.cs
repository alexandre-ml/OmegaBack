#nullable disable

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entidades.Entidades
{

    public partial class Cliente : WBaseLegacyEntity
    {
        public int EmpFil { get; set; }
        public decimal Codigo { get; set; }
        public string NomeFantasia { get; set; }
        public string Operador { get; set; }
        public DateTime? DataAlter { get; set; }
        public short? HoraAlter { get; set; }
        public short? TimeStamp { get; set; }
    }

    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> entity)
        {
            entity.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            entity.HasKey(e => new { e.EmpFil, e.Codigo })
                .HasName("CRCL_KEY0")
                .IsClustered(false);

            entity.ToTable("CRCL");

            entity.Property(e => e.EmpFil).HasColumnName("EMP_FIL").HasColumnType("int").IsRequired();
            entity.Property(e => e.Codigo).HasColumnName("CLIENTE").HasColumnType("decimal(14,0)").IsRequired();
            entity.Property(e => e.NomeFantasia).HasColumnName("NOM_FANTAS").HasColumnType("varchar(25)");
            entity.Property(e => e.DataAlter).HasColumnName("DATA_ALTER").HasColumnType("smalldatetime");
            entity.Property(e => e.HoraAlter).HasColumnName("HORA_ALTER");
            entity.Property(e => e.Operador).HasColumnName("OPERADOR").HasMaxLength(10).IsUnicode(false);
            entity.Property(e => e.TimeStamp).HasColumnName("TIME_STAMP");
        }
    }
}