using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkoutHistoryApp.Models
{
    public class ExerciseType
    {
        [Key]
        public int ExerciseTypeID { get; set; }
        public string ExerciseTypeName { get; set; }
        //an exercise type can have many exercises
        public ICollection<Exercise> Exercises { get; set; }
    }

    public class ExerciseTypeDto
    {
        public int ExerciseTypeID { get; set; }
        public string ExerciseTypeName { get; set; }
    }

}