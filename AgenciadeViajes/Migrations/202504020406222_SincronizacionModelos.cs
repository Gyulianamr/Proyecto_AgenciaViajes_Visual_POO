namespace AgenciadeViajes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SincronizacionModelos : DbMigration
    {
        public override void Up()
        {
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.Facturas", "Saldopendiente", c => c.Double(nullable: false));
        }
    }
}
