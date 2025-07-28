using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using System.ComponentModel;

namespace trip_planner_agent
{
    public class Program
    {
        public static async Task Main()
        {
            await TripPlannerAgent.PlanTrip();
        }
    }
}