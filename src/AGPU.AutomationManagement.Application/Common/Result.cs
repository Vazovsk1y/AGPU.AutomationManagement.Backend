﻿namespace AGPU.AutomationManagement.Application.Common;

public class Result
{
    public bool IsFailure => !IsSuccess;

    public bool IsSuccess { get; }

    public IReadOnlyCollection<Error> Errors { get; }

    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None || !isSuccess && error == Error.None)
        {
            throw new InvalidOperationException("Unable create result object.");
        }

        IsSuccess = isSuccess;
        Errors = isSuccess ? [ ] : [ error ];
    }

    protected Result(bool isSuccess, IEnumerable<Error> errors)
    {
        var errorsCollection = errors as List<Error> ?? errors.ToList();
        if (isSuccess && errorsCollection.Count != 0
            || !isSuccess && errorsCollection.Distinct().Count() != errorsCollection.Count
            || !isSuccess && errorsCollection.Count == 0
            || !isSuccess && errorsCollection.Contains(Error.None))
        {
            throw new InvalidOperationException("Unable create result object.");
        }

        IsSuccess = isSuccess;
        Errors = isSuccess ? [ ] : errorsCollection;
    }

    public static Result Success() => new(true, Error.None);

    public static Result<T> Success<T>(T value) => new(value, true, Error.None);

    public static Result Failure(Error error) => new(false, error);

    public static Result<T> Failure<T>(Error error) => new(default, false, error);

    public static Result Failure(IEnumerable<Error> errors) => new(false, errors);

    public static Result<T> Failure<T>(IEnumerable<Error> errors) => new(default, false, errors);

    public static Result FailureIf(bool condition, params Error[] errors)
    {
        return condition ? Failure(errors) : Success();
    }

    public static Result SuccessIf(bool condition, params Error[] errors)
    {
        return condition ? Success() : Failure(errors);
    }
    
    public static Result Combine(params Result[] results)
    {
        return results.Any(e => e.IsFailure) ? Failure(results.Where(e => e.IsFailure).SelectMany(e => e.Errors)) : Success();
    }
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error)
        => _value = value;

    protected internal Result(TValue? value, bool isSuccess, IEnumerable<Error> errors) : base(isSuccess, errors)
        => _value = value;

    public TValue Value => IsFailure ?
        throw new InvalidOperationException("The value of failed result can't be accessed.")
        :
        _value!;

    public static implicit operator Result<TValue>(TValue value) => new(value, true, Error.None);
}