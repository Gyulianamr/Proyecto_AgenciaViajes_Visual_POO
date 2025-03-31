using AgenciadeViajes.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AgenciadeViajes.Controllers
{
    public class GuiaTuristicoController : ApiController
    {
        private Proyectodb db = new Proyectodb();
      
        // GET: api/GuiaTuristico
        /// <summary>
        /// Obtiene todos los guías turísticos.
        /// </summary>
        public IEnumerable<GuiaTuristico> Get()
        {
            return db.GuiaTuristicos;
        }

       

        // GET: api/GuiaTuristico/5
        /// <summary>
        /// Obtiene un guía turístico por ID.
        /// </summary>
        public IHttpActionResult Get(int id)
        {
            try
            {
                var guia = db.GuiaTuristicos.Find(id);
                if (guia == null)
                {
                    return NotFound();
                }
                return Ok(guia);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Ocurrió un error al obtener el guía turístico.", ex));
            }
        }

        // POST: api/GuiaTuristico
        /// <summary>
        /// Crea un nuevo guía turístico.
        /// </summary>
        public IHttpActionResult Post(GuiaTuristico guia)
        {
            if (guia == null)
            {
                return BadRequest("El guía turístico no puede estar vacío.");
            }

            try
            {
                db.GuiaTuristicos.Add(guia);
                db.SaveChanges();
                return CreatedAtRoute("DefaultApi", new { id = guia.Id }, guia);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Ocurrió un error al crear el guía turístico.", ex));
            }
        }

        // PUT: api/GuiaTuristico/5
        /// <summary>
        /// Actualiza un guía turístico existente.
        /// </summary>
        public IHttpActionResult Put(int id, GuiaTuristico guia)
        {
            if (guia == null || id != guia.Id)
            {
                return BadRequest("Los datos del guía turístico no son válidos.");
            }

            try
            {
                var guiaExistente = db.GuiaTuristicos.Find(id);
                if (guiaExistente == null)
                {
                    return NotFound();
                }

                db.Entry(guia).State = EntityState.Modified;
                db.SaveChanges();
                return Ok(guia);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Ocurrió un error al actualizar el guía turístico.", ex));
            }
        }

        // DELETE: api/GuiaTuristico/5
        /// <summary>
        /// Elimina un guía turístico por ID.
        /// </summary>
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var guia = db.GuiaTuristicos.Find(id);
                if (guia == null)
                {
                    return NotFound();
                }

                db.GuiaTuristicos.Remove(guia);
                db.SaveChanges();
                return Ok($"El guía turístico con ID {id} ha sido eliminado.");
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Ocurrió un error al eliminar el guía turístico.", ex));
            }
        }

    }
}
