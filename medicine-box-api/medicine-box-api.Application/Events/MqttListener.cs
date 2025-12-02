using medicine_box_api.Domain.Configuration;
using medicine_box_api.Domain.Dtos;
using medicine_box_api.Domain.Interface;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Protocol;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace medicine_box_api.Application.Events;
public class MqttListener(
    ILogger<MqttListener> logger,
    IOptions<MqttConfiguration> cfg,
    IMqttTopics topics) : BackgroundService
{
    private IMqttClient mqttClient;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var factory = new MqttClientFactory();
        mqttClient = factory.CreateMqttClient();

        mqttClient.ApplicationMessageReceivedAsync += args =>
        {
            var topic = args.ApplicationMessage.Topic ?? string.Empty;

            var payload = args.ApplicationMessage.ConvertPayloadToString() ?? string.Empty;

            var message = new MqttMessage();

            logger.LogDebug("MQTT IN  Topic={Topic} QoS={Qos} Retain={Retain} Payload={Payload}",
                topic,
                args.ApplicationMessage.QualityOfServiceLevel,
                args.ApplicationMessage.Retain,
                payload);

            try
            {
                message = JsonSerializer.Deserialize<MqttMessage>(payload, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (JsonException jex)
            {
                logger.LogWarning(jex, "JSON inválido. Topic={Topic} Payload={Payload}", topic, payload);
            }

            logger.LogDebug($"Mensagem pos-parsing: {message}");

            return Task.CompletedTask;
        };

        mqttClient.DisconnectedAsync += async _ =>
        {
            logger.LogWarning("MQTT desconectado. Tentando reconectar em 3s...");
            await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);
            if (!cancellationToken.IsCancellationRequested)
                await ConnectAndSubscribeAsync(cancellationToken);
        };

        await ConnectAndSubscribeAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (mqttClient != null && mqttClient.IsConnected)
            await mqttClient.DisconnectAsync();

        await base.StopAsync(cancellationToken);
    }

    #region Métodos Privados
    private async Task ConnectAndSubscribeAsync(CancellationToken ct)
    {
        var config = cfg.Value;

        var mqttPort = int.Parse(config.Port ?? "8883");

        var optionsBuilder = new MqttClientOptionsBuilder()
            .WithClientId(config.ClientId + "-listener")
            .WithTcpServer(config.Host, mqttPort)
            .WithKeepAlivePeriod(TimeSpan.FromSeconds(config.KeepAliveSeconds))
            .WithCleanSession(config.CleanSession);

        if (!string.IsNullOrWhiteSpace(config.Username))
            optionsBuilder = optionsBuilder.WithCredentials(config.Username, config.Password);

        if (config.TLS)
            optionsBuilder = optionsBuilder.WithTlsOptions(tls => tls
                .WithSslProtocols(System.Security.Authentication.SslProtocols.Tls12));

        await mqttClient!.ConnectAsync(optionsBuilder.Build(), ct);
        logger.LogInformation("MQTT conectado em {Host}:{Port}", config.Host, config.Port);


        var alarmStatusTopic = topics.AlarmStatus() ?? string.Empty;

        var subs = new MqttClientSubscribeOptionsBuilder()
            .WithTopicFilter(alarmStatusTopic, MqttQualityOfServiceLevel.AtLeastOnce)
            .Build();

        await mqttClient.SubscribeAsync(subs, ct);
        logger.LogInformation("MQTT subscribed: {Topic}", alarmStatusTopic);
    }

    #endregion
}
