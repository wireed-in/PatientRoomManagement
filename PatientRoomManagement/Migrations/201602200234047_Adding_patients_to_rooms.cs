namespace PatientRoomManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Adding_patients_to_rooms : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Patients", "Room_Id", c => c.Int());
            CreateIndex("dbo.Patients", "Room_Id");
            AddForeignKey("dbo.Patients", "Room_Id", "dbo.Rooms", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Patients", "Room_Id", "dbo.Rooms");
            DropIndex("dbo.Patients", new[] { "Room_Id" });
            DropColumn("dbo.Patients", "Room_Id");
        }
    }
}
