using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAdvanced
{
    /// <summary>
    /// Hosted service that executes the requested operation
    /// using a hosted application lifecycle.
    /// </summary>
    /// <typeparam name="TOperation">Type that implements the requested operation.</typeparam>
    internal class OperationHostedService<TOperation> : BackgroundService
        where TOperation : IOperation
    {
        private readonly TOperation operation;
        private readonly IHostApplicationLifetime hostLifetime;
        private readonly ILogger<OperationHostedService<TOperation>> logger;

        public OperationHostedService(
            TOperation operation,
            IHostApplicationLifetime hostLifetime,
            ILogger<OperationHostedService<TOperation>> logger)
        {
            this.operation = operation;
            this.hostLifetime = hostLifetime;
            this.logger = logger;
        }

        /// <summary>
        /// This is called by the host when it is run.
        /// </summary>
        /// <param name="cancellationToken">Triggered by Ctrl+C.</param>
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            // Avoid blocking at startup
            // See https://blog.stephencleary.com/2020/05/backgroundservice-gotcha-startup.html
            await Task.Yield();

            bool cancelled = false;
            try
            {
                // Execute the requested operation, passing in the cancellation token
                // so that it can be passed down the call stack
                await operation.Execute(cancellationToken);
                // Ensure that we trigger our cancellation logic when the token is cancelled
                cancellationToken.ThrowIfCancellationRequested();
                logger.LogInformation("Operation completed");
            }
            catch (OperationCanceledException)
            {
                cancelled = true;
                logger.LogInformation("Operation cancelled");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception executing operation");
                // Non-zero exit code is helpful to let scripts know about failure
                Environment.ExitCode = -1;
            }
            if (!cancelled)
            {
                // Must explicitly stop the application to terminate the process
                hostLifetime.StopApplication();
            }
        }
    }
}