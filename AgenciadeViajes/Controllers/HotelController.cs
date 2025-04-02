using AgenciadeViajes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AgenciadeViajes.Controllers
{
    public class HotelController : ApiController
    {
        private Proyectodb db = new Proyectodb();

        // GET: api/Hotel
        /// <summary>
        /// Obtiene todos los hoteles registrados
        /// </summary>
        /// <returns>Lista de todos los hoteles</returns>
        public IHttpActionResult Get()
        {
            var verhotel = from H in db.Hotel
                           join Th in db.TiposdeHabitacion
                           on H.Id equals Th.Id
                           select new
                           {
                               HotelId = H.Id,
                               NombreHotel= H.Nombre,
                               NombreHabitacion = Th.NombreHabitacion,
                               PrecioPornoche= Th.PrecioPorNoche
                           };


            return Ok(verhotel.ToList());
        }

        // GET: api/Hotel/5
        /// <summary>
        /// Busca un hotel por su ID
        /// </summary>
        /// <returns>lista de hotel por id</returns>
        public IHttpActionResult GetBuscar(int id)
        {
            Hotel hotel = db.Hotel.Find(id);
            if (hotel == null) return NotFound();
            return Ok(hotel);
        }

        // POST: api/Hotel
        /// <summary>
        /// Crea un nuevo hotel
        /// </summary>
        /// <returns>Agrega un hotel</returns>
        public IHttpActionResult Post(Hotel hotel)
        {
            try
            {
                TipoHabitacion tipohabitacion = db.TiposdeHabitacion.Find(hotel.Tipohabitacion.Id);
                if (tipohabitacion == null)
                {
                    return BadRequest("La habitacion no se encontro");
                }
                hotel.Tipohabitacion = tipohabitacion;
                db.Hotel.Add(hotel);
                db.SaveChanges();
                return CreatedAtRoute("DefaultApi", new { id = hotel.Id }, hotel);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Hotel/5
        /// <summary>
        /// Actualiza un hotel existente
        /// </summary>
        /// <returns>Actualizacion de hoteles</returns>
        public IHttpActionResult Put(int id, Hotel hotel)
        {
            try
            {
                if (id != hotel.Id) return BadRequest("IDs no coinciden");

                var hotelExistente = db.Hotel.Find(id);
                if (hotelExistente == null) return NotFound();
                TipoHabitacion tipohabitacion = db.TiposdeHabitacion.Find(hotel.Tipohabitacion.Id);
                if (tipohabitacion == null)
                {
                    return BadRequest("La habitacion no se encontro");
                }
                hotel.Tipohabitacion = tipohabitacion;

                // Actualizar propiedades
                hotelExistente.Nombre = hotel.Nombre;
                hotelExistente.Tipohabitacion.PrecioPorNoche = hotel.Tipohabitacion.PrecioPorNoche;
                hotelExistente.Estrellas = hotel.Estrellas;
                hotelExistente.Direccion = hotel.Direccion;


                db.SaveChanges();
                return Ok(hotelExistente);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Hotel/5
        /// <summary>
        /// Elimina un hotel por su ID
        /// </summary>
        /// <returns>Elimina Hoteles</returns>
        public IHttpActionResult Delete(int id)
        {
            Hotel hotel = db.Hotel.Find(id);
            if (hotel == null) return NotFound();

            db.Hotel.Remove(hotel);
            db.SaveChanges();
            return Ok(hotel);
        }
    }
}

