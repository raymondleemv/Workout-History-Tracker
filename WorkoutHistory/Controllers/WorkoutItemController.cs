using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using WorkoutHistoryApp.Models;
using WorkoutHistoryApp.Models.ViewModels;
using System.Web.Script.Serialization;


namespace WorkoutHistoryApp.Controllers
{
    public class WorkoutItemController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static WorkoutItemController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44307/api/");
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: WorkoutItem/New
        public ActionResult New(int id)
        {
            NewWorkoutItem ViewModel = new NewWorkoutItem();

            string url = "WorkoutData/FindWorkout/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            WorkoutDto workout = response.Content.ReadAsAsync<WorkoutDto>().Result;

            ViewModel.SelectedWorkout = workout;

            url = "ExerciseData/ListExercises";
            response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<ExerciseDto> Exercises = response.Content.ReadAsAsync<IEnumerable<ExerciseDto>>().Result;

            ViewModel.AllExercises = Exercises;

            return View(ViewModel);
        }

        // POST: WorkoutItem/Create
        [HttpPost]
        public ActionResult Create(WorkoutItem WorkoutItem)
        {
            //objective: add a new WorkoutItem into our system using the API
            //curl -H "Content-Type:application/json" -d @WorkoutItem.json https://localhost:44307/api/WorkoutItemdata/addWorkoutItem 
            string url = "WorkoutItemData/AddWorkoutItem";


            string jsonpayload = jss.Serialize(WorkoutItem);
            //Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return Redirect("~/Workout/Details/" + WorkoutItem.WorkoutID);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: WorkoutItem/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateWorkoutItem ViewModel = new UpdateWorkoutItem();

            string url = "WorkoutItemData/FindWorkoutItem/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ViewModel.SelectedWorkoutItem = response.Content.ReadAsAsync<WorkoutItemDto>().Result;

            url = "ExerciseData/ListExercises";
            response = client.GetAsync(url).Result;
            ViewModel.AllExercises = response.Content.ReadAsAsync<IEnumerable<ExerciseDto>>().Result;

            return View(ViewModel);
        }

        // POST: WorkoutItem/Update/5
        [HttpPost]
        public ActionResult Update(int id, WorkoutItem WorkoutItem)
        {

            string url = "WorkoutItemData/UpdateWorkoutItem/" + id;
            string jsonpayload = jss.Serialize(WorkoutItem);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            //Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return Redirect("~/Workout/Details/" + WorkoutItem.WorkoutID);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: WorkoutItem/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "WorkoutItemData/FindWorkoutItem/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            WorkoutItemDto selectedWorkoutItem = response.Content.ReadAsAsync<WorkoutItemDto>().Result;
            //Debug.WriteLine("selected workout history:");
            //Debug.WriteLine(selectedWorkoutItem.Date);
            return View(selectedWorkoutItem);
        }

        // POST: WorkoutItem/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "WorkoutItemData/FindWorkoutItem/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            WorkoutItemDto selectedWorkoutItem = response.Content.ReadAsAsync<WorkoutItemDto>().Result;

            url = "WorkoutItemData/DeleteWorkoutItem/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return Redirect("~/Workout/Details/" + selectedWorkoutItem.WorkoutID);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
