namespace WebCodeCli.Domain.Domain.Exceptions;

/// <summary>
/// 存储空间配额超出异常
/// </summary>
public class QuotaExceededException : Exception
{
    public QuotaExceededException() : base("存储空间不足")
    {
    }

    public QuotaExceededException(string message) : base(message)
    {
    }

    public QuotaExceededException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
