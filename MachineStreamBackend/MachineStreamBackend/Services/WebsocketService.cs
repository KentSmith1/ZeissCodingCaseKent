using Newtonsoft.Json.Linq;
using System.Net.WebSockets;
using System.Text;

namespace MachineStreamBackend.Services
{
    static class WebsocketService
    {
        private static String connectionString = "ws://machinestream.herokuapp.com/ws";
        private static List<MachineStreamMessage> MessageList = new List<MachineStreamMessage>();

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
                            //Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, result.Count));
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
            //Console.WriteLine(machineStreamMessage);
            MessageList.Add(machineStreamMessage); 
        }

        public static List<MachineStreamResult> GetMachineStreamMessages()
        {
            var MachineStreamResults = new List<MachineStreamResult>();
            MessageList.ForEach(m => MachineStreamResults.Add(new MachineStreamResult
            {
                Topic = m.Topic,
                Ref = m.Ref,
                Event = m.Event,
                Payload = new MachineStreamResultPayload
                {
                    MachineID = m.Payload.MachineID,
                    ID = m.Payload.ID,
                    TimeStamp = m.Payload.TimeStamp,
                    Status = m.Payload.Status.ToString()
                }
            }));
            return MachineStreamResults;
        }

        public static List<MachineStreamResult> GetMachineStreamMessagesByMachine(string id)
        {
            return GetMachineStreamMessages().Where(x => x.Payload.MachineID == id).ToList();
        }

        public static string GetMachineStatus(string id)
        {
            var List = GetMachineStreamMessages();
            return List.Where(x => x.Payload.MachineID == id && x.Payload.TimeStamp == List.Max(a => a.Payload.TimeStamp)).SingleOrDefault().Payload.Status;
        }

    }
}
