namespace ActivitiesScheduler.MainApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            double mutation = Convert.ToDouble(args[0]);
            int generations = Convert.ToInt32(args[1]);

            Schedule schedule = new Schedule(mutation);
            schedule.Initialize();
            double previousFitness = schedule.TotalFitness();

            Schedule nextSchedule = schedule.NextGeneration();
            double nextFitness = nextSchedule.TotalFitness();

            int generation = 1;

            while (Math.Abs((nextFitness - previousFitness) / previousFitness) >= 0.01 && generation < generations) // Quit if improvement isn't better than 1%
            {
                previousFitness = schedule.TotalFitness();
                nextSchedule = schedule.NextGeneration();
                nextFitness = nextSchedule.TotalFitness();

                generation++;
                schedule = nextSchedule;
                Console.WriteLine($"Fitness: {nextFitness}");
            }

            Console.WriteLine();
            Console.WriteLine($"Generations: {generations}");
            Console.WriteLine($"TotalFitness: {schedule.TotalFitness()}");
        }
    }
}