using System;
namespace AgenciadeViajes.Models
{
    public class Reservacion
    {
        private int _idReservacion;
        private Cotizacion _cotizacion;
        private DateTime _fechaReservacion;
        private string _estado; // Ej: Confirmada, Cancelada, En Proceso.
        private DateTime _fechaViaje;
        private DateTime _fechaRegreso;
        private double _montoPagado;

        // Constructor vacío
        public Reservacion() { }

        // Constructor con parámetros
        public Reservacion(int idReservacion, Cotizacion cotizacion, DateTime fechaReservacion, string estado, DateTime fechaViaje, DateTime fechaRegreso, double montoPagado)
        {
            Id = idReservacion;
            Cotizacion = cotizacion;
            FechaReservacion = fechaReservacion;
            Estado = estado;
            FechaViaje = fechaViaje;
            FechaRegreso = fechaRegreso;
            MontoPagado = montoPagado;
        }

        // Propiedad IdReservacion
        public int Id
        {
            get { return _idReservacion; }
            set
            {
               
                _idReservacion = value;
            }
        }

        // Propiedad Cotizacion
        public Cotizacion Cotizacion
        {
            get { return _cotizacion; }
            set
            {
                if (value == null)
                    throw new ArgumentException("La cotización no puede ser nula");
                _cotizacion = value;
            }
        }

        // Propiedad FechaReservacion
        public DateTime FechaReservacion
        {
            get { return _fechaReservacion; }
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("La fecha de reservación no puede ser futura");
                _fechaReservacion = value;
            }
        }

        // Propiedad Estado
        public string Estado
        {
            get { return _estado; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El estado no puede estar vacío");
                _estado = value;
            }
        }

        // Propiedad FechaViaje
        public DateTime FechaViaje
        {
            get { return _fechaViaje; }
            set
            {
                if (value < DateTime.Now)
                    throw new ArgumentException("La fecha de viaje debe ser futura");
                _fechaViaje = value;
            }
        }

        // Propiedad FechaRegreso
        public DateTime FechaRegreso
        {
            get { return _fechaRegreso; }
            set
            {
                if (value <= FechaViaje)
                    throw new ArgumentException("La fecha de regreso debe ser posterior a la fecha de viaje");
                _fechaRegreso = value;
            }
        }

        // Propiedad MontoPagado
        public double MontoPagado
        {
            get { return _montoPagado; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("El monto pagado no puede ser negativo");
                _montoPagado = value;
            }
        }

        // Método para calcular saldo pendiente
        public double CalcularSaldoPendiente()
        {
            if (Cotizacion == null)
            {
                throw new ArgumentNullException("Cotizacion", "La cotización no puede ser nula.");
            }

            if (Cotizacion.CostoTotal < 0)
            {
                throw new ArgumentException("El costo total no puede ser negativo.");
            }

            if (MontoPagado < 0)
            {
                throw new ArgumentException("El monto pagado no puede ser negativo.");
            }

            return Cotizacion.CostoTotal - MontoPagado;
        }

        // Método para verificar si la reservación está pagada
        public bool EsPagada()
        {
            return CalcularSaldoPendiente() == 0;
        }

        // Método para generar descripción de la reservación
    }
}

