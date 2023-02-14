using CommonService.Enums;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace AdminPortal.Business.Repositories
{
    public class ErrorMessageTemplateRepository : IErrorMessageTemplateRepository
    {
        private readonly IDbContextFactory<Context> _factory;
        private readonly ILogger<ErrorMessageTemplateRepository> _logger;

        public ErrorMessageTemplateRepository(IDbContextFactory<Context> factory, ILogger<ErrorMessageTemplateRepository> logger)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<ErrorMessageTemplate>> GetAll(Context outerContext = null)
        {
            var context = outerContext ?? await _factory.CreateDbContextAsync();
            try
            {
                return await(from x in context.ErrorMessageTemplates
                             select x).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{MethodName}: Error thrown", nameof(GetAll));
                throw ex;
            }
            finally
            {
                if (outerContext == null) await context.DisposeAsync();
            }
        }

        public async Task<ErrorMessageTemplate> GetErrorMessageAsync(ErrorCode code, LanguageCode lang, Context outerContext = null)
        {
            var context = outerContext ?? await _factory.CreateDbContextAsync();
            try
            {
                return await(from x in context.ErrorMessageTemplates
                             where x.Code == code && x.Language == lang
                             select x).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{MethodName}: Error thrown", nameof(GetErrorMessageAsync));
                throw ex;
            }
            finally
            {
                if (outerContext == null) await context.DisposeAsync();
            }
        }
    }
}
