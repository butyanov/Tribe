using Microsoft.EntityFrameworkCore;
using Tribe.Data;

namespace Tribe.Api.Middleware;

public class TransactionsMiddleware(ILogger<TransactionsMiddleware> logger, RequestDelegate next)
{
    public async Task Invoke(HttpContext context, DataContext dbContext)
    {
        var execStrategy = dbContext.Database.CreateExecutionStrategy();
        await execStrategy.ExecuteAsync(
            async () =>
            {
                await using var transaction = await dbContext.Database.BeginTransactionAsync();
        
                try
                {
                    logger.LogInformation($"Begin transaction {transaction.TransactionId}");

                    await next(context);

                    await transaction.CommitAsync();

                    logger.LogInformation($"Committed transaction {transaction.TransactionId}");
                    
                }
                catch (Exception e)
                {
                    logger.LogInformation($"Rollback transaction executed {transaction.TransactionId}");

                    await transaction.RollbackAsync();

                    logger.LogError(e.Message, e.StackTrace);

                    throw;
                }
            });
    }
}