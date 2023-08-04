using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEDP.MVVM.Model
{
    internal class CourseUIModel
    {
        // это снаружи
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        // это внутри курса
        public string CourseTags { get; set; }
        public int CourseLikes { get; set; }
        public List<int> CourseTeachersID { get; set; }
        //оформление
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
