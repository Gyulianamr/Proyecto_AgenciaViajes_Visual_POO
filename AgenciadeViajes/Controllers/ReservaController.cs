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
    [RoutePrefix("api/Reservaciones")]
    public class ReservacionesController : ApiController
    {
        private Proyectodb db = new Proyectodb();

        // GET: api/Reservaciones
        [HttpGet]
    

        
        public IHttpActionResult Get(int id)
        {
            Reservacion reservacion = db.Reservas
                .Include(r => r.Cotizacion)
                .Include(r => r.Cotizacion.Paquete)
                .FirstOrDefault(r => r.Id == id);

            if (reservacion == null) return NotFound();
            return Ok(reservacion);
        }

        // POST: api/Reservaciones
        [HttpPost]
        public IHttpActionResult Post([FromBody] Reservacion reservacion)
        {
            try
            {
               
                Cotizacion cotizacion = db.Cotizaciones.Find(reservacion.Cotizacion.Id);
                if (cotizacion == null)
                    return BadRequest("Cotización no válida");
                reservacion.Cotizacion = cotizacion;

                // Validar la fecha de reservación: si se envía una fecha futura, se sobreescribe con la fecha actual
                if (reservacion.FechaReservacion > DateTime.Now)
                    reservacion.FechaReservacion = DateTime.Now;

                // Validar el monto pagado (no debe ser negativo)
                if (reservacion.MontoPagado < 0)
                    return BadRequest("El monto pagado no puede ser negativo");

                // Validar fechas del viaje
                if (reservacion.FechaViaje < DateTime.Now)
                    return BadRequest("La fecha de viaje debe ser futura");
                if (reservacion.FechaRegreso <= reservacion.FechaViaje)
                    return BadRequest("La fecha de regreso debe ser posterior a la fecha de viaje");

                // Agregar la reservación y guardar los cambios
                db.Reservas.Add(reservacion);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = reservacion.Id }, reservacion);
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

        // PUT: api/Reservaciones
        [HttpPut]
        public IHttpActionResult Put(Reservacion reservacion)
        {
            try
            {
                var existente = db.Reservas.Find(reservacion.Id);
                if (existente == null) return NotFound();

                // Validar cotización
                Cotizacion cotizacion = db.Cotizaciones.Find(reservacion.Cotizacion.Id);
                if (cotizacion == null) return BadRequest("Cotización no válida");
                existente.Cotizacion = cotizacion;

                // Actualizar campos
                existente.Estado = reservacion.Estado;
                existente.FechaViaje = reservacion.FechaViaje;
                existente.FechaRegreso = reservacion.FechaRegreso;
                existente.MontoPagado = reservacion.MontoPagado;

                // Validar fechas
                if (existente.FechaViaje < DateTime.Now)
                    return BadRequest("Fecha viaje debe ser futura");

                if (existente.FechaRegreso <= existente.FechaViaje)
                    return BadRequest("Fecha regreso inválida");

                db.Entry(existente).State = EntityState.Modified;
                db.SaveChanges();
                return Ok(existente);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Reservaciones/5
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            Reservacion reservacion = db.Reservas.Find(id);
            if (reservacion == null) return NotFound();

            db.Reservas.Remove(reservacion);
            db.SaveChanges();
            return Ok(reservacion);
        }

        
    }
}
