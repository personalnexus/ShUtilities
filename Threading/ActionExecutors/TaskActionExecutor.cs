using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShUtilities.Threading.ActionExecutors
{
    /// <summary>
    /// Implementation of <see cref="IActionExecutor"/> that starts a new task for each execution
    /// </summary>
    public class TaskActionExecutor : IActionExecutor
    {
        public static IActionExecutor Default { get; } = new TaskActionExecutor();

        public void Execute(Action action, CancellationToken cancellationToken) => Task.Run(action, cancellationToken);
    }
}
