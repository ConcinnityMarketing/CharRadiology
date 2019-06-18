namespace CharRadiology.Core.Enums
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
        ExpiredPassword = 3,
        ExpiredSession = 4,
        Other = 5
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
    public enum MailStatusCodes
    {
        Success = 1,
        Bounce = 2,
        InvalidSecurityCode = 3,
        Other = 4
    }

}
