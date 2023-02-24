using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkoutHistoryApp.Models.ViewModels
{
    public class ProgressExercise
    {
        public IEnumerable<string> WorkoutDates { get; set; }
        public IEnumerable<int> WorkoutVolumes { get; set; }
    }
}