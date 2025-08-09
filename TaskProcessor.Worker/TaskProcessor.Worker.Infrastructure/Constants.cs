namespace TaskProcessor.Worker.Infrastructure
{
    public struct Constants
    {
        public const int KeywordHighVolume = 100000;

        public struct FileTypes
        {
            public const string Json = ".json";
        }

        public struct QueueNames
        {
            public const string SubtaskUpdate = "dispatcher.subtask_update";
            public const string RunSuperTask = "worker.run_super_task";
        }
    }
}
