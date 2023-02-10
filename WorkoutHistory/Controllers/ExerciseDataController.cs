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
    public class ExerciseDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Exercises in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Exercises in the database
        /// </returns>
        /// <example>
        /// GET: api/ExerciseData/ListExercises
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ExerciseDto))]
        public IHttpActionResult ListExercises()
        {
            List<Exercise> Exercises = db.Exercise.ToList();
            List<ExerciseDto> ExerciseDtos = new List<ExerciseDto>();

            Exercises.ForEach(e => ExerciseDtos.Add(new ExerciseDto()
            {
                ExerciseID = e.ExerciseID,
                ExerciseName = e.ExerciseName,
                ExerciseTypeID = e.ExerciseTypeID,
                ExerciseTypeName = e.ExerciseType.ExerciseTypeName
            }));

            return Ok(ExerciseDtos);
        }

        /// <summary>
        /// Returns all Exercises in the system associated with a particular ExerciseType.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Exercises in the database under a particular ExerciseType
        /// </returns>
        /// <param name="id">ExerciseType Primary Key</param>
        /// <example>
        /// GET: api/ExerciseData/ListExercisesForExerciseType/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ExerciseDto))]
        public IHttpActionResult ListExercisesForExerciseType(int id)
        {
            List<Exercise> Exercises = db.Exercise.Where(
                e=>e.ExerciseTypeID == id).ToList();
            List<ExerciseDto> ExerciseDtos = new List<ExerciseDto>();

            Exercises.ForEach(e => ExerciseDtos.Add(new ExerciseDto()
            {
                ExerciseID = e.ExerciseID,
                ExerciseName = e.ExerciseName,
                ExerciseTypeID = e.ExerciseTypeID,
                ExerciseTypeName = e.ExerciseType.ExerciseTypeName
            }));

            return Ok(ExerciseDtos);
        }

        /// <summary>
        /// Returns one Exercise in the system based on the id.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Exercise in the system matching up to the Exercise ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Exercise</param>
        /// <example>
        /// GET: api/ExerciseData/FindExercise/5
        /// </example>
        [ResponseType(typeof(ExerciseDto))]
        [HttpGet]
        public IHttpActionResult FindExercise(int id)
        {
            Exercise Exercise = db.Exercise.Find(id);
            ExerciseDto ExerciseDto = new ExerciseDto()
            {
                ExerciseID = Exercise.ExerciseID,
                ExerciseName = Exercise.ExerciseName,
                ExerciseTypeID = Exercise.ExerciseTypeID,
                ExerciseTypeName = Exercise.ExerciseType.ExerciseTypeName
            };
            if (Exercise == null)
            {
                return NotFound();
            }

            return Ok(ExerciseDto);
        }

        /// <summary>
        /// Updates a particular Exercise in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Exercise ID primary key</param>
        /// <param name="Exercise">JSON FORM DATA of an Exercise</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/ExerciseData/UpdateExercise/5
        /// FORM DATA: Exercise JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateExercise(int id, Exercise Exercise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Exercise.ExerciseID)
            {

                return BadRequest();
            }

            db.Entry(Exercise).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExerciseExists(id))
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
        /// Adds an Exercise to the system
        /// </summary>
        /// <param name="Exercise">JSON FORM DATA of an Exercise</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Exercise ID, Exercise Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/ExerciseData/AddExercise
        /// FORM DATA: Exercise JSON Object
        /// </example>
        [ResponseType(typeof(Exercise))]
        [HttpPost]
        public IHttpActionResult AddExercise(Exercise Exercise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Exercise.Add(Exercise);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Exercise.ExerciseID }, Exercise);
        }

        /// <summary>
        /// Deletes an Exercise from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Exercise</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/ExerciseData/DeleteExercise/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Exercise))]
        [HttpPost]
        public IHttpActionResult DeleteExercise(int id)
        {
            Exercise Exercise = db.Exercise.Find(id);
            if (Exercise == null)
            {
                return NotFound();
            }

            db.Exercise.Remove(Exercise);
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

        private bool ExerciseExists(int id)
        {
            return db.Exercise.Count(e => e.ExerciseID == id) > 0;
        }
    }
}