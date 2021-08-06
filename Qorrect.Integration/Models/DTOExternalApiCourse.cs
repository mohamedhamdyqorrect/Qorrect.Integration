using Qorrect.Integration.Helper;
using System;
using System.Collections.Generic;

namespace Qorrect.Integration.Models
{

    public class DTOAddCourseRequest
    {
        public string BearerToken { get; set; }
        public string CourseSubscriptionId { get; set; }
    }

    public class DTOAddCourseLevelRequest
    {
        public string BearerToken { get; set; }
        public int CourseId { get; set; }
        public Guid ParentId { get; set; }
    }

    public class DTOExternalApiCourse
    {
        public List<DTOCourses> courses { get; set; }
    }

    public class DTOCourses
    {
        public string fullname { get; set; }
        public string shortname { get; set; }
        public int categoryid { get; set; }
        public string summary { get; set; }
        public string categoryname { get; set; }

    }

    public class DTOAddEditCourse
    {
        public Guid? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid? CourseSubscriptionId { get; set; }
        public DTOCourseData CourseData { get; set; }
    }

    public class DTOCourseData
    {
        public CourseType? CourseType { get; set; }
        public int? CreditHours { get; set; }
        public int? LecturesHours { get; set; }
        public int? PracticalHours { get; set; }
        public string Description { get; set; }
        public double? TotalMarks { get; set; }
        public double? TotalHours { get; set; }
        public List<DTOLevelCourseDataTag> Tags { get; set; }
        public bool IsValidTotalMarks { get { return (TotalMarks == null || TotalMarks >= 0); } }
        public bool IsValidTotalHours { get { return (TotalHours == null || TotalHours >= 0); } }
    }

    public class DTOLevelCourseDataTag
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
    }

    public class DTOBedoCourse
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public int CreditHours { get; set; }
        public int PracticalHours { get; set; }
        public int ClassesHours { get; set; }
        public int LectureHours { get; set; }
        public string Description { get; set; }
        public double? TotalMarks { get; set; }
    }

    public class DTOAddEditNodeLevel
    {
        public DTOAddEditNodeLevel()
        {
            TeachingHours = Marks = ILOCount = ChildrenCount = LeafsCount = 0;
        }
        public Guid? Id { get; set; }
        public Guid ParentId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ChildOutlineNodeName { get; set; }
        public NodeType? ChildOutlineNodeType { get; set; }
        public int? ChildrenCount { get; set; }
        public int? LeafsCount { get; set; }
        public double? Marks { get; set; }
        public int? ILOCount { get; set; }
        public double? TeachingHours { get; set; }
        public int? Order { get; set; }
        public string LeafOutlineNodeName { get; set; }
    }

    public class CourseLeaf
    {
        public CourseLeaf()
        {
            IntendedLearningOutcomes = new List<Guid>();
        }

        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public double? TeachingHours { get; set; }
        public double? Marks { get; set; }
        public int? Weight { get; set; }
        public Guid? ParentId { get; set; }
        public List<Guid> IntendedLearningOutcomes { get; set; }
        public int? Order { get; set; }
        public List<CourseLeaf> leaves { get; set; }
    }


    public class DTOBedoUnites
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? Order { get; set; }
        public double? Marks { get; set; }

        public List<DTOBedoLessons> Lessons { get; set; }
    }

    public class DTOBedoLessons
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? Order { get; set; }
        public int? ParentId { get; set; }

    }

    public class DTOApplyOutlineStructure
    {
        public Guid Id { get; set; }
    }

}
