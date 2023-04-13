namespace ActivitiesScheduler.MainApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            double mutation = Convert.ToDouble(args[0]);       // Arg 1: Mutation
            int generations = Convert.ToInt32(args[1]);        // Arg 2: Generations
            double minimumChange = Convert.ToDouble(args[2]);  // Arg 3: Minimum Change

            Schedule schedule = new Schedule(mutation);
            schedule.Initialize();
            double previousFitness = schedule.TotalFitness();

            Schedule nextSchedule = schedule.NextGeneration();
            double nextFitness = nextSchedule.TotalFitness();

            int generation = 1;

            while (Math.Abs((nextFitness - previousFitness) / previousFitness) >= minimumChange && generation < generations) // Quit if improvement isn't better than 1%
            {
                previousFitness = schedule.TotalFitness();
                nextSchedule = schedule.NextGeneration();
                nextFitness = nextSchedule.TotalFitness();

                Console.WriteLine($"Generation #:{generation}\t\tFitness: {nextFitness:F2}");
                schedule = nextSchedule;
                generation++;
            }

            Console.WriteLine();
            Console.WriteLine($"Generations: {generation}");
            Console.WriteLine($"TotalFitness: {schedule.TotalFitness()}");
        }
    }
}