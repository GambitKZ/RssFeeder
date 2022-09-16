using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RssFeeder.Application.Common.Exceptions;

namespace RssFeeder.Web.AzureFunction.Handlers;

public class ExceptionHandler
{
    private readonly IDictionary<Type, Action<Exception, ContentResult>> _exceptionHandlers;

    public ExceptionHandler()
    {
        // Register known exception types and handlers.
        _exceptionHandlers = new Dictionary<Type, Action<Exception, ContentResult>>
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException }
            };
    }

    public ContentResult HandleException(Exception exception)
    {
        Type type = exception.GetType();
        var contextResult = new ContentResult
        {
            ContentType = "application/JSON"
        };

        if (_exceptionHandlers.ContainsKey(type))
        {
            _exceptionHandlers[type].Invoke(exception, contextResult);
            return contextResult;
        }

        contextResult.Content = "Request has failed to process";
        contextResult.StatusCode = 500;

        return contextResult;
    }

    private void HandleValidationException(Exception exception, ContentResult contentResult)
    {
        var ex = (ValidationException)exception;

        var errors = JsonConvert.SerializeObject(ex.Errors);

        contentResult.Content = errors;

        contentResult.StatusCode = 400;
    }

    private void HandleNotFoundException(Exception exception, ContentResult contentResult)
    {
        throw new NotImplementedException();
    }
}