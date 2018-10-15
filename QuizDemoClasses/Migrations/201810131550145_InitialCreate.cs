namespace QuizDemoClasses.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Choices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionId = c.Int(nullable: false),
                        Lable = c.String(),
                        Points = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QuestionCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Category = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QuestionDurations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RegistrationId = c.Int(nullable: false),
                        TestQuestionId = c.Int(nullable: false),
                        RequesTime = c.DateTime(nullable: false),
                        LeaveTime = c.DateTime(nullable: false),
                        AnswerTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        QuestionType = c.String(),
                        Questino1 = c.String(),
                        Points = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Choice_Id = c.Int(),
                        QuestionCategory_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Choices", t => t.Choice_Id)
                .ForeignKey("dbo.QuestionCategories", t => t.QuestionCategory_Id)
                .Index(t => t.Choice_Id)
                .Index(t => t.QuestionCategory_Id);
            
            CreateTable(
                "dbo.Registrations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.Int(nullable: false),
                        TestId = c.Int(nullable: false),
                        RegistrationDate = c.DateTime(nullable: false),
                        Token = c.String(),
                        TokenExpireTime = c.DateTime(nullable: false),
                        QuestionDuration_Id = c.Int(),
                        TestPaper_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuestionDurations", t => t.QuestionDuration_Id)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .ForeignKey("dbo.Tests", t => t.TestId, cascadeDelete: true)
                .ForeignKey("dbo.TestPapers", t => t.TestPaper_Id)
                .Index(t => t.StudentId)
                .Index(t => t.TestId)
                .Index(t => t.QuestionDuration_Id)
                .Index(t => t.TestPaper_Id);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        AccessLevel = c.String(),
                        EntryDate = c.DateTime(nullable: false),
                        Email = c.String(),
                        PhonNo = c.Long(nullable: false),
                        PassHash = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        DurationInMint = c.DateTime(nullable: false),
                        TestQuestion_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TestQuestions", t => t.TestQuestion_Id)
                .Index(t => t.TestQuestion_Id);
            
            CreateTable(
                "dbo.TestQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TestId = c.Int(nullable: false),
                        QuestionId = c.Int(nullable: false),
                        QuestionNumber = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        QuestionDuration_Id = c.Int(),
                        TestPaper_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .ForeignKey("dbo.QuestionDurations", t => t.QuestionDuration_Id)
                .ForeignKey("dbo.TestPapers", t => t.TestPaper_Id)
                .Index(t => t.QuestionId)
                .Index(t => t.QuestionDuration_Id)
                .Index(t => t.TestPaper_Id);
            
            CreateTable(
                "dbo.TestPapers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RegistrartionId = c.Int(nullable: false),
                        TestQuestionId = c.Int(nullable: false),
                        ChoiceId = c.Int(nullable: false),
                        Answer = c.String(),
                        MarkScored = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Choices", t => t.ChoiceId, cascadeDelete: true)
                .Index(t => t.ChoiceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Registrations", "TestPaper_Id", "dbo.TestPapers");
            DropForeignKey("dbo.Registrations", "TestId", "dbo.Tests");
            DropForeignKey("dbo.Tests", "TestQuestion_Id", "dbo.TestQuestions");
            DropForeignKey("dbo.TestQuestions", "TestPaper_Id", "dbo.TestPapers");
            DropForeignKey("dbo.TestPapers", "ChoiceId", "dbo.Choices");
            DropForeignKey("dbo.TestQuestions", "QuestionDuration_Id", "dbo.QuestionDurations");
            DropForeignKey("dbo.TestQuestions", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Registrations", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Registrations", "QuestionDuration_Id", "dbo.QuestionDurations");
            DropForeignKey("dbo.Questions", "QuestionCategory_Id", "dbo.QuestionCategories");
            DropForeignKey("dbo.Questions", "Choice_Id", "dbo.Choices");
            DropIndex("dbo.TestPapers", new[] { "ChoiceId" });
            DropIndex("dbo.TestQuestions", new[] { "TestPaper_Id" });
            DropIndex("dbo.TestQuestions", new[] { "QuestionDuration_Id" });
            DropIndex("dbo.TestQuestions", new[] { "QuestionId" });
            DropIndex("dbo.Tests", new[] { "TestQuestion_Id" });
            DropIndex("dbo.Registrations", new[] { "TestPaper_Id" });
            DropIndex("dbo.Registrations", new[] { "QuestionDuration_Id" });
            DropIndex("dbo.Registrations", new[] { "TestId" });
            DropIndex("dbo.Registrations", new[] { "StudentId" });
            DropIndex("dbo.Questions", new[] { "QuestionCategory_Id" });
            DropIndex("dbo.Questions", new[] { "Choice_Id" });
            DropTable("dbo.TestPapers");
            DropTable("dbo.TestQuestions");
            DropTable("dbo.Tests");
            DropTable("dbo.Students");
            DropTable("dbo.Registrations");
            DropTable("dbo.Questions");
            DropTable("dbo.QuestionDurations");
            DropTable("dbo.QuestionCategories");
            DropTable("dbo.Choices");
        }
    }
}
