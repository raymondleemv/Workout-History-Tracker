namespace WorkoutHistoryApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamedWorkouHistorytabletoWorkout : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WorkoutHistories", "ExerciseID", "dbo.Exercises");
            DropIndex("dbo.WorkoutHistories", new[] { "ExerciseID" });
            CreateTable(
                "dbo.Workouts",
                c => new
                    {
                        WorkoutID = c.Int(nullable: false, identity: true),
                        Date = c.String(),
                        ExerciseID = c.Int(nullable: false),
                        Weight = c.Int(nullable: false),
                        Reps = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.WorkoutID)
                .ForeignKey("dbo.Exercises", t => t.ExerciseID, cascadeDelete: true)
                .Index(t => t.ExerciseID);
            
            DropTable("dbo.WorkoutHistories");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.WorkoutHistories",
                c => new
                    {
                        WorkoutHistoryID = c.Int(nullable: false, identity: true),
                        Date = c.String(),
                        ExerciseID = c.Int(nullable: false),
                        Weight = c.Int(nullable: false),
                        Reps = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.WorkoutHistoryID);
            
            DropForeignKey("dbo.Workouts", "ExerciseID", "dbo.Exercises");
            DropIndex("dbo.Workouts", new[] { "ExerciseID" });
            DropTable("dbo.Workouts");
            CreateIndex("dbo.WorkoutHistories", "ExerciseID");
            AddForeignKey("dbo.WorkoutHistories", "ExerciseID", "dbo.Exercises", "ExerciseID", cascadeDelete: true);
        }
    }
}
