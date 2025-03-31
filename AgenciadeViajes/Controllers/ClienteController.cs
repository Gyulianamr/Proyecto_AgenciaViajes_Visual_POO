using AgenciadeViajes.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Http;


namespace AgenciadeViajes.Controllers
{

    public class ClienteController : ApiController
    {
        private Proyectodb db = new Proyectodb();

        // GET: api/Cliente
        /// <summary>
        /// Obtiene la lista completa de clientes
        /// </summary>
        /// <returns>Lista de todos los clientes registrados</returns>
        public IEnumerable<Cliente> Get()
        {
            return db.Clientes;
        }

        // GET: api/Cliente/5
        /// <summary>
        /// Busca un cliente por su ID
        /// </summary>
        /// <param name="id">ID del cliente a buscar</param>
        /// <returns>
        /// Cliente encontrado o NotFound si no existe
        /// </returns>
        public IHttpActionResult GetBuscar(int id)
        {
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return Ok(cliente);
        }

        // POST: api/Cliente
        /// <summary>
        /// Crea un nuevo cliente
        /// </summary>
        /// <param name="cliente">Datos del cliente a crear</param>
        /// <returns>
        /// Cliente creado con su ID generado
        /// </returns>
        /// POST: api/Clientes
        /// <summary>
        /// Crea un nuevo cliente
        /// </summary>
        public IHttpActionResult Post(Cliente cliente)
        {
            if (cliente == null)
            {
                return BadRequest("El cliente no puede estar vacío.");
            }

            try
            {
            
             

                if (string.IsNullOrWhiteSpace(cliente.Ciudad))
                {
                    return BadRequest("La ciudad del cliente es obligatoria.");
                }

                if (cliente.Ciudad.Length < 2 || cliente.Ciudad.Length > 50)
                {
                    return BadRequest("La ciudad debe tener entre 2 y 50 caracteres.");
                }

                // Agregar el cliente a la base de datos
                db.Clientes.Add(cliente);
                db.SaveChanges();

                // Retornar el cliente creado con su ID
                return CreatedAtRoute("DefaultApi", new { id = cliente.Id }, cliente);
            }
            catch (Exception ex)
            {
                // Manejo de errores generales
                return InternalServerError(new Exception("Ocurrió un error inesperado al crear el cliente.", ex));
            }
        }

        // PUT: api/Cliente/5
        /// <summary>
        /// Actualiza los datos de un cliente existente
        /// </summary>
        /// <param name="id">ID del cliente a modificar</param>
        /// <param name="cliente">Datos actualizados del cliente</param>
        /// <returns>
        /// Cliente modificado o NotFound si no existe
        /// </returns>
        public IHttpActionResult Put(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest("El ID del cliente no coincide con la URL");
            }

            db.Entry(cliente).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(cliente);
        }

        // DELETE: api/Cliente/5
        /// <summary>
        /// Elimina un cliente por su ID
        /// </summary>
        /// <param name="id">ID del cliente a eliminar</param>
        /// <returns>
        /// Cliente eliminado o NotFound si no existe
        /// </returns>
        public IHttpActionResult Delete(int id)
        {
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return NotFound();
            }

            db.Clientes.Remove(cliente);
            db.SaveChanges();
            return Ok(cliente);
        }
    }
}
