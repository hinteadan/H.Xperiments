using H.Necessaire;
using H.Necessaire.Runtime.CLI.Commands;
using Polly;
using System;
using System.Threading.Tasks;

namespace H.Qubiz.Xperiments.CLI.Commands
{
    internal class PollyCommand : CommandBase
    {
        const int maxRetries = 5;
        public override async Task<OperationResult> Run()
        {
            await Task.CompletedTask;

            Log($"Running Polly Command");
            using (new TimeMeasurement(x => Log($"DONE Running Polly Command in {x}")))
            {

                ResiliencePipeline pipeline
                    = new ResiliencePipelineBuilder()
                    .AddRetry(new Polly.Retry.RetryStrategyOptions
                    {
                        BackoffType = DelayBackoffType.Constant,
                        Delay = TimeSpan.FromSeconds(1),
                        MaxRetryAttempts = maxRetries,
                        OnRetry = async x => {
                            await Task.CompletedTask;
                            Log($"Retrying, attempt {x.AttemptNumber}/{maxRetries}...");
                        },
                        ShouldHandle = async x => {
                            await Task.CompletedTask;
                            if (x.AttemptNumber == 2)
                                return false;
                            if (x.Outcome.Result is null)
                                return true;
                            return true;
                        },
                    })
                    .Build()
                    ;

                await pipeline.ExecuteAsync(
                    async ctx => { await Task.CompletedTask; },
                    ResilienceContextPool.Shared.Get(continueOnCapturedContext: false)
                );

            }

            return OperationResult.Win();
        }
    }
}
