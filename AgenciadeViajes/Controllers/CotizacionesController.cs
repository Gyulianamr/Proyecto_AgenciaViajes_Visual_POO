using AgenciadeViajes.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AgenciadeViajes.Controllers
{
    [RoutePrefix("api/Cotizaciones")]
    public class CotizacionesController : ApiController
    {
        private Proyectodb db = new Proyectodb();

        // GET: api/Cotizaciones
        [HttpGet]
        public IEnumerable<Cotizacion> Get()
        {
            return db.Cotizaciones
                .Include(c => c.Cliente)
                .Include(c => c.Agente_Responsable)
                .Include(c => c.Paquete);
        }

        // GET: api/Cotizaciones/5
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            Cotizacion cotizacion = db.Cotizaciones
                .Include(c => c.Cliente)
                .Include(c => c.Agente_Responsable)
                .Include(c => c.Paquete)
                .FirstOrDefault(c => c.Id == id);

            if (cotizacion == null) return NotFound();
            return Ok(cotizacion);
        }

        // POST: api/Cotizaciones
        [HttpPost]
        public IHttpActionResult Post([FromBody] Cotizacion cotizacion)
        {
            try
            {
                // Validación manual de relaciones
                Cliente cliente = db.Clientes.Find(cotizacion.Cliente.Id);
                if (cliente == null)
                { return BadRequest("Cliente no válido"); }
                cotizacion.Cliente = cliente;

                AgentedeViaje agente = db.AgenteViajes.Find(cotizacion.Agente_Responsable.Id);
                if (agente == null)
                {
                    return BadRequest("Agente no válido");
                }
                cotizacion.Agente_Responsable = agente;

                Paquete_Turistico paquete = db.PaqueteTuristicos.Find(cotizacion.Paquete.Id);
                if (paquete == null)
                {
                    return BadRequest("Paquete no válido");
                }
                cotizacion.Paquete = paquete;

                cotizacion.CostoTotal = cotizacion.Costo();

                // Forzar fecha actual si es futura
                if (cotizacion.FechaCotizacion > DateTime.Now)
                    cotizacion.FechaCotizacion = DateTime.Now;


                db.Cotizaciones.Add(cotizacion);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = cotizacion.Id }, cotizacion);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("Error de BD: " + ex.InnerException?.Message);
            }
        }

        // PUT: api/Cotizaciones/5
        [HttpPut]
        public IHttpActionResult Put(Cotizacion cotizacion)
        {
            try
            {


                var existente = db.Cotizaciones.Find(cotizacion.Id);
                if (existente == null) return NotFound();

                // Validación manual de relaciones
                Cliente cliente = db.Clientes.Find(cotizacion.Cliente.Id);
                if (cliente == null)
                { return BadRequest("Cliente no válido"); }
                cotizacion.Cliente = cliente;

                AgentedeViaje agente = db.AgenteViajes.Find(cotizacion.Agente_Responsable.Id);
                if (agente == null)
                {
                    return BadRequest("Agente no válido");
                }
                cotizacion.Agente_Responsable = agente;

                Paquete_Turistico paquete = db.PaqueteTuristicos.Find(cotizacion.Paquete.Id);
                if (paquete == null)
                {
                    return BadRequest("Paquete no válido");
                }
                cotizacion.Paquete = paquete;

                cotizacion.CostoTotal = cotizacion.Costo();

                // Forzar fecha actual si es futura
                if (cotizacion.FechaCotizacion > DateTime.Now)
                    cotizacion.FechaCotizacion = DateTime.Now;

                db.Entry(existente).State = EntityState.Modified;
                db.SaveChanges();
                return Ok(existente);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Cotizaciones/5
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            Cotizacion cotizacion = db.Cotizaciones.Find(id);
            if (cotizacion == null) return NotFound();

            db.Cotizaciones.Remove(cotizacion);
            db.SaveChanges();
            return Ok(cotizacion);
        }

    }
    
}
