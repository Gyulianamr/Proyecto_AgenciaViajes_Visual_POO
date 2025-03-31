using AgenciadeViajes.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AgenciadeViajes.Controllers
{
    public class PaquetesTuristicosController : ApiController
    {

        private Proyectodb db = new Proyectodb();

        // GET: api/PaqueteTuristico
        public IEnumerable<Paquete_Turistico> Get()
            {
                return db.PaqueteTuristicos
                    .Include(p => p.Destino)
                    .Include(p => p.Vuelo)
                    .Include(p => p.Hotel)
                    .Include(p => p.Seguro)
                    .Include(p => p.GuiaTuristico)
                    .Include(p => p.Actividades);
            }

            // GET: api/PaqueteTuristico/5
            public IHttpActionResult GetBuscar(int id)
            {
                Paquete_Turistico paquete = db.PaqueteTuristicos
                    .Include(p => p.Destino)
                    .Include(p => p.Vuelo)
                    .Include(p => p.Hotel)
                    .FirstOrDefault(p => p.Id == id);

                if (paquete == null) return NotFound();
                return Ok(paquete);
            }

        // POST: api/PaqueteTuristico
        public IHttpActionResult Post(Paquete_Turistico paquete)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (paquete.DestinoId == 0)
                {
                    return BadRequest("El campo DestinoId es requerido");
                }

                // Validar destino (obligatorio)
                var destino = db.Destinos.Find(paquete.DestinoId);
                if (destino == null)
                {
                    return BadRequest("Destino no encontrado");
                }

                // Validar entidades opcionales
                if (paquete.VueloId.HasValue && db.Vuelos.Find(paquete.VueloId.Value) == null)
                {
                    return BadRequest("Vuelo no encontrado");
                }

                if (paquete.HotelId.HasValue && db.Hotel.Find(paquete.HotelId.Value) == null)
                {
                    return BadRequest("Hotel no encontrado");
                }

                if (paquete.SeguroId.HasValue && db.Seguros.Find(paquete.SeguroId.Value) == null)
                {
                    return BadRequest("Seguro no encontrado");
                }

                if (paquete.GuiaTuristicoId.HasValue && db.GuiaTuristicos.Find(paquete.GuiaTuristicoId.Value) == null)
                {
                    return BadRequest("Guía turístico no encontrado");
                }

                if (paquete.ActividadesId.HasValue && db.Actividades.Find(paquete.ActividadesId.Value) == null)
                {
                    return BadRequest("Actividad no encontrada");
                }

                // Cargar entidades relacionadas
                paquete.Vuelo = paquete.VueloId.HasValue ? db.Vuelos.Find(paquete.VueloId.Value) : null;
                paquete.Hotel = paquete.HotelId.HasValue ? db.Hotel.Find(paquete.HotelId.Value) : null;
                paquete.Seguro = paquete.SeguroId.HasValue ? db.Seguros.Find(paquete.SeguroId.Value) : null;
                paquete.GuiaTuristico = paquete.GuiaTuristicoId.HasValue ? db.GuiaTuristicos.Find(paquete.GuiaTuristicoId.Value) : null;
                paquete.Actividades = paquete.ActividadesId.HasValue ? db.Actividades.Find(paquete.ActividadesId.Value) : null;
                paquete.Destino = destino;

                // Calcular precio
                paquete.PrecioTotal = paquete.CalcularCostoTotal();

                // Guardar
                db.PaqueteTuristicos.Add(paquete);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = paquete.Id }, paquete);
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                                    .SelectMany(x => x.ValidationErrors)
                                    .Select(x => x.ErrorMessage);
                return BadRequest(string.Join("; ", errorMessages));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/PaqueteTuristico/5
        public IHttpActionResult Put(int id, Paquete_Turistico paquete)
            {
                try
                {
                    if (id != paquete.Id)
                        return BadRequest("ID no coincide");

                    var existente = db.PaqueteTuristicos
                        .Include(p => p.Destino)
                        .FirstOrDefault(p => p.Id == id);

                    if (existente == null) return NotFound();

                    // Actualizar relaciones
                    existente.VueloId = paquete.VueloId;
                    existente.HotelId = paquete.HotelId;
                    existente.SeguroId = paquete.SeguroId;
                    existente.GuiaTuristicoId = paquete.GuiaTuristicoId;
                    existente.ActividadesId = paquete.ActividadesId;

                    // Actualizar propiedades básicas
                    existente.Nombre = paquete.Nombre;
                    existente.FechaExpiracion = paquete.FechaExpiracion;
                    existente.Estado = paquete.Estado;
                    existente.Destino = db.Destinos.Find(paquete.Destino.Id);

                    // Recalcular precio
                    existente.PrecioTotal = existente.CalcularCostoTotal();

                    db.Entry(existente).State = EntityState.Modified;
                    db.SaveChanges();

                    return Ok(existente);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            // DELETE: api/PaqueteTuristico/5
            public IHttpActionResult Delete(int id)
            {
                Paquete_Turistico paquete = db.PaqueteTuristicos.Find(id);
                if (paquete == null) return NotFound();

                db.PaqueteTuristicos.Remove(paquete);
                db.SaveChanges();
                return Ok(paquete);
            }

            [HttpGet]
            [Route("api/PaqueteTuristico/buscar-por-destino-precio")]
            public IHttpActionResult BuscarPorDestinoYPrecio(int destinoId, double precioMaximo)
            {
                var resultados = db.PaqueteTuristicos
                    .Where(p => p.Destino.Id == destinoId && p.PrecioTotal <= precioMaximo)
                    .OrderBy(p => p.PrecioTotal)
                    .Select(p => new {
                        p.Id,
                        p.Nombre,
                        Destino = p.Destino.NomDestino,
                        p.PrecioTotal,
                        Vuelo = p.Vuelo != null ? p.Vuelo.Compañia : "Sin vuelo",
                        Hotel = p.Hotel != null ? p.Hotel.Nombre : "Sin hotel",
                        Seguro = p.Seguro != null ? p.Seguro.Tipo : "Sin seguro"
                    }).ToList();

                if (!resultados.Any()) return NotFound();
                return Ok(resultados);
            }

            [HttpGet]
            [Route("api/PaqueteTuristico/buscar-por-nombre-precio")]
            public IHttpActionResult BuscarPorNombreYPrecio(string nombre, double precioMinimo)
            {
                var resultados = db.PaqueteTuristicos
                    .Where(p => p.Nombre.Contains(nombre) && p.PrecioTotal >= precioMinimo)
                    .OrderBy(p => p.PrecioTotal)
                    .Select(p => new {
                        p.Id,
                        p.Nombre,
                        p.PrecioTotal,
                        FechaExpiracion = p.FechaExpiracion.ToString("yyyy-MM-dd"),
                        Estado = p.Estado ? "Activo" : "Inactivo"
                    }).ToList();

                if (!resultados.Any()) return NotFound();
                return Ok(resultados);
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                base.Dispose(disposing);
            }
        }
    }
