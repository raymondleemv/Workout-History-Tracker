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
using Microsoft.Ajax.Utilities;

namespace WorkoutHistoryApp.Controllers
{
    public class WorkoutController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static WorkoutController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44307/api/");
        }

        // GET: Workout/List
        public ActionResult List()
        {
            //objective: communicate with our Workout data api to retrieve a list of Workouts
            //curl https://localhost:44307/api/Workoutdata/ListWorkouts


            string url = "WorkoutData/ListWorkouts";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<WorkoutDto> Workouts = response.Content.ReadAsAsync<IEnumerable<WorkoutDto>>().Result;
            //Debug.WriteLine("Number of Workouts received : ");
            //Debug.WriteLine(Workouts.Count());


            return View(Workouts);
        }

        // GET: Workout/Details/5
        public ActionResult Details(int id)
        {
            DetailsWorkout ViewModel= new DetailsWorkout();
            //objective: communicate with our Workout data api to retrieve one Workout
            //curl https://localhost:44307/api/Workoutdata/FindWorkout/{id}

            string url = "WorkoutData/FindWorkout/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            WorkoutDto selectedWorkout = response.Content.ReadAsAsync<WorkoutDto>().Result;
            ViewModel.SelectedWorkout = selectedWorkout;
            url = "WorkoutItemData/ListWorkoutItemsForWorkout/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<WorkoutItemDto> workoutItemsForSelectedWorkout = response.Content.ReadAsAsync<IEnumerable<WorkoutItemDto>>().Result;

            ViewModel.WorkoutItemsForSelectedWorkout = workoutItemsForSelectedWorkout;

            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Workout/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Workout/Create
        [HttpPost]
        public ActionResult Create(Workout Workout)
        {
            //objective: add a new Workout into our system using the API
            //curl -H "Content-Type:application/json" -d @Workout.json https://localhost:44307/api/Workoutdata/addWorkout 
            string url = "WorkoutData/AddWorkout";
            string jsonpayload = jss.Serialize(Workout);
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

        // GET: Workout/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "WorkoutData/FindWorkout/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            WorkoutDto selectedWorkout = response.Content.ReadAsAsync<WorkoutDto>().Result;
            return View(selectedWorkout);
        }

        // POST: Workout/Update/5
        [HttpPost]
        public ActionResult Update(int id, Workout Workout)
        {

            string url = "WorkoutData/UpdateWorkout/" + id;
            string jsonpayload = jss.Serialize(Workout);
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

        // GET: Workout/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "WorkoutData/FindWorkout/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            WorkoutDto selectedWorkout = response.Content.ReadAsAsync<WorkoutDto>().Result;
            return View(selectedWorkout);
        }

        // POST: Workout/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "WorkoutData/DeleteWorkout/" + id;
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
