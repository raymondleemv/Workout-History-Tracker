using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkoutHistoryApp.Models.ViewModels
{
    public class DetailsWorkout
    {
        public WorkoutDto SelectedWorkout { get; set; }
        public IEnumerable<WorkoutItemDto> WorkoutItemsForSelectedWorkout { get; set; }
    }
}