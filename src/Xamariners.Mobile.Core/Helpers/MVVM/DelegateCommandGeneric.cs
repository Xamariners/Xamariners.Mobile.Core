using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Xamariners.Mobile.Core.Helpers.MVVM
{
public class DelegateCommand<T> : DelegateCommandBase
  {
    private readonly Action<T> _executeMethod;
    private Func<T, bool> _canExecuteMethod;

    /// <summary>
    /// Initializes a new instance of <see cref="T:Prism.Commands.DelegateCommand`1" />.
    /// </summary>
    /// <param name="executeMethod">Delegate to execute when Execute is called on the command. This can be null to just hook up a CanExecute delegate.</param>
    /// <remarks><see cref="M:Prism.Commands.DelegateCommand`1.CanExecute(`0)" /> will always return true.</remarks>
    public DelegateCommand(Action<T> executeMethod)
      : this(executeMethod, (Func<T, bool>) (o => true))
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="T:Prism.Commands.DelegateCommand`1" />.
    /// </summary>
    /// <param name="executeMethod">Delegate to execute when Execute is called on the command. This can be null to just hook up a CanExecute delegate.</param>
    /// <param name="canExecuteMethod">Delegate to execute when CanExecute is called on the command. This can be null.</param>
    /// <exception cref="T:System.ArgumentNullException">When both <paramref name="executeMethod" /> and <paramref name="canExecuteMethod" /> ar <see langword="null" />.</exception>
    public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
    {
      if (executeMethod == null || canExecuteMethod == null)
        throw new ArgumentNullException(nameof (executeMethod), "DelegateCommandDelegatesCannotBeNull");
      TypeInfo typeInfo = typeof (T).GetTypeInfo();
      if (typeInfo.IsValueType && (!typeInfo.IsGenericType || !typeof (Nullable<>).GetTypeInfo().IsAssignableFrom(typeInfo.GetGenericTypeDefinition().GetTypeInfo())))
        throw new InvalidCastException("DelegateCommandInvalidGenericPayloadType");
      this._executeMethod = executeMethod;
      this._canExecuteMethod = canExecuteMethod;
    }

    /// <summary>
    /// Executes the command and invokes the <see cref="T:System.Action`1" /> provided during construction.
    /// </summary>
    /// <param name="parameter">Data used by the command.</param>
    public void Execute(T parameter)
    {
      this._executeMethod(parameter);
    }

    /// <summary>
    /// Determines if the command can execute by invoked the <see cref="T:System.Func`2" /> provided during construction.
    /// </summary>
    /// <param name="parameter">Data used by the command to determine if it can execute.</param>
    /// <returns>
    /// <see langword="true" /> if this command can be executed; otherwise, <see langword="false" />.
    /// </returns>
    public bool CanExecute(T parameter)
    {
      return this._canExecuteMethod(parameter);
    }

    protected override void Execute(object parameter)
    {
      this.Execute((T) parameter);
    }

    protected override bool CanExecute(object parameter)
    {
      return this.CanExecute((T) parameter);
    }

    /// <summary>
    /// Observes a property that implements INotifyPropertyChanged, and automatically calls DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
    /// </summary>
    /// <typeparam name="TType">The type of the return value of the method that this delegate encapulates</typeparam>
    /// <param name="propertyExpression">The property expression. Example: ObservesProperty(() =&gt; PropertyName).</param>
    /// <returns>The current instance of DelegateCommand</returns>
    public DelegateCommand<T> ObservesProperty<TType>(
      Expression<Func<TType>> propertyExpression)
    {
      this.ObservesPropertyInternal<TType>(propertyExpression);
      return this;
    }

    /// <summary>
    /// Observes a property that is used to determine if this command can execute, and if it implements INotifyPropertyChanged it will automatically call DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
    /// </summary>
    /// <param name="canExecuteExpression">The property expression. Example: ObservesCanExecute(() =&gt; PropertyName).</param>
    /// <returns>The current instance of DelegateCommand</returns>
    public DelegateCommand<T> ObservesCanExecute(
      Expression<Func<bool>> canExecuteExpression)
    {
      this._canExecuteMethod = Expression.Lambda<Func<T, bool>>(canExecuteExpression.Body, Expression.Parameter(typeof (T), "o")).Compile();
      this.ObservesPropertyInternal<bool>(canExecuteExpression);
      return this;
    }
  }
}
