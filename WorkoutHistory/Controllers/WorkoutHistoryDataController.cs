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
    public class WorkoutHistoryDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all WorkoutHistorys in the system that has unique dates.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all WorkoutHistorys in the system that has unique dates.
        /// </returns>
        /// <example>
        /// GET: api/WorkoutHistoryData/ListWorkoutHistoryDates
        /// </example>
        [HttpGet]
        [ResponseType(typeof(WorkoutHistoryDto))]
        public IHttpActionResult ListWorkoutHistoryDates()
        {
            List<string> WorkoutHistoryDates = db.WorkoutHistory.Select(
                wh => wh.Date).Distinct().ToList();

            return Ok(WorkoutHistoryDates);
        }

        /// <summary>
        /// Returns all WorkoutHistorys in the system that matches the input date.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all WorkoutHistorys in the database that matches the input date.
        /// </returns>
        /// <example>
        /// GET: api/WorkoutHistoryData/ListWorkoutHistoryForDate
        /// </example>
        [HttpGet]
        [ResponseType(typeof(WorkoutHistoryDto))]
        public IHttpActionResult ListWorkoutHistoryForDate(string id)
        {
            List<WorkoutHistory> WorkoutHistory = db.WorkoutHistory.Where(
                wh => wh.Date == id).ToList();
            List<WorkoutHistoryDto> WorkoutHistoryDtos = new List<WorkoutHistoryDto>();

            WorkoutHistory.ForEach(wh => WorkoutHistoryDtos.Add(new WorkoutHistoryDto()
            {
                WorkoutHistoryID = wh.WorkoutHistoryID,
                Date = wh.Date,
                ExerciseID = wh.ExerciseID,
                ExerciseTypeID = wh.Exercise.ExerciseTypeID,
                ExerciseTypeName = wh.Exercise.ExerciseType.ExerciseTypeName,
                ExerciseName = wh.Exercise.ExerciseName,
                Description = wh.Description
            }));

            return Ok(WorkoutHistoryDtos);
        }

        /// <summary>
        /// Returns a WorkoutHistory in the system based on the input ID.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An WorkoutHistory in the system matching up to the WorkoutHistory ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the WorkoutHistory</param>
        /// <example>
        /// GET: api/WorkoutHistoryData/FindWorkoutHistory/5
        /// </example>
        [ResponseType(typeof(WorkoutHistoryDto))]
        [HttpGet]
        public IHttpActionResult FindWorkoutHistory(int id)
        {
            WorkoutHistory WorkoutHistory = db.WorkoutHistory.Find(id);
            WorkoutHistoryDto WorkoutHistoryDto = new WorkoutHistoryDto()
            {
                WorkoutHistoryID = WorkoutHistory.WorkoutHistoryID,
                Date = WorkoutHistory.Date,
                ExerciseID = WorkoutHistory.ExerciseID,
                ExerciseTypeID = WorkoutHistory.Exercise.ExerciseTypeID,
                ExerciseTypeName = WorkoutHistory.Exercise.ExerciseType.ExerciseTypeName,
                ExerciseName = WorkoutHistory.Exercise.ExerciseName,
                Description = WorkoutHistory.Description,
            };
            if (WorkoutHistory == null)
            {
                return NotFound();
            }

            return Ok(WorkoutHistoryDto);
        }

        /// <summary>
        /// Updates a particular WorkoutHistory in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the WorkoutHistory ID primary key</param>
        /// <param name="WorkoutHistory">JSON FORM DATA of an WorkoutHistory</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/WorkoutHistoryData/UpdateWorkoutHistory/5
        /// FORM DATA: WorkoutHistory JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateWorkoutHistory(int id, WorkoutHistory WorkoutHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != WorkoutHistory.WorkoutHistoryID)
            {

                return BadRequest();
            }

            db.Entry(WorkoutHistory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkoutHistoryExists(id))
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
        /// Adds an WorkoutHistory to the system
        /// </summary>
        /// <param name="WorkoutHistory">JSON FORM DATA of an WorkoutHistory</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: WorkoutHistory ID, WorkoutHistory Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/WorkoutHistoryData/AddWorkoutHistory
        /// FORM DATA: WorkoutHistory JSON Object
        /// </example>
        [ResponseType(typeof(WorkoutHistory))]
        [HttpPost]
        public IHttpActionResult AddWorkoutHistory(WorkoutHistory WorkoutHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.WorkoutHistory.Add(WorkoutHistory);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = WorkoutHistory.WorkoutHistoryID }, WorkoutHistory);
        }

        /// <summary>
        /// Deletes an WorkoutHistory from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the WorkoutHistory</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/WorkoutHistoryData/DeleteWorkoutHistory/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(WorkoutHistory))]
        [HttpPost]
        public IHttpActionResult DeleteWorkoutHistory(int id)
        {
            WorkoutHistory WorkoutHistory = db.WorkoutHistory.Find(id);
            if (WorkoutHistory == null)
            {
                return NotFound();
            }

            db.WorkoutHistory.Remove(WorkoutHistory);
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

        private bool WorkoutHistoryExists(int id)
        {
            return db.WorkoutHistory.Count(wh => wh.WorkoutHistoryID == id) > 0;
        }
    }
}