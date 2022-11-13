using MachineStreamBackend.Services;

namespace MachineStreamBackend
{
    public class MessagesRepository
    {
        private MessagesContext context = new MessagesContext();
        
        public List<StreamMessagePayload> GetStreamMessagePayloads()
        {
            return context.MessagePayloads.ToList();
        }

        public void AddStreamMessagePayload(StreamMessagePayload payload)
        {
            context.MessagePayloads.Add(payload);
            context.SaveChanges();
        }

        public List<StreamMessagePayload> GetPayloadByMachineID(string MachineID)
        {
            return context.MessagePayloads.Where(x => x.MachineID == MachineID).ToList();
        }

        public StreamMessagePayload GetLatestPayloadByMachineID(string MachineID)
        {
            var list = GetPayloadByMachineID(MachineID);
            return list.SingleOrDefault(x => x.MachineID == MachineID && x.TimeStamp == list.Max(y => y.TimeStamp));
        }
    }
}
