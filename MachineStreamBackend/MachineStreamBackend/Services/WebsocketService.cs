using Newtonsoft.Json.Linq;
using System.Net.WebSockets;
using System.Text;

namespace MachineStreamBackend.Services
{
    public static class WebsocketService
    {
        private static String connectionString = "ws://machinestream.herokuapp.com/ws";
        private static List<MachineStreamMessage> MessageList = new List<MachineStreamMessage>();
        private static MessagesRepository messagesRepository = new MessagesRepository();

        public static async void StartWS()
        {
            using (var ws = new ClientWebSocket())
            {
                while (true)
                {
                    await ws.ConnectAsync(new Uri(connectionString), CancellationToken.None);
                    var buffer = new byte[256];
                    while (ws.State == WebSocketState.Open)
                    {
                        var result = await ws.ReceiveAsync(buffer, CancellationToken.None);
                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
                        }
                        else
                        {
                            ProcessMessage(Encoding.ASCII.GetString(buffer, 0, result.Count));
                        }
                    }
                }
            }
        }

        private static void ProcessMessage(string message)
        {
            JObject jsonMessage = JObject.Parse(message);
            MachineStreamMessage machineStreamMessage = new MachineStreamMessage
            {
                Topic = jsonMessage.Value<string>("topic"),
                Ref = jsonMessage.Value<string>("ref"),
                Event = jsonMessage.Value<string>("event"),
                Payload = new StreamMessagePayload
                {
                    MachineID = jsonMessage.GetValue("payload").Value<string>("machine_id"),
                    ID = jsonMessage.GetValue("payload").Value<string>("id"),
                    TimeStamp = jsonMessage.GetValue("payload").Value<DateTime>("timestamp"),
                    Status = Enum.Parse<Status>(jsonMessage.GetValue("payload").Value<string>("status"))
                }
            };
            messagesRepository.AddStreamMessagePayload(machineStreamMessage.Payload);
        }

        public static List<MachineStreamResultPayload> GetPayloadMessages()
        {
            return messagesRepository.GetStreamMessagePayloads().Select(x => MapPayload(x)).ToList();
        }

        public static List<MachineStreamResultPayload> GetPayloadMessagesByMachine(string id)
        {
            return messagesRepository.GetPayloadByMachineID(id).Select(x => MapPayload(x)).ToList();
        }

        public static MachineStreamResultPayload GetLatestPayloadMessagesByMachine(string id)
        {
            return MapPayload(messagesRepository.GetLatestPayloadByMachineID(id));
        }

        private static MachineStreamResultPayload MapPayload(StreamMessagePayload payload)
        {
            return new MachineStreamResultPayload
            {
                ID = payload.ID,
                MachineID = payload.MachineID,
                TimeStamp = payload.TimeStamp,
                Status = payload.Status.ToString()
            };
        }

    }
}
