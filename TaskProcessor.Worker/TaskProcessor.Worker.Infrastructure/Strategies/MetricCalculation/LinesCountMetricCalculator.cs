using TaskProcessor.Worker.Infrastructure.Models;

namespace TaskProcessor.Worker.Infrastructure.Strategies.MetricCalculation
{
    public class LinesCountMetricCalculator : IMetricCalculator
    {
        private readonly object _lock = new();

        public void Calculate(Keyword keyword, FileProcessingTaskRequest fileProcessingTaskRequest)
        {
            lock (_lock)
            {
                fileProcessingTaskRequest.LinesCount += 1;
            }
        }
    }
}
