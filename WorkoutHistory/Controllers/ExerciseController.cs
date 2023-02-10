using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using WorkoutHistoryApp.Models;
using System.Web.Script.Serialization;
using WorkoutHistoryApp.Models.ViewModels;

namespace WorkoutHistoryApp.Controllers
{
    public class ExerciseController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ExerciseController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44307/api/");
        }

        // GET: Exercise/List
        public ActionResult List()
        {
            //objective: communicate with our Exercise data api to retrieve a list of Exercises
            //curl https://localhost:44307/api/Exercisedata/ListExercises


            string url = "ExerciseData/ListExercises";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<ExerciseDto> Exercises = response.Content.ReadAsAsync<IEnumerable<ExerciseDto>>().Result;
            //Debug.WriteLine("Number of Exercises received : ");
            //Debug.WriteLine(Exercises.Count());


            return View(Exercises);
        }

        // GET: Exercise/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our Exercise data api to retrieve one Exercise
            //curl https://localhost:44307/api/Exercisedata/FindExercise/{id}

            string url = "ExerciseData/FindExercise/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            ExerciseDto SelectedExercise = response.Content.ReadAsAsync<ExerciseDto>().Result;
            //Debug.WriteLine("Exercise received : ");
            //Debug.WriteLine(SelectedExercise.ExerciseName);

            return View(SelectedExercise);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Exercise/New
        public ActionResult New()
        {
            string url = "ExerciseTypeData/ListExerciseTypes";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<ExerciseTypeDto> exerciseTypes = response.Content.ReadAsAsync<IEnumerable<ExerciseTypeDto>>().Result;
            
            //Debug.WriteLine("Number of exerciseTypes received : ");
            //Debug.WriteLine(exerciseTypes.Count());
            
            return View(exerciseTypes);
        }

        // POST: Exercise/Create
        [HttpPost]
        public ActionResult Create(Exercise Exercise)
        {
            //Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(Exercise.ExerciseName);
            //objective: add a new Exercise into our system using the API
            //curl -H "Content-Type:application/json" -d @Exercise.json https://localhost:44307/api/Exercisedata/addExercise 
            string url = "ExerciseData/AddExercise";


            string jsonpayload = jss.Serialize(Exercise);
            //Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }


        }

        // GET: Exercise/Edit/5
        public ActionResult Edit(int id)
        {
            DetailsExercise ViewModel = new DetailsExercise();
            string url = "ExerciseData/FindExercise/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ExerciseDto selectedExercise = response.Content.ReadAsAsync<ExerciseDto>().Result;
            ViewModel.SelectedExercise = selectedExercise;

            url = "ExerciseTypeData/ListExerciseTypes";
            response = client.GetAsync(url).Result;
            IEnumerable<ExerciseTypeDto> AllExerciseTypes = response.Content.ReadAsAsync<IEnumerable<ExerciseTypeDto>>().Result;
            ViewModel.AllExerciseTypes = AllExerciseTypes;

            return View(ViewModel);
        }

        // POST: Exercise/Update/5
        [HttpPost]
        public ActionResult Update(int id, Exercise Exercise)
        {

            string url = "ExerciseData/UpdateExercise/" + id;
            string jsonpayload = jss.Serialize(Exercise);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            //Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Exercise/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "ExerciseData/FindExercise/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ExerciseDto selectedExercise = response.Content.ReadAsAsync<ExerciseDto>().Result;
            return View(selectedExercise);
        }

        // POST: Exercise/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "ExerciseData/DeleteExercise/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
