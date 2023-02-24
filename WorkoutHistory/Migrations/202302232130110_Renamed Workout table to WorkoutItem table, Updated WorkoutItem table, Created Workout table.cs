namespace WorkoutHistoryApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamedWorkouttabletoWorkoutItemtableUpdatedWorkoutItemtableCreatedWorkouttable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Workouts", new[] { "ExerciseID" });
            CreateTable(
                "dbo.WorkoutItems",
                c => new
                    {
                        WorkoutItemID = c.Int(nullable: false, identity: true),
                        WorkoutID = c.Int(nullable: false),
                        ExerciseID = c.Int(nullable: false),
                        Weight = c.Int(nullable: false),
                        Reps = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.WorkoutItemID)
                .ForeignKey("dbo.Workouts", t => t.WorkoutID, cascadeDelete: true)
                .Index(t => t.WorkoutID)
                .Index(t => t.ExerciseID);
            
            AddColumn("dbo.Workouts", "WorkoutDate", c => c.String());
            DropColumn("dbo.Workouts", "Date");
            DropColumn("dbo.Workouts", "ExerciseID");
            DropColumn("dbo.Workouts", "Weight");
            DropColumn("dbo.Workouts", "Reps");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Workouts", "Reps", c => c.Int(nullable: false));
            AddColumn("dbo.Workouts", "Weight", c => c.Int(nullable: false));
            AddColumn("dbo.Workouts", "ExerciseID", c => c.Int(nullable: false));
            AddColumn("dbo.Workouts", "Date", c => c.String());
            DropForeignKey("dbo.WorkoutItems", "WorkoutID", "dbo.Workouts");
            DropIndex("dbo.WorkoutItems", new[] { "ExerciseID" });
            DropIndex("dbo.WorkoutItems", new[] { "WorkoutID" });
            DropColumn("dbo.Workouts", "WorkoutDate");
            DropTable("dbo.WorkoutItems");
            CreateIndex("dbo.Workouts", "ExerciseID");
        }
    }
}
