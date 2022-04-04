using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IsMobileTelephone
{
    public class IsMobileTelephonePlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService =
                (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            bool type = false;
            string data = "";
            if (context.InputParameters.Contains("Pattern") &&
                context.InputParameters.Contains("Data"))
            {
                string pattern = (string)context.InputParameters["Pattern"];
                data = (string)context.InputParameters["Data"];

                var regex = new Regex(pattern);
                var matches = regex.Matches(data);
                type = matches.Count != 0 ? true : false;

            }
            context.OutputParameters["Type"] = type;
            context.OutputParameters["FormatedNumber"] = FormatText(String.Concat(data.Where(c => !Char.IsWhiteSpace(c))));
        }
        private string FormatText(string text)
        {
            text = text.Contains("00") ? text.Replace("00", "+") : text;
            string formatedText = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (i == 3)
                    formatedText += '(';
                formatedText += text[i];
                if (i == 5)
                    formatedText += ')';
            }
            return formatedText;
        }
    }
}
