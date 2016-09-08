using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Checkpointer.Models;

namespace Checkpointer.Models
{
    public class Goal
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int parent { get; set; }
        public int sort { get; set; }

        public IList<Goal> GetChildren()
        {
            GoalContext db = new GoalContext();

            IList<Goal> children = db.Goals.Where(item => item.parent == id).ToList();

            return children;
        }

        public void DeleteChildren()
        {
            IList<Goal> children = GetChildren();
            if (children.Any())
            {
                GoalContext db = new GoalContext();

                foreach (var child in children)
                {
                    // Run DeleteChildren recursively on children until no more children have children.
                    child.DeleteChildren();

                    // Then remove the child.
                    Goal childToRemove = db.Goals.Find(child.id);
                    db.Goals.Remove(childToRemove);
                    db.SaveChanges();
                }
            }
        }
    }

    public class GoalChildrenList
    {
        public int? parent { get; set; }
        public IList<Goal> goalChildren { get; set; }
    }

    public class GoalContext : DbContext
    {
        public DbSet<Goal> Goals { get; set; }
    }
}