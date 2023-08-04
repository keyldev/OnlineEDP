namespace EduPlatformAPI.Models
{
    public class UserModel
    {
        public int uID { get; set; } // auto increment
        public string uLogin { get; set; } // просто букавы
        public string uPassword { get; set; } // md5 или sha256, ещё не решил
        public string uEmail { get; set; } // 
        public string uName { get; set; } // 
        public string uSurname { get; set; } // 
        public int uRole { get; set; } // 0 - студент, 1 - препод, 2 - админ/дев
        public string uGroup { get; set; } // к примеру, ПИ-192
        public string[] uSkills { get; set; } // типа, UI\UX, C# C++++
        public int[] uCourseIDs { get; set; } // на каких курсах
        public int[] uCourseLikeIDs { get; set; } // че лайкнул

    }
}
