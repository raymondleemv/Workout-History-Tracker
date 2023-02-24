namespace WorkoutHistoryApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateWorkoutHistoryTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkoutHistories", "Weight", c => c.Int(nullable: false));
            AddColumn("dbo.WorkoutHistories", "Reps", c => c.Int(nullable: false));
            DropColumn("dbo.WorkoutHistories", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WorkoutHistories", "Description", c => c.String());
            DropColumn("dbo.WorkoutHistories", "Reps");
            DropColumn("dbo.WorkoutHistories", "Weight");
        }
    }
}
