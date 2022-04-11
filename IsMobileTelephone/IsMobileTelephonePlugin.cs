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
            if (context.InputParameters.Contains("Data"))
            {
                data = (string)context.InputParameters["Data"];

                data = FormatText(data);
                if (data.StartsWith("0") && !data.StartsWith("00"))
                {
                    string tempData = "+49";
                    for (int i = 1; i < data.Length; i++)
                        tempData += data[i];
                    data = tempData;
                }
                if (!data.StartsWith("0") && !data.StartsWith("00") && !data.StartsWith("+"))
                {
                    string temptData = "+49";
                    data = temptData + data;
                }
                if(data.StartsWith("00"))
                {
                    string tempData = "+";
                    for (int i = 2; i < data.Length; i++)
                        tempData += data[i];
                    data = tempData;
                }
                //   s
                type = (data.StartsWith("+4915") || data.StartsWith("+4916") || data.StartsWith("+4917")) ? true : false;
            }

            context.OutputParameters["Type"] = type;
            context.OutputParameters["FormatedNumber"] = data;
        }
        private string FormatText(string text)
        {
            char[] symbolsToDelete = { ' ', '\\', '/', '-', '_', '(' , ')', '.'};
            foreach (char c in symbolsToDelete) { text = text.Replace(c, ' '); }
            text = String.Concat(text.Where(c => !Char.IsWhiteSpace(c)));
            return text;
        }
    }
}
