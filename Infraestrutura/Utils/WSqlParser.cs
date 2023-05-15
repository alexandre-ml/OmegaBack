using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Infraestrutura.Utils
{
    public class WSqlParser
    {
        #region Constantes
        public readonly List<TSqlTokenType> JoinTokens = new()
        {
            TSqlTokenType.Left,  TSqlTokenType.Right, TSqlTokenType.Cross,
            TSqlTokenType.Inner, TSqlTokenType.Outer, TSqlTokenType.Full,
            TSqlTokenType.Join,  TSqlTokenType.RightOuterJoin
        };

        public readonly List<TSqlTokenType> BlankTokens = new()
        {
            TSqlTokenType.WhiteSpace, TSqlTokenType.SingleLineComment, TSqlTokenType.MultilineComment
        };

        public readonly List<TSqlTokenType> UnionTokens = new() { TSqlTokenType.Union, TSqlTokenType.All };
        #endregion // Constantes

        #region Propriedades
        public string SQL { get; protected set; }
        public IList<ParseError> Errors { get; protected set; }
        public bool HasErrors { get => Errors.Count > 0; }

        // Select Comum
        public bool IsUnaryExpression { get => SelectElements != null; }
        public IList<SelectElement>? SelectElements { get; protected set; }
        public FromClause? FromClause { get; protected set; }
        public WhereClause? WhereClause { get; protected set; }
        public OrderByClause? OrderByClause { get; protected set; }

        // Union
        public bool IsBynaryExpression { get => FirstQuery != null; }
        public QueryExpression? FirstQuery { get; protected set; }
        public QueryExpression? SecondQuery { get; protected set; }
        #endregion // Propriedades

        protected TSqlParser parser;
        protected TSqlFragment fragment;
        public WSqlParser(string sql)
        {
            SQL = sql;
            parser = new TSql160Parser(false);

            using var reader = new StringReader(SQL);
            fragment = parser.Parse(reader, out var lErrors);
            Errors = lErrors;

            if (HasErrors)
            {
                SelectElements = null;
                FromClause = null;
                WhereClause = null;
                OrderByClause = null;
                FirstQuery = null;
                SecondQuery = null;
            }
            else
                ExtractClauses();
        }

        protected void ExtractClauses()
        {
            try
            {
                var scr = fragment as TSqlScript;
                var batch = scr.Batches[0];
                var stmt = batch.Statements[0] as SelectStatement;
                var qe = stmt.QueryExpression as QuerySpecification;

                SelectElements = qe?.SelectElements;
                FromClause = qe?.FromClause;
                WhereClause = qe?.WhereClause;
                OrderByClause = qe?.OrderByClause;

                var bqe = stmt.QueryExpression as BinaryQueryExpression;

                FirstQuery = bqe?.FirstQueryExpression;
                SecondQuery = bqe?.SecondQueryExpression;
            }
            catch (Exception)
            {
                var v = new CustomVisitor(this);
                fragment.Accept(v);
            }
        }
        // Separa os UNIONS em querys independentes
        // Se bão tiver union, retorna a lista com uma única query.
        public List<string> GetSelectClauses()
        {
            var ret = new List<string>();
            if (fragment != null)
            {
                var query = string.Empty;
                var lastTokenType = TSqlTokenType.None;

                for (int i = 0; i <= fragment.LastTokenIndex; i++)
                {
                    var token = fragment.ScriptTokenStream[i];
                    if (UnionTokens.Contains(token.TokenType))
                    {
                        if (!UnionTokens.Contains(lastTokenType))
                            ret.Add(query.Trim());
                        query = string.Empty;
                    }
                    else
                        query += token.Text; // Não inclui as palavras UNION nem UNION ALL

                    if (!BlankTokens.Contains(token.TokenType))
                        lastTokenType = token.TokenType;
                }

                if (query.Trim() != string.Empty)
                    ret.Add(query.Trim());
            }
            return ret;
        }

        public List<string> GetFromClause()
        {
            var ret = new List<string>();
            if (FromClause != null)
            {
                var from = string.Empty;
                var lastTokenType = TSqlTokenType.None;

                // Não inclui a palavra chave FROM
                for (int i = FromClause.FirstTokenIndex + 1; i <= FromClause.LastTokenIndex; i++)
                {
                    var token = fragment.ScriptTokenStream[i];
                    if (JoinTokens.Contains(token.TokenType) && !JoinTokens.Contains(lastTokenType))
                    {
                        ret.Add(from.Trim());
                        from = string.Empty;
                    }

                    from += token.Text;
                    if (!BlankTokens.Contains(token.TokenType))
                        lastTokenType = token.TokenType;
                }

                if (from.Trim() != string.Empty)
                    ret.Add(from.Trim());
            }
            return ret;
        }

        public string GetWhereClause()
        {
            string ret = string.Empty;
            if (WhereClause != null)
            {
                // Extrai o WHERE sem a palavra chada no início
                ret = SQL.Substring(WhereClause.StartOffset + 6, WhereClause.FragmentLength - 6);
                ret = ret.Replace("\r", string.Empty);
                ret = ret.Replace("\n", " ");

            }
            return ret;
        }

        //public static string RemoveComments(string sql)
        //{
        //    string pattern = @"(?<=^ ([^'""] |['][^']*['] |[""][^""]*[""])*) (--.*$|/\*(.|\n)*?\*/)";
        //    return Regex.Replace(sql, pattern, "", RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
        //}

        #region Visitor
        internal class CustomVisitor : TSqlFragmentVisitor
        {
            public WSqlParser Parser { get; }
            public CustomVisitor(WSqlParser parser)
            {
                Parser = parser;
            }

            public override void Visit(QuerySpecification node)
            {
                if (Parser.SelectElements == null)
                    Parser.SelectElements = node.SelectElements;

                if (Parser.FromClause == null)
                    Parser.FromClause = node.FromClause;

                if (Parser.WhereClause == null)
                    Parser.WhereClause = node.WhereClause;

                base.Visit(node);
            }
            #endregion // Visitor
        }
    }
}
