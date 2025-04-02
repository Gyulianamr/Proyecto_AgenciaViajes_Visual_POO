using System;

namespace AgenciadeViajes.Models
{
    public class Paquete_Turistico
    {
        private int _idPaquete;
        private string _nombre;
        private Destino _destino;
        private double _precioTotal;
        private DateTime _fechaExpiracion;
        private int duracion_Dias;
        private bool _estado;

        public int DestinoId { get; set; }  // Agregar la clave foránea correcta
        public virtual Destino Destino { get; set; }

        public int? VueloId { get; set; }
        public virtual Vuelo Vuelo { get; set; }

        public int? HotelId { get; set; }
        public virtual Hotel Hotel { get; set; }

        public int? SeguroId { get; set; }
        public virtual Seguro Seguro { get; set; }

        public int? GuiaTuristicoId { get; set; }
        public virtual GuiaTuristico GuiaTuristico { get; set; }

        public int? ActividadesId { get; set; }
        public virtual Actividades Actividades { get; set; }
        public int Duracion_Dias {
            get { return duracion_Dias; }
            set { duracion_Dias = value; } }

        // Constructor vacío
        public Paquete_Turistico() { }

        // Constructor con parámetros
        public Paquete_Turistico(
            int idPaquete,
            string nombre,
            Destino destino,
            double precioTotal,
            DateTime fechaExpiracion,
            bool estado,
            Vuelo vuelo = null,
            Hotel hotel = null,
            Seguro seguro = null,
            GuiaTuristico guiaTuristico = null,
            Actividades actividades = null)
        {
            Id = idPaquete;
            Nombre = nombre;
            Destino = destino;
            PrecioTotal = precioTotal;
            FechaExpiracion = fechaExpiracion;
            Estado = estado;
            Vuelo = vuelo;
            Hotel = hotel;
            Seguro = seguro;
            GuiaTuristico = guiaTuristico;
            Actividades = actividades;
        }

        // Propiedad Id
        public int Id
        {
            get { return _idPaquete; }
            set
            {
                _idPaquete = value;
            }
        }

        // Propiedad Nombre
        public string Nombre
        {
            get { return _nombre; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre no puede estar vacío");
                if (value.Length < 3 || value.Length > 100)
                    throw new ArgumentException("El nombre debe tener entre 3 y 100 caracteres");
                _nombre = value;
            }
        }

     

        // Propiedad PrecioTotal
        public double PrecioTotal
        {
            get { return _precioTotal; }
            set
            {
                
                _precioTotal = value;
            }
        }

        // Propiedad FechaExpiracion
        public DateTime FechaExpiracion
        {
            get { return _fechaExpiracion; }
            set
            {
                if (value <= DateTime.Now)
                    throw new ArgumentException("La fecha de expiración debe ser futura");
                _fechaExpiracion = value;
            }
        }

        // Propiedad Estado
        public bool Estado
        {
            get { return _estado; }
            set { _estado = value; }
        }

        // Método para calcular el costo total
        public double CalcularCostoTotal()
        {
            double total = 0;

            if (Vuelo != null)
                total += Vuelo.Precio;

            if (Hotel != null)
                total += Hotel.Tipohabitacion.PrecioPorNoche*Duracion_Dias;

            if (Seguro != null)
                total += Seguro.Precio;

            if (GuiaTuristico != null)
                total += GuiaTuristico.Salario;

            if (Actividades != null)
                total += Actividades.PrecioHora;

            return total;
        }
    }
}