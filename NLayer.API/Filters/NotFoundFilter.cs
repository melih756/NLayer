using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayer.Core.DTO;
using NLayer.Core.Services;

namespace NLayer.API.Filters
{
    public class NotFoundFilter<T>:IAsyncActionFilter where T:class
    {
        private readonly Service<T> _service;

        public NotFoundFilter(Service<T> service)
        {
            _service = service;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
       {
            var idValue = context.ActionArguments.Values.FirstOrDefault();

            if (idValue == null)
            {
                await next.Invoke();
                return;
            }
            var id = (int)idValue;
            var anyEntity = await _service.Any(x=>x.Id==id);

            if (anyEntity)
            {
                await next.Invoke();
                return;
            }

            context.Result = new NotFoundObjectResult(CustomResponseDTO<NoContentDTO>.Fail(404,"not found"));
            throw new NotImplementedException();
       }
    }
}
