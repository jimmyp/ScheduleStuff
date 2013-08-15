namespace ScheduleStuff.Sample
{
    public class IncrementCount : IVolatileTask
    {
        public void Run()
        {
            Counter.Value++;
        }
    }
}