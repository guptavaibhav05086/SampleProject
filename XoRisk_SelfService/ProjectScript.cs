//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace XoRisk_SelfService
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProjectScript
    {
        public int ProjectId { get; set; }
        public int ScriptId { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public string ProjectCode { get; set; }
    
        public virtual Project Project { get; set; }
        public virtual Script Script { get; set; }
    }
}
