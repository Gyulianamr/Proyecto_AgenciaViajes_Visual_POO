using System;

namespace AgenciadeViajes.Models
{
    public class Historial_ReservasClientes
    {
        private int _idHistorial;
        private Cliente _idCliente; // Clave foránea que relaciona el historial con el cliente
        private Reservacion _reservacion; // Detalle de la reservación

        // Constructor vacío
        public Historial_ReservasClientes() { }

        // Constructor con parámetros
        public Historial_ReservasClientes(int idHistorial, Cliente idCliente, Reservacion reservacion)
        {
            Id = idHistorial;
            IdCliente = idCliente;
            Reservacion = reservacion;
        }

        // Propiedad IdHistorial
        public int Id
        {
            get { return _idHistorial; }
            set
            {
                _idHistorial = value;
            }
        }

        // Propiedad IdCliente
        public Cliente IdCliente
        {
            get { return _idCliente; }
            set
            {
                _idCliente = value;
            }
        }

        // Propiedad Reservacion
        public Reservacion Reservacion
        {
            get { return _reservacion; }
            set
            {
                if (value == null)
                    throw new ArgumentException("La reservación no puede ser nula");
                _reservacion = value;
            }
        }

    }
}
