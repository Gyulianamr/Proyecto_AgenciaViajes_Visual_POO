using System;



namespace AgenciadeViajes.Models
{
    public class Factura
    {
        private int _idPago;
        private Reservacion _reservacion;
        private DateTime _fechaPago;
        public double Saldopendiente { get; set; }
        private double _montoPagado;
        private Metodo_Pago _metodoPago; 
        private string _estado;

        // Constructor vacío
        public Factura() { }

     
        public Factura(int idPago, Reservacion reservacion, DateTime fechaPago, double montoPagado, Metodo_Pago metodoPago, string estado, double saldo)
        {
            Id = idPago;
            Reservacion = reservacion;
            FechaPago = fechaPago;
            MontoPagado = montoPagado;
            MetodoPago = metodoPago;
            Estado = estado;
            Saldopendiente = saldo;
        }

        
        public int Id
        {
            get { return _idPago; }
            set
            {
                _idPago = value;
            }
        }

       
        public Reservacion Reservacion
        {
            get { return _reservacion; }
            set
            {
                
                _reservacion = value;
            }
        }

        
        public DateTime FechaPago
        {
            get { return _fechaPago; }
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("La fecha de pago no puede ser futura");
                _fechaPago = value;
            }
        }

        
        public double MontoPagado
        {
            get { return _montoPagado; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("El monto pagado debe ser mayor a 0");
                _montoPagado = value;
            }
        }

        
        public Metodo_Pago MetodoPago
        {
            get { return _metodoPago; }
            set
            {
                if (value == null)
                    throw new ArgumentException("El método de pago no puede ser nulo");
                _metodoPago = value;
            }
        }

        
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

        
        public bool EsPagoCompleto()
        {
            return MontoPagado >= Reservacion.CalcularSaldoPendiente();
        }


    }
}