﻿using Extensions.Standard; // Softmax

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
            Initialize();
            MutationRate = mutationRate;
        }

        private void Initialize()
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

        public void RunGeneration()
        {
            Activities.Shuffle(random);
            for (int p = 0; p < Activities.Count; p += 2)
            {
                // Parents
                Activity parent_activity1 = Activities[p];
                Activity parent_activity2 = Activities[p + 1];

                //TODO Is this correct for offsprings?
                // Offspring
                Activity offspring_activity1 = parent_activity1.Clone();
                Activity offspring_activity2 = parent_activity2.Clone();

                // Mutation
                if (random.NextDouble() < MutationRate)
                {
                    // Handle mutation
                    offspring_activity1.Room = Rooms[random.Next(Rooms.Count)];
                    offspring_activity1.TimeSlot = 10 + random.Next(6);
                    offspring_activity1.Facilitator = Facilitators[random.Next(Facilitators.Count)];
                }
                if (random.NextDouble() < MutationRate)
                {
                    // Handle mutation
                    offspring_activity2.Room = Rooms[random.Next(Rooms.Count)];
                    offspring_activity2.TimeSlot = 10 + random.Next(6);
                    offspring_activity2.Facilitator = Facilitators[random.Next(Facilitators.Count)];
                }
            }
        }
    }
}