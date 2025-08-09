using TaskProcessor.Worker.Infrastructure.Models;

namespace TaskProcessor.Worker.Infrastructure.Strategies.MetricCalculation
{
    public interface IMetricCalculator
    {
        void Calculate(Keyword keyword, FileProcessingTaskRequest fileProcessingTaskRequest);
    }
}
