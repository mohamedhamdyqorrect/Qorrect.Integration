using System.Collections.Generic;

namespace Qorrect.Integration.Models
{
    public class DTOClientRequest
    {
        public string RequestUri { get; set; }
        public string MethodType { get; set; }
        public HashSet<DTOHeaderKey> Headers { get; set; }
        public string RequestBody { get; set; }
        public string ResponseBody { get; set; }
        public string Status { get; set; }
        public string CourseId { get; set; }
        public string Device { get; set; }
    }

    public class DTOHeaderKey
    {
        public string Key { get; set; }
        public HashSet<string> Value { get; set; }
    }
}
