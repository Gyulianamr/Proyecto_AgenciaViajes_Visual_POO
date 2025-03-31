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

    public class FacturasController : ApiController
    {
        private Proyectodb db = new Proyectodb();

        // GET: api/Factura
        [HttpGet]
        public IHttpActionResult Get()
        {
            var facturas = db.Factura.Include(f => f.Reservacion).ToList();
            return Ok(facturas);
        }

        // GET: api/Factura/5
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            Factura factura = db.Factura
                .Include(f => f.Reservacion)
                .FirstOrDefault(f => f.Id == id);

            if (factura == null) return NotFound();
            return Ok(factura);
        }

        // POST: api/Factura
        [HttpPost]
        public IHttpActionResult Post([FromBody] Factura factura)
        {
            try
            {
                // Validar reservación
                Reservacion reservacion = db.Reservas.Find(factura.Reservacion.Id);
                if (reservacion == null)
                    return BadRequest("Reservación no válida");

                Metodo_Pago metodo = db.MetododePagos.Find(factura.MetodoPago.Id);
                if (metodo == null)
                {
                    return BadRequest("Metodo no válida");
                }
                factura.MetodoPago = metodo;
                factura.Reservacion = reservacion;

                factura.Saldopendiente = reservacion.Cotizacion.Costo() - reservacion.MontoPagado;

                // Validar la fecha de pago
                if (factura.FechaPago > DateTime.Now)
                    return BadRequest("La fecha de pago no puede ser futura");

                // Validar el monto pagado
                if (factura.MontoPagado <= 0)
                    return BadRequest("El monto pagado debe ser mayor a 0");

                // Agregar factura y guardar cambios
                db.Factura.Add(factura);
                db.SaveChanges();
                return CreatedAtRoute("DefaultApi", new { id = factura.Id }, factura);
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

        // PUT: api/Factura
        [HttpPut]
        public IHttpActionResult Put(Factura factura)
        {
            try
            {
                var existente = db.Factura.Find(factura.Id);
                if (existente == null) return NotFound();

                // Validar reservación
                Reservacion reservacion = db.Reservas.Find(factura.Reservacion.Id);
                if (reservacion == null) return BadRequest("Reservación no válida");

                // Actualizar campos
                existente.Reservacion = reservacion;
                existente.FechaPago = factura.FechaPago;
                existente.MontoPagado = factura.MontoPagado;
                existente.MetodoPago = factura.MetodoPago;
                existente.Estado = factura.Estado;
                existente.Saldopendiente = factura.Saldopendiente;

                // Validar la fecha de pago
                if (factura.FechaPago > DateTime.Now)
                    return BadRequest("La fecha de pago no puede ser futura");

                // Validar el monto pagado
                if (factura.MontoPagado <= 0)
                    return BadRequest("El monto pagado debe ser mayor a 0");

                db.Entry(existente).State = EntityState.Modified;
                db.SaveChanges();
                return Ok(existente);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Factura/5
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            Factura factura = db.Factura.Find(id);
            if (factura == null) return NotFound();

            db.Factura.Remove(factura);
            db.SaveChanges();
            return Ok(factura);
        }
    }
}
