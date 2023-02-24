using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkoutHistoryApp.Models.ViewModels
{
    public class EditExercise
    {
        public ExerciseDto SelectedExercise { get; set; }
        public IEnumerable<ExerciseTypeDto> AllExerciseTypes { get; set; }
    }
}