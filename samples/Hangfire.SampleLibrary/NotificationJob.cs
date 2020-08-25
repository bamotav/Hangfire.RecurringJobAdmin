using System;

namespace Hangfire.SampleLibrary
{
    public class NotificationJob
    {
        public void Run()
        {
            Console.WriteLine("Test job from another library");
        }
    }
}
