using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PostMateApp.Core.Application.Helpers
{
    public static class YoutubeHelper
    {
        public static string ExtractIntegrationLink(string url)
        {
            string pattern = @"(?:https?://)?(?:www\.)?youtu(?:\.be|be\.com)/(watch\?v=)?([^\?&]+)";
            Match match = Regex.Match(url, pattern);
            return match.Success ? $"https://www.youtube.com/embed/{match.Groups[2].Value}" : null;
        }
    }

}
