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
                foreach(Match match in matches)
                {
                    address = match.Value;
                    break;
                }
                var splitedAddress = address.Split('\n');
                street = splitedAddress[4];
                for (int i = 0; i < 5; i++)
                    postalCode += splitedAddress[6][i];
                for (int i = 6; i < splitedAddress[6].Length; i++)
                    city += splitedAddress[6][i];
            }
            context.OutputParameters["Street"] = street;
            context.OutputParameters["PostalCode"] = postalCode;
            context.OutputParameters["City"] = city;
        }
    }

}
