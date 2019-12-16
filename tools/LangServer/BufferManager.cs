using System.Collections.Concurrent;

class BufferManager
{
    private ConcurrentDictionary<string, string> buffers = new ConcurrentDictionary<string, string>();
    private ConcurrentDictionary<string, (PragmaScript.Scope, PragmaScript.TypeChecker)> scopes = new ConcurrentDictionary<string, (PragmaScript.Scope, PragmaScript.TypeChecker)>();

    public void UpdateBuffer(string documentPath, string text)
    {
        buffers.AddOrUpdate(documentPath, text, (k, v) => text);
    }

    public string GetBuffer(string documentPath)
    {
        if (buffers.TryGetValue(documentPath, out var result))
        {
            return result;
        }
        else
        {
            return null;
        }
    }
    public void Annotate(string documentPath, PragmaScript.Scope rootScope, PragmaScript.TypeChecker tc)
    {
        scopes.AddOrUpdate(documentPath, (rootScope, tc), (k, v) => (rootScope, tc));
    }
    public (PragmaScript.Scope, PragmaScript.TypeChecker tc) GetScope(string documentPath)
    {
        if (scopes.TryGetValue(documentPath, out var result))
        {
            return result;
        }
        else
        {
            return (null, null);
        }
    }

}