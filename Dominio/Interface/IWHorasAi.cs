using Entidades.Entidades;

namespace Dominio.Interface
{
    public interface IWHorasAi : IWGeneric<HorasAi>
    {
        bool Ok(HorasAi obj);
        bool InsOk(HorasAi obj);
        bool AltOk(HorasAi obj);
        bool ExcOk(HorasAi obj);
    }
}
