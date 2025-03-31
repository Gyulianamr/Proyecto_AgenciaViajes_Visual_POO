using AgenciadeViajes.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web.Http;

namespace AgenciadeViajes.Controllers
{
    public class ActividadesController : ApiController
    {
        private Proyectodb db = new Proyectodb();

        // GET: api/Actividades
        /// <summary>
        /// Obtiene la lista completa de actividades
        /// </summary>
        public IEnumerable<Actividades> Get()
        {
            return db.Actividades;
        }

        // GET: api/Actividades/5
        /// <summary>
        /// Busca una actividad por su ID
        /// </summary>
        public IHttpActionResult Getbuscar(int id)
        {
            Actividades actividad = db.Actividades.Find(id);
            if (actividad == null)
            {
                return NotFound();
            }
            return Ok(actividad);
        }

        /// POST: api/Actividades
        /// <summary>
        /// Crea una nueva actividad
        /// </summary>
        /// POST: api/Actividades
        /// <summary>
        /// Crea una nueva actividad
        /// </summary>
        public IHttpActionResult Post(Actividades actividad)
        {
            if (actividad == null)
            {
                return BadRequest("La actividad no puede estar vacía.");
            }

            try
            {
               
                if (string.IsNullOrWhiteSpace(actividad.Nombre))
                {
                    return BadRequest("El nombre de la actividad es obligatorio.");
                }

                if (actividad.DuracionHoras <= 0)
                {
                    return BadRequest("La duración debe ser mayor o igual a 1.");
                }

                if (actividad.PrecioHora <= 0)
                {
                    return BadRequest("El precio por hora debe ser mayor que 0.");
                }

                
                db.Actividades.Add(actividad);
                db.SaveChanges();
                return CreatedAtRoute("DefaultApi", new { id = actividad.Id }, actividad);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Ocurrió un error inesperado.", ex));
            }
        }
        // PUT: api/Actividades/5
        /// <summary>
        /// Actualiza una actividad existente
        /// </summary>
        public IHttpActionResult Put(Actividades actividad)
        {
            if (actividad == null)
            {
                return BadRequest("Informacion no puede ser nula");
            }
            try
            {
                int id = actividad.Id;
                db.Entry(actividad).State = EntityState.Modified;
                db.SaveChanges();
                return Ok(actividad);
            }
            catch
            {
                return NotFound();
            }
        }

        // DELETE: api/Actividades/5
        /// <summary>
        /// Elimina una actividad por su ID
        /// </summary>
        public IHttpActionResult Delete(int id)
        {
            Actividades actividad = db.Actividades.Find(id);
            if (actividad == null)
            {
                return NotFound();
            }

            db.Actividades.Remove(actividad);
            db.SaveChanges();
            return Ok(actividad);
        }

    }
}
