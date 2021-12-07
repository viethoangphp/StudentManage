using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Models.EntityModel
{
    public partial class DBContext : DbContext
    {
        public DBContext()
            : base("name=DBContext")
        {
        }

        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Config> Configs { get; set; }
        public virtual DbSet<DetailEvalution> DetailEvalutions { get; set; }
        public virtual DbSet<EvaluativeCriteria> EvaluativeCriterias { get; set; }
        public virtual DbSet<EvaluativeMain> EvaluativeMains { get; set; }
        public virtual DbSet<EvalutionForm> EvalutionForms { get; set; }
        public virtual DbSet<Faculty> Faculties { get; set; }
        public virtual DbSet<GroupUser> GroupUsers { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Semester> Semesters { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TemplateForm> TemplateForms { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<TimeEvalution> TimeEvalutions { get; set; }
        public virtual DbSet<UnionBook> UnionBooks { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Config>()
                .Property(e => e.KeyName)
                .IsUnicode(false);

            modelBuilder.Entity<DetailEvalution>()
                .Property(e => e.Image_proof)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<EvaluativeCriteria>()
                .HasMany(e => e.DetailEvalutions)
                .WithRequired(e => e.EvaluativeCriteria)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EvaluativeMain>()
                .HasMany(e => e.EvaluativeCriterias)
                .WithRequired(e => e.EvaluativeMain)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EvalutionForm>()
                .HasMany(e => e.DetailEvalutions)
                .WithRequired(e => e.EvalutionForm)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Faculty>()
                .Property(e => e.Phone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Faculty>()
                .HasMany(e => e.Classes)
                .WithRequired(e => e.Faculty)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GroupUser>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.GroupUser)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Position>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.Position)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Semester>()
                .HasMany(e => e.EvalutionForms)
                .WithRequired(e => e.Semester)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TemplateForm>()
                .HasMany(e => e.EvaluativeMains)
                .WithRequired(e => e.TemplateForm)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TemplateForm>()
                .HasMany(e => e.GroupUsers)
                .WithRequired(e => e.TemplateForm)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TimeEvalution>()
                .HasMany(e => e.GroupUsers)
                .WithRequired(e => e.TimeEvalution)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.StudentCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Phone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.Type)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Avatar)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.DetailEvalutions)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Tickets)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.Response_by);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Tickets1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.Create_by);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UnionBooks)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.Create_By);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UnionBooks1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.UserID);
        }
    }
}
