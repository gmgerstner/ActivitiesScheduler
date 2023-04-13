using Extensions.Standard; // Softmax
using System.Linq;

namespace ActivitiesScheduler.MainApp
{
    internal class Schedule
    {
        public List<Activity> Activities { get; set; } = new List<Activity>();
        public List<string> Facilitators { get; set; } = new List<string>();
        public List<Room> Rooms { get; set; } = new List<Room>();
        public List<Section> Sections { get; set; } = new List<Section>();

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
                new Room("Slater 003", 45),
                new Room("Roman 216",  30),
                new Room("Loft 206",   75),
                new Room("Roman 201",  50),
                new Room("Loft 310",  108),
                new Room("Beach 201" , 60),
                new Room("Beach 301",  75),
                new Room("Logos 325", 450),
                new Room("Frank 119",  60),
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
                Activities.Add(new Activity(section, Rooms[Program.random.Next(Rooms.Count)], 10 + Program.random.Next(6), Facilitators[Program.random.Next(Facilitators.Count)]));
            }
        }

        public double Fitness()
        {
            double sum = 0;
            foreach (Activity activity in Activities)
            {
                sum += activity.Fitness(this);
            }
            return sum;
        }

        public Schedule Clone()
        {
            Schedule clone = new Schedule();
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

        public static Schedule CreateChildSchedule(Schedule parent1, Schedule parent2)
        {
            var childSchedule = new Schedule();

            double roll = Program.random.NextDouble();
            if (roll <= Program.MutationRate)
            {
                // Mutation
                childSchedule = new Schedule();
                childSchedule.Initialize();
            }
            else
            {
                // Crossover
                foreach (var activity1 in parent1.Activities)
                {
                    var activity2 = parent2.Activities.Find(a => a.Section.Name == activity1.Section.Name)!;
                    var choice = Program.random.Next(2);
                    if (choice == 0)
                    {
                        childSchedule.Activities.Add(activity1.Clone());
                    }
                    else
                    {
                        childSchedule.Activities.Add(activity2.Clone());
                    }
                }
            }
            return childSchedule.Clone();
        }
    }
}