using Extensions.Standard; // Softmax

namespace ActivitiesScheduler.MainApp
{
    internal class Schedule
    {
        private Random random = new Random();
        public List<Activity> Activities { get; set; } = new List<Activity>();
        public List<string> Facilitators { get; set; } = new List<string>();
        public List<Room> Rooms { get; set; } = new List<Room>();
        public List<Section> Sections { get; set; } = new List<Section>();
        public double MutationRate { get; }

        public Schedule(double mutationRate)
        {
            MutationRate = mutationRate;
        }

        public void Initialize()
        {
            Facilitators = new List<string>
            {
                "Lock",
                "Glen",
                "Banks",
                "Richards",
                "Shaw",
                "Singer",
                "Uther",
                "Tyler",
                "Numen",
                "Zeldin",
            };

            Rooms = new List<Room>
            {
                new Room("Slater 003", 45 ),
                new Room("Roman 216",  30 ),
                new Room("Loft 206",   75 ),
                new Room("Roman 201",  50 ),
                new Room("Loft 310",  108 ),
                new Room("Beach 201" , 60 ),
                new Room("Beach 301",  75 ),
                new Room("Logos 325", 450 ),
                new Room("Frank 119",  60  ),
            };

            Sections = new List<Section>()
            {
                new Section("SLA100A", 50, "Glen, Lock, Banks, Zeldin"   .Split(", ").ToList(), "Numen, Richards"                               .Split(", ").ToList()),
                new Section("SLA100B", 50, "Glen, Lock, Banks, Zeldin"   .Split(", ").ToList(), "Numen, Richards"                               .Split(", ").ToList()),
                new Section("SLA191A", 50, "Glen, Lock, Banks, Zeldin"   .Split(", ").ToList(), "Numen, Richards"                               .Split(", ").ToList()),
                new Section("SLA191B", 50, "Glen, Banks, Zeldin, Shaw"   .Split(", ").ToList(), "Numen, Richards"                               .Split(", ").ToList()),
                new Section("SLA201",  50, "Glen, Lock, Banks, Zeldin"   .Split(", ").ToList(), "Numen, Richards, Singer"                       .Split(", ").ToList()),
                new Section("SLA291",  50, "Lock, Banks, Zeldin, Singer" .Split(", ").ToList(), "Numen, Richards, Shaw, Tyler"                  .Split(", ").ToList()),
                new Section("SLA303",  60, "Glen, Zeldin, Banks"         .Split(", ").ToList(), "Numen, Singer, Shaw"                           .Split(", ").ToList()),
                new Section("SLA304",  25, "Glen, Banks, Tyler"          .Split(", ").ToList(), "Numen, Singer, Shaw, Richards, Uther, Zeldin"  .Split(", ").ToList()),
                new Section("SLA394",  20, "Tyler, Singer"               .Split(", ").ToList(), "Richards, Zeldin"                              .Split(", ").ToList()),
                new Section("SLA449",  60, "Tyler, Singer, Shaw"         .Split(", ").ToList(), "Zeldin, Uther"                                 .Split(", ").ToList()),
                new Section("SLA451", 100, "Tyler, Singer, Shaw"         .Split(", ").ToList(), "Zeldin, Uther, Richards, Banks"                .Split(", ").ToList()),
            };

            Activities = new List<Activity>();
            foreach (Section section in Sections)
            {
                Activities.Add(new Activity(section, Rooms[random.Next(Rooms.Count)], 10 + random.Next(6), Facilitators[random.Next(Facilitators.Count)]));
            }
        }

        public double TotalFitness()
        {
            double sum = 0;
            foreach (Activity activity in Activities)
            {
                sum += activity.Fitness(this);
            }
            return sum;
        }

        private Schedule Clone()
        {
            Schedule clone = new Schedule(MutationRate);
            foreach (Activity activity in Activities)
            {
                clone.Activities.Add(activity);
            }
            foreach (string facilitator in Facilitators)
            {
                clone.Facilitators.Add(facilitator);
            }
            foreach (Room room in Rooms)
            {
                clone.Rooms.Add(room);
            }
            foreach (Section section in Sections)
            {
                clone.Sections.Add(section);
            }
            return clone;
        }

        public Schedule NextGeneration()
        {
            Schedule nextGeneration = Clone();

            //TODO Handle crossover (This is incomplete and probably incorrect)
            for (int p = 0; p < nextGeneration.Activities.Count; p += 2)
            {
                // Parents
                Activity parent_activity1 = Activities[p];
                Activity parent_activity2 = Activities[p + 1];

                //TODO Is this correct for offsprings?
                // Offspring
                Activity offspring_activity1 = parent_activity1.Clone();
                Activity offspring_activity2 = parent_activity2.Clone();
            }

            // Handle mutation
            foreach (var activity in nextGeneration.Activities)
            {
                if (random.NextDouble() < MutationRate)
                {
                    // Handle mutation
                    activity.Room = Rooms[random.Next(Rooms.Count)];
                    activity.TimeSlot = 10 + random.Next(6);
                    activity.Facilitator = Facilitators[random.Next(Facilitators.Count)];
                }
            }

            return nextGeneration;
        }
    }
}

//Below is an example of crossover

//class Parent
//{
//    public int Property1 { get; set; }
//    public string Property2 { get; set; }
//}

//class Child
//{
//    public int Property1 { get; set; }
//    public string Property2 { get; set; }
//}

//class CrossoverOperator
//{
//    public static Child Crossover(Parent parent1, Parent parent2)
//    {
//        // Create a new child object
//        Child child = new Child();

//        // Select a random crossover point
//        int crossoverPoint = Random.Next(0, 2);

//        // Combine properties from the two parents based on the crossover point
//        if (crossoverPoint == 0)
//        {
//            child.Property1 = parent1.Property1;
//            child.Property2 = parent2.Property2;
//        }
//        else
//        {
//            child.Property1 = parent2.Property1;
//            child.Property2 = parent1.Property2;
//        }

//        return child;
//    }
//}
