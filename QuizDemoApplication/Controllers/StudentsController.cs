using QuizDemoApplication.Models;
using QuizDemoClasses;
using System.Linq;
using System.Web.Mvc;

namespace QuizDemoApplication.Controllers
{
    public class StudentsController : Controller
    {
        private QuizDemoContext _db = new QuizDemoContext();
        // GET: Students
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult StudentRegistration()
        {

            ViewBag.Types = _db.Tests.Where(x => x.IsActive == true).Select(x => new { x.Id, x.Name }).ToList();
            SessionModel model = null;
            if (Session["SessionModel"] == null)
                model = new SessionModel();

            else
                model = (SessionModel)Session["SessionModel"];


            return View(model);
        }

        public ActionResult Instruction(SessionModel sessionModel)
        {
            if (sessionModel != null)
            {
                var test = _db.Tests.Where(c => c.IsActive == true && c.Id == sessionModel.TestId).FirstOrDefault();
                if (test != null)
                {
                    ViewBag.TestName = test.Name;
                    ViewBag.TestDescription = test.Description;
                    ViewBag.QuestionCount = test.TestQuestion.Count;
                    ViewBag.TestDuration = test.DurationInMint;

                }
            }
            return View(sessionModel);

        }
    }
}
