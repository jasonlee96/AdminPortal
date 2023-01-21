using CommonService.Enums;

namespace AdminPortal.Api.Repositories
{
    public interface IErrorMessageTemplateRepository
    {
        Task<ErrorMessageTemplate> GetErrorMessageAsync(ErrorCode code, LanguageCode lang, Context outerContext = null); 
    }
}
