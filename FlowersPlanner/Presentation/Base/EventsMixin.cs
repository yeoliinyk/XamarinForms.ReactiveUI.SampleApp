using System;
using FlowersPlanner.Presentation.Controls;

namespace FlowersPlanner.Presentation.Base
{
    public static class EventsMixin
    {
        public static CustomSelectorEvents Events(this CustomSelector This)
        {
            return new CustomSelectorEvents(This);
        }
    }
}
