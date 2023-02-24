using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkoutHistoryApp.Models.ViewModels
{
    public class DetailsExercise
    {
        public ExerciseDto SelectedExercise { get; set; }
        public IEnumerable<WorkoutDto> WorkoutsForSelectedExercise { get; set; }
    }
}