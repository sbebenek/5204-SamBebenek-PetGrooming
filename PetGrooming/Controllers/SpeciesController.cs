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
using System.Diagnostics;

namespace PetGrooming.Controllers
{
    public class SpeciesController : Controller
    {

        private PetGroomingContext db = new PetGroomingContext();

        public ActionResult List()
        {
            //How could we modify this to include a search bar?
            List<Species> species = db.Species.SqlQuery("Select * from Species").ToList();
            return View(species);
        }

        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // selecting all pets that are of the given speciesid
            List<Pet> pets = db.Pets.SqlQuery("select * from pets where speciesid=@SpeciesID", new SqlParameter("@SpeciesID", id)).ToList();
            if (pets == null)
            {
                return HttpNotFound();
            }
            return View(pets);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(string SpeciesName)
        {
            string query = "insert into species (Name) values (@SpeciesName)";
            SqlParameter[] sqlparams = new SqlParameter[1]; //0,1,2,3,4,5 pieces of information to add
            //each piece of information is a key and value pair
            sqlparams[0] = new SqlParameter("@SpeciesName", SpeciesName);

            Debug.WriteLine("sqlparams = " + sqlparams);

            //db.Database.ExecuteSqlCommand will run insert, update, delete statements
            //db.Pets.SqlCommand will run a select statement, for example.
            db.Database.ExecuteSqlCommand(query, sqlparams);

            //run the list method to return to a list of pets so we can see our new one!
            return RedirectToAction("List", new { add = true });
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Species species = db.Species.SqlQuery("select * from species where speciesid=@SpeciesID", new SqlParameter("@SpeciesID", id)).FirstOrDefault();
            if (species == null)
            {
                return HttpNotFound();
            }
            //need the pet data
            return View(species);
        }

        [HttpPost]
        public ActionResult Update(int id, string SpeciesName)
        {

            string query = "update species set Name = @SpeciesName where SpeciesID = @Id";

            
            SqlParameter[] sqlparams = new SqlParameter[2]; //0,1,2,3,4,5 pieces of information to add

            sqlparams[0] = new SqlParameter("@SpeciesName", SpeciesName);
            sqlparams[1] = new SqlParameter("@Id", id);



            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("List", new { update = true });
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string query = "delete from species where SpeciesID=@id";
            SqlParameter sqlparam = new SqlParameter("id", id);

            db.Database.ExecuteSqlCommand(query, sqlparam);

            //also delete all pets of this species
            string speciesquery = "delete from pets where SpeciesID=@id";
            db.Database.ExecuteSqlCommand(speciesquery, sqlparam);



            return RedirectToAction("List", new { delete = true });
        }

        
    }
}