using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Xamariners.Mobile.Core.Helpers.MVVM;

namespace Xamariners.Mobile.Core.Infrastructure
{
    public class XDelegateCommand<TParam> : DelegateCommand<TParam>
    {
        private readonly Func<TParam, bool> _validateMethod;

        public ManualResetEvent WaitHandle { get; }

        public bool IsValid { get; private set; }

        public XDelegateCommand(Action<TParam> executeMethod, Func<TParam, bool> validateMethod) : base(executeMethod)
        {
            WaitHandle = new ManualResetEvent(false);

            _validateMethod = validateMethod;
        }

        public XDelegateCommand(Action<TParam> executeMethod, Func<TParam, bool> canExecuteMethod, Func<TParam, bool> validateMethod) : base(executeMethod, canExecuteMethod)
        {
            WaitHandle = new ManualResetEvent(false);
            _validateMethod = validateMethod;
        }

        protected override void Execute(object parameter)
        {
            if (Validate((TParam) parameter))
            {  
                try
                {
                    // block thread with waitone
                    WaitHandle.Reset();

                    base.Execute(parameter);
                }
                catch
                {
                    Reset();
                }
            }
        }

        protected void Execute()
        {
            if (Validate(default(TParam)))
            {
                try
                {
                    // block thread with waitone
                    WaitHandle.Reset();

                    base.Execute(null);
                }
                catch
                {
                    Reset();
                }
            }
        }

        private bool Validate(TParam parameter)
        {
            IsValid = true;
            if (_validateMethod != null)
            {
                IsValid = _validateMethod.Invoke(parameter);
                return IsValid;
            }

            return false;
        }

        public void Reset()
        {
            // unblock thread
            WaitHandle.Set();
        }
    }

    public class XDelegateCommand : DelegateCommand
    {
        private readonly Func<bool> _validateMethod;
        public ManualResetEvent WaitHandle { get; }
        public bool IsValid { get; private set; }

        public XDelegateCommand(Action executeMethod, Func<bool> validateMethod) : base(executeMethod)
        {
            WaitHandle = new ManualResetEvent(false);
            _validateMethod = validateMethod;
        }

        public XDelegateCommand(Action executeMethod, Func<bool> canExecuteMethod, Func<bool> validateMethod) : base(executeMethod, canExecuteMethod)
        {
            WaitHandle = new ManualResetEvent(false);
            _validateMethod = validateMethod;
        }

        protected override void Execute(object parameter)
        {
            if (Validate(parameter))
            {
                try
                {
                    // block thread with waitone
                    WaitHandle.Reset();

                    base.Execute(parameter);
                }
                catch
                {
                    Reset();
                }
            }
        }

        public new void Execute()
        {
            if (Validate(null))
            {
                try
                {
                    // block thread with waitone
                    WaitHandle.Reset();

                    base.Execute();
                }
                catch
                {
                    Reset();
                }
            }
        }

        private bool Validate(object parameter)
        {
            IsValid = true;
            if (_validateMethod != null)
            {
                IsValid = _validateMethod.Invoke();
                return IsValid;
            }

            return false;
        }

        public void Reset()
        {
            // unblock thread
            WaitHandle.Set();
        }

        private static bool IsAsyncMethod(Type classType, string methodName)
        {
            // Obtain the method with the specified name.
            MethodInfo method = classType.GetMethod(methodName);

            Type attType = typeof(AsyncStateMachineAttribute);

            // Obtain the custom attribute for the method. 
            // The value returned contains the StateMachineType property. 
            // Null is returned if the attribute isn't present for the method. 
            var attrib = (AsyncStateMachineAttribute)method.GetCustomAttribute(attType);

            return (attrib != null);
        }
    }
}