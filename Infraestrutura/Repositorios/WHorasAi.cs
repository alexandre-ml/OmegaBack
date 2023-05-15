using Dominio.Interface;
using Entidades.Entidades;
using Infraestrutura.Configuracoes;
using Infraestrutura.Utils;

namespace Infraestrutura.Repositorios
{
    public class WHorasAi : WBData<HorasAi>, IWHorasAi
    {
        private HorasAi? _horasAi;

        public WHorasAi(WDbContext dbContext) : base(dbContext)
        {
        }

        public new async Task Add(HorasAi obj)
        {
            if (InsOk(obj))
            {
                await base.Add(obj);
            }
            else
            {
                string erro = string.Empty;

                WUtils.erro(Erro, ref erro);

                throw new Exception(erro);
            }

        }
        public new async Task Update(HorasAi obj)
        {
            if (AltOk(obj))
            {
                var objUpdate = await GetEntidadeByExpressionAsync(db =>
                  db.Empresa == obj.Empresa &&
                  db.Filial == obj.Filial &&
                  db.Pessoa == obj.Pessoa &&
                  db.DtLancto == obj.DtLancto &&
                  db.Sequencia == obj.Sequencia, false
                  );

                objUpdate.Area = obj.Area;
                objUpdate.DataAlter = new DateTime(2023,05,14);
                objUpdate.HoraAlter = 1518;

                await base.Update(objUpdate);
            }
            else
            {
                string erro = string.Empty;

                WUtils.erro(Erro, ref erro);

                throw new Exception(erro);
            }
        }
        public new async Task Delete(HorasAi obj)
        {
            if (ExcOk(obj))
            {
                var objUpdate = await GetEntidadeByExpressionAsync(db =>
                  db.Empresa == obj.Empresa &&
                  db.Filial == obj.Filial &&
                  db.Pessoa == obj.Pessoa &&
                  db.DtLancto == obj.DtLancto &&
                  db.Sequencia == obj.Sequencia, false
                  );

                await base.Delete(objUpdate);
            }
            else
            {
                string erro = string.Empty;

                WUtils.erro(Erro, ref erro);

                throw new Exception(erro);
            }
        }
        public bool AltOk(HorasAi obj)
        {
            if (Ok(obj))
            {
                if (VerificaExisteObj(obj))
                    return true;

                Erro.Add("Lançamento de Horas não Cadastrado");
            }

            return false;
        }

        public bool ExcOk(HorasAi obj)
        {
            if (VerificaExisteObj(obj))
                return true;

            Erro.Add("Lançamento de Horas não Cadastrado");

            return false;
        }

        public bool InsOk(HorasAi obj)
        {
            if (Ok(obj))
            {
                if (VerificaExisteObj(obj) == false)
                    return true;

                Erro.Add("Lançamento de Horas já Cadastrado");
            }

            return false;
        }

        public bool Ok(HorasAi obj)
        {
            if (WUtils.ValidarPropInt(obj.Empresa))
                Erro.Add("Empresa não informada");

            if (WUtils.ValidarPropInt(obj.Filial))
                Erro.Add("Filial não informada");

            if (WUtils.ValidarPropData(obj.DtLancto))
                Erro.Add("Data de lançamento não informada");

            if (WUtils.ValidarPropInt(obj.Sequencia))
                Erro.Add("Sequencia não informada");

            if (WUtils.ValidarPropString(obj.Pessoa))
                Erro.Add("Pessoa não informada");

            return Erro.Count == 0;
        }

        private bool VerificaExisteObj(HorasAi obj)
        {
            var data = GetEntidadeByExpressionAsync(db =>
                  db.Empresa == obj.Empresa &&
                  db.Filial == obj.Filial &&
                  db.Pessoa == obj.Pessoa &&
                  db.DtLancto == obj.DtLancto &&
                  db.Sequencia == obj.Sequencia, false
                );

            while (!data.IsCompleted) ;

            { 
                //data.Dispose();

                return data.Result != null;
            }

            //return false;
        }
    }
}
