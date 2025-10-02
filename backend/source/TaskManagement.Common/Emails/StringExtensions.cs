using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Common.Emails
{
    public static class StringExtensions
    {
        public static string ReplaceTag(this string text, string tag, string value) =>
            !string.IsNullOrWhiteSpace(value)
                ? text.Replace($"<<{tag}>>", value)
                : text.Replace($"<<{tag}>>", "?????");
    }
}







