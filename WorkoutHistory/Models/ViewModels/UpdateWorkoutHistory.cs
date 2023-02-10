using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkoutHistoryApp.Models.ViewModels
{
    public class UpdateWorkoutHistory
    {
        //the species itself that we want to display
        public WorkoutHistoryDto SelectedWorkoutHistory { get; set; }

        //all of the related animals to that particular species
        public IEnumerable<ExerciseDto> AllExercises { get; set; }
    }
}