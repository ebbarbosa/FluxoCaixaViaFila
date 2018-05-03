using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Stone.FluxoCaixaViaFila.Infra.MQ
{
    public abstract class QueueHostedService : IHostedService
    {

        private Task _executingTask;
        private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();
        private readonly IServiceProvider _serviceProvider;
        private IServiceScope _scope;
        protected int CheckUpdateTime = 3;

        protected QueueHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            GetRequiredServices(_serviceProvider);

            // Store the task we're executing
            _executingTask = ExecuteAsync(_stoppingCts.Token);

            // If the task is completed then return it,
            // this will bubble cancellation and failure to the caller
            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }

            // Otherwise it's running
            return Task.CompletedTask;

        }

        protected abstract Task ExecuteAsync(CancellationToken stoppingToken);

        protected abstract void GetRequiredServices(IServiceProvider serviceProvider);

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop called without start
            if (_executingTask == null)
            {
                return;
            }

            try
            {
                // Signal cancellation to the executing method
                _stoppingCts.Cancel();
            }
            finally
            {
                // Wait until the task completes or the stop token triggers
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite,
                    cancellationToken));
                _scope.Dispose();
            }
        }

        public virtual void Dispose()
        {
            _stoppingCts.Cancel();
        }
    }
}