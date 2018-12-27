using TodoServices.Domain;
using TodoServices.Domain.Model;
using System;
using System.Linq;

namespace TodoServices.Command
{
    public static class TodoContextInitializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context">������� ��������</param>
        public static void Seed(SqlDbContext context)
        {
            if (!context.Todos.Any())
            {
                context.Todos.AddRange(Todos);
                context.SaveChanges();            
            }
        }
        public static void RemoveData(SqlDbContext context)
        {

            context.Todos.Remove(context.Todos.First());
            context.SaveChanges();
            
        }


        private static Todo[] Todos
        {
            get
            {
                var now = DateTime.Now;
                return new Todo[]
                 {
                      new Todo{ AuthorId = 1, CreateDate = now, EstimatedCost = 10, Title = "task 1", Description = "description for task 1",

                        Childs = new Todo[]
                        { new Todo { AuthorId = 1, CreateDate = now, EstimatedCost = 20, Title = "Item 1", Description = "description for Item 1",ParentId = 1 },
                          new Todo { AuthorId = 1, CreateDate = now, EstimatedCost = 20, Title = "Item 2", Description = "description for Item 2",ParentId = 1},
                          new Todo { AuthorId = 1, CreateDate = now, EstimatedCost = 20, Title = "Item 3", Description = "description for Item 3",ParentId = 1}
                        }
                      },
                      new Todo{ AuthorId = 1, CreateDate = now, EstimatedCost = 20, Title = "task 2", Description = "description for task 2",

                        Childs = new Todo[]
                        { new Todo { AuthorId = 1 , CreateDate = now, EstimatedCost = 20, Title = "Item 1", Description = "description for Item 1",ParentId = 1},
                          new Todo { AuthorId = 1 , CreateDate = now, EstimatedCost = 20, Title = "Item 2", Description = "description for Item 2",ParentId = 1},
                          new Todo { AuthorId = 1 , CreateDate = now, EstimatedCost = 20, Title = "Item 3", Description = "description for Item 3",ParentId = 1}
                        }
                      },
                      new Todo{ AuthorId = 1, CreateDate = now, EstimatedCost = 30, Title = "task 3", Description = "description for task 3",
                        Childs = new Todo[]
                        { new Todo { AuthorId = 1, CreateDate = now, EstimatedCost = 20, Title = "Item 1", Description = "description for Item 1",ParentId = 1},
                          new Todo { AuthorId = 1 , CreateDate = now, EstimatedCost = 20, Title = "Item 2", Description = "description for Item 2",ParentId = 1},
                          new Todo { AuthorId = 1 ,CreateDate = now, EstimatedCost = 20, Title = "Item 3", Description = "description for Item 3",ParentId = 1}
                        }
                      }
                 };
            }
        }
    }


   

}