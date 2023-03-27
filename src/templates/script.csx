#r "Newtonsoft.Json"
#load "./_context.csx"

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

// This is the main entry point for the script. The context parameter contains information about the current request and response.
// Define the context values you want to use in the script.
ApimContext context = new ApimContext();

// Call the ExtractedScript method
string result = ExtractedScript(context);

// Write out the result
Console.WriteLine(result);

private static string ExtractedScript(ApimContext context)
{
    return "{0}";
}        
