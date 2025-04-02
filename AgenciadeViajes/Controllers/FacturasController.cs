using AgenciadeViajes.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading;
using Microsoft.Ajax.Utilities;

namespace AgenciadeViajes.Controllers
{

    
    public class FacturasController : ApiController
    {
       
        private Proyectodb db = new Proyectodb();

        /// <summary>
        /// Obtiene todas las facturas con detalles relacionados.
        /// </summary>
        /// <returns>Lista de facturas.</returns>
        public IHttpActionResult Get()
        {
            var facturas = from F in db.Factura
                           join R in db.Reservas
                           on F.Reservacion.Id equals R.Id
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
                               IdFactura = F.Id,
                               cliente = C.Cliente.Nombre + " " + C.Cliente.Apellido,
                               PaqueteTuristico = P.Nombre,
                               Destino = D.NomDestino,
                               TotalPagar = R.Saldopendiente,
                               FechaPago = F.FechaPago,
                               MetodoPago = F.MetodoPago.Nombre,
                               MontoPagado = F.MontoPagado,
                               Estado = F.Estado,
                           };

            if (facturas.Count() == 0)
            {
                return NotFound();
            }

            return Ok(facturas);
        }

        /// <summary>
        /// Obtiene los detalles de una factura específica por su ID.
        /// </summary>
        /// <param name="id">ID de la factura a buscar.</param>
        /// <returns>Factura encontrada, si existe.</returns>
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var facturas = from F in db.Factura
                           where F.Id == id
                           join R in db.Reservas
                           on F.Reservacion.Id equals R.Id
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
                               IdFactura = F.Id,
                               cliente = C.Cliente.Nombre + " " + C.Cliente.Apellido,
                               PaqueteTuristico = P.Nombre,
                               Destino = D.NomDestino,
                               TotalPagar = R.Saldopendiente,
                               FechaPago = F.FechaPago,
                               MetodoPago = F.MetodoPago.Nombre,
                               MontoPagado = F.MontoPagado,
                               Estado = F.Estado,
                           };

            if (facturas == null)
            {
                return NotFound();
            }

            return Ok(facturas);
        }

        /// <summary>
        /// Crea una nueva factura.
        /// </summary>
        /// <param name="factura">Objeto Factura con los datos necesarios para la creación.</param>
        /// <returns>Factura creada.</returns>
        [HttpPost]
        public IHttpActionResult Post(Factura factura)
        {
            try
            {
                Reservacion reservacion = db.Reservas.Find(factura.Reservacion.Id);
                if (reservacion == null)
                    return BadRequest("Reservación no válida");

                Metodo_Pago metodo = db.MetododePagos.Find(factura.MetodoPago.Id);
                if (metodo == null)
                {
                    return BadRequest("Metodo no válido");
                }
                factura.MetodoPago = metodo;
                factura.Reservacion = reservacion;

                if (factura.FechaPago > DateTime.Now)
                    return BadRequest("La fecha de pago no puede ser futura");

                if (factura.MontoPagado <= 0)
                    return BadRequest("El monto pagado debe ser mayor a 0");

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

        /// <summary>
        /// Actualiza los datos de una factura existente.
        /// </summary>
        /// <param name="factura">Objeto Factura con los datos a actualizar.</param>
        /// <returns>Factura actualizada.</returns>
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

        /// <summary>
        /// Elimina una factura existente por su ID.
        /// </summary>
        /// <param name="id">ID de la factura a eliminar.</param>
        /// <returns>Factura eliminada.</returns>
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
