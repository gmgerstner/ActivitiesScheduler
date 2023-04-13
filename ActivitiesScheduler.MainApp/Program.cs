namespace ActivitiesScheduler.MainApp
{
    internal class Program
    {
        public static Random random = new Random();
        public static double MutationRate;

        private static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            MutationRate = Convert.ToDouble(args[0]);             // Arg 1: Mutation
            int generations = Convert.ToInt32(args[1]);           // Arg 2: Generations
            double minimumChange = Convert.ToDouble(args[2]);     // Arg 3: Minimum change
            int initialPopulationSize = Convert.ToInt32(args[3]); // Arg 4: Initial population size

            // Create first generation randomly
            List<Schedule> schedules = new List<Schedule>();
            for (int i = 0; i < initialPopulationSize; i++)
            {
                Schedule schedule = new Schedule();
                schedule.Initialize();
                schedules.Add(schedule);
            }
            var previousFitness = schedules.Average(s => s.Fitness());
            Console.WriteLine($"Generation #: {1}\t\tFitness: {previousFitness}");

            // Run generations
            for (int g = 1; g < generations; g++)
            {
                List<Schedule> schedules_sorted = schedules.OrderByDescending(schedule => schedule.Fitness()).ToList();
                List<Schedule> schedules_survivors = schedules_sorted
                    .Take((int)Math.Round(schedules.Count / 2m)) // Only better half survives
                    .Select(s => s.Clone()) // Clone so as to keep from modifying previous generation
                    .ToList();
                List<Schedule> nextGeneration = new List<Schedule>();
                foreach (var parent1 in schedules_survivors)
                {
                    Schedule parent2;
                    do
                    {
                        parent2 = schedules_survivors[random.Next(schedules_survivors.Count)];
                    } while (parent1 == parent2);
                    Schedule child1 = Schedule.CreateChildSchedule(parent1, parent2);
                    Schedule child2 = Schedule.CreateChildSchedule(parent1, parent2);
                    nextGeneration.Add(child1);
                    nextGeneration.Add(child2);
                }
                var nextFitness = nextGeneration.Average(s => s.Fitness());

                var change = Math.Abs((nextFitness - previousFitness) / previousFitness);
                Console.WriteLine($"Generation #: {g + 1}\tFitness: {nextFitness:F2}\tImprovement: {change * 100:F2}%");

                // Replace generation with new one
                previousFitness = nextFitness;
                schedules = nextGeneration;
                if (change < minimumChange)
                {
                    break;
                }
            }

            var bestSchedule = schedules.OrderByDescending(s => s.Fitness()).First();

            Console.WriteLine();
            Console.WriteLine($"Best Fitness: {bestSchedule.Fitness:F2}");

            Console.WriteLine();
            Console.WriteLine("*** Schedule ***");
            Console.WriteLine("Section - Time Slot - Room - Facilitator");
            foreach (var activity in bestSchedule.Activities.OrderBy(a => a.TimeSlot).ThenBy(a => a.Section.Name))
            {
                var timeSlot_text = (activity.TimeSlot > 12) ? (activity.TimeSlot - 12) : activity.TimeSlot;
                var am_pm = (activity.TimeSlot >= 12) ? "PM" : "AM";
                Console.WriteLine($"{activity.Section.Name} - {timeSlot_text}:00 {am_pm} - {activity.Room.Name} - {activity.Facilitator}");
            }
        }
    }
}