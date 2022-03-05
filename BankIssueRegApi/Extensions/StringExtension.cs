using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BankIssueRegApi.Extensions
{
    public static class StringExtension
    {

    
        public static string Format(this Regex re, string input, IDictionary<string, string> args)
        {
            return re.Replace(input, match => args[match.Groups[1].Value]);
        }
    }
}
