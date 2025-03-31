using AgenciadeViajes.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Http;


namespace AgenciadeViajes.Controllers
{

    public class DestinoController : ApiController
    {
        private Proyectodb db = new Proyectodb();

        // GET: api/Destino
        /// <summary>
        /// Obtiene todos los destinos registrados
        /// </summary>
        /// <returns>Lista completa de destinos</returns>
        public IEnumerable<Destino> Get()
        {
            return db.Destinos;
        }

        // GET: api/Destino/5
        /// <summary>
        /// Busca un destino por su ID
        /// </summary>
        /// <param name="id">ID del destino</param>
        /// <returns>Destino encontrado o NotFound</returns>
        public IHttpActionResult GetBuscar(int id)
        {
            Destino destino = db.Destinos.Find(id);
            if (destino == null)
            {
                return NotFound();
            }
            return Ok(destino);
        }

        // POST: api/Destino
        /// <summary>
        /// Crea un nuevo destino turístico
        /// </summary>
        /// <param name="destino">Datos del destino a crear</param>
        /// <returns>Destino creado con ID asignado</returns>
        public IHttpActionResult Post(Destino destino)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Destinos.Add(destino);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = destino.Id }, destino);
        }

        // PUT: api/Destino/5
        /// <summary>
        /// Actualiza los datos de un destino existente
        /// </summary>
        /// <param name="id">ID del destino a modificar</param>
        /// <param name="destino">Datos actualizados del destino</param>
        /// <returns>Destino modificado</returns>
        public IHttpActionResult Put(int id, Destino destino)
        {
            if (id != destino.Id)
            {
                return BadRequest("El ID del destino no coincide con la URL");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(destino).State = EntityState.Modified;
            db.SaveChanges();

            return Ok(destino);
        }

        // DELETE: api/Destino/5
        /// <summary>
        /// Elimina un destino por su ID
        /// </summary>
        /// <param name="id">ID del destino a eliminar</param>
        /// <returns>Destino eliminado</returns>
        public IHttpActionResult Delete(int id)
        {
            Destino destino = db.Destinos.Find(id);
            if (destino == null)
            {
                return NotFound();
            }

            db.Destinos.Remove(destino);
            db.SaveChanges();

            return Ok(destino);
        }
    }
}
