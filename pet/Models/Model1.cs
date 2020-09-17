namespace pet.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1_m")
        {
        }

        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<Member> Member { get; set; }
        public virtual DbSet<Room> Room { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Evalution> Evalution { get; set; }
        public virtual DbSet<OrderCancel> OrderCancel { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<QuestionAnswer> QuestionAnswer { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
