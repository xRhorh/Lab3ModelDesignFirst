using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelDesignFirst_L1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test Model Designer First");
            TestOneToMany();

            Console.ReadKey();
        }

        public partial class ModelOneToManyContainer : DbContext
        {
            public ModelOneToManyContainer()
                : base("name=ModelOneToManyContainer")
            { }

            protected override void OnModelCreating(
                DbModelBuilder modelBuilder)
            {
                throw new UnintentionalCodeFirstException();
            }

            public virtual DbSet<Customer> Customers { get; set; }
            public virtual DbSet<Orders> Orders { get; set; } 
        }

        static void TestPerson()
        {
            using (Model1Container context = new Model1Container())
            {
                Person p = new Person()
                {
                    FirstName = "Julie",
                    LastName = "Andrew",
                    MidleName = "T",
                    TelephonNumber = "1234567890"
                };
                context.People.Add(p);
                context.SaveChanges();
                var items = context.People;
                foreach (var x in items)
                    Console.WriteLine("{0} {1}", x.Id, x.FirstName);
            }
        }

        static void TestOneToMany()
        {
            Console.WriteLine("One to many association");
            using (ModelOneToManyContainer context = 
                new ModelOneToManyContainer())
            {
                Customer c = new Customer()
                {
                    Name = "Customer 1",
                    City = "Iasi"
                };

                Orders o1 = new Orders()
                {
                    TotalValue = 200,
                    Date = DateTime.Now,
                    Customer = c
                };

                Orders o2 = new Orders()
                {
                    TotalValue = 300,
                    Date = DateTime.Now,
                    Customer = c
                };

                context.Customers.Add(c);
                context.Orders.Add(o1);
                context.Orders.Add(o2);
                context.SaveChanges();

                var items = context.Customers;
                foreach(var x in items)
                {
                    Console.WriteLine("Customer : {0}, {1}, {2}", x.CustomerId, x.Name, x.City);
                    foreach (var ox in x.Orders)
                        Console.WriteLine("\tOrders: {0}, {1}, {2}", ox.OrderId, ox.Date, ox.TotalValue);
                }
            }
        }
    }
}
