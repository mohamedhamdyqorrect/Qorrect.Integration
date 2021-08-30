using System.Collections.Generic;

namespace Qorrect.Integration.Models
{

    public class Cours
    {
        public string fullname { get; set; }
        public string shortname { get; set; }
        public string idnumber { get; set; }
    }


    public class DTOModleCourse
    {
        public List<Cours> courses { get; set; }
    }
}
