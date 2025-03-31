using System;

namespace AgenciadeViajes.Models
{
    public class AgentedeViaje : Empleado
    {
        

        public AgentedeViaje() { }

        public AgentedeViaje(int idpersona, DateTime fechacontratacion,
                            TimeSpan horallegada, TimeSpan horasalida,
                            double salario,  string nombre,
                            string apellido, string telefono, string correo)
            : base( fechacontratacion, horallegada, horasalida,
                  salario, idpersona, nombre, apellido, telefono, correo)
        {
            
        }

     
    }
}