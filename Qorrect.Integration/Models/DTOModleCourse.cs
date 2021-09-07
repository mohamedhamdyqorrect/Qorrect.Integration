using System.Collections.Generic;

namespace Qorrect.Integration.Models
{

    public class Cours
    {
        public string fullname { get; set; }
        public string shortname { get; set; }
        public string id { get; set; }
        public string idnumber { get; set; }
    }


    public class DTOModleCourse
    {
        public List<Cours> courses { get; set; }
    }
    public class ModelUnit
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool visible { get; set; }
        public List<modellesson> modules { get; set; }//lessons
    }
    public class modellesson
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool visible { get; set; }
    }
}
