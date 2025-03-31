using System;

namespace AgenciadeViajes.Models
{
    public class Cliente : Persona
    {
        private int _id;
        private string _ciudad;

        public Cliente() { }

        public Cliente(int id, int idpersona, string nombre, string apellido,
                     string telefono, string correo, string ciudad)
            : base(idpersona, nombre, apellido, telefono, correo)
        {
   
            Ciudad = ciudad;
        }

        
        public string Ciudad
        {
            get { return _ciudad; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("La ciudad no puede estar vacía");
                _ciudad = value;
            }
        }
    }
}