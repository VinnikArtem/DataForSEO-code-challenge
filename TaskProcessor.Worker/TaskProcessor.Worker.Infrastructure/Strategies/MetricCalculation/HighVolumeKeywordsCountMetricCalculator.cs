using TaskProcessor.Worker.Infrastructure.Models;

namespace TaskProcessor.Worker.Infrastructure.Strategies.MetricCalculation
{
    public class HighVolumeKeywordsCountMetricCalculator : IMetricCalculator
    {
        private readonly object _lock = new();

        public void Calculate(Keyword keyword, FileProcessingTaskRequest fileProcessingTaskRequest)
        {
            if (!keyword.KeywordInfo.SearchVolume.HasValue || keyword.KeywordInfo.SearchVolume.Value <= Constants.KeywordHighVolume) return;

            lock (_lock)
            {
                fileProcessingTaskRequest.HighVolumeKeywordsCount += 1;
            }
        }
    }
}
