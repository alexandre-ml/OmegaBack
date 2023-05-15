using Infraestrutura.Enums;
using System.Globalization;

namespace Infraestrutura.Utils
{
    public static class WTipos
    {
        #region Operadores
        public static string SearchOperatorToStr(WSearchOperator searchOper)
        {
            string ret = string.Empty;

            switch (searchOper)
            {
                case WSearchOperator.soEqual: ret = "="; break;
                case WSearchOperator.soNotEqual: ret = "!="; break;
                case WSearchOperator.soLessThan: ret = "<"; break;
                case WSearchOperator.soLessThanOrEqualTo: ret = "<="; break;
                case WSearchOperator.soGreaterThan: ret = ">"; break;
                case WSearchOperator.soGreaterThanOrEqualTo: ret = ">="; break;
                case WSearchOperator.soLike: ret = "LIKE"; break;
                case WSearchOperator.soNotLike: ret = "NOT LIKE"; break;
                case WSearchOperator.soBetween: ret = "BETWEEN @1 AND @2"; break;
                case WSearchOperator.soNotBetween: ret = "NOT BETWEEN @1 AND @2"; break;
                case WSearchOperator.soInList: ret = "IN (@)"; break;
                case WSearchOperator.soNotInList: ret = "NOT IN (@)"; break;
                case WSearchOperator.soBlank: ret = "IS NULL"; break;
                case WSearchOperator.soNotBlank: ret = "IS NOT NULL"; break;
            }

            return ret;
        }

        public static string SummaryFunctionToStr(WSummaryFunction fnc, string coluna)
        {
            string ret = string.Empty;

            switch (fnc)
            {
                case WSummaryFunction.sfSum: ret = "SUM(@)"; break;
                case WSummaryFunction.sfCount: ret = "COUNT(@)"; break;
                case WSummaryFunction.sfAverage: ret = "AVG(@)"; break;
                case WSummaryFunction.sfMin: ret = "MIN(@)"; break;
                case WSummaryFunction.sfMax: ret = "MAX(@)"; break;
            }

            ret = ret.Replace("@", coluna);

            return ret;
        }

        /// <summary>
        /// Formata um valor pra ser usado na cláusula WHERE de uma query, levando em conta
        /// o tipo de dado JSON original para o campo
        /// </summary>
        /// <param name="tipoJson"></param>
        /// <param name="valor"></param>
        /// <returns>O valor formatado de acordo com o tipo.</returns>
        public static string GetValueForWhere(string tipoJson, string valor)
        {
            string obj = string.Empty;
            if (tipoJson == "string")
                obj = $"'{valor ?? string.Empty}'";
            else
            if (tipoJson == "number")
            {
                valor ??= "0";
                try { _ = Convert.ToDouble(valor, CultureInfo.InvariantCulture); obj = valor; }
                catch { obj = "0"; }
            }
            else
            if (tipoJson == "date")
            {
                DateTime data;
                try { data = Convert.ToDateTime(valor, CultureInfo.InvariantCulture); }
                catch { data = DateTime.Now; }
                obj = $"'{data.ToString("yyyy-MM-dd")}'";
            }

            return obj;
        }
        #endregion // Operadores

        #region Exportação
        public static string GetFileExtForFormat(WExportFormat format)
        {
            string ret = string.Empty;

            if (format == WExportFormat.Excel ||
                format == WExportFormat.ExcelReport)
                ret = "xlsx";

            return ret;
        }
        #endregion Exportação
    }
}
