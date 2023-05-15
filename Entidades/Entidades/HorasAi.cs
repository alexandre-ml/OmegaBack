using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entidades.Entidades
{
    public partial class HorasAi : WBaseLegacyEntity
    {

        public short Empresa { get; set; }
        public short Filial { get; set; }
        public string Pessoa { get; set; }
        public DateTime DtLancto { get; set; }
        public short Sequencia { get; set; }
        public short? Area { get; set; }
        public short? Atividade { get; set; }
        public int? NroOs { get; set; }
        public double? HorasTrab { get; set; }
        public string? Observacao { get; set; }
        public string? Operador { get; set; }
        public DateTime? DataAlter { get; set; }
        public short? HoraAlter { get; set; }
        public short? TimeStamp { get; set; }
        public double? Cliente { get; set; }
        public short? Reservado01 { get; set; }
        public short? Reservado02 { get; set; }
        public int? Reservado03 { get; set; }
        public int? Reservado04 { get; set; }
        public double? Reservado05 { get; set; }
        public double? Reservado06 { get; set; }
        public string? Reservado07 { get; set; }
        public string? Reservado08 { get; set; }
        public DateTime? Reservado09 { get; set; }
        public DateTime? Reservado10 { get; set; }
    }

    public class HorasAiConfiguration : IEntityTypeConfiguration<HorasAi>
    {
        public void Configure(EntityTypeBuilder<HorasAi> entity)
        {
            entity.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            entity.HasKey(e => new { e.Empresa, e.Filial, e.Pessoa, e.DtLancto, e.Sequencia })
                .HasName("AIHR_KEY0")
                .IsClustered(false);

            entity.ToTable("AIHR");

            entity.Property(e => e.Empresa).HasColumnName("EMPRESA").HasColumnType("smallint").IsRequired();
            entity.Property(e => e.Filial).HasColumnName("FILIAL").HasColumnType("smallint").IsRequired();
            entity.Property(e => e.Pessoa).HasColumnName("PESSOA").HasColumnType("char(10)").IsRequired();            
            entity.Property(e => e.Area).HasColumnName("AREA").HasColumnType("smallint");            
            entity.Property(e => e.DtLancto).HasColumnName("DT_LANCTO").HasColumnType("smalldatetime");
            entity.Property(e => e.Sequencia).HasColumnName("SEQUENCIA").HasColumnType("smallint");
            entity.Property(e => e.Atividade).HasColumnName("ATIVIDADE").HasColumnType("smallint");
            entity.Property(e => e.NroOs).HasColumnName("NRO_OS").HasColumnType("int");
            entity.Property(e => e.HorasTrab).HasColumnName("HRS_TRAB").HasColumnType("decimal(15,6)");
            entity.Property(e => e.Observacao).HasColumnName("OBSERVACAO").HasColumnType("varchar(30)");
            entity.Property(e => e.Cliente).HasColumnName("CLIENTE").HasColumnType("decimal(14,0)");
            entity.Property(e => e.Reservado01).HasColumnName("RESERVADO_01").HasColumnType("smallint");
            entity.Property(e => e.Reservado02).HasColumnName("RESERVADO_02").HasColumnType("smallint");
            entity.Property(e => e.Reservado03).HasColumnName("RESERVADO_03").HasColumnType("int");
            entity.Property(e => e.Reservado04).HasColumnName("RESERVADO_04").HasColumnType("int");
            entity.Property(e => e.Reservado05).HasColumnName("RESERVADO_05").HasColumnType("decimal(15,6)");
            entity.Property(e => e.Reservado06).HasColumnName("RESERVADO_06").HasColumnType("decimal(15,6)");
            entity.Property(e => e.Reservado07).HasColumnName("RESERVADO_07").HasColumnType("varchar(50)");
            entity.Property(e => e.Reservado08).HasColumnName("RESERVADO_08").HasColumnType("varchar(50)");
            entity.Property(e => e.Reservado09).HasColumnName("RESERVADO_09").HasColumnType("smalldatetime");
            entity.Property(e => e.Reservado10).HasColumnName("RESERVADO_10").HasColumnType("smalldatetime");
            entity.Property(e => e.DataAlter).HasColumnName("DATA_ALTER").HasColumnType("smalldatetime");
            entity.Property(e => e.HoraAlter).HasColumnName("HORA_ALTER");
            entity.Property(e => e.Operador).HasColumnName("OPERADOR").HasMaxLength(10).IsUnicode(false);
            entity.Property(e => e.TimeStamp).HasColumnName("TIME_STAMP");
        }
    }
}