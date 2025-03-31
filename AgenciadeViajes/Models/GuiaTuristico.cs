using System;

namespace AgenciadeViajes.Models
{
    public class GuiaTuristico : Empleado
    {
        private int _id;
        private string _zonaEspecializada;
        private string _idiomas;
        private double _salarioPorHora;

        public GuiaTuristico() { }

        public GuiaTuristico(int idpersona, string zonaEspecializada, string idiomas, double salarioPorHora,
                           int idemp, DateTime fechacontratacion, TimeSpan horallegada,
                           TimeSpan horasalida, double salario,  string nombre,
                           string apellido, string telefono, string correo)
            : base( fechacontratacion, horallegada, horasalida, salario,
                  idpersona, nombre, apellido, telefono, correo)
        {
           
            ZonaEspecializada = zonaEspecializada;
            Idiomas = idiomas;
            SalarioPorHora = salarioPorHora;
        }

      

        public string ZonaEspecializada
        {
            get { return _zonaEspecializada; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("La zona especializada no puede estar vacía");

                if (value.Length < 5 || value.Length > 100)
                    throw new ArgumentException("La zona debe tener entre 5 y 100 caracteres");

                _zonaEspecializada = value;
            }
        }

        public string Idiomas
        {
            get { return _idiomas; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Debe especificar al menos un idioma");

                if (value.Split(',').Length < 1)
                    throw new ArgumentException("Debe indicar mínimo 1 idioma");

                _idiomas = value;
            }
        }

        public double SalarioPorHora
        {
            get { return _salarioPorHora; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("El salario por hora no puede ser negativo");

                if (value > 1000)
                    throw new ArgumentException("El salario por hora no puede exceder 1000");

                _salarioPorHora = value;
            }
        }
    }
}