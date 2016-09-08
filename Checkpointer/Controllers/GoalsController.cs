using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Checkpointer.Models;

namespace Checkpointer.Controllers
{
    public class GoalsController : Controller
    {
        private GoalContext db = new GoalContext();

        // GET: Goals
        public ActionResult Index()
        {
            GoalChildrenList model = new GoalChildrenList { parent = 0, goalChildren = db.Goals.OrderBy(goal => goal.sort).ToList() };

            return View(model);
        }

        // GET: Goals/Create/0
        public ActionResult Create(int? id)
        {
            ViewBag.parentID = id;
            if (id > 0)
            {
                ViewBag.parentName = db.Goals.Find(id).name;
            }
            else
            {
                ViewBag.parentName = "None (top level goal)";
            }
            return View();
        }

        // POST: Goals/Create/
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,description,parent,sort")] Goal goal)
        {
            ViewBag.parentID = goal.parent;
            if (goal.parent > 0)
            {
                ViewBag.parentName = db.Goals.Find(goal.parent).name;
            }
            else
            {
                ViewBag.parentName = "None (top level goal)";
            }

            // Validation
            if (goal.name == null || goal.name.Trim().Length == 0)
                ModelState.AddModelError("name", "Name is required.");
            if (!unchecked(goal.parent == (int)goal.parent))
                ModelState.AddModelError("parent", "Parent must be an integer.");
            if (!unchecked(goal.sort == (int)goal.sort))
                ModelState.AddModelError("sort", "Sort must be an integer.");

            if (ModelState.IsValid)
            {
                goal.name = goal.name.Trim();

                db.Goals.Add(goal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(goal);
        }

        // GET: Goals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Goal goal = db.Goals.Find(id);
            if (goal == null)
            {
                return HttpNotFound();
            }

            return View(goal);
        }

        // POST: Goals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,description,parent,sort")] Goal goal)
        {
            // Validation
            if (goal.name == null || goal.name.Trim().Length == 0)
                ModelState.AddModelError("name", "Name is required.");
            if (!unchecked(goal.parent == (int)goal.parent))
                ModelState.AddModelError("parent", "Parent must be an integer.");
            if (!unchecked(goal.sort == (int)goal.sort))
                ModelState.AddModelError("sort", "Sort must be an integer.");

            if (ModelState.IsValid)
            {
                goal.name = goal.name.Trim();

                db.Entry(goal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(goal);
        }

        // GET: Goals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Goal goal = db.Goals.Find(id);
            if (goal == null)
            {
                return HttpNotFound();
            }
            return View(goal);
        }

        // POST: Goals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Goal goal = db.Goals.Find(id);
            
            goal.DeleteChildren();

            db.Goals.Remove(goal);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
