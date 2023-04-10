namespace ActivitiesScheduler.MainApp
{
    internal class Section
    {
        public Section(string name, int expectedEnrollment, List<string> preferedFacilitators, List<string> otherFacilitators)
        {
            ExpectedEnrollment = expectedEnrollment;
            Name = name;
            PreferedFacilitators = preferedFacilitators;
            OtherFacilitators = otherFacilitators;
        }

        public int ExpectedEnrollment { get; set; }
        public string Name { get; set; }
        public List<string> PreferedFacilitators { get; set; }
        public List<string> OtherFacilitators { get; set; }
    }
}