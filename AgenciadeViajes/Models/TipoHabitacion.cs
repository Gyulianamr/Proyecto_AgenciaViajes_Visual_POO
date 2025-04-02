using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgenciadeViajes.Models
{
    public class TipoHabitacion
    {
        private int _id;
        private string _nombreHabitacion;
        private string _descripcion;
        private double _precioPorNoche;
        private bool _desayunoIncluido;
        public TipoHabitacion()
        {
        }

        public TipoHabitacion( int id, string nombreHabitacion, string descripcion, double precioPorNoche)
        {
     
            Id = id;
            NombreHabitacion = nombreHabitacion;
            Descripcion = descripcion;
            PrecioPorNoche = precioPorNoche;
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string NombreHabitacion
        {
            get { return _nombreHabitacion; }
            set { _nombreHabitacion = value; }
        }

        public string Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }

        public double PrecioPorNoche
        {
            get { return _precioPorNoche; }
            set { _precioPorNoche = value; }
        }

        public bool DesayunoIncluido
        {
            get { return _desayunoIncluido; }
            set { _desayunoIncluido = value; }
        }

    }
}