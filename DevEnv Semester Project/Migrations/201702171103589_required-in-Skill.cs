namespace DevEnv_Semester_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requiredinSkill : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Skills", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Skills", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Skills", "Description", c => c.String());
            AlterColumn("dbo.Skills", "Name", c => c.String());
        }
    }
}
