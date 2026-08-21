using System.Net;
using System.Text.Json;
using ClientPortal.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using ClientPortal.Domain.Exceptions;

namespace ClientPortal.WebAPI.Middleware;

public class ExceptionHandlerMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException ex)
        {
            await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound, "Not Found");
        }
        catch (FeaturesOutOfScopeException ex)
        {
            await HandleExceptionAsync(context, ex, HttpStatusCode.Conflict, "Features Out Of Scope");
        }
        catch (InvalidFeatureStatusTransitionException ex)
        {
            await HandleExceptionAsync(context, ex, HttpStatusCode.Conflict, "Invalid Feature Status Transition");
        }
        catch (MinimumFeatureScopeException ex)
        {
            await HandleExceptionAsync(context, ex, HttpStatusCode.Conflict,
                "New Feature Scope Exceeds Minimum Feature Scope");
        }
        catch (ArgumentException ex)
        {
            await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest, "Argument Error");
        }
        catch (InvalidMagicLinkException ex)
        {
            await HandleExceptionAsync(context, ex, HttpStatusCode.Unauthorized, "Invalid Magic Link");
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    private static Task HandleExceptionAsync(
        HttpContext context, 
        Exception exception, 
        HttpStatusCode statusCode, 
        string title)

    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = title,
            Detail = exception.Message,
            Instance = context.Request.Path
        };

        var jsonResponse = JsonSerializer.Serialize(problemDetails);
        return context.Response.WriteAsync(jsonResponse);
    }
}