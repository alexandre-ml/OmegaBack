using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Utils
{
    public static class WUtils
    {
        #region Visual
        /*     
        public static void mostraMsg(Form? form, string msg)
        {
            MessageBox.Show(form, msg, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static void mostraMsg(Form? form, ref string msg, bool temErro)
        {
            if (temErro)
                mostraErro(form, msg);
            msg = $"{DateTime.Now:dd-MM-yyyy hh:mm}{Environment.NewLine}{msg}";
        }
        public static void mostraErro(Form? form, string msg)
        {
            MessageBox.Show(form, msg, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static bool confirma(Form? form, string msg)
        {
            var ret = MessageBox.Show(form, msg, "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return (ret == DialogResult.Yes);
        }

        public static void erro(Form? form, string msg)
        {
            MessageBox.Show(form, msg, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void erro(Form? form, List<string> msgs)
        {
            var msg = string.Empty;
            foreach (var m in msgs)
                msg += m + Environment.NewLine;
            MessageBox.Show(form, msg, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static string GetVersion()
        {
            var ret = string.Empty;
            var ver = Assembly.GetExecutingAssembly().GetName().Version;
            if (ver != null)
                ret += $"{ver.Major}.{ver.Minor}.{ver.Build}";
            return ret;
        }

        public static T FirstOrDefault<T>(IEnumerator coll, Func<T, bool> pred)
            where T : class
        {
            T ret = null;

            if (coll != null)
            {
                coll.Reset();
                while (ret == null && coll.MoveNext())
                {
                    ret = coll.Current as T;
                    if (!pred(ret))
                        ret = null;
                }
            }
            return ret;
        }
        public static void resizeDescriptionArea(ref PropertyGrid grid, int nNumLines)
        {
            try
            {
                var info = grid.GetType().GetProperty("Controls");
                var collection = (Control.ControlCollection)info.GetValue(grid, null);

                foreach (var control in collection)
                {
                    var type = control.GetType();

                    if ("DocComment" == type.Name)
                    {
                        foreach (var f in type.BaseType.GetRuntimeFields())
                        {
                            if (f.Name.Contains("UserSized"))
                            {
                                f.SetValue(control, true);
                                break;
                            }
                        }
                        info = type.GetProperty("Lines");
                        info?.SetValue(control, nNumLines);
                        grid.HelpVisible = true;
                        break;
                    }
                }
            }
            catch (Exception)
            {
            }
        }        
        */
        #endregion // Visual

        public static bool ValidarPropInt(int? valor)
        {
            if (valor == null || valor < 1)
                return true;

            return false;
        }

        public static bool ValidarPropString(string? valor)
        {
            if (string.IsNullOrEmpty(valor))
                return true;

            return false;
        }

        public static bool ValidarPropData(DateTime? valor)
        {
            if (valor == null)
                return true;

            return false;
        }

        public static void erro(List<string> msgs, ref string erro)
        {
            var msg = string.Empty;

            foreach (var m in msgs)
            {
                msg += m + Environment.NewLine;
                erro += m + Environment.NewLine;
            }

            Console.WriteLine(msg);
        }
        #region Windows
        public static string GetWindowsUserName()
        {
            var user = System.Security.Principal.WindowsIdentity.GetCurrent();
            var parts = user.Name.Split('\\');
            return parts[parts.Length - 1];
        }

        public static bool IsAdmim()
        {
            var userName = GetWindowsUserName();

            return (userName == "GUSTAVO" ||
                    userName == "ARNALDO" ||
                    userName == "LIDINEI" ||
                    userName == "MARCIO");
        }
        #endregion // Windows

        #region Arquivos
        public static bool ShiftBackup(string nomeArquivoOrg, ushort maxQtdeBack)
        {
            var ret = true;
            var lNomeArquivoOrg = nomeArquivoOrg;
            try
            {
                // Renomeia o arquivo atual para 0; ele será renomeado para 1 ao final
                // desse procedimento. Ao fazer isso aqui, o sistema fica livre logo de
                // cara para criar um novo arquivo sem o risco de outro executável tentar
                // renomear o log paralelamente a este.
                try
                {
                    File.Move(lNomeArquivoOrg, $"{lNomeArquivoOrg}-{0:D3}", true);
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                    ret = false;
                }

                string lNomeNovo = $"{lNomeArquivoOrg}-{maxQtdeBack:D3}";
                string lNomeAnt;

                // Renomeia o 4 para 5, o 3 para 4, o 2 para 3, o 1 para 2 e o 0 para 1
                for (int i = maxQtdeBack - 1; i >= 0; i--)
                {
                    lNomeAnt = lNomeNovo;
                    lNomeNovo = $"{lNomeArquivoOrg}-{i:D3}";

                    try
                    {
                        File.Move(lNomeNovo, lNomeAnt, true);
                    }
                    catch { };
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                ret = false;
            }

            return ret;
        }
        #endregion // Arquivos

        #region InicializaAmbiente
        public static void InicializaOmegaXml()
        {

        }
        #endregion
    }
}
