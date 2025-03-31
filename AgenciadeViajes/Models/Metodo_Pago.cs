using System;

namespace AgenciadeViajes.Models
{
    public class Metodo_Pago
    {

        private int _idMetodo;
        private string _nombre;
        private string _descripcion;
        private bool _activo;

        public Metodo_Pago() { }

        public Metodo_Pago(int idMetodo, string nombre, string descripcion, bool activo)
        {
            Id = idMetodo;
            Nombre = nombre;
            Descripcion = descripcion;
            Activo = activo;
        }

        public int Id
        {
            get { return _idMetodo; }
            set
            {
              
                _idMetodo = value;
            }
        }

        public string Nombre
        {
            get { return _nombre; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre es requerido");

                if (value.Length < 3 || value.Length > 50)
                    throw new ArgumentException("Nombre debe tener 3-50 caracteres");

                _nombre = value;
            }
        }

        public string Descripcion
        {
            get { return _descripcion; }
            set
            {
                if (value?.Length > 200)
                    throw new ArgumentException("Descripción no puede exceder 200 caracteres");
                _descripcion = value;
            }
        }

        public bool Activo
        {
            get { return _activo; }
            set { _activo = value; }
        }

        public void HabilitarMetodo()
        {
            _activo = true;
        }

        public void DeshabilitarMetodo()
        {
            _activo = false;
        }

        public bool EsMetodoActivo()
        {
            return _activo;
        }

    }
}