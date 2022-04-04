using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using System.Text.RegularExpressions;

namespace RegexCustomAction
{
    public class RegexActionPlugin: IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService =
                (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            var street = "";
            string postalCode = "";
            var city = "";
            if (context.InputParameters.Contains("Pattern") &&
                context.InputParameters.Contains("Data"))
            {
                string pattern = (string)context.InputParameters["Pattern"];
                string data = (string)context.InputParameters["Data"];

                var regex = new Regex(pattern);
                var matches = regex.Matches(data);
                string address = "";
                int s = 0;
                foreach(Match match in matches)
                {
                    if (s == 0) { s++;continue; }
                    address = match.Value;
                }
                var splitedAddress = address.Split('\n');
                string[] clearAddress = new string[2];
                int count = 0;
                for (int j = 0; j < splitedAddress.Length; j++)
                {
                    if (splitedAddress[j].Length != 0 && splitedAddress[j] != "Anschrift")
                    {
                        clearAddress[count] = splitedAddress[j];
                        count++;
                    }

                }
                city = clearAddress[0];
                for (int i = 0; i < 5; i++)
                    postalCode += clearAddress[1][i];
                for (int i = 6; i < clearAddress[1].Length; i++)
                    street += clearAddress[1][i];
            }
            context.OutputParameters["Street"] = street;
            context.OutputParameters["PostalCode"] = postalCode;
            context.OutputParameters["City"] = city;
        }
    }

}
