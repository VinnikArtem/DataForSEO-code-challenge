using TaskProcessor.Worker.Infrastructure.Models;

namespace TaskProcessor.Worker.Infrastructure.Strategies.MetricCalculation
{
    public class MisspelledKeywordsCountMetricCalculator : IMetricCalculator
    {
        private readonly object _lock = new();

        public void Calculate(Keyword keyword, FileProcessingTaskRequest fileProcessingTaskRequest)
        {
            if (keyword.Spell == null) return;

            lock (_lock)
            {
                fileProcessingTaskRequest.MisspelledKeywordsCount += 1;
            }
        }
    }
}
