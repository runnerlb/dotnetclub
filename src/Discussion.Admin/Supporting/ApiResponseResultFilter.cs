using Discussion.Core.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Discussion.Admin.Supporting
{
    public class ApiResponseResultFilter : IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                objectResult.Value = ApiResponse.ActionResult(objectResult.Value);
            }
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
        }
    }
}