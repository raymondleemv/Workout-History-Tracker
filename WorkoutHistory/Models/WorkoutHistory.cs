using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkoutHistoryApp.Models
{
    public class WorkoutHistory
    {

        [Key]
        public int WorkoutHistoryID { get; set; }

        public string Date { get; set; }
        [ForeignKey("Exercise")]
        public int ExerciseID { get; set; }
        public virtual Exercise Exercise { get; set; }

        public string Description { get; set; }
    }

    public class WorkoutHistoryDto
    {
        public int WorkoutHistoryID { get; set; }
        public string Date { get; set; }
        public int ExerciseID { get; set; }
        public string ExerciseName { get; set; }
        public int ExerciseTypeID { get; set; }
        public string ExerciseTypeName { get; set; }        
        public string Description { get; set; }

    }
}