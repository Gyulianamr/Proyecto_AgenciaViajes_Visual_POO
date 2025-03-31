using System;

namespace AgenciadeViajes.Models
{
    public class Vuelo
    {
        private int _idVuelo;
        private string _tipo;
        private Destino _origen;
        private Destino _destino;
        private string _compañia;
        private TimeSpan _horaSalida;
        private TimeSpan _horaLlegada;
        private int _capacidad;
        private double _precio;

        public Vuelo() { }


        public Vuelo(int idVuelo, string tipo, string compañia, Destino origen,
                     Destino destino, TimeSpan horaSalida, TimeSpan horaLlegada,
                     int capacidad, double precio)
        {
            Id = idVuelo;
            Tipo = tipo;
            Compañia = compañia;
            Origen = origen;
            Destino = destino;
            HoraSalida = horaSalida;
            HoraLlegada = horaLlegada;
            Capacidad = capacidad;
            Precio = precio;
        }


        public int Id
        {
            get { return _idVuelo; }
            set
            {
                
                _idVuelo = value;
            }
        }


        public string Tipo
        {
            get { return _tipo; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El tipo no puede estar vacío");
                if (value.Length < 3 || value.Length > 50)
                    throw new ArgumentException("El tipo debe tener entre 3 y 50 caracteres");
                _tipo = value;
            }
        }

        public string Compañia
        {
            get { return _compañia; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("La compañía no puede estar vacía");
                _compañia = value;
            }
        }


        public Destino Origen
        {
            get { return _origen; }
            set
            {
                if (value == null)
                    throw new ArgumentException("El origen no puede ser nulo");
                _origen = value;
            }
        }


        public Destino Destino
        {
            get { return _destino; }
            set
            {
                if (value == null)
                    throw new ArgumentException("El destino no puede ser nulo");
                _destino = value;
            }
        }


        public TimeSpan HoraSalida
        {
            get { return _horaSalida; }
            set
            {
                
                _horaSalida = value;
            }
        }


        public TimeSpan HoraLlegada
        {
            get { return _horaLlegada; }
            set
            {
                
                _horaLlegada = value;
            }
        }


        public int Capacidad
        {
            get { return _capacidad; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("La capacidad debe ser mayor a 0");
                _capacidad = value;
            }
        }


        public double Precio
        {
            get { return _precio; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("El precio no puede ser negativo");
                _precio = value;
            }
        }

        public object Nombre { get; internal set; }
    }
}