namespace QuizDemoClasses.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TestQuestions", "Count", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TestQuestions", "Count");
        }
    }
}
