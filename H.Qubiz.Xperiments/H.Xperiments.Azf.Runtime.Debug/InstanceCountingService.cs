
namespace H.Xperiments.Azf.Runtime.Debug
{
    public class InstanceCountingService
    {
        static int instanceIdSequence = -1;

        readonly int instanceId = 0;
        int injectionCount = 0;

        public InstanceCountingService()
        {
            instanceId = Interlocked.Increment(ref instanceIdSequence);
        }

        public InstanceCountingService CountInjection()
        {
            Interlocked.Increment(ref injectionCount);
            return this;
        }

        public Info GetInfo() => new Info
        {
            InstanceID = instanceId,
            InjectionCount = injectionCount,
        };

        public class Info
        {
            public int InstanceID { get; set; }
            public int InjectionCount { get; set; }
        }
    }
}
