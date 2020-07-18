
using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TheRiceMill.Application.Exceptions;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Presentation.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException)
            {
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                context.Result = new JsonResult(new ResponseBadRequestViewModel
                {
                    Errors = ((ValidationException) context.Exception).Failures
                        .SelectMany(p => p.Value.Select(x => p.Key + " " + x).ToList()).ToList(),
                    Message = "Invalid Object"
                });
                return;
            }
            var code = HttpStatusCode.InternalServerError;

            if (context.Exception is AlreadyExistsException || context.Exception is CannotDeleteException)
            {
                code = HttpStatusCode.BadRequest;
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)code;
                context.Result = new JsonResult(new CustomResponseExceptionViewModel()
                {
                    Message = context.Exception.Message
                });
                return;
            }
            

            if (context.Exception is NotFoundException)
            {
                code = HttpStatusCode.NotFound;
            }

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int) code;
            context.Result = new JsonResult(new ResponseExceptionViewModel
            {
                Exception = context.Exception.Message,
                StackTrace = context.Exception.StackTrace,
                Message = "Internal Server Error has occured"
            });
        }
    }
}