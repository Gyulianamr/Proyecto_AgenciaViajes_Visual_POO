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

        /// <summary>
        /// Obtiene la lista de todos los paquetes turísticos.
        /// </summary>
        /// <returns>Lista de paquetes turísticos</returns>
        public IHttpActionResult Get()
        {
            var verpaque = from P in db.PaqueteTuristicos
                           select new
                           {
                               Nombre_paquete = P.Nombre,
                               VueloId = P.Vuelo != null ? P.Vuelo.Id : (int?)null,
                               OrigenDestino = P.Vuelo != null && P.Vuelo.Origen != null ? P.Vuelo.Origen.NomDestino : null,
                               DestinoNombre = P.Vuelo != null && P.Vuelo.Destino != null ? P.Vuelo.Destino.NomDestino : null,
                               HotelNombre = P.Hotel != null ? P.Hotel.Nombre : null,
                               ActividadesNombre = P.Actividades != null ? P.Actividades.Nombre : null,
                               GuiaTuristicoNombre = P.GuiaTuristico != null ? P.GuiaTuristico.Nombre : null,
                               SeguroNombre = P.Seguro != null ? P.Seguro.Nombre : null,
                               PrecioTotal = P.PrecioTotal
                           };

            // Verificar si hay resultados
            if (!verpaque.Any())
            {
                return NotFound();
            }

            return Ok(verpaque);
        }

        /// <summary>
        /// Obtiene un paquete turístico por su ID.
        /// </summary>
        /// <param name="id">ID del paquete turístico</param>
        /// <returns>Paquete turístico correspondiente al ID</returns>
        public IHttpActionResult Get(int id)
        {
            var verpaque = from P in db.PaqueteTuristicos
                           where P.Id == id
                           select new
                           {
                               Nombre_paquete = P.Nombre,
                               VueloId = P.Vuelo != null ? P.Vuelo.Id : (int?)null,
                               OrigenDestino = P.Vuelo != null && P.Vuelo.Origen != null ? P.Vuelo.Origen.NomDestino : null,
                               DestinoNombre = P.Vuelo != null && P.Vuelo.Destino != null ? P.Vuelo.Destino.NomDestino : null,
                               HotelNombre = P.Hotel != null ? P.Hotel.Nombre : null,
                               ActividadesNombre = P.Actividades != null ? P.Actividades.Nombre : null,
                               GuiaTuristicoNombre = P.GuiaTuristico != null ? P.GuiaTuristico.Nombre : null,
                               SeguroNombre = P.Seguro != null ? P.Seguro.Nombre : null,
                               PrecioTotal = P.PrecioTotal
                           };

            // Verificar si no hay resultados
            if (!verpaque.Any())
            {
                return NotFound();
            }

            return Ok(verpaque);
        }

        /// <summary>
        /// Crea un nuevo paquete turístico.
        /// </summary>
        /// <param name="paquete">Datos del nuevo paquete turístico</param>
        /// <returns>Resultado de la operación de creación</returns>
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

                var destino = db.Destinos.Find(paquete.DestinoId);
                if (destino == null)
                {
                    return BadRequest("Destino no encontrado");
                }

                // Validar si el vuelo, hotel, seguro, guía turístico y actividades existen
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

                // Asignar las relaciones correspondientes
                paquete.Vuelo = paquete.VueloId.HasValue ? db.Vuelos.Find(paquete.VueloId.Value) : null;
                paquete.Hotel = paquete.HotelId.HasValue ? db.Hotel.Find(paquete.HotelId.Value) : null;
                paquete.Seguro = paquete.SeguroId.HasValue ? db.Seguros.Find(paquete.SeguroId.Value) : null;
                paquete.GuiaTuristico = paquete.GuiaTuristicoId.HasValue ? db.GuiaTuristicos.Find(paquete.GuiaTuristicoId.Value) : null;
                paquete.Actividades = paquete.ActividadesId.HasValue ? db.Actividades.Find(paquete.ActividadesId.Value) : null;
                paquete.Destino = destino;

                // Calcular el precio total
                paquete.PrecioTotal = paquete.CalcularCostoTotal();

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

        /// <summary>
        /// Actualiza un paquete turístico existente.
        /// </summary>
        /// <param name="id">ID del paquete turístico a actualizar</param>
        /// <param name="paquete">Nuevo paquete turístico con los datos actualizados</param>
        /// <returns>Resultado de la operación de actualización</returns>
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

                // Actualizar los campos del paquete
                existente.VueloId = paquete.VueloId;
                existente.HotelId = paquete.HotelId;
                existente.SeguroId = paquete.SeguroId;
                existente.GuiaTuristicoId = paquete.GuiaTuristicoId;
                existente.ActividadesId = paquete.ActividadesId;
                existente.Nombre = paquete.Nombre;
                existente.FechaExpiracion = paquete.FechaExpiracion;
                existente.Estado = paquete.Estado;
                existente.Destino = db.Destinos.Find(paquete.Destino.Id);

                // Calcular el nuevo precio total
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

        /// <summary>
        /// Elimina un paquete turístico por su ID.
        /// </summary>
        /// <param name="id">ID del paquete turístico a eliminar</param>
        /// <returns>Resultado de la operación de eliminación</returns>
        public IHttpActionResult Delete(int id)
        {
            Paquete_Turistico paquete = db.PaqueteTuristicos.Find(id);
            if (paquete == null) return NotFound();

            db.PaqueteTuristicos.Remove(paquete);
            db.SaveChanges();
            return Ok(paquete);
        }

        /// <summary>
        /// Busca paquetes turísticos por destino y precio máximo.
        /// </summary>
        /// <param name="destinoId">ID del destino</param>
        /// <param name="precioMaximo">Precio máximo</param>
        /// <returns>Lista de paquetes turísticos que cumplen con los criterios</returns>
        [HttpGet]
        [Route("api/PaqueteTuristico/buscar-por-destino-precio")]
        public IHttpActionResult BuscarPorDestinoYPrecio(int destinoId, double precioMaximo)
        {
            if (destinoId <= 0)
            {
                return BadRequest("El ID del destino debe ser mayor que cero.");
            }

            if (precioMaximo <= 0)
            {
                return BadRequest("El precio máximo debe ser mayor que cero.");
            }

            var resultados = from p in db.PaqueteTuristicos
                             where p.Destino != null && p.Destino.Id == destinoId && p.PrecioTotal <= precioMaximo
                             orderby p.PrecioTotal
                             select new
                             {
                                 p.Id,
                                 p.Nombre,
                                 Destino = p.Destino != null ? p.Destino.NomDestino : "Destino no especificado",
                                 p.PrecioTotal,
                                 Vuelo = p.Vuelo != null ? p.Vuelo.Compañia : "Sin vuelo",
                                 Hotel = p.Hotel != null ? p.Hotel.Nombre : "Sin hotel",
                                 Seguro = p.Seguro != null ? p.Seguro.Tipo : "Sin seguro"
                             };

            if (!resultados.Any())
            {
                return NotFound();
            }

            return Ok(resultados);
        }

        /// <summary>
        /// Busca paquetes turísticos por nombre y precio mínimo.
        /// </summary>
        /// <param name="nombre">Nombre del paquete turístico</param>
        /// <param name="precioMinimo">Precio mínimo</param>
        /// <returns>Lista de paquetes turísticos que cumplen con los criterios</returns>
        [HttpGet]
        [Route("api/PaqueteTuristico/buscar-por-nombre-precio")]
        public IHttpActionResult BuscarPorNombreYPrecio(string nombre, double precioMinimo)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                return BadRequest("El nombre no puede estar vacío.");
            }

            if (precioMinimo <= 0)
            {
                return BadRequest("El precio mínimo debe ser mayor que cero.");
            }

            var resultados = from p in db.PaqueteTuristicos
                             where p.Nombre.Contains(nombre) && p.PrecioTotal >= precioMinimo
                             orderby p.PrecioTotal
                             select new
                             {
                                 p.Id,
                                 p.Nombre,
                                 p.PrecioTotal,
                                 Vuelo = p.Vuelo != null ? p.Vuelo.Compañia : "Sin vuelo",
                                 Hotel = p.Hotel != null ? p.Hotel.Nombre : "Sin hotel",
                                 Seguro = p.Seguro != null ? p.Seguro.Tipo : "Sin seguro"
                             };

            if (!resultados.Any())
            {
                return NotFound();
            }

            return Ok(resultados);
        }

      
    }

}
