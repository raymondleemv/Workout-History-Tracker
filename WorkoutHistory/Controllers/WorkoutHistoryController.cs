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
    public class WorkoutHistoryController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static WorkoutHistoryController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44307/api/");
        }

        // GET: WorkoutHistory/List
        public ActionResult List()
        {
            //objective: communicate with our WorkoutHistory data api to retrieve a list of WorkoutHistorys
            //curl https://localhost:44307/api/WorkoutHistorydata/listWorkoutHistorys

            string url = "WorkoutHistoryData/ListWorkoutHistoryDates";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<string> WorkoutHistoryDates = response.Content.ReadAsAsync<IEnumerable<string>>().Result;

            return View(WorkoutHistoryDates);
        }

        // GET: WorkoutHistory/Details/5
        public ActionResult Details(string id)
        {
            //objective: communicate with our WorkoutHistory data api to retrieve one WorkoutHistory
            //curl https://localhost:44307/api/WorkoutHistorydata/FindWorkoutHistory/{id}

            //Debug.WriteLine("Details - WorkoutHistory");
            //Debug.WriteLine(id);

            string url = "WorkoutHistoryData/ListWorkoutHistoryForDate/" + id;
            //Debug.WriteLine(url);
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<WorkoutHistoryDto> SelectedWorkoutHistory = response.Content.ReadAsAsync<IEnumerable<WorkoutHistoryDto>>().Result;
            //Debug.WriteLine("WorkoutHistory received : ");

            return View(SelectedWorkoutHistory);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: WorkoutHistory/New
        public ActionResult New()
        {
            string url = "ExerciseData/ListExercises";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<ExerciseDto> Exercises = response.Content.ReadAsAsync<IEnumerable<ExerciseDto>>().Result;
            return View(Exercises);
        }

        // POST: WorkoutHistory/Create
        [HttpPost]
        public ActionResult Create(WorkoutHistory WorkoutHistory)
        {
            //Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(WorkoutHistory.Date);
            //objective: add a new WorkoutHistory into our system using the API
            //curl -H "Content-Type:application/json" -d @WorkoutHistory.json https://localhost:44307/api/WorkoutHistorydata/addWorkoutHistory 
            string url = "WorkoutHistoryData/AddWorkoutHistory";


            string jsonpayload = jss.Serialize(WorkoutHistory);
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

        // GET: WorkoutHistory/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateWorkoutHistory ViewModel = new UpdateWorkoutHistory();

            string url = "WorkoutHistoryData/FindWorkoutHistory/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ViewModel.SelectedWorkoutHistory = response.Content.ReadAsAsync<WorkoutHistoryDto>().Result;

            url = "ExerciseData/ListExercises";
            response = client.GetAsync(url).Result;
            ViewModel.AllExercises = response.Content.ReadAsAsync<IEnumerable<ExerciseDto>>().Result;

            return View(ViewModel);
        }

        // POST: WorkoutHistory/Update/5
        [HttpPost]
        public ActionResult Update(int id, WorkoutHistory WorkoutHistory)
        {

            string url = "WorkoutHistoryData/UpdateWorkoutHistory/" + id;
            string jsonpayload = jss.Serialize(WorkoutHistory);
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

        // GET: WorkoutHistory/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "WorkoutHistoryData/FindWorkoutHistory/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            WorkoutHistoryDto selectedWorkoutHistory = response.Content.ReadAsAsync<WorkoutHistoryDto>().Result;
            //Debug.WriteLine("selected workout history:");
            //Debug.WriteLine(selectedWorkoutHistory.Date);
            return View(selectedWorkoutHistory);
        }

        // POST: WorkoutHistory/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "WorkoutHistoryData/DeleteWorkoutHistory/" + id;
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
