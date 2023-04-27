using System.Web;
using MQTTnet;
using MQTTnet.Extensions.WebSocket4Net;
using MQTTnet.Formatter;
using MQTTnet.Protocol;
using MQTTnet.Diagnostics;

Console.WriteLine("Running mqtt example application!");

int i = 0;
string username = args[i++];
string password = args[i++];
string endpoint = args[i++];
string rootTopic = args[i++];
string authorizer = args[i++];

Console.WriteLine($"{Environment.NewLine}==================={Environment.NewLine}");
Console.WriteLine($"Args Used:{Environment.NewLine}");
Console.WriteLine($"{nameof(username)}: {username}");
Console.WriteLine($"{nameof(password)}: {password}");
Console.WriteLine($"{nameof(endpoint)}: {endpoint}");
Console.WriteLine($"{nameof(rootTopic)}: {rootTopic}");
Console.WriteLine($"{nameof(authorizer)}: {authorizer}");
Console.WriteLine($"{Environment.NewLine}==================={Environment.NewLine}");

var factory = new MqttFactory();
var options = factory
    .UseWebSocket4Net()
    .CreateClientOptionsBuilder()
    .WithClientId(username)
    .WithCredentials($"username?x-amz-customauthorizer-name={HttpUtility.UrlEncode(authorizer)}", password)
    // .WithTcpServer($"{endpoint}", 443)
    .WithWebSocketServer($"{endpoint}/mqtt")
    .WithWillTopic($"{rootTopic}/s/{username}")
    .WithWillRetain(false)
    .WithWillPayload(new byte[] { 0 })
    .WithWillQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
    .WithTls()
    .WithProtocolVersion(MqttProtocolVersion.V500)
    .WithKeepAlivePeriod(TimeSpan.FromSeconds(35))
    .Build();

var logger = new MqttNetEventLogger("MqttNet");
logger.LogMessagePublished += (obj, logArgs) => Console.WriteLine(logArgs.LogMessage.ToString());

var client = factory.CreateMqttClient(logger);
var result = await client.ConnectAsync(options);

int delaySeconds = 10;
while(client.IsConnected)
{
    Console.WriteLine($"Client is connected. Checking again in {delaySeconds} seconds...");
    await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
}