
using AdminPortal.Business.Repositories;
using CommonService.Enums;
using System.Net;

namespace AdminPortal.Api.AppCache
{
    public class AppSetting : IAppSetting
    {
        private readonly IErrorMessageTemplateRepository _errorMessageTemplateRepository;
        public List<ErrorMessageTemplate> ErrorsMessages { get; set; }

        public DateTime ExpiresAt { get; set; }

        public AppSetting(IErrorMessageTemplateRepository errorMessageTemplateRepository) 
        {
            _errorMessageTemplateRepository = errorMessageTemplateRepository ?? throw new ArgumentException(nameof(errorMessageTemplateRepository));
            ExpiresAt = DateTime.MinValue;
            Check();
        }

        public async Task Check()
        {
            if(DateTime.Now <= ExpiresAt)
            {
                await Refresh();
            }
        }

        public async Task Refresh()
        {
            ErrorsMessages.Clear();
            ErrorsMessages = await _errorMessageTemplateRepository.GetAll();
        }

        public async Task<ErrorMessageTemplate?> SelectErrorMessage(ErrorCode code, LanguageCode lang)
        {
            await Check();

            return ErrorsMessages.FirstOrDefault(x => x.Code== code && x.Language == lang);
        }
    }
}
