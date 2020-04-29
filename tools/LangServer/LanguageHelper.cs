using PragmaScript;

public class LanguageHelper
{
    public static bool InsideToken(Token token, int pos, int line)
    {
        // TODO(pragma): we have tokens that span multiple lines. implement.
        if (token.Line != line)
        {
            return false;
        }
        return pos >= token.Pos && pos < token.Pos + token.length;
    }

    public static AST.Node FindNode(Scope scope, int posZeroBased, int lineZeroBased, string filePath)
    {
        AST.Node FindNodeRec(AST.Node node, int pos, int line)
        {
            if (node.token.filename != filePath)
            {
                return null;
            }
            if (InsideToken(node.token, pos, line))
            {
                return node;
            }
            else
            {
                if (node is AST.FieldAccess fa)
                {
                    if (InsideToken(fa.fieldNameToken, pos, line))
                    {
                        return node;
                    }
                }
                foreach (var child in node.GetChilds())
                {
                    var result = FindNodeRec(child, pos, line);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            return null;
        }
        AST.Node result = FindNodeRec(scope.owner, posZeroBased + 1, lineZeroBased + 1);
        return result;
    }

    public static Scope GetCurrentScope(Scope rootScope, int charIdxZeroBased, int lineIdxZeroBased, string filePath)
    {
        Scope GetCurrentScopeRec(Scope scope, int charIdx, int lineIdx)
        {
            var result = scope;
            foreach (var c in result.childs)
            {
                if (c.tokenRanges.Count == 0)
                {
                    continue;
                }
                bool inside = false;
                foreach (var tr in c.tokenRanges)
                {
                    if (tr.start.filename != filePath)
                    {
                        continue;
                    }
                    inside |= lineIdx > tr.start.Line && lineIdx < tr.end.Line;
                    if (tr.start.Line == lineIdx)
                    {
                        if (tr.end.Line != tr.start.Line)
                        {
                            inside |= charIdx >= tr.start.Pos;
                        }
                        else
                        {
                            inside |= charIdx >= tr.start.Pos && charIdx <= tr.end.Pos;
                        }
                    }
                }
                if (inside)
                {
                    result = GetCurrentScopeRec(c, charIdx, lineIdx);
                    break;
                }
            }
            return result;
        }
        var result = GetCurrentScopeRec(rootScope, charIdxZeroBased + 1, lineIdxZeroBased + 1);
        return result;
    }
}