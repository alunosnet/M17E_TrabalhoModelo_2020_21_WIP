namespace M17E_TrabalhoModelo_2020_21_WIP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userspassword : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "password", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "password", c => c.String(nullable: false));
        }
    }
}
