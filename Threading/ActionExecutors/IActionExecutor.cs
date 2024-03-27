using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShUtilities.Threading.ActionExecutors
{
    /// <summary>
    /// Abstraction for running actions, e.g. in a separate task, that is unit testable
    /// </summary>
    public interface IActionExecutor
    {
        void Execute(Action action, CancellationToken cancellationToken);
    }
}
