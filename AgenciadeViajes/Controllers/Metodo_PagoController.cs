using AgenciadeViajes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace AgenciadeViajes.Controllers
{
 
    public class Metodo_PagoController : ApiController
    {
        private Proyectodb db = new Proyectodb();

        // GET: api/MetodoPago
        /// <summary>
        /// Obtiene todos los métodos de pago registrados
        /// </summary>
        /// <param name="incluirInactivos">Indica si se deben incluir métodos inactivos (valor predeterminado: false)</param>
        /// <returns>Lista de métodos de pago</returns>
        public IHttpActionResult Get(bool incluirInactivos = false)
        {
            var query = from m in db.MetododePagos
                        where incluirInactivos || m.Activo
                        select m;
            return Ok(query.ToList());
        }

        // GET: api/MetodoPago/5
        /// <summary>
        /// Obtiene un método de pago por su ID
        /// </summary>
        /// <param name="id">ID del método de pago</param>
        /// <returns>Método de pago correspondiente al ID</returns>
        public IHttpActionResult GetById(int id)
        {
            Metodo_Pago metodo = db.MetododePagos.Find(id);
            if (metodo == null) return NotFound();
            return Ok(metodo);
        }

        // POST: api/MetodoPago
        /// <summary>
        /// Crea un nuevo método de pago
        /// </summary>
        /// <param name="metodo">Datos del método de pago a crear</param>
        /// <returns>Método de pago creado</returns>
        public IHttpActionResult Post([FromBody] Metodo_Pago metodo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(metodo.Nombre))
                    return BadRequest("El nombre del método de pago es requerido");

                metodo.Activo = true; // Por defecto se crea como activo
                db.MetododePagos.Add(metodo);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = metodo.Id }, metodo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/MetodoPago/5
        /// <summary>
        /// Actualiza un método de pago existente
        /// </summary>
        /// <param name="id">ID del método de pago a actualizar</param>
        /// <param name="metodo">Datos actualizados del método de pago</param>
        /// <returns>Método de pago actualizado</returns>
        public IHttpActionResult Put(int id, [FromBody] Metodo_Pago metodo)
        {
            try
            {
                if (id != metodo.Id)
                    return BadRequest("ID del método de pago no coincide");

                if (string.IsNullOrWhiteSpace(metodo.Nombre))
                    return BadRequest("El nombre del método de pago es requerido");

                var existente = db.MetododePagos.Find(id);
                if (existente == null) return NotFound();

                existente.Nombre = metodo.Nombre;
                existente.Descripcion = metodo.Descripcion;
                existente.Activo = metodo.Activo;

                db.SaveChanges();
                return Ok(existente);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/MetodoPago/5
        /// <summary>
        /// Elimina lógicamente un método de pago (lo marca como inactivo)
        /// </summary>
        /// <param name="id">ID del método de pago</param>
        /// <returns>Método de pago deshabilitado</returns>
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var metodo = db.MetododePagos.Find(id);
                if (metodo == null) return NotFound();

                metodo.DeshabilitarMetodo();
                db.SaveChanges();

                return Ok(metodo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PATCH: api/MetodoPago/5/habilitar
        /// <summary>
        /// Habilita un método de pago
        /// </summary>
        /// <param name="id">ID del método de pago</param>
        /// <returns>Método de pago habilitado</returns>
        [HttpPatch]
        [Route("api/MetodoPago/{id}/habilitar")]
        public IHttpActionResult Habilitar(int id)
        {
            var metodo = db.MetododePagos.Find(id);
            if (metodo == null) return NotFound();

            metodo.HabilitarMetodo();
            db.SaveChanges();
            return Ok(metodo);
        }

        // GET: api/MetodoPago/5/activo
        /// <summary>
        /// Verifica si un método de pago está activo
        /// </summary>
        /// <param name="id">ID del método de pago</param>
        /// <returns>Estado de actividad del método de pago</returns>
        [HttpGet]
        [Route("api/MetodoPago/{id}/activo")]
        public IHttpActionResult EstaActivo(int id)
        {
            var metodo = db.MetododePagos.Find(id);
            if (metodo == null) return NotFound();

            return Ok(new { Activo = metodo.EsMetodoActivo() });
        }

        // GET: api/MetodoPago/buscar?nombre=credito
        /// <summary>
        /// Busca métodos de pago por coincidencia parcial en el nombre
        /// </summary>
        /// <param name="nombre">Fragmento del nombre a buscar</param>
        /// <returns>Lista de métodos de pago que coinciden con el criterio</returns>
        [HttpGet]
        [Route("api/MetodoPago/buscar")]
        public IHttpActionResult BuscarPorNombre(string nombre)
        {
            var query = from m in db.MetododePagos
                        where m.Nombre.Contains(nombre)
                        orderby m.Nombre
                        select m;

            var resultados = query.ToList();

            if (!resultados.Any()) return NotFound();

            return Ok(resultados);
        }

        // GET: api/MetodoPago/existe?nombre=PayPal
        /// <summary>
        /// Verifica si existe un método de pago con el nombre especificado
        /// </summary>
        /// <param name="nombre">Nombre exacto a verificar</param>
        /// <returns>
        /// True si existe un método con ese nombre, False en caso contrario
        /// </returns>
        [HttpGet]
        [Route("api/MetodoPago/existe")]
        public IHttpActionResult ExisteMetodo(string nombre)
        {
            bool existe = db.MetododePagos
                          .Any(m => m.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));

            return Ok(new { Existe = existe });
        }
    }
}
