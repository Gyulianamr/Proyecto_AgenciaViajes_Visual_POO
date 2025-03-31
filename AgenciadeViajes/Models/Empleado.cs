using System;

namespace AgenciadeViajes.Models
{
    public abstract class Empleado : Persona
    {
        private int _id;
        private DateTime _fechacontratacion;
        private TimeSpan _horallegada;
        private TimeSpan _horasalida;
        private double _salario;

        public Empleado() { }

        public Empleado( DateTime fechacontratacion, TimeSpan horallegada,
                      TimeSpan horasalida, double salario, int idpersona, string nombre,
                      string apellido, string telefono, string correo)
            : base(idpersona, nombre, apellido, telefono, correo)
        {
           
            FechaContratacion = fechacontratacion;
            HoraLlegada = horallegada;
            HoraSalida = horasalida;
            Salario = salario;
        }

       

        public DateTime FechaContratacion
        {
            get { return _fechacontratacion; }
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("La fecha de contratación no puede ser futura");
                _fechacontratacion = value;
            }
        }

        public TimeSpan HoraLlegada
        {
            get { return _horallegada; }
            set
            {
                if (HoraSalida != default && value > HoraSalida)
                    throw new ArgumentException("La hora de llegada debe ser anterior a la de salida");
                _horallegada = value;
            }
        }

        public TimeSpan HoraSalida
        {
            get { return _horasalida; }
            set
            {
                if (HoraLlegada != default && value <= HoraLlegada)
                    throw new ArgumentException("La hora de salida debe ser posterior a la de llegada");
                _horasalida = value;
            }
        }

        public double Salario
        {
            get { return _salario; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("El salario no puede ser negativo");
                _salario = value;
            }
        }
    }
}
