using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Runtime.CompilerServices
{
    public struct AsyncVoidMethodBuilder
    {
        public static AsyncVoidMethodBuilder Create()
        {
            return new AsyncVoidMethodBuilder();
        }

        public void SetException(Exception e) { }
        public void SetResult() { }

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {

        }
    }

#if ASYNC_DEFINED
    public struct AsyncTaskMethodBuilder
    {
        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            stateMachine.MoveNext();

            while (false)
            {
                var props = stateMachine.GetType().GetFields((Reflection.BindingFlags)0xffff);
                var field = props[4];
                var o = field.GetValue(stateMachine);

                stateMachine.MoveNext();
            }
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {

        }

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            var props = stateMachine.GetType().GetFields((Reflection.BindingFlags)0xffff);
            var field = props[4];
            var o = field.GetValue(stateMachine);

            stateMachine.MoveNext();

        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            var props = stateMachine.GetType().GetFields((Reflection.BindingFlags)0xffff);
            var field = props[4];
            var o = field.GetValue(stateMachine);

            

            stateMachine.MoveNext();
        }
        public static AsyncTaskMethodBuilder Create()
        {
            return new AsyncTaskMethodBuilder();
        }

        public void SetException(Exception e) { }
        public void SetResult() { }
        public Task Task { get { return System.Threading.Tasks.Task.FromResult(true); } }
    }

    public struct AsyncTaskMethodBuilder<T>
    {
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
    where TAwaiter : INotifyCompletion
    where TStateMachine : IAsyncStateMachine
        {

        }
        public static AsyncTaskMethodBuilder<T> Create()
        {
            return new AsyncTaskMethodBuilder<T>();
        }

        public void SetException(Exception e) { }
        public void SetResult(T result) { }
        public Task<T> Task { get { return null; } }
    }
#endif
}
