using System;

namespace Ploeh.AutoFixtureUnitTest
{
    public class CommandMock<T>
    {
        public CommandMock()
        {
            this.OnCommand = x => { };
        }

        public Action<T> OnCommand { get; set; }

        public void Command(T x)
        {
            this.OnCommand(x);
        }
    }

    public class CommandMock<T1, T2>
    {
        public CommandMock()
        {
            this.OnCommand = (x1, x2) => { };
        }

        public Action<T1, T2> OnCommand { get; set; }

        public void Command(T1 x1, T2 x2)
        {
            this.OnCommand(x1, x2);
        }
    }

    public class CommandMock<T1, T2, T3>
    {
        public CommandMock()
        {
            this.OnCommand = (x1, x2, x3) => { };
        }

        public Action<T1, T2, T3> OnCommand { get; set; }

        public void Command(T1 x1, T2 x2, T3 x3)
        {
            this.OnCommand(x1, x2, x3);
        }
    }

    public class CommandMock<T1, T2, T3, T4>
    {
        public CommandMock()
        {
            this.OnCommand = (x1, x2, x3, x4) => { };
        }

        public Action<T1, T2, T3, T4> OnCommand { get; set; }

        public void Command(T1 x1, T2 x2, T3 x3, T4 x4)
        {
            this.OnCommand(x1, x2, x3, x4);
        }
    }
}
