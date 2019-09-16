using System;
using System.Diagnostics;

namespace ShUtilities.Diagnostics
{
    /// <summary>
    /// Tunes priority of the current process, e.g. to allow a process to consume all available CPU resources as long as no other normal-priority process needs them
    /// </summary>
    public class ProcessTuning: IDisposable
    {
        public ProcessTuning()
        {
            CurrentProcess = Process.GetCurrentProcess();
        }

        public void Dispose()
        {
            CurrentProcess?.Dispose();
            CurrentProcess = null;
        }

        /// <summary>
        /// Configures the current process with a low priority and no boost
        /// </summary>
        public static ProcessTuning MakeLowPriorityProcess()
        {
            var result = new ProcessTuning
            {
                IsPriorityBoostEnabled = false,
                PriorityClass = ProcessPriorityClass.BelowNormal
            };
            return result;
        }

        /// <summary>
        /// Configures the current process with a high priority
        /// </summary>
        public static ProcessTuning MakeHighPriorityProcess()
        {
            var result = new ProcessTuning
            {
                IsPriorityBoostEnabled = true,
                IsHigherPriorityAllowed = true,
                PriorityClass = ProcessPriorityClass.AboveNormal
            };
            return result;
        }

        public bool IsPriorityBoostEnabled
        {
            get => CurrentProcess.PriorityBoostEnabled;
            set => CurrentProcess.PriorityBoostEnabled = value;
        }

        public ProcessPriorityClass PriorityClass
        {
            get => CurrentProcess.PriorityClass;
            set
            {
                // Don't want to risk an uncaught exception just because someone misconfigured the enum name
                if (IsValidPriorityClass(value))
                {
                    CurrentProcess.PriorityClass = value;
                }
            }
        }

        public bool IsHigherPriorityAllowed { get; set; }

        /// <summary>
        /// The process priority class as a string, so we can map it from the configuration.
        /// </summary>
        public string PriorityClassString
        {
            get => PriorityClass.ToString();
            set
            {
                // Don't want to risk an uncaught exception just because someone misconfigured the enum name
                if (Enum.TryParse(value, out ProcessPriorityClass priorityClass))
                {
                    PriorityClass = priorityClass;
                }
            }
        }

        public Process CurrentProcess { get; private set; }

        /// <summary>
        /// Because the default use-case for this class is to configure low priority processes, 
        /// don't make it too easy to (accidentally) give the process a higher priority.
        /// </summary>
        private bool IsValidPriorityClass(ProcessPriorityClass enumValue)
        {
            bool result = IsHigherPriorityAllowed;
            result |= enumValue == ProcessPriorityClass.Normal;
            result |= enumValue == ProcessPriorityClass.BelowNormal;
            result |= enumValue == ProcessPriorityClass.Idle;
            return result;
        }
    }
}
