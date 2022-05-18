using System;
using System.Linq.Expressions;

namespace Xamariners.Mobile.Core.Helpers.MVVM
{
    /// <summary>
  /// An <see cref="!:ICommand" /> whose delegates do not take any parameters for <see cref="M:Prism.Commands.DelegateCommand.Execute" /> and <see cref="M:Prism.Commands.DelegateCommand.CanExecute" />.
  /// </summary>
  /// <see cref="T:Prism.Commands.DelegateCommandBase" />
  /// <see cref="T:Prism.Commands.DelegateCommand`1" />
  public class DelegateCommand : DelegateCommandBase
  {
    private Action _executeMethod;
    private Func<bool> _canExecuteMethod;

    /// <summary>
    /// Creates a new instance of <see cref="T:Prism.Commands.DelegateCommand" /> with the <see cref="T:System.Action" /> to invoke on execution.
    /// </summary>
    /// <param name="executeMethod">The <see cref="T:System.Action" /> to invoke when <see cref="!:ICommand.Execute" /> is called.</param>
    public DelegateCommand(Action executeMethod)
      : this(executeMethod, (Func<bool>) (() => true))
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="T:Prism.Commands.DelegateCommand" /> with the <see cref="T:System.Action" /> to invoke on execution
    /// and a <see langword="Func" /> to query for determining if the command can execute.
    /// </summary>
    /// <param name="executeMethod">The <see cref="T:System.Action" /> to invoke when <see cref="!:ICommand.Execute" /> is called.</param>
    /// <param name="canExecuteMethod">The <see cref="T:System.Func`1" /> to invoke when <see cref="!:ICommand.CanExecute" /> is called</param>
    public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
    {
      if (executeMethod == null || canExecuteMethod == null)
        throw new ArgumentNullException(nameof (executeMethod), new Exception("DelegateCommandDelegatesCannotBeNull"));
      this._executeMethod = executeMethod;
      this._canExecuteMethod = canExecuteMethod;
    }

    /// <summary>Executes the command.</summary>
    public void Execute()
    {
      this._executeMethod();
    }

    /// <summary>Determines if the command can be executed.</summary>
    /// <returns>Returns <see langword="true" /> if the command can execute,otherwise returns <see langword="false" />.</returns>
    public bool CanExecute()
    {
      return this._canExecuteMethod();
    }

    protected override void Execute(object parameter)
    {
      this.Execute();
    }

    protected override bool CanExecute(object parameter)
    {
      return this.CanExecute();
    }

    /// <summary>
    /// Observes a property that implements INotifyPropertyChanged, and automatically calls DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
    /// </summary>
    /// <typeparam name="T">The object type containing the property specified in the expression.</typeparam>
    /// <param name="propertyExpression">The property expression. Example: ObservesProperty(() =&gt; PropertyName).</param>
    /// <returns>The current instance of DelegateCommand</returns>
    public DelegateCommand ObservesProperty<T>(Expression<Func<T>> propertyExpression)
    {
      this.ObservesPropertyInternal<T>(propertyExpression);
      return this;
    }

    /// <summary>
    /// Observes a property that is used to determine if this command can execute, and if it implements INotifyPropertyChanged it will automatically call DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
    /// </summary>
    /// <param name="canExecuteExpression">The property expression. Example: ObservesCanExecute(() =&gt; PropertyName).</param>
    /// <returns>The current instance of DelegateCommand</returns>
    public DelegateCommand ObservesCanExecute(
      Expression<Func<bool>> canExecuteExpression)
    {
      this._canExecuteMethod = canExecuteExpression.Compile();
      this.ObservesPropertyInternal<bool>(canExecuteExpression);
      return this;
    }
  }
}
