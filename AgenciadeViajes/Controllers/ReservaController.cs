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
  
    public class ReservacionesController : ApiController
    {
        
        private Proyectodb db = new Proyectodb();

        /// <summary>
        /// Obtiene todas las reservaciones.
        /// </summary>
        /// <returns>Lista de reservaciones con sus detalles.</returns>
        public IHttpActionResult Get()
        {
            var VerReserva = from R in db.Reservas
                             join C in db.Cotizaciones
                             on R.Cotizacion.Id equals C.Id
                             join cliente in db.Clientes
                             on C.Cliente.Id equals cliente.Id
                             join P in db.PaqueteTuristicos
                             on C.Paquete.Id equals P.Id
                             join D in db.Destinos
                             on P.Destino.Id equals D.Id
                             select new
                             {
                                 IdReservacion = R.Id,
                                 FechaReservacion = R.FechaReservacion,
                                 Idcotizacion = C.Id,
                                 NombrePaquete = P.Nombre,
                                 Destino = D.NomDestino,
                                 cliente = cliente.Nombre,
                                 FechaViaje = R.FechaViaje,
                                 FechaRegreso = R.FechaRegreso,
                                 MontoPagado = R.MontoPagado,
                                 SaldoPendiente = R.Saldopendiente,
                                 EstadoReserva = R.Estado,
                             };

            if (VerReserva == null)
            {
                return NotFound();
            }

            return Ok(VerReserva);
        }

        /// <summary>
        /// Obtiene una reservación específica por su ID.
        /// </summary>
        /// <param name="id">ID de la reservación.</param>
        /// <returns>Detalles de la reservación si se encuentra.</returns>
        public IHttpActionResult Get(int id)
        {
            var VerReserva = from R in db.Reservas
                             where R.Id == id
                             join C in db.Cotizaciones
                             on R.Cotizacion.Id equals C.Id
                             join cliente in db.Clientes
                             on C.Cliente.Id equals cliente.Id
                             join P in db.PaqueteTuristicos
                             on C.Paquete.Id equals P.Id
                             join D in db.Destinos
                             on P.Destino.Id equals D.Id
                             select new
                             {
                                 IdReservacion = R.Id,
                                 FechaReservacion = R.FechaReservacion,
                                 Idcotizacion = C.Id,
                                 NombrePaquete = P.Nombre,
                                 Destino = D.NomDestino,
                                 cliente = cliente.Nombre,
                                 FechaViaje = R.FechaViaje,
                                 FechaRegreso = R.FechaRegreso,
                                 MontoPagado = R.MontoPagado,
                                 SaldoPendiente = R.Saldopendiente,
                                 EstadoReserva = R.Estado,
                             };

            if (VerReserva == null)
            {
                return NotFound();
            }

            return Ok(VerReserva);
        }

        /// <summary>
        /// Crea una nueva reservación.
        /// </summary>
        /// <param name="reservacion">Detalles de la reservación a crear.</param>
        /// <returns>Reservación creada.</returns>
        [HttpPost]
        public IHttpActionResult Post([FromBody] Reservacion reservacion)
        {
            try
            {
                Cotizacion cotizacion = db.Cotizaciones.Find(reservacion.Cotizacion.Id);
                if (cotizacion == null)
                    return BadRequest("Cotización no válida");
                reservacion.Cotizacion = cotizacion;

                if (reservacion.FechaReservacion > DateTime.Now)
                    reservacion.FechaReservacion = DateTime.Now;

                if (reservacion.MontoPagado < 0)
                    return BadRequest("El monto pagado no puede ser negativo");

                if (reservacion.FechaViaje < DateTime.Now)
                    return BadRequest("La fecha de viaje debe ser futura");

                if (reservacion.FechaRegreso <= reservacion.FechaViaje)
                    return BadRequest("La fecha de regreso debe ser posterior a la fecha de viaje");

                reservacion.Saldopendiente = reservacion.CalcularSaldoPendiente();

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

        /// <summary>
        /// Actualiza una reservación existente.
        /// </summary>
        /// <param name="reservacion">Detalles de la reservación a actualizar.</param>
        /// <returns>Reservación actualizada.</returns>
        [HttpPut]
        public IHttpActionResult Put(Reservacion reservacion)
        {
            try
            {
                var existente = db.Reservas.Find(reservacion.Id);
                if (existente == null) return NotFound();

                Cotizacion cotizacion = db.Cotizaciones.Find(reservacion.Cotizacion.Id);
                if (cotizacion == null) return BadRequest("Cotización no válida");
                existente.Cotizacion = cotizacion;

                existente.Estado = reservacion.Estado;
                existente.FechaViaje = reservacion.FechaViaje;
                existente.FechaRegreso = reservacion.FechaRegreso;
                existente.MontoPagado = reservacion.MontoPagado;
                existente.Saldopendiente = reservacion.Saldopendiente;

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

        /// <summary>
        /// Elimina una reservación específica por su ID.
        /// </summary>
        /// <param name="id">ID de la reservación a eliminar.</param>
        /// <returns>Reservación eliminada.</returns>
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
