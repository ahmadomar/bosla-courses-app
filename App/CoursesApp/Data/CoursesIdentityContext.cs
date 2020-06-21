using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace CoursesApp.Data
{
    public class CoursesIdentityContext : IdentityDbContext<MyIdentityUser>
    {
        public CoursesIdentityContext():base("Courses_Connection")
        {

        }
    }

    public class MyIdentityUser : IdentityUser
    {

    }
}