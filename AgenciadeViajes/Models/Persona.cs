using System;
using System.Text.RegularExpressions;

namespace AgenciadeViajes.Models
{

    public abstract class Persona
    {
        private int _id;
        private string _nombre;
        private string _apellido;
        private string _telefono;
        private string _correo;

        public Persona() { }

        public Persona(int idpersona, string nombre, string apellido, string telefono, string correo)
        {
            Id = idpersona;
            Nombre = nombre;
            Apellido = apellido;
            Telefono = telefono;
            Correo = correo;
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

                if (value.Length < 2 || value.Length > 50)
                    throw new ArgumentException("El nombre debe tener entre 2 y 50 caracteres");

                _nombre = value;
            }
        }

        public string Apellido
        {
            get { return _apellido; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El apellido no puede estar vacío");

                if (value.Length < 2 || value.Length > 50)
                    throw new ArgumentException("El apellido debe tener entre 2 y 50 caracteres");

                _apellido = value;
            }
        }

        public string Telefono
        {
            get { return _telefono; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El teléfono no puede estar vacío");

                if (!Regex.IsMatch(value, @"^\d{8}$"))
                    throw new ArgumentException("El teléfono debe contener exactamente 8 dígitos numéricos");

                _telefono = value;
            }
        }

        public string Correo
        {
            get { return _correo; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El correo electrónico no puede estar vacío");

                if (!Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    throw new ArgumentException("Formato de correo electrónico no válido");

                _correo = value;
            }
        }
    }
}