using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Runtime.CompilerServices
{
    public class AsyncTaskMethodBuilder
    {
        static Stack<Platform.StackFrame> globalFrames = new Stack<Platform.StackFrame>();
        static Stack<object[]> globalValues = null;
        Platform.StackFrame _frame;
        bool hasFinished;

        public static void Init()
        {
            globalFrames = new Stack<Platform.StackFrame>();

            globalValues = new Stack<object[]>();
            globalValues.Push(new object[] { 0, 1000 });
            globalValues.Push(new object[] { 0, 5, "def", 5 });
        }

        public static async Task RunProgram(Task program)
        {
            await program;
        }

        void GetStackFrames()
        {
            Stack<object[]> stackValues = new Stack<object[]>();

            foreach(var frame in globalFrames)
            {
                var values = frame.GetFrameValues();
                stackValues.Push(values);
            }
        }

        public static AsyncTaskMethodBuilder Create()
        {
            return new AsyncTaskMethodBuilder();
        }

        public void Start<TStateMachine>(ref TStateMachine stateMachine) 
            where TStateMachine : IAsyncStateMachine
        {
            _frame = new Platform.StackFrame(stateMachine);
            //_frame.Define(stateMachine);

            if(globalValues != null)
            {
                var obj = globalValues.Pop();
                _frame.Define(stateMachine, obj);

                if(globalValues.Count == 0)
                {
                    globalValues = null;
                }
            }

            globalFrames.Push(_frame);

            hasFinished = false;

            _frame.Redefine(stateMachine);
            Console.WriteLine($"{_frame.Name}-start");

            int i, total = 3;

            //while (!hasFinished)
            for(i=0; i< total; i++)
            {
                if (hasFinished)
                    break;

                GetStackFrames();
                //var json = Newtonsoft.Json.JsonConvert.SerializeObject(globalFrames);
                stateMachine.MoveNext();
                GetStackFrames();
                _frame.Redefine(stateMachine);
                Console.WriteLine($"   {_frame.Name}: Step completed");
            }

            if(i == total)
            {
                GetStackFrames();
                hasFinished = false;
            }
            Console.WriteLine($"{_frame.Name}-end");

        }

        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
            throw new InvalidOperationException("Not implemented");
        }

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            throw new InvalidOperationException("Not implemented");
        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            Console.WriteLine($"   {_frame.Name}: Step completed");
        }

        public void SetException(Exception e)
        {
            hasFinished = true;
            throw new InvalidOperationException();
        }

        public void SetResult()
        {
            hasFinished = true;
            globalFrames.Pop();
        }

        //public Task Task { get { return System.Threading.Tasks.Task.FromResult(true); } }
        public Task Task { get { return Task.Delay(300); } }
    }
}
