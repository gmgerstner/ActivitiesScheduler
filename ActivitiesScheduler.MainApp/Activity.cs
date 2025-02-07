﻿namespace ActivitiesScheduler.MainApp
{
    internal class Activity
    {
        public Activity(Section section, Room room, int timeSlot, string facilitator)
        {
            Section = section;
            Room = room;
            TimeSlot = timeSlot;
            Facilitator = facilitator;
        }

        public Section Section { get; set; }
        public Room Room { get; set; }
        public int TimeSlot { get; set; }
        public string Facilitator { get; set; }

        public double Fitness(Schedule clientAgency)
        {
            double fitness = 0; // For each activity, fitness starts at 0. 

            // Activity is scheduled at the same time in the same room as another of the activities: -0.5 
            foreach (var activity in clientAgency.Activities)
            {
                if (activity == this) continue;
                if ((activity.Room == Room) && (activity.TimeSlot == TimeSlot))
                {
                    fitness -= 0.5;
                }
            }

            // Room size 
            if (Room.Capacity < 6 * Section.ExpectedEnrollment)
            {
                // Activities is in a room with capacity > 6 times expected enrollment: -0.4 
                fitness -= 0.4;
            }
            else if (Room.Capacity < 3 * Section.ExpectedEnrollment)
            {
                // Activities is in a room with capacity > 3 times expected enrollment: -0.2 
                fitness -= 0.5;
            }
            else if (Room.Capacity < Section.ExpectedEnrollment)
            {
                // Activities is in a room too small for its expected enrollment: -0.5 
                fitness -= 0.5;
            }
            // Otherwise + 0.3 
            else
            {
                fitness += 0.3;
            }

            if (Section.PreferedFacilitators.Contains(Facilitator))
            {
                // Activities is overseen by a preferred facilitator: + 0.5 
                fitness += 0.5;
            }
            else if (Section.OtherFacilitators.Contains(Facilitator))
            {
                // Activities is overseen by another facilitator listed for that activity: +0.2 
                fitness += 0.2;
            }
            else
            {
                // Activities is overseen by some other facilitator: -0.1 
                fitness -= 0.1;
            }

            // ***** Facilitator load ***** 

            int activitiesInCurrentTimeSlot = clientAgency.Activities
                .Where(a => a.Facilitator == Facilitator)
                .Where(a => a.TimeSlot == TimeSlot)
                .Count();
            if (activitiesInCurrentTimeSlot == 1)
            {
                // Activity facilitator is scheduled for only 1 activity in this time slot: + 0.2 
                fitness += 0.02;
            }
            else if (activitiesInCurrentTimeSlot >= 2)
            {
                // Activity facilitator is scheduled for more than one activity at the same time: - 0.2 
                fitness -= 0.2;
            }
            int activitiesTotal = clientAgency.Activities
                .Where(a => a.Facilitator == Facilitator)
                .Count();
            if (activitiesTotal > 4)
            {
                // Facilitator is scheduled to oversee more than 4 activities total: -0.5 
                fitness -= 0.5;
            }
            if (activitiesTotal <= 2)
            {
                // Facilitator is scheduled to oversee 1 or 2 activities*: -0.4 
                if (activitiesTotal >= 2 && Facilitator != "Tyler")
                {
                    // Exception: Dr. Tyler is committee chair and has other demands on his time. 
                    //     *No penalty if he’s only required to oversee < 2 activities. 
                    fitness -= 0.4;
                }
            }
            //TODO Finish coding fitness function
            // If any facilitator scheduled for consecutive time slots: Same rules as for SLA 191 and SLA 101 in consecutive time slots—see below.              

            // ***** Activity-specific adjustments *****

            // The 2 sections of SLA 101 are more than 4 hours apart: + 0.5 
            var sla101actvities = clientAgency
                .Activities
                .Where(a => a.Section.Name.StartsWith("SLA100")) // Requirements specify 101 but is probably wrong and should be 100
                .ToList();
            int time_diff_101 = Math.Abs(sla101actvities[0].TimeSlot - sla101actvities[1].TimeSlot);
            // The 2 sections of SLA 191 are more than 4 hours apart: + 0.5 
            if (time_diff_101 > 4)
            {
                fitness += 0.5;
            }
            // Both sections of SLA 101 are in the same time slot: -0.5 
            if(time_diff_101 == 0)
            {
                fitness -= 0.5;
            }

            var sla191actvities = clientAgency
                .Activities
                .Where(a => a.Section.Name.StartsWith("SLA191"))
                .ToList();
            int time_diff_191 = Math.Abs(sla101actvities[0].TimeSlot - sla101actvities[1].TimeSlot);
            // Both sections of SLA 191 are in the same time slot: -0.5 
            if(time_diff_191 == 0)
            {
                fitness -= 0.5;
            }

            // A section of SLA 191 and a section of SLA 101 are overseen in consecutive time slots (e.g., 10 AM & 11 AM): +0.5 
            //      In this case only (consecutive time slots), one of the activities is in Roman or Beach, and the other isn’t: -0.4 
            // It’s fine if neither is in one of those buildings, of activity; we just want to avoid having consecutive activities being widely separated.  
            // A section of SLA 191 and a section of SLA 101 are taught separated by 1 hour (e.g., 10 AM & 12:00 Noon): + 0.25 
            // A section of SLA 191 and a section of SLA 101 are taught in the same time slot: -0.25

            return fitness;
        }

        public Activity Clone()
        {
            Activity activity = new Activity(Section, Room, TimeSlot, Facilitator);
            return activity;
        }

    }
}