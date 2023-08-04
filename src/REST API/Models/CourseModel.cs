namespace EduPlatformAPI.Models
{
    public class CourseModel
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        // это внутри курса
        public string CourseTags { get; set; }
        public int CourseLikes { get; set; }
        public Dictionary<string, object> UIBlocks { get; set; }
    }
    public class HeaderWithDescriptionClass
    {
        public int id { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
    }
    public class HeaderWithFileClass
    {
        public int id { get; set; }
        public string FileName { get; set; }
        public string FileURL { get; set; }
    }
    public class SectionViewClass
    {
        public int id { get; set; }
        public string Title { get; set; }
        public Dictionary<string, object> SectionFilling { get; set; }
    }
}
