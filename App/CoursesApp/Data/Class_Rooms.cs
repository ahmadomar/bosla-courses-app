//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CoursesApp.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Class_Rooms
    {
        public int Class_Room_Id { get; set; }
        public string Title { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public bool Is_Live { get; set; }
        public int Course_Id { get; set; }
    
        public virtual Cours Cours { get; set; }
    }
}
