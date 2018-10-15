namespace QuizDemoClasses.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UpdateListAbleTbl : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Questions", "Choice_Id", "dbo.Choices");
            DropForeignKey("dbo.Tests", "TestQuestion_Id", "dbo.TestQuestions");
            DropIndex("dbo.Questions", new[] { "Choice_Id" });
            DropIndex("dbo.Tests", new[] { "TestQuestion_Id" });
            AlterColumn("dbo.Registrations", "Token", c => c.Guid(nullable: false));
            AlterColumn("dbo.Students", "AccessLevel", c => c.Int(nullable: false));
            AlterColumn("dbo.Students", "PhonNo", c => c.String());
            AlterColumn("dbo.Tests", "DurationInMint", c => c.Double(nullable: false));
            CreateIndex("dbo.Choices", "QuestionId");
            CreateIndex("dbo.TestQuestions", "TestId");
            AddForeignKey("dbo.Choices", "QuestionId", "dbo.Questions", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TestQuestions", "TestId", "dbo.Tests", "Id", cascadeDelete: true);
            DropColumn("dbo.Questions", "Choice_Id");
            DropColumn("dbo.Tests", "TestQuestion_Id");
        }

        public override void Down()
        {
            AddColumn("dbo.Tests", "TestQuestion_Id", c => c.Int());
            AddColumn("dbo.Questions", "Choice_Id", c => c.Int());
            DropForeignKey("dbo.TestQuestions", "TestId", "dbo.Tests");
            DropForeignKey("dbo.Choices", "QuestionId", "dbo.Questions");
            DropIndex("dbo.TestQuestions", new[] { "TestId" });
            DropIndex("dbo.Choices", new[] { "QuestionId" });
            AlterColumn("dbo.Tests", "DurationInMint", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Students", "PhonNo", c => c.Long(nullable: false));
            AlterColumn("dbo.Students", "AccessLevel", c => c.String());
            AlterColumn("dbo.Registrations", "Token", c => c.String());
            CreateIndex("dbo.Tests", "TestQuestion_Id");
            CreateIndex("dbo.Questions", "Choice_Id");
            AddForeignKey("dbo.Tests", "TestQuestion_Id", "dbo.TestQuestions", "Id");
            AddForeignKey("dbo.Questions", "Choice_Id", "dbo.Choices", "Id");
        }
    }
}
