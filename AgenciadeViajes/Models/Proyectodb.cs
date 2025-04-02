using System.Data.Entity;

namespace AgenciadeViajes.Models
{
    public class Proyectodb : DbContext
    {
        public Proyectodb() : base("MyDbConnectionString")
        {
        }

        public DbSet<Factura> Factura { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Actividades> Actividades { get; set; }
        public DbSet<Destino> Destinos { get; set; }
        public DbSet<Hotel> Hotel { get; set; }
        public DbSet<Metodo_Pago> MetododePagos { get; set; }
        public DbSet<Paquete_Turistico> PaqueteTuristicos { get; set; }
        public DbSet<Reservacion> Reservas { get; set; }
        public DbSet<Seguro> Seguros { get; set; }
        public DbSet<Vuelo> Vuelos { get; set; }
        public DbSet<AgentedeViaje> AgenteViajes { get; set; }
        public DbSet<GuiaTuristico> GuiaTuristicos { get; set; }
        public DbSet<Historial_ReservasClientes> HistorialClientes { get; set; }
        public DbSet<Cotizacion> Cotizaciones { get; set; }
        public DbSet<TipoHabitacion> TiposdeHabitacion {  get; set; }


    }
}