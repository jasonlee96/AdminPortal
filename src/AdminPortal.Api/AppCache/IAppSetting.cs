using CommonService.Enums;

namespace AdminPortal.Api.AppCache
{
    public interface IAppSetting
    {
        Task<ErrorMessageTemplate?> SelectErrorMessage(ErrorCode code, LanguageCode lang);
    }
}
