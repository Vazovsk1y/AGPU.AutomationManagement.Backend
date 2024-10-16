using AGPU.AutomationManagement.Application.Common;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace AGPU.AutomationManagement.Application.Extensions;

public static class ResultEx
{
    public static T Match<T, TValue>(this Result<TValue> result, Func<TValue, T> onSuccess, Func<IEnumerable<Error>, T> onFailure)
    {
        return result.IsSuccess ? onSuccess.Invoke(result.Value) : onFailure.Invoke(result.Errors);
    }
    
    public static T Match<T>(this Result result, Func<T> onSuccess, Func<IEnumerable<Error>, T> onFailure)
    {
        return result.IsSuccess ? onSuccess.Invoke() : onFailure.Invoke(result.Errors);
    }
    
    public static async Task<T> MatchAsync<T>(this Result result, Func<Task<T>> onSuccess, Func<IEnumerable<Error>, T> onFailure)
    {
        return result.IsSuccess ? await onSuccess.Invoke() : onFailure.Invoke(result.Errors);
    }

    public static Result OnSuccess(this Result result, Func<Result> newResult)
    {
        return result.IsSuccess ? newResult.Invoke() : result;
    }
    
    public static Result<T> ToFailure<T>(this Result result)
    {
        if (result.IsSuccess)
        {
            throw new ArgumentException("Success result can't be converted to failure.", nameof(result));
        }
        
        return Result.Failure<T>(result.Errors);
    }
    
    public static Result<T> ToFailure<T>(this ValidationResult result)
    {
        if (result.IsValid)
        {
            throw new ArgumentException("Success result can't be converted to failure.", nameof(result));
        }
        
        return Result.Failure<T>(result.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)));
    }

    public static Result<T> ToFailure<T>(this IdentityResult result)
    {
        if (result.Succeeded)
        {
            throw new ArgumentException("Success result can't be converted to failure.", nameof(result));
        }
        
        return Result.Failure<T>(result.Errors.Select(e => new Error(e.Code, e.Description)));
    }
    
    public static Result ToFailure(this ValidationResult result)
    {
        if (result.IsValid)
        {
            throw new ArgumentException("Success result can't be converted to failure.", nameof(result));
        }
        
        return Result.Failure(result.Errors.Select(e => new Error(e.ErrorCode, e.ErrorMessage)));
    }
}