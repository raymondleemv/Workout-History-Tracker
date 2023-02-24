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
    public class WorkoutDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Workouts in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Workouts in the database
        /// </returns>
        /// <example>
        /// GET: api/WorkoutData/ListWorkouts
        /// </example>
        [HttpGet]
        [ResponseType(typeof(WorkoutDto))]
        public IHttpActionResult ListWorkouts()
        {
            List<Workout> Workouts = db.Workout.OrderBy(w => w.WorkoutDate).ToList();
            List<WorkoutDto> WorkoutDtos = new List<WorkoutDto>();

            Workouts.ForEach(e => WorkoutDtos.Add(new WorkoutDto()
            {
                WorkoutID = e.WorkoutID,
                WorkoutDate = e.WorkoutDate
            }));

            return Ok(WorkoutDtos);
        }

        /// <summary>
        /// Returns one Workout in the system based on the id.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Workout in the system matching up to the Workout ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Workout</param>
        /// <example>
        /// GET: api/WorkoutData/FindWorkout/5
        /// </example>
        [ResponseType(typeof(WorkoutDto))]
        [HttpGet]
        public IHttpActionResult FindWorkout(int id)
        {
            Workout Workout = db.Workout.Find(id);
            WorkoutDto WorkoutDto = new WorkoutDto()
            {
                WorkoutID = Workout.WorkoutID,
                WorkoutDate = Workout.WorkoutDate
            };
            if (Workout == null)
            {
                return NotFound();
            }

            return Ok(WorkoutDto);
        }

        /// <summary>
        /// Updates a particular Workout in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Workout ID primary key</param>
        /// <param name="Workout">JSON FORM DATA of an Workout</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/WorkoutData/UpdateWorkout/5
        /// FORM DATA: Workout JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateWorkout(int id, Workout Workout)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Workout.WorkoutID)
            {

                return BadRequest();
            }

            db.Entry(Workout).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkoutExists(id))
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
        /// Adds an Workout to the system
        /// </summary>
        /// <param name="Workout">JSON FORM DATA of an Workout</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Workout ID, Workout Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/WorkoutData/AddWorkout
        /// FORM DATA: Workout JSON Object
        /// </example>
        [ResponseType(typeof(Workout))]
        [HttpPost]
        public IHttpActionResult AddWorkout(Workout Workout)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Workout.Add(Workout);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Workout.WorkoutID }, Workout);
        }

        /// <summary>
        /// Deletes an Workout from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Workout</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/WorkoutData/DeleteWorkout/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Workout))]
        [HttpPost]
        public IHttpActionResult DeleteWorkout(int id)
        {
            Workout Workout = db.Workout.Find(id);
            if (Workout == null)
            {
                return NotFound();
            }

            db.Workout.Remove(Workout);
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

        private bool WorkoutExists(int id)
        {
            return db.Workout.Count(e => e.WorkoutID == id) > 0;
        }
    }
}