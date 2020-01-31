using System;
using System.Collections.Generic;
using System.Data;
//required for SqlParameter class
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PetGrooming.Data;
using PetGrooming.Models;
//required to use viewmodels
using PetGrooming.Models.ViewModels;
using System.Diagnostics;

namespace PetGrooming.Controllers
{
    public class PetController : Controller
    {
        /*
        These reading resources will help you understand and navigate the MVC environment
 
        Q: What is an MVC controller?

        - https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions-1/controllers-and-routing/aspnet-mvc-controllers-overview-cs

        Q: What does it mean to "Pass Data" from the Controller to the View?

        - http://www.webdevelopmenthelp.net/2014/06/using-model-pass-data-asp-net-mvc.html

        Q: What is an SQL injection attack?

        - https://www.w3schools.com/sql/sql_injection.asp

        Q: How can we prevent SQL injection attacks?

        - https://www.completecsharptutorial.com/ado-net/insert-records-using-simple-and-parameterized-query-c-sql.php

        Q: How can I run an SQL query against a database inside a controller file?

        - https://www.entityframeworktutorial.net/EntityFramework4.3/raw-sql-query-in-entity-framework.aspx
 
         */
        private PetGroomingContext db = new PetGroomingContext();

        // GET: Pet
        public ActionResult List()
        {
            //How could we modify this to include a search bar?
            List<Pet> pets = db.Pets.SqlQuery("Select * from Pets").ToList();
            return View(pets);
        }

        // GET: Pet/Details/5
        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Pet pet = db.Pets.Find(id); //EF 6 technique
            Pet pet = db.Pets.SqlQuery("select * from pets where petid=@PetID", new SqlParameter("@PetID",id)).FirstOrDefault();
            if (pet == null)
            {
                return HttpNotFound();
            }
            return View(pet);
        }

        //THE [HttpPost] Means that this method will only be activated on a POST form submit to the following URL
        //URL: /Pet/Add
        [HttpPost]
        public ActionResult Add(string PetName, Double PetWeight, String PetColor, int SpeciesID, string PetNotes, string Sex)
        {
            //STEP 1: PULL DATA! The data is access as arguments to the method. Make sure the datatype is correct!
            //The variable name  MUST match the name attribute described in Views/Pet/Add.cshtml

            //Tests are very useul to determining if you are pulling data correctly!
            //Debug.WriteLine("Want to create a pet with name " + PetName + " and weight " + PetWeight.ToString()) ;
            Debug.WriteLine("I want to create a pet with name " + PetName + " that is a " + Sex);

            //STEP 2: FORMAT QUERY! the query will look something like "insert into () values ()"...
            string query = "insert into pets (PetName, Weight, color, SpeciesID, Notes, Sex) values (@PetName,@PetWeight,@PetColor,@SpeciesID,@PetNotes,@Sex)";
            SqlParameter[] sqlparams = new SqlParameter[6]; //0,1,2,3,4,5 pieces of information to add
            //each piece of information is a key and value pair
            sqlparams[0] = new SqlParameter("@PetName",PetName);
            sqlparams[1] = new SqlParameter("@PetWeight", PetWeight);
            sqlparams[2] = new SqlParameter("@PetColor", PetColor);
            sqlparams[3] = new SqlParameter("@SpeciesID", SpeciesID);
            sqlparams[4] = new SqlParameter("@PetNotes",PetNotes);
            sqlparams[5] = new SqlParameter("@Sex", Sex);

            Debug.WriteLine("sqlparams = " + sqlparams);

            //db.Database.ExecuteSqlCommand will run insert, update, delete statements
            //db.Pets.SqlCommand will run a select statement, for example.
            db.Database.ExecuteSqlCommand(query, sqlparams);

            
            //run the list method to return to a list of pets so we can see our new one!
            return RedirectToAction("List", new { add = true});
        }


        public ActionResult Add()
        {
            //STEP 1: PUSH DATA!
            //What data does the Add.cshtml page need to display the interface?
            //A list of species to choose for a pet

            //alternative way of writing SQL -- will learn more about this week 4
            //List<Species> Species = db.Species.ToList();

            List<Species> species = db.Species.SqlQuery("select * from Species").ToList();

            return View(species);
        }


        //ActionResult is a way of routing information to the view
        //URL: /Pet/Update/2 -> update pet with id 2
        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //how to get pet data? run query
            //first or default converts the first sql result into a pet object
            Pet selectedpet = db.Pets.SqlQuery("select * from pets where petid = @PetID", new SqlParameter("PetID",id)).FirstOrDefault();
            if (selectedpet == null)
            {
                return HttpNotFound();
            }

            //grabbing all pets
            string query = "select * from species";
            List<Species> selectedspecies = db.Species.SqlQuery(query).ToList();

            //create instance of our viewmodel
            UpdatePet viewmodel = new UpdatePet();
            viewmodel.pet = selectedpet;
            viewmodel.species = selectedspecies;

            return View(viewmodel);
        }

        [HttpPost]
        public ActionResult Update(string PetName, string PetColor, string PetNotes, double PetWeight, int SpeciesID, string Sex, int id)
        {
           
            //STEP 1: PULL DATA
            Debug.WriteLine("I'm pulling data of " + id + " " + PetName + " and " + PetColor + " and " + PetWeight.ToString());
            
            //step 2: 
            //TODO: finish this update string
            string query = "update pets set PetName = @PetName, Weight = @PetWeight, color = @PetColor, SpeciesID = @SpeciesID, " +
                "Notes = @PetNotes, Sex = @Sex where PetID = @Id";

            /*UPDATE table_name
            SET column1 = value1, column2 = value2, ...
            WHERE condition;*/
            SqlParameter[] sqlparams = new SqlParameter[7]; //0,1,2,3,4,5 pieces of information to add

            sqlparams[0] = new SqlParameter("@PetName", PetName);
            sqlparams[1] = new SqlParameter("@PetWeight", PetWeight);
            sqlparams[2] = new SqlParameter("@PetColor", PetColor);
            sqlparams[3] = new SqlParameter("@Id", id);
            sqlparams[4] = new SqlParameter("@PetNotes", PetNotes);
            sqlparams[5] = new SqlParameter("@Sex", Sex);
            sqlparams[6] = new SqlParameter("@SpeciesID", SpeciesID);


            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("Show/" + id, new { update = true});
        }
        

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Debug.WriteLine("I want to delete the pet at id " + id);

            string query = "delete from pets where PetID=@id";
            SqlParameter sqlparam = new SqlParameter("id" , id);

            db.Database.ExecuteSqlCommand(query, sqlparam);

            return RedirectToAction("List", new { delete = true });
        }
        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
