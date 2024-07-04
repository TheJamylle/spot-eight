using System.Diagnostics.CodeAnalysis;

namespace SpotEight.Core.Application.Common.Exceptions;

[ExcludeFromCodeCoverage]
public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException() : base() { }
}