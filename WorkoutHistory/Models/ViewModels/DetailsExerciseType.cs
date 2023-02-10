using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkoutHistoryApp.Models.ViewModels
{
    public class DetailsExerciseType
    {
        public ExerciseTypeDto SelectedExerciseType { get; set; }
        public IEnumerable<ExerciseDto> AllExercises { get; set; }
    }
}