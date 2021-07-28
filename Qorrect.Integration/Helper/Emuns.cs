namespace Qorrect.Integration.Helper
{
    public enum SupportedSerilogSinks
    {
        Console = 0,
        File = 1,
        CloudWatch = 2,
        Sentry = 3
    }

    public enum SupportedProductionEnvironments
    {
        DockerUAT = 1,
        DockerMust = 2,
        DockerQAAWS = 3,
        DockerBM = 4,
        DockerEtec = 5
    }
    public enum SupportedTestingEnvironments
    {
        DockerQA = 1,
        DockerStaging = 2
    }
    public enum OrderingMarkingMethod
    {
        AllOrNothing = 1,
        AbsolutePosition = 2,
        LongestContiguousSubset = 3
    }
    public enum OrderingElementsLayout
    {
        Vertical = 1,
        Horizontal = 2
    }
    public enum AssignCourseSubscriptionStatus
    {
        CourseSubscriptionNotExist = 1,
        UserIdNotExist = 2,
        Success = 3
    }
    public enum ImportItemsStatus
    {
        ExamNotExistValidation = 1,
        ExamStatusValidation = 2,
        ErrorSave = 3,
        Success = 4,
        ItemValidationFail = 5,
        ItemIdDuplicationFail = 6,
        ItemOrderDuplicationFail = 7
    }

    public enum CurrentImportStatus
    {
        Invalid = 0,
        InProgress = 1
    }

    public enum ImportStudentErrorType
    {
        UserValidationFail = 1,
        ErrorSave = 2,
        Success = 3,
        NotExistCourseSubscriptionFail = 4,
        StudentIdDuplicationFail = 5,
        StudentEmailDuplicationFail = 6
    }
    public enum ExamItemsClassification
    {
        Objective = 1,
        Subjective = 2,
        Both = 3
    }
    public enum NodeType
    {
        Root = 1,
        Node = 2,
        Leaf = 3
    }
    public enum CourseType
    {
        Elective = 1,
        Compulsory = 2
    }
    //this enum should include only answered and notanswered values only, other values has been migrated to StudentItemAnswersMarkingStatus enum
    //note old values only exist to migrate old data then should be removed
    public enum StudentItemAnswersStatus
    {
        Correct = 1, // should be removed from here after migration applied
        Wrong = 2, //should be removed from here after migration applied
        NotAnswered = 3,
        Answered = 4,
        PartiallyCorrect = 5 //should be removed from here after migration applied
    }
    public enum StudentItemAnswersMarkingStatus
    {
        NotMarked = 0,
        Correct = 1,
        Wrong = 2,
        PartiallyCorrect = 3
    }
    public enum StudentMonitorExamStatus
    {
        Pending = 1,
        Active = 2,
        Finished = 3, // all finished 
        Absent = 4,
        InGrace = 5,
        GraceExpired = 6,
        TimeOut = 7,
        FinishedGrace = 8,
        FinishedInExam = 9
    };

    public enum ExamStudentStatus
    {
        Pending = 0,
        Started = 1,
        Finished = 2,
        TimeOut = 3,
        Missed = 4,
        InGrace = 5,
        FinishedGrace = 6,
        GraceExpired = 7,
    }
    public enum ExamResultPublishStatus
    {
        Notpublished = 0,
        PublishedResultsOnly = 1,
        PublishedWithFeedback = 2,
        MarkingNeeded = 3,
        SubmissionNeeded = 4
    }
    public enum SystemCulture
    {
        enUS = 1,
        arEG = 2,
    }
    public enum ExamStatus
    {
        Draft = 1,
        Published = 2,
        InProgress = 3,
        Finished = 4,
        InGrace = 5,
        ReadyToPublish = 6,
    }
    public enum ExamType
    {
        Manual = 1,
        Quick = 2,
        Rubric = 3
    }
    public enum ExamQuestionView
    {
        AllInOne = 1,
        OneByOne = 2
    }
    public enum DeliveryType
    {
        ComputerBased = 1,
        PaperBased = 2
    }
    public enum SystemRoles
    {

        Admin = 1,
        Teacher = 2,
        Student = 3,
        Manager = 4,
        Writer = 5,
        Reviewer = 6,
        SubjectCreator = 7,
    }
    public enum ApplicationType
    {
        JavaScript = 0,
        NativeConfidential = 1,
    }
    public enum CourseListViewType
    {
        ListView = 1,
        CardView = 2,
    }
    public enum ItemStatus
    {
        New = 1,
        Calibrated = 2,
        Pending = 3,
        Expired = 4,
        Released = 5,
        Modified = 6,
    }
    public enum ItemCategory
    {
        Mapped = 1,
        PartialyMapped = 2,
        NotMapped = 3,
    }
    public enum ItemType
    {
        NotImplemented = 0,
        MCQ = 1,
        TF = 2,
        TFD = 3,
        Essay = 4,
        MRQ = 5,
        Matching = 6,
        ExtendedMRQ = 7,
        ExtendedMatching = 8,
        Ordering = 9,
        TestLet = 10
    }
    public enum ItemClassification
    {
        Objective = 1,
        Subjective = 2,
        Both = 3
    }
    public enum Difficulty
    {
        Easy = 0,
        Moderate = 1,
        Hard = 2,
    }
    public enum UserStatus
    {
        Allowed = 0,
        ForceResetPassword = 1,

    }
    public enum OrderDirection
    {
        asc = 0,
        desc = 1,
    }

    public enum ExamCourseStatus
    {
        NotAssigned = 0,
        Assigned = 1,
        Mixed = 2
    }

    public enum StudentExamResultStatus
    {
        Passed = 0,
        Failed = 1,
        Pending = 2,
        Absent = 3
    }
    public enum TeacherExamResultStatus
    {
        Passed = 0,
        Failed = 1,
        Pending = 2,
        Absent = 3
    }
    public enum TFAnswerText
    {
        True = 1,
        False = 2,
    }
    public enum TFDAnswerText
    {
        True = 1,
        False = 2,
        DontKnow = 3,
    }
    public enum AllowedExcelFileExtensions
    {
        xlsx = 1,
        xls = 2
    }
    public enum AllowedWordFileExtensions
    {
        docx = 1,
        doc = 2
    }

    public enum AllowedWindowsVideoExtensions
    {
        webm = 1,
        ogv = 2
    }

    public enum AllowedMacVideoExtensions
    {
        mp4 = 1
    }

    public enum MarkingStatus
    {
        NotMarked = 1,
        PartiallyMarked = 2,
        Marked = 3
    }
    public enum ItemMarkingAnsweringFilter
    {
        Marked = 1,
        NotMarked = 2,
        NotAnswered = 3,
    }

    public enum ValidationStatus
    {
        BadRequest = 400,
        NotFound = 404,
        Unauthorized = 401,
        Accepted = 202
    }
    public enum StudentAnswerStatus
    {
        Correct = 1,
        Wrong = 2,
        NotAnswered = 3,
        PartiallyCorrect = 4,
    }
    public enum ExamResultPublishStatusFilter
    {
        NotPublished = 1,
        Published = 2,
        NeedMarkingOrSubmission = 3,
    }

    public enum MRQMarkingMethod
    {
        OptionWise = 1,
        AllOrNone = 2
    }

    public enum UploadMediaStatus
    {
        Success = 1,
        Fail = 2
    }

    public enum ItemSeenStatus
    {
        IsSeen = 1,
        NotSeen = 2,
        NotApplicable = 3
    }

    /**
     *  IsReusedResponse = false -> Reused, NotReused
     *  IsReusedResponse = true -> Forked
     */
    public enum ReuseStatus
    {
        NotReused = 0,
        Reused = 1,
        Forked = 2
    }

    public enum ExamLayout
    {
        Regular = 0,
        Sectioned = 1
    }

    public enum SectionedExamNavigation
    {
        ForwardOnlyWithoutTimeRestriction = 0,
        RectrictToSectionTime = 1,
        AllowPreviousNavigation = 2
    }

    public enum SectionedExamMarkingScheme
    {
        PassOverAllExamOnly = 0,
        PassOverAllExamAndSections = 1
    }

    public enum ItemNavigationType
    {
        TimeRestricted = 1,
        MoveForwardOnly = 2,
        AllowBackward = 3
    }

    public enum SectionValidationStatus
    {
        Valid = 0,
        NotValid = 1
    }

    public enum SectionVisitStatus
    {
        NotVisited = 0,
        Visited = 1
    }
    public enum SectionStudentStatus
    {
        Pending = 0,
        Finished = 1,
        Timeout = 2
    }
    public enum SectionStudentResultStatus
    {
        Pending = 0,
        Passed = 1,
        Failed = 2
    }

    /// <summary>
    /// add this enum because course subscription roles order not same as system order
    /// </summary>
    public enum CourseSubscriptionRoleEnum
    {
        Manager = 1,
        Writer = 2,
        Examiner = 3,
        Reviewer = 4,
        Student = 5,
        SubjectCreator = 6,
    }

    public enum GenderTypeEnum
    {
        Male = 1,
        Female = 2
    }
    public enum StudentResultGrade
    {
        Poor = 0,
        Fair = 1,
        Good = 2,
        VeryGood = 3,
        Excellent = 4
    }

    public enum UpdateUserStatusEnum
    {
        UsersNotExist = 1,
        Success = 2
    }
    public enum PValueRangeDistibutionEnum
    {
        NotImplemented = 0,
        TooDifficult = 1,
        VeryDifficult = 2,
        Difficult = 3,
        Average = 4,
        Easy = 5,
        VeryEasy = 6,
        TooEasy = 7
    }

    public enum DiscriminationRangeDistibutionEnum
    {
        NotImplemented = 0,
        BadDisc = 1,
        NoDisc = 2,
        Weak = 3,
        Fair = 4,
        Good = 5,
        VeryGood = 6
    }
    public enum ImportUserTypeEnum
    {
        Excel = 1
    }
    public enum ImportUserStatusEnum
    {
        CreatingTemp = 1,
        Preview = 2,
        Importing = 3,
        FailedCreatingTemp = 4,
        FailedImporting = 5,
        Success = 6
    }

    public enum UserTemplateColumnsEnum
    {
        RowIndex,
        FullName,
        Email,
        Mobile,
        IsAdmin,
        IsExamManager,
        IsSubjectCreator,
        IsItemWriter,
        IsExaminer,
        IsReviewer,
        IsExaminee,
        Gender,
        ID,
        Password,
        BirthDate,
        RegistrationCode,
        RegistrationDate
    }
    public enum ImportUsertabsEnum
    {
        ValidUsers = 1,
        InValidUsers = 2,
        DuplicatedUsers = 3,
        ExistedUsers = 4

    }
    public enum ImportUserResultStatusEnum
    {
        BatchNotExist = 1,
        ValidTempTableNotExist = 2,
        ErrorSaveUsers = 3,
        ErrorUpdateUsers = 4,
        Success = 5
    }

    public enum ItemReviewStatus
    {
        NeedsReview = 0,
        UnderReview = 1,
        Approved = 2,
        Rejected = 3
    }
    public enum ExamReviewStatus
    {
        NeedsReview = 0,
        UnderReview = 1,
        Approved = 2,
        Rejected = 3
    }
    public enum ItemsSubmitStatus
    {
        Success = 0,
        ItemsUnderReviewError = 1,
        DifferentCourseSubscriptionError = 2,
        DifferentItemCreatorError = 3
    }

    public enum ItemsTab
    {
        MyItems = 0,
        UnderReview = 1,
        Reviewed = 2,
        Approved = 3
    }
    public enum ExamsTab
    {
        MyExams = 0,
        UnderReview = 1,
        Reviewed = 2,
        Approved = 3
    }
    public enum ExportExcelUserColumns
    {
        Name = 0,
        Email = 1,
        MobileNumber = 2,
        IsAdmin = 3,
        IsExamManager = 4,
        IsSubjectCreator = 5,
        IsItemWriter = 6,
        IsExaminer = 7,
        IsReviewer = 8,
        IsExaminee = 9,
        Gender = 10,
        Id = 11,
        Password = 12,
        Birthdate = 13,
        RegistrationCode = 14,
        RegistrationDate = 15,
    }
}
