using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Xamariners.Mobile.Core.Helpers.MVVM;

namespace Xamariners.Mobile.Core.Infrastructure
{
    public class XDelegateTimerCommand<TParam> : DelegateCommand<TParam>
    {
        private readonly Func<TParam, bool> _validateMethod;

        private Timer _timer;
        private int _timerRetry = 0;
        private bool _stillProcessing = false;
        private bool _stillWaiting = false;
        private bool _isBusy;
        private Action _onBusy;

        public ManualResetEvent WaitHandle { get; }
        public bool IsValid { get; private set; }

        private Action<object> _showSpinner;
        private TimeSpan _elapsed;

        private const int INTERVAL = 250;
        private const int SPINNER_DUETIME = INTERVAL * 2;

        public XDelegateTimerCommand(Action<TParam> executeMethod, Func<TParam, bool> validateMethod, Action OnBusy = default(Action), Action<object> showSpinner = default(Action<object>)) : base(executeMethod)
        {
            WaitHandle = new ManualResetEvent(false);

            _validateMethod = validateMethod;
            _onBusy = OnBusy;
            _showSpinner = showSpinner;
        }

        public XDelegateTimerCommand(Action<TParam> executeMethod, Func<TParam, bool> canExecuteMethod, Func<TParam, bool> validateMethod, Action OnBusy = default(Action), Action<object> showSpinner = default(Action<object>)) : base(executeMethod, canExecuteMethod)
        {
            WaitHandle = new ManualResetEvent(false);
            _validateMethod = validateMethod;
            _onBusy = OnBusy;
            _showSpinner = showSpinner;
        }

        public new void Execute(object parameter)
        {
            if (Validate((TParam)parameter))
            {
                ResetTimer();
                _elapsed = new TimeSpan();
                _timer = new Timer(OnTick, parameter, 0, INTERVAL);
            }
        }

        public void Execute()
        {
            if (Validate(default(TParam)))
            {
                ResetTimer();
                _elapsed = new TimeSpan();
                _timer = new Timer(OnTick, null, 0, INTERVAL);
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

        public void ResetTimer()
        {
            _timer?.Dispose();
            _timerRetry = 0;
            _stillProcessing = false;
            _stillWaiting = false;
            _isBusy = false;
           
            // unblock thread
            WaitHandle.Set();
        }

        private void OnTick(object parameter)
        {
            _elapsed = _elapsed.Add(TimeSpan.FromMilliseconds(INTERVAL));

            if (_elapsed.Equals(TimeSpan.FromMilliseconds(SPINNER_DUETIME)))
            {
                Debug.WriteLine("SPINNER_DUETIME");
                _showSpinner?.Invoke(parameter);
            }

            if (_timerRetry > 0 && !_stillWaiting)
            {
                _stillWaiting = true;

                if (!_isBusy)
                    _onBusy?.Invoke();

                return;
            }

            _timerRetry++;

            if (_stillProcessing) return;

            _stillProcessing = true;
            
            // block thread with waitone
            WaitHandle.Reset();

            try
            {
                base.Execute(parameter);
            }
            catch
            {
                ResetTimer();
            }
        }
    }

    public class XDelegateTimerCommand : DelegateCommand
    {
        private readonly Func<bool> _validateMethod;
        private Timer _timer;
        private int _timerRetry = 0;
        private bool _stillProcessing = false;
        private bool _stillWaiting = false;
        private bool _isBusy;
        private Action _onBusy;

        private Action<object> _showSpinner;
        private TimeSpan _elapsed;

        public ManualResetEvent WaitHandle { get; }
        public bool IsValid { get; private set; }

        private const int INTERVAL = 250;
        private const int SPINNER_DUETIME = INTERVAL * 2;

        public XDelegateTimerCommand(Action executeMethod, Func<bool> validateMethod, Action onBusy = default(Action), Action<object> showSpinner = default(Action<object>)) : base(executeMethod)
        {
            WaitHandle = new ManualResetEvent(false);
            _validateMethod = validateMethod;
            _onBusy = onBusy;
            _showSpinner = showSpinner;
        }

        public XDelegateTimerCommand(Action executeMethod, Func<bool> canExecuteMethod, Func<bool> validateMethod, Action onBusy = default(Action), Action<object> showSpinner = default(Action<object>)) : base(executeMethod, canExecuteMethod)
        {
            WaitHandle = new ManualResetEvent(false);
            _validateMethod = validateMethod;
            _onBusy = onBusy;
            _showSpinner = showSpinner;
        }

        protected override void Execute(object parameter)
        {
            if (Validate(parameter))
            {
                ResetTimer();
                _elapsed = new TimeSpan();
                _timer = new Timer(OnTick, parameter, 0, INTERVAL);
            }
        }

        public new void Execute()
        {
            if (Validate(null))
            {
                ResetTimer();
                _elapsed = new TimeSpan();
                _timer = new Timer(OnTick, null, 0, INTERVAL);
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

        private void OnTick(object parameter)
        {
            _elapsed = _elapsed.Add(TimeSpan.FromMilliseconds(INTERVAL));

            if (_elapsed.Equals(TimeSpan.FromMilliseconds(SPINNER_DUETIME)))
            {
                _showSpinner?.Invoke(parameter);
            }

            if (_timerRetry > 0 && !_stillWaiting)
            {
                _stillWaiting = true;

                if (!_isBusy)
                    _onBusy?.Invoke();

                return;
            }

            _timerRetry++;

            if (_stillProcessing)
                return;

            _stillProcessing = true;
            
            // block thread with waitone
            WaitHandle.Reset();

            try
            {
                base.Execute(parameter);
            }
            catch
            {
                ResetTimer();
            }
        }

        public void ResetTimer()
        {
            _timer?.Dispose();
            _timerRetry = 0;
            _stillProcessing = false;
            _stillWaiting = false;
            _isBusy = false;
            
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