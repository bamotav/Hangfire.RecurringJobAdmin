using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.RecurringJobAdmin
{
    public enum JobState
    {
        DeletedState,
        FailedState,
        EnqueuedState,
    }

}
