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
    public class AgentedeViajeController : ApiController
    {
        private Proyectodb db = new Proyectodb();
        // GET: api/AgentedeViaje
        public IEnumerable<AgentedeViaje> Get()
        {
            return db.AgenteViajes;
        }


            // GET: api/AgentedeViaje/5
            public IHttpActionResult Get(int id)
            {
                try
                {
                    var agente = db.AgenteViajes.Find(id);
                    if (agente == null)
                    {
                        return NotFound();
                    }
                    return Ok(agente);
                }
                catch (Exception ex)
                {
                    return InternalServerError(new Exception("Ocurrió un error al obtener el agente de viaje.", ex));
                }
            }

            // POST: api/AgentedeViaje
            public IHttpActionResult Post(AgentedeViaje agente)
            {
                if (agente == null)
                {
                    return BadRequest("El agente de viaje no puede estar vacío.");
                }

                try
                {
                    db.AgenteViajes.Add(agente);
                    db.SaveChanges();
                    return CreatedAtRoute("DefaultApi", new { id = agente.Id }, agente);
                }
                catch (Exception ex)
                {
                    return InternalServerError(new Exception("Ocurrió un error al crear el agente de viaje.", ex));
                }
            }

            // PUT: api/AgentedeViaje/5
            public IHttpActionResult Put(int id, AgentedeViaje agente)
            {
                if (agente == null || id != agente.Id)
                {
                    return BadRequest("Los datos del agente de viaje no son válidos.");
                }

                try
                {
                    var agenteExistente = db.AgenteViajes.Find(id);
                    if (agenteExistente == null)
                    {
                        return NotFound();
                    }

                    db.Entry(agente).State = EntityState.Modified;
                    db.SaveChanges();
                    return Ok(agente);
                }
                catch (Exception ex)
                {
                    return InternalServerError(new Exception("Ocurrió un error al actualizar el agente de viaje.", ex));
                }
            }

            // DELETE: api/AgentedeViaje/5
            public IHttpActionResult Delete(int id)
            {
                try
                {
                    var agente = db.AgenteViajes.Find(id);
                    if (agente == null)
                    {
                        return NotFound();
                    }

                    db.AgenteViajes.Remove(agente);
                    db.SaveChanges();
                    return Ok($"El agente de viaje con ID {id} ha sido eliminado.");
                }
                catch (Exception ex)
                {
                    return InternalServerError(new Exception("Ocurrió un error al eliminar el agente de viaje.", ex));
                }
            }
        
    }
}
