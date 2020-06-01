using System;
using System.IO;

public static class UriHelper
{
    /// <summary>
    ///     Get the local file-system path for the specified document URI.
    /// </summary>
    /// <param name="documentUri">
    ///     The LSP document URI.
    /// </param>
    /// <returns>
    ///     The file-system path, or <c>null</c> if the URI does not represent a file-system path.
    /// </returns>
    public static string GetFileSystemPath(Uri documentUri)
    {
        if (documentUri == null)
            throw new ArgumentNullException(nameof(documentUri));

        if (documentUri.Scheme != "file")
            return null;

        // The language server protocol represents "C:\Foo\Bar" as "file:///c:/foo/bar".
        string fileSystemPath = Uri.UnescapeDataString(documentUri.AbsolutePath);
        if (Path.DirectorySeparatorChar == '\\')
        {
            if (fileSystemPath.StartsWith("/"))
                fileSystemPath = fileSystemPath.Substring(1);

            fileSystemPath = fileSystemPath.Replace('/', '\\');
        }

        var fullPath = Path.GetFullPath(fileSystemPath);
        if (fullPath.StartsWith("/")) 
        {
            return fullPath.Substring(1);
        }
        else
        {
            return fullPath;
        }
    }

    /// <summary>
    ///     Convert a file-system path to an LSP document URI.
    /// </summary>
    /// <param name="fileSystemPath">
    ///     The file-system path.
    /// </param>
    /// <returns>
    ///     The LSP document URI.
    /// </returns>
    public static Uri FromFileSystemPath(string fileSystemPath)
    {
        if (string.IsNullOrWhiteSpace(fileSystemPath))
            throw new ArgumentException("Argument cannot be null, empty, or entirely composed of whitespace: 'fileSystemPath'.", nameof(fileSystemPath));

        if (!Path.IsPathRooted(fileSystemPath))
            throw new ArgumentException($"Path '{fileSystemPath}' is not an absolute path.", nameof(fileSystemPath));

        if (Path.DirectorySeparatorChar == '\\')
            fileSystemPath = fileSystemPath.Replace('\\', '/');

        fileSystemPath = fileSystemPath.Replace(":", System.Uri.EscapeDataString(":"));
        return new Uri("file:///" + fileSystemPath);
    }
}