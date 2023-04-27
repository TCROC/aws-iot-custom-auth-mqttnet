using System.Web;
using MQTTnet;
using MQTTnet.Extensions.WebSocket4Net;
using MQTTnet.Formatter;
using MQTTnet.Protocol;
using MQTTnet.Diagnostics;
using MQTTnet.Client;
using System.Net.Security;

Console.WriteLine("Running mqtt example application!");

int i = 0;
string username = args[i++];
string password = args[i++];
string endpoint = args[i++];
string rootTopic = args[i++];
string authorizer = args[i++];
string transportImplementation = args[i++]; // websocket4net | dotnet
string transport = args[i++]; // websocket | tcp

Console.WriteLine($"{Environment.NewLine}==================={Environment.NewLine}");
Console.WriteLine($"Args Used{Environment.NewLine}");
Console.WriteLine($"{nameof(username)}:                  {username}");
Console.WriteLine($"{nameof(password)}:                  {password}");
Console.WriteLine($"{nameof(endpoint)}:                  {endpoint}");
Console.WriteLine($"{nameof(rootTopic)}:                 {rootTopic}");
Console.WriteLine($"{nameof(authorizer)}:                {authorizer}");
Console.WriteLine($"{nameof(transportImplementation)}:   {transportImplementation}");
Console.WriteLine($"{nameof(transport)}:                 {transport}");
Console.WriteLine($"{Environment.NewLine}==================={Environment.NewLine}");

var factory = new MqttFactory();

if (transportImplementation == "websocket4net")
{
    factory = factory.UseWebSocket4Net();
}

var optionsBuilder = factory.CreateClientOptionsBuilder();

if (transport == "tcp")
{
    optionsBuilder = optionsBuilder.WithTcpServer($"{endpoint}", 443);
}
else if (transport == "websocket")
{
    optionsBuilder = optionsBuilder.WithWebSocketServer($"{endpoint}/mqtt");
}

var options = optionsBuilder
    .WithClientId(username)
    .WithCredentials($"username?x-amz-customauthorizer-name={HttpUtility.UrlEncode(authorizer)}", password)
    .WithWillTopic($"{rootTopic}/s/{username}")
    .WithWillRetain(false)
    .WithWillPayload(new byte[] { 0 })
    .WithWillQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
    .WithTls(
        new MqttClientOptionsBuilderTlsParameters
        {
            UseTls = true,
            ApplicationProtocols = new List<SslApplicationProtocol> { new("mqtt") }
        }
    )
    .WithProtocolVersion(MqttProtocolVersion.V500)
    .WithKeepAlivePeriod(TimeSpan.FromSeconds(35))
    .Build();

var logger = new MqttNetEventLogger("MqttNet");
logger.LogMessagePublished += (obj, logArgs) => Console.WriteLine(logArgs.LogMessage.ToString());

var client = factory.CreateMqttClient(logger);
var result = await client.ConnectAsync(options);

int delaySeconds = 10;
while (client.IsConnected)
{
    Console.WriteLine($"Client is connected. Checking again in {delaySeconds} seconds...");
    await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
}