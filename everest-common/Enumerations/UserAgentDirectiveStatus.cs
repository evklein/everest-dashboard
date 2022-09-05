using System;
using System.ComponentModel;

namespace everest_common.Enumerations
{
    public enum UserAgentDirectiveStatus
    {
        [Description("Not running")]
        NotRunning,
        [Description("Currently running")]
        Running,
        [Description("Queued for run")]
        Queued,
        [Description("Dequeued")]
        Dequeued,
        Disconnected,
    }
}

