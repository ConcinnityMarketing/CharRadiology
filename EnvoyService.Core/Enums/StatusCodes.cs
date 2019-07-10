namespace EnvoyService.Core.Enums
{
    public enum HttpVerb
    {
        GET,
        POST,
        PUT,
        DELETE
    }
    public enum LoginStatusCodes
    {
        Success = 1,
        InvalidLogin = 2,
        InvalidPassword = 3,
        ExpiredSession = 4,
        Unsubscribed = 5,
        ExpiredPassword = 6,
        AccountLocked = 7,
        Other = 8
    }
    public enum GenericStatusCodes
    {
        Success = 1,
        Fail = 2,
        Other = 3
    }
    public enum RegistrationStatusCodes
    {
        Success = 1,
        EmailExists = 2,
        AgeVerificationFail = 3,
        Other = 4,
        BandanaOffer = 5,
        PromoLimitReached = 6,
        IndivIDExists = 7,
        NotAgeVerfiedSameEmail = 8
    }
    public enum ReportStatusCodes
    {
        Complete = 1,
        CompleteNoData = 2,
        InvalidRequest = 3,
        ReportNotReady = 4,
        Fail = 5,
        Other = 6
    }
    public enum PinEntryStatusCodes
    {
        Success = 1,
        InvalidPin = 2,
        InvalidYear = 3,
        ProfileExists = 4,
        Other = 5
    }
    public enum RetailLocatorStatusCodes
    {
        Success = 1,
        InvalidZip = 2,
        InvalidGeocode = 3,
        Other = 4
    }
    public enum LikeTypes
    {
        LIKE = 0,
        FLAG = 1
    }
    public enum SocialTypes
    {
        hobbies = 1,
        photos = 2,
        quote = 3,
        topics = 4,
        product = 5
    }
    public enum SweepsStatusCodes
    {
        winner = 1,
        winnerfirst = 2,
        loser = 3,
        loserfirst = 4,
        fail = 5,
        previousentry = 6,
        previouswinner = 7
    }
    public enum ApprovalTypes
    {
        post = 1,
        comment = 2,
        tag = 3,
        image = 4
    }
    public enum SurveyStatusCodes
    {
        Success = 1,
        SurveyExists = 2,
        Other = 3
    }
    public enum UnsubscribeStatusCodes
    {
        Success = 1,
        Fail = 2,
        Other = 3
    }
    public enum MailStatusCodes
    {
        Success = 1,
        Bounce = 2,
        InvalidSecurityCode = 3,
        Other = 4
    }
    public enum AppRetailerStatusCodes
    {
        Success = 1,
        InvalidGeocode = 2,
        Other = 3
    }
    public enum GeoSearchCodes
    {
        ByLatLong = 1,
        ByCityState = 2,
        ByZip = 3
    }
    public enum AVStatusCodes
    {
        SuccessAV = 1,
        NotAgeVerified = 2,
        UnderAge = 3,
        Other = 4
    }
    public enum UserStatusCodes
    {
        Success = 1,
        IncorrectRequest = 2,
        DoesNotExist = 3,
        Other = 4
    }
    public enum VisitStatusCodes
    {
        Success = 1,
        AlreadyCheckedIn = 2,
        CheckInNotAvailable = 3,
        Other = 4
    }
    public enum TrailStatusCodes
    {
        Success = 1,
        NoTrailsExist = 2,
        NoTrailDetailsExist = 3,
        Other = 4
    }
    public enum NoteStatusCodes
    {
        Success = 1,
        NoNotesExist = 2,
        NoNoteDetailsExist = 3,
        Other = 4
    }
    public enum HoursStatusCodes
    {
        Success = 1,
        NoHoursExist = 2,
        Other = 3
    }
    public enum ContestStatusCodes
    {
        Success = 1,
        CodeAlreadyUsed = 2,
        InvalidCode = 3,
        InvalidIndivID = 4,
        Other = 5
    }

}
