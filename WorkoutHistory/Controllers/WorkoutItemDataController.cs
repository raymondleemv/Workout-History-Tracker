using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WorkoutHistoryApp.Models;
using System.Diagnostics;

namespace WorkoutHistoryApp.Controllers
{
    public class WorkoutItemDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns the workout volume for exercise specified by id in workout specified by workoutID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: the workout volume for exercise specified by id in workout specified by workoutID
        /// </returns>
        /// <example>
        /// GET: api/WorkoutItemData/ListWorkoutVolumeForExerciseInWorkout
        /// </example>
        [HttpGet]
        [ResponseType(typeof(WorkoutItemDto))]
        public IHttpActionResult ListWorkoutVolumeForExerciseInWorkout(int id, int workoutID)
        {
            List<WorkoutItem> Workouts = db.WorkoutItem.Where(w => w.WorkoutID == workoutID && w.ExerciseID == id).ToList();
            int WorkoutVolume = 0;
            Workouts.ForEach(w => WorkoutVolume += w.Weight * w.Reps);

            return Ok(WorkoutVolume);
        }

        /// <summary>
        /// Returns all Workouts that cotains the exercise specified by id in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Workouts that cotains the exercise specified by id  in the database
        /// </returns>
        /// <example>
        /// GET: api/WorkoutItemData/ListWorkoutsForExercise
        /// </example>
        [HttpGet]
        [ResponseType(typeof(WorkoutDto))]
        public IHttpActionResult ListWorkoutsForExercise(int id)
        {
            List<int> WorkoutIDs = db.WorkoutItem.Where(w => w.ExerciseID == id).Select(w => w.WorkoutID).Distinct().ToList();
            List<Workout> Workouts = db.Workout.Where(w => WorkoutIDs.Contains(w.WorkoutID)).OrderBy(w => w.WorkoutDate).ToList();
            List<WorkoutDto> WorkoutDtos = new List<WorkoutDto>();
            Workouts.ForEach(e => WorkoutDtos.Add(new WorkoutDto()
            {
                WorkoutID = e.WorkoutID,
                WorkoutDate = e.WorkoutDate
            }));
            return Ok(WorkoutDtos);
        }

        /// <summary>
        /// Returns all WorkoutItems in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all WorkoutItems in the database
        /// </returns>
        /// <example>
        /// GET: api/WorkoutItemData/ListWorkoutItemsForWorkout
        /// </example>
        [HttpGet]
        [ResponseType(typeof(WorkoutDto))]
        public IHttpActionResult ListWorkoutItemsForWorkout(int id)
        {
            List<WorkoutItem> WorkoutItems = db.WorkoutItem.Where(w => w.WorkoutID == id).ToList();
            List<WorkoutItemDto> WorkoutItemDtos = new List<WorkoutItemDto>();

            WorkoutItems.ForEach(w => WorkoutItemDtos.Add(new WorkoutItemDto()
            {
                WorkoutItemID = w.WorkoutItemID,
                WorkoutID = w.WorkoutID,
                WorkoutDate = w.Workout.WorkoutDate,
                ExerciseID = w.ExerciseID,
                ExerciseTypeID = w.Exercise.ExerciseTypeID,
                ExerciseTypeName = w.Exercise.ExerciseType.ExerciseTypeName,
                ExerciseName = w.Exercise.ExerciseName,
                Weight = w.Weight,
                Reps = w.Reps
            }));

            return Ok(WorkoutItemDtos);
        }

        /// <summary>
        /// Returns a WorkoutItem in the system based on the input ID.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An WorkoutItem in the system matching up to the WorkoutItem ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the WorkoutItem</param>
        /// <example>
        /// GET: api/WorkoutItemData/FindWorkoutItem/5
        /// </example>
        [ResponseType(typeof(WorkoutItemDto))]
        [HttpGet]
        public IHttpActionResult FindWorkoutItem(int id)
        {
            WorkoutItem WorkoutItem = db.WorkoutItem.Find(id);
            WorkoutItemDto WorkoutItemDto = new WorkoutItemDto()
            {
                WorkoutItemID = WorkoutItem.WorkoutItemID,
                WorkoutID = WorkoutItem.WorkoutID,
                WorkoutDate = WorkoutItem.Workout.WorkoutDate,
                ExerciseID = WorkoutItem.ExerciseID,
                ExerciseTypeID = WorkoutItem.Exercise.ExerciseTypeID,
                ExerciseTypeName = WorkoutItem.Exercise.ExerciseType.ExerciseTypeName,
                ExerciseName = WorkoutItem.Exercise.ExerciseName,
                //Description = WorkoutItem.Description,
                Weight = WorkoutItem.Weight,
                Reps = WorkoutItem.Reps
            };
            if (WorkoutItem == null)
            {
                return NotFound();
            }

            return Ok(WorkoutItemDto);
        }

        /// <summary>
        /// Updates a particular WorkoutItem in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the WorkoutItem ID primary key</param>
        /// <param name="WorkoutItem">JSON FORM DATA of an WorkoutItem</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/WorkoutItemData/UpdateWorkoutItem/5
        /// FORM DATA: WorkoutItem JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateWorkoutItem(int id, WorkoutItem WorkoutItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != WorkoutItem.WorkoutItemID)
            {

                return BadRequest();
            }

            db.Entry(WorkoutItem).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkoutItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds an WorkoutItem to the system
        /// </summary>
        /// <param name="WorkoutItem">JSON FORM DATA of an WorkoutItem</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: WorkoutItem ID, WorkoutItem Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/WorkoutItemData/AddWorkoutItem
        /// FORM DATA: WorkoutItem JSON Object
        /// </example>
        [ResponseType(typeof(WorkoutItem))]
        [HttpPost]
        public IHttpActionResult AddWorkoutItem(WorkoutItem WorkoutItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.WorkoutItem.Add(WorkoutItem);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = WorkoutItem.WorkoutItemID }, WorkoutItem);
        }

        /// <summary>
        /// Deletes an WorkoutItem from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the WorkoutItem</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/WorkoutItemData/DeleteWorkoutItem/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(WorkoutItem))]
        [HttpPost]
        public IHttpActionResult DeleteWorkoutItem(int id)
        {
            WorkoutItem WorkoutItem = db.WorkoutItem.Find(id);
            if (WorkoutItem == null)
            {
                return NotFound();
            }

            db.WorkoutItem.Remove(WorkoutItem);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WorkoutItemExists(int id)
        {
            return db.WorkoutItem.Count(w => w.WorkoutItemID == id) > 0;
        }
    }
}