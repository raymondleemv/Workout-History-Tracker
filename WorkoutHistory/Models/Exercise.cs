using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkoutHistoryApp.Models
{
    public class Exercise
    {

        [Key]
        public int ExerciseID { get; set; }
        public string ExerciseName { get; set; }
        [ForeignKey("ExerciseType")]
        public int ExerciseTypeID { get; set; }
        public virtual ExerciseType ExerciseType { get; set; }
    }


    public class ExerciseDto
    {
        public int ExerciseID { get; set; }
        public string ExerciseName { get; set; }
        public int ExerciseTypeID { get; set; }
        public string ExerciseTypeName { get; set; }
    }
}