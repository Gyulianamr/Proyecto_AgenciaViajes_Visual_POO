using System;

namespace AgenciadeViajes.Models
{
    public class Cotizacion
    {
        private int _idCotizacion;
        private Cliente _cliente;
        public AgentedeViaje Agente_Responsable { get; set; }
        public int Cantidad_Personas { get; set; }
        private Paquete_Turistico _paquete;
        private DateTime _fechaCotizacion;
        private double _costoTotal;
        



        // Constructor vacío
        public Cotizacion() { }

        // Constructor con parámetros
        public Cotizacion(int idCotizacion, Cliente cliente, AgentedeViaje agente, int personas, Paquete_Turistico paquete, DateTime fechaCotizacion, double Costo)
        {
            Id = idCotizacion;
            Cliente = cliente;
            Agente_Responsable = agente;
            Paquete = paquete;
            FechaCotizacion = fechaCotizacion;
            CostoTotal = Costo;
            Cantidad_Personas = personas;
        }

        // Propiedad IdCotizacion
        public int Id
        {
            get { return _idCotizacion; }
            set
            {
                _idCotizacion = value;
            }
        }

        // Propiedad Cliente
        public Cliente Cliente
        {
            get { return _cliente; }
            set
            {
                if (value == null)
                    throw new ArgumentException("El cliente no puede ser nulo");
                _cliente = value;
            }
        }

        // Propiedad Paquete
        public Paquete_Turistico Paquete
        {
            get { return _paquete; }
            set
            {
                if (value == null)
                    throw new ArgumentException("El paquete turístico no puede ser nulo");
                _paquete = value;
            }
        }

        // Propiedad FechaCotizacion
        public DateTime FechaCotizacion
        {
            get { return _fechaCotizacion; }
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("La fecha de la cotización no puede ser futura");
                _fechaCotizacion = value;
            }
        }

        // Propiedad CostoTotal
        public double CostoTotal
        {
            get { return _costoTotal; }
            set
            {  _costoTotal = value;}
        }

        public double Costo()
        {
            double total = Paquete.PrecioTotal * Cantidad_Personas;
            return total;
        }
    }
}
