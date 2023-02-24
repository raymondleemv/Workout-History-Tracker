using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkoutHistoryApp.Models
{
    public class Workout
    {
        [Key]
        public int WorkoutID { get; set; }
        public string WorkoutDate { get; set; }
    }


    public class WorkoutDto
    {
        public int WorkoutID { get; set; }
        public string WorkoutDate { get; set; }
    }
}