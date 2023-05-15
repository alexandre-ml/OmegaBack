using Dominio.Interface;
using Entidades.Entidades;
using Infraestrutura.Configuracoes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.SafeHandles;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Infraestrutura.Repositorios
{
    public class WBData<T> : IWGeneric<T> where T : class
    {
        protected readonly DbContextOptions<WDbContext> _optionsBuilder;
        protected readonly WDbContext _dbContext;

        protected List<string> Erro;
        public WBData(WDbContext dbContext)
        {
            _optionsBuilder = new DbContextOptions<WDbContext>();
            _dbContext = dbContext;

            Erro = new List<string>();
        }

        public async Task Add(T obj)
        {
            await _dbContext.Set<T>().AddAsync(obj);

            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(T obj)
        {
            _dbContext.Set<T>().Remove(obj);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<T?> GetEntidadeByExpressionAsync(Expression<Func<T, bool>> exT, bool AsNoTracking = true)
        {
            //var cmd = _dbContext.Set<T>()
            //    .Where(exT);

            //if (AsNoTracking)
            //    cmd = cmd.AsNoTracking();

            //return await cmd.FirstOrDefaultAsync();

            if (AsNoTracking)
            {
                return await _dbContext.Set<T>().Where(exT).AsNoTracking().FirstOrDefaultAsync();
            }
            else
            {
                return await _dbContext.Set<T>().Where(exT).FirstOrDefaultAsync();
            }
        }

        public async Task<List<T>> GetListAsync()
        {
            var cmd = _dbContext.Set<T>()
                .AsNoTracking();

            //if(exT != null)
            //    cmd.Where(exT);

            return await cmd.ToListAsync();           
        }

        public async Task Update(T obj)
        {
            _dbContext.Set<T>().Update(obj);

            await _dbContext.SaveChangesAsync();
        }

        protected async Task<T?> VerificaExisteObj(HorasAi obj, Expression<Func<T, bool>> exT)
        {
            var data = await GetEntidadeByExpressionAsync(exT);
            
            return data;
        }

        #region Disposed
        // To detect redundant calls
        private bool _disposedValue;

        // Instantiate a SafeHandle instance.
        private SafeHandle _safeHandle = new SafeFileHandle(IntPtr.Zero, true);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose() => Dispose(true);

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _safeHandle.Dispose();
                }

                _disposedValue = true;
            }
        }
        #endregion
    }
}
