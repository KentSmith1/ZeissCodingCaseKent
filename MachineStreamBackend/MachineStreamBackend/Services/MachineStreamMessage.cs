namespace MachineStreamBackend.Services
{
    public class MachineStreamMessage
    {
        public String Topic { get; set; }
        public String? Ref   {get; set;}
        public StreamMessagePayload Payload { get; set; }
        public String Event { get; set; }

    }

    public class StreamMessagePayload
    {        
        public String MachineID { get; set; }
        public String ID { get; set; }  
        public DateTime TimeStamp { get; set; }
        public Status Status { get; set;  } 
    }

    public class MachineStreamResult
    {
        public String Topic { get; set; }
        public String? Ref { get; set; }
        public MachineStreamResultPayload Payload { get; set; }
        public String Event { get; set; }

    }

    public class MachineStreamResultPayload
    {
        public String MachineID { get; set; }
        public String ID { get; set; }
        public DateTime TimeStamp { get; set; }
        public String Status { get; set; }
    }

    public enum Status
    {
        idle,
        running,
        finished,
        errored,
        repaired
    }
}
