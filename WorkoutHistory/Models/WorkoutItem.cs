using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkoutHistoryApp.Models
{
    public class WorkoutItem
    {

        [Key]
        public int WorkoutItemID { get; set; }

        [ForeignKey("Workout")]
        public int WorkoutID { get; set; }
        public virtual Workout Workout { get; set; }

        [ForeignKey("Exercise")]
        public int ExerciseID { get; set; }
        public virtual Exercise Exercise { get; set; }
        public int Weight { get; set; }
        public int Reps { get; set; }
    }

    public class WorkoutItemDto
    {
        public int WorkoutItemID { get; set; }
        public int WorkoutID { get; set; }
        public string WorkoutDate { get; set; }
        public int ExerciseID { get; set; }
        public string ExerciseName { get; set; }
        public int ExerciseTypeID { get; set; }
        public string ExerciseTypeName { get; set; }        
        public int Weight { get; set; }
        public int Reps { get; set; }

    }
}