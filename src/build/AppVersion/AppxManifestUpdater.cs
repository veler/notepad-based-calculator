using System;
using System.IO;

internal sealed class AppxManifestUpdater
{
    private readonly string _versionRule;

    internal AppxManifestUpdater(string versionRule)
    {
        _versionRule = versionRule;
    }

    public void UpdateFile(string fileName)
    {
        string text = File.ReadAllText(fileName);

        string ouputText = UpdateTextWithRule(text);

        File.WriteAllText(fileName, ouputText);
    }

    public string UpdateTextWithRule(string text)
    {
        VersionString v = null;

        Tuple<string, int, int> g = GetVersionString(text);
        if (g != null)
        {
            VersionString.TryParse(g.Item1, out v);
        }
        if (v != null)
        {
            string newVersion = new VersionUpdateRule(_versionRule).Update(v);
            return text.Substring(0, g.Item2) + newVersion + text.Substring(g.Item2 + g.Item3);
        }
        return text;
    }

    public static Tuple<string, int, int> GetVersionString(string input)
    {
        string identityTagStart = "<Identity";
        string identityTagEnd = ">";
        string versionAttributeStart = "Version=\"";
        string versionAttributeEnd = "\"";

        int identityTagStartPosition = input.IndexOf(identityTagStart);
        if (identityTagStartPosition > -1)
        {
            int identityTagEndPosition = input.IndexOf(identityTagEnd, identityTagStartPosition + identityTagStart.Length);
            if (identityTagEndPosition > -1)
            {
                int versionAttributeStartPosition = input.IndexOf(versionAttributeStart, identityTagStartPosition + identityTagStart.Length);
                if (versionAttributeStartPosition > identityTagStartPosition && versionAttributeStartPosition < identityTagEndPosition)
                {
                    int versionAttributeEndPosition = input.IndexOf(versionAttributeEnd, versionAttributeStartPosition + versionAttributeStart.Length);

                    if (versionAttributeEndPosition > versionAttributeStartPosition && versionAttributeEndPosition < identityTagEndPosition)
                    {
                        string oldVersion = input.Substring(versionAttributeStartPosition + versionAttributeStart.Length, versionAttributeEndPosition - (versionAttributeStartPosition + versionAttributeStart.Length));
                        return new Tuple<string, int, int>(
                            oldVersion,
                            versionAttributeStartPosition + versionAttributeStart.Length,
                            oldVersion.Length);
                    }
                }
            }
        }

        return null;
    }
}
