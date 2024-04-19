using System.Text.RegularExpressions;
using System.Linq;

namespace SoupAPI
{
    public class ScriptHelper
    {
        public static string[] SplitNonEmpty(string src, string seperator) => Regex.Split(src, seperator, RegexOptions.Multiline).Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
    }
}