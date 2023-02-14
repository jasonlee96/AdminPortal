using CommonService.Enums;

namespace AdminPortal.Business.Repositories
{
    public interface IErrorMessageTemplateRepository
    {
        Task<ErrorMessageTemplate> GetErrorMessageAsync(ErrorCode code, LanguageCode lang, Context outerContext = null); 
        Task<List<ErrorMessageTemplate>> GetAll(Context outerContext = null);
    }
}
