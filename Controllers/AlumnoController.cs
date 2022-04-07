using CRUD_Alumnos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRUD_Alumnos.Controllers
{
    public class AlumnoController : Controller
    {
        // GET: Alumno
        public ActionResult Index()
        {
            try
            {
                string sql = @"
                            select a.Id, a.Nombres, a.Apellidos, A.Edad, a.Sexo, a.FechaRegistro, c.Nombre as NombreCiudad
                            from Alumno a
                            inner join Ciudad c on a.CodCiudad = c.Id";

                using (var db = new AlumnosContext())
                {
                    /*var data = from a in db.Alumno
                               join c in db.Ciudad on a.CodCiudad equals c.Id
                               select new AlumnoCE()
                               {
                                   Id = a.Id,
                                   Nombres = a.Nombres,
                                   Apellidos = a.Apellidos,
                                   Edad = a.Edad,
                                   Sexo = a.Sexo,
                                   NombreCiudad = c.Nombre,
                                   FechaRegistro = a.FechaRegistro
                               };
                    return View(data.ToList());*/
                    return View(db.Database.SqlQuery<AlumnoCE>(sql).ToList());
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Alumno a)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (var db = new AlumnosContext())
                {
                    a.FechaRegistro = DateTime.Now;
                    db.Alumno.Add(a);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al agregar el Alumno - " + ex.Message);
                return View();
            }
        }

        public ActionResult ListaCiudades()
        {
            using (var db = new AlumnosContext())
            {
                return PartialView(db.Ciudad.ToList());
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                using (var db = new AlumnosContext())
                {
                    //Alumno al = db.Alumno.Where(a => a.Id == id).FirstOrDefault();
                    Alumno al = db.Alumno.Find(id);
                    return View(al);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Alumno a)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                using (var db = new AlumnosContext())
                {
                    Alumno al = db.Alumno.Find(a.Id);
                    al.Nombres = a.Nombres;
                    al.Apellidos = a.Apellidos;
                    al.Edad = a.Edad;
                    al.Sexo = a.Sexo;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult Details(int id)
        {
            using (var db = new AlumnosContext())
            {
                Alumno al = db.Alumno.Find(id);
                return View(al);
            }
        }

        public ActionResult Delete(int id)
        {
            using (var db = new AlumnosContext())
            {
                Alumno al = db.Alumno.Find(id);
                db.Alumno.Remove(al);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public static string CityName(int CodCiudad)
        {
            using (var db = new AlumnosContext())
            {
                return db.Ciudad.Find(CodCiudad).Nombre;
            }
        }
    }
}