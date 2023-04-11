namespace ActivitiesScheduler.MainApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            Schedule schedule = new Schedule(0.01);
            schedule.Initialize();
            double previousFitness = schedule.TotalFitness();

            Schedule nextSchedule = schedule.NextGeneration();
            double nextFitness = nextSchedule.TotalFitness();

            double generations = 1;

            while ((nextFitness - previousFitness) / previousFitness >= 0.01 || generations > 100) // Quit if improvement isn't better than 1%
            {
                previousFitness = schedule.TotalFitness();
                nextSchedule = schedule.NextGeneration();
                nextFitness = nextSchedule.TotalFitness();

                generations++;
                schedule = nextSchedule;
            }

            Console.WriteLine();
            Console.WriteLine($"Generations: {generations}");
            Console.WriteLine($"TotalFitness: {schedule.TotalFitness()}");
        }
    }
}