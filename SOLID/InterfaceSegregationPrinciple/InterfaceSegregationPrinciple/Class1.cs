using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceSegregationPrinciple
{

    //ISP violation: One interface with too many responsiblities
    public interface IWorker
    {
        void work();
        void Eat();
    }
    //Correcting ISP by splitting into two interfaces
    public interface IWorkable
    {
        void work();
    }
    public interface IFeedable
    {
        void Eat();
    }

    //Implementing only what is need
    public class Worker:IWorkable,IFeedable
    {
        public void work() => Console.WriteLine("Working...");
        public void Eat() => Console.WriteLine("Eating...");
    }
    public class Robot : IWorkable
    {
        public void work() => Console.WriteLine("Robot Working...");
        //Robot doesn't need to implement Eat method
    }
}
