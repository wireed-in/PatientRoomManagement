namespace PatientRoomManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AssignmentDates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assignments", "SignInDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Assignments", "SignOutDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assignments", "SignOutDate");
            DropColumn("dbo.Assignments", "SignInDate");
        }
    }
}
