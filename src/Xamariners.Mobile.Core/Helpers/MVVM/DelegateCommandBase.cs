using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Windows.Input;

namespace Xamariners.Mobile.Core.Helpers.MVVM
{
   /// <summary>
  /// An <see cref="T:System.Windows.Input.ICommand" /> whose delegates can be attached for <see cref="M:Prism.Commands.DelegateCommandBase.Execute(System.Object)" /> and <see cref="M:Prism.Commands.DelegateCommandBase.CanExecute(System.Object)" />.
  /// </summary>
  public abstract class DelegateCommandBase : ICommand
  {
    private readonly HashSet<string> _observedPropertiesExpressions = new HashSet<string>();
    private bool _isActive;
    private SynchronizationContext _synchronizationContext;

    /// <summary>
    /// Creates a new instance of a <see cref="T:Prism.Commands.DelegateCommandBase" />, specifying both the execute action and the can execute function.
    /// </summary>
    /// <param name="executeMethod">The <see cref="T:System.Action" /> to execute when <see cref="M:System.Windows.Input.ICommand.Execute(System.Object)" /> is invoked.</param>
    /// <param name="canExecuteMethod">The <see cref="T:System.Func`2" /> to invoked when <see cref="M:System.Windows.Input.ICommand.CanExecute(System.Object)" /> is invoked.</param>
    protected DelegateCommandBase()
    {
      this._synchronizationContext = SynchronizationContext.Current;
    }

    /// <summary>
    /// Occurs when changes occur that affect whether or not the command should execute.
    /// </summary>
    public virtual event EventHandler CanExecuteChanged;

    /// <summary>
    /// Raises <see cref="E:System.Windows.Input.ICommand.CanExecuteChanged" /> so every
    /// command invoker can requery <see cref="M:System.Windows.Input.ICommand.CanExecute(System.Object)" />.
    /// </summary>
    protected virtual void OnCanExecuteChanged()
    {
      EventHandler handler = this.CanExecuteChanged;
      if (handler == null)
        return;
      if (this._synchronizationContext != null && this._synchronizationContext != SynchronizationContext.Current)
        this._synchronizationContext.Post((SendOrPostCallback) (o => handler((object) this, EventArgs.Empty)), (object) null);
      else
        handler((object) this, EventArgs.Empty);
    }

    /// <summary>
    /// Raises <see cref="E:Prism.Commands.DelegateCommandBase.CanExecuteChanged" /> so every command invoker
    /// can requery to check if the command can execute.
    /// <remarks>Note that this will trigger the execution of <see cref="!:DelegateCommandBase.InvokeCanExecute" /> once for each invoker.</remarks>
    /// </summary>
    public void RaiseCanExecuteChanged()
    {
      this.OnCanExecuteChanged();
    }

    void ICommand.Execute(object parameter)
    {
      this.Execute(parameter);
    }

    bool ICommand.CanExecute(object parameter)
    {
      return this.CanExecute(parameter);
    }

    protected abstract void Execute(object parameter);

    protected abstract bool CanExecute(object parameter);

    /// <summary>
    /// Observes a property that implements INotifyPropertyChanged, and automatically calls DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
    /// </summary>
    /// <typeparam name="T">The object type containing the property specified in the expression.</typeparam>
    /// <param name="propertyExpression">The property expression. Example: ObservesProperty(() =&gt; PropertyName).</param>
    protected internal void ObservesPropertyInternal<T>(Expression<Func<T>> propertyExpression)
    {
      if (this._observedPropertiesExpressions.Contains(propertyExpression.ToString()))
        throw new ArgumentException(string.Format("{0} is already being observed.", (object) propertyExpression.ToString()), nameof (propertyExpression));
      this._observedPropertiesExpressions.Add(propertyExpression.ToString());
      PropertyObserver.Observes<T>(propertyExpression, new Action(this.RaiseCanExecuteChanged));
    }

    /// <summary>
    /// Gets or sets a value indicating whether the object is active.
    /// </summary>
    /// <value><see langword="true" /> if the object is active; otherwise <see langword="false" />.</value>
    public bool IsActive
    {
      get
      {
        return this._isActive;
      }
      set
      {
        if (this._isActive == value)
          return;
        this._isActive = value;
        this.OnIsActiveChanged();
      }
    }

    /// <summary>
    /// Fired if the <see cref="P:Prism.Commands.DelegateCommandBase.IsActive" /> property changes.
    /// </summary>
    public virtual event EventHandler IsActiveChanged;

    /// <summary>
    /// This raises the <see cref="E:Prism.Commands.DelegateCommandBase.IsActiveChanged" /> event.
    /// </summary>
    protected virtual void OnIsActiveChanged()
    {
      EventHandler isActiveChanged = this.IsActiveChanged;
      if (isActiveChanged == null)
        return;
      isActiveChanged((object) this, EventArgs.Empty);
    }
  }
}
