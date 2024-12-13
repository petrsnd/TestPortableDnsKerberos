namespace TestPortableDnsKerberos
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Kerberos.NET.Client;
    using Kerberos.NET.Credentials;
    using Kerberos.NET.PortableDns;

    internal static class Program
    {
        private static string PromptForInput(string value, bool secure = false, string defaultValue = "")
        {
            Console.Write(value);
            Console.Write(defaultValue.Length > 0 ? $"[{defaultValue}]: " : ": ");
            var sb = new StringBuilder();
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.KeyChar == '\r' || key.KeyChar == '\n')
                {
                    Console.WriteLine();
                    break;
                }
                else if (key.KeyChar == '\b' || key.KeyChar == 0x7f)
                {
                    if (sb.Length > 0)
                    {
                        Console.Write("\b \b");
                        sb.Remove(sb.Length - 1, 1);
                    }
                }
                else
                {
                    sb.Append(key.KeyChar);
                    if (secure)
                    {
                        Console.Write('*');
                    }
                    else
                    {
                        Console.Write(key.KeyChar);
                    }
                }
            }

            return sb.Length == 0 ? defaultValue : sb.ToString();
        }

        private static async Task Main()
        {
            var nameServer = PromptForInput("Name Server", defaultValue: "192.168.117.5");
            var username = PromptForInput("Username", defaultValue: "dan@petrsnd.corp");
            var password = PromptForInput("Password", true);
            var serviceName = PromptForInput("Service Principal Name:", defaultValue: "termsrv/rdp1.petrsnd.corp");

            PortableDnsClient.Configure(nameServer);
            var client = new KerberosClient();
            var kerbCred = new KerberosPasswordCredential(username, password);
            await client.Authenticate(kerbCred);
            var ticket = await client.GetServiceTicket(serviceName);
            Console.WriteLine("Negotiate " + Convert.ToBase64String(ticket.EncodeGssApi().ToArray()));
        }
    }
}
