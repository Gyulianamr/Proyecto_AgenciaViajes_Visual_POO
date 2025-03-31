using System;

namespace AgenciadeViajes.Models
{
    public class Destino
    {
        private int _idDestino;
        private string _nomdestino;
        private string _pais;
        private string _descrip;
        private string _tipo;
        private string _moneda;
        private bool _reqvisa;

        public Destino() { }

        public Destino(int idDestino, string nomdestino, string pais, string descrip,
                     string tipo, string moneda, bool reqvisa)
        {
            Id = idDestino;
            NomDestino = nomdestino;
            Pais = pais;
            Descripcion = descrip;
            Tipo = tipo;
            Moneda = moneda;
            RequiereVisa = reqvisa;
        }

        public int Id
        {
            get { return _idDestino; }
            set
            {
                _idDestino = value;
            }
        }

        public string NomDestino
        {
            get { return _nomdestino; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre del destino es requerido");

                if (value.Length < 3 || value.Length > 100)
                    throw new ArgumentException("El nombre debe tener entre 3 y 100 caracteres");

                _nomdestino = value;
            }
        }

        public string Pais
        {
            get { return _pais; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El país es requerido");

                if (value.Length < 3 || value.Length > 50)
                    throw new ArgumentException("El país debe tener entre 3 y 50 caracteres");

                _pais = value;
            }
        }

        public string Descripcion
        {
            get { return _descrip; }
            set
            {
                if (value?.Length > 500)
                    throw new ArgumentException("La descripción no puede exceder 500 caracteres");
                _descrip = value;
            }
        }

        public string Tipo
        {
            get { return _tipo; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El tipo de destino es requerido");

                if (value.Length > 50)
                    throw new ArgumentException("El tipo no puede exceder 50 caracteres");

                _tipo = value;
            }
        }

        public string Moneda
        {
            get { return _moneda; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("La moneda es requerida");

                if (value.Length != 3)
                    throw new ArgumentException("La moneda debe ser un código de 3 caracteres");

                _moneda = value;
            }
        }

        public bool RequiereVisa
        {
            get { return _reqvisa; }
            set { _reqvisa = value; }
        }
    }
}