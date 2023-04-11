namespace ActivitiesScheduler.MainApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            Schedule schedule = new Schedule(0.01);
            double previousFitness = schedule.TotalFitness();
            double generations = 1;
            while ((schedule.TotalFitness() - previousFitness) / previousFitness >= 0.01 || generations > 100) // Quite if improvement isn't better than 1%
            {
                generations++;
                previousFitness = schedule.TotalFitness();
                schedule.RunGeneration();
            }

            Console.WriteLine();
            Console.WriteLine($"Generations: {generations}");
            Console.WriteLine($"TotalFitness: {schedule.TotalFitness()}");
        }
    }
}