using System;

namespace AgenciadeViajes.Models
{
    public class Hotel
    {
        private int _id;
        private string _nombre;
        private int _estrellas;
        private bool _desayunoIncluido;
        private string _direccion;
        private double _precio;

        public Hotel() { }

        public Hotel(int id, string nombre, double precio, int estrellas,
                   bool desayunoIncluido, string direccion)
        {
            Id = id;
            Nombre = nombre;
            Precio = precio;
            Estrellas = estrellas;
            DesayunoIncluido = desayunoIncluido;
            Direccion = direccion;
        }

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
            }
        }

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

        public int Estrellas
        {
            get { return _estrellas; }
            set
            {
                if (value < 1 || value > 5)
                    throw new ArgumentException("Las estrellas deben estar entre 1 y 5");
                _estrellas = value;
            }
        }

        public bool DesayunoIncluido
        {
            get { return _desayunoIncluido; }
            set { _desayunoIncluido = value; }
        }

        public string Direccion
        {
            get { return _direccion; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("La dirección no puede estar vacía");

                if (value.Length < 10 || value.Length > 200)
                    throw new ArgumentException("La dirección debe tener entre 10 y 200 caracteres");

                _direccion = value;
            }
        }
    }
}