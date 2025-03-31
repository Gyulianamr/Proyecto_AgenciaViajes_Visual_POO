using AgenciadeViajes.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace AgenciadeViajes.Controllers
{

    public class VueloController : ApiController
    {
        private Proyectodb db = new Proyectodb();

        // GET: api/Transporte
        /// <summary>
        /// Obtiene todos los transportes registrados
        /// </summary>
        public IEnumerable<Vuelo> Get()
        {
            return db.Vuelos;
        }

        // GET: api/Transporte/5
        /// <summary>
        /// Busca transporte por ID
        /// </summary>
        public IHttpActionResult GetBuscar(int id)
        {
            Vuelo vuelo = db.Vuelos.Find(id);
            if (vuelo == null) return NotFound();
            return Ok(vuelo);
        }

        // POST: api/Transporte
        /// <summary>
        /// Crea nuevo transporte
        /// </summary>
        public IHttpActionResult Post(Vuelo transporte)
        {
            try
            { Destino destinosalida= db.Destinos.Find(transporte.Origen.Id);
                if (destinosalida == null) return NotFound();
                Destino destinollegada = db.Destinos.Find(transporte.Destino.Id);
                if (destinollegada == null) return NotFound();

                transporte.Origen = destinosalida;
                transporte.Destino = destinollegada;

                db.Vuelos.Add(transporte);
                db.SaveChanges();
                return CreatedAtRoute("DefaultApi", new { id = transporte.Id }, transporte);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Transporte/5
        /// <summary>
        /// Actualiza transporte existente
        /// </summary>
        public IHttpActionResult Put(int id, Vuelo transporte)
        {
            try
            {
                if (id != transporte.Id)
                    return BadRequest("ID no coincide");

                var transporteExistente = db.Vuelos.Find(id);
                if (transporteExistente == null)
                    return NotFound();

                // Actualizar propiedades
                transporteExistente.Nombre = transporte.Nombre;
                transporteExistente.Precio = transporte.Precio;
                transporteExistente.Tipo = transporte.Tipo;
                transporteExistente.Compañia = transporte.Compañia;
                transporteExistente.HoraSalida = transporte.HoraSalida;
                transporteExistente.HoraLlegada = transporte.HoraLlegada;
                transporteExistente.Capacidad = transporte.Capacidad;

                db.SaveChanges();
                return Ok(transporteExistente);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Transporte/5
        /// <summary>
        /// Elimina transporte por ID
        /// </summary>
        public IHttpActionResult Delete(int id)
        {
            Vuelo transporte = db.Vuelos.Find(id);
            if (transporte == null) return NotFound();

            db.Vuelos.Remove(transporte);
            db.SaveChanges();
            return Ok(transporte);
        }
    }
}
