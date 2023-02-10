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
    public class ExerciseTypeDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all ExerciseTypes in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all ExerciseTypes in the database, including their associated species.
        /// </returns>
        /// <example>
        /// GET: api/ExerciseTypeData/ListExerciseTypes
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ExerciseTypeDto))]
        public IHttpActionResult ListExerciseTypes()
        {
            List<ExerciseType> ExerciseTypes = db.ExerciseType.ToList();
            List<ExerciseTypeDto> ExerciseTypeDtos = new List<ExerciseTypeDto>();

            ExerciseTypes.ForEach(et => ExerciseTypeDtos.Add(new ExerciseTypeDto(){ 
                ExerciseTypeID = et.ExerciseTypeID,
                ExerciseTypeName = et.ExerciseTypeName,
            }));

            return Ok(ExerciseTypeDtos);
        }

        /// <summary>
        /// Returns one ExerciseType in the system based on the id.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An ExerciseType in the system matching up to the ExerciseType ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the ExerciseType</param>
        /// <example>
        /// GET: api/ExerciseTypeData/FindExerciseType/5
        /// </example>
        [ResponseType(typeof(ExerciseTypeDto))]
        [HttpGet]
        public IHttpActionResult FindExerciseType(int id)
        {
            ExerciseType ExerciseType = db.ExerciseType.Find(id);
            ExerciseTypeDto ExerciseTypeDto = new ExerciseTypeDto()
            {
                ExerciseTypeID = ExerciseType.ExerciseTypeID,
                ExerciseTypeName = ExerciseType.ExerciseTypeName,
            };
            if (ExerciseType == null)
            {
                return NotFound();
            }

            return Ok(ExerciseTypeDto);
        }

        /// <summary>
        /// Updates a particular ExerciseType in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the ExerciseType ID primary key</param>
        /// <param name="ExerciseType">JSON FORM DATA of an ExerciseType</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/ExerciseTypeData/UpdateExerciseType/5
        /// FORM DATA: ExerciseType JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateExerciseType(int id, ExerciseType ExerciseType)
        { 
            if (!ModelState.IsValid)
            {      
                return BadRequest(ModelState);
            }

            if (id != ExerciseType.ExerciseTypeID)
            {
                
                return BadRequest();
            }

            db.Entry(ExerciseType).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExerciseTypeExists(id))
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
        /// Adds an ExerciseType to the system
        /// </summary>
        /// <param name="ExerciseType">JSON FORM DATA of an ExerciseType</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: ExerciseType ID, ExerciseType Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/ExerciseTypeData/AddExerciseType
        /// FORM DATA: ExerciseType JSON Object
        /// </example>
        [ResponseType(typeof(ExerciseType))]
        [HttpPost]
        public IHttpActionResult AddExerciseType(ExerciseType exerciseType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ExerciseType.Add(exerciseType);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = exerciseType.ExerciseTypeID }, exerciseType);
        }

        /// <summary>
        /// Deletes an ExerciseType from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the ExerciseType</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/ExerciseTypeData/DeleteExerciseType/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(ExerciseType))]
        [HttpPost]
        public IHttpActionResult DeleteExerciseType(int id)
        {
            ExerciseType exerciseType = db.ExerciseType.Find(id);
            if (exerciseType == null)
            {
                return NotFound();
            }

            db.ExerciseType.Remove(exerciseType);
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

        private bool ExerciseTypeExists(int id)
        {
            return db.ExerciseType.Count(et => et.ExerciseTypeID == id) > 0;
        }
    }
}