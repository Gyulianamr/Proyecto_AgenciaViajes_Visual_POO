using AgenciadeViajes.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;


namespace AgenciadeViajes.Controllers
{

    public class SeguroController : ApiController
    {
        private Proyectodb db = new Proyectodb();

        // GET: api/Seguro
        /// <summary>
        /// Obtiene todos los seguros registrados
        /// </summary>
        public IEnumerable<Seguro> Get()
        {
            return db.Seguros;
        }

        // GET: api/Seguro/5
        /// <summary>
        /// Busca un seguro por su ID
        /// </summary>
        public IHttpActionResult GetBuscar(int id)
        {
            Seguro seguro = db.Seguros.Find(id);
            if (seguro == null) return NotFound();
            return Ok(seguro);
        }

        // POST: api/Seguro
        /// <summary>
        /// Crea un nuevo seguro
        /// </summary>
        public IHttpActionResult Post(Seguro seguro)
        {
            try
            {
                db.Seguros.Add(seguro);
                db.SaveChanges();
                return CreatedAtRoute("DefaultApi", new { id = seguro.Id }, seguro);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Seguro/5
        /// <summary>
        /// Actualiza un seguro existente
        /// </summary>
        public IHttpActionResult Put(int id, Seguro seguro)
        {
            try
            {
                if (id != seguro.Id) return BadRequest("IDs no coinciden");

                var seguroExistente = db.Seguros.Find(id);
                if (seguroExistente == null) return NotFound();

                // Actualizar propiedades
                seguroExistente.Nombre = seguro.Nombre;
                seguroExistente.Precio = seguro.Precio;
                seguroExistente.Tipo = seguro.Tipo;
                seguroExistente.Cobertura = seguro.Cobertura;
                seguroExistente.DuracionDias = seguro.DuracionDias;

                db.SaveChanges();
                return Ok(seguroExistente);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Seguro/5
        /// <summary>
        /// Elimina un seguro por su ID
        /// </summary>
        public IHttpActionResult Delete(int id)
        {
            Seguro seguro = db.Seguros.Find(id);
            if (seguro == null) return NotFound();

            db.Seguros.Remove(seguro);
            db.SaveChanges();
            return Ok(seguro);
        }
    }
}
