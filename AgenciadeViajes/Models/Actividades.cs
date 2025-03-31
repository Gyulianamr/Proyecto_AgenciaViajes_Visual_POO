using System;

namespace AgenciadeViajes.Models
{
    public class Actividades
    {
        private int _idActividad;
        private string _nombre;
        private string _descripcion;
        private int _duracionHoras;
        private bool _requiereReserva;
        private double _precioHora;

        public Actividades() { }

        public Actividades(int id, string nombre, string descripcion, int duracionHoras,
                           bool requiereReserva, double precioHora)
        {
            this.Id = id;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.DuracionHoras = duracionHoras;
            this.RequiereReserva = requiereReserva;
            this.PrecioHora = precioHora;
        }

        public int Id
        {
            get { return _idActividad; }
            set { _idActividad = value; }
        }

        public string Nombre
        {
            get { return _nombre; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre no puede estar vacío.");
                _nombre = value;
            }
        }

        public string Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }

        public int DuracionHoras
        {
            get { return _duracionHoras; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("La duración debe ser mayor o igual a 1.");
                _duracionHoras = value;
            }
        }

        public bool RequiereReserva
        {
            get { return _requiereReserva; }
            set { _requiereReserva = value; }
        }

        public double PrecioHora
        {
            get { return _precioHora; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("El precio por hora debe ser mayor que 0.");
                _precioHora = value;
            }
        }
    }
}