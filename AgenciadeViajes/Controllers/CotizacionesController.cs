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

        /// <summary>
        /// obtiene las cotizaciones disponibles
        /// </summary>
        /// <returns>Muestra todas las listas de cotizaciones disponibles</returns>
       
        public IHttpActionResult Get()
        {
            var vercoti = from C in db.Cotizaciones
                          join A in db.AgenteViajes
                          on C.Agente_Responsable.Id equals A.Id
                          join P in db.PaqueteTuristicos
                          on C.Paquete.Id equals P.Id
                          join Clien in db.Clientes
                          on C.Cliente.Id equals Clien.Id
                          select new
                          {
                              IdCotizacion = C.Id,
                              cantidad_Personas= C.Cantidad_Personas,
                              cliente= Clien.Nombre+" "+ Clien.Apellido, 
                              Agente_Responsable = A.Nombre + " " + A.Apellido,
                              Nombre_paquete = P.Nombre,
                              destino = P.Destino.NomDestino,
                              FechaCotizacion= C.FechaCotizacion,
                              CostoTotal= C.CostoTotal,
                          };
            
                         return Ok(vercoti);
        }


        // GET: api/Cotizacion/5
        /// <summary>
        /// Obtiene todas las cotizaciones disponibles por su id
        /// </summary>
        /// <param name="id">Busqueda por id de cotizacion</param>
        /// <returns>Lista de cotizacion por id</returns>
        public IHttpActionResult Get(int id)
        {
            var vercoti = from C in db.Cotizaciones
                          where C.Id == id
                          join A in db.AgenteViajes
                          on C.Agente_Responsable.Id equals A.Id
                          join P in db.PaqueteTuristicos
                          on C.Paquete.Id equals P.Id
                          join Clien in db.Clientes
                          on C.Cliente.Id equals Clien.Id
                          select new
                          {
                              IdCotizacion = C.Id,
                              cantidad_Personas = C.Cantidad_Personas,
                              cliente = Clien.Nombre + " " + Clien.Apellido,
                              Agente_Responsable = A.Nombre + " " + A.Apellido,
                              Nombre_paquete = P.Nombre,
                              destino = P.Destino.NomDestino,
                              FechaCotizacion = C.FechaCotizacion,
                              CostoTotal = C.CostoTotal,
                          };

            return Ok(vercoti);
        }

        // POST: api/Cotizacion
        /// <summary>
        /// Agrega una cotizacion
        /// </summary>
        /// <param name="cotizacion"></param>
        /// <returns>Agrega una cotizacion a la base de datos</returns>
        public IHttpActionResult Post([FromBody] Cotizacion cotizacion)
        {
            try
            {
                
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

        // PUT: api/Cotizacion/5
        /// <summary>
        /// Actualiza una cotizacion por medio del id
        /// </summary>
        /// <param name="cotizacion"></param>
        /// <returns>Actualizacion de cotizacion ya existentes</returns>
        public IHttpActionResult Put(Cotizacion cotizacion)
        {
            try
            {
                var existente = db.Cotizaciones.Find(cotizacion.Id);
                if (existente == null) return NotFound();

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

        // DELETE: api/Cotizacion/5
        /// <summary>
        /// Elimina la cotizacion por su id
        /// </summary>
        /// <param name="id">Busca por id para eliminar una cotizacion</param>
        /// <returns></returns>
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
