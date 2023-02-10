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
    public class ExerciseTypeController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ExerciseTypeController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44307/api/");
        }

        // GET: ExerciseType/List
        public ActionResult List()
        {
            //objective: communicate with our ExerciseType data api to retrieve a list of exerciseTypes
            //curl https://localhost:44307/api/ExerciseTypeData/listExerciseTypes

           
            string url = "exercisetypedata/listexercisetypes";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<ExerciseTypeDto> exerciseTypes = response.Content.ReadAsAsync<IEnumerable<ExerciseTypeDto>>().Result;
            //Debug.WriteLine("Number of exerciseTypes received : ");
            //Debug.WriteLine(exerciseTypes.Count());


            return View(exerciseTypes);
        }

        // GET: ExerciseType/Details/5
        public ActionResult Details(int id)
        {
            DetailsExerciseType ViewModel = new DetailsExerciseType();

            //objective: communicate with our ExerciseType data api to retrieve one ExerciseType
            //curl https://localhost:44307/api/ExerciseTypeData/FindExerciseType/{id}

            string url = "ExerciseTypeData/FindExerciseType/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            ExerciseTypeDto SelectedExerciseType = response.Content.ReadAsAsync<ExerciseTypeDto>().Result;
            //Debug.WriteLine("exercise type received : ");
            //Debug.WriteLine(SelectedExerciseType.ExerciseTypeName);

            ViewModel.SelectedExerciseType = SelectedExerciseType;

            //show exercises under this exercise type
            url = "ExerciseData/ListExercisesForExerciseType/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<ExerciseDto> allExercises = response.Content.ReadAsAsync<IEnumerable<ExerciseDto>>().Result;

            ViewModel.AllExercises = allExercises;

            return View(ViewModel);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: ExerciseType/New
        public ActionResult New()
        {
            return View();
        }

        // POST: ExerciseType/Create
        [HttpPost]
        public ActionResult Create(ExerciseType exerciseType)
        {
            //Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(ExerciseType.ExerciseTypeName);
            //objective: add a new ExerciseType into our system using the API
            //curl -H "Content-Type:application/json" -d @ExerciseType.json https://localhost:44307/api/ExerciseTypeData/addExerciseType 
            string url = "ExerciseTypeData/AddExerciseType";

            
            string jsonpayload = jss.Serialize(exerciseType);
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

        // GET: ExerciseType/Edit/5
        public ActionResult Edit(int id)
        {

            //the existing ExerciseType information
            string url = "ExerciseTypeData/FindExerciseType/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ExerciseTypeDto exerciseType = response.Content.ReadAsAsync<ExerciseTypeDto>().Result;

            return View(exerciseType);
        }

        // POST: ExerciseType/Update/5
        [HttpPost]
        public ActionResult Update(int id, ExerciseType ExerciseType)
        {
            
            string url = "ExerciseTypeData/UpdateExerciseType/" + id;
            string jsonpayload = jss.Serialize(ExerciseType);
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

        // GET: ExerciseType/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "ExerciseTypeData/FindExerciseType/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ExerciseTypeDto selectedExerciseType = response.Content.ReadAsAsync<ExerciseTypeDto>().Result;
            return View(selectedExerciseType);
        }

        // POST: ExerciseType/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "ExerciseTypeData/DeleteExerciseType/"+id;
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
