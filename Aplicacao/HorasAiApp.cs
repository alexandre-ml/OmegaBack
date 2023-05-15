using Aplicacao.Interfaces;
using Dominio.Interface;
using Entidades.Entidades;
using System.Linq.Expressions;

namespace Aplicacao
{
    public class HorasAiApp : IWHorasAiApp
    {
        private readonly IWHorasAi _app;

        public HorasAiApp(IWHorasAi app)
        {
            _app = app;
        }

        public async Task Add(HorasAi obj)
        {
            await _app.Add(obj);
        }

        public async Task Delete(HorasAi obj)
        {
            await _app.Delete(obj);
        }

        public async Task<HorasAi?> GetEntidadeByExpressionAsync(Expression<Func<HorasAi, bool>> exT, bool AsNoTracking = true)
        {
            return await _app.GetEntidadeByExpressionAsync(exT, AsNoTracking);
        }

        public async Task<List<HorasAi?>> GetListAsync()
        {
            return await _app.GetListAsync();
        }

        public async Task Update(HorasAi obj)
        {
            await _app.Update(obj);
        }
    }
}
